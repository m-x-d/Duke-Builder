
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

#endregion

namespace mxd.DukeBuilder.Data
{
	public class PreviewManager
	{
		#region ================== Constants

		// Image format
		private const PixelFormat IMAGE_FORMAT = PixelFormat.Format32bppArgb;

		//mxd. Maximum dimensions of a single preview image
		private const int MAX_PREVIEW_SIZE = 256;

		#endregion

		#region ================== Variables

		// Images
		private List<Bitmap> images;

		// Processing
		private Queue<ImageData> imageque;

		// Disposing
		private bool isdisposed;

		#endregion

		#region ================== Properties

		// Disposing
		internal bool IsDisposed { get { return isdisposed; } }

		// Loading
		internal bool IsLoading { get { return (imageque.Count > 0); } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal PreviewManager()
		{
			// Initialize
			images = new List<Bitmap>();
			imageque = new Queue<ImageData>();

			// We have no destructor
			GC.SuppressFinalize(this);
		}

		// Disposer
		internal void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Clean up
				foreach(var b in images) b.Dispose();
				images = null;

				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Private Methods

		// This makes a preview for the given image and updates the image settings
		private void MakeImagePreview(ImageData img)
		{
			lock(img)
			{
				// Load image if needed
				if(!img.IsImageLoaded) img.LoadImage();
				Bitmap preview = MakePreviewBitmap(img.GetBitmap()); //mxd

				// Unload image if no longer needed
				if(!img.IsReferenced) img.UnloadImage();

				lock(images)
				{
					// Set numbers
					img.PreviewIndex = images.Count;
					img.PreviewState = ImageLoadState.Ready;
					
					// Add to previews list
					images.Add(preview);
				}
			}
		}

		//mxd
		private static Bitmap MakePreviewBitmap(Bitmap src)
		{
			// Determine preview size
			float scalex = (src.Width > MAX_PREVIEW_SIZE) ? (MAX_PREVIEW_SIZE / (float)src.Width) : 1.0f;
			float scaley = (src.Height > MAX_PREVIEW_SIZE) ? (MAX_PREVIEW_SIZE / (float)src.Height) : 1.0f;
			float scale = Math.Min(scalex, scaley);
			int previewwidth = Math.Max((int)(src.Width * scale), 1);
			int previewheight = Math.Max((int)(src.Height * scale), 1);

			//mxd. Expected and actual image sizes and format match?
			if(previewwidth == src.Width && previewheight == src.Height && src.PixelFormat == IMAGE_FORMAT)
			{
				return new Bitmap(src);
			}
			else
			{
				// Make new image
				var preview = new Bitmap(previewwidth, previewheight, IMAGE_FORMAT);
				using(Graphics g = Graphics.FromImage(preview))
				{
					g.PageUnit = GraphicsUnit.Pixel;
					g.InterpolationMode = InterpolationMode.NearestNeighbor;
					g.PixelOffsetMode = PixelOffsetMode.None;

					// Draw image
					RectangleF imgrect = new Rectangle(0, 0, previewwidth, previewheight);
					g.DrawImage(src, imgrect);
				}

				return preview;
			}
		}

		#endregion

		#region ================== Public Methods

		//mxd. This returns a preview
		internal Bitmap GetPreview(int previewindex)
		{
			// Get the preview we need
			lock(images) { return images[previewindex]; }
		}

		// Background loading
		// Return true when we have more work to do, so that the
		// thread will not wait too long before calling again
		internal bool BackgroundLoad()
		{
			// Get next item
			ImageData image = null;
			lock(imageque)
			{
				// Fetch next image to process
				if(imageque.Count > 0) image = imageque.Dequeue();
			}

			// Any image to process?
			if(image != null)
			{
				// Make image preview?
				if(!image.IsPreviewLoaded) MakeImagePreview(image);
			}

			return (image != null);
		}

		// This adds an image for preview creation
		internal void AddImage(ImageData image)
		{
			lock(imageque)
			{
				// Add to list
				image.PreviewState = ImageLoadState.Loading;
				imageque.Enqueue(image);
			}
		}

		#endregion
	}
}
