﻿#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.FeatureSource.Preview
{
    /// <summary>
    /// A control that allows local previewing of a feature source
    /// </summary>
    public partial class LocalFeatureSourcePreviewCtrl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFeatureSourcePreviewCtrl"/> class.
        /// </summary>
        public LocalFeatureSourcePreviewCtrl()
        {
            InitializeComponent();
        }

        private IFeatureService _fsvc;

        /// <summary>
        /// Initializes this instance
        /// </summary>
        /// <param name="featureService">The feature service.</param>
        public void Init(IFeatureService featureService)
        {
            _fsvc = featureService;
        }

        const int IDX_SCHEMA = 0;
        const int IDX_CLASS = 1;
        const int IDX_PROP = 2;
        const int IDX_IDENTITY = 3;
        const int IDX_GEOMETRY = 4;
        const int IDX_RASTER = 5;

        private string currentFsId;

        /// <summary>
        /// Gets or sets a value indicating whether [supports SQL].
        /// </summary>
        /// <value><c>true</c> if [supports SQL]; otherwise, <c>false</c>.</value>
        public bool SupportsSQL
        {
            get;
            set;
        }

        private FdoProviderCapabilities _caps;

        /// <summary>
        /// Reloads the tree.
        /// </summary>
        /// <param name="fsId">The fs id.</param>
        /// <param name="caps">The caps.</param>
        public void ReloadTree(string fsId, FdoProviderCapabilities caps)
        {
            currentFsId = fsId;
            _caps = caps;
            ClearPreviewPanes();
            trvSchema.Nodes.Clear();
            
            //FIXME: Do this lazily ala. FDO Toolbox
            var schema = _fsvc.DescribeFeatureSource(currentFsId);

            Dictionary<string, List<ClassDefinition>> classes = new Dictionary<string, List<ClassDefinition>>();
            foreach (var cls in schema.AllClasses)
            {
                string[] tokens = cls.QualifiedName.Split(':');
                if (!classes.ContainsKey(tokens[0]))
                    classes[tokens[0]] = new List<ClassDefinition>();

                classes[tokens[0]].Add(cls);
            }

            string[] schemaNames = schema.SchemaNames;
            foreach (var s in schemaNames)
            {
                var schemaNode = new TreeNode(s);
                schemaNode.Tag = s;
                schemaNode.ImageIndex = schemaNode.SelectedImageIndex = IDX_SCHEMA;

                trvSchema.Nodes.Add(schemaNode);

                if (classes.ContainsKey(s))
                {
                    foreach (var cls in classes[s])
                    {
                        var classNode = new TreeNode(cls.Name);
                        classNode.Text = cls.Name;
                        classNode.Tag = cls;
                        classNode.ImageIndex = classNode.SelectedImageIndex = IDX_CLASS;

                        classNode.ToolTipText = string.Format(Properties.Resources.FsPreview_ClassNodeTooltip,
                            cls.Name,
                            cls.Description,
                            cls.DefaultGeometryPropertyName,
                            Environment.NewLine);

                        foreach (var prop in cls.Properties)
                        {
                            var propNode = new TreeNode(prop.Name);
                            propNode.Text = prop.Name;
                            propNode.Tag = prop;

                            if (prop.Type == PropertyDefinitionType.Geometry)
                            {
                                var g = (GeometricPropertyDefinition)prop;
                                propNode.ImageIndex = propNode.SelectedImageIndex = IDX_GEOMETRY;
                                propNode.ToolTipText = string.Format(Properties.Resources.FsPreview_GeometryPropertyNodeTooltip,
                                    g.Name,
                                    g.Description,
                                    g.GeometryTypesToString(),
                                    g.IsReadOnly,
                                    g.HasElevation,
                                    g.HasMeasure,
                                    g.SpatialContextAssociation,
                                    Environment.NewLine);
                            }
                            else if (prop.Type == PropertyDefinitionType.Data)
                            {
                                var d = (DataPropertyDefinition)prop;
                                if (cls.IdentityProperties.Contains((DataPropertyDefinition)prop))
                                    propNode.ImageIndex = propNode.SelectedImageIndex = IDX_IDENTITY;
                                else
                                    propNode.ImageIndex = propNode.SelectedImageIndex = IDX_PROP;

                                propNode.ToolTipText = string.Format(Properties.Resources.FsPreview_DataPropertyNodeTooltip,
                                    d.Name,
                                    d.Description,
                                    d.DataType.ToString(),
                                    d.IsNullable,
                                    d.IsReadOnly,
                                    d.Length,
                                    d.Precision,
                                    d.Scale,
                                    Environment.NewLine);
                            }
                            else if (prop.Type == PropertyDefinitionType.Raster)
                            {
                                var r = (RasterPropertyDefinition)prop;
                                propNode.ImageIndex = propNode.SelectedImageIndex = IDX_RASTER;

                                propNode.ToolTipText = string.Format(Properties.Resources.FsPreview_RasterPropertyNodeTooltip,
                                    r.Name,
                                    r.Description,
                                    r.IsNullable,
                                    r.DefaultImageXSize,
                                    r.DefaultImageYSize,
                                    r.SpatialContextAssociation,
                                    Environment.NewLine);
                            }
                            else
                            {
                                propNode.ImageIndex = propNode.SelectedImageIndex = IDX_PROP;
                            }

                            classNode.Nodes.Add(propNode);
                        }

                        schemaNode.Nodes.Add(classNode);
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReloadTree(currentFsId, _caps);
        }

        private void btnSql_Click(object sender, EventArgs e)
        {
            var cls = GetSelectedClass();
            if (cls != null)
            {
                AddPreviewPane(cls, QueryMode.SQL);
            }
        }

        private void btnStandard_Click(object sender, EventArgs e)
        {
            var cls = GetSelectedClass();
            if (cls != null)
            {
                AddPreviewPane(cls, QueryMode.Standard);
            }
        }

        private void ClearPreviewPanes()
        {
            tabPreviews.TabPages.Clear();
            hasSql = false;
        }

        private bool hasSql = false;

        void AddPreviewPane(ClassDefinition cls, QueryMode mode)
        {
            if (mode == QueryMode.SQL)
            {
                if (!hasSql)
                {
                    var pane = new PreviewPane(currentFsId, mode, cls, _fsvc, _caps);
                    var page = new TabPage();
                    page.Text = Properties.Resources.SQLQuery;
                    page.Tag = mode;
                    pane.Dock = DockStyle.Fill;
                    page.Controls.Add(pane);
                    tabPreviews.TabPages.Add(page);
                    tabPreviews.SelectedIndex = tabPreviews.TabPages.IndexOf(page);
                    hasSql = true;
                }
            }
            else
            {
                var pane = new PreviewPane(currentFsId, mode, cls, _fsvc, _caps);
                var page = new TabPage();
                page.Text = Properties.Resources.StandardQuery + " - " + cls.QualifiedName;
                page.Tag = mode;
                pane.Dock = DockStyle.Fill;
                page.Controls.Add(pane);
                tabPreviews.TabPages.Add(page);
                tabPreviews.SelectedIndex = tabPreviews.TabPages.IndexOf(page);
            }

            btnClose.Enabled = (tabPreviews.TabPages.Count > 0);
        }

        ClassDefinition GetSelectedClass()
        {
            if (trvSchema.SelectedNode != null)
            {
                return trvSchema.SelectedNode.Tag as ClassDefinition;
            }
            return null;
        }

        private void trvSchema_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Level)
            {
                case 1: //Class
                    var cls = e.Node.Tag as ClassDefinition;
                    if (cls != null)
                    {
                        btnStandard.Enabled = true;
                        btnSql.Enabled = this.SupportsSQL;
                    }
                    break;
                default:
                    btnStandard.Enabled = false;
                    btnSql.Enabled = false;
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (tabPreviews.SelectedIndex >= 0)
            {
                //This is almost the same tab removal logic from TabFactory.cs in Maestro.Base
                //done this way to remove any doubts about Mono

                int idx = -1;
                //HACK: Mono (2.4) will chuck a hissy fit if we remove
                //a tab from a TabControl that has a selected tab so we
                //have to null the selected tab, but this cause weird
                //visual effects once the tab is removed, so we record
                //the selected index, so we can assign the one beside it
                //to be the selected tab after removal.
                idx = tabPreviews.SelectedIndex;
                var tab = tabPreviews.TabPages[idx];
                tabPreviews.SelectedTab = null;
                tabPreviews.TabPages.RemoveAt(idx);

                if ((QueryMode)tab.Tag == QueryMode.SQL)
                    hasSql = false;

                if (idx > 0)
                {
                    idx--;
                    tabPreviews.SelectedIndex = idx;
                }
                else
                {
                    //Set to first tab if available.
                    if (tabPreviews.TabCount > 0)
                    {
                        tabPreviews.SelectedIndex = 0;
                    }
                }

                btnClose.Enabled = (tabPreviews.TabPages.Count > 0);
            }
        }
    }
}
