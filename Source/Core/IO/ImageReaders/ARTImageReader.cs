#region ================== Namespaces

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.IO
{
	internal unsafe class ARTImageReader : IImageReader
	{
		#region ================== Variables

		// Palette to use
		private readonly Playpal palette;
		private bool hasalpha; //mxd
		
		#endregion

		#region ================== Properties

		public bool HasAlpha { get { return hasalpha; } }

		#endregion


		#region ================== Constructor / Disposer

		// Constructor
		public ARTImageReader(Playpal palette)
		{
			// Initialize
			this.palette = palette;
		}

		#endregion

		#region ================== Methods

		public Bitmap ReadAsBitmap(Stream stream, int width, int height)
		{
			// Read pixel data
			Bitmap bmp;
			PixelColor[] pixeldata = ReadAsPixelData(stream, width, height);
			if(pixeldata != null)
			{
				// Create bitmap and lock pixels
				try
				{
					bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
					BitmapData bitmapdata = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
					PixelColor* targetdata = (PixelColor*)bitmapdata.Scan0.ToPointer();

					// Copy the pixels
					int size = pixeldata.Length - 1;
					for(PixelColor* cp = targetdata + size; cp >= targetdata; cp--)
						*cp = pixeldata[size--];

					// Done
					bmp.UnlockBits(bitmapdata);
				}
				catch(Exception e)
				{
					// Unable to make bitmap
					General.ErrorLogger.Add(ErrorType.Error, "Unable to make ART image data. " + e.GetType().Name + ": " + e.Message);
					return null;
				}
			}
			else
			{
				// Failed loading picture
				bmp = null;
			}

			// Return result
			return bmp;
		}

		public Bitmap ReadAsBitmap(Stream stream)
		{
			throw new NotSupportedException("Not supported by ARTReader");
		}

		private PixelColor[] ReadAsPixelData(Stream stream, int width, int height)
		{
			// The pixels are stored as bytes, corresponding to indexes in the palette stored in PALETTE.DAT.
			// The pixels in each tile are stored columnwise, starting from the top-left.

			// Allocate memory
			int size = width * height;
			PixelColor[] pixeldata = new PixelColor[size];
			hasalpha = false;

			// Read image bytes from stream
			byte[] bytes = new byte[size];
			stream.Read(bytes, 0, size);

			// Convert bytes with palette
			int counter = 0;
			for(int h = 0; h < height; h++)
			{
				for(int w = 0; w < width; w++)
				{
					int palindex = bytes[h + w * height];
					if(palindex == 255) hasalpha = true;
					pixeldata[counter] = palette[palindex];
					counter++;
				}
			}

			// Return pointer
			return pixeldata;
		}

		#endregion
	}
}
