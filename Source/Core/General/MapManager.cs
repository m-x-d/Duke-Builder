
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
using System.IO;
using System.Windows.Forms;
using mxd.DukeBuilder.Actions;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Editing;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.VisualModes;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder
{
	public sealed class MapManager : IDisposable
	{
		#region ================== Variables

		// Status
		private bool changed;
		
		// Map information
		private string filename;
		private string filepathname;
		private string filepath;
		private string temppath;

		// Main objects
		private MapSet map;
		private MapSetIO io;
		private MapOptions options;
		private ConfigurationInfo configinfo;
		private GameConfiguration config;
		private DataManager data;
		private D3DDevice graphics;
		private Renderer2D renderer2d;
		private Renderer3D renderer3d;
		private GridSetup grid;
		private UndoManager undoredo;
		private CopyPasteManager copypaste;
		private Launcher launcher;
		private ThingsFilter thingsfilter;
		private VisualCamera visualcamera;
		
		// Disposing
		private bool isdisposed;

		#endregion

		#region ================== Properties

		public string FilePathName { get { return filepathname; } }
		public string FileName { get { return filename; } }
		public string FilePath { get { return filepath; } }
		public string TempPath { get { return temppath; } }
		public MapOptions Options { get { return options; } }
		public MapSet Map { get { return map; } }
		public DataManager Data { get { return data; } }
		public bool IsChanged { get { return changed; } set { if(changed != value) General.MainWindow.UpdateTitle(); changed |= value; } }
		public bool IsDisposed { get { return isdisposed; } }
		internal D3DDevice Graphics { get { return graphics; } }
		public IRenderer2D Renderer2D { get { return renderer2d; } }
		public IRenderer3D Renderer3D { get { return renderer3d; } }
		internal Renderer2D CRenderer2D { get { return renderer2d; } }
		internal Renderer3D CRenderer3D { get { return renderer3d; } }
		public GameConfiguration Config { get { return config; } }
		public ConfigurationInfo ConfigSettings { get { return configinfo; } }
		public GridSetup Grid { get { return grid; } }
		public UndoManager UndoRedo { get { return undoredo; } }
		internal CopyPasteManager CopyPaste { get { return copypaste; } }
		public IMapSetIO FormatInterface { get { return io; } }
		internal Launcher Launcher { get { return launcher; } }
		public ThingsFilter ThingsFilter { get { return thingsfilter; } }
		public VisualCamera VisualCamera { get { return visualcamera; } set { visualcamera = value; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal MapManager()
		{
			// Create temporary path
			temppath = General.MakeTempDirname();
			Directory.CreateDirectory(temppath);
			General.WriteLogLine("Temporary directory:  " + temppath);
			
			// Basic objects
			grid = new GridSetup();
			undoredo = new UndoManager();
			copypaste = new CopyPasteManager();
			launcher = new Launcher();
			thingsfilter = new NullThingsFilter();
		}
		
		// Disposer
		public void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Let the plugins know
				General.Plugins.OnMapCloseBegin();
				
				// Stop processing
				General.MainWindow.StopProcessing();
				
				// Change to no mode
				General.Editing.ChangeMode((EditMode)null);
				
				// Unbind any methods
				General.Actions.UnbindMethods(this);
				
				// Dispose
				if(grid != null) grid.Dispose();
				if(launcher != null) launcher.Dispose();
				if(copypaste != null) copypaste.Dispose();
				if(undoredo != null) undoredo.Dispose();
				General.WriteLogLine("Unloading data resources...");
				if(data != null) data.Dispose();
				General.WriteLogLine("Unloading map data...");
				if(map != null) map.Dispose();
				General.WriteLogLine("Stopping graphics device...");
				if(renderer2d != null) renderer2d.Dispose();
				if(renderer3d != null) renderer3d.Dispose();
				if(graphics != null) graphics.Dispose();
				visualcamera = null;
				grid = null;
				launcher = null;
				copypaste = null;
				undoredo = null;
				data = null;
				//tempwad = null;
				map = null;
				renderer2d = null;
				renderer3d = null;
				graphics = null;
				
				// We may spend some time to clean things up here
				GC.Collect();
				
				// Let the plugins know
				General.Plugins.OnMapCloseEnd();
				
				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== New / Open
		
		// Initializes for a new map
		internal bool InitializeNewMap(MapOptions options)
		{
			// Apply settings
			this.filename = "unnamed.map";
			this.filepathname = string.Empty;
			this.filepath = string.Empty;
			this.changed = false;
			this.options = options;

			General.WriteLogLine("Creating new map with configuration \"" + options.ConfigFile + "\"");

			// Initiate graphics
			General.WriteLogLine("Initializing graphics device...");
			graphics = new D3DDevice(General.MainWindow.Display);
			if(!graphics.Initialize()) return false;
			
			// Create renderers
			renderer2d = new Renderer2D(graphics);
			renderer3d = new Renderer3D(graphics);
			
			// Load game configuration
			General.WriteLogLine("Loading game configuration...");
			configinfo = General.GetConfigurationInfo(options.ConfigFile);
			config = new GameConfiguration(General.LoadGameConfiguration(options.ConfigFile));
			configinfo.ApplyDefaults(config);
			General.Editing.UpdateCurrentEditModes();
			
			// Create map data
			map = new MapSet();

			// Initialize map format interface
			General.WriteLogLine("Initializing map format interface " + config.FormatInterface + "...");
			io = MapSetIO.Create(config.FormatInterface, this);
			
			// Load data manager
			General.WriteLogLine("Loading data resources...");
			data = new DataManager();
			data.Load(CreateResourcesList());
			
			// Update structures
			options.ApplyGridSettings();
			map.UpdateConfiguration();
			map.Update();
			thingsfilter.Update();
			
			// Bind any methods
			General.Actions.BindMethods(this);

			// Set defaults
			this.visualcamera = new VisualCamera();
			General.Editing.ChangeMode(configinfo.StartMode);
			ClassicMode cmode = (General.Editing.Mode as ClassicMode);
			if(cmode != null) cmode.SetZoom(Rendering.Renderer2D.DEFAULT_ZOOM);
			renderer2d.SetViewMode((ViewMode)General.Settings.DefaultViewMode);
			
			// Success
			this.changed = false;
			General.WriteLogLine("Map creation done");
			General.MainWindow.UpdateTitle(); //mxd
			return true;
		}

		// Initializes for an existing map
		internal bool InitializeOpenMap(string filepathname, MapOptions options)
		{
			// Apply settings
			this.filename = Path.GetFileName(filepathname);
			this.filepathname = filepathname;
			this.filepath = Path.GetDirectoryName(filepathname);
			this.changed = false;
			this.options = options;

			General.WriteLogLine("Opening map \"" + this.filename + "\" with configuration \"" + options.ConfigFile + "\"");

			// Initiate graphics
			General.WriteLogLine("Initializing graphics device...");
			graphics = new D3DDevice(General.MainWindow.Display);
			if(!graphics.Initialize()) return false;

			// Create renderers
			renderer2d = new Renderer2D(graphics);
			renderer3d = new Renderer3D(graphics);

			// Load game configuration
			General.WriteLogLine("Loading game configuration...");
			configinfo = General.GetConfigurationInfo(options.ConfigFile);
			config = new GameConfiguration(General.LoadGameConfiguration(options.ConfigFile));
			configinfo.ApplyDefaults(config);
			General.Editing.UpdateCurrentEditModes();
			
			// Create map data
			bool maprestored = false; //mxd
			map = new MapSet();

			string mapname = Path.GetFileNameWithoutExtension(filepathname);
			if(!string.IsNullOrEmpty(mapname))
			{
				string hash = MurmurHash2.Hash(mapname + File.GetLastWriteTime(filepathname)).ToString();
				string backuppath = Path.Combine(General.MapRestorePath, mapname + "." + hash + ".restore");

				// Backup exists and it's newer than the map itself?
				if(File.Exists(backuppath) && File.GetLastWriteTime(backuppath) > File.GetLastWriteTime(filepathname))
				{
					if(General.ShowWarningMessage("Looks like your previous editing session has gone terribly wrong." + Environment.NewLine
						   + "Would you like to restore the map from the backup?", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						General.WriteLogLine("Initializing map format interface " + config.FormatInterface + "...");
						io = MapSetIO.Create(config.FormatInterface, this);
						General.WriteLogLine("Restoring map from \"" + backuppath + "\"...");

#if DEBUG
						// Restore map
						map.Deserialize(new MemoryStream(File.ReadAllBytes(backuppath)));
#else
						try
						{
							// Restore map
							map.Deserialize(new MemoryStream(File.ReadAllBytes(backuppath)));

							// Delete the backup
							File.Delete(backuppath);
						}
						catch(Exception e) 
						{
							General.ErrorLogger.Add(ErrorType.Error, "Unable to restore the map data structures from the backup. " + e.GetType().Name + ": " + e.Message);
							General.ShowErrorMessage("Unable to restore the map data structures from the backup.", MessageBoxButtons.OK);
							return false;
						}	
#endif
						maprestored = true;
					}
				}
			}
			
			// Read the map from file
			if(!maprestored)
			{
				map.BeginAddRemove();

				General.WriteLogLine("Initializing map format interface " + config.FormatInterface + "...");
				io = MapSetIO.Create(config.FormatInterface, this);
				General.WriteLogLine("Reading map data structures from file...");

#if !DEBUG
				try
				{
#endif
					using(FileStream stream = File.OpenRead(filepathname))
					{
						io.Read(map, stream);
					}
#if !DEBUG
				}
				catch(Exception e)
				{
					General.ErrorLogger.Add(ErrorType.Error, "Unable to read the map data structures with the specified configuration. " + e.GetType().Name + ": " + e.Message);
					General.ShowErrorMessage("Unable to read the map data structures with the specified configuration.", MessageBoxButtons.OK);
					return false;
				}
#endif

				map.EndAddRemove();
			}
			
			
			// Load data manager
			General.WriteLogLine("Loading data resources...");
			data = new DataManager();
			data.Load(CreateResourcesList());

			// Remove unused sectors
			map.RemoveUnusedSectors(true);
			
			// Update structures
			options.ApplyGridSettings();
			map.UpdateConfiguration();
			//map.SnapAllToAccuracy();
			map.Update();
			thingsfilter.Update();
			
			// Bind any methods
			General.Actions.BindMethods(this);

			// Set defaults
			this.visualcamera = new VisualCamera();
			General.Editing.ChangeMode(configinfo.StartMode);
			renderer2d.SetViewMode((ViewMode)General.Settings.DefaultViewMode);

			// Center map in screen
			if(General.Editing.Mode is ClassicMode) (General.Editing.Mode as ClassicMode).CenterInScreen();
			
			// Success
			this.changed = maprestored; //mxd
			General.WriteLogLine("Map loading done");
			General.MainWindow.UpdateTitle(); //mxd
			return true;
		}
		
		#endregion

		#region ================== Save

		// Initializes for an existing map
		internal bool SaveMap(string newfilepathname, SavePurpose purpose)
		{
			General.WriteLogLine("Saving map to " + newfilepathname);
			
			// Suspend data resources
			data.Suspend();
			
			try
			{
				// Backup existing file, if any
				if(File.Exists(newfilepathname))
				{
					if(File.Exists(newfilepathname + ".backup3")) File.Delete(newfilepathname + ".backup3");
					if(File.Exists(newfilepathname + ".backup2")) File.Move(newfilepathname + ".backup2", newfilepathname + ".backup3");
					if(File.Exists(newfilepathname + ".backup1")) File.Move(newfilepathname + ".backup1", newfilepathname + ".backup2");
					File.Copy(newfilepathname, newfilepathname + ".backup1");
				}
				
				// Kill the target file if it is different from source file
				if(newfilepathname != filepathname)
				{
					// Kill target file
					if(File.Exists(newfilepathname)) File.Delete(newfilepathname);

					// Kill .dbs settings file
					string settingsfile = newfilepathname.Substring(0, newfilepathname.Length - 4) + ".dbs";
					if(File.Exists(settingsfile)) File.Delete(settingsfile);
				}

				//mxd. Save map
				using(FileStream stream = File.OpenWrite(newfilepathname))
				{
					io.Write(map, stream);
				}
			}
			catch(IOException)
			{
				General.ShowErrorMessage("IO Error while writing target file: " + newfilepathname + ". Please make sure the location is accessible and not in use by another program.", MessageBoxButtons.OK);
				data.Resume();
				General.WriteLogLine("Map saving failed");
				return false;
			}
			catch(UnauthorizedAccessException)
			{
				General.ShowErrorMessage("Error while accessing target file: " + newfilepathname + ". Please make sure the location is accessible and not in use by another program.", MessageBoxButtons.OK);
				data.Resume();
				General.WriteLogLine("Map saving failed");
				return false;
			}

			// Resume data resources
			data.Resume();
			
			// Not saved for testing purpose?
			if(purpose != SavePurpose.Testing)
			{
				// Saved in a different file?
				if(newfilepathname != filepathname)
				{
					// Keep new filename
					filepathname = newfilepathname;
					filename = Path.GetFileName(filepathname);
					filepath = Path.GetDirectoryName(filepathname);

					// Reload resources
					ReloadResources();
				}

				try
				{
					// Open or create the map settings
					string settingsfile = newfilepathname.Substring(0, newfilepathname.Length - 4) + ".dbs";
					options.WriteConfiguration(settingsfile);
				}
				catch(Exception e)
				{
					// Warning only
					General.ErrorLogger.Add(ErrorType.Warning, "Could not write the map settings configuration file. " + e.GetType().Name + ": " + e.Message);
				}
				
				// Changes saved
				changed = false;
			}
			
			// Success!
			General.WriteLogLine("Map saving done");
			General.MainWindow.UpdateTitle(); //mxd
			return true;
		}

		//mxd. Don't save the map if it was not changed
		internal bool MapSaveRequired(string newfilepathname, SavePurpose purpose)
		{
			return (changed ||newfilepathname != filepathname || purpose != SavePurpose.Normal);
		}

		//mxd. Saves .dbs file
		internal bool SaveSettingsFile(string newfilepathname)
		{
			try
			{
				string settingsfile = newfilepathname.Substring(0, newfilepathname.Length - 4) + ".dbs";
				options.WriteConfiguration(settingsfile);
			}
			catch(Exception e)
			{
				// Warning only
				General.ErrorLogger.Add(ErrorType.Warning, "Could not write the map settings configuration file. " + e.GetType().Name + ": " + e.Message);
				return false;
			}

			return true;
		}

		//mxd
		internal void SaveMapBackup()
		{
			if(isdisposed || map == null || map.IsDisposed || string.IsNullOrEmpty(filepathname) || options == null)
			{
				General.WriteLogLine("Map backup saving failed: required structures already disposed...");
				return;
			}

#if !DEBUG
			try
			{
#endif
			string mapname = Path.GetFileNameWithoutExtension(filepathname);
			if(!string.IsNullOrEmpty(mapname))
			{
				// Make backup file path
				if(!Directory.Exists(General.MapRestorePath)) Directory.CreateDirectory(General.MapRestorePath);
				string hash = MurmurHash2.Hash(mapname + File.GetLastWriteTime(filepathname)).ToString();
				string backuppath = Path.Combine(General.MapRestorePath, mapname + "." + hash + ".restore");

				// Export map
				MemoryStream ms = map.Serialize();
				ms.Seek(0, SeekOrigin.Begin);
				//File.WriteAllBytes(backuppath, SharpCompressHelper.CompressStream(ms).ToArray());
				File.WriteAllBytes(backuppath, ms.ToArray());

				// Log it
				General.WriteLogLine("Map backup saved to \"" + backuppath + "\"");
			}
			else
			{
				// Log it
				General.WriteLogLine("Map backup saving failed: invalid map name");
			}
#if !DEBUG
			}
			catch(Exception e)
			{
				// Log it
				General.WriteLogLine("Map backup saving failed: " + e.Source + ": " + e.Message);
			}
#endif
		}
		
		#endregion

		#region ================== Selection Groups
		
		// This adds selection to a group
		private void AddSelectionToGroup(int groupindex)
		{
			General.Interface.SetCursor(Cursors.WaitCursor);
			
			// Make undo
			undoredo.CreateUndo("Assign to group " + groupindex);
			
			// Make selection
			map.AddSelectionToGroup(0x01 << groupindex);
			
			General.Interface.DisplayStatus(StatusType.Action, "Assigned selection to group " + groupindex);
			General.Interface.SetCursor(Cursors.Default);
		}
		
		// This selects a group
		private void SelectGroup(int groupindex)
		{
			// Select
			int groupmask = 0x01 << groupindex;
			map.SelectVerticesByGroup(groupmask);
			map.SelectLinedefsByGroup(groupmask);
			map.SelectSectorsByGroup(groupmask);
			map.SelectThingsByGroup(groupmask);
			
			// Redraw to show selection
			General.Interface.DisplayStatus(StatusType.Action, "Selected group " + groupindex);
			General.Interface.RedrawDisplay();
		}
		
		// Select actions
		[BeginAction("selectgroup1")] internal void SelectGroup1() { SelectGroup(0); }
		[BeginAction("selectgroup2")] internal void SelectGroup2() { SelectGroup(1); }
		[BeginAction("selectgroup3")] internal void SelectGroup3() { SelectGroup(2); }
		[BeginAction("selectgroup4")] internal void SelectGroup4() { SelectGroup(3); }
		[BeginAction("selectgroup5")] internal void SelectGroup5() { SelectGroup(4); }
		[BeginAction("selectgroup6")] internal void SelectGroup6() { SelectGroup(5); }
		[BeginAction("selectgroup7")] internal void SelectGroup7() { SelectGroup(6); }
		[BeginAction("selectgroup8")] internal void SelectGroup8() { SelectGroup(7); }
		[BeginAction("selectgroup9")] internal void SelectGroup9() { SelectGroup(8); }
		[BeginAction("selectgroup10")] internal void SelectGroup10() { SelectGroup(9); }
		
		// Assign actions
		[BeginAction("assigngroup1")] internal void AssignGroup1() { AddSelectionToGroup(0); }
		[BeginAction("assigngroup2")] internal void AssignGroup2() { AddSelectionToGroup(1); }
		[BeginAction("assigngroup3")] internal void AssignGroup3() { AddSelectionToGroup(2); }
		[BeginAction("assigngroup4")] internal void AssignGroup4() { AddSelectionToGroup(3); }
		[BeginAction("assigngroup5")] internal void AssignGroup5() { AddSelectionToGroup(4); }
		[BeginAction("assigngroup6")] internal void AssignGroup6() { AddSelectionToGroup(5); }
		[BeginAction("assigngroup7")] internal void AssignGroup7() { AddSelectionToGroup(6); }
		[BeginAction("assigngroup8")] internal void AssignGroup8() { AddSelectionToGroup(7); }
		[BeginAction("assigngroup9")] internal void AssignGroup9() { AddSelectionToGroup(8); }
		[BeginAction("assigngroup10")] internal void AssignGroup10() { AddSelectionToGroup(9); }
		
		#endregion
		
		#region ================== Methods

		// This updates everything after the configuration or settings have been changed
		internal void UpdateConfiguration()
		{
			// Update map
			map.UpdateConfiguration();

			// Update settings
			renderer3d.CreateProjection();

			// Things filters
			General.MainWindow.UpdateThingsFilters();
		}
		
		// This changes thing filter
		public void ChangeThingFilter(ThingsFilter newfilter)
		{
			// We have a special filter for null
			if(newfilter == null) newfilter = new NullThingsFilter();
			
			// Deactivate old filter
			if(thingsfilter != null) thingsfilter.Deactivate();

			// Change
			thingsfilter = newfilter;

			// Activate filter
			thingsfilter.Activate();

			// Update interface
			General.MainWindow.ReflectThingsFilter();

			// Redraw
			General.MainWindow.RedrawDisplay();
		}

		//mxd
		private DataLocationList CreateResourcesList()
		{
			DataLocationList list = new DataLocationList();

			// Add resources from engine location
			string enginepath = Path.GetDirectoryName(General.Map.ConfigSettings.TestProgram);
			if(!string.IsNullOrEmpty(enginepath))
			{

				AddResourcesFrom(enginepath, DataLocationType.RESOURCE_GRP, list);
				AddResourcesFrom(enginepath, DataLocationType.RESOURCE_ART, list);
			}

			// Add resources from map location if it differs from engine location
			if(!string.IsNullOrEmpty(filepath) && filepath != enginepath)
			{
				AddResourcesFrom(filepath, DataLocationType.RESOURCE_GRP, list);
				AddResourcesFrom(filepath, DataLocationType.RESOURCE_ART, list);
			}

			return list;
		}

		//mxd
		private static void AddResourcesFrom(string path, DataLocationType type, DataLocationList addto)
		{
			string pattern;
			switch(type)
			{
				case DataLocationType.RESOURCE_ART: pattern = "*.art"; break;
				case DataLocationType.RESOURCE_GRP: pattern = "*.grp"; break;
				default: throw new NotSupportedException("Unsupported DataLocationType!");
			}

			string[] files = Directory.GetFiles(path, pattern);
			foreach(string file in files)
			{
				addto.Add(new DataLocation(type, file));
			}
		}
		
		// This reloads resources
		[BeginAction("reloadresources")]
		internal void DoReloadResource()
		{
			// Set this to false so we can see if errors are added
			General.ErrorLogger.IsErrorAdded = false;

			ReloadResources();

			if(General.ErrorLogger.IsErrorAdded)
			{
				// Show any errors if preferred
				General.MainWindow.DisplayStatus(StatusType.Warning, "There were errors during resources loading!");
				if(General.Settings.ShowErrorsWindow) General.MainWindow.ShowErrors();
			}
			else
			{
				General.MainWindow.DisplayReady();
			}
		}

		private void ReloadResources()
		{
			// Keep old display info
			StatusInfo oldstatus = General.MainWindow.Status;
			Cursor oldcursor = Cursor.Current;
			
			// Show status
			General.MainWindow.DisplayStatus(StatusType.Busy, "Reloading data resources...");
			Cursor.Current = Cursors.WaitCursor;
			
			// Clean up
			data.Dispose();
			data = null;
			config = null;
			configinfo = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			
			// Reload game configuration
			General.WriteLogLine("Reloading game configuration...");
			configinfo = General.GetConfigurationInfo(options.ConfigFile);
			config = new GameConfiguration(General.LoadGameConfiguration(options.ConfigFile));
			General.Editing.UpdateCurrentEditModes();
			
			// Reload data resources
			General.WriteLogLine("Reloading data resources...");
			data = new DataManager();
			data.Load(CreateResourcesList());
			
			// Apply new settings to map elements
			map.UpdateConfiguration();

			// Re-link the background image
			grid.LinkBackground();
			
			// Inform all plugins that the resources are reloaded
			General.Plugins.ReloadResources();
			
			// Inform editing mode that the resources are reloaded
			if(General.Editing.Mode != null) General.Editing.Mode.OnReloadResources();
			
			// Reset status
			General.MainWindow.DisplayStatus(oldstatus);
			Cursor.Current = oldcursor;
		}

		// Game Configuration action
		[BeginAction("mapoptions")]
		internal void ShowMapOptions()
		{
			// Cancel volatile mode, if any
			General.Editing.DisengageVolatileMode();
			
			// Show map options dialog
			MapOptionsForm optionsform = new MapOptionsForm(options);
			if(optionsform.ShowDialog(General.MainWindow) == DialogResult.OK)
			{
				// Update interface
				General.MainWindow.UpdateInterface();

				// Stop data manager
				data.Dispose();
				
				// Apply new options
				this.options = optionsform.Options;

				// Load new game configuration
				General.WriteLogLine("Loading game configuration...");
				configinfo = General.GetConfigurationInfo(options.ConfigFile);
				config = new GameConfiguration(General.LoadGameConfiguration(options.ConfigFile));
				configinfo.ApplyDefaults(config);
				General.Editing.UpdateCurrentEditModes();
				
				// Setup new map format IO
				General.WriteLogLine("Initializing map format interface " + config.FormatInterface + "...");
				io = MapSetIO.Create(config.FormatInterface, this);

				// Let the plugins know
				General.Plugins.MapReconfigure();
				
				// Update interface
				General.MainWindow.SetupInterface();
				General.MainWindow.UpdateThingsFilters();
				General.MainWindow.UpdateInterface();
				
				// Reload resources
				ReloadResources();
				
				// Done
				General.MainWindow.DisplayReady();
			}

			// Done
			optionsform.Dispose();
		}

		// This shows the things filters setup
		[BeginAction("thingsfilterssetup")]
		internal void ShowThingsFiltersSetup()
		{
			// Show things filter dialog
			ThingsFiltersForm f = new ThingsFiltersForm();
			f.ShowDialog(General.MainWindow);
			f.Dispose();
			General.MainWindow.UpdateThingsFilters();
		}
		
		// This returns true is the given type matches
		/*public bool IsType(Type t)
		{
			return io.GetType().Equals(t);
		}*/

		#endregion
	}
}
