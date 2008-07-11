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
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;

namespace OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors
{
	public class TaskPane : FusionEditor.BasisWidgetEditor
	{
		private System.Windows.Forms.TextBox MenuContainer;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox InitialTask;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components = null;

		public TaskPane()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public override void SetItem(WidgetType w)
		{
			try
			{
				m_isUpdating = true;
				m_w = w;
				this.Enabled = m_w != null;

				InitialTask.Text = GetSettingValue("InitialTask"); 
				MenuContainer.Text = GetSettingValue("MenuContainer"); 
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.MenuContainer = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.InitialTask = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// MenuContainer
			// 
			this.MenuContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.MenuContainer.Location = new System.Drawing.Point(104, 34);
			this.MenuContainer.Name = "MenuContainer";
			this.MenuContainer.Size = new System.Drawing.Size(504, 20);
			this.MenuContainer.TabIndex = 11;
			this.MenuContainer.Text = "";
			this.MenuContainer.TextChanged += new System.EventHandler(this.MenuContainer_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "Menu container";
			// 
			// InitialTask
			// 
			this.InitialTask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.InitialTask.Location = new System.Drawing.Point(104, 10);
			this.InitialTask.Name = "InitialTask";
			this.InitialTask.Size = new System.Drawing.Size(504, 20);
			this.InitialTask.TabIndex = 9;
			this.InitialTask.Text = "";
			this.InitialTask.TextChanged += new System.EventHandler(this.InitialTask_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Initial task";
			// 
			// TaskPane
			// 
			this.Controls.Add(this.MenuContainer);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.InitialTask);
			this.Controls.Add(this.label1);
			this.Name = "TaskPane";
			this.Size = new System.Drawing.Size(616, 64);
			this.ResumeLayout(false);

		}
		#endregion

		private void InitialTask_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("InitialTask", InitialTask.Text);
		}

		private void MenuContainer_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("MenuContainer", MenuContainer.Text);
		}
	}
}
