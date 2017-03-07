
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
using System.Globalization;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Types;

#endregion

namespace mxd.DukeBuilder.Config
{
	public class SpriteInfo : INumberedTitle, IComparable<SpriteInfo>
	{
		#region ================== Constants

		/*public const int THING_BLOCKING_NONE = 0;
		public const int THING_BLOCKING_FULL = 1;
		public const int THING_BLOCKING_HEIGHT = 2;
		public const int THING_ERROR_NONE = 0;
		public const int THING_ERROR_INSIDE = 1;
		public const int THING_ERROR_INSIDE_STUCK = 2;*/
		
		#endregion

		#region ================== Variables

		// Properties
		private int tileindex;
		private string title;
		//private int tileindex;
		//private ActorStructure actor;
		//private long spritelongname;
		private int color;
		//private bool arrow;
		//private float radius;
		//private float height;
		//private bool hangs;
		//private int blocking;
		//private int errorcheck;
		private bool fixedsize;
		private SpriteCategory category;
		//private ArgumentInfo[] args;
		private bool isknown;
		//private bool absolutez;
		//private SizeF spritescale;

		// Property descriptors
		private PropertyInfo lotaginfo;
		private PropertyInfo hitaginfo;
		private PropertyInfo paletteinfo;
		private PropertyInfo angleinfo;
		
		#endregion

		#region ================== Properties

		public int Index { get { return tileindex; } }
		public string Title { get { return title; } }
		//public string Sprite { get { return sprite; } }
		//public ActorStructure Actor { get { return actor; } }
		//public long SpriteLongName { get { return spritelongname; } }
		public int Color { get { return color; } }
		//public bool Arrow { get { return arrow; } }
		//public float Radius { get { return radius; } }
		//public float Height { get { return height; } }
		//public bool Hangs { get { return hangs; } }
		//public int Blocking { get { return blocking; } }
		//public int ErrorCheck { get { return errorcheck; } }
		public bool FixedSize { get { return fixedsize; } }
		public SpriteCategory Category { get { return category; } }
		//public ArgumentInfo[] Args { get { return args; } }
		public bool IsKnown { get { return isknown; } }
		//public bool IsNull { get { return (index == 0); } }
		//public bool AbsoluteZ { get { return absolutez; } }
		//public SizeF SpriteScale { get { return spritescale; } }

