
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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.Windows;
using SlimDX.Direct3D9;

#endregion

namespace mxd.DukeBuilder.Data
{
	public abstract unsafe class ImageData
	{
		#region ================== Variables
		
		// Properties
		protected int tileindex;
		protected int width;
		protected int height;
		protected int offsetx;
		protected int offsety;
		private bool usecolorcorrection;
		protected bool hasalpha; //mxd. If true, has pixels with zero alpha

		//mxd. Hashing
		private static int hashcounter;
		private readonly int hashcode;
		
		// Loading
		private volatile ImageLoadState previewstate;
		private volatile ImageLoadState imagestate;
		private volatile int previewindex;
		protected volatile bool loadfailed;
		private volatile bool allowunload;
		
		// References
		private volatile bool usedinmap;
		private volatile int references;
		
		// GDI bitmap
		protected Bitmap bitmap;
		
		// Direct3D texture
		private int mipmaplevels;	// 0 = all mipmaps
		private Texture texture;
		
		// Disposing
		private bool isdisposed;
		
		#endregion
		
		#region ================== Properties

		public int TileIndex { get { return tileindex; } }
		public bool UseColorCorrection { get { return usecolorcorrection; } set { usecolorcorrection = value; } }
		public bool HasAlpha { get { return hasalpha; } } //mxd
		public Texture Texture { get { lock(this) { return texture; } } }
		public bool IsPreviewLoaded { get { return (previewstate == ImageLoadState.Ready); } }
		public bool IsImageLoaded { get { return (imagestate == ImageLoadState.Ready); } }
		public bool LoadFailed { get { return loadfailed; } }
		public bool IsDisposed { get { return isdisposed; } }
		public bool AllowUnload { get { return allowunload; } set { allowunload = value; } }
		public ImageLoadState ImageState { get { return imagestate; } internal set { imagestate = value; } }
		public ImageLoadState PreviewState { get { return previewstate; } internal set { previewstate = value; } }
		public bool IsReferenced { get { return (references > 0) || usedinmap; } }
		public bool UsedInMap { get { return usedinmap; } }
		public int MipMapLevels { get { return mipmaplevels; } set { mipmaplevels = value; } }
		public int Width { get { return width; } }
		public int Height { get { return height; } }
		public int OffsetX { get { return offsetx; } }
		public int OffsetY { get { return offsety; } }
		internal int PreviewIndex { get { return previewindex; } set { previewindex = value; } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public ImageData()
		{
			// Defaults
			this.usecolorcorrection = true;
			this.allowunload = true;

			//mxd. Hashing
			this.hashcode = hashcounter++;
		}

		//mxd. Constructor
		public ImageData(int tileindex, int width, int height, int offsetx, int offsety)
		{
			this.tileindex = tileindex;
			this.width = width;
			this.height = height;
			this.offsetx = offsetx;
			this.offsety = offsety;
			
			// Defaults
			this.usecolorcorrection = true;
			this.allowunload = true;

			// Hashing
			this.hashcode = hashcounter++;
		}

		// Destructor
		~ImageData()
		{
			this.Dispose();
		}
		
		// Disposer
		public virtual void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				lock(this)
				{
					// Clean up
					if(bitmap != null) bitmap.Dispose();
					if(texture != null) texture.Dispose();
					bitmap = null;
					texture = null;
					
					// Done
					usedinmap = false;
					imagestate = ImageLoadState.None;
					previewstate = ImageLoadState.None;
					isdisposed = true;
				}
			}
		}
		
		#endregion
		
		#region ================== Management
		
		// This sets the status of the texture usage in the map
		internal void SetUsedInMap(bool used)
		{
			if(used != usedinmap)
			{
				usedinmap = used;
				General.Map.Data.ProcessImage(this);
			}
		}
		
		// This adds a reference
		public void AddReference()
		{
			references++;
			if(references == 1) General.Map.Data.ProcessImage(this);
		}
		
		// This removes a reference
		public void RemoveReference()
		{
			references--;
			if(references < 0) General.Fail("FAIL! (references < 0) Somewhere this image is dereferenced more than it was referenced.");
			if(references == 0) General.Map.Data.ProcessImage(this);
		}
		
		// This unloads the image
		public virtual void UnloadImage()
		{
			lock(this)
			{
				if(bitmap != null) bitmap.Dispose();
				bitmap = null;
				imagestate = ImageLoadState.None;
			}
		}

