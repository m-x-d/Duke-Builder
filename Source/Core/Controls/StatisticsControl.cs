using System.Drawing;
using System.Windows.Forms;
using mxd.DukeBuilder.Map;

namespace mxd.DukeBuilder.Controls
{
	public partial class StatisticsControl : UserControl
	{
		public StatisticsControl() 
		{
			InitializeComponent();
			this.Visible = false;
		}

		public void UpdateStatistics()
		{
			int selectedwalls = 0;
			foreach(Linedef l in General.Map.Map.SelectedLinedefs)
			{
				if(l.Front != null) selectedwalls++;
				if(l.Back != null) selectedwalls++;
			}
			
			// Update statistics
			vertscount.Text = General.Map.Map.SelectedVerticesCount + " / " + General.Map.Map.Vertices.Count + " / " + General.Map.FormatInterface.MaxWalls;
			wallscount.Text = selectedwalls + " / " + General.Map.Map.Sidedefs.Count + " / " + General.Map.FormatInterface.MaxWalls;
			linescount.Text = General.Map.Map.SelectedLinedefsCount + " / " + General.Map.Map.Linedefs.Count + " / " + General.Map.FormatInterface.MaxWalls;
			sectorscount.Text = General.Map.Map.SelectedSectorsCount + " / " + General.Map.Map.Sectors.Count + " / " + General.Map.FormatInterface.MaxSectors;
			thingscount.Text = General.Map.Map.SelectedThingsCount + " / " + General.Map.Map.Things.Count + " / " + General.Map.FormatInterface.MaxSprites;

			// Exceeding them limits?
			vertscount.ForeColor = (General.Map.Map.Vertices.Count > General.Map.FormatInterface.MaxWalls ? Color.Red : (General.Map.Map.SelectedVerticesCount > 0 ? SystemColors.Highlight : SystemColors.GrayText));
			wallscount.ForeColor = (General.Map.Map.Sidedefs.Count > General.Map.FormatInterface.MaxWalls ? Color.Red : (selectedwalls > 0 ? SystemColors.Highlight : SystemColors.GrayText));
			linescount.ForeColor = (General.Map.Map.Sidedefs.Count > General.Map.FormatInterface.MaxWalls ? Color.Red : (General.Map.Map.SelectedLinedefsCount > 0 ? SystemColors.Highlight : SystemColors.GrayText));
			sectorscount.ForeColor = (General.Map.Map.Sectors.Count > General.Map.FormatInterface.MaxSectors ? Color.Red : (General.Map.Map.SelectedSectorsCount > 0 ? SystemColors.Highlight : SystemColors.GrayText));
			thingscount.ForeColor = (General.Map.Map.Things.Count > General.Map.FormatInterface.MaxSprites ? Color.Red : (General.Map.Map.SelectedThingsCount > 0 ? SystemColors.Highlight : SystemColors.GrayText));

			vertslabel.ForeColor = vertscount.ForeColor;
			wallslabel.ForeColor = wallscount.ForeColor;
			lineslabel.ForeColor = linescount.ForeColor;
			sectorslabel.ForeColor = sectorscount.ForeColor;
			thingslabel.ForeColor = thingscount.ForeColor;
		}
	}
}
