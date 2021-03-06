﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Http
{
    internal class XmlFeatureReader : XmlReaderBase, IFeatureReader
    {
        internal XmlFeatureReader(Stream stream)
            : base(stream)
        {
        }

        public XmlFeatureReader(HttpWebResponse resp)
            : base(resp)
        { }

        public IFeatureReader GetFeatureObject(string name) => ((IFeature)this.Current).GetFeatureObject(name);

        public IFeatureReader GetFeatureObject(int index) => ((IFeature)this.Current).GetFeatureObject(index);

        public override ReaderType ReaderType => ReaderType.Feature;

        protected override void InitProperties()
        {
            //SelectAggregate responses start with PropertySet
            _reader.Read();
            if (_reader.Name != "xml") //NOXLATE
                throw new Exception("Bad document. Expected xml prolog"); //LOCALIZEME
            _reader.Read();
            if (_reader.Name != this.ResponseRootElement)
                throw new Exception("Bad document. Expected element: " + this.ResponseRootElement); //LOCALIZEME
            _reader.Read();
            //FIXME: Gracefully handle empty result sets. Empty result sets do not include a schema element
            if (_reader.Name != this.DefinitionRootElement)
                throw new Exception("Bad document. Expected element: " + this.DefinitionRootElement); //LOCALIZEME

            var schemaXml = _reader.ReadOuterXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(schemaXml);
            FeatureSchema schema = new FeatureSchema("", "");
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            mgr.AddNamespace("gml", "http://www.opengis.net/gml");
            mgr.AddNamespace("fdo", "http://fdo.osgeo.org/schemas");

            XmlNode root = doc.SelectSingleNode("xs:schema", mgr);
            schema.ReadXml(root, mgr);

            //This is a query of a single feature class after all!
            Debug.Assert(schema.Classes.Count == 1);

            this.ClassDefinition = schema.Classes[0];
            List<XmlProperty> properties = new List<XmlProperty>();
            foreach (var prop in this.ClassDefinition.Properties)
            {
                string name = prop.Name;
                switch (prop.Type)
                {
                    case PropertyDefinitionType.Association:
                        break;

                    case PropertyDefinitionType.Data:
                        {
                            DataPropertyDefinition dp = (DataPropertyDefinition)prop;
                            switch (dp.DataType)
                            {
                                case DataPropertyType.Blob:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Blob));
                                    break;

                                case DataPropertyType.Boolean:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Boolean));
                                    break;

                                case DataPropertyType.Byte:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Byte));
                                    break;

                                case DataPropertyType.Clob:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Clob));
                                    break;

                                case DataPropertyType.DateTime:
                                    properties.Add(new XmlProperty(name, PropertyValueType.DateTime));
                                    break;

                                case DataPropertyType.Double:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Double));
                                    break;

                                case DataPropertyType.Int16:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Int16));
                                    break;

                                case DataPropertyType.Int32:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Int32));
                                    break;

                                case DataPropertyType.Int64:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Int64));
                                    break;

                                case DataPropertyType.Single:
                                    properties.Add(new XmlProperty(name, PropertyValueType.Single));
                                    break;

                                case DataPropertyType.String:
                                    properties.Add(new XmlProperty(name, PropertyValueType.String));
                                    break;
                            }
                        }
                        break;

                    case PropertyDefinitionType.Geometry:
                        {
                            properties.Add(new XmlProperty(name, PropertyValueType.Geometry));
                        }
                        break;

                    case PropertyDefinitionType.Object:
                        {
                            properties.Add(new XmlProperty(name, PropertyValueType.Feature));
                        }
                        break;

                    case PropertyDefinitionType.Raster:
                        {
                            properties.Add(new XmlProperty(name, PropertyValueType.Raster));
                        }
                        break;
                }
            }
            _properties = properties.ToArray();
            foreach (var prop in _properties)
            {
                _propertyMap[prop.Name] = prop;
            }
            this.FieldCount = _properties.Length;

            if (_reader.Name != this.ValuesRootElement)
                throw new Exception("Bad document");

            _reader.Read();

            if (_reader.NodeType != XmlNodeType.EndElement)
            {
                if (_reader.Name == this.ValuesRootElement)
                    _reader = null; //No features :(
            }
        }

        protected override string ResponseRootElement => "FeatureSet"; //NOXLATE

        protected override string DefinitionRootElement => "xs:schema"; //NOXLATE

        protected override string DefinitionChildElement
        {
            get { throw new NotImplementedException(); }
        }

        protected override string DefinitionChildNameElement
        {
            get { throw new NotImplementedException(); }
        }

        protected override string DefinitionChildTypeElement
        {
            get { throw new NotImplementedException(); }
        }

        protected override string ValuesRootElement => "Features"; //NOXLATE

        protected override string ValuesRowElement => "Feature"; //NOXLATE

        protected override string ValuesRowPropertyElement => "Property"; //NOXLATE

        protected override string ValuesRowPropertyNameElement => "Name"; //NOXLATE

        protected override string ValuesRowPropertyValueElement => "Value"; //NOXLATE

        public ClassDefinition ClassDefinition
        {
            get;
            private set;
        }

        protected override IRecord ReadNextRecord()
        {
            if (_reader == null || (_reader.Name != this.ValuesRowElement))
            {
                return null;
            }
            else
            {
                if (_recDoc == null)
                    _recDoc = new XmlDocument();

                //TODO: We can probably make this more efficient.
                _recDoc.LoadXml(_reader.ReadOuterXml());
                var propertyNodeList = _recDoc[this.ValuesRowElement].SelectNodes(this.ValuesRowPropertyElement);
                var rec = new XmlRecord(_properties, _wktReader, propertyNodeList, this.ValuesRowPropertyNameElement, this.ValuesRowPropertyValueElement);
                var feat = new FeatureBase(this.ClassDefinition);
                feat.Update(rec);
                return feat;
            }
        }

        private class Enumerator : IEnumerator<IFeature>
        {
            private XmlFeatureReader _reader;

            public Enumerator(XmlFeatureReader reader)
            {
                _reader = reader;
            }

            public IFeature Current => (IFeature)_reader.Current;

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current => this.Current;

            public bool MoveNext() => _reader.ReadNext();

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        public IEnumerator<IFeature> GetEnumerator() => new Enumerator(this);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => new Enumerator(this);
    }
}