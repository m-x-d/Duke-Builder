
#region ================== Copyright (c) 2007 Pascal vd Heiden

/*
 * Copyright (c) 2007 Pascal vd Heiden, www.codeimp.com
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System;
using System.Windows.Forms;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.Controls
{
	internal partial class ColorControl : UserControl
	{
		#region ================== Properties

		public string Label { get { return label.Text; } set { label.Text = value; } }
		public PixelColor Color { get { return PixelColor.FromColor(panel.BackColor); } set { panel.BackColor = System.Drawing.Color.FromArgb(value.ToInt()); } }

		#endregion

		#region ================== Constructor

		public ColorControl()
		{
			// Initialize
			InitializeComponent();
		}

		#endregion

		#region ================== Events

		// Show color picker
		private void panel_Click(object sender, EventArgs e)
		{
			// Show color dialog
			dialog.Color = panel.BackColor;
			if(dialog.ShowDialog(this.ParentForm) == DialogResult.OK)
			{
				// Apply new color
				panel.BackColor = dialog.Color;
			}
		}

		// Resized
		private void ColorControl_Resize(object sender, EventArgs e)
		{
			panel.Left = ClientSize.Width - panel.Width - 3;
			label.Left = 0;
			label.Width = panel.Left;
		}

		#endregion
	}
}
