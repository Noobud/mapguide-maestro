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
using OSGeo.MapGuide.MaestroAPI.ApplicationDefinition;
using System.Xml;

namespace OSGeo.MapGuide.Maestro.FusionEditor.CustomizedEditors
{
	/// <summary>
	/// Summary description for ActivityIndicator.
	/// </summary>
	public class ActivityIndicator : BasisWidgetEditor
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox ElementId;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ActivityIndicator()
		{
			// This call is required by the Windows.Forms Form Designer.
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
				if(components != null)
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
				
				ElementId.Text = GetSettingValue("ElementId");
			}
			finally
			{
				m_isUpdating = false;
			}
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.ElementId = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "ElementId";
			// 
			// ElementId
			// 
			this.ElementId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.ElementId.Location = new System.Drawing.Point(104, 8);
			this.ElementId.Name = "ElementId";
			this.ElementId.Size = new System.Drawing.Size(504, 20);
			this.ElementId.TabIndex = 1;
			this.ElementId.Text = "";
			this.ElementId.TextChanged += new System.EventHandler(this.ElementId_TextChanged);
			// 
			// ActivityIndicator
			// 
			this.Controls.Add(this.ElementId);
			this.Controls.Add(this.label1);
			this.Name = "ActivityIndicator";
			this.Size = new System.Drawing.Size(616, 40);
			this.ResumeLayout(false);

		}
		#endregion

		private void ElementId_TextChanged(object sender, System.EventArgs e)
		{
			if (m_isUpdating || m_w == null)
				return;

			SetSettingValue("ElementId", ElementId.Text);
		}
	}
}