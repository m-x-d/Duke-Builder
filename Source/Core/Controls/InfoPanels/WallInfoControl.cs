#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Controls
{
	public partial class WallInfoControl : UserControl
	{
		#region ================== Constructor

		public WallInfoControl()
		{
			InitializeComponent();
		}

		#endregion

		#region ================== Methods

		public void ShowInfo(Sidedef w)
		{
			// Set titles
			propertiesgroup.Text = " Wall " + w.Index + " (" + (w.IsFront ? "front" : "back") + ", sector " + w.Sector.Index + (w.Sector.FirstWall == w ? ", first wall) " : ") ");
			
			// Set properties
			General.DisplayZoomedImage(tex, General.Map.Data.GetImageData(w.TileIndex).GetPreview());
			texname.Text = w.TileIndex.ToString();

			bool showmaskedtex = w.Other != null && (w.Masked || w.MaskedSolid);
			if(showmaskedtex)
			{
				General.DisplayZoomedImage(maskedtex, General.Map.Data.GetImageData(w.MaskedTileIndex).GetPreview());
				maskedname.Text = w.MaskedTileIndex.ToString();
			}

			offsetx.Text = w.OffsetX.ToString();
			offsety.Text = w.OffsetY.ToString();
			repeatx.Text = w.RepeatX.ToString();
			repeaty.Text = w.RepeatY.ToString();
			hitag.Text = w.HiTag.ToString();
			lotag.Text = w.LoTag.ToString();
			extra.Text = w.Extra.ToString();
			palette.Text = w.PaletteIndex.ToString();
			shade.Text = w.Shade.ToString();

			// Disable identification labels when showing default values
			hitag.Enabled = w.HiTag > 0;
			hitaglabel.Enabled = w.HiTag > 0;
			lotag.Enabled = w.LoTag > 0;
			lotaglabel.Enabled = w.LoTag > 0;
			extra.Enabled = w.Extra != -1;
			extralabel.Enabled = w.Extra != -1;
			palette.Enabled = w.PaletteIndex > 0;
			palettelabel.Enabled = w.PaletteIndex > 0;

			offsetx.Enabled = w.OffsetX != 0;
			offsetxlabel.Enabled = w.OffsetX != 0;
			offsety.Enabled = w.OffsetY != 0;
			offsetylabel.Enabled = w.OffsetY != 0;

			// Set flags
			List<string> flagnames = new List<string>();
			foreach(KeyValuePair<string, string> group in General.Map.Config.WallFlags)
				if(w.IsFlagSet(group.Key)) flagnames.Add(group.Value);
			flags.Setup(flagnames);

			// Resize
			maskedtexpanel.Visible = showmaskedtex;
			var texcontrol = (showmaskedtex ? maskedtexpanel : tex);
			propertiespanel.Left = texcontrol.Right + texcontrol.Margin.Right + propertiespanel.Margin.Left;
			propertiesgroup.Width = propertiespanel.Right + propertiespanel.Margin.Right;
			flags.Left = propertiesgroup.Right + propertiesgroup.Margin.Right + flags.Margin.Left;
			this.Width = (flagnames.Count > 0 ? flags.Right + flags.Margin.Right : propertiesgroup.Right + propertiesgroup.Margin.Right);
		}

		#endregion

		#region ================== Event overrides

		// When visible changed
		protected override void OnVisibleChanged(EventArgs e)
		{
			// Hiding panels
			if(!this.Visible)
			{
				tex.BackgroundImage = null;
				maskedtex.BackgroundImage = null;
			}

			// Call base
			base.OnVisibleChanged(e);
		}

		#endregion
	}
}
