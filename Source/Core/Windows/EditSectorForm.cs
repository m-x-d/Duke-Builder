
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
			if(first.CeilingSlope != VALUE_MISMATCH) ceilslope.Text = ((float)Math.Round(General.Wrap(Angle2D.RadToDeg(first.CeilingSlope) - 90, -90, 90), 3)).ToString();

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
			if(first.FloorSlope != VALUE_MISMATCH) floorslope.Text = ((float)Math.Round(General.Wrap(Angle2D.RadToDeg(first.FloorSlope) - 90, -90, 90), 3)).ToString();

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


			// Collect results...
			BuildSector props = new BuildSector();
			
			// Floor/ceiling
			props.FloorHeight = floorheight.GetResult(VALUE_MISMATCH);
			props.CeilingHeight = ceilheight.GetResult(VALUE_MISMATCH);
			props.Visibility = visibility.GetResult(VALUE_MISMATCH);

			// Clamp values
			if(props.Visibility != VALUE_MISMATCH)
				props.Visibility = General.Clamp(props.Visibility, General.Map.FormatInterface.MinVisibility, General.Map.FormatInterface.MaxVisibility);

			// Identification
			props.HiTag = hitag.GetResult(VALUE_MISMATCH);
			props.LoTag = lotag.GetResult(VALUE_MISMATCH);
			props.Extra = extra.GetResult(VALUE_MISMATCH);

			// Ceiling
			props.CeilingTileIndex = ceiltex.GetResult(VALUE_MISMATCH);
			props.CeilingOffsetX = ceiloffsetx.GetResult(VALUE_MISMATCH);
			props.CeilingOffsetY = ceiloffsety.GetResult(VALUE_MISMATCH);
			props.CeilingShade = ceilshade.GetResult(VALUE_MISMATCH);
			props.CeilingPaletteIndex = ceilpalette.GetResult(VALUE_MISMATCH);
			float ceilslopedeg = ceilslope.GetResultFloat(VALUE_MISMATCH);
			props.CeilingSlope = (ceilslopedeg != VALUE_MISMATCH ? Angle2D.DegToRad(ceilslopedeg + 90) : VALUE_MISMATCH);

			// Update slope flag?
			if(ceilslopedeg != VALUE_MISMATCH && ceilslopedeg != 0)
				ceilflagsstate[General.Map.FormatInterface.SectorSlopeFlag] = CheckState.Checked;
			else if(ceilslopedeg == 0)
				ceilflagsstate[General.Map.FormatInterface.SectorSlopeFlag] = CheckState.Unchecked;

			// Clamp values
			if(props.CeilingTileIndex != VALUE_MISMATCH) 
				props.CeilingTileIndex = General.Clamp(props.CeilingTileIndex, General.Map.FormatInterface.MinTileIndex, General.Map.FormatInterface.MaxTileIndex);
			if(props.CeilingOffsetX != VALUE_MISMATCH)
				props.CeilingOffsetX = General.Wrap(props.CeilingOffsetX, General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
			if(props.CeilingOffsetY != VALUE_MISMATCH)
				props.CeilingOffsetY = General.Wrap(props.CeilingOffsetY, General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
			if(props.CeilingShade != VALUE_MISMATCH)
				props.CeilingShade = General.Clamp(props.CeilingShade, General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
			if(props.CeilingSlope != VALUE_MISMATCH)
				props.CeilingSlope = General.Clamp(props.CeilingSlope, General.Map.FormatInterface.MinSlope, General.Map.FormatInterface.MaxSlope);

			// Floor
			props.FloorTileIndex = floortex.GetResult(VALUE_MISMATCH);
			props.FloorOffsetX = flooroffsetx.GetResult(VALUE_MISMATCH);
			props.FloorOffsetY = flooroffsety.GetResult(VALUE_MISMATCH);
			props.FloorShade = floorshade.GetResult(VALUE_MISMATCH);
			props.FloorPaletteIndex = floorpalette.GetResult(VALUE_MISMATCH);
			float floorslopedeg = floorslope.GetResultFloat(VALUE_MISMATCH);
			props.FloorSlope = (floorslopedeg != VALUE_MISMATCH ? Angle2D.DegToRad(floorslopedeg + 90) : VALUE_MISMATCH);

			// Update slope flag?
			if(floorslopedeg != VALUE_MISMATCH && floorslopedeg != 0) 
				floorflagsstate[General.Map.FormatInterface.SectorSlopeFlag] = CheckState.Checked;
			else if(floorslopedeg == 0)
				floorflagsstate[General.Map.FormatInterface.SectorSlopeFlag] = CheckState.Unchecked;

			// Clamp values
			if(props.FloorTileIndex != VALUE_MISMATCH)
				props.FloorTileIndex = General.Clamp(props.FloorTileIndex, General.Map.FormatInterface.MinTileIndex, General.Map.FormatInterface.MaxTileIndex);
			if(props.FloorOffsetX != VALUE_MISMATCH)
				props.FloorOffsetX = General.Wrap(props.FloorOffsetX, General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
			if(props.FloorOffsetY != VALUE_MISMATCH)
				props.FloorOffsetY = General.Wrap(props.FloorOffsetY, General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
			if(props.FloorShade != VALUE_MISMATCH)
				props.FloorShade = General.Clamp(props.FloorShade, General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
			if(props.FloorSlope != VALUE_MISMATCH)
				props.FloorSlope = General.Clamp(props.FloorSlope, General.Map.FormatInterface.MinSlope, General.Map.FormatInterface.MaxSlope);

			// Go for all sectors
			foreach(Sector s in sectors)
			{
				// Floor/ceiling
				if(props.FloorHeight != VALUE_MISMATCH) s.FloorHeight = props.FloorHeight;
				if(props.CeilingHeight != VALUE_MISMATCH) s.CeilingHeight = props.CeilingHeight;
				if(props.Visibility != VALUE_MISMATCH) s.Visibility = props.Visibility;

				// Identification
				if(props.HiTag != VALUE_MISMATCH) s.HiTag = props.HiTag;
				if(props.LoTag != VALUE_MISMATCH) s.LoTag = props.LoTag;
				if(props.Extra != VALUE_MISMATCH) s.Extra = props.Extra;

				// Ceiling
				foreach(KeyValuePair<string, CheckState> group in ceilflagsstate)
				{
					switch(group.Value)
					{
						case CheckState.Checked: s.SetFlag(group.Key, true, false); break;
						case CheckState.Unchecked: s.SetFlag(group.Key, false, false); break;
					}
				}

				if(props.CeilingTileIndex != VALUE_MISMATCH) s.CeilingTileIndex = props.CeilingTileIndex;
				if(props.CeilingOffsetX != VALUE_MISMATCH) s.CeilingOffsetX = props.CeilingOffsetX;
				if(props.CeilingOffsetY != VALUE_MISMATCH) s.CeilingOffsetY = props.CeilingOffsetY;
				if(props.CeilingShade != VALUE_MISMATCH) s.CeilingShade = props.CeilingShade;
				if(props.CeilingPaletteIndex != VALUE_MISMATCH) s.CeilingPaletteIndex = props.CeilingPaletteIndex;
				if(props.CeilingSlope != VALUE_MISMATCH) s.CeilingSlope = props.CeilingSlope;

				// Floor
				foreach(KeyValuePair<string, CheckState> group in floorflagsstate)
				{
					switch(group.Value)
					{
						case CheckState.Checked: s.SetFlag(group.Key, true, true); break;
						case CheckState.Unchecked: s.SetFlag(group.Key, false, true); break;
					}
				}

				if(props.FloorTileIndex != VALUE_MISMATCH) s.FloorTileIndex = props.FloorTileIndex;
				if(props.FloorOffsetX != VALUE_MISMATCH) s.FloorOffsetX = props.FloorOffsetX;
				if(props.FloorOffsetY != VALUE_MISMATCH) s.FloorOffsetY = props.FloorOffsetY;
				if(props.FloorShade != VALUE_MISMATCH) s.FloorShade = props.FloorShade;
				if(props.FloorPaletteIndex != VALUE_MISMATCH) s.FloorPaletteIndex = props.FloorPaletteIndex;
				if(props.FloorSlope != VALUE_MISMATCH) s.FloorSlope = props.FloorSlope;
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
		private void SectorEditForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			General.ShowHelp("w_sectoredit.html");
			hlpevent.Handled = true;
		}

		#endregion
	}
}