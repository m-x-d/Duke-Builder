
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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.IO
{
	internal abstract class MapSetIO : IMapSetIO
	{
		#region ================== Variables

		// Map manager
		protected MapManager manager;

		//mxd. Known flags
		protected ISectorFlags sectorflags;
		protected IWallFlags wallflags;
		protected ISpriteFlags spriteflags;

		#endregion

		#region ================== Properties

		public abstract int Version { get; }
		public abstract int MaxWalls { get; }
		public abstract int MaxSectors { get; }
		public abstract int MaxSprites { get; }
		public abstract float MaxSlope { get; }
		public abstract float MinSlope { get; }
		public abstract int MaxImageOffset { get; }
		public abstract int MinImageOffset { get; }
		public abstract int MaxImageRepeat { get; }
		public abstract int MinImageRepeat { get; }
		public abstract int MaxSpriteOffset { get; }
		public abstract int MinSpriteOffset { get; }
		public abstract int MaxSpriteRepeat { get; }
		public abstract int MinSpriteRepeat { get; }
		public abstract int MaxTag { get; }
		public abstract int MinTag { get; }
		public abstract int MaxExtra { get; }
		public abstract int MinExtra { get; }
		public abstract int MaxShade { get; }
		public abstract int MinShade { get; }
		public abstract int MaxVisibility { get; }
		public abstract int MinVisibility { get; }
		public abstract int MaxTileIndex { get; }
		public abstract int MinTileIndex { get; }
		public abstract int MaxCoordinate { get; }
		public abstract int MinCoordinate { get; }
		public abstract int MaxSpriteAngle { get; }
		public abstract int MinSpriteAngle { get; }

		public virtual int LeftBoundary { get { return MinCoordinate; } }
		public virtual int RightBoundary { get { return MaxCoordinate; } }
		public virtual int TopBoundary { get { return MaxCoordinate; } }
		public virtual int BottomBoundary { get { return MinCoordinate; } }

		public ISectorFlags SectorFlags { get { return sectorflags; } }
		public IWallFlags WallFlags { get { return wallflags; } }
		public ISpriteFlags SpriteFlags { get { return spriteflags; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal MapSetIO(MapManager manager)
		{
			// Initialize
			this.manager = manager;
		}
		
		#endregion

		#region ================== Static Methods

		// This returns and instance of the specified IO class
		public static MapSetIO Create(string classname)
		{
			return Create(classname, null);
		}
		
		// This returns and instance of the specified IO class
		public static MapSetIO Create(string classname, MapManager manager)
		{
			try
			{
				// Create arguments
				object[] args = { manager };
				
				// Make the full class name
				string fullname = "mxd.DukeBuilder.IO." + classname;
				
				// Create IO class
				MapSetIO result = (MapSetIO)General.ThisAssembly.CreateInstance(fullname, false,
					BindingFlags.Default, null, args, CultureInfo.CurrentCulture, new object[0]);
				
				// Check result
				if(result != null) return result;

				// No such class
				throw new ArgumentException("No such map format interface found: \"" + classname + "\"");
			}
			// Catch errors
			catch(TargetInvocationException e)
			{
				// Throw the actual exception
				Debug.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
				Debug.WriteLine(e.InnerException.Source + " throws " + e.InnerException.GetType().Name + ":");
				Debug.WriteLine(e.InnerException.Message);
				Debug.WriteLine(e.InnerException.StackTrace);
				throw e.InnerException;
			}
		}
		
		#endregion
		
		#region ================== Methods

		// Required implementations
		public abstract void Read(MapSet map, Stream mapdata);
		public abstract void Write(MapSet map, Stream mapdata);
		
		#endregion
	}
}
