#region ================== Namespaces

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Controls
{
	public partial class WallPropertiesControl : UserControl
	{
		#region ================== Constants

		private const int VALUE_MISMATCH = int.MinValue;

		#endregion
		
		#region ================== Constructor

		public WallPropertiesControl()
		{
			InitializeComponent();

			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime)
			{
				// Fill flags lists
				foreach(KeyValuePair<string, string> lf in General.Map.Config.WallFlags)
					flags.Add(lf.Value, lf.Key);

				// Initialize image selectors
				tex.Initialize();
				maskedtex.Initialize();
			}
		}

		#endregion

		#region ================== Methods

		public void Setup(List<Sidedef> walls)
		{
			// Use the first wall as a reference
			BuildWall first = new BuildWall(walls[0]);
			Dictionary<string, int> intflags = new Dictionary<string, int>();
			foreach(var group in General.Map.Config.WallFlags)
				intflags[group.Key] = (first.Flags.ContainsKey(group.Key) && first.Flags[group.Key] ? 1 : 0);

			int fw = ((walls[0] == walls[0].Sector.FirstWall) ? 1 : 0);

			// Go for all walls to compare properties
			foreach(Sidedef w in walls)
			{
				// Flags
				foreach(string flagname in General.Map.Config.WallFlags.Keys)
				{
					int flag = (w.IsFlagSet(flagname) ? 1 : 0);
					if(flag != intflags[flagname]) intflags[flagname] = VALUE_MISMATCH;
				}

				// First wall
				if(fw != ((w == w.Sector.FirstWall) ? 1 : 0)) fw = VALUE_MISMATCH;

				// Properties
				if(first.TileIndex != w.TileIndex) first.TileIndex = VALUE_MISMATCH;
				if(first.MaskedTileIndex != w.MaskedTileIndex) first.MaskedTileIndex = VALUE_MISMATCH;
				if(first.OffsetX != w.OffsetX) first.OffsetX = VALUE_MISMATCH;
				if(first.OffsetY != w.OffsetY) first.OffsetY = VALUE_MISMATCH;
				if(first.RepeatX != w.RepeatX) first.RepeatX = VALUE_MISMATCH;
				if(first.RepeatY != w.RepeatY) first.RepeatY = VALUE_MISMATCH;
				if(first.Shade != w.Shade) first.Shade = VALUE_MISMATCH;
				if(first.PaletteIndex != w.PaletteIndex) first.PaletteIndex = VALUE_MISMATCH;

				// Identification
				if(first.HiTag != w.HiTag) first.HiTag = VALUE_MISMATCH;
				if(first.LoTag != w.LoTag) first.LoTag = VALUE_MISMATCH;
				if(first.Extra != w.Extra) first.Extra = VALUE_MISMATCH;
			}

			// Update interface
			this.SuspendLayout();

			// Flags
			foreach(CheckBox c in flags.Checkboxes)
			{
				switch(intflags[c.Tag.ToString()])
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

			// First wall
			switch(fw)
			{
				case 1:
					firstwall.Checked = true;
					break;

				case VALUE_MISMATCH:
					firstwall.ThreeState = true;
					firstwall.CheckState = CheckState.Indeterminate;
					break;
			}

			// Properties
			tex.TextureName = (first.TileIndex != VALUE_MISMATCH ? first.TileIndex.ToString() : "");
			maskedtex.TextureName = (first.MaskedTileIndex != VALUE_MISMATCH ? first.MaskedTileIndex.ToString() : "");
			if(first.OffsetX != VALUE_MISMATCH) offsetx.Text = first.OffsetX.ToString();
			if(first.OffsetY != VALUE_MISMATCH) offsety.Text = first.OffsetY.ToString();
			if(first.RepeatX != VALUE_MISMATCH) repeatx.Text = first.RepeatX.ToString();
			if(first.RepeatY != VALUE_MISMATCH) repeaty.Text = first.RepeatY.ToString();
			if(first.Shade != VALUE_MISMATCH) shade.Text = first.Shade.ToString();
			if(first.PaletteIndex != VALUE_MISMATCH) palette.Text = first.PaletteIndex.ToString();

			// Identification
			//TODO: handlers?
			if(first.HiTag != VALUE_MISMATCH) hitag.Text = first.HiTag.ToString();
			if(first.LoTag != VALUE_MISMATCH) lotag.Text = first.LoTag.ToString();
			if(first.Extra != VALUE_MISMATCH) extra.Text = first.Extra.ToString();

			this.ResumeLayout();
		}

		public void ApplyTo(IEnumerable<Sidedef> walls)
		{
			// Collect flags...
			Dictionary<string, CheckState> flagsstate = new Dictionary<string, CheckState>();
			foreach(CheckBox c in flags.Checkboxes) flagsstate[c.Tag.ToString()] = c.CheckState;

			// Apply to all walls...
			foreach(Sidedef w in walls)
			{
				// Flags
				foreach(KeyValuePair<string, CheckState> group in flagsstate)
				{
					switch(group.Value)
					{
						case CheckState.Checked: w.SetFlag(group.Key, true); break;
						case CheckState.Unchecked: w.SetFlag(group.Key, false); break;
					}
				}

				// First wall
				switch(firstwall.CheckState)
				{
					case CheckState.Checked:
						w.Sector.FirstWall = w;
						break;

					case CheckState.Unchecked:
						if(w.Sector.FirstWall == w) 
							w.Sector.FirstWall = General.GetFirst(w.Sector.Sidedefs);
						break;
				}

				// Properties
				w.TileIndex = General.Clamp(tex.GetResult(w.TileIndex), General.Map.FormatInterface.MinTileIndex, General.Map.FormatInterface.MaxTileIndex);
				w.MaskedTileIndex = General.Clamp(maskedtex.GetResult(w.MaskedTileIndex), General.Map.FormatInterface.MinTileIndex, General.Map.FormatInterface.MaxTileIndex);
				w.OffsetX = General.Wrap(offsetx.GetResult(w.OffsetX), General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
				w.OffsetY = General.Wrap(offsety.GetResult(w.OffsetY), General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
				w.RepeatX = General.Clamp(repeatx.GetResult(w.RepeatX), General.Map.FormatInterface.MinImageRepeat, General.Map.FormatInterface.MaxImageRepeat);
				w.RepeatY = General.Clamp(repeaty.GetResult(w.RepeatY), General.Map.FormatInterface.MinImageRepeat, General.Map.FormatInterface.MaxImageRepeat);
				w.Shade = General.Clamp(shade.GetResult(w.Shade), General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
				w.PaletteIndex = palette.GetResult(w.PaletteIndex);

				// Identification
				w.HiTag = hitag.GetResult(w.HiTag);
				w.LoTag = lotag.GetResult(w.LoTag);
				w.Extra = extra.GetResult(w.Extra);
			}
		}

		public bool IsValid()
		{
			// Verify properties
			if(!VerifyValue(hitag.GetResult(0), General.Map.FormatInterface.MinTag, General.Map.FormatInterface.MaxTag, "Wall hitag")) return false;
			if(!VerifyValue(lotag.GetResult(0), General.Map.FormatInterface.MinTag, General.Map.FormatInterface.MaxTag, "Wall lotag")) return false;
			if(!VerifyValue(extra.GetResult(0), General.Map.FormatInterface.MinExtra, General.Map.FormatInterface.MaxExtra, "Wall extra")) return false;
			if(!VerifyValue(palette.GetResult(0), 0, 255, "Wall palette")) return false;

			return true;
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
	}
}
