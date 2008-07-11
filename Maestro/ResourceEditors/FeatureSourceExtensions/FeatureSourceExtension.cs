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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.Maestro;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions
{
	/// <summary>
	/// Summary description for FeatureSourceExtension.
	/// </summary>
	public class FeatureSourceExtension : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ImageList imageList;
		private System.ComponentModel.IContainer components;

		private EditorInterface m_editor;
		private FeatureSourceTypeExtensionCollection m_extensions;
		private bool m_isUpdating = false;
		private System.Windows.Forms.TreeView treeView;
		private Globalizator.Globalizator m_globalizor;
		private FeatureSource m_feature;
		private FeatureSourceDescription.FeatureSourceSchema[] m_fsd;

		private const int ICON_EXTENSION = 5;
		private const int ICON_JOIN = 6;
		private const int ICON_CALCULATED = 7;
		private const int ICON_KEY = 8;
		private OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.Calculated calculated;
        private OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.Join join;
		private OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.Key key;
        private ToolStrip toolStrip;
        private ToolStripButton AddExtensionButton;
        private ToolStripButton AddComputationButton;
        private ToolStripButton AddJoinButton;
        private ToolStripButton AddKeyButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton RemoveItemButton;
		private OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.Extension extension;

		public delegate void NameChangedDelegate(object item, string newname);

		public FeatureSourceExtension()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			calculated.Visible = join.Visible = extension.Visible = key.Visible = false;
			calculated.Dock = join.Dock = extension.Dock  = key.Dock = DockStyle.Fill;
		}

		public void SetItem(EditorInterface editor, FeatureSource feature, FeatureSourceTypeExtensionCollection extensions, Globalizator.Globalizator globalizor)
		{
			m_editor = editor;
			m_extensions = extensions;
			m_globalizor = globalizor;
			m_feature = feature;

			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;

				treeView.Nodes.Clear();
				if (m_extensions == null)
					return;

				ArrayList approved = new ArrayList();
				try
				{
					foreach(FeatureSourceDescription.FeatureSourceSchema scm in m_feature.DescribeSource().Schemas)
					{
						bool isApproved = true;
						foreach(FeatureSourceTypeExtension t in m_extensions)
							if (scm.Name == t.Name)
							{
								isApproved = false;
								break;
							}
						if (isApproved)
							approved.Add(scm);
					}

					m_fsd = (FeatureSourceDescription.FeatureSourceSchema[])approved.ToArray(typeof(FeatureSourceDescription.FeatureSourceSchema));
				}
				catch
				{
					m_fsd = new FeatureSourceDescription.FeatureSourceSchema[0];
				}

				foreach(FeatureSourceTypeExtension t in m_extensions)
				{
					TreeNode n = new TreeNode(t.Name, ICON_EXTENSION, ICON_EXTENSION);
					n.Tag = t;

					if (t.CalculatedProperty != null)
						foreach(CalculatedPropertyType ct in t.CalculatedProperty)
						{
							TreeNode cn = new TreeNode(ct.Name, ICON_CALCULATED, ICON_CALCULATED);
							cn.Tag = ct;
							n.Nodes.Add(cn);
						}

					if (t.AttributeRelate != null)
						foreach(AttributeRelateType at in t.AttributeRelate)
						{
							TreeNode an = new TreeNode(at.Name, ICON_JOIN, ICON_JOIN);
							an.Tag = at;

							
							foreach(RelatePropertyType rpt in at.RelateProperty)
							{
								TreeNode rn = new TreeNode(rpt.FeatureClassProperty + " : " + rpt.AttributeClassProperty, ICON_KEY, ICON_KEY);
								rn.Tag = rpt;
								an.Nodes.Add(rn);
							}

							n.Nodes.Add(an);
						}

					treeView.Nodes.Add(n);
				}

				treeView.ExpandAll();
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSourceExtension));
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.AddExtensionButton = new System.Windows.Forms.ToolStripButton();
            this.AddComputationButton = new System.Windows.Forms.ToolStripButton();
            this.AddJoinButton = new System.Windows.Forms.ToolStripButton();
            this.AddKeyButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RemoveItemButton = new System.Windows.Forms.ToolStripButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.key = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.Key();
            this.extension = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.Extension();
            this.join = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.Join();
            this.calculated = new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.Calculated();
            this.panel1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeView);
            this.panel1.Controls.Add(this.toolStrip);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(176, 344);
            this.panel1.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(176, 319);
            this.treeView.TabIndex = 1;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "AddExtension.ico");
            this.imageList.Images.SetKeyName(1, "AddDataJoin.ico");
            this.imageList.Images.SetKeyName(2, "AddCalculator.ico");
            this.imageList.Images.SetKeyName(3, "AddKey.ico");
            this.imageList.Images.SetKeyName(4, "Extension.ico");
            this.imageList.Images.SetKeyName(5, "DataJoin.ico");
            this.imageList.Images.SetKeyName(6, "Calculator.ico");
            this.imageList.Images.SetKeyName(7, "Key.ico");
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddExtensionButton,
            this.AddComputationButton,
            this.AddJoinButton,
            this.AddKeyButton,
            this.toolStripSeparator1,
            this.RemoveItemButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(176, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // AddExtensionButton
            // 
            this.AddExtensionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddExtensionButton.Image = ((System.Drawing.Image)(resources.GetObject("AddExtensionButton.Image")));
            this.AddExtensionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddExtensionButton.Name = "AddExtensionButton";
            this.AddExtensionButton.Size = new System.Drawing.Size(23, 22);
            this.AddExtensionButton.Text = "toolStripButton1";
            this.AddExtensionButton.ToolTipText = "Add a new extension";
            this.AddExtensionButton.Click += new System.EventHandler(this.AddExtensionButton_Click);
            // 
            // AddComputationButton
            // 
            this.AddComputationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddComputationButton.Enabled = false;
            this.AddComputationButton.Image = ((System.Drawing.Image)(resources.GetObject("AddComputationButton.Image")));
            this.AddComputationButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddComputationButton.Name = "AddComputationButton";
            this.AddComputationButton.Size = new System.Drawing.Size(23, 22);
            this.AddComputationButton.Text = "toolStripButton2";
            this.AddComputationButton.ToolTipText = "Add a new calculated property";
            this.AddComputationButton.Click += new System.EventHandler(this.AddComputationButton_Click);
            // 
            // AddJoinButton
            // 
            this.AddJoinButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddJoinButton.Enabled = false;
            this.AddJoinButton.Image = ((System.Drawing.Image)(resources.GetObject("AddJoinButton.Image")));
            this.AddJoinButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddJoinButton.Name = "AddJoinButton";
            this.AddJoinButton.Size = new System.Drawing.Size(23, 22);
            this.AddJoinButton.Text = "toolStripButton3";
            this.AddJoinButton.ToolTipText = "Add a new database join";
            this.AddJoinButton.Click += new System.EventHandler(this.AddJoinButton_Click);
            // 
            // AddKeyButton
            // 
            this.AddKeyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddKeyButton.Enabled = false;
            this.AddKeyButton.Image = ((System.Drawing.Image)(resources.GetObject("AddKeyButton.Image")));
            this.AddKeyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddKeyButton.Name = "AddKeyButton";
            this.AddKeyButton.Size = new System.Drawing.Size(23, 22);
            this.AddKeyButton.Text = "toolStripButton4";
            this.AddKeyButton.ToolTipText = "Add a new key mapping";
            this.AddKeyButton.Click += new System.EventHandler(this.AddKeyButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // RemoveItemButton
            // 
            this.RemoveItemButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveItemButton.Enabled = false;
            this.RemoveItemButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveItemButton.Image")));
            this.RemoveItemButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveItemButton.Name = "RemoveItemButton";
            this.RemoveItemButton.Size = new System.Drawing.Size(23, 22);
            this.RemoveItemButton.Text = "toolStripButton5";
            this.RemoveItemButton.ToolTipText = "Delete the selected item";
            this.RemoveItemButton.Click += new System.EventHandler(this.RemoveItemButton_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(176, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 344);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.key);
            this.panel2.Controls.Add(this.extension);
            this.panel2.Controls.Add(this.join);
            this.panel2.Controls.Add(this.calculated);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(179, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(205, 344);
            this.panel2.TabIndex = 2;
            // 
            // key
            // 
            this.key.Location = new System.Drawing.Point(24, 280);
            this.key.Name = "key";
            this.key.Size = new System.Drawing.Size(152, 56);
            this.key.TabIndex = 4;
            this.key.NameChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.FeatureSourceExtension.NameChangedDelegate(this.item_NameChanged);
            // 
            // extension
            // 
            this.extension.Location = new System.Drawing.Point(16, 192);
            this.extension.Name = "extension";
            this.extension.Size = new System.Drawing.Size(160, 80);
            this.extension.TabIndex = 2;
            this.extension.NameChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.FeatureSourceExtension.NameChangedDelegate(this.item_NameChanged);
            // 
            // join
            // 
            this.join.Location = new System.Drawing.Point(16, 112);
            this.join.Name = "join";
            this.join.Size = new System.Drawing.Size(160, 64);
            this.join.TabIndex = 1;
            this.join.NameChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.FeatureSourceExtension.NameChangedDelegate(this.item_NameChanged);
            // 
            // calculated
            // 
            this.calculated.Location = new System.Drawing.Point(16, 16);
            this.calculated.Name = "calculated";
            this.calculated.Size = new System.Drawing.Size(150, 88);
            this.calculated.TabIndex = 3;
            this.calculated.NameChanged += new OSGeo.MapGuide.Maestro.ResourceEditors.FeatureSourceExtensions.FeatureSourceExtension.NameChangedDelegate(this.item_NameChanged);
            // 
            // FeatureSourceExtension
            // 
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "FeatureSourceExtension";
            this.Size = new System.Drawing.Size(384, 344);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{

			Control c = null;
			if (e == null || e.Node == null || e.Node.Tag == null)
			{
				AddJoinButton.Enabled = AddComputationButton.Enabled = AddKeyButton.Enabled = RemoveItemButton.Enabled = false;
			}
			else if (e.Node.Tag as FeatureSourceTypeExtension != null)
			{
				extension.SetItem(m_fsd, e.Node.Tag as FeatureSourceTypeExtension);
				c = extension;
				AddJoinButton.Enabled = AddComputationButton.Enabled = RemoveItemButton.Enabled = true;
				AddKeyButton.Enabled = false;
			}
			else if (e.Node.Tag as CalculatedPropertyType != null)
			{
				calculated.SetItem(e.Node.Tag as CalculatedPropertyType);
				c = calculated;
				AddJoinButton.Enabled = AddComputationButton.Enabled = RemoveItemButton.Enabled = true;
				AddKeyButton.Enabled = false;
			}
			else if (e.Node.Tag as AttributeRelateType != null)
			{
				join.SetItem(e.Node.Tag as AttributeRelateType, m_editor);
				c = join;
				AddJoinButton.Enabled = AddComputationButton.Enabled = RemoveItemButton.Enabled = true;
				AddKeyButton.Enabled = true;
			}
			else if (e.Node.Tag as RelatePropertyType != null)
			{
				AttributeRelateType atr = e.Node.Parent.Tag as AttributeRelateType;
				FeatureSourceTypeExtension ext = e.Node.Parent.Parent.Tag as FeatureSourceTypeExtension;

				FeatureSourceDescription.FeatureSourceSchema scm1 = null;
				FeatureSourceDescription.FeatureSourceSchema scm2 = null;

				foreach(FeatureSourceDescription.FeatureSourceSchema scm in m_fsd)
					if (scm.Fullname == ext.FeatureClass)
					{
						scm1 = scm;
						break;
					}

				try
				{
					foreach(FeatureSourceDescription.FeatureSourceSchema scm in m_editor.CurrentConnection.DescribeFeatureSource(atr.ResourceId).Schemas)
						if (scm.Fullname == atr.AttributeClass)
						{
							scm2 = scm;
							break;
						}
				}
				catch
				{
					scm2 = null;
				}

				key.SetItem(scm1, scm2, e.Node.Tag as RelatePropertyType);
				c = key;
				AddJoinButton.Enabled = AddComputationButton.Enabled = false;
				RemoveItemButton.Enabled = true;
				AddKeyButton.Enabled = true;
			}
			else
				AddJoinButton.Enabled = AddComputationButton.Enabled = AddKeyButton.Enabled = RemoveItemButton.Enabled = false;


			foreach(Control cx in panel2.Controls)
				cx.Visible = cx == c;
		}

		private void item_NameChanged(object item, string newname)
		{
			if (treeView.SelectedNode != null && treeView.SelectedNode.Tag == item)
				treeView.SelectedNode.Text = newname;
		}

        private void AddExtensionButton_Click(object sender, EventArgs e)
        {
            FeatureSourceTypeExtension v = new FeatureSourceTypeExtension();
            v.Name = "New extension";
            m_extensions.Add(v);

            TreeNode n = new TreeNode(v.Name, ICON_EXTENSION, ICON_EXTENSION);
            n.Tag = v;
            treeView.Nodes.Add(n);
            treeView.SelectedNode = n;
        }

        private void AddComputationButton_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Tag == null)
                return;

            FeatureSourceTypeExtension p = treeView.SelectedNode.Tag as FeatureSourceTypeExtension;
            TreeNode pn = treeView.SelectedNode;
            if (p == null)
            {
                p = treeView.SelectedNode.Parent.Tag as FeatureSourceTypeExtension;
                pn = treeView.SelectedNode.Parent;
            }

            if (p == null)
                return;

            CalculatedPropertyType ct = new CalculatedPropertyType();
            ct.Name = "New property";
            if (p.CalculatedProperty == null)
                p.CalculatedProperty = new CalculatedPropertyTypeCollection();
            p.CalculatedProperty.Add(ct);

            TreeNode cn = new TreeNode(ct.Name, ICON_CALCULATED, ICON_CALCULATED);
            cn.Tag = ct;
            pn.Nodes.Add(cn);
            pn.Expand();
            treeView.SelectedNode = cn;
        }

        private void AddJoinButton_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Tag == null)
                return;

            FeatureSourceTypeExtension p = treeView.SelectedNode.Tag as FeatureSourceTypeExtension;
            TreeNode pn = treeView.SelectedNode;
            if (p == null)
            {
                p = treeView.SelectedNode.Parent.Tag as FeatureSourceTypeExtension;
                pn = treeView.SelectedNode.Parent;
            }

            if (p == null)
                return;

            if (p.AttributeRelate == null)
                p.AttributeRelate = new AttributeRelateTypeCollection();
            AttributeRelateType at = new AttributeRelateType();
            at.Name = "New join";
            p.AttributeRelate.Add(at);

            TreeNode an = new TreeNode(at.Name, ICON_JOIN, ICON_JOIN);
            an.Tag = at;
            pn.Nodes.Add(an);
            pn.Expand();
            treeView.SelectedNode = an;
        }

        private void AddKeyButton_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Tag == null)
                return;

            AttributeRelateType p = treeView.SelectedNode.Tag as AttributeRelateType;
            TreeNode pn = treeView.SelectedNode;
            if (p == null)
            {
                p = treeView.SelectedNode.Parent.Tag as AttributeRelateType;
                pn = treeView.SelectedNode.Parent;
            }

            if (p == null)
                return;

            if (p.RelateProperty == null)
                p.RelateProperty = new RelatePropertyTypeCollection();

            RelatePropertyType rpt = new RelatePropertyType();
            p.RelateProperty.Add(rpt);

            TreeNode rn = new TreeNode(rpt.FeatureClassProperty + ":" + rpt.AttributeClassProperty, ICON_KEY, ICON_KEY);
            rn.Tag = rpt;
            pn.Nodes.Add(rn);
            pn.Expand();
            treeView.SelectedNode = rn;
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Tag == null)
                return;

            if (treeView.SelectedNode.Tag as CalculatedPropertyType != null)
            {
                FeatureSourceTypeExtension p = treeView.SelectedNode.Parent.Tag as FeatureSourceTypeExtension;
                for (int i = 0; i < p.CalculatedProperty.Count; i++)
                    if (p.CalculatedProperty[i] == treeView.SelectedNode.Tag)
                    {
                        TreeNode n = treeView.SelectedNode.Parent;
                        treeView.SelectedNode.Remove();
                        treeView.SelectedNode = n;
                        p.CalculatedProperty.RemoveAt(i);
                        break;
                    }
            }
            else if (treeView.SelectedNode.Tag as AttributeRelateType != null)
            {
                FeatureSourceTypeExtension p = treeView.SelectedNode.Parent.Tag as FeatureSourceTypeExtension;
                for (int i = 0; i < p.AttributeRelate.Count; i++)
                    if (p.AttributeRelate[i] == treeView.SelectedNode.Tag)
                    {
                        TreeNode n = treeView.SelectedNode.Parent;
                        treeView.SelectedNode.Remove();
                        treeView.SelectedNode = n;
                        p.AttributeRelate.RemoveAt(i);
                        break;
                    }
            }
            else if (treeView.SelectedNode.Tag as RelatePropertyType != null)
            {
                AttributeRelateType p = treeView.SelectedNode.Parent.Tag as AttributeRelateType;
                for (int i = 0; i < p.RelateProperty.Count; i++)
                    if (p.RelateProperty[i] == treeView.SelectedNode.Tag)
                    {
                        TreeNode n = treeView.SelectedNode.Parent;
                        treeView.SelectedNode.Remove();
                        treeView.SelectedNode = n;
                        p.RelateProperty.RemoveAt(i);
                        break;
                    }
            }
            else if (treeView.SelectedNode.Tag as FeatureSourceTypeExtension != null)
            {
                for (int i = 0; i < m_extensions.Count; i++)
                    if (m_extensions[i] == treeView.SelectedNode.Tag)
                    {
                        treeView.SelectedNode.Remove();
                        m_extensions.RemoveAt(i);
                        break;
                    }
            }
        }
	}
}