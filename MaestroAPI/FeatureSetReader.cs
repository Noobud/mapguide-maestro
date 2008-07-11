#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
#endregion
using System;
using System.IO;
using System.Xml;
using Topology.Geometries;

namespace OSGeo.MapGuide.MaestroAPI
{
	/// <summary>
	/// Represents a set of results from a query
	/// </summary>
	public class FeatureSetReader
	{
		private FeatureSetColumn[] m_columns;
		private FeatureSetRow m_row;
		private XmlTextReader m_reader;

		private OSGeo.MapGuide.MgFeatureReader m_rd;

		public FeatureSetReader(OSGeo.MapGuide.MgFeatureReader rd)
		{
			m_rd = rd;
			m_columns = new FeatureSetColumn[rd.GetPropertyCount()];
			for(int i = 0; i < m_columns.Length; i++)
				m_columns[i] = new FeatureSetColumn(rd.GetPropertyName(i), rd.GetPropertyType(rd.GetPropertyName(i)));

		}

		//TODO: Make internal
		public FeatureSetReader(Stream m_source)
		{
			m_reader = new XmlTextReader(m_source);
			m_reader.WhitespaceHandling = WhitespaceHandling.Significant;

			//First we extract the response layout
			m_reader.Read();
			if (m_reader.Name != "xml")
				throw new Exception("Bad document");
			m_reader.Read();
			if (m_reader.Name != "FeatureSet")
				throw new Exception("Bad document");

			m_reader.Read();
			if (m_reader.Name != "xs:schema")
				throw new Exception("Bad document");

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(m_reader.ReadOuterXml());
			XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
			mgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
			mgr.AddNamespace("gml", "http://www.opengis.net/gml");
			mgr.AddNamespace("fdo", "http://fdo.osgeo.org/schemas");

			//TODO: Assumes there is only one type returned... perhaps more can be returned....
			XmlNodeList lst = doc.SelectNodes("xs:schema/xs:complexType/xs:complexContent/xs:extension/xs:sequence/xs:element", mgr);
			if (lst.Count == 0)
				lst = doc.SelectNodes("xs:schema/xs:complexType/xs:sequence/xs:element", mgr);
			m_columns = new FeatureSetColumn[lst.Count];
			for(int i = 0;i<lst.Count;i++)
				m_columns[i] = new FeatureSetColumn(lst[i]);

			m_row = null;

			if (m_reader.Name != "Features")
				throw new Exception("Bad document");

			m_reader.Read();

			if (m_reader.Name == "Features")
				m_reader = null; //No features :(
			else if (m_reader.Name != "Feature")
				throw new Exception("Bad document");
		}

		public FeatureSetColumn[] Columns
		{
			get { return m_columns; }
		}

		public bool Read()
		{
			if (m_rd != null)
			{
				bool next = m_rd.ReadNext();
				m_row = new FeatureSetRow(this, m_rd);
				return next;
			}
			else
			{

				if (m_reader == null || m_reader.Name != "Feature")
				{
					m_row = null;
					return false;
				}

				string xmlfragment = m_reader.ReadOuterXml();
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xmlfragment);

				m_row = new FeatureSetRow(this, doc["Feature"]);
				if (m_reader.Name != "Feature")
				{
					m_reader.Close();
					m_reader = null;
				}

				return true;
			}
		}