		// This returns the bitmap image
		public Bitmap GetBitmap()
		{
			lock(this)
			{
				// Image loaded successfully?
				if(!loadfailed && (imagestate == ImageLoadState.Ready) && (bitmap != null))
				{
					return bitmap;
				}

				// Image loading failed?
				return (loadfailed ? Properties.Resources.Failed : Properties.Resources.Hourglass);
			}
		}
		
		// This loads the image
		public void LoadImage()
		{
			// Do the loading
			LocalLoadImage();

			// Notify the main thread about the change so that sectors can update their buffers
			//IntPtr strptr = Marshal.StringToCoTaskMemAuto(this.name);
			General.SendMessage(General.MainWindow.Handle, (int)MainForm.ThreadMessages.ImageDataLoaded, tileindex, 0);
		}
		
		// This requests loading the image
		protected virtual void LocalLoadImage()
		{
			BitmapData bmpdata = null;
			
			lock(this)
			{
				// Bitmap loaded successfully?
				if(bitmap != null)
				{
					// Bitmap has incorrect format?
					if(bitmap.PixelFormat != PixelFormat.Format32bppArgb)
					{
						//General.ErrorLogger.Add(ErrorType.Warning, "Image \"" + name + "\" does not have A8R8G8B8 pixel format. Conversion was needed.");
						Bitmap oldbitmap = bitmap;
						try
						{
							// Convert to desired pixel format
							bitmap = new Bitmap(oldbitmap.Size.Width, oldbitmap.Size.Height, PixelFormat.Format32bppArgb);
							Graphics g = Graphics.FromImage(bitmap);
							g.PageUnit = GraphicsUnit.Pixel;
							g.CompositingQuality = CompositingQuality.HighQuality;
							g.InterpolationMode = InterpolationMode.NearestNeighbor;
							g.SmoothingMode = SmoothingMode.None;
							g.PixelOffsetMode = PixelOffsetMode.None;
							g.Clear(Color.Transparent);
							g.DrawImage(oldbitmap, 0, 0, oldbitmap.Size.Width, oldbitmap.Size.Height);
							g.Dispose();
							oldbitmap.Dispose();
						}
						catch(Exception e)
						{
							bitmap = oldbitmap;
							General.ErrorLogger.Add(ErrorType.Warning, "Cannot lock image " + tileindex + " for pixel format conversion. The image may not be displayed correctly.\n" + e.GetType().Name + ": " + e.Message);
						}
					}
					
					// This applies brightness correction on the image
					if(usecolorcorrection)
					{
						try
						{
							// Try locking the bitmap
							bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
						}
						catch(Exception e)
						{
							General.ErrorLogger.Add(ErrorType.Warning, "Cannot lock image " + tileindex + " for color correction. The image may not be displayed correctly.\n" + e.GetType().Name + ": " + e.Message);
						}

						// Bitmap locked?
						if(bmpdata != null)
						{
							// Apply color correction
							PixelColor* pixels = (PixelColor*)(bmpdata.Scan0.ToPointer());
							General.Colors.ApplyColorCorrection(pixels, bmpdata.Width * bmpdata.Height);
							bitmap.UnlockBits(bmpdata);
						}
					}
				}
				else
				{
					// Loading failed
					// We still mark the image as ready so that it will
					// not try loading again until Reload Resources is used
					loadfailed = true;
					bitmap = new Bitmap(Properties.Resources.Failed);
				}

				if(bitmap != null)
				{
					width = bitmap.Width;
					height = bitmap.Height;

					// Do we still have to set a scale?
					/*if((scale.x == 0.0f) && (scale.y == 0.0f))
					{
						if((General.Map != null) && (General.Map.Config != null))
						{
							scale.x = General.Map.Config.DefaultTextureScale;
							scale.y = General.Map.Config.DefaultTextureScale;
						}
						else
						{
							scale.x = 1.0f;
							scale.y = 1.0f;
						}
					}*/
				}
				
				// Image is ready
				imagestate = ImageLoadState.Ready;
			}
		}
		
