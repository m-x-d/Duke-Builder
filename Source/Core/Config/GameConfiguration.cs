
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
using System.Collections.Generic;
using System.Globalization;
using mxd.DukeBuilder.Config.ImageSets;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Editing;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Config
{
	public class GameConfiguration
	{
		#region ================== Variables

		// Original configuration
		private Configuration cfg;
		
		// General settings
		private string configname;
		private string enginename;
		private string iconfilename;
		private string formatinterface;
		private int playerstarttileindex;
		private string testparameters;
		private string nomonstersparam;
		
		// Skills
		private List<SkillInfo> skills;
		
		// Sprites
		private Dictionary<string, string> spriteflags;
		private List<int> sortedspriteflags;
		private List<SpriteCategory> thingcategories;
		private Dictionary<int, SpriteInfo> things;
		
		// Walls
		private Dictionary<string, string> wallflags;
		private List<int> sortedwallflags;
		
		// Sectors
		private Dictionary<string, string> sectorflags;
		private List<int> sortedsectorflags;

		private Dictionary<int, SectorEffectInfo> sectoreffects;
		private EnumList sectoreffectslist;

		// Palettes
		private EnumList paletteslist;
		
		// Enums
		private Dictionary<string, EnumList> enums;

		//mxd. Default textures
		private int defaultwalltile;
		private int defaultfloortile;
		private int defaultceilingtile;

		//mxd. Default drawing settings
		private int defaultshade;
		private int defaultvisibility;
		private int defaultfloorheight;
		private int defaultceilingheight;
		private int defaultspritetile;
		private float defaultspriteangle;
		private List<string> defaultspriteflags; 
		
		// Defaults
		private List<DefinedImageSet> imagesets;
		private List<ThingsFilter> thingfilters;
		
		#endregion

		#region ================== Properties

		// General settings
		public string Name { get { return configname; } }
		public string EngineName { get { return enginename; } }
		public string IconFilename { get { return iconfilename; } }
		public string FormatInterface { get { return formatinterface; } }
		public int PlayerStartTileIndex { get { return playerstarttileindex; } }
		public string TestParameters { get { return testparameters; } }
		public string NoMonstersParameter { get { return nomonstersparam; } }

		// Skills
		public List<SkillInfo> Skills { get { return skills; } }

		// Sprites
		public IDictionary<string, string> SpriteFlags { get { return spriteflags; } }
		public List<int> SortedSpriteFlags { get { return sortedspriteflags; } }
		
		// Linedefs
		public IDictionary<string, string> WallFlags { get { return wallflags; } }
		public List<int> SortedWallFlags { get { return sortedwallflags; } }

		// Sectors
		public IDictionary<string, string> SectorFlags { get { return sectorflags; } }
		public List<int> SortedSectorFlags { get { return sortedsectorflags; } }
		public Dictionary<int, SectorEffectInfo> SectorEffects { get { return sectoreffects; } }
		internal EnumList SectorEffectsList { get { return sectoreffectslist; } }

		// Palettes
		internal EnumList PalettesList { get { return paletteslist; } }

		// Enums
		//public IDictionary<string, EnumList> Enums { get { return enums; } }

		//mxd. Default tiles
		public int DefaultWallTile { get { return defaultwalltile; } set { defaultwalltile = value; } }
		public int DefaultFloorTile { get { return defaultfloortile; } set { defaultfloortile = value; } }
		public int DefaultCeilingTile { get { return defaultceilingtile; } set { defaultceilingtile = value; } }
		public int DefaultSpriteTile { get { return defaultspritetile; } set { defaultspritetile = value; } }

		//mxd. Default drawing settings
		public int DefaultShade { get { return defaultshade; } set { defaultshade = value; } }
		public int DefaultVisibility { get { return defaultvisibility; } set { defaultvisibility = value; } }
		public int DefaultFloorHeight { get { return defaultfloorheight; } set { defaultfloorheight = value; } }
		public int DefaultCeilingHeight { get { return defaultceilingheight; } set { defaultceilingheight = value; } }
		public float DefaultSpriteAngle { get { return defaultspriteangle; } set { defaultspriteangle = value; } } // In radians!
		public List<string> DefaultSpriteFlags { get { return defaultspriteflags; } set { defaultspriteflags = value; } }

		// Defaults
		internal List<DefinedImageSet> ImageSets { get { return imagesets; } }
		public List<ThingsFilter> ThingsFilters { get { return thingfilters; } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal GameConfiguration(Configuration cfg)
		{
			// Initialize
			this.cfg = cfg;
			this.spriteflags = new Dictionary<string, string>();
			this.sortedspriteflags = new List<int>();
			this.defaultspriteflags = new List<string>();
			this.thingcategories = new List<SpriteCategory>();
			this.things = new Dictionary<int, SpriteInfo>();
			this.wallflags = new Dictionary<string, string>();
			this.sortedwallflags = new List<int>();
			this.sectorflags = new Dictionary<string, string>();
			this.sortedsectorflags = new List<int>();
			this.sectoreffects = new Dictionary<int, SectorEffectInfo>();
			this.sectoreffectslist = new EnumList();
			this.paletteslist = new EnumList();
			this.skills = new List<SkillInfo>();
			this.imagesets = new List<DefinedImageSet>();
			this.thingfilters = new List<ThingsFilter>();
			this.enums = new Dictionary<string, EnumList>(StringComparer.Ordinal);
			
			// Read general settings
			configname = cfg.ReadSetting("name", "<unnamed game>");
			enginename = cfg.ReadSetting("engine", string.Empty);
			iconfilename = cfg.ReadSetting("icon", string.Empty);
			formatinterface = cfg.ReadSetting("formatinterface", string.Empty);
			testparameters = cfg.ReadSetting("testparameters", string.Empty);
			playerstarttileindex = cfg.ReadSetting("playerstarttileindex", 0);
			nomonstersparam = cfg.ReadSetting("nomonstersparameter", string.Empty);
			
			// Default tiles
			defaultwalltile = cfg.ReadSetting("defaultwalltile", -1);
			defaultceilingtile = cfg.ReadSetting("defaultceilingtile", -1);
			defaultfloortile = cfg.ReadSetting("defaultfloortile", -1);
			defaultspritetile = cfg.ReadSetting("defaultspritetile", 1);

			// Default drawing settings
			defaultshade = cfg.ReadSetting("defaultshade", 0);
			defaultvisibility = cfg.ReadSetting("defaultvisibility", 0);
			defaultfloorheight = cfg.ReadSetting("defaultfloorheight", 0);
			defaultceilingheight = cfg.ReadSetting("defaultceilingheight", -16384);
			defaultspriteangle = Angle2D.DegToRad(cfg.ReadSetting("defaultspriteangle", 0));
			
			// Skills
			LoadSkills();

			// Enums
			LoadEnums();

			// Palettes
			LoadPalettes();
			
			// Things
			LoadFlags(spriteflags, sortedspriteflags, "spriteflags");
			LoadSpriteCategories();
			
			// Walls
			LoadFlags(wallflags, sortedwallflags, "wallflags");

			// Sectors
			LoadFlags(sectorflags, sortedsectorflags, "sectorflags");
			LoadSectorEffects();

			// Defaults
			LoadImageSets();
			LoadSpriteFilters();
		}
		
		#endregion

		#region ================== Loading
		
		// This loads the enumerations
		private void LoadEnums()
		{
			// Get enums list
			IDictionary dic = cfg.ReadSetting("enums", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				// Make new enum
				EnumList list = new EnumList(de.Key.ToString(), cfg);
				enums.Add(de.Key.ToString(), list);
			}
		}

		// This loads palette numbers/descriptions
		private void LoadPalettes()
		{
			//TODO: how does that "texture layers" thing from http://infosuite.duke4.net/index.php?page=references_palettes even work?..
			IDictionary dic = cfg.ReadSetting("palettes", new Hashtable());
			HashSet<int> processed = new HashSet<int>();
			foreach(DictionaryEntry de in dic)
			{
				// Try paring the palette number
				int index;
				if(int.TryParse(de.Key.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out index))
				{
					if(processed.Contains(index))
					{
						General.ErrorLogger.Add(ErrorType.Warning, "Palette index " + index + " is double-defined in \"palettes\" structure of game configuration \"" + this.Name + "\"");
						continue;
					}

					// Add to collections
					paletteslist.Add(new EnumItem(index.ToString(), de.Value.ToString()));
					processed.Add(index);
				}
				else
				{
					General.ErrorLogger.Add(ErrorType.Warning, "Structure \"palettes\" contains invalid keys in game configuration \"" + this.Name + "\"");
				}
			}
		}

		// Things and thing categories
		private void LoadSpriteCategories()
		{
			// Get thing categories
			IDictionary dic = cfg.ReadSetting("spritetypes", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				if(de.Value is IDictionary)
				{
					// Make a category
					SpriteCategory thingcat = new SpriteCategory(cfg, de.Key.ToString(), enums);

					// Add all things in category to the big list
					foreach(SpriteInfo t in thingcat.Sprites)
					{
						if(!things.ContainsKey(t.Index))
						{
							things.Add(t.Index, t);
						}
						else
						{
							General.ErrorLogger.Add(ErrorType.Warning, "Sprite number " + t.Index + " is defined more than once (as \"" + things[t.Index].Title + "\" and \"" + t.Title + "\") in game configuration \"" + this.Name + "\"");
						}
					}

					// Add category to list
					thingcategories.Add(thingcat);
				}
			}
		}

		// Sector effects
		private void LoadSectorEffects()
		{
			// Get sector effects
			IDictionary dic = cfg.ReadSetting("sectoreffects", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				// Try paring the sector effect
				int index;
				if(int.TryParse(de.Key.ToString(), NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out index))
				{
					if(sectoreffects.ContainsKey(index))
					{
						General.ErrorLogger.Add(ErrorType.Warning, "Sector effect " + index + " is double-defined in \"sectoreffects\" structure of game configuration \"" + this.Name + "\"");
						continue;
					}
					
					SectorEffectInfo si;
					if(de.Value is IDictionary)
					{
						string path = "sectoreffects." + index + ".";
						string title = cfg.ReadSetting(path + "title", "Effect " + index);
						string tagsstr = cfg.ReadSetting(path + "setags", string.Empty);
						HashSet<int> setags = new HashSet<int>();
						
						// Parse setags...
						if(!string.IsNullOrEmpty(tagsstr) && !General.GetNumbersFromString(tagsstr, setags))
						{
							General.ErrorLogger.Add(ErrorType.Warning, "Unable to get Sector Effect tags from string \"" + tagsstr + "\" while parsing Sector Effect " + index + " in game configuration \"" + this.Name + "\"");
						}

						// Make effect
						si = new SectorEffectInfo(index, title, setags);
					}
					else
					{
						// Make effect
						si = new SectorEffectInfo(index, de.Value.ToString());
					}
					
					// Add to collections
					sectoreffects.Add(index, si);
					sectoreffectslist.Add(new EnumItem(index.ToString(), si.Title));
				}
				else
				{
					General.ErrorLogger.Add(ErrorType.Warning, "Structure \"sectoreffects\" contains invalid keys in game configuration \"" + this.Name + "\"");
				}
			}

			// Sort the actions list
			//sortedsectoreffects.Sort();
		}

		// Brightness levels
		/*private void LoadBrightnessLevels()
		{
			IDictionary dic;
			int level;

			// Get brightness levels structure
			dic = cfg.ReadSetting("sectorbrightness", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				// Try paring the level
				if(int.TryParse(de.Key.ToString(),
					NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite,
					CultureInfo.InvariantCulture, out level))
				{
					brightnesslevels.Add(level);
				}
				else
				{
					General.ErrorLogger.Add(ErrorType.Warning, "Structure 'sectorbrightness' contains invalid keys in game configuration \"" + this.Name + "\"");
				}
			}

			// Sort the list
			brightnesslevels.Sort();
		}*/

		// Sector generalized effects
		/*private void LoadSectorGeneralizedEffects()
		{
			IDictionary dic;

			// Get sector effects
			dic = cfg.ReadSetting("gen_sectortypes", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				// Check for valid structure
				if(de.Value is IDictionary)
				{
					// Add option
					geneffectoptions.Add(new GeneralizedOption("gen_sectortypes", "", de.Key.ToString(), de.Value as IDictionary));
				}
				else
				{
					General.ErrorLogger.Add(ErrorType.Warning, "Structure 'gen_sectortypes' contains invalid entries in game configuration \"" + this.Name + "\"");
				}
			}
		}*/

		//mxd
		private void LoadFlags(Dictionary<string, string> flags, List<int> sortedflags, string settingname)
		{
			IDictionary dic = cfg.ReadSetting(settingname, new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				int flag;
				string flagstr = de.Key.ToString();
				if(int.TryParse(flagstr, NumberStyles.Integer, CultureInfo.InvariantCulture, out flag))
				{
					if(flags.ContainsKey(flagstr))
						General.ErrorLogger.Add(ErrorType.Warning, "Structure \"" + settingname + "\" contains duplicate flag definition for flag \"" + de.Value + "\" in game configuration \"" + this.Name + "\"");
					else
						sortedflags.Add(flag);
					
					flags[flagstr] = de.Value.ToString();
				}
				else
				{
					General.ErrorLogger.Add(ErrorType.Warning, "Structure \"" + settingname + "\" contains invalid flag number \"" + de.Key + "\" for flag " + de.Value + " in game configuration \"" + this.Name + "\"");
				}
			}

			// Sort the flags
			sortedflags.Sort();
		}

		// Skills
		private void LoadSkills()
		{
			// Get skills
			IDictionary dic = cfg.ReadSetting("skills", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				int num;
				if(int.TryParse(de.Key.ToString(), out num))
				{
					skills.Add(new SkillInfo(num, de.Value.ToString()));
				}
				else
				{
					General.ErrorLogger.Add(ErrorType.Warning, "Structure \"skills\" contains invalid skill number \"" + de.Key + "\" in game configuration \"" + this.Name + "\"");
				}
			}
		}
		
		// Texture Sets
		private void LoadImageSets()
		{
			// Get sets
			IDictionary dic = cfg.ReadSetting("imagesets", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				DefinedImageSet s = new DefinedImageSet(cfg, "imagesets." + de.Key);
				imagesets.Add(s);
			}
		}
		
		// Thing Filters
		private void LoadSpriteFilters()
		{
			// Get sets
			IDictionary dic = cfg.ReadSetting("spritefilters", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				ThingsFilter f = new ThingsFilter(cfg, "spritefilters." + de.Key);
				thingfilters.Add(f);
			}
		}
		
		#endregion

		#region ================== Methods

		// ReadSetting
		public string ReadSetting(string setting, string defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		public int ReadSetting(string setting, int defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		public float ReadSetting(string setting, float defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		public short ReadSetting(string setting, short defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		public long ReadSetting(string setting, long defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		public bool ReadSetting(string setting, bool defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		public byte ReadSetting(string setting, byte defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		public IDictionary ReadSetting(string setting, IDictionary defaultsetting) { return cfg.ReadSetting(setting, defaultsetting); }
		
		// This gets a list of things categories
		internal List<SpriteCategory> GetThingCategories()
		{
			return new List<SpriteCategory>(thingcategories);
		}
		
		// This gets a list of things
		internal Dictionary<int, SpriteInfo> GetThingTypes()
		{
			return new Dictionary<int, SpriteInfo>(things);
		}

		// This returns information on a sector effect
		/*public SectorEffectInfo GetSectorEffectInfo(int effect)
		{
			// Known type?
			if(sectoreffects.ContainsKey(effect))
			{
				return sectoreffects[effect];
			}
			else if(effect == 0)
			{
				return new SectorEffectInfo(0, "None", true, false);
			}
			else
			{
				return new SectorEffectInfo(effect, "Unknown", false, false);
			}
		}*/
		
		#endregion

		#region ================== Default Settings

		// This sets the default thing flags
		/*public void SetDefaultThingFlags(int flags)
		{
			defaultthingflags = flags;
		}*/

		// This applies default settings to a thing
		public void ApplyDefaultSpriteSettings(Thing t)
		{
			t.TileIndex = defaultspritetile;
			t.Angle = defaultspriteangle;
			foreach(string f in defaultspriteflags) t.SetFlag(f, true);
			General.Map.IsChanged = true; //mxd
		}

		// This attempts to find the default drawing settings
		public void FindDefaultDrawSettings()
		{
			// Only possible when a map is loaded
			if(General.Map == null) return;

			// Default texture missing?
			if(defaultwalltile < 0)
			{
				// Find default texture from map
				foreach(Sidedef sd in General.Map.Map.Sidedefs)
				{
					if(sd.TileIndex > 0 && General.Map.Data.GetImageExists(sd.TileIndex))
					{
						defaultwalltile = sd.TileIndex;
						break;
					}
				}
			}

			// Default floor missing?
			if(defaultfloortile < 0)
			{
				// Find default texture from map
				foreach(Sector s in General.Map.Map.Sectors)
				{
					if(s.FloorTileIndex > 0 && General.Map.Data.GetImageExists(s.FloorTileIndex))
					{
						defaultfloortile = s.FloorTileIndex;
						break;
					}
				}
			}

			// Default ceiling missing?
			if(defaultceilingtile < 0)
			{
				// Find default texture from map
				foreach(Sector s in General.Map.Map.Sectors)
				{
					if(s.CeilingTileIndex > 0 && General.Map.Data.GetImageExists(s.CeilingTileIndex))
					{
						defaultceilingtile = s.CeilingTileIndex;
						break;
					}
				}
			}

			//mxd. Find the first valid tile index
			int validtexture = 0;
			if(defaultwalltile < 0 || defaultfloortile < 0 || defaultceilingtile < 0)
			{
				foreach(var data in General.Map.Data.Images)
				{
					validtexture = data.TileIndex;
					break;
				}
			}

			// Texture indices may not be undefined
			if(defaultwalltile < 0) defaultwalltile = validtexture;
			if(defaultfloortile < 0) defaultfloortile = validtexture;
			if(defaultceilingtile < 0) defaultceilingtile = validtexture;
		}

		#endregion
	}
}