		public FeatureSetRow Row
		{
			get { return m_row; }
		}

	}

	public class FeatureSetColumn
	{
		internal FeatureSetColumn(string name, int type)
		{
			m_name = name;
			m_type = Utility.ConvertMgTypeToNetType(type);
		}

		internal FeatureSetColumn(XmlNode node)
		{
			m_name = node.Attributes["name"].Value;
			m_allowNull = node.Attributes["minOccurs"] != null && node.Attributes["minOccurs"].Value == "0";
            if (node.Attributes["type"] != null && node.Attributes["type"].Value == "gml:AbstractGeometryType")
                m_type = Utility.GeometryType;
            else if (node["xs:simpleType"] == null)
                m_type = Utility.RasterType;
            else
                switch (node["xs:simpleType"]["xs:restriction"].Attributes["base"].Value.ToLower())
                {
                    case "xs:string":
                        m_type = typeof(string);
                        break;
                    case "fdo:byte":
                        m_type = typeof(Byte);
                        break;
                    case "fdo:int32":
                        m_type = typeof(int);
                        break;
                    case "fdo:int16":
                        m_type = typeof(short);
                        break;
                    case "fdo:int64":
                        m_type = typeof(long);
                        break;
                    case "xs:float":
                        m_type = typeof(float);
                        break;
                    case "xs:double":
                    case "xs:decimal":
                        m_type = typeof(double);
                        break;
                    case "xs:boolean":
                        m_type = typeof(bool);
                        return;
                    case "xs:datetime":
                        m_type = typeof(DateTime);
                        break;
                    default:
                        throw new Exception("Failed to find appropriate type for: " + node["xs:simpleType"]["xs:restriction"].Attributes["base"].Value);
                }
		}

		private string m_name;
		private Type m_type;
		private bool m_allowNull;

		public string Name { get { return m_name; } }
		public Type Type { get { return m_type; } }
	}

	public class FeatureSetRow
	{
        private Topology.IO.WKTReader m_reader = null;
        
        private bool m_hasMgReader = true;
        private Topology.IO.MapGuide.MgReader m_mgReader = null;

        private Topology.IO.WKTReader Reader
        {
            get
            {
                if (m_reader == null)
                    m_reader = new Topology.IO.WKTReader();
                return m_reader;
            }
        }

        private Topology.IO.MapGuide.MgReader MgReader
        {
            get
            {
                if (m_hasMgReader)
                {
                    try
                    {
                        m_mgReader = null;
                        m_mgReader = new Topology.IO.MapGuide.MgReader();
                    }
                    catch
                    {
                        m_hasMgReader = false;
                    }
                }

                return m_mgReader;
            }
        }

		private FeatureSetRow(FeatureSetReader parent)
		{
			m_parent = parent;
			m_items = new object[parent.Columns.Length];
			m_nulls = new bool[parent.Columns.Length];
			m_lazyloadGeometry = new bool[parent.Columns.Length];
			for(int i = 0;i < m_nulls.Length; i++)
			{
				m_nulls[i] = true;
				m_lazyloadGeometry[i] = false;
			}
		}

		internal FeatureSetRow(FeatureSetReader parent, OSGeo.MapGuide.MgFeatureReader rd)
			: this(parent)
		{
			for(int i = 0; i < m_parent.Columns.Length; i++)
			{
				string p = m_parent.Columns[i].Name;
				int ordinal = GetOrdinal(p);
				m_nulls[ordinal] = false;
                				
				if (parent.Columns[ordinal].Type == typeof(string))
					m_items[ordinal] = rd.GetString(p);
				else if (parent.Columns[ordinal].Type == typeof(int))
					m_items[ordinal] = rd.GetInt32(p);
				else if (parent.Columns[ordinal].Type == typeof(short))
					m_items[ordinal] = rd.GetInt16(p);
				else if (parent.Columns[ordinal].Type == typeof(double))
					m_items[ordinal] = rd.GetDouble(p);
				else if (parent.Columns[ordinal].Type == typeof(bool))
					m_items[ordinal] = rd.GetBoolean(p);
				else if (parent.Columns[ordinal].Type == typeof(DateTime))
					m_items[ordinal] = rd.GetDateTime(p);
				else if (parent.Columns[ordinal].Type == Utility.GeometryType)
				{
                    //TODO: Uncomment this once the API gets updated to 2.0.0
                    //It is optional to include the Topology.IO.MapGuide dll
                    /*if (this.MgReader != null)
                        m_items[ordinal] = this.MgReader.ReadGeometry(ref rd, p);
                    else*/
                    {
                        //No MapGuide dll, convert to WKT and then to internal representation
                        System.IO.MemoryStream ms = Utility.MgStreamToNetStream(rd, rd.GetType().GetMethod("GetGeometry"), new string[] { p });
                        OSGeo.MapGuide.MgAgfReaderWriter rdw = new OSGeo.MapGuide.MgAgfReaderWriter();
                        OSGeo.MapGuide.MgGeometry g = rdw.Read(rd.GetGeometry(p));
                        OSGeo.MapGuide.MgWktReaderWriter rdww = new OSGeo.MapGuide.MgWktReaderWriter();
                        m_items[ordinal] = m_reader.Read(rdww.Write(g));
                    }
				}
				else
					throw new Exception("Unknown type: " + parent.Columns[ordinal].Type.FullName);
			}
		}

		internal FeatureSetRow(FeatureSetReader parent, XmlNode node)
			: this(parent)
		{
			foreach(XmlNode p in node.SelectNodes("Property"))
			{
				int ordinal = GetOrdinal(p["Name"].InnerText);
				if (!m_nulls[ordinal])
					throw new Exception("Bad document, multiple: " + p["Name"].InnerText + " values in a single feature");
				m_nulls[ordinal] = false;
				
				if (parent.Columns[ordinal].Type == typeof(string))
					m_items[ordinal] = p["Value"].InnerText;
				else if (parent.Columns[ordinal].Type == typeof(int))
					m_items[ordinal] = XmlConvert.ToInt32(p["Value"].InnerText);
				else if (parent.Columns[ordinal].Type == typeof(short))
					m_items[ordinal] = XmlConvert.ToInt16(p["Value"].InnerText);
				else if (parent.Columns[ordinal].Type == typeof(double))
					m_items[ordinal] = XmlConvert.ToDouble(p["Value"].InnerText);
				else if (parent.Columns[ordinal].Type == typeof(bool))
					m_items[ordinal] = XmlConvert.ToBoolean(p["Value"].InnerText);
				else if (parent.Columns[ordinal].Type == typeof(DateTime))
					m_items[ordinal] = XmlConvert.ToDateTime(p["Value"].InnerText, XmlDateTimeSerializationMode.Unspecified);
				else if (parent.Columns[ordinal].Type == Utility.GeometryType)
				{
					m_items[ordinal] = p["Value"].InnerText; //Geometry.WKTReader.Deserialize(p["Value"].InnerText);
					m_lazyloadGeometry[ordinal] = true;
				}
				else
					throw new Exception("Unknown type: " + parent.Columns[ordinal].Type.FullName);
			}
		}

		private FeatureSetReader m_parent;
		private object[] m_items;
		private bool[] m_nulls;
		private bool[] m_lazyloadGeometry;

		public bool IsValueNull(string name)
		{
			return IsValueNull(GetOrdinal(name));
		}

		public bool IsValueNull(int index)
		{
			if (index >= m_nulls.Length)
				throw new InvalidOperationException("Index " + index.ToString() + ", was out of bounds");
			else
				return m_nulls[index];
		}

		public object this[int index]
		{
			get 
			{
				if (index >= m_items.Length)
					throw new InvalidOperationException("Index " + index.ToString() + ", was out of bounds");
				else
				{
					if (m_lazyloadGeometry[index] && !m_nulls[index])
					{
						m_items[index] = this.Reader.Read((string)m_items[index]);
						m_lazyloadGeometry[index] = false;
					}

					return m_items[index];
				}
			}
		}

		public int GetOrdinal(string name)
		{
			name = name.Trim();

			for(int i = 0; i < m_parent.Columns.Length; i++)
				if (m_parent.Columns[i].Name.Equals(name))
					return i;

			for(int i = 0; i < m_parent.Columns.Length; i++)
				if (m_parent.Columns[i].Name.ToLower().Equals(name.ToLower()))
					return i;

			string[] t = new string[m_parent.Columns.Length];
			for(int i = 0; i < m_parent.Columns.Length; i++)
				t[i] = m_parent.Columns[i].Name;

			throw new InvalidOperationException("Column name: " + name + ", was not found\nColumn names (" + m_parent.Columns.Length.ToString() + "): " + string.Join(", ", t));
		}

		public object this[string name]
		{
			get 
			{
				return this[GetOrdinal(name)];
			}
		}
	}
}