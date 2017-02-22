
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
using System.Globalization;
using mxd.DukeBuilder.IO;

#endregion

namespace mxd.DukeBuilder.Config
{
	public class SpriteCategory
	{
		#region ================== Variables

		// Sprites
		private List<SpriteInfo> sprites;
		
		// Category properties
		private string name;
		private string title;
		private bool sorted;

		// Thing properties for inheritance
		//private int tileindex;
		private int color;
		//private int arrow;
		private float radius;
		private float height;
		//private int hangs;
		//private int blocking;
		//private int errorcheck;
		private bool fixedsize;
		//private bool absolutez;
		//private float spritescale;
		
		// Disposing
		//private bool isdisposed;
		
		#endregion

		#region ================== Properties

		public string Name { get { return name; } }
		public string Title { get { return title; } }
		//public int TileIndex { get { return tileindex; } }
		public bool Sorted { get { return sorted; } }
		public int Color { get { return color; } }
		//public int Arrow { get { return arrow; } }
		public float Radius { get { return radius; } }
		public float Height { get { return height; } }
		//public int Hangs { get { return hangs; } }
		//public int Blocking { get { return blocking; } }
		//public int ErrorCheck { get { return errorcheck; } }
		public bool FixedSize { get { return fixedsize; } }
		//public bool IsDisposed { get { return isdisposed; } }
		//public bool AbsoluteZ { get { return absolutez; } }
		//public float SpriteScale { get { return spritescale; } }
		public IEnumerable<SpriteInfo> Sprites { get { return sprites; } }

		#endregion

		#region ================== Constructor / Disposer
		
		// Constructor
		/*internal SpriteCategory(string name, string title)
		{
			// Initialize
			this.name = name;
			this.title = title;
			this.sprites = new List<SpriteInfo>();
			
			// Set default properties
			//this.tileindex = 0;
			this.sorted = true;
			this.color = 18;
			//this.arrow = 1;
			this.radius = 10;
			this.height = 20;
			//this.hangs = 0;
			//this.blocking = 0;
			//this.errorcheck = 1;
			this.fixedsize = false;
			//this.absolutez = false;
			//this.spritescale = 1.0f;
			
			// We have no destructor
			//GC.SuppressFinalize(this);
		}*/
		
		// Constructor
		internal SpriteCategory(Configuration cfg, string name, IDictionary<string, EnumList> enums)
		{
			// Initialize
			this.name = name;
			this.sprites = new List<SpriteInfo>();
			
			// Read properties
			string key = "spritetypes." + name + ".";
			this.title = cfg.ReadSetting(key + "title", "<category>");
			//this.tileindex = cfg.ReadSetting("thingtypes." + name + ".tileindex", 0);
			this.sorted = (cfg.ReadSetting(key + "sort", 0) != 0);
			this.color = cfg.ReadSetting(key + "color", 0);
			//this.arrow = cfg.ReadSetting("thingtypes." + name + ".arrow", 0);
			this.radius = cfg.ReadSetting(key + "width", 10);
			this.height = cfg.ReadSetting(key + "height", 20);
			//this.hangs = cfg.ReadSetting("thingtypes." + name + ".hangs", 0);
			//this.blocking = cfg.ReadSetting("thingtypes." + name + ".blocking", 0);
			//this.errorcheck = cfg.ReadSetting("thingtypes." + name + ".error", 1);
			this.fixedsize = cfg.ReadSetting(key + "fixedsize", false);
			//this.absolutez = cfg.ReadSetting("thingtypes." + name + ".absolutez", false);
			//this.spritescale = cfg.ReadSetting("thingtypes." + name + ".spritescale", 1.0f);
			
			// Safety
			if(this.radius < 4f) this.radius = 8f;
			
			// Go for all items in category
			IDictionary dic = cfg.ReadSetting(key, new Hashtable());
			foreach(DictionaryEntry de in dic)
			{
				// Check if the item key is numeric
				int index;
				if(int.TryParse(de.Key.ToString(), NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out index))
				{
					// Check if the item value is a structure
					if(de.Value is IDictionary)
					{
						// Create this thing
						sprites.Add(new SpriteInfo(this, index, cfg, enums));
					}
					// Check if the item value is a string
					else if(de.Value is string)
					{
						// Interpret this as the title
						sprites.Add(new SpriteInfo(this, index, de.Value.ToString()));
					}
				}
			}
		}

		// Disposer
		/*internal void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Clean up
				things = null;

				// Done
				isdisposed = true;
			}
		}*/
		
		#endregion

		#region ================== Methods

		// This sorts the category, if preferred
		internal void SortIfNeeded()
		{
			if(sorted) sprites.Sort();
		}
		
		// This adds a thing to the category
		/*internal void AddThing(ThingTypeInfo t)
		{
			// Add
			things.Add(t);
		}*/

		// String representation
		public override string ToString()
		{
			return title;
		}
		
		#endregion
	}
}

