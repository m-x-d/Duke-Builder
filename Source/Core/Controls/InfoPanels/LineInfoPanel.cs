
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

using System.Windows.Forms;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Controls
{
	internal partial class LineInfoPanel : UserControl
	{
		#region ================== Constructor
		
		public LineInfoPanel()
		{
			// Initialize
			InitializeComponent();
		}

		#endregion

		#region ================== Methods

		// This shows the info
		public void ShowInfo(Linedef l)
		{
			// Setup line
			length.Text = l.Length.ToString("0.##");
			angle.Text = l.AngleDeg + "\u00B0";
			infopanel.Text = " Line " + l.Index;
			
			// Setup walls
			bool havefront = (l.Front != null);
			bool haveback = (l.Back != null);
			
			front.Visible = havefront;
			if(havefront) front.ShowInfo(l.Front);
			
			back.Visible = haveback;
			if(haveback) back.ShowInfo(l.Back);

			// Reposition panels
			if(havefront && haveback)
			{
				back.Left = front.Right;
				this.Width = back.Right + back.Margin.Right;
			}
			else if(!havefront)
			{
				back.Left = infopanel.Right + infopanel.Margin.Right;
				this.Width = back.Right + back.Margin.Right;
			}
			else
			{
				this.Width = front.Right + front.Margin.Right;
			}

			// Show the whole thing
			this.Show();
			this.Update();
		}

		#endregion
	}
}