		// This creates the Direct3D texture
		public virtual void CreateTexture()
		{
			lock(this)
			{
				// Only do this when texture is not created yet
				if(((texture == null) || (texture.Disposed)) && this.IsImageLoaded && !loadfailed)
				{
					Image img = bitmap;
					if(loadfailed) img = Properties.Resources.Failed;
					
					// Write to memory stream and read from memory
					using(MemoryStream memstream = new MemoryStream((img.Size.Width * img.Size.Height * 4) + 4096))
					{
						img.Save(memstream, ImageFormat.Bmp);
						memstream.Seek(0, SeekOrigin.Begin);

						texture = Texture.FromStream(General.Map.Graphics.Device, memstream, (int)memstream.Length,
										img.Size.Width, img.Size.Height, mipmaplevels, Usage.None, Format.Unknown,
										Pool.Managed, General.Map.Graphics.PostFilter, General.Map.Graphics.MipGenerateFilter, 0);
					}
				}
			}
		}

		// This updates a dynamic texture
		/*public void UpdateTexture()
		{
			if(!dynamictexture)
				throw new Exception("The image must be a dynamic image to support direct updating.");
			
			lock(this)
			{
				if((texture != null) && !texture.Disposed)
				{
					// Lock the bitmap and texture
					BitmapData bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					DataRectangle texdata = texture.LockRectangle(0, LockFlags.Discard);

					// Copy data
					int* bp = (int*)bmpdata.Scan0.ToPointer();
					int* tp = (int*)texdata.Data.DataPointer.ToPointer();
					for(int y = 0; y < bmpdata.Height; y++)
					{
						for(int x = 0; x < bmpdata.Width; x++)
						{
							*tp = *bp;
							bp++;
							tp++;
						}

						// Skip extra data in texture
						int extrapitch = (texdata.Pitch >> 2) - bmpdata.Width;
						tp += extrapitch;
					}

					// Unlock
					texture.UnlockRectangle(0);
					bitmap.UnlockBits(bmpdata);
				}
			}
		}*/
		
		// This destroys the Direct3D texture
		/*public void ReleaseTexture()
		{
			lock(this)
			{
				// Trash it
				if(texture != null) texture.Dispose();
				texture = null;
			}
		}*/

		// This draws a preview
		/*public virtual void DrawPreview(Graphics target, Point targetpos)
		{
			lock(this)
			{
				// Preview ready?
				if(!loadfailed && (previewstate == ImageLoadState.Ready))
				{
					// Draw preview
					General.Map.Data.Previews.DrawPreview(previewindex, target, targetpos);
				}
				// Loading failed?
				else if(loadfailed)
				{
					// Draw error bitmap
					targetpos = new Point(targetpos.X + ((General.Map.Data.Previews.MaxImageWidth - Properties.Resources.Hourglass.Width) >> 1),
										  targetpos.Y + ((General.Map.Data.Previews.MaxImageHeight - Properties.Resources.Hourglass.Height) >> 1));
					target.DrawImageUnscaled(Properties.Resources.Failed, targetpos);
				}
				else
				{
					// Draw loading bitmap
					targetpos = new Point(targetpos.X + ((General.Map.Data.Previews.MaxImageWidth - Properties.Resources.Hourglass.Width) >> 1),
										  targetpos.Y + ((General.Map.Data.Previews.MaxImageHeight - Properties.Resources.Hourglass.Height) >> 1));
					target.DrawImageUnscaled(Properties.Resources.Hourglass, targetpos);
				}
			}
		}*/
		
		// This returns a copy of preview image
		/*public virtual Image GetPreviewCopy(int targetsize)
		{
			lock(this)
			{
				// Preview ready?
				if(previewstate == ImageLoadState.Ready)
				{
					// Make a copy
					return General.Map.Data.Previews.GetPreviewCopy(previewindex, targetsize);
				}

				// Loading failed?
				return (loadfailed ? Properties.Resources.Failed : Properties.Resources.Hourglass);
			}
		}*/

		//mxd. This returns a preview image
		public virtual Image GetPreview()
		{
			lock(this)
			{
				// Preview ready?
				if(previewstate == ImageLoadState.Ready)
				{
					// Return it
					return General.Map.Data.Previews.GetPreview(previewindex);
				}

				// Loading failed?
				return (loadfailed ? Properties.Resources.Failed : Properties.Resources.Hourglass);
			}
		}

		//mxd. This greatly speeds up Dictionary lookups
		public override int GetHashCode()
		{
			return hashcode;
		}
		
		#endregion
	}
}
