
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
	internal partial class SpriteInfoPanel : UserControl
	{
		#region ================== Constructor

		public SpriteInfoPanel()
		{
			// Initialize
			InitializeComponent();
		}

		#endregion

		#region ================== Methods

		// This shows the info
		public void ShowInfo(Thing s)
		{
			infopanel.Text = " Sprite " + s.Index + (s.Sector != null ? " (sector " + s.Sector.Index + ") " : " ");
			General.DisplayZoomedImage(spritetex, General.Map.Data.GetImageData(s.TileIndex).GetPreview());
			spritename.Text = s.TileIndex.ToString();

			var info = General.Map.Data.GetSpriteInfoEx(s.TileIndex);
			bool havedefinedtype = (info != null);
			type.Text = (havedefinedtype ? info.Title : "--");
			type.Enabled = havedefinedtype;
			typelabel.Enabled = havedefinedtype;

			// Determine z info to show
			s.DetermineSector();
			string zinfo = (s.Sector != null ? s.Position.z + s.Sector.FloorHeight : s.Position.z).ToString();
			position.Text = s.Position.x + ", " + s.Position.y + ", " + zinfo;
			angle.Text = (int)Math.Round(Angle2D.RadToDeg(s.Angle)) + "\u00B0";
			clipdistance.Text = s.ClipDistance.ToString();
			owner.Text = s.Owner.ToString();

			offsetx.Text = s.OffsetX.ToString();
			offsety.Text = s.OffsetY.ToString();
			repeatx.Text = s.RepeatX.ToString();
			repeaty.Text = s.RepeatY.ToString();

			hitag.Text = s.HiTag.ToString();
			lotag.Text = s.LoTag.ToString();
			extra.Text = s.Extra.ToString();
			shade.Text = s.Shade.ToString();
			palette.Text = s.PaletteIndex.ToString();

			// Disable identification labels when showing default values
			hitag.Enabled = s.HiTag > 0;
			hitaglabel.Enabled = s.HiTag > 0;
			lotag.Enabled = s.LoTag > 0;
			lotaglabel.Enabled = s.LoTag > 0;
			extra.Enabled = s.Extra != -1;
			extralabel.Enabled = s.Extra != -1;
			owner.Enabled = s.Owner != -1;
			ownerlabel.Enabled = s.Owner != -1;

			offsetx.Enabled = s.OffsetX != 0;
			offsetxlabel.Enabled = s.OffsetX != 0;
			offsety.Enabled = s.OffsetY != 0;
			offsetylabel.Enabled = s.OffsetY != 0;

			repeatx.Enabled = s.RepeatX != 0;
			repeatxlabel.Enabled = s.RepeatX != 0;
			repeaty.Enabled = s.RepeatY != 0;
			repeatylabel.Enabled = s.RepeatY != 0;

			palette.Enabled = s.PaletteIndex > 0;
			palettelabel.Enabled = s.PaletteIndex > 0;

			// Flags
			List<string> flagnames = new List<string>();
			foreach(KeyValuePair<string, string> group in General.Map.Config.SpriteFlags)
				if(s.IsFlagSet(group.Key)) flagnames.Add(group.Value);
			flags.Setup(flagnames);

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
			if(!this.Visible) spritetex.BackgroundImage = null;

			// Call base
			base.OnVisibleChanged(e);
		}

		#endregion
	}
}
