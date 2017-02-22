
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
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Plugins;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.Config
{
	public class ProgramConfiguration
	{
		#region ================== Variables

		// Original configuration
		private Configuration cfg;
		
		// Cached variables
		private int visualfov;
		private float visualmousesensx;
		private float visualmousesensy;
		private int imagebrightness;
		private float doublesidedalpha;
		private byte doublesidedalphabyte;
		private float backgroundalpha;
		private bool testmonsters;
		private int defaultviewmode;
		private bool classicbilinear;
		private bool visualbilinear;
		private int mousespeed;
		private int movespeed;
		private float viewdistance;
		private bool invertyaxis;
		private int autoscrollspeed;
		private int zoomfactor;
		private bool showerrorswindow;
		private bool animatevisualselection;
		private int previousversion;
		private PasteOptions pasteoptions;
		private int dockersposition;
		private bool collapsedockers;
		private int dockerswidth;
		private bool toolbarundo;
		private bool toolbarcopy;
		//private bool toolbarprefabs;
		private bool toolbarfilter;
		private bool toolbarviewmodes;
		private bool toolbargeometry;
		private bool toolbartesting;
		private bool toolbarfile;
		private float filteranisotropy;
		private bool showimagesizes;
		
		#endregion

		#region ================== Properties

		internal Configuration Config { get { return cfg; } }
		public int VisualFOV { get { return visualfov; } internal set { visualfov = value; } }
		public int ImageBrightness { get { return imagebrightness; } internal set { imagebrightness = value; } }
		public float DoubleSidedAlpha { get { return doublesidedalpha; } internal set { doublesidedalpha = value; doublesidedalphabyte = (byte)(doublesidedalpha * 255f); } }
		public byte DoubleSidedAlphaByte { get { return doublesidedalphabyte; } }
		public float BackgroundAlpha { get { return backgroundalpha; } internal set { backgroundalpha = value; } }
		public float VisualMouseSensX { get { return visualmousesensx; } internal set { visualmousesensx = value; } }
		public float VisualMouseSensY { get { return visualmousesensy; } internal set { visualmousesensy = value; } }
		public bool TestMonsters { get { return testmonsters; } internal set { testmonsters = value; } }
		public int DefaultViewMode { get { return defaultviewmode; } internal set { defaultviewmode = value; } }
		public bool ClassicBilinear { get { return classicbilinear; } internal set { classicbilinear = value; } }
		public bool VisualBilinear { get { return visualbilinear; } internal set { visualbilinear = value; } }
		public int MouseSpeed { get { return mousespeed; } internal set { mousespeed = value; } }
		public int MoveSpeed { get { return movespeed; } internal set { movespeed = value; } }
		public float ViewDistance { get { return viewdistance; } internal set { viewdistance = value; } }
		public bool InvertYAxis { get { return invertyaxis; } internal set { invertyaxis = value; } }
		public int AutoScrollSpeed { get { return autoscrollspeed; } internal set { autoscrollspeed = value; } }
		public int ZoomFactor { get { return zoomfactor; } internal set { zoomfactor = value; } }
		public bool ShowErrorsWindow { get { return showerrorswindow; } internal set { showerrorswindow = value; } }
		public bool AnimateVisualSelection { get { return animatevisualselection; } internal set { animatevisualselection = value; } }
		internal int PreviousVersion { get { return previousversion; } }
		internal PasteOptions PasteOptions { get { return pasteoptions; } set { pasteoptions = value; } }
		public int DockersPosition { get { return dockersposition; } internal set { dockersposition = value; } }
		public bool CollapseDockers { get { return collapsedockers; } internal set { collapsedockers = value; } }
		public int DockersWidth { get { return dockerswidth; } internal set { dockerswidth = value; } }
		public bool ToolbarUndo { get { return toolbarundo; } internal set { toolbarundo = value; } }
		public bool ToolbarCopy { get { return toolbarcopy; } internal set { toolbarcopy = value; } }
		//public bool ToolbarPrefabs { get { return toolbarprefabs; } internal set { toolbarprefabs = value; } }
		public bool ToolbarFilter { get { return toolbarfilter; } internal set { toolbarfilter = value; } }
		public bool ToolbarViewModes { get { return toolbarviewmodes; } internal set { toolbarviewmodes = value; } }
		public bool ToolbarGeometry { get { return toolbargeometry; } internal set { toolbargeometry = value; } }
		public bool ToolbarTesting { get { return toolbartesting; } internal set { toolbartesting = value; } }
		public bool ToolbarFile { get { return toolbarfile; } internal set { toolbarfile = value; } }
		public float FilterAnisotropy { get { return filteranisotropy; } internal set { filteranisotropy = value; } }
		public bool ShowImageSizes { get { return showimagesizes; } internal set { showimagesizes = value; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal ProgramConfiguration()
		{
			// We have no destructor
			//GC.SuppressFinalize(this);
			//defaultthingflags = new List<string>();
			pasteoptions = new PasteOptions();
		}

		#endregion

		#region ================== Loading / Saving

		// This loads the program configuration
		internal bool Load(string cfgfilepathname, string defaultfilepathname)
		{
			// First parse it
			if(Read(cfgfilepathname, defaultfilepathname))
			{
				// Read the cache variables
				visualfov = cfg.ReadSetting("visualfov", 80);
				visualmousesensx = cfg.ReadSetting("visualmousesensx", 40f);
				visualmousesensy = cfg.ReadSetting("visualmousesensy", 40f);
				imagebrightness = cfg.ReadSetting("imagebrightness", 3);
				doublesidedalpha = cfg.ReadSetting("doublesidedalpha", 0.4f);
				doublesidedalphabyte = (byte)(doublesidedalpha * 255f);
				backgroundalpha = cfg.ReadSetting("backgroundalpha", 1.0f);
				//squarethings = cfg.ReadSetting("squarethings", false);
				testmonsters = cfg.ReadSetting("testmonsters", true);
				defaultviewmode = cfg.ReadSetting("defaultviewmode", (int)ViewMode.Normal);
				classicbilinear = cfg.ReadSetting("classicbilinear", false);
				visualbilinear = cfg.ReadSetting("visualbilinear", false);
				mousespeed = cfg.ReadSetting("mousespeed", 100);
				movespeed = cfg.ReadSetting("movespeed", 1600);
				viewdistance = cfg.ReadSetting("viewdistance", 48000.0f);
				invertyaxis = cfg.ReadSetting("invertyaxis", false);
				autoscrollspeed = cfg.ReadSetting("autoscrollspeed", 0);
				zoomfactor = cfg.ReadSetting("zoomfactor", 3);
				showerrorswindow = cfg.ReadSetting("showerrorswindow", true);
				animatevisualselection = cfg.ReadSetting("animatevisualselection", true);
				previousversion = cfg.ReadSetting("currentversion", 0);
				dockersposition = cfg.ReadSetting("dockersposition", 1);
				collapsedockers = cfg.ReadSetting("collapsedockers", true);
				dockerswidth = cfg.ReadSetting("dockerswidth", 300);
				pasteoptions.ReadConfiguration(cfg, "pasteoptions");
				toolbarundo = cfg.ReadSetting("toolbarundo", false);
				toolbarcopy = cfg.ReadSetting("toolbarcopy", false);
				toolbarfilter = cfg.ReadSetting("toolbarfilter", true);
				toolbarviewmodes = cfg.ReadSetting("toolbarviewmodes", false);
				toolbargeometry = cfg.ReadSetting("toolbargeometry", true);
				toolbartesting = cfg.ReadSetting("toolbartesting", true);
				toolbarfile = cfg.ReadSetting("toolbarfile", true);
				filteranisotropy = cfg.ReadSetting("filteranisotropy", 8.0f);
				showimagesizes = cfg.ReadSetting("showimagesizes", true);
				
				// Success
				return true;
			}

			// Failed
			return false;
		}

		// This saves the program configuration
		internal void Save(string filepathname)
		{
			Version v = General.ThisAssembly.GetName().Version;
			
			// Write the cache variables
			cfg.WriteSetting("visualfov", visualfov);
			cfg.WriteSetting("visualmousesensx", visualmousesensx);
			cfg.WriteSetting("visualmousesensy", visualmousesensy);
			cfg.WriteSetting("imagebrightness", imagebrightness);
			//cfg.WriteSetting("squarethings", squarethings);
			cfg.WriteSetting("testmonsters", testmonsters);
			cfg.WriteSetting("doublesidedalpha", doublesidedalpha);
			cfg.WriteSetting("backgroundalpha", backgroundalpha);
			cfg.WriteSetting("defaultviewmode", defaultviewmode);
			cfg.WriteSetting("classicbilinear", classicbilinear);
			cfg.WriteSetting("visualbilinear", visualbilinear);
			cfg.WriteSetting("mousespeed", mousespeed);
			cfg.WriteSetting("movespeed", movespeed);
			cfg.WriteSetting("viewdistance", viewdistance);
			cfg.WriteSetting("invertyaxis", invertyaxis);
			cfg.WriteSetting("autoscrollspeed", autoscrollspeed);
			cfg.WriteSetting("zoomfactor", zoomfactor);
			cfg.WriteSetting("showerrorswindow", showerrorswindow);
			cfg.WriteSetting("animatevisualselection", animatevisualselection);
			cfg.WriteSetting("currentversion", v.Major * 1000000 + v.Revision);
			cfg.WriteSetting("dockersposition", dockersposition);
			cfg.WriteSetting("collapsedockers", collapsedockers);
			cfg.WriteSetting("dockerswidth", dockerswidth);
			pasteoptions.WriteConfiguration(cfg, "pasteoptions");
			cfg.WriteSetting("toolbarundo", toolbarundo);
			cfg.WriteSetting("toolbarcopy", toolbarcopy);
			cfg.WriteSetting("toolbarfilter", toolbarfilter);
			cfg.WriteSetting("toolbarviewmodes", toolbarviewmodes);
			cfg.WriteSetting("toolbargeometry", toolbargeometry);
			cfg.WriteSetting("toolbartesting", toolbartesting);
			cfg.WriteSetting("toolbarfile", toolbarfile);
			cfg.WriteSetting("filteranisotropy", filteranisotropy);
			cfg.WriteSetting("showimagesizes", showimagesizes);
			
			// Save settings configuration
			General.WriteLogLine("Saving program configuration...");
			cfg.SaveConfiguration(filepathname);
		}
		
		// This reads the configuration
		private bool Read(string cfgfilepathname, string defaultfilepathname)
		{
			// Check if no config for this user exists yet
			if(!File.Exists(cfgfilepathname))
			{
				// Copy new configuration
				General.WriteLogLine("Local user program configuration is missing!");
				File.Copy(defaultfilepathname, cfgfilepathname);
				General.WriteLogLine("New program configuration copied for local user");
			}

			// Load it
			cfg = new Configuration(cfgfilepathname, true);
			if(cfg.ErrorResult)
			{
				// Error in configuration
				// Ask user for a new copy
				DialogResult result = General.ShowErrorMessage("Error in program configuration near line " + cfg.ErrorLine + ": " + cfg.ErrorDescription + "\n\nWould you like to overwrite your settings with a new configuration to restore the default settings?", MessageBoxButtons.YesNoCancel);
				if(result == DialogResult.Yes)
				{
					// Remove old configuration and make a new copy
					General.WriteLogLine("User requested a new copy of the program configuration");
					File.Delete(cfgfilepathname);
					File.Copy(defaultfilepathname, cfgfilepathname);
					General.WriteLogLine("New program configuration copied for local user");

					// Load it
					cfg = new Configuration(cfgfilepathname, true);
					if(cfg.ErrorResult)
					{
						// Error in configuration
						General.WriteLogLine("Error in program configuration near line " + cfg.ErrorLine + ": " + cfg.ErrorDescription);
						General.ShowErrorMessage("Default program configuration is corrupted. Please re-install Duke Builder.", MessageBoxButtons.OK);
						return false;
					}
				}
				else if(result == DialogResult.Cancel)
				{
					// User requested to cancel startup
					General.WriteLogLine("User cancelled startup");
					return false;
				}
			}

			// Check if a version number is missing
			previousversion = cfg.ReadSetting("currentversion", -1);
			if(previousversion == -1)
			{
				// Remove old configuration and make a new copy
				General.WriteLogLine("Program configuration is outdated, new configuration will be copied for local user");
				File.Delete(cfgfilepathname);
				File.Copy(defaultfilepathname, cfgfilepathname);
				
				// Load it
				cfg = new Configuration(cfgfilepathname, true);
				if(cfg.ErrorResult)
				{
					// Error in configuration
					General.WriteLogLine("Error in program configuration near line " + cfg.ErrorLine + ": " + cfg.ErrorDescription);
					General.ShowErrorMessage("Default program configuration is corrupted. Please re-install Duke Builder.", MessageBoxButtons.OK);
					return false;
				}
			}
			
			// Success
			return true;
		}
		
		#endregion

		#region ================== Methods

		// This makes the path prefix for the given assembly
		private string GetPluginPathPrefix(Assembly asm)
		{
			Plugin p = General.Plugins.FindPluginByAssembly(asm);
			return GetPluginPathPrefix(p.Name);
		}

		// This makes the path prefix for the given assembly
		private string GetPluginPathPrefix(string assemblyname)
		{
			return "plugins." + assemblyname.ToLowerInvariant() + ".";
		}

		// ReadPluginSetting
		public string ReadPluginSetting(string setting, string defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public int ReadPluginSetting(string setting, int defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public float ReadPluginSetting(string setting, float defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public short ReadPluginSetting(string setting, short defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public long ReadPluginSetting(string setting, long defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public bool ReadPluginSetting(string setting, bool defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public byte ReadPluginSetting(string setting, byte defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public IDictionary ReadPluginSetting(string setting, IDictionary defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }

		// ReadPluginSetting with specific plugin
		public string ReadPluginSetting(string pluginname, string setting, string defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(pluginname) + setting, defaultsetting); }
		public int ReadPluginSetting(string pluginname, string setting, int defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(pluginname) + setting, defaultsetting); }
		public float ReadPluginSetting(string pluginname, string setting, float defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(pluginname) + setting, defaultsetting); }
		public short ReadPluginSetting(string pluginname, string setting, short defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(pluginname) + setting, defaultsetting); }
		public long ReadPluginSetting(string pluginname, string setting, long defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(pluginname) + setting, defaultsetting); }
		public bool ReadPluginSetting(string pluginname, string setting, bool defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(pluginname) + setting, defaultsetting); }
		public byte ReadPluginSetting(string pluginname, string setting, byte defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(pluginname) + setting, defaultsetting); }
		public IDictionary ReadPluginSetting(string pluginname, string setting, IDictionary defaultsetting) { return cfg.ReadSetting(GetPluginPathPrefix(pluginname) + setting, defaultsetting); }
		
		// WritePluginSetting
		public bool WritePluginSetting(string setting, object settingvalue) { return cfg.WriteSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, settingvalue); }
		
		// DeletePluginSetting
		public bool DeletePluginSetting(string setting) { return cfg.DeleteSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting); }

		// ReadSetting
		internal string ReadSetting(string setting, string defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		internal int ReadSetting(string setting, int defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		internal float ReadSetting(string setting, float defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		internal short ReadSetting(string setting, short defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		internal long ReadSetting(string setting, long defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		internal bool ReadSetting(string setting, bool defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		internal byte ReadSetting(string setting, byte defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		internal IDictionary ReadSetting(string setting, IDictionary defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }

		// WriteSetting
		internal bool WriteSetting(string setting, object settingvalue) { return cfg.WriteSetting(setting, settingvalue); }
		internal bool WriteSetting(string setting, object settingvalue, string pathseperator) { return cfg.WriteSetting(setting, settingvalue, pathseperator); }

		// DeleteSetting
		internal bool DeleteSetting(string setting) { return cfg.DeleteSetting(setting); }
		internal bool DeleteSetting(string setting, string pathseperator) { return cfg.DeleteSetting(setting, pathseperator); }

		#endregion
	}
}
