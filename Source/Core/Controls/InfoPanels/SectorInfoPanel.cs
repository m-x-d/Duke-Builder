
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
using System.Collections.Generic;
using System.Windows.Forms;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Controls
{
	internal partial class SectorInfoPanel : UserControl
	{
		#region ================== Constructor

		public SectorInfoPanel()
		{
			// Initialize
			InitializeComponent();
		}

		#endregion

		#region ================== Methods

		// This shows the info
		public void ShowInfo(Sector s)
		{
			// Lookup effect description in config
			if(s.LoTag != 0 && General.Map.Config.SectorEffects.ContainsKey(s.LoTag))
			{
				lotag.Text = General.Map.Config.SectorEffects[s.LoTag].ToString();
				lotaglabel.Text = "Effect:";
			}
			else
			{
				lotag.Text = s.LoTag.ToString();
				lotaglabel.Text = "LoTag:";
			}

			// Sector info
			sectorinfo.Text = " Sector " + s.Index + " (" + (s.Sidedefs != null ? s.Sidedefs.Count.ToString() : "no") + " walls)";
			firstwall.Text = (s.FirstWall != null ? s.FirstWall.Index.ToString() : "--");
			hitag.Text = s.HiTag.ToString();
			extra.Text = s.Extra.ToString();
			ceiling.Text = s.CeilingHeight.ToString();
			floor.Text = s.FloorHeight.ToString();
			height.Text = (s.CeilingHeight - s.FloorHeight).ToString();
			visibility.Text = s.Visibility.ToString();
			
			// Floor info
			General.DisplayZoomedImage(floortex, General.Map.Data.GetImageData(s.FloorTileIndex).GetPreview());
			floorname.Text = s.FloorTileIndex.ToString();
			flooroffsetx.Text = s.FloorOffsetX.ToString();
			flooroffsety.Text = s.FloorOffsetY.ToString();
			floorshade.Text = s.FloorShade.ToString();
			floorpalette.Text = s.FloorPaletteIndex.ToString();
			floorslope.Text = Math.Round(General.Wrap(Angle2D.RadToDeg(s.FloorSlope) - 90, -90, 90), 1) + "\u00B0";

			// Ceiling info
			General.DisplayZoomedImage(ceiltex, General.Map.Data.GetImageData(s.CeilingTileIndex).GetPreview());
			ceilname.Text = s.CeilingTileIndex.ToString();
			ceiloffsetx.Text = s.CeilingOffsetX.ToString();
			ceiloffsety.Text = s.CeilingOffsetY.ToString();
			ceilshade.Text = s.CeilingShade.ToString();
			ceilpalette.Text = s.CeilingPaletteIndex.ToString();
			ceilslope.Text = Math.Round(General.Wrap(Angle2D.RadToDeg(s.CeilingSlope) - 90, -90, 90), 1) + "\u00B0";

			// Disable identification labels when showing default values
			hitag.Enabled = s.HiTag > 0;
			hitaglabel.Enabled = s.HiTag > 0;
			lotag.Enabled = s.LoTag > 0;
			lotaglabel.Enabled = s.LoTag > 0;
			extra.Enabled = s.Extra != -1;
			extralabel.Enabled = s.Extra != -1;

			flooroffsetx.Enabled = s.FloorOffsetX != 0;
			flooroffsetxlabel.Enabled = s.FloorOffsetX != 0;
			flooroffsety.Enabled = s.FloorOffsetY != 0;
			flooroffsetylabel.Enabled = s.FloorOffsetY != 0;

			ceiloffsetx.Enabled = s.CeilingOffsetX != 0;
			ceiloffsetxlabel.Enabled = s.CeilingOffsetX != 0;
			ceiloffsety.Enabled = s.CeilingOffsetY != 0;
			ceiloffsetylabel.Enabled = s.CeilingOffsetY != 0;

			floorpalette.Enabled = s.FloorPaletteIndex > 0;
			floorpalettelabel.Enabled = s.FloorPaletteIndex > 0;

			bool floorsloped = (s.FloorSloped && s.FloorSlope != 0);
			floorslope.Enabled = floorsloped;
			floorslopelabel.Enabled = floorsloped;

			ceilpalette.Enabled = s.CeilingPaletteIndex > 0;
			ceilpalettelabel.Enabled = s.CeilingPaletteIndex > 0;

			bool ceilsloped = (s.CeilingSloped && s.CeilingSlope != 0);
			ceilslope.Enabled = ceilsloped;
			ceilslopelabel.Enabled = ceilsloped;

			// Floor flags
			List<string> floorflagnames = new List<string>();
			foreach(KeyValuePair<string, string> group in General.Map.Config.SectorFlags)
				if(s.IsFlagSet(group.Key, true)) floorflagnames.Add(group.Value);
			floorflags.Setup(floorflagnames);

			// Ceiling flags
			List<string> ceilingflagnames = new List<string>();
			foreach(KeyValuePair<string, string> group in General.Map.Config.SectorFlags)
				if(s.IsFlagSet(group.Key, false)) ceilingflagnames.Add(group.Value);
			ceilingflags.Setup(ceilingflagnames);

			// Resize and reposition
			if(floorflagnames.Count > 0)
			{
				ceilinggroup.Left = floorflags.Right + floorflags.Margin.Right + ceilinggroup.Margin.Left;
			}
			else
			{
				ceilinggroup.Left = floorgroup.Right + floorgroup.Margin.Right + ceilinggroup.Margin.Left;
			}

			if(ceilingflagnames.Count > 0)
			{
				ceilingflags.Left = ceilinggroup.Right + ceilinggroup.Margin.Right + ceilingflags.Margin.Left;
				this.Width = ceilingflags.Right + ceilingflags.Margin.Right;
			}
			else
			{
				this.Width = ceilinggroup.Right + ceilinggroup.Margin.Right;
			}

			// Show the whole thing
			this.Show();
			this.Update();
		}

		#endregion

		#region ================== Event overrides

		// When visible changed
		protected override void OnVisibleChanged(EventArgs e)
		{
			// Hiding panels
			if(!this.Visible)
			{
				floortex.BackgroundImage = null;
				ceiltex.BackgroundImage = null;
			}

			// Call base
			base.OnVisibleChanged(e);
		}

		#endregion
	}
}
