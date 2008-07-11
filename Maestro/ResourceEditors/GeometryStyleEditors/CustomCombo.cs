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

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
	/// <summary>
	/// A combobox that is able to display custom items
	/// </summary>
	public class CustomCombo : System.Windows.Forms.ComboBox 
	{

		public CustomCombo()
			: base()
		{
			if (!this.DesignMode)
			{
				base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
				base.DropDownStyle = ComboBoxStyle.DropDownList;
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			bool handled = false;

			if (e.Index < 0)
				return;

			if (m_render != null)
				handled = m_render(e, this.Items[e.Index]);

			if (!handled && this.Items[e.Index].GetType().GetInterface(typeof(CustomComboItem).FullName, false) != null)
				((CustomComboItem)this.Items[e.Index]).DrawItem(e);
			else if (!handled)
			{
				//Standard handler
				e.DrawBackground();
				StringFormat format = new StringFormat();
				format.Trimming = StringTrimming.EllipsisCharacter;
				format.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.FitBlackBox;

				using(Brush b = new SolidBrush(this.ForeColor))
					e.Graphics.DrawString(this.Items[e.Index].ToString(), this.Font, b, e.Bounds, format);

				base.OnDrawItem (e);

				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					e.DrawFocusRectangle();
			}
		}

		/// <summary>
		/// Property is overridden to prevent the designer from modifying the value
		/// </summary>
		[ System.ComponentModel.Browsable(false), System.ComponentModel.ReadOnly(true) ]
		public new DrawMode DrawMode
		{
			get { return base.DrawMode; }
			set { base.DrawMode = value; }
		}

		/// <summary>
		/// Property is overridden to prevent the designer for modifying the value
		/// </summary>
		[ System.ComponentModel.Browsable(false), System.ComponentModel.ReadOnly(true) ]
		public new ComboBoxStyle DropDownStyle
		{
			get { return base.DropDownStyle; }
			set { base.DropDownStyle = value; }
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			}
			base.Dispose( disposing );
		}


		/// <summary>
		/// Delegate used for handling custom rendering. Return true if handled, false otherwise
		/// </summary>
		public delegate bool RenderCustomItem(DrawItemEventArgs e, object value);

		/// <summary>
		/// Holder for custom render
		/// </summary>
		private RenderCustomItem m_render = null;

		/// <summary>
		/// Accessor for custom render
		/// </summary>
		public RenderCustomItem CustomRender
		{
			get { return m_render; }
			set { m_render = value; }
		}

		/// <summary>
		/// Interface for items in custom combo
		/// </summary>
		public interface CustomComboItem
		{
			/// <summary>
			/// Method that gets called when an object should be rendered
			/// </summary>
			void DrawItem(DrawItemEventArgs e);
		}
	}
}