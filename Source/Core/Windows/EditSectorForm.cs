
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
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Windows
{
	internal partial class EditSectorForm : DelayedForm
	{
		#region ================== Constants

		private const int VALUE_MISMATCH = int.MinValue;

		#endregion
		
		#region ================== Variables

		private ICollection<Sector> sectors;

		#endregion

		#region ================== Constructor

		// Constructor
		public EditSectorForm()
		{
			// Initialize
			InitializeComponent();

			// Fill flags lists
			foreach(KeyValuePair<string, string> lf in General.Map.Config.SectorFlags)
			{
				floorflags.Add(lf.Value, lf.Key);
				ceilflags.Add(lf.Value, lf.Key);
			}

			// Initialize image selectors
			floortex.Initialize();
			ceiltex.Initialize();
		}

		#endregion

		#region ================== Methods

		// This sets up the form to edit the given sectors
		public void Setup(ICollection<Sector> sectors)
		{
			// Keep this list
			this.sectors = sectors;
			if(sectors.Count > 1) this.Text = "Edit sectors (" + sectors.Count + ")";

			// Use the first sector as a reference
			BuildSector first = new BuildSector(General.GetFirst(sectors));
			Dictionary<string, int> intflagsceiling = new Dictionary<string, int>();
			Dictionary<string, int> intflagsfloor = new Dictionary<string, int>();
			foreach(var group in General.Map.Config.SectorFlags)
			{
				intflagsceiling[group.Key] = (first.CeilingFlags.ContainsKey(group.Key) && first.CeilingFlags[group.Key] ? 1 : 0);
				intflagsfloor[group.Key] = (first.FloorFlags.ContainsKey(group.Key) && first.FloorFlags[group.Key] ? 1 : 0);
			}

			// Go for all sectors to compare properties
			foreach(Sector s in sectors)
			{
				// Floor/ceiling
				if(first.FloorHeight != s.FloorHeight) first.FloorHeight = VALUE_MISMATCH;
				if(first.CeilingHeight != s.CeilingHeight) first.CeilingHeight = VALUE_MISMATCH;
				if(first.Visibility != s.Visibility) first.Visibility = VALUE_MISMATCH;

				// Identification
				if(first.HiTag != s.HiTag) first.HiTag = VALUE_MISMATCH;
				if(first.LoTag != s.LoTag) first.LoTag = VALUE_MISMATCH;
				if(first.Extra != s.Extra) first.Extra = VALUE_MISMATCH;

				// Ceiling
				foreach(string flagname in General.Map.Config.SectorFlags.Keys)
				{
					int flag = (s.IsFlagSet(flagname, false) ? 1 : 0);
					if(flag != intflagsceiling[flagname]) intflagsceiling[flagname] = VALUE_MISMATCH;
				}

				if(first.CeilingTileIndex != s.CeilingTileIndex) first.CeilingTileIndex = VALUE_MISMATCH;
				if(first.CeilingOffsetX != s.CeilingOffsetX) first.CeilingOffsetX = VALUE_MISMATCH;
				if(first.CeilingOffsetY != s.CeilingOffsetY) first.CeilingOffsetY = VALUE_MISMATCH;
				if(first.CeilingShade != s.CeilingShade) first.CeilingShade = VALUE_MISMATCH;
				if(first.CeilingPaletteIndex != s.CeilingPaletteIndex) first.CeilingPaletteIndex = VALUE_MISMATCH;
				if(first.CeilingSlope != s.CeilingSlope) first.CeilingSlope = VALUE_MISMATCH;

				// Floor
				foreach(string flagname in General.Map.Config.SectorFlags.Keys)
				{
					int flag = (s.IsFlagSet(flagname, true) ? 1 : 0);
					if(flag != intflagsfloor[flagname]) intflagsfloor[flagname] = VALUE_MISMATCH;
				}

				if(first.FloorTileIndex != s.FloorTileIndex) first.FloorTileIndex = VALUE_MISMATCH;
				if(first.FloorOffsetX != s.FloorOffsetX) first.FloorOffsetX = VALUE_MISMATCH;
				if(first.FloorOffsetY != s.FloorOffsetY) first.FloorOffsetY = VALUE_MISMATCH;
				if(first.FloorShade != s.FloorShade) first.FloorShade = VALUE_MISMATCH;
				if(first.FloorPaletteIndex != s.FloorPaletteIndex) first.FloorPaletteIndex = VALUE_MISMATCH;
				if(first.FloorSlope != s.FloorSlope) first.FloorSlope = VALUE_MISMATCH;
			}

			// Update interface
			this.SuspendLayout();

			// Floor/ceiling
			if(first.FloorHeight != VALUE_MISMATCH) floorheight.Text = first.FloorHeight.ToString();
			if(first.CeilingHeight != VALUE_MISMATCH) ceilheight.Text = first.CeilingHeight.ToString();
			if(first.Visibility != VALUE_MISMATCH) visibility.Text = first.Visibility.ToString();

			// Identification
			//TODO: handlers
			if(first.HiTag != VALUE_MISMATCH) hitag.Text = first.HiTag.ToString();
			if(first.LoTag != VALUE_MISMATCH) lotag.Text = first.LoTag.ToString();
			if(first.Extra != VALUE_MISMATCH) extra.Text = first.Extra.ToString();

			// Ceiling
			foreach(CheckBox c in ceilflags.Checkboxes)
			{
				switch(intflagsceiling[c.Tag.ToString()])
				{
					case 1:
						c.Checked = true;
						break;

					case VALUE_MISMATCH:
						c.ThreeState = true;
						c.CheckState = CheckState.Indeterminate;
						break;
				}
			}

			ceiltex.TextureName = (first.CeilingTileIndex != VALUE_MISMATCH ? first.CeilingTileIndex.ToString() : "");
			if(first.CeilingOffsetX != VALUE_MISMATCH) ceiloffsetx.Text = first.CeilingOffsetX.ToString();
			if(first.CeilingOffsetY != VALUE_MISMATCH) ceiloffsety.Text = first.CeilingOffsetY.ToString();
			if(first.CeilingShade != VALUE_MISMATCH) ceilshade.Text = first.CeilingShade.ToString();
			if(first.CeilingPaletteIndex != VALUE_MISMATCH) ceilpalette.Text = first.CeilingPaletteIndex.ToString();
			if(first.CeilingSlope != VALUE_MISMATCH) ceilslope.Text = ((float)Math.Round(General.Wrap(Angle2D.RadToDeg(first.CeilingSlope) - 90, -90, 90), 1)).ToString();

			// Floor
			foreach(CheckBox c in floorflags.Checkboxes)
			{
				switch(intflagsfloor[c.Tag.ToString()])
				{
					case 1:
						c.Checked = true;
						break;

					case VALUE_MISMATCH:
						c.ThreeState = true;
						c.CheckState = CheckState.Indeterminate;
						break;
				}
			}

			floortex.TextureName = (first.FloorTileIndex != VALUE_MISMATCH ? first.FloorTileIndex.ToString() : "");
			if(first.FloorOffsetX != VALUE_MISMATCH) flooroffsetx.Text = first.FloorOffsetX.ToString();
			if(first.FloorOffsetY != VALUE_MISMATCH) flooroffsety.Text = first.FloorOffsetY.ToString();
			if(first.FloorShade != VALUE_MISMATCH) floorshade.Text = first.FloorShade.ToString();
			if(first.FloorPaletteIndex != VALUE_MISMATCH) floorpalette.Text = first.FloorPaletteIndex.ToString();
			if(first.FloorSlope != VALUE_MISMATCH) floorslope.Text = ((float)Math.Round(General.Wrap(Angle2D.RadToDeg(first.FloorSlope) - 90, -90, 90), 1)).ToString();

			this.ResumeLayout();

			// Show sector height
			UpdateSectorHeight();
		}

		// This updates the sector height field
		private void UpdateSectorHeight()
		{
			bool showheight = true;
			int delta = 0;
			Sector first = null;
			
			// Check all selected sectors
			foreach(Sector s in sectors)
			{
				if(first == null)
				{
					// First sector in list
					delta = s.CeilingHeight - s.FloorHeight;
					first = s;
				}
				else if(delta != (s.CeilingHeight - s.FloorHeight))
				{
					// We can't show heights because the delta heights for the sectors is different
					showheight = false;
					break;
				}
			}

			if(showheight)
			{
				int fh = floorheight.GetResult(first.FloorHeight);
				int ch = ceilheight.GetResult(first.CeilingHeight);
				int height = ch - fh;
				sectorheight.Text = height.ToString();
				sectorheight.Visible = true;
				sectorheightlabel.Visible = true;
			}
			else
			{
				sectorheight.Visible = false;
				sectorheightlabel.Visible = false;
			}
		}

		private static bool VerifyValue(int value, int min, int max, string name)
		{
			if(value < min || value > max)
			{
				General.ShowWarningMessage(name + " must be between " + min + " and " + max + ".", MessageBoxButtons.OK);
				return false;
			}

			return true;
		}

		#endregion

		#region ================== Events

		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			// Verify properties
			if(!VerifyValue(hitag.GetResult(0), General.Map.FormatInterface.MinTag, General.Map.FormatInterface.MaxTag, "Sector hitag")) return;
			if(!VerifyValue(lotag.GetResult(0), General.Map.FormatInterface.MinTag, General.Map.FormatInterface.MaxTag, "Sector lotag")) return;
			if(!VerifyValue(extra.GetResult(0), General.Map.FormatInterface.MinExtra, General.Map.FormatInterface.MaxExtra, "Sector extra")) return;
			if(!VerifyValue(ceilpalette.GetResult(0), 0, 255, "Ceiling palette")) return;
			if(!VerifyValue(floorpalette.GetResult(0), 0, 255, "Floor palette")) return;

			// Make undo
			General.Map.UndoRedo.CreateUndo("Edit " + (sectors.Count > 1 ? sectors.Count + " sectors" : "sector"));

			// Collect flags...
			Dictionary<string, CheckState> floorflagsstate = new Dictionary<string, CheckState>();
			foreach(CheckBox c in floorflags.Checkboxes) floorflagsstate[c.Tag.ToString()] = c.CheckState;

			Dictionary<string, CheckState> ceilflagsstate = new Dictionary<string, CheckState>();
			foreach(CheckBox c in ceilflags.Checkboxes) ceilflagsstate[c.Tag.ToString()] = c.CheckState;

			// Update slope flags?
			float ceilslopedeg = ceilslope.GetResultFloat(VALUE_MISMATCH);
			if(ceilslopedeg != VALUE_MISMATCH)
			{
				ceilflagsstate[General.Map.FormatInterface.SectorFlags.Sloped] = (ceilslopedeg != 0 ? CheckState.Checked : CheckState.Unchecked);
			}

			float floorslopedeg = floorslope.GetResultFloat(VALUE_MISMATCH);
			if(floorslopedeg != VALUE_MISMATCH)
			{
				floorflagsstate[General.Map.FormatInterface.SectorFlags.Sloped] = (floorslopedeg != 0 ? CheckState.Checked : CheckState.Unchecked);
			}

			// Go for all sectors
			foreach(Sector s in sectors)
			{
				// Floor/ceiling
				s.FloorHeight = floorheight.GetResult(s.FloorHeight);
				s.CeilingHeight = ceilheight.GetResult(s.CeilingHeight);
				s.Visibility = visibility.GetResult(s.Visibility);

				// Ceiling
				foreach(KeyValuePair<string, CheckState> group in ceilflagsstate)
				{
					switch(group.Value)
					{
						case CheckState.Checked: s.SetFlag(group.Key, true, false); break;
						case CheckState.Unchecked: s.SetFlag(group.Key, false, false); break;
					}
				}

				s.CeilingTileIndex = General.Clamp(ceiltex.GetResult(s.CeilingTileIndex), General.Map.FormatInterface.MinTileIndex, General.Map.FormatInterface.MaxTileIndex);
				s.CeilingOffsetX = General.Wrap(ceiloffsetx.GetResult(s.CeilingOffsetX), General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
				s.CeilingOffsetY = General.Wrap(ceiloffsety.GetResult(s.CeilingOffsetY), General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
				s.CeilingShade = General.Clamp(ceilshade.GetResult(s.CeilingShade), General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
				s.CeilingPaletteIndex = ceilpalette.GetResult(s.CeilingPaletteIndex);
				s.CeilingSlope = General.Clamp(Angle2D.DegToRad(ceilslope.GetResultFloat(Angle2D.RadToDeg(s.CeilingSlope) - 90) + 90), General.Map.FormatInterface.MinSlope, General.Map.FormatInterface.MaxSlope);

				// Floor
				foreach(KeyValuePair<string, CheckState> group in floorflagsstate)
				{
					switch(group.Value)
					{
						case CheckState.Checked: s.SetFlag(group.Key, true, true); break;
						case CheckState.Unchecked: s.SetFlag(group.Key, false, true); break;
					}
				}

				s.FloorTileIndex = General.Clamp(floortex.GetResult(s.FloorTileIndex), General.Map.FormatInterface.MinTileIndex, General.Map.FormatInterface.MaxTileIndex);
				s.FloorOffsetX = General.Wrap(flooroffsetx.GetResult(s.FloorOffsetX), General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
				s.FloorOffsetY = General.Wrap(flooroffsety.GetResult(s.FloorOffsetY), General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
				s.FloorShade = General.Clamp(floorshade.GetResult(s.FloorShade), General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
				s.FloorPaletteIndex = floorpalette.GetResult(s.FloorPaletteIndex);
				s.FloorSlope = General.Clamp(Angle2D.DegToRad(floorslope.GetResultFloat(Angle2D.RadToDeg(s.FloorSlope) - 90) + 90), General.Map.FormatInterface.MinSlope, General.Map.FormatInterface.MaxSlope);

				// Identification
				s.HiTag = hitag.GetResult(s.HiTag);
				s.LoTag = lotag.GetResult(s.LoTag);
				s.Extra = extra.GetResult(s.Extra);
			}
			
			// Update the used textures
			General.Map.Data.UpdateUsedImages();
			
			// Done
			General.Map.IsChanged = true;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			// Be gone
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// Ceiling height changes
		private void ceilingheight_TextChanged(object sender, EventArgs e)
		{
			UpdateSectorHeight();
		}

		// Floor height changes
		private void floorheight_TextChanged(object sender, EventArgs e)
		{
			UpdateSectorHeight();
		}

		// Help
		private void SectorEditForm_HelpRequested(object sender, HelpEventArgs e)
		{
			General.ShowHelp("w_sectoredit.html");
			e.Handled = true;
		}

		#endregion
	}
}