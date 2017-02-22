#region ================== Namespaces

using System.IO;
using mxd.DukeBuilder.IO;

#endregion

namespace mxd.DukeBuilder.Data
{
	public sealed class ARTImage : ImageData
	{
		#region ================== Constructor / Disposer

		// Constructor
		internal ARTImage(Tile tile) : base(tile.Index, tile.Width, tile.Height, tile.OffsetX, tile.OffsetY) { }

		#endregion

		#region ================== Methods

		// This loads the image
		protected override void LocalLoadImage()
		{
			// Leave when already loaded
			if(this.IsImageLoaded) return;

			lock(this)
			{
				Stream data = General.Map.Data.GetImageStream(this.tileindex);
				if(data != null)
				{
					// Check image size...
					if(width < 1 || height < 1)
					{
						General.ErrorLogger.Add(ErrorType.Error, "Art image " + this.tileindex + " has invalid size: " + width + "x" + height);
					}
					else if(data.Length != width * height)
					{
						General.ErrorLogger.Add(ErrorType.Error, "Art image " + this.tileindex + " data length doesn't match image size");
					}
					else
					{
						// Copy lump data to memory
						byte[] membytes = new byte[(int)data.Length];

						lock(data)
						{
							data.Seek(0, SeekOrigin.Begin);
							data.Read(membytes, 0, (int)data.Length);
						}

						using(MemoryStream mem = new MemoryStream(membytes))
						{
							// Get a reader for the data
							IImageReader reader = ImageDataFormat.GetImageReader(mem, SupportedImageFormat.ART, General.Map.Data.Palette);
							if(!(reader is ARTImageReader))
							{
								// Data is in an unknown format!
								General.ErrorLogger.Add(ErrorType.Error, "Art image " + this.tileindex + " data format could not be read.");
								bitmap = null;
							}
							else
							{
								ARTImageReader artreader = (ARTImageReader)reader;

								// Read data as bitmap
								mem.Seek(0, SeekOrigin.Begin);
								if(bitmap != null) bitmap.Dispose();
								bitmap = artreader.ReadAsBitmap(mem, width, height);
								hasalpha = artreader.HasAlpha;
							}
						}
					}

					// Failed when no bitmap
					loadfailed = (bitmap == null);
				}
				else
				{
					// Missing a tile!
					General.ErrorLogger.Add(ErrorType.Error, "Missing ART tile " + this.tileindex + ". Make sure the required resources are located in the map or engine directory.");
					loadfailed = true;
				}

				// Pass on to base
				base.LocalLoadImage();
			}
		}

		#endregion
	}
}
