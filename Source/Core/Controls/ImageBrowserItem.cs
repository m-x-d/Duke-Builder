#region ================== Namespaces

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using mxd.DukeBuilder.Data;

#endregion

namespace mxd.DukeBuilder.Controls
{
	internal class ImageBrowserItem : IComparable<ImageBrowserItem>
	{
		#region ================== Variables

		private ImageData icon;
		private bool imageloaded;
		private string tooltip;
		private int namewidth;
		private bool ispo2;

		private static Font font = SystemFonts.MessageBoxFont; // Cached to improve performance...

		#endregion

		#region ================== Properties

		public ImageData Icon { get { return icon; } }
		public bool IsPreviewLoaded { get { return icon.IsPreviewLoaded; } }
		public string TileName { get { return icon.TileIndex.ToString(); } }
		public int TileIndex { get { return icon.TileIndex; } }
		public int ImageNameWidth { get { return namewidth; } }
		public string ToolTip { get { return tooltip; } }

		public bool IsPowerOf2 { get { return ispo2; } } // True when both width and height are Po2
		public bool HasAlpha { get { return icon.HasAlpha; } }

		#endregion

		#region ================== Constructor

		// Constructors
		public ImageBrowserItem(ImageData icon, string tooltip)
		{
			// Initialize
			this.icon = icon;
			this.imageloaded = icon.IsPreviewLoaded; //mxd
			this.tooltip = tooltip; //mxd

			//mxd. Cache some image properties
			ispo2 = (General.NextPowerOf2(icon.Width) == icon.Width && General.NextPowerOf2(icon.Height) == icon.Height);

			//mxd. Calculate names width
			this.namewidth = (int)Math.Ceiling(General.Interface.MeasureString(icon.TileIndex.ToString(), font, 10000, StringFormat.GenericTypographic).Width);
		}

		#endregion

		#region ================== Methods

		internal bool CheckRedrawNeeded()
		{
			if(icon.IsPreviewLoaded != imageloaded)
			{
				imageloaded = icon.IsPreviewLoaded;
				return true;
			}
			return false;
		}

		internal void Draw(Graphics g, Image bmp, int x, int y, int w, int h, bool selected, bool showimagesize)
		{
			if(bmp == null) return;

			g.InterpolationMode = InterpolationMode.NearestNeighbor;

			var iw = bmp.Width;
			var ih = bmp.Height;

			if(iw < w && ih < h) //mxd
			{
				float scaler = Math.Min(w / (float)iw, h / (float)ih);
				iw = (int)Math.Floor(iw * scaler);
				ih = (int)Math.Floor(ih * scaler);
			}
			else if(iw > w && iw >= ih)
			{
				ih = (int)Math.Floor(h * (ih / (float)iw));
				iw = w;
			}
			else if(ih > h)
			{
				iw = (int)Math.Floor(w * (iw / (float)ih));
				ih = h;
			}

			int ix = (iw < w ? x + (w - iw) / 2 : x);
			int iy = (ih < h ? y + (h - ih) / 2 : y);

			// Pick colors and brushes
			Color bgcolor = Color.Black;
			Brush fgbrush = (icon.UsedInMap ? Brushes.Orange : Brushes.White);
			Brush selectedbgbrush = Brushes.LightGray;
			Pen frame = (icon.UsedInMap ? Pens.Orange : Pens.LightGray);
			Pen selection = Pens.Red;
			Brush selectionbrush = Brushes.Red;
			Brush selectiontextbrush = Brushes.White;

			// Selected image bg
			if(selected) g.FillRectangle(selectedbgbrush, x, y, w, h);

			// Image
			g.PixelOffsetMode = PixelOffsetMode.Half;
			g.DrawImage(bmp, ix, iy, iw, ih);
			g.PixelOffsetMode = PixelOffsetMode.Default;
			
			// Image name bg
			if(selected)
			{
				g.FillRectangle(selectionbrush, x - 1, y + h + 1, w + 2, font.Height + 1);
			}
			else
			{
				using(Brush bg = new SolidBrush(Color.FromArgb(128, bgcolor)))
				{
					g.FillRectangle(bg, x - 1, y + h + 2, w + 2, font.Height - 1);
				}
			}

			// Frame
			g.DrawRectangle((selected ? selection : frame), x - 1, y - 1, w + 1, h + 1);

			// Image name
			g.DrawString(TileName, font, (selected ? selectiontextbrush : fgbrush), x - 2, y + h + 1);

			// Image size
			if(showimagesize && icon.IsPreviewLoaded)
			{
				string imagesize = Math.Abs(icon.Width) + "x" + Math.Abs(icon.Height);
				SizeF textsize = g.MeasureString(imagesize, font);
				textsize.Width += 2;
				textsize.Height -= 3;

				if(w > textsize.Width + 6 && h > textsize.Height + 6)
				{
					// Draw bg
					if(selected)
					{
						g.FillRectangle(selectionbrush, x, y, textsize.Width, textsize.Height);
					}
					else
					{
						using(Brush bg = new SolidBrush(Color.FromArgb(128, bgcolor)))
						{
							g.FillRectangle(bg, x, y, textsize.Width, textsize.Height);
						}
					}

					// Draw text
					g.DrawString(imagesize, font, (selected ? selectiontextbrush : fgbrush), x, y - 1);
				}
			}
		}

		// Comparer
		public int CompareTo(ImageBrowserItem other)
		{
			return this.TileIndex.CompareTo(other.TileIndex);
		}

		//mxd. This greatly speeds up Dictionary lookups
		/*public override int GetHashCode()
		{
			return icon.GetHashCode();
		}*/

		#endregion
	}
}
