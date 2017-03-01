#region ================== Namespaces

using System.Windows.Forms;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Controls.InfoPanels
{
	public partial class WallInfoPanel : UserControl
	{
		#region ================== Constructor

		public WallInfoPanel()
		{
			InitializeComponent();
		}

		#endregion

		#region ================== Methods

		// This shows the info
		public void ShowInfo(Sidedef w)
		{
			// Setup line
			length.Text = w.Line.Length.ToString("0.##");
			angle.Text = General.WrapAngle(w.Line.AngleDeg) + "\u00B0";
			infopanel.Text = " Line " + w.Line.Index;

			// Setup wall
			wall.ShowInfo(w);

			// Reposition panels
			this.Width = wall.Right + wall.Margin.Right;

			// Show the whole thing
			this.Show();
			this.Update();
		}

		#endregion
	}
}
