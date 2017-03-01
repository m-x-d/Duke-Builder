
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
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Windows;
using SlimDX;

#endregion

namespace mxd.DukeBuilder.Controls
{
	/// <summary>
	/// Control that provides an image picker.
	/// </summary>
	public partial class ImageSelectorControl : UserControl
	{
		#region ================== Event handlers

		public event EventHandler OnValueChanged; //mxd

		#endregion

		#region ================== Variables

		private MouseButtons button;
		private int currenttileindex;
		private string previousimagename; //mxd
		private int previewsize = 64; //mxd
		
		#endregion

		#region ================== Properties
		
		public string TextureName { get { return name.Text; } set { name.Text = value; } }

		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public ImageSelectorControl()
		{
			// Initialize
			InitializeComponent();
			currenttileindex = -1;
		}
		
		// Setup
		public virtual void Initialize()
		{
			//TODO: add named textures, if I ever implement the support
			//name.AutoCompleteCustomSource.AddRange(General.Map.Data.TextureNames.ToArray());
		}
		
		#endregion

		#region ================== Events

		// When resized
		private void ImageSelectorControl_Resize(object sender, EventArgs e)
		{
			// Fixed size
			preview.Width = this.ClientSize.Width;
			preview.Height = this.ClientSize.Height - name.Height - 4;
			name.Width = this.ClientSize.Width;
			name.Top = this.ClientSize.Height - name.Height;
		}
		
		// Layout change
		private void ImageSelectorControl_Layout(object sender, LayoutEventArgs e)
		{
			ImageSelectorControl_Resize(sender, EventArgs.Empty);
		}
		
		// Image clicked
		private void preview_Click(object sender, EventArgs e)
		{
			preview.BackColor = SystemColors.Highlight;
			if(button == MouseButtons.Left)
			{
				updatetimer.Stop();
				currenttileindex = BrowseImage(GetResult(currenttileindex));
				name.Text = currenttileindex.ToString();
			}
		}
		
		// Name text changed
		private void name_TextChanged(object sender, EventArgs e)
		{
			updatetimer.Stop();
			currenttileindex = -1; //mxd
			if(string.IsNullOrEmpty(name.Text))
			{
				// Only possible when showing multiple images, right?
				ShowPreview(Properties.Resources.ImageStack);
			}
			else
			{
				// Find the image
				var image = General.Map.Data.GetImageData(GetResult(currenttileindex));
				if(image.PreviewState != ImageLoadState.Ready && currenttileindex > -1) updatetimer.Start();

				// Show it centered
				ShowPreview(image.GetPreview());
			}
		}
		
		// Mouse pressed
		private void preview_MouseDown(object sender, MouseEventArgs e)
		{
			button = e.Button;
			if(button == MouseButtons.Left)
			{
				preview.BackColor = AdjustedColor(SystemColors.Highlight, 0.2f);
			}
		}

		// Mouse leaves
		private void preview_MouseLeave(object sender, EventArgs e)
		{
			preview.BackColor = SystemColors.AppWorkspace;
		}
		
		// Mouse enters
		private void preview_MouseEnter(object sender, EventArgs e)
		{
			preview.BackColor = SystemColors.Highlight;
		}

		private void updatetimer_Tick(object sender, EventArgs e)
		{
			updatetimer.Stop();
			if(General.Map != null && currenttileindex > -1)
			{
				// Trigger update
				name_TextChanged(this, EventArgs.Empty);
			}
		}
		
		#endregion

		#region ================== Methods
		
		// This refreshes the control
		/*new public void Refresh()
		{
			if(General.Map == null) return;
			if(currenttileindex == -1) currenttileindex = GetResult(currenttileindex);
			ShowPreview(FindImage(currenttileindex));
			base.Refresh();
		}*/
		
		// This redraws the image preview
		private void ShowPreview(Image image)
		{
			if(image != null)
			{
				// Show it centered
				preview.Image = new Bitmap(image);
				preview.Refresh();
			}
			else
			{
				// Dispose old image
				preview.Image = null;
			}

			//mxd. Dispatch event
			if(OnValueChanged != null && previousimagename != name.Text)
			{
				previousimagename = name.Text;
				OnValueChanged(this, EventArgs.Empty);
			}
		}
		
		// This must determine and return the image to show
		/*protected Image FindImage(int tileindex)
		{
			// Set the image
			var image = General.Map.Data.GetImageData(tileindex);
			if(image is UnknownImage)
			{
				ff
			}

			return image.GetPreview();
		}*/

		// This must show the image browser and return the selected texture name
		protected int BrowseImage(int tileindex)
		{
			// Browse for image
			int newtileindex = ImageBrowserForm.Browse(this.ParentForm, tileindex);
			if(newtileindex > -1)
			{
				currenttileindex = newtileindex;
				return newtileindex;
			}

			return tileindex;
		}

		// This determines the result value
		public int GetResult(int original)
		{
			// Already precached?
			if(currenttileindex > -1) return currenttileindex;
			
			// Anyting entered?
			string text = name.Text.Trim();
			if(text.Length > 0)
			{
				// Try to get tileindex
				int index;
				if(int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out index))
				{
					currenttileindex = index;
					return index;
				}
			}

			// Nothing given, keep original value
			return original;
		}

		// This brightens or darkens a color
		private static Color AdjustedColor(Color c, float amount)
		{
			Color4 cc = new Color4(c);

			// Adjust color
			cc.Red = Saturate((cc.Red * (1f + amount)) + (amount * 0.5f));
			cc.Green = Saturate((cc.Green * (1f + amount)) + (amount * 0.5f));
			cc.Blue = Saturate((cc.Blue * (1f + amount)) + (amount * 0.5f));

			// Return result
			return Color.FromArgb(cc.ToArgb());
		}

		// This clamps a value between 0 and 1
		private static float Saturate(float v)
		{
			if(v < 0f) return 0f; 
			if(v > 1f) return 1f; 
			return v;
		}
		
		#endregion
	}
}
