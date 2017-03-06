
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
using mxd.DukeBuilder.Controls;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Geometry;

#endregion

namespace mxd.DukeBuilder.Windows
{
	public partial class EditSpriteForm : DelayedForm
	{
		#region ================== Constants

		private const int VALUE_MISMATCH = int.MinValue;

		#endregion

		#region ================== Variables

		private ICollection<Thing> sprites;
		private bool preventchanges;
		
		#endregion

		#region ================== Constructor

		// Constructor
		public EditSpriteForm()
		{
			// Initialize
			InitializeComponent();
			
			// Fill flags list
			foreach(var tf in General.Map.Config.SpriteFlags)
				flags.Add(tf.Value, tf.Key);
			
			// Setup types list
			spritetype.Setup();

			// Initialize image selectors
			tex.Initialize();
		}

		#endregion

		#region ================== Methods

		// This sets up the form to edit the given things
		public void Setup(ICollection<Thing> sprites)
		{
			// Keep this list
			this.sprites = sprites;
			if(sprites.Count > 1) this.Text = "Edit sprites (" + sprites.Count + ")";

			// Use the first sprite as a reference
			BuildSprite first = new BuildSprite(General.GetFirst(sprites));
			Dictionary<string, int> intflags = new Dictionary<string, int>();
			foreach(var group in General.Map.Config.SpriteFlags)
				intflags[group.Key] = (first.Flags.ContainsKey(group.Key) && first.Flags[group.Key] ? 1 : 0);

			// Go for all sprites to compare properties
			foreach(Thing s in sprites)
			{
				// Type
				if(first.TileIndex != s.TileIndex) first.TileIndex = VALUE_MISMATCH;

				// Flags
				foreach(string flagname in General.Map.Config.SpriteFlags.Keys)
				{
					int flag = (s.IsFlagSet(flagname) ? 1 : 0);
					if(flag != intflags[flagname]) intflags[flagname] = VALUE_MISMATCH;
				}

				// Properties
				if(first.OffsetX != s.OffsetX) first.OffsetX = VALUE_MISMATCH;
				if(first.OffsetY != s.OffsetY) first.OffsetY = VALUE_MISMATCH;
				if(first.RepeatX != s.RepeatX) first.RepeatX = VALUE_MISMATCH;
				if(first.RepeatY != s.RepeatY) first.RepeatY = VALUE_MISMATCH;

				if(first.Shade != s.Shade) first.Shade = VALUE_MISMATCH;
				if(first.PaletteIndex != s.PaletteIndex) first.PaletteIndex = VALUE_MISMATCH;
				if(first.ClipDistance != s.ClipDistance) first.ClipDistance = VALUE_MISMATCH;
				if(first.Owner != s.Owner) first.Owner = VALUE_MISMATCH;

				// Properties
				if(first.X != (int)s.Position.x) first.X = VALUE_MISMATCH;
				if(first.Y != (int)s.Position.y) first.Y = VALUE_MISMATCH;
				if(first.Z != (int)s.Position.z) first.Z = VALUE_MISMATCH;
				if(first.VelX != (int)s.Velocity.x) first.VelX = VALUE_MISMATCH;
				if(first.VelY != (int)s.Velocity.y) first.VelY = VALUE_MISMATCH;
				if(first.VelZ != (int)s.Velocity.z) first.VelZ = VALUE_MISMATCH;

				// Angle
				if(first.Angle != s.AngleDeg) first.Angle = VALUE_MISMATCH;

				// Identification
				if(first.HiTag != s.HiTag) first.HiTag = VALUE_MISMATCH;
				if(first.LoTag != s.LoTag) first.LoTag = VALUE_MISMATCH;
				if(first.Extra != s.Extra) first.Extra = VALUE_MISMATCH;
			}

			// Update interface
			preventchanges = true;
			this.SuspendLayout();

			// Set type
			if(first.TileIndex != VALUE_MISMATCH)
			{
				spritetype.SelectType(first.TileIndex);
				tex.TextureName = first.TileIndex.ToString();
			}
			else
			{
				tex.TextureName = "";
			}

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

			// Properties
			if(first.OffsetX != VALUE_MISMATCH) offsetx.Text = first.OffsetX.ToString();
			if(first.OffsetY != VALUE_MISMATCH) offsety.Text = first.OffsetY.ToString();
			if(first.RepeatX != VALUE_MISMATCH) repeatx.Text = first.RepeatX.ToString();
			if(first.RepeatY != VALUE_MISMATCH) repeaty.Text = first.RepeatY.ToString();

			if(first.Shade != VALUE_MISMATCH) shade.Text = first.Shade.ToString();
			if(first.PaletteIndex != VALUE_MISMATCH) palette.Text = first.PaletteIndex.ToString();
			if(first.ClipDistance != VALUE_MISMATCH) clipdistance.Text = first.ClipDistance.ToString();
			if(first.Owner != VALUE_MISMATCH) owner.Text = first.Owner.ToString();

			// Position
			if(first.X != VALUE_MISMATCH) posx.Text = first.X.ToString();
			if(first.Y != VALUE_MISMATCH) posy.Text = (-first.Y).ToString();
			if(first.Z != VALUE_MISMATCH) posz.Text = first.Z.ToString();
			if(first.VelX != VALUE_MISMATCH) velx.Text = first.VelX.ToString();
			if(first.VelY != VALUE_MISMATCH) vely.Text = first.VelY.ToString();
			if(first.VelZ != VALUE_MISMATCH) velz.Text = first.VelZ.ToString();

			// Angle
			if(first.Angle != VALUE_MISMATCH)
			{
				angle.Text = first.Angle.ToString();
				anglecontrol.Value = first.Angle;
			}
			else
			{
				anglecontrol.Value = AngleControl.NO_ANGLE;
			}

			// Identification
			//TODO: handlers
			if(first.HiTag != VALUE_MISMATCH) hitag.Text = first.HiTag.ToString();
			if(first.LoTag != VALUE_MISMATCH) lotag.Text = first.LoTag.ToString();
			if(first.Extra != VALUE_MISMATCH) extra.Text = first.Extra.ToString();

			this.ResumeLayout();
			preventchanges = false;
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

		// Selected type changes
		private void spritetype_OnTypeChanged(int tileindex)
		{
			if(preventchanges) return;
			preventchanges = true;
			tex.TextureName = tileindex.ToString(); // Update preview image
			preventchanges = false;
		}

		private void spritetype_OnTypeDoubleClicked(int tileindex)
		{
			spritetype_OnTypeChanged(tileindex);
			apply_Click(this, EventArgs.Empty);
		}

		private void tex_OnValueChanged(object sender, EventArgs e)
		{
			if(preventchanges) return;
			preventchanges = true;
			spritetype.SelectType(tex.GetResult(-1));
			preventchanges = false;
		}

		// Angle text changes
		private void angle_TextChanged(object sender, EventArgs e)
		{
			if(preventchanges) return;
			preventchanges = true;
			anglecontrol.Value = angle.GetResult(int.MinValue);
			preventchanges = false;
		}

		// Angle control clicked
		private void anglecontrol_ValueChanged(object sender, EventArgs e)
		{
			if(preventchanges) return;
			preventchanges = true;
			angle.Text = anglecontrol.Value.ToString();
			preventchanges = false;
		}

		// Apply clicked
		private void apply_Click(object sender, EventArgs e)
		{
			// Verify properties
			if(!VerifyValue(hitag.GetResult(0), General.Map.FormatInterface.MinTag, General.Map.FormatInterface.MaxTag, "Sprite hitag")) return;
			if(!VerifyValue(lotag.GetResult(0), General.Map.FormatInterface.MinTag, General.Map.FormatInterface.MaxTag, "Sprite lotag")) return;
			if(!VerifyValue(extra.GetResult(0), General.Map.FormatInterface.MinExtra, General.Map.FormatInterface.MaxExtra, "Sprite extra")) return;
			if(!VerifyValue(palette.GetResult(0), 0, 255, "Sprite palette")) return;

			// Verify the coordinates
			int px = posx.GetResult(0);
			int py = -posy.GetResult(0);
			if((px < General.Map.FormatInterface.MinCoordinate) || (px > General.Map.FormatInterface.MaxCoordinate) ||
			   (py < General.Map.FormatInterface.MinCoordinate) || (py > General.Map.FormatInterface.MaxCoordinate))
			{
				General.ShowWarningMessage("Sprite coordinates must be between " + General.Map.FormatInterface.MinCoordinate + " and " + General.Map.FormatInterface.MaxCoordinate + ".", MessageBoxButtons.OK);
				return;
			}

			// Make undo
			General.Map.UndoRedo.CreateUndo("Edit " + (sprites.Count > 1 ? sprites.Count + " sprites" : "sprite"));

			// Collect flags...
			Dictionary<string, CheckState> flagsstate = new Dictionary<string, CheckState>();
			foreach(CheckBox c in flags.Checkboxes) flagsstate[c.Tag.ToString()] = c.CheckState;

			bool applyposition = (posx.ApplyMode != NumericTextboxApplyMode.NO_VALUE ||
								  posy.ApplyMode != NumericTextboxApplyMode.NO_VALUE ||
								  posz.ApplyMode != NumericTextboxApplyMode.NO_VALUE);

			bool applyvelocity = (velx.ApplyMode != NumericTextboxApplyMode.NO_VALUE ||
								  vely.ApplyMode != NumericTextboxApplyMode.NO_VALUE ||
								  velz.ApplyMode != NumericTextboxApplyMode.NO_VALUE);
			
			// Apply to all sprites...
			foreach(Thing s in sprites)
			{
				// Type
				s.TileIndex = General.Clamp(tex.GetResult(s.TileIndex), General.Map.FormatInterface.MinTileIndex, General.Map.FormatInterface.MaxTileIndex);

				// Properties
				s.OffsetX = General.Wrap(offsetx.GetResult(s.OffsetX), General.Map.FormatInterface.MinSpriteOffset, General.Map.FormatInterface.MaxSpriteOffset);
				s.OffsetY = General.Wrap(offsety.GetResult(s.OffsetY), General.Map.FormatInterface.MinSpriteOffset, General.Map.FormatInterface.MaxSpriteOffset);
				s.RepeatX = General.Clamp(repeatx.GetResult(s.RepeatX), General.Map.FormatInterface.MinSpriteRepeat, General.Map.FormatInterface.MaxSpriteRepeat);
				s.RepeatY = General.Clamp(repeaty.GetResult(s.RepeatY), General.Map.FormatInterface.MinSpriteRepeat, General.Map.FormatInterface.MaxSpriteRepeat);
				s.Shade = General.Clamp(shade.GetResult(s.Shade), General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
				s.PaletteIndex = palette.GetResult(s.PaletteIndex);
				s.Owner = owner.GetResult(s.Owner);
				
				// Position and velocity
				if(applyposition)
				{
					s.Move(new Vector3D(posx.GetResultFloat(s.Position.x), -posy.GetResultFloat(-s.Position.y), posz.GetResultFloat(s.Position.z)));
				}
				if(applyvelocity)
				{
					s.Velocity = new Vector3D(velx.GetResultFloat(s.Velocity.x), vely.GetResultFloat(s.Velocity.y), velz.GetResultFloat(s.Velocity.z));
				}

				// Angle
				s.Angle = Angle2D.DegToRad(General.Wrap(angle.GetResultFloat(Angle2D.RadToDeg(s.Angle)), 0f, 359f));

				// Identification
				s.HiTag = hitag.GetResult(s.HiTag);
				s.LoTag = lotag.GetResult(s.LoTag);
				s.Extra = extra.GetResult(s.Extra);

				// Flags
				foreach(KeyValuePair<string, CheckState> group in flagsstate)
				{
					switch(group.Value)
					{
						case CheckState.Checked: s.SetFlag(group.Key, true); break;
						case CheckState.Unchecked: s.SetFlag(group.Key, false); break;
					}
				}

				// Update settings
				s.UpdateConfiguration();
			}

			// Update defaults
			var defaultflags = new List<string>();
			foreach(CheckBox c in flags.Checkboxes)
				if(c.CheckState == CheckState.Checked) defaultflags.Add(c.Tag.ToString());
			General.Map.Config.DefaultSpriteTile = spritetype.GetResult(General.Map.Config.DefaultSpriteTile);
			General.Map.Config.DefaultSpriteAngle = Angle2D.DegToRad((float)angle.GetResult((int)Angle2D.RadToDeg(General.Map.Config.DefaultSpriteAngle) - 90) + 90);
			General.Map.Config.DefaultSpriteFlags = defaultflags;
			
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

		// Help
		private void ThingEditForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			General.ShowHelp("w_thingeditor.html");
			hlpevent.Handled = true;
		}
		
		#endregion
	}
}