
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
using System.Collections.Generic;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Config;

#endregion

namespace mxd.DukeBuilder.Editing
{
	public class ThingsFilter
	{
		#region ================== Variables

		// Display name of this filter
		protected string name;
		
		// Filter by category
		protected string categoryname;

		// Filter by properties
		protected int tileindex;	// -1 indicates not used
		protected int angle;		// -1 indicates not used
		protected int zheight;		// int.MinValue indicates not used
		protected int shade;		// int.MinValue indicates not used
		protected int paletteindex; // -1 indicates not used

		// Filter by flags
		protected Dictionary<string, bool> flags; 
		
		// Filter by tags
		protected int hitag;		// -1 indicates not used
		protected int lotag;		// -1 indicates not used
		protected int extra;		// -1 indicates not used
		
		// List of things
		protected List<Thing> visiblethings;
		protected List<Thing> hiddenthings;
		protected Dictionary<Thing, bool> thingsvisiblestate;

		#endregion

		#region ================== Properties

		public string Name { get { return name; } internal set { name = value; } }
		public string CategoryName { get { return categoryname; } internal set { categoryname = value; } }
		internal int TileIndex { get { return tileindex; } set { tileindex = value; } }
		internal int Angle { get { return angle; } set { angle = value; } }
		internal int ZHeight { get { return zheight; } set { zheight = value; } }
		internal int Shade { get { return shade; } set { shade = value; } }
		internal int PaletteIndex { get { return paletteindex; } set { paletteindex = value; } }

		internal Dictionary<string, bool> Flags { get { return flags; } set { flags = new Dictionary<string, bool>(value); } }

		internal int HiTag { get { return hitag; } set { hitag = value; } }
		internal int LoTag { get { return lotag; } set { lotag = value; } }
		internal int Extra { get { return extra; } set { extra = value; } }

		public ICollection<Thing> VisibleThings { get { return visiblethings; } }
		public ICollection<Thing> HiddenThings { get { return hiddenthings; } }
		
		#endregion
		
		#region ================== Constructor / Disposer
		
		// Copy constructor
		internal ThingsFilter(ThingsFilter f)
		{
			// Copy
			name = f.name;
			categoryname = f.categoryname;

			tileindex = f.tileindex;
			angle = f.angle;
			zheight = f.zheight;
			shade = f.shade;
			paletteindex = f.paletteindex;

			flags = new Dictionary<string, bool>(f.flags);

			hitag = f.hitag;
			lotag = f.lotag;
			extra = f.extra;
		}
		
		// Constructor for filter from configuration
		internal ThingsFilter(Configuration cfg, string path)
		{
			// Read settings from config
			name = cfg.ReadSetting(path + ".name", "Unnamed filter");
			categoryname = cfg.ReadSetting(path + ".category", "");

			tileindex = cfg.ReadSetting(path + ".tileindex", -1);
			angle = cfg.ReadSetting(path + ".angle", -1);
			zheight = cfg.ReadSetting(path + ".zheight", int.MinValue);
			shade = cfg.ReadSetting(path + ".shade", int.MinValue);
			paletteindex = cfg.ReadSetting(path + ".paletteindex", -1);

			flags = new Dictionary<string, bool>();

			hitag = cfg.ReadSetting(path + ".hitag", -1);
			lotag = cfg.ReadSetting(path + ".lotag", -1);
			extra = cfg.ReadSetting(path + ".extra", -1);
			
			// Read flags. Key is string, value must be boolean, which indicates whether the flag must be set or unset
			IDictionary dic = cfg.ReadSetting(path + ".flags", new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				string flag = de.Key.ToString();
				if(flags.ContainsKey(flag))
					General.ErrorLogger.Add(ErrorType.Warning, "Flag \"" + flag + "\" is double-defined in \"" + name + "\" things filter");
				else
					flags.Add(flag, (bool)de.Value);
			}
		}

