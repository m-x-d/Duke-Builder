
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

using System.Collections;
using System.IO;
using System.Reflection;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Plugins;

#endregion

namespace mxd.DukeBuilder.Map
{
	public sealed class MapOptions
	{
		#region ================== Variables

		// Map configuration
		private Configuration mapconfig;
		
		// Game configuration
		private string configfile;
		
		#endregion

		#region ================== Properties

		internal string ConfigFile { get { return configfile; } set { configfile = value; } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal MapOptions()
		{
			// Initialize
			this.configfile = "";
			this.mapconfig = new Configuration(true);
		}

		// Constructor to load from Duke Builder Map Settings Configuration
		internal MapOptions(Configuration cfg)
		{
			// Initialize
			this.configfile = cfg.ReadSetting("gameconfig", "");
			this.mapconfig = new Configuration(true);
			
			// Read map configuration
			this.mapconfig.Root = cfg.ReadSetting("map", new Hashtable());
		}
		
		#endregion

		#region ================== Methods

		// This makes the path prefix for the given assembly
		private string GetPluginPathPrefix(Assembly asm)
		{
			Plugin p = General.Plugins.FindPluginByAssembly(asm);
			return "plugins." + p.Name.ToLowerInvariant() + ".";
		}

		// ReadPluginSetting
		public string ReadPluginSetting(string setting, string defaultsetting) { return mapconfig.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public int ReadPluginSetting(string setting, int defaultsetting) { return mapconfig.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public float ReadPluginSetting(string setting, float defaultsetting) { return mapconfig.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public short ReadPluginSetting(string setting, short defaultsetting) { return mapconfig.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public long ReadPluginSetting(string setting, long defaultsetting) { return mapconfig.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public bool ReadPluginSetting(string setting, bool defaultsetting) { return mapconfig.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public byte ReadPluginSetting(string setting, byte defaultsetting) { return mapconfig.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }
		public IDictionary ReadPluginSetting(string setting, IDictionary defaultsetting) { return mapconfig.ReadSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, defaultsetting); }

		// ReadPluginSetting with specific plugin
		public string ReadPluginSetting(string pluginname, string setting, string defaultsetting) { return mapconfig.ReadSetting(pluginname.ToLowerInvariant() + "." + setting, defaultsetting); }
		public int ReadPluginSetting(string pluginname, string setting, int defaultsetting) { return mapconfig.ReadSetting(pluginname.ToLowerInvariant() + "." + setting, defaultsetting); }
		public float ReadPluginSetting(string pluginname, string setting, float defaultsetting) { return mapconfig.ReadSetting(pluginname.ToLowerInvariant() + "." + setting, defaultsetting); }
		public short ReadPluginSetting(string pluginname, string setting, short defaultsetting) { return mapconfig.ReadSetting(pluginname.ToLowerInvariant() + "." + setting, defaultsetting); }
		public long ReadPluginSetting(string pluginname, string setting, long defaultsetting) { return mapconfig.ReadSetting(pluginname.ToLowerInvariant() + "." + setting, defaultsetting); }
		public bool ReadPluginSetting(string pluginname, string setting, bool defaultsetting) { return mapconfig.ReadSetting(pluginname.ToLowerInvariant() + "." + setting, defaultsetting); }
		public byte ReadPluginSetting(string pluginname, string setting, byte defaultsetting) { return mapconfig.ReadSetting(pluginname.ToLowerInvariant() + "." + setting, defaultsetting); }
		public IDictionary ReadPluginSetting(string pluginname, string setting, IDictionary defaultsetting) { return mapconfig.ReadSetting(pluginname.ToLowerInvariant() + "." + setting, defaultsetting); }

		// WritePluginSetting
		public bool WritePluginSetting(string setting, object settingvalue) { return mapconfig.WriteSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting, settingvalue); }

		// DeletePluginSetting
		public bool DeletePluginSetting(string setting) { return mapconfig.DeleteSetting(GetPluginPathPrefix(Assembly.GetCallingAssembly()) + setting); }
		
		// This stores the map options in a configuration
		internal void WriteConfiguration(string settingsfile)
		{
			// Write grid settings
			General.Map.Grid.WriteToConfig(mapconfig, "grid");

			// Load the file or make a new file
			Configuration mapcfg = (File.Exists(settingsfile) ? new Configuration(settingsfile, true) : new Configuration(true));
			
			// Write configuration type information
			mapcfg.WriteSetting("type", "Duke Builder Map Settings Configuration");
			mapcfg.WriteSetting("gameconfig", configfile);
			
			// Update the settings file with this map configuration
			mapcfg.WriteSetting("map", mapconfig.Root);

			// Save file
			mapcfg.SaveConfiguration(settingsfile);
		}

		// This loads the grid settings
		internal void ApplyGridSettings()
		{
			General.Map.Grid.ReadFromConfig(mapconfig, "grid");
		}
		
		#endregion
	}
}
