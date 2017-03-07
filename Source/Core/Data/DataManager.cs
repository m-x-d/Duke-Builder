
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
using System.IO;
using System.Threading;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Config.ImageSets;
using mxd.DukeBuilder.Data.DataReaders;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder.Data
{
	public sealed class DataManager
	{
		#region ================== Variables
		
		// Data containers
		private List<DataReader> containers;
		
		// Palette
		private Playpal palette;
		
		// Textures
		private Dictionary<int, ImageData> images;
		private List<MatchingImageSet> imagesets;
		private List<ResourceImageSet> resourceimages;
		private AllImageSet allimages;
		
		// Background loading
		private Queue<ImageData> imageque;
		private Thread backgroundloader;
		private volatile bool updatedusedimages;
		private bool notifiedbusy;
		
		// Image previews
		private PreviewManager previews;
		
		// Special images
		private ImageData missingtexture3d;
		private UnknownImage unknownimage;
		private ImageData hourglass3d;
		private ImageData crosshair;
		private ImageData crosshairbusy;
		private ImageData thingbox;
		private ImageData whitetexture;
		private ImageData thingtexture; //mxd

		//mxd. Image Browser images
		private ImageData foldertexture;
		private ImageData folderuptexture;
		
		// Used images
		private Dictionary<int, int> usedimages;
		
		// Sprite types and categories defined in Game configuration
		private List<SpriteCategory> spritecategories;
		private Dictionary<int, SpriteInfo> spritetypes;
		
		// Timing
		private double loadstarttime;
		private double loadfinishtime;
		
		// Disposing
		private bool isdisposed;

		#endregion

		#region ================== Properties

		public Playpal Palette { get { return palette; } }
		public PreviewManager Previews { get { return previews; } }
		public IEnumerable<ImageData> Images { get { return images.Values; } }
		public bool IsDisposed { get { return isdisposed; } }
		public ImageData MissingTexture3D { get { return missingtexture3d; } }
		public ImageData Hourglass3D { get { return hourglass3d; } }
		public ImageData Crosshair3D { get { return crosshair; } }
		public ImageData CrosshairBusy3D { get { return crosshairbusy; } }
		public ImageData ThingBox { get { return thingbox; } }
		public ImageData WhiteTexture { get { return whitetexture; } }
		public ImageData ThingTexture { get { return thingtexture; } } //mxd
		internal ImageData FolderTexture { get { return foldertexture; } } //mxd
		internal ImageData FolderUpTexture { get { return folderuptexture; } } //mxd
		public List<SpriteCategory> SpriteCategories { get { return spritecategories; } }
		public ICollection<SpriteInfo> SpriteTypes { get { return spritetypes.Values; } }
		internal ICollection<MatchingImageSet> ImageSets { get { return imagesets; } }
		internal ICollection<ResourceImageSet> ResourceImageSets { get { return resourceimages; } }
		internal AllImageSet AllImageSet { get { return allimages; } }
		
		public bool IsLoading
		{
			get
			{
				return imageque != null && backgroundloader != null && backgroundloader.IsAlive 
					&& ((imageque.Count > 0) || previews.IsLoading);
			}
		}
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal DataManager()
		{
			// Load special images
			missingtexture3d = new ResourceImage("mxd.DukeBuilder.Resources.MissingTexture3D.png");
			missingtexture3d.LoadImage();
			hourglass3d = new ResourceImage("mxd.DukeBuilder.Resources.Hourglass3D.png");
			hourglass3d.LoadImage();
			crosshair = new ResourceImage("mxd.DukeBuilder.Resources.Crosshair.png");
			crosshair.LoadImage();
			crosshairbusy = new ResourceImage("mxd.DukeBuilder.Resources.CrosshairBusy.png");
			crosshairbusy.LoadImage();
			thingbox = new ResourceImage("mxd.DukeBuilder.Resources.ThingBox.png");
			thingbox.LoadImage();
			whitetexture = new ResourceImage("mxd.DukeBuilder.Resources.White.png");
			whitetexture.UseColorCorrection = false;
			whitetexture.LoadImage();
			whitetexture.CreateTexture();
			thingtexture = new ResourceImage("mxd.DukeBuilder.Resources.ThingTexture2D.png");
			thingtexture.UseColorCorrection = false;
			thingtexture.LoadImage();
			thingtexture.CreateTexture();
			unknownimage = new UnknownImage(Properties.Resources.MissingTexture3D); //mxd. There should be only one!

			//mxd. Textures browser images
			foldertexture = new ResourceImage("mxd.DukeBuilder.Resources.Folder96.png") { UseColorCorrection = false };
			foldertexture.LoadImage();
			folderuptexture = new ResourceImage("mxd.DukeBuilder.Resources.Folder96Up.png") { UseColorCorrection = false };
			folderuptexture.LoadImage();
		}
		
		// Disposer
		internal void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Clean up
				Unload();
				missingtexture3d.Dispose();
				missingtexture3d = null;
				hourglass3d.Dispose();
				hourglass3d = null;
				crosshair.Dispose();
				crosshair = null;
				crosshairbusy.Dispose();
				crosshairbusy = null;
				thingbox.Dispose();
				thingbox = null;
				whitetexture.Dispose();
				whitetexture = null;
				thingtexture.Dispose(); //mxd
				thingtexture = null; //mxd
				unknownimage.Dispose(); //mxd
				unknownimage = null; //mxd
				foldertexture.Dispose(); //mxd
				foldertexture = null; //mxd
				folderuptexture.Dispose(); //mxd
				folderuptexture = null; //mxd
				
				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Loading / Unloading

		// This loads all data resources
		internal void Load(DataLocationList locations)
		{
			// Create collections
			containers = new List<DataReader>();
			images = new Dictionary<int, ImageData>();
			imageque = new Queue<ImageData>();
			previews = new PreviewManager();
			imagesets = new List<MatchingImageSet>();
			usedimages = new Dictionary<int, int>();
			spritecategories = General.Map.Config.GetThingCategories();
			spritetypes = General.Map.Config.GetThingTypes();
			
			// Load texture sets
			foreach(DefinedImageSet ts in General.Map.ConfigSettings.TextureSets)
				imagesets.Add(new MatchingImageSet(ts));
			
			// Sort the texture sets
			imagesets.Sort();
			
			// Special textures sets
			allimages = new AllImageSet();
			resourceimages = new List<ResourceImageSet>();
			
			// Go for all locations
			foreach(DataLocation dl in locations)
			{
				DataReader c;
				try
				{
					// Choose container type
					switch(dl.type)
					{
						// GRP file container
						case DataLocationType.RESOURCE_GRP:
							c = new GRPReader(dl);
							break;

						case DataLocationType.RESOURCE_ART:
							c = new ARTReader(dl);
							break;

						// Directory container
						//case DataLocationType.RESOURCE_DIRECTORY:
							//c = new DirectoryReader(dl);
							//break;

						// PK3 file container
						//case DataLocationType.RESOURCE_PK3:
							//c = new PK3Reader(dl);
							//break;

						default: throw new NotImplementedException("Unknown DataLocationType");
					}
				}
				catch(Exception e)
				{
					// Unable to load resource
					General.ErrorLogger.Add(ErrorType.Error, "Unable to load resources from location \"" + dl.location + "\". Please make sure the location is accessible and not in use by another program. The resources will now be loaded with this location excluded. You may reload the resources to try again.\n" + e.GetType().Name + " when creating data reader: " + e.Message);
					General.WriteLogLine(e.StackTrace);
					continue;
				}	

				// Add container
				containers.Add(c);
				resourceimages.Add(c.ImageSet);
			}
			
			// Load stuff
			LoadPalette();
			images = LoadImages();

			// Sort things
			foreach(SpriteCategory tc in spritecategories) tc.SortIfNeeded();

			// Update the used textures
			General.Map.Data.UpdateUsedImages();
			
			// Add texture names to texture sets
			foreach(KeyValuePair<int, ImageData> img in images)
			{
				// Add to all sets where it matches
				foreach(var ms in imagesets) ms.AddImage(img.Value);

				// Add to all
				allimages.AddImage(img.Value);
			}
			
			// Start background loading
			StartBackgroundLoader();
			
			// Output info
			General.WriteLogLine("Loaded " + images.Count + " images, " + spritetypes.Count + " sprite types");
		}
		
		// This unloads all data
		internal void Unload()
		{
			// Stop background loader
			StopBackgroundLoader();
			
			// Dispose preview manager
			previews.Dispose();
			previews = null;
			
			// Dispose resources
			foreach(ImageData i in images.Values) i.Dispose();
			palette = null;
			
			// Dispose containers
			foreach(DataReader c in containers) c.Dispose();
			containers.Clear();
			
			// Trash collections
			containers = null;
			images = null;
			imageque = null;
		}
		
		#endregion
		
		#region ================== Suspend / Resume

		// This suspends data resources
		internal void Suspend()
		{
			// Stop background loader
			StopBackgroundLoader();
			
			// Go for all containers
			foreach(DataReader d in containers)
			{
				// Suspend
				General.WriteLogLine("Suspended data resource \"" + d.Location.location + "\"");
				d.Suspend();
			}
		}

		// This resumes data resources
		internal void Resume()
		{
			// Go for all containers
			foreach(DataReader d in containers)
			{
				try
				{
					// Resume
					General.WriteLogLine("Resumed data resource \"" + d.Location.location + "\"");
					d.Resume();
				}
				catch(Exception e)
				{
					// Unable to load resource
					General.ErrorLogger.Add(ErrorType.Error, "Unable to load resources from location \"" + d.Location.location + "\". Please make sure the location is accessible and not in use by another program. The resources will now be loaded with this location excluded. You may reload the resources to try again.\n" + e.GetType().Name + " when resuming data reader: " + e.Message + ")");
					General.WriteLogLine(e.StackTrace);
				}
			}
			
			// Start background loading
			StartBackgroundLoader();
		}
		
		#endregion

		#region ================== Background Loading
		
		// This starts background loading
		private void StartBackgroundLoader()
		{
			// Timing
			loadstarttime = General.Clock.GetCurrentTime();
			loadfinishtime = 0;
			
			// If a loader is already running, stop it first
			if(backgroundloader != null) StopBackgroundLoader();

			// Start a low priority thread to load images in background
			General.WriteLogLine("Starting background resource loading...");
			backgroundloader = new Thread(BackgroundLoad)
			                   {
				                   Name = "Background Loader", 
								   Priority = ThreadPriority.Lowest, 
								   IsBackground = true,
			                   };
			backgroundloader.Start();
		}
		
		// This stops background loading
		private void StopBackgroundLoader()
		{
			General.WriteLogLine("Stopping background resource loading...");
			if(backgroundloader != null)
			{
				// Stop the thread and wait for it to end
				backgroundloader.Interrupt();
				backgroundloader.Join();

				// Reset load states on all images in the list
				while(imageque.Count > 0)
				{
					ImageData img = imageque.Dequeue();
					
					switch(img.ImageState)
					{
						case ImageLoadState.Loading:
							img.ImageState = ImageLoadState.None;
							break;

						case ImageLoadState.Unloading:
							img.ImageState = ImageLoadState.Ready;
							break;
					}

					switch(img.PreviewState)
					{
						case ImageLoadState.Loading:
							img.PreviewState = ImageLoadState.None;
							break;

						case ImageLoadState.Unloading:
							img.PreviewState = ImageLoadState.Ready;
							break;
					}
				}
				
				// Done
				notifiedbusy = false;
				backgroundloader = null;
				General.SendMessage(General.MainWindow.Handle, (int)MainForm.ThreadMessages.UpdateStatus, 0, 0);
			}
		}
		
		// The background loader
		private void BackgroundLoad()
		{
			try
			{
				do
				{
					// Do we have to update the used-in-map status?
					if(updatedusedimages) BackgroundUpdateUsedTextures();
					
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
						// Load this image?
						if(image.IsReferenced && (image.ImageState != ImageLoadState.Ready))
						{
							image.LoadImage();
						}
						
						// Unload this image?
						if(!image.IsReferenced && image.AllowUnload && (image.ImageState != ImageLoadState.None))
						{
							// Still unreferenced?
							image.UnloadImage();
						}
					}
					
					// Doing something?
					if(image != null)
					{
						// Wait a bit and update icon
						if(!notifiedbusy)
						{
							notifiedbusy = true;
							General.SendMessage(General.MainWindow.Handle, (int)MainForm.ThreadMessages.UpdateStatus, 0, 0);
						}
						Thread.Sleep(0);
					}
					else
					{
						// Process previews only when we don't have images to process
						// because these are lower priority than the actual images
						if(previews.BackgroundLoad())
						{
							// Wait a bit and update icon
							if(!notifiedbusy)
							{
								notifiedbusy = true;
								General.SendMessage(General.MainWindow.Handle, (int)MainForm.ThreadMessages.UpdateStatus, 0, 0);
							}
							Thread.Sleep(0);
						}
						else
						{
							if(notifiedbusy)
							{
								notifiedbusy = false;
								General.SendMessage(General.MainWindow.Handle, (int)MainForm.ThreadMessages.UpdateStatus, 0, 0);
							}
							
							// Timing
							if(loadfinishtime == 0)
							{
								loadfinishtime = General.Clock.GetCurrentTime();
								double deltatimesec = (loadfinishtime - loadstarttime) / 1000.0d;
								General.WriteLogLine("Resources loading took " + deltatimesec.ToString("########0.00") + " seconds");
							}
							
							// Wait longer to release CPU resources
							Thread.Sleep(50);
						}
					}
				}
				while(true);
			}
			catch(ThreadInterruptedException) { }
		}
		
		// This adds an image for background loading or unloading
		internal void ProcessImage(ImageData img)
		{
			// Load this image?
			if((img.ImageState == ImageLoadState.None) && img.IsReferenced)
			{
				// Add for loading
				img.ImageState = ImageLoadState.Loading;
				lock(imageque) { imageque.Enqueue(img); }
			}
			
			// Unload this image?
			if((img.ImageState == ImageLoadState.Ready) && !img.IsReferenced && img.AllowUnload)
			{
				// Add for unloading
				img.ImageState = ImageLoadState.Unloading;
				lock(imageque) { imageque.Enqueue(img); }
			}
			
			// Update icon
			General.SendMessage(General.MainWindow.Handle, (int)MainForm.ThreadMessages.UpdateStatus, 0, 0);
		}

		// This updates the used-in-map status on all textures and flats
		private void BackgroundUpdateUsedTextures()
		{
			lock(usedimages)
			{
				// Set used on all textures
				foreach(KeyValuePair<int, ImageData> i in images)
				{
					i.Value.SetUsedInMap(usedimages.ContainsKey(i.Key));
					if(i.Value.IsImageLoaded != i.Value.IsReferenced) ProcessImage(i.Value);
				}
				
				// Done
				updatedusedimages = false;
			}
		}
		
		#endregion
		
		#region ================== Palette

		// This loads the PALETTE.DAT palette
		private void LoadPalette()
		{
			// Go for all opened containers
			for(int i = containers.Count - 1; i >= 0; i--)
			{
				// Load palette
				palette = containers[i].LoadPalette();
				if(palette != null) break;
			}

			// Make empty palette when still no palette found
			if(palette == null)
			{
				General.ErrorLogger.Add(ErrorType.Warning, "None of the loaded resources define a color palette. Did you forget to configure a game GRP for this game configuration?");
				palette = new Playpal();
			}
		}

		#endregion

		#region ================== Image loading

		// This loads the textures
		private Dictionary<int, ImageData> LoadImages()
		{
			var list = new Dictionary<int, ImageData>();

			// Go for all opened containers
			foreach(DataReader dr in containers)
			{
				// Load images
				ICollection<ImageData> drimages = dr.LoadImages();
				if(drimages != null)
				{
					// Go for all textures
					foreach(ImageData img in drimages)
					{
						// Add or replace in textures list
						list[img.TileIndex] = img;

						// Add to preview manager
						previews.AddImage(img);
					}
				}
			}
			
			// Output info
			return list;
		}

		// This returns a specific texture stream
		internal Stream GetImageStream(int tileindex)
		{
			// Go for all opened containers
			for(int i = containers.Count - 1; i >= 0; i--)
			{
				// This contain provides this patch?
				Stream s = containers[i].GetImageStream(tileindex);
				if(s != null) return s;
			}

			// No such patch found
			return null;
		}
		
		// This checks if a given texture is known
		public bool GetImageExists(int tileindex)
		{
			return images.ContainsKey(tileindex);
		}
		
		// This returns an image by long
		public ImageData GetImageData(int tileindex)
		{
			return (images.ContainsKey(tileindex) ? images[tileindex] : unknownimage);
		}

		#endregion

		#region ================== Sprites
		
		// This gets sprite information by index
		public SpriteInfo GetSpriteInfo(int tileindex)
		{
			return (spritetypes.ContainsKey(tileindex) ? spritetypes[tileindex] : new SpriteInfo(tileindex));
		}

		// This gets sprite information by index
		// Returns null when sprite type info could not be found
		public SpriteInfo GetSpriteInfoEx(int tileindex)
		{
			return (spritetypes.ContainsKey(tileindex) ? spritetypes[tileindex] : null);
		}

		#endregion
		
		#region ================== Tools

		// This signals the background thread to update the
		// used-in-map status on all textures and flats
		public void UpdateUsedImages()
		{
			lock(usedimages)
			{
				usedimages.Clear();

				// Go through the walls to find the used images
				foreach(Sidedef sd in General.Map.Map.Sidedefs)
				{
					// Add used textures to dictionary
					usedimages[sd.TileIndex] = 0;
					usedimages[sd.MaskedTileIndex] = 0;
				}

				// Go through the sectors to find the used images
				foreach(Sector s in General.Map.Map.Sectors)
				{
					// Add used textures to dictionary
					usedimages[s.FloorTileIndex] = 0;
					usedimages[s.CeilingTileIndex] = 0;
				}

				// Go through the sprites to find the used images
				foreach(Thing t in General.Map.Map.Things)
				{
					// Add used textures to dictionary
					//TODO: mark all sprite rotations as used!
					usedimages[t.TileIndex] = 0;
				}

				// Notify the background thread that it needs to update the images
				updatedusedimages = true;
			}
		}
		
		#endregion
	}
}