		// Constructor for a new filter
		internal ThingsFilter()
		{
			// Initialize everything as <any>
			name = "Unnamed filter";
			categoryname = "";

			tileindex = -1;
			angle = -1;
			zheight = int.MinValue;
			shade = int.MinValue;
			paletteindex = -1;

			flags = new Dictionary<string, bool>();

			hitag = -1;
			lotag = -1;
			extra = -1;
		}

		#endregion

		#region ================== Methods
		
		/// <summary>
		/// This checks if a thing is visible. Throws an exception when the specified Thing does not exist in the map (filter not updated?).
		/// </summary>
		public bool IsThingVisible(Thing t)
		{
			return thingsvisiblestate[t];
		}

		// This writes the filter to configuration
		internal void WriteSettings(Configuration cfg, string path)
		{
			// Write settings to config
			cfg.WriteSetting(path + ".name", name);
			cfg.WriteSetting(path + ".category", categoryname);

			cfg.WriteSetting(path + ".tileindex", tileindex);
			cfg.WriteSetting(path + ".angle", angle);
			cfg.WriteSetting(path + ".zheight", zheight);
			cfg.WriteSetting(path + ".shade", shade);
			cfg.WriteSetting(path + ".paletteindex", paletteindex);

			// Write flags to config
			foreach(var group in flags)
				cfg.WriteSetting(path + ".flags." + group.Key, group.Value);

			cfg.WriteSetting(path + ".hitag", hitag);
			cfg.WriteSetting(path + ".lotag", lotag);
			cfg.WriteSetting(path + ".extra", extra);
		}
		
		// This is called when the filter is activated
		internal virtual void Activate()
		{
			// Update the list of things
			Update();
		}
		
		// This is called when the filter is deactivates
		internal virtual void Deactivate()
		{
			// Clear lists
			visiblethings = null;
			hiddenthings = null;
			thingsvisiblestate = null;
		}
		
		/// <summary>
		/// This updates the list of things.
		/// </summary>
		public virtual void Update()
		{
			// Make new list
			visiblethings = new List<Thing>(General.Map.Map.Things.Count);
			hiddenthings = new List<Thing>(General.Map.Map.Things.Count);
			thingsvisiblestate = new Dictionary<Thing, bool>(General.Map.Map.Things.Count);
			
			foreach(Thing t in General.Map.Map.Things)
			{
				bool qualifies = true;

				// Check against simple properties
				qualifies &= (tileindex == -1) || (t.TileIndex == tileindex);
				qualifies &= (angle == -1) || (t.AngleDeg == angle);
				qualifies &= (zheight == int.MinValue) || ((int)(t.Position.z) == zheight);
				qualifies &= (shade == int.MinValue) || (t.Shade == shade);
				qualifies &= (paletteindex == -1) || (t.PaletteIndex == paletteindex);

				qualifies &= (hitag == -1) || (t.HiTag == hitag);
				qualifies &= (lotag == -1) || (t.LoTag == lotag);
				qualifies &= (extra == -1) || (t.Extra == extra);
				
				// Still qualifies?
				if(qualifies)
				{
					// Get thing info
					SpriteInfo ti = General.Map.Data.GetSpriteInfoEx(t.TileIndex);
					
					// Check thing category
					if(ti == null ||  ti.Category == null)
						qualifies = (categoryname.Length == 0);
					else
						qualifies = ((ti.Category.Name == categoryname) || (categoryname.Length == 0));
				}
				
				// Still qualifies?
				if(qualifies)
				{
					// Go for all flags
					foreach(var group in flags)
					{
						if(t.IsFlagSet(group.Key) != group.Value)
						{
							qualifies = false;
							break;
						}
					}
				}
				
				// Put the thing in the lists
				if(qualifies) visiblethings.Add(t); else hiddenthings.Add(t);
				thingsvisiblestate.Add(t, qualifies);
			}
		}

		// String representation
		public override string ToString()
		{
			return name;
		}
		
		#endregion
	}
}
