
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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using mxd.DukeBuilder.Actions;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Editing;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Plugins;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.Types;
using mxd.DukeBuilder.Windows;
using SlimDX.Direct3D9;

#endregion

namespace mxd.DukeBuilder
{
	public static class General
	{
		#region ================== API Declarations

		[DllImport("user32.dll")]
		internal static extern bool LockWindowUpdate(IntPtr hwnd);

		[DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
		internal static extern void ZeroMemory(IntPtr dest, int size);

		[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
		internal static extern int SendMessage(IntPtr hwnd, uint Msg, int wParam, int lParam);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MessageBeep(MessageBeepType type);

		#endregion

		#region ================== Constants

		// SendMessage API
		internal const int WM_USER = 0x400;
		internal const int WM_SYSCOMMAND = 0x112;
		internal const int SC_KEYMENU = 0xF100;
		//internal const int CB_SETITEMHEIGHT = 0x153;
		
		// Files and Folders
		private const string SETTINGS_FILE = "DukeBuilder.cfg";
		private const string DEFAULT_SETTINGS_FILE = "DukeBuilder.default.cfg"; //mxd
		private const string SETTINGS_DIR = "Duke Builder";
		private const string LOG_FILE = "DukeBuilder.log";
		private const string GAME_CONFIGS_DIR = "Configurations";
		private const string GAME_CONFIG_ICONS_DIR = "Configurations\\Icons";
		private const string PLUGINS_DIR = "Plugins";
		private const string SETUP_DIR = "Setup";
		private const string HELP_FILE = "Refmanual.chm";
		private const string MAP_RESTORE_DIR = "Restore"; //mxd

		#endregion

		#region ================== Variables

		// Files and Folders
		private static string apppath;
		private static string setuppath;
		private static string settingspath;
		private static string restorepath; //mxd
		private static string logfile;
		private static string temppath;
		private static string configspath;
		private static string configiconspath;
		private static string pluginspath;
		
		// Main objects
		private static Assembly thisasm;
		private static MainForm mainwindow;
		private static ProgramConfiguration settings;
		private static MapManager map;
		private static EditingManager editing;
		private static ActionManager actions;
		private static PluginManager plugins;
		private static ColorCollection colors;
		private static TypesManager types;
		private static Clock clock;
		private static ErrorLogger errorlogger;
		private static Mutex appmutex;
		
		// Configurations
		private static List<ConfigurationInfo> configs;
		
		// States
		private static bool debugbuild;

		#endregion

		#region ================== Properties

		public static Assembly ThisAssembly { get { return thisasm; } }
		public static string AppPath { get { return apppath; } }
		public static string TempPath { get { return temppath; } }
		public static string ConfigsPath { get { return configspath; } }
		internal static string SettingsPath { get { return settingspath; } } //mxd
		internal static string MapRestorePath { get { return restorepath; } } //mxd
		internal static string LogFile { get { return logfile; } } //mxd
		public static string ConfigIconsPath { get { return configiconspath; } }
		public static string PluginsPath { get { return pluginspath; } }
		internal static MainForm MainWindow { get { return mainwindow; } }
		public static IMainForm Interface { get { return mainwindow; } }
		public static ProgramConfiguration Settings { get { return settings; } }
		public static ColorCollection Colors { get { return colors; } }
		internal static List<ConfigurationInfo> Configs { get { return configs; } }
		public static MapManager Map { get { return map; } }
		public static ActionManager Actions { get { return actions; } }
		internal static PluginManager Plugins { get { return plugins; } }
		public static Clock Clock { get { return clock; } }
		public static bool DebugBuild { get { return debugbuild; } }
		internal static TypesManager Types { get { return types; } }
		public static EditingManager Editing { get { return editing; } }
		public static ErrorLogger ErrorLogger { get { return errorlogger; } }
		
		#endregion

		#region ================== Configurations

		// This returns the game configuration info by filename
		internal static ConfigurationInfo GetConfigurationInfo(string filename)
		{
			// Go for all config infos
			foreach(ConfigurationInfo ci in configs)
			{
				// Check if filename matches
				if(string.Compare(Path.GetFileNameWithoutExtension(ci.Filename),
								  Path.GetFileNameWithoutExtension(filename), true) == 0)
				{
					// Return this info
					return ci;
				}
			}

			// None found
			return null;
		}

		// This loads and returns a game configuration
		internal static Configuration LoadGameConfiguration(string filename)
		{
			// Make the full filepathname
			string filepathname = Path.Combine(configspath, filename);
			
			// Load configuration
			try
			{
				// Try loading the configuration
				Configuration cfg = new Configuration(filepathname, true);

				// Check for erors
				if(cfg.ErrorResult)
				{
					// Error in configuration
					errorlogger.Add(ErrorType.Error, "Unable to load the game configuration file \"" + filename + "\". " +
													 "Error in file \"" + cfg.ErrorFile + "\" near line " + cfg.ErrorLine + ": " + cfg.ErrorDescription);
					return null;
				}

				// Check if this is a Duke Builder config
				if(cfg.ReadSetting("type", "") != "Duke Builder Game Configuration")
				{
					// Old configuration
					errorlogger.Add(ErrorType.Error, "Unable to load the game configuration file \"" + filename + "\". " +
													 "This configuration is not a Duke Builder game configuration.");
					return null;
				}

				// Return config
				return cfg;
			}
			catch(Exception e)
			{
				// Unable to load configuration
				errorlogger.Add(ErrorType.Error, "Unable to load the game configuration file \"" + filename + "\". " + e.GetType().Name + ": " + e.Message);
				WriteLog(e.StackTrace);
				return null;
			}
		}

		// This loads all game configurations
		private static void LoadAllGameConfigurations()
		{
			// Display status
			mainwindow.DisplayStatus(StatusType.Busy, "Loading game configurations...");

			// Make array
			configs = new List<ConfigurationInfo>();

			// Go for all cfg files in the configurations directory
			string[] filenames = Directory.GetFiles(configspath, "*.cfg", SearchOption.TopDirectoryOnly);
			foreach(string filepath in filenames)
			{
				// Check if it can be loaded
				Configuration cfg = LoadGameConfiguration(Path.GetFileName(filepath));
				if(cfg != null)
				{
					string fullfilename = Path.GetFileName(filepath);
					ConfigurationInfo cfginfo = new ConfigurationInfo(cfg, fullfilename);
					
					// Add to lists
					WriteLogLine("Registered game configuration \"" + cfginfo.Name + "\" from \"" + fullfilename + "\"");
					configs.Add(cfginfo);
				}
			}

			// Sort the list
			configs.Sort();
		}
		
		#endregion

		#region ================== Startup

		// Main program entry
		[STAThread]
		internal static void Main(string[] args)
		{
			// Determine states
			#if DEBUG
				debugbuild = true;
			#else
				debugbuild = false;
				//mxd. Custom exception dialog.
				AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
				Application.ThreadException += Application_ThreadException;
			#endif

			// Enable OS visual styles
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false); //mxd
			//Application.DoEvents();		// This must be here to work around a .NET bug

			//mxd. Set CultureInfo
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			
			// Hook to DLL loading failure event
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			
			// Set current thread name
			Thread.CurrentThread.Name = "Main Application";

			// Application is running
			appmutex = new Mutex(false, "dukebuilder");
			
			// Get a reference to this assembly
			thisasm = Assembly.GetExecutingAssembly();
			//thisversion = thisasm.GetName().Version;
			
			// Find application path
			Uri localpath = new Uri(Path.GetDirectoryName(thisasm.GetName().CodeBase));
			apppath = Uri.UnescapeDataString(localpath.AbsolutePath);
			
			// Setup directories
			temppath = Path.GetTempPath();
			setuppath = Path.Combine(apppath, SETUP_DIR);
			settingspath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SETTINGS_DIR);
			restorepath = Path.Combine(settingspath, MAP_RESTORE_DIR); //mxd
			configspath = Path.Combine(apppath, GAME_CONFIGS_DIR);
			configiconspath = Path.Combine(apppath, GAME_CONFIG_ICONS_DIR); //mxd
			pluginspath = Path.Combine(apppath, PLUGINS_DIR);
			logfile = Path.Combine(settingspath, LOG_FILE);
			
			// Make program settings directory if missing
			if(!Directory.Exists(settingspath)) Directory.CreateDirectory(settingspath);
			
			// Remove the previous log file and start logging
			if(File.Exists(logfile)) File.Delete(logfile);
			WriteLogLine("Duke Builder R" + thisasm.GetName().Version.Revision + " startup");
			WriteLogLine("Application path:        " + apppath);
			WriteLogLine("Temporary path:          " + temppath);
			WriteLogLine("Local settings path:     " + settingspath);
			
			// Load configuration
			WriteLogLine("Loading program configuration...");
			settings = new ProgramConfiguration();
			string defaultsettingsfile = Path.Combine(apppath, DEFAULT_SETTINGS_FILE);
			string usersettingsfile = Path.Combine(settingspath, SETTINGS_FILE);
			if(settings.Load(usersettingsfile, defaultsettingsfile))
			{
				// Create error logger
				errorlogger = new ErrorLogger();
				
				// Create action manager
				actions = new ActionManager();
				
				// Bind static methods to actions
				actions.BindMethods(typeof(General));

				// Initialize static classes
				//MapSet.Initialize();
				//ilInit();

				// Create main window
				WriteLogLine("Loading main interface window...");
				mainwindow = new MainForm();
				mainwindow.SetupInterface();
				mainwindow.UpdateInterface();
				mainwindow.UpdateThingsFilters();

				// Show main window
				WriteLogLine("Showing main interface window...");
				mainwindow.Show();
				mainwindow.Update();
				
				// Start Direct3D
				WriteLogLine("Starting Direct3D graphics driver...");
				try { D3DDevice.Startup(); }
				catch(Direct3D9NotFoundException) { AskDownloadDirectX(); return; }
				catch(Direct3DX9NotFoundException) { AskDownloadDirectX(); return; }
				
				// Load plugin manager
				WriteLogLine("Loading plugins...");
				plugins = new PluginManager();
				plugins.LoadAllPlugins();
				
				// Load game configurations
				WriteLogLine("Loading game configurations...");
				LoadAllGameConfigurations();

				// Create editing modes
				WriteLogLine("Creating editing modes manager...");
				editing = new EditingManager();
				
				// Now that all settings have been combined (core & plugins) apply the defaults
				WriteLogLine("Applying configuration settings...");
				actions.ApplyDefaultShortcutKeys();
				mainwindow.ApplyShortcutKeys();
				foreach(ConfigurationInfo info in configs) info.ApplyDefaults(null);
				
				// Load color settings
				WriteLogLine("Loading color settings...");
				colors = new ColorCollection(settings.Config);
				
				// Create application clock
				WriteLogLine("Creating application clock...");
				clock = new Clock();
				
				// Create types manager
				WriteLogLine("Creating types manager...");
				types = new TypesManager();
				
				// All done
				WriteLogLine("Startup done");
				mainwindow.DisplayReady();
				
				// Show any errors if preferred
				if(errorlogger.IsErrorAdded)
				{
					mainwindow.DisplayStatus(StatusType.Warning, "There were errors during program statup!");
					if(settings.ShowErrorsWindow) mainwindow.ShowErrors();
				}
				
				// Run application from the main window
				Application.Run(mainwindow);
			}
			else
			{
				// Terminate
				Terminate(false);
			}
		}