		// Property descriptors
		public PropertyInfo HiTagInfo { get { return hitaginfo; } }
		public PropertyInfo LoTagInfo { get { return lotaginfo; } }
		public PropertyInfo PaletteInfo { get { return paletteinfo; } }
		public PropertyInfo AngleInfo { get { return angleinfo; } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal SpriteInfo(int tileindex)
		{
			// Initialize
			this.tileindex = tileindex;
			this.category = null;
			this.title = "Sprite";
			this.color = 0;
			//this.radius = 10f;
			//this.height = 20f;
			this.fixedsize = false;
			this.isknown = false;
		}

		// Constructor
		internal SpriteInfo(SpriteCategory cat, int tileindex, Configuration cfg, IDictionary<string, EnumList> enums)
		{
			string key = "spritetypes." + cat.Name + "." + tileindex.ToString(CultureInfo.InvariantCulture) + ".";
			
			// Initialize
			this.tileindex = tileindex;
			this.category = cat;
			this.isknown = true;
			
			// Read properties
			this.title = cfg.ReadSetting(key + "title", "<" + key + ">");
			//this.sprite = cfg.ReadSetting("thingtypes." + cat.Name + "." + key + ".sprite", cat.Sprite);
			this.color = cfg.ReadSetting(key + "color", cat.Color);
			//this.arrow = (cfg.ReadSetting("thingtypes." + cat.Name + "." + key + ".arrow", cat.Arrow) != 0);
			//this.radius = cfg.ReadSetting(key + "width", cat.Radius);
			//this.height = cfg.ReadSetting(key + "height", cat.Height);
			//this.hangs = (cfg.ReadSetting("thingtypes." + cat.Name + "." + key + ".hangs", cat.Hangs) != 0);
			//this.blocking = cfg.ReadSetting("thingtypes." + cat.Name + "." + key + ".blocking", cat.Blocking);
			//this.errorcheck = cfg.ReadSetting("thingtypes." + cat.Name + "." + key + ".error", cat.ErrorCheck);
			this.fixedsize = cfg.ReadSetting(key + "fixedsize", cat.FixedSize);

			//this.absolutez = cfg.ReadSetting("thingtypes." + cat.Name + "." + key + ".absolutez", cat.AbsoluteZ);
			//float sscale = cfg.ReadSetting("thingtypes." + cat.Name + "." + key + ".spritescale", cat.SpriteScale);
			//this.spritescale = new SizeF(sscale, sscale);
			
			// Read property infos
			hitaginfo = new PropertyInfo(cfg, key + "hitag", PropertyType.HiTag, enums);
			lotaginfo = new PropertyInfo(cfg, key + "lotag", PropertyType.LoTag, enums);
			paletteinfo = new PropertyInfo(cfg, key + "palette", PropertyType.Palette, enums);
			angleinfo = new PropertyInfo(cfg, key + "angle", PropertyType.AngleBuild, enums);

			// Read sprite angles
			List<int> spriteangles = new List<int>();
			string anglessstr = cfg.ReadSetting(key + "spriteangles", string.Empty);
			if(!string.IsNullOrEmpty(anglessstr) && !General.GetNumbersFromString(anglessstr, spriteangles, false))
			{
				General.ErrorLogger.Add(ErrorType.Warning, "Unable to get Sprite angles from string \"" + anglessstr + "\" while parsing Sprite \"" + tileindex + " - " + this.title + "\"");
			}

			if(spriteangles.Count == 8)
			{
				//TODO: implement sprite rotations...
			}

			// Read the args
			//for(int i = 0; i < Linedef.NUM_ARGS; i++)
				//this.args[i] = new ArgumentInfo(cfg, "thingtypes." + cat.Name + "." + key, i, enums);
			
			// Safety
			//if(this.radius < 4f) this.radius = 8f;
			
			// Make long name for sprite lookup
			/*if(this.sprite.Length <= 8)
				this.spritelongname = Lump.MakeLongName(this.sprite);
			else
				this.spritelongname = long.MaxValue;*/
			
			// We have no destructor
			//GC.SuppressFinalize(this);
		}

		// Constructor
		public SpriteInfo(SpriteCategory cat, int tileindex, string title)
		{
			// Initialize
			this.tileindex = tileindex;
			this.category = cat;
			this.title = title;
			//this.actor = null;
			this.isknown = true;
			//this.args = new ArgumentInfo[Linedef.NUM_ARGS];
			//for(int i = 0; i < Linedef.NUM_ARGS; i++) this.args[i] = new ArgumentInfo(i);
			
			// Read properties
			//this.sprite = cat.Sprite;
			this.color = cat.Color;
			//this.arrow = (cat.Arrow != 0);
			//this.radius = cat.Radius;
			//this.height = cat.Height;
			//this.hangs = (cat.Hangs != 0);
			//this.blocking = cat.Blocking;
			//this.errorcheck = cat.ErrorCheck;
			this.fixedsize = cat.FixedSize;
			//this.absolutez = cat.AbsoluteZ;
			//this.spritescale = new SizeF(cat.SpriteScale, cat.SpriteScale);

			// Safety
			//if(this.radius < 4f) this.radius = 8f;
			
			// Make long name for sprite lookup
			/*if(this.sprite.Length <= 8)
				this.spritelongname = Lump.MakeLongName(this.sprite);
			else
				this.spritelongname = long.MaxValue;*/

			// We have no destructor
			//GC.SuppressFinalize(this);
		}

		// Constructor
		/*internal ThingTypeInfo(ThingCategory cat, ActorStructure actor)
		{
			// Initialize
			this.index = actor.DoomEdNum;
			this.category = cat;
			this.title = "";
			this.actor = actor;
			this.isknown = true;
			this.args = new ArgumentInfo[Linedef.NUM_ARGS];
			for(int i = 0; i < Linedef.NUM_ARGS; i++) this.args[i] = new ArgumentInfo(i);
			
			// Read properties
			this.sprite = cat.Sprite;
			this.color = cat.Color;
			this.arrow = (cat.Arrow != 0);
			this.radius = cat.Radius;
			this.height = cat.Height;
			this.hangs = (cat.Hangs != 0);
			this.blocking = cat.Blocking;
			this.errorcheck = cat.ErrorCheck;
			this.fixedsize = cat.FixedSize;
			this.absolutez = cat.AbsoluteZ;
			this.spritescale = new SizeF(cat.SpriteScale, cat.SpriteScale);

			// Safety
			if(this.radius < 4f) this.radius = 8f;
			
			// Apply settings from actor
			ModifyByDecorateActor(actor);
			
			// We have no destructor
			GC.SuppressFinalize(this);
		}*/

		#endregion

		#region ================== Methods
		
		// This updates the properties from a decorate actor
		/*internal void ModifyByDecorateActor(ActorStructure actor)
		{
			// Keep reference to actor
			this.actor = actor;
			
			// Set the title
			if(actor.HasPropertyWithValue("$title"))
				title = actor.GetPropertyAllValues("$title");
			else if(actor.HasPropertyWithValue("tag"))
				title = actor.GetPropertyAllValues("tag");
			else if(string.IsNullOrEmpty(title))
				title = actor.ClassName;
				
			// Remove doublequotes from title
			if((title.Length > 2) && title.StartsWith("\"") && title.EndsWith("\""))
				title = title.Substring(1, title.Length - 2);
			
			// Set sprite
			string suitablesprite = actor.FindSuitableSprite();
			if(!string.IsNullOrEmpty(suitablesprite)) sprite = suitablesprite;
			
			if(this.sprite.Length <= 8)
				this.spritelongname = Lump.MakeLongName(this.sprite);
			else
				this.spritelongname = long.MaxValue;
			
			// Set sprite scale
			if(actor.HasPropertyWithValue("scale"))
			{
				float scale = actor.GetPropertyValueFloat("scale", 0);
				this.spritescale = new SizeF(scale, scale);
			}
			else
			{
				if(actor.HasPropertyWithValue("xscale"))
					this.spritescale.Width = actor.GetPropertyValueFloat("xscale", 0);
				
				if(actor.HasPropertyWithValue("yscale"))
					this.spritescale.Height = actor.GetPropertyValueFloat("yscale", 0);
			}
			
			// Size
			if(actor.HasPropertyWithValue("radius")) radius = actor.GetPropertyValueInt("radius", 0);
			if(actor.HasPropertyWithValue("height")) height = actor.GetPropertyValueInt("height", 0);
			
			// Safety
			if(this.radius < 4f) this.radius = 8f;
			if(this.spritescale.Width <= 0.0f) this.spritescale.Width = 1.0f;
			if(this.spritescale.Height <= 0.0f) this.spritescale.Height = 1.0f;
			
			// Options
			hangs = actor.GetFlagValue("spawnceiling", hangs);
			int blockvalue = (blocking > 0) ? blocking : 2;
			blocking = actor.GetFlagValue("solid", (blocking != 0)) ? blockvalue : 0;
		}*/

		// This is used for sorting
		public int CompareTo(SpriteInfo other)
		{
			return string.Compare(this.title, other.title, true);
		}
		
		// String representation
		public override string ToString()
		{
			return this.title + " (" + tileindex + ")";
		}
		
		#endregion
	}
}