		// This handles DLL linking errors
		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			// Check if SlimDX failed loading
			if(args.Name.Contains("SlimDX")) AskDownloadDirectX();

			// Return null
			return null;
		}
		
		// This asks the user to download DirectX
		private static void AskDownloadDirectX()
		{
			// Cancel loading map from command-line parameters, if any.
			// This causes problems, because when the window is shown, the map will
			// be loaded and DirectX is initialized (which we seem to be missing)
			//CancelAutoMapLoad();
			
			// Ask the user to download DirectX
			if(MessageBox.Show("This application requires the latest version of Microsoft DirectX installed on your computer." + Environment.NewLine +
				"Do you want to install and/or update Microsoft DirectX now?", "DirectX Error", MessageBoxButtons.YesNo,
				MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				// Open DX web setup
				//System.Diagnostics.Process.Start("http://www.microsoft.com/downloads/details.aspx?FamilyId=2DA43D38-DB71-4C1B-BC6A-9B6652CD92A3").WaitForExit(1000);
				Process.Start(Path.Combine(setuppath, "dxwebsetup.exe")).WaitForExit(1000);
			}

			// End program here
			Terminate(false);
		}

		// This parses the command line arguments
		/*private static void ParseCommandLineArgs(string[] args)
		{
			autoloadresources = new DataLocationList();
			
			// Keep a copy
			cmdargs = args;
			
			// Make a queue so we can parse the values from left to right
			Queue<string> argslist = new Queue<string>(args);
			
			// Parse list
			while(argslist.Count > 0)
			{
				// Get next arg
				string curarg = argslist.Dequeue();
				
				// Delay window?
				if(string.Compare(curarg, "-DELAYWINDOW", true) == 0)
				{
					// Delay showing the main window
					delaymainwindow = true;
				}
				// No settings?
				else if(string.Compare(curarg, "-NOSETTINGS", true) == 0)
				{
					// Don't load or save program settings
					nosettings = true;
				}
				// Map name info?
				else if(string.Compare(curarg, "-MAP", true) == 0)
				{
					// Store next arg as map name information
					autoloadmap = argslist.Dequeue();
				}
				// Config name info?
				else if((string.Compare(curarg, "-CFG", true) == 0) ||
						(string.Compare(curarg, "-CONFIG", true) == 0))
				{
					// Store next arg as config filename information
					autoloadconfig = argslist.Dequeue();
				}
				// Strict patches rules?
				else if(string.Compare(curarg, "-STRICTPATCHES", true) == 0)
				{
					autoloadstrictpatches = true;
				}
				// Resource?
				else if(string.Compare(curarg, "-RESOURCE", true) == 0)
				{
					DataLocation dl = new DataLocation();

					// Parse resource type
					string resourcetype = argslist.Dequeue();
					if(string.Compare(resourcetype, "WAD", true) == 0)
						dl.type = DataLocation.RESOURCE_WAD;
					else if(string.Compare(resourcetype, "DIR", true) == 0)
						dl.type = DataLocation.RESOURCE_DIRECTORY;
					else if(string.Compare(resourcetype, "PK3", true) == 0)
						dl.type = DataLocation.RESOURCE_PK3;
					else
					{
						WriteLogLine("Unexpected resource type \"" + resourcetype + "\" in program parameters. Expected \"wad\", \"dir\" or \"pk3\".");
						break;
					}

					// We continue parsing args until an existing filename is found
					// all other arguments must be one of the optional keywords.
					while(string.IsNullOrEmpty(dl.location))
					{
						curarg = argslist.Dequeue();

						if((string.Compare(curarg, "ROOTTEXTURES", true) == 0) &&
						   (dl.type == DataLocation.RESOURCE_DIRECTORY))
						{
							// Load images in the root directory of the resource as textures
							dl.option1 = true;
						}
						else if((string.Compare(curarg, "ROOTFLATS", true) == 0) &&
								(dl.type == DataLocation.RESOURCE_DIRECTORY))
						{
							// Load images in the root directory of the resource as flats
							dl.option2 = true;
						}
						else if((string.Compare(curarg, "STRICTPATCHES", true) == 0) &&
								(dl.type == DataLocation.RESOURCE_WAD))
						{
							// Use strict rules for patches
							dl.option1 = true;
						}
						else if(string.Compare(curarg, "NOTEST", true) == 0)
						{
							// Exclude this resource from testing parameters
							dl.notfortesting = true;
						}
						else
						{
							// This must be an existing file, or it is an invalid argument
							if(dl.type == DataLocation.RESOURCE_DIRECTORY)
							{
								if(Directory.Exists(curarg))
									dl.location = curarg;
							}
							else
							{
								if(File.Exists(curarg))
									dl.location = curarg;
							}
							
							if(string.IsNullOrEmpty(dl.location))
							{
								General.WriteLogLine("Unexpected argument \"" + curarg + "\" in program parameters. Expected a valid resource option or a resource filename.");
								break;
							}
						}
					}

					// Add resource to list
					if(!string.IsNullOrEmpty(dl.location))
						autoloadresources.Add(dl);
				}
				// Every other arg
				else
				{
					// No command to load file yet?
					if(autoloadfile == null)
					{
						// Check if this is a file we can load
						if(File.Exists(curarg))
						{
							// Load this file!
							autoloadfile = curarg.Trim();
						}
						else
						{
							// Note in the log that we cannot find this file
							WriteLogLine("Cannot find the specified file \"" + curarg + "\"");
						}
					}
				}
			}
		}*/
		
		// This cancels automatic map loading
		/*internal static void CancelAutoMapLoad()
		{
			autoloadfile = null;
		}*/
		
		#endregion
		
		#region ================== Terminate

		// This is for plugins to use
		public static void Exit(bool properexit)
		{
			// Plugin wants to exit nicely?
			if(properexit)
			{
				// Close dialog forms first
				while((Form.ActiveForm != mainwindow) && (Form.ActiveForm != null))
					Form.ActiveForm.Close();

				// Close main window
				mainwindow.Close();
			}
			else
			{
				// Terminate, no questions asked
				Terminate(true);
			}
		}
		
		// This terminates the program
		internal static void Terminate(bool properexit)
		{
			// Terminate properly?
			if(properexit)
			{
				WriteLogLine("Termination requested");
				
				// Unbind static methods from actions
				actions.UnbindMethods(typeof(General));
				
				// Save colors
				colors.SaveColors(settings.Config);
				
				// Save action controls
				actions.SaveSettings();
				
				// Save game configuration settings
				foreach(ConfigurationInfo ci in configs) ci.SaveSettings();
				
				// Save settings configuration
				//if(!General.NoSettings)
				//{
					WriteLogLine("Saving program configuration...");
					settings.Save(Path.Combine(settingspath, SETTINGS_FILE));
				//}
				
				// Clean up
				if(map != null) map.Dispose(); map = null;
				if(editing != null) editing.Dispose(); editing = null;
				if(mainwindow != null) mainwindow.Dispose();
				//if(actions != null) actions.Dispose();
				if(clock != null) clock.Dispose();
				if(plugins != null) plugins.Dispose();
				if(types != null) types.Dispose();
				try { D3DDevice.Terminate(); } catch { }

				// Application ends here and now
				WriteLogLine("Termination done");
				Application.Exit();
			}
			else
			{
				// Just end now
				WriteLogLine("Immediate program termination");
				Application.Exit();
			}

			// Die.
			Process.GetCurrentProcess().Kill();
		}
		
		#endregion
		
		#region ================== Management
		
		// This creates a new map
		[BeginAction("newmap")]
		internal static void NewMap()
		{
			MapOptions newoptions = new MapOptions();

			// Cancel volatile mode, if any
			Editing.DisengageVolatileMode();
			
			// Ask the user to save changes (if any)
			if(AskSaveMap())
			{
				// Open map options dialog
				MapOptionsForm optionswindow = new MapOptionsForm(newoptions) { IsForNewMap = true };
				if(optionswindow.ShowDialog(mainwindow) == DialogResult.OK)
				{
					// Display status
					mainwindow.DisplayStatus(StatusType.Busy, "Creating new map...");
					Cursor.Current = Cursors.WaitCursor;
					
					// Clear the display
					mainwindow.ClearDisplay();

					// Trash the current map, if any
					if(map != null) map.Dispose();
					
					// Let the plugins know
					plugins.OnMapNewBegin();

					// Set this to false so we can see if errors are added
					ErrorLogger.IsErrorAdded = false;
					
					// Create map manager with given options
					map = new MapManager();
					if(map.InitializeNewMap(newoptions))
					{
						map.Config.FindDefaultDrawSettings();

						// Let the plugins know
						plugins.OnMapNewEnd();

						// All done
						mainwindow.SetupInterface();
						mainwindow.RedrawDisplay();
						mainwindow.UpdateThingsFilters();
						mainwindow.UpdateInterface();
						mainwindow.HideInfo();
					}
					else
					{
						// Unable to create map manager
						map.Dispose();
						map = null;

						// Show splash logo on display
						mainwindow.ShowSplashDisplay();
					}

					if(errorlogger.IsErrorAdded)
					{
						// Show any errors if preferred
						mainwindow.DisplayStatus(StatusType.Warning, "There were errors during loading!");
						if(settings.ShowErrorsWindow) mainwindow.ShowErrors();
					}
					else
					{
						mainwindow.DisplayReady();
					}
					
					Cursor.Current = Cursors.Default;
				}
			}
		}

		// This closes the current map
		[BeginAction("closemap")]
		internal static void ActionCloseMap() { CloseMap(); }
		internal static bool CloseMap()
		{
			// Cancel volatile mode, if any
			Editing.DisengageVolatileMode();

			// Ask the user to save changes (if any)
			if(AskSaveMap())
			{
				// Display status
				mainwindow.DisplayStatus(StatusType.Busy, "Closing map...");
				WriteLogLine("Unloading map...");
				Cursor.Current = Cursors.WaitCursor;
				
				// Trash the current map
				if(map != null) map.Dispose();
				map = null;
				
				// Clear errors
				ErrorLogger.Clear();
				
				// Show splash logo on display
				mainwindow.ShowSplashDisplay();
				
				// Done
				Cursor.Current = Cursors.Default;
				editing.UpdateCurrentEditModes();
				mainwindow.SetupInterface();
				mainwindow.RedrawDisplay();
				mainwindow.HideInfo();
				mainwindow.UpdateThingsFilters();
				mainwindow.UpdateInterface();
				mainwindow.DisplayReady();
				WriteLogLine("Map unload done");
				return true;
			}

			// User cancelled
			return false;
		}

		// This loads a map from file
		[BeginAction("openmap")]
		internal static void OpenMap()
		{
			// Cancel volatile mode, if any
			Editing.DisengageVolatileMode();

			// Open map file dialog
			OpenFileDialog openfile = new OpenFileDialog();
			openfile.Filter = "Build MAP Files (*.map)|*.map";
			openfile.Title = "Open Map";
			openfile.AddExtension = false;
			openfile.CheckFileExists = true;
			openfile.Multiselect = false;
			openfile.ValidateNames = true;
			if(openfile.ShowDialog(mainwindow) == DialogResult.OK)
			{
				// Update main window
				mainwindow.Update();

				// Open map file
				OpenMapFile(openfile.FileName);
			}

			openfile.Dispose();
		}
		
		// This opens the specified file
		internal static void OpenMapFile(string filename)
		{
			// Cancel volatile mode, if any
			Editing.DisengageVolatileMode();
			
			// Ask the user to save changes (if any)
			if(AskSaveMap())
			{
				//mxd. Create map options
				MapOptions options = null;
				string dbsfile = filename.Substring(0, filename.Length - 4) + ".dbs";
				if(File.Exists(dbsfile))
				{
					try { options = new MapOptions(new Configuration(dbsfile, true)); }
					catch(Exception e)
					{
						ErrorLogger.Add(ErrorType.Error, "Failed to load map settings file \"" + dbsfile + "\". " + e.GetType().Name + ": " + e.Message);
					}
				}
				if(options == null) options = new MapOptions();
				
				// Open map options dialog
				MapOptionsForm openmapwindow = new MapOptionsForm(options);
				if(openmapwindow.ShowDialog(mainwindow) == DialogResult.OK)
					OpenMapFileWithOptions(filename, openmapwindow.Options);
			}
		}
		
		// This opens the specified file without dialog
		private static void OpenMapFileWithOptions(string filename, MapOptions options)
		{
			// Display status
			mainwindow.DisplayStatus(StatusType.Busy, "Opening map file...");
			Cursor.Current = Cursors.WaitCursor;

			// Clear the display
			mainwindow.ClearDisplay();

			// Trash the current map, if any
			if(map != null) map.Dispose();

			// Let the plugins know
			plugins.OnMapOpenBegin();

			// Set this to false so we can see if errors are added
			ErrorLogger.IsErrorAdded = false;

			// Create map manager with given options
			map = new MapManager();
			if(map.InitializeOpenMap(filename, options))
			{
				map.Config.FindDefaultDrawSettings();

				// Add recent file
				mainwindow.AddRecentFile(filename);

				// Let the plugins know
				plugins.OnMapOpenEnd();

				// All done
				mainwindow.SetupInterface();
				mainwindow.RedrawDisplay();
				mainwindow.UpdateThingsFilters();
				mainwindow.UpdateInterface();
				mainwindow.HideInfo();
			}
			else
			{
				// Unable to create map manager
				map.Dispose();
				map = null;

				// Show splash logo on display
				mainwindow.ShowSplashDisplay();
			}

			if(errorlogger.IsErrorAdded)
			{
				// Show any errors if preferred
				mainwindow.DisplayStatus(StatusType.Warning, "There were errors during loading!");
				if(Settings.ShowErrorsWindow) mainwindow.ShowErrors();
			}
			else
			{
				mainwindow.DisplayReady();
			}
			
			Cursor.Current = Cursors.Default;
		}
		
		// This saves the current map
		// Returns tre when saved, false when cancelled or failed
		[BeginAction("savemap")]
		internal static void ActionSaveMap() { SaveMap(); }
		internal static bool SaveMap()
		{
			if(map == null) return false;
			bool result = false;
			
			// Cancel volatile mode, if any
			editing.DisengageVolatileMode();
			
			// Check if a wad file is known
			if(map.FilePathName == "")
			{
				// Call to SaveMapAs
				result = SaveMapAs();
			}
			else
			{
				//mxd. Do we need to save the map?
				if(!map.MapSaveRequired(map.FilePathName, SavePurpose.Normal))
				{
					// Still save settings file
					result = map.SaveSettingsFile(map.FilePathName);

					// Display status
					mainwindow.DisplayStatus(StatusType.Info, "Map is up to date. Updated map settings file.");

					// All done
					mainwindow.UpdateInterface();
					return result;
				}
				
				// Display status
				mainwindow.DisplayStatus(StatusType.Busy, "Saving map file...");
				Cursor.Current = Cursors.WaitCursor;

				// Set this to false so we can see if errors are added
				ErrorLogger.IsErrorAdded = false;
				
				// Save the map
				Plugins.OnMapSaveBegin(SavePurpose.Normal);
				if(map.SaveMap(map.FilePathName, SavePurpose.Normal))
				{
					// Add recent file
					mainwindow.AddRecentFile(map.FilePathName);
					result = true;
				}
				Plugins.OnMapSaveEnd(SavePurpose.Normal);

				// All done
				mainwindow.UpdateInterface();

				if(errorlogger.IsErrorAdded)
				{
					// Show any errors if preferred
					mainwindow.DisplayStatus(StatusType.Warning, "There were errors during saving!");
					if(Settings.ShowErrorsWindow) mainwindow.ShowErrors();
				}
				else
				{
					mainwindow.DisplayStatus(StatusType.Info, "Map saved in " + map.FileName + ".");
				}

				Cursor.Current = Cursors.Default;
			}

			return result;
		}


		// This saves the current map as a different file
		// Returns tre when saved, false when cancelled or failed
		[BeginAction("savemapas")]
		internal static void ActionSaveMapAs() { SaveMapAs(); }
		internal static bool SaveMapAs()
		{
			if(map == null) return false;
			bool result = false;

			// Cancel volatile mode, if any
			editing.DisengageVolatileMode();

			// Show save as dialog
			SaveFileDialog savefile = new SaveFileDialog
			                          {
				                          Filter = "Build MAP Files (*.map)|*.map", 
										  Title = "Save Map As", 
										  AddExtension = true, 
										  CheckPathExists = true, 
										  OverwritePrompt = true, 
										  ValidateNames = true, 
										  FileName = (!string.IsNullOrEmpty(map.FileName) ? Path.GetFileNameWithoutExtension(map.FileName) : "E1L1")
			                          };

			if(savefile.ShowDialog(mainwindow) == DialogResult.OK)
			{
				// Check if we're saving to the same file as the original.
				// Because some muppets use Save As even when saving to the same file.
				string currentfilename = (map.FilePathName.Length > 0) ? Path.GetFullPath(map.FilePathName).ToLowerInvariant() : "";
				string savefilename = Path.GetFullPath(savefile.FileName).ToLowerInvariant();
				if(currentfilename == savefilename)
				{
					SaveMap();
				}
				else
				{
					// Display status
					mainwindow.DisplayStatus(StatusType.Busy, "Saving map file...");
					Cursor.Current = Cursors.WaitCursor;
					
					// Set this to false so we can see if errors are added
					errorlogger.IsErrorAdded = false;
					
					// Save the map
					plugins.OnMapSaveBegin(SavePurpose.AsNewFile);
					if(map.SaveMap(savefile.FileName, SavePurpose.AsNewFile))
					{
						// Add recent file
						mainwindow.AddRecentFile(map.FilePathName);
						result = true;
					}
					plugins.OnMapSaveEnd(SavePurpose.AsNewFile);
					
					// All done
					mainwindow.UpdateInterface();
					
					if(errorlogger.IsErrorAdded)
					{
						// Show any errors if preferred
						mainwindow.DisplayStatus(StatusType.Warning, "There were errors during saving!");
						if(settings.ShowErrorsWindow) mainwindow.ShowErrors();
					}
					else
					{
						mainwindow.DisplayStatus(StatusType.Info, "Map saved in " + map.FileName + ".");
					}
					
					Cursor.Current = Cursors.Default;
				}
			}
			
			savefile.Dispose();
			return result;
		}
		
		// This asks to save the map if needed
		// Returns false when action was cancelled
		internal static bool AskSaveMap()
		{
			// Map open and not saved?
			if(map != null && map.IsChanged)
			{
				// Ask to save changes
				DialogResult result = MessageBox.Show(mainwindow, "Do you want to save changes to " + map.FileName + "?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				// Save map
				if(result == DialogResult.Yes) return SaveMap();

				// Abort
				if(result == DialogResult.Cancel) return false;
			}

			return true;
		}
		
		#endregion

		#region ================== Debug
		
		// This shows a major failure
		public static void Fail(string message)
		{
			WriteLogLine("FAIL: " + message);
			Debug.Fail(message);
		}
		
		// This outputs log information
		public static void WriteLogLine(string line)
		{
			// Output to console
			Console.WriteLine(line);
			
			// Write to log file
			try { File.AppendAllText(logfile, line + Environment.NewLine); }
			catch(Exception) { }
		}

		// This outputs log information
		public static void WriteLog(string text)
		{
			// Output to console
			Console.Write(text);

			// Write to log file
			try { File.AppendAllText(logfile, text); }
			catch(Exception) { }
		}
		
		#endregion

		#region ================== Tools
		
		// This swaps two pointers
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// This calculates the bits needed for a number
		public static int BitsForInt(int v)
		{
			int[] LOGTABLE = new[] {
			  0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3,
			  4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
			  5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			  5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			  6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			  6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			  6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			  6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 };
			
			int r;	// r will be lg(v)
			int t, tt;

			if(Int2Bool(tt = v >> 16))
			{
				r = Int2Bool(t = tt >> 8) ? 24 + LOGTABLE[t] : 16 + LOGTABLE[tt];
			}
			else
			{
				r = Int2Bool(t = v >> 8) ? 8 + LOGTABLE[t] : LOGTABLE[v];
			}
			
			return r;
		}
		
		// This clamps a value
		public static float Clamp(float value, float min, float max)
		{
			return Math.Min(Math.Max(min, value), max);
		}

		// This clamps a value
		public static int Clamp(int value, int min, int max)
		{
			return Math.Min(Math.Max(min, value), max);
		}

		// This clamps a value
		public static byte Clamp(byte value, byte min, byte max)
		{
			return Math.Min(Math.Max(min, value), max);
		}

		//mxd. This wraps a value
		public static int Wrap(int value, int min, int max)
		{
			value = value % max;
			if(value < min) value += max;
			return value;
		}

		//mxd. This wraps a value
		public static float Wrap(float value, float min, float max)
		{
			value = value % max;
			if(value < min) value += max;
			return value;
		}

		//mxd. This wraps an angle in degrees between 0 and 359
		public static int WrapAngle(int angle)
		{
			angle %= 360;
			if(angle < 0) angle += 360;
			return angle;
		}
		
		// This returns an element from a collection by index
		public static T GetByIndex<T>(ICollection<T> collection, int index)
		{
			IEnumerator<T> e = collection.GetEnumerator();
			for(int i = -1; i < index; i++) e.MoveNext();
			return e.Current;
		}

		//mxd. This returns the first element from a collection
		public static T GetFirst<T>(ICollection<T> collection)
		{
			IEnumerator<T> e = collection.GetEnumerator();
			e.MoveNext();
			return e.Current;
		}

		//mxd. This returns the next power of 2. Taken from http://bits.stephan-brumme.com/roundUpToNextPowerOfTwo.html
		public static int NextPowerOf2(int x)
		{
			x--;
			x |= x >> 1;  // handle  2 bit numbers
			x |= x >> 2;  // handle  4 bit numbers
			x |= x >> 4;  // handle  8 bit numbers
			x |= x >> 8;  // handle 16 bit numbers
			x |= x >> 16; // handle 32 bit numbers
			x++;

			return x;
		}
		
		// Convert bool to integer
		internal static int Bool2Int(bool v)
		{
			return v ? 1 : 0;
		}

		// Convert integer to bool
		internal static bool Int2Bool(int v)
		{
			return (v != 0);
		}

		//mxd. Converts a string containing space-separated integers to HashSet<int>
		public static bool GetNumbersFromString(string str, HashSet<int> target)
		{
			// Parse numbers...
			bool success = true;
			if(!string.IsNullOrEmpty(str))
			{
				string[] parts = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				foreach(string part in parts)
				{
					int num;
					if(int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
					{
						target.Add(num);
					}
					else
					{
						success = false;
					}
				}
			}

			return success;
		}

		//mxd. Converts a string containing space-separated integers to List<int>
		public static bool GetNumbersFromString(string str, List<int> target, bool allowduplicates)
		{
			// Parse numbers...
			bool success = true;
			if(!string.IsNullOrEmpty(str))
			{
				string[] parts = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				foreach(string part in parts)
				{
					int num;
					if(int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
					{
						if(!allowduplicates && target.Contains(num))
						{
							success = false;
							continue;
						}

						target.Add(num);
					}
					else
					{
						success = false;
					}
				}
			}

			return success;
		} 

		// This shows a message and logs the message
		public static DialogResult ShowErrorMessage(string message, MessageBoxButtons buttons)
		{
			// Log the message
			WriteLogLine(message);
			
			// Use normal cursor
			Cursor oldcursor = Cursor.Current;
			Cursor.Current = Cursors.Default;
			
			// Show message
			IWin32Window window = null;
			if((Form.ActiveForm != null) && Form.ActiveForm.Visible) window = Form.ActiveForm;
			DialogResult result = MessageBox.Show(window, message, Application.ProductName, buttons, MessageBoxIcon.Error);

			// Restore old cursor
			Cursor.Current = oldcursor;
			
			// Return result
			return result;
		}

		// This shows a message and logs the message
		public static DialogResult ShowWarningMessage(string message, MessageBoxButtons buttons)
		{
			return ShowWarningMessage(message, buttons, MessageBoxDefaultButton.Button1);
		}

		// This shows a message and logs the message
		public static DialogResult ShowWarningMessage(string message, MessageBoxButtons buttons, MessageBoxDefaultButton defaultbutton)
		{
			// Log the message
			WriteLogLine(message);

			// Use normal cursor
			Cursor oldcursor = Cursor.Current;
			Cursor.Current = Cursors.Default;

			// Show message
			IWin32Window window = null;
			if((Form.ActiveForm != null) && Form.ActiveForm.Visible) window = Form.ActiveForm;
			DialogResult result = MessageBox.Show(window, message, Application.ProductName, buttons, MessageBoxIcon.Warning, defaultbutton);

			// Restore old cursor
			Cursor.Current = oldcursor;

			// Return result
			return result;
		}

		// This shows the reference manual
		public static void ShowHelp(string pagefile)
		{
			ShowHelp(pagefile, HELP_FILE);
		}

		// This shows the reference manual
		public static void ShowHelp(string pagefile, string chmfile)
		{
			// Check if the file can be found in the root
			string filepathname = Path.Combine(apppath, chmfile);
			if(!File.Exists(filepathname))
			{
				// Check if the file exists in the plugins directory
				filepathname = Path.Combine(pluginspath, chmfile);
				if(!File.Exists(filepathname))
				{
					// Fail
					WriteLogLine("ERROR: Can't find the help file \"" + chmfile + "\"");
					return;
				}
			}
			
			// Show help file
			Help.ShowHelp(mainwindow, filepathname, HelpNavigator.Topic, pagefile);
		}

		// This returns a unique temp filename
		internal static string MakeTempFilename(string tempdir)
		{
			return MakeTempFilename(tempdir, "tmp");
		}

		// This returns a unique temp filename
		internal static string MakeTempFilename(string tempdir, string extension)
		{
			string filename;
			const string chars = "abcdefghijklmnopqrstuvwxyz1234567890";
			Random rnd = new Random();

			do
			{
				// Generate a filename
				filename = "";
				int i;
				for(i = 0; i < 8; i++) filename += chars[rnd.Next(chars.Length)];
				filename = Path.Combine(tempdir, filename + "." + extension);
			}
			// Continue while file is not unique
			while(File.Exists(filename) || Directory.Exists(filename));

			// Return the filename
			return filename;
		}

		// This returns a unique temp directory name
		internal static string MakeTempDirname()
		{
			string dirname;
			const string chars = "abcdefghijklmnopqrstuvwxyz1234567890";
			Random rnd = new Random();

			do
			{
				// Generate a filename
				dirname = "";
				int i;
				for(i = 0; i < 8; i++) dirname += chars[rnd.Next(chars.Length)];
				dirname = Path.Combine(temppath, dirname);
			}
			// Continue while file is not unique
			while(File.Exists(dirname) || Directory.Exists(dirname));

			// Return the filename
			return dirname;
		}

		// This shows an image in a panel either zoomed or centered depending on size
		public static void DisplayZoomedImage(Panel panel, Image image)
		{
			// Image not null?
			if(image != null)
			{
				// Set the image
				panel.BackgroundImage = new Bitmap(image); //mxd. Make an image copy instead of assigning directly

				//mxd. Display it zoomed
				panel.BackgroundImageLayout = ImageLayout.Zoom;

				// Small enough to fit in panel?
				/*if((image.Size.Width < panel.ClientRectangle.Width) &&
				   (image.Size.Height < panel.ClientRectangle.Height))
				{
					// Display centered
					panel.BackgroundImageLayout = ImageLayout.Zoom;
				}
				else
				{
					// Display zoomed
					panel.BackgroundImageLayout = ImageLayout.Zoom;
				}*/
			}
			else
			{
				//mxd. Clear the image
				panel.BackgroundImage = null;
			}
		}

		// This calculates the new rectangle when one is scaled into another keeping aspect ratio
		/*public static RectangleF MakeZoomedRect(Size source, RectangleF target)
		{
			return MakeZoomedRect(new SizeF(source.Width, source.Height), target);
		}

		// This calculates the new rectangle when one is scaled into another keeping aspect ratio
		public static RectangleF MakeZoomedRect(Size source, Rectangle target)
		{
			return MakeZoomedRect(new SizeF(source.Width, source.Height),
								  new RectangleF(target.Left, target.Top, target.Width, target.Height));
		}
		
		// This calculates the new rectangle when one is scaled into another keeping aspect ratio
		public static RectangleF MakeZoomedRect(SizeF source, RectangleF target)
		{
			float scale;
			
			// Image fits?
			if((source.Width <= target.Width) &&
			   (source.Height <= target.Height))
			{
				// Just center
				scale = 1.0f;
			}
			// Image is wider than tall?
			else if((source.Width - target.Width) > (source.Height - target.Height))
			{
				// Scale down by width
				scale = target.Width / source.Width;
			}
			else
			{
				// Scale down by height
				scale = target.Height / source.Height;
			}
			
			// Return centered and scaled
			return new RectangleF(target.Left + (target.Width - source.Width * scale) * 0.5f,
								  target.Top + (target.Height - source.Height * scale) * 0.5f,
								  source.Width * scale, source.Height * scale);
		}*/

		// This opens a URL in the default browser
		public static void OpenWebsite(string url)
		{
			if(url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
			   url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
			   url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
			{
				Process.Start(url);
			}
		}

		//mxd
		public static int GetDropDownWidth(ComboBox cb)
		{
			int maxwidth = 0;
			foreach(var obj in cb.Items)
			{
				int temp = TextRenderer.MeasureText(obj.ToString(), cb.Font).Width;
				if(temp > maxwidth) maxwidth = temp;
			}
			return maxwidth > 0 ? maxwidth + 6 : 1;
		}
		
		// This returns the short path name for a file
		/*public static string GetShortFilePath(string longpath)
		{
			const int maxlen = 256;
			StringBuilder shortname = new StringBuilder(maxlen);
			GetShortPathName(longpath, shortname, maxlen);
			return shortname.ToString();
		}*/

		#endregion

		#region ==================  mxd. Uncaught exceptions handling

		// In some cases the program can remain operational after these
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				// Try handling it in user-friendy way...
				ExceptionDialog dlg = new ExceptionDialog(e);
				dlg.Setup();
				if(dlg.ShowDialog() == DialogResult.Cancel)
					Terminate(false);
			}
			catch
			{
				string exceptionmsg;

				// Try getting exception details...
				try { exceptionmsg = "Fatal Windows Forms error occurred: " + e.Exception.Message + "\n\nStack Trace:\n" + e.Exception.StackTrace; }
				catch(Exception exc) { exceptionmsg = "Failed to get initial exception details: " + exc.Message + "\n\nStack Trace:\n" + exc.StackTrace; }

				// Try logging it...
				try { WriteLogLine(exceptionmsg); }
				catch { }

				// Try displaying it to the user...
				try { MessageBox.Show(exceptionmsg, "Fatal Windows Forms Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
				finally { Process.GetCurrentProcess().Kill(); }
			}
		}

		// These are usually unrecoverable
		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				// Try handling it in user-friendy way...
				ExceptionDialog dlg = new ExceptionDialog(e);
				dlg.Setup();
				if(dlg.ShowDialog() == DialogResult.Cancel)
					Terminate(false);
			}
			catch
			{
				string exceptionmsg;

				// Try getting exception details...
				try
				{
					Exception ex = (Exception)e.ExceptionObject;
					exceptionmsg = "Fatal Non-UI error:\n" + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace;
				}
				catch(Exception exc)
				{
					exceptionmsg = "Failed to get initial exception details:\n" + exc.Message + "\n\nStack Trace:\n" + exc.StackTrace;
				}

				// Try logging it...
				try { WriteLogLine(exceptionmsg); }
				catch { }

				// Try displaying it to the user...
				try { MessageBox.Show(exceptionmsg, "Fatal Non-UI Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
				finally { Process.GetCurrentProcess().Kill(); }
			}
		}

		#endregion

	}
}

