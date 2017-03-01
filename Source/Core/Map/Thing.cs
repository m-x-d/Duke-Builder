
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
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.Map
{
	public sealed class Thing : SelectableElement
	{
		#region ================== Variables

		// Map
		private MapSet map;

		// Sector
		private Sector sector;

		// List items
		private LinkedListNode<Thing> selecteditem;
		
		// Build properties
		private Vector3D pos;
		private Dictionary<string, bool> flags;
		private int tileindex;		// Texture index into ART file 
		private int shade;			// Shade offset
		private int paletteindex;	// Palette lookup table number (0 = standard colours) 
		private int clipdistance;	// Size of the movement clipping square (face sprites only) 
		private int repeatx;		// Change pixel size to stretch/shrink textures
		private int repeaty;
		private int offsetx;		// Centre sprite animations 
		private int offsety;
		private int status;			// ???
		//private int anglebuild;		// Angle as entered / stored in file
		private float anglerad;		// Angle in radians

		private int owner; // ???
		private Vector3D vel;

		private int hitag;			// Significance is game-specific
		private int lotag;
		private int extra;

		// Configuration
		private float size;
		private PixelColor color;
		private bool fixedsize;
		//private float iconoffset;	// Arrow or dot coordinate offset on the texture

		#endregion

		#region ================== Properties

		// Map
		public MapSet Map { get { return map; } }

		// Sector
		public Sector Sector { get { return sector; } }

		// Build properties
		public Vector3D Position { get { return pos; } }
		internal Dictionary<string, bool> Flags { get { return flags; } }
		public int TileIndex { get { return tileindex; } set { BeforePropsChange(); tileindex = value; } }
		public int Shade { get { return shade; } set { BeforePropsChange(); shade = value; } }
		public int PaletteIndex { get { return paletteindex; } set { BeforePropsChange(); paletteindex = value; } }
		public int ClipDistance { get { return clipdistance; } set { BeforePropsChange(); clipdistance = value; } }
		public int RepeatX { get { return repeatx; } set { BeforePropsChange(); repeatx = value; } }
		public int RepeatY { get { return repeaty; } set { BeforePropsChange(); repeaty = value; } }
		public int OffsetX { get { return offsetx; } set { BeforePropsChange(); offsetx = value; } }
		public int OffsetY { get { return offsety; } set { BeforePropsChange(); offsety = value; } }
		public int Status { get { return status; } set { BeforePropsChange(); status = value; } }
		public float Angle { get { return anglerad; } set { BeforePropsChange(); anglerad = value; } }
		public int AngleDeg { get { return (int)Math.Round(Angle2D.RadToDeg(anglerad)); } set { BeforePropsChange(); anglerad = Angle2D.DegToRad(value); } }
		//public int AngleBuild { get { return anglebuild; } }

		// ???
		public int Owner { get { return owner; } set { BeforePropsChange(); owner = value; } }
		public Vector3D Velocity { get { return vel; } set { BeforePropsChange(); vel = value; } }
		
		// Identification
		public int HiTag { get { return hitag; } set { BeforePropsChange(); hitag = value; if((hitag < General.Map.FormatInterface.MinTag) || (hitag > General.Map.FormatInterface.MaxTag)) throw new ArgumentOutOfRangeException("HiTag", "Invalid hitag number"); } }
		public int LoTag { get { return lotag; } set { BeforePropsChange(); lotag = value; if((lotag < General.Map.FormatInterface.MinTag) || (lotag > General.Map.FormatInterface.MaxTag)) throw new ArgumentOutOfRangeException("LoTag", "Invalid lotag number"); } }
		public int Extra { get { return extra; } set { BeforePropsChange(); extra = value; if((extra < General.Map.FormatInterface.MinExtra) || (extra > General.Map.FormatInterface.MaxExtra)) throw new ArgumentOutOfRangeException("Extra", "Invalid extra number"); } }

		// Configuration
		public float Size { get { return size; } }
		public PixelColor Color { get { return color; } }
		public bool FixedSize { get { return fixedsize; } }
		//public float IconOffset { get { return iconoffset; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal Thing(MapSet map, int listindex)
		{
			// Initialize
			this.map = map;
			this.listindex = listindex;
			this.flags = new Dictionary<string, bool>(StringComparer.Ordinal);
			this.owner = -1;
			this.extra = -1;
			
			if(map == General.Map.Map) General.Map.UndoRedo.RecAddThing(this);
		}

		// Disposer
		public override void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Already set isdisposed so that changes can be prohibited
				isdisposed = true;

				if(map == General.Map.Map)
					General.Map.UndoRedo.RecRemThing(this);

				// Remove from main list
				map.RemoveThing(listindex);
				
				// Clean up
				map = null;
				sector = null;

				// Dispose base
				base.Dispose();
			}
		}

		#endregion

		#region ================== Management
		
		// Call this before changing properties
		protected override void BeforePropsChange()
		{
			if(map == General.Map.Map) General.Map.UndoRedo.RecPrpThing(this);
		}
		
		// Serialize / deserialize
		internal void ReadWrite(IReadWriteStream s)
		{
			if(!s.IsWriting) BeforePropsChange();

			ReadWrite(s, ref flags);

			s.rwVector3D(ref pos);
			s.rwInt(ref tileindex);
			s.rwInt(ref shade);
			s.rwInt(ref paletteindex);
			s.rwInt(ref clipdistance);
			s.rwInt(ref repeatx);
			s.rwInt(ref repeaty);
			s.rwInt(ref offsetx);
			s.rwInt(ref offsety);
			s.rwInt(ref status);
			s.rwFloat(ref anglerad);

			s.rwInt(ref owner);
			s.rwVector3D(ref vel);

			s.rwInt(ref hitag);
			s.rwInt(ref lotag);
			s.rwInt(ref extra);
			
			//if(!s.IsWriting) anglerad = Angle2D.BuildToReal(anglebuild);
		}

		// This copies all properties to another thing
		public void CopyPropertiesTo(Thing t)
		{
			t.BeforePropsChange();
			
			// Copy build properties
			t.flags = new Dictionary<string, bool>(flags, StringComparer.Ordinal);
			t.pos = pos;
			t.tileindex = tileindex;
			t.shade = shade;
			t.paletteindex = paletteindex;
			t.clipdistance = clipdistance;
			t.repeatx = repeatx;
			t.repeaty = repeaty;
			t.offsetx = offsetx;
			t.offsety = offsety;
			t.status = status;
			t.anglerad = anglerad;
			//t.anglebuild = anglebuild;

			t.owner = owner;
			t.vel = vel;

			t.hitag = hitag;
			t.lotag = lotag;
			t.extra = extra;

			// Copy internal properties
			t.size = size;
			t.color = color;
			//t.iconoffset = iconoffset;
			t.fixedsize = fixedsize;

			base.CopyPropertiesTo(t);
		}

		// This determines which sector the thing is in and links it
		public void DetermineSector()
		{
			//TODO: check if the sprite vertical coords are inside found sector, because overlapping sectors are a thing
			
			// Find the nearest linedef on the map
			Linedef nl = map.NearestLinedef(pos);
			if(nl != null)
			{
				// Check what side of line we are at
				if(nl.SideOfLine(pos) < 0f)
				{
					// Front side
					sector = (nl.Front != null ? nl.Front.Sector : null);
				}
				else
				{
					// Back side
					sector = (nl.Back != null ? nl.Back.Sector : null);
				}
			}
			else
			{
				sector = null;
			}
		}

		// This determines which sector the thing is in and links it
		public void DetermineSector(VisualBlockMap blockmap)
		{
			// Find nearest sectors using the blockmap
			List<Sector> possiblesectors = blockmap.GetBlock(blockmap.GetBlockCoordinates(pos)).Sectors;

			// Check in which sector we are
			sector = null;
			foreach(Sector s in possiblesectors)
			{
				if(s.Intersect(pos))
				{
					sector = s;
					break;
				}
			}
		}

		// Selected
		protected override void DoSelect()
		{
			base.DoSelect();
			selecteditem = map.SelectedThings.AddLast(this);
			General.MainWindow.UpdateStatistics(); //mxd
		}

		// Deselect
		protected override void DoUnselect()
		{
			base.DoUnselect();
			if(selecteditem.List != null) selecteditem.List.Remove(selecteditem);
			selecteditem = null;
			General.MainWindow.UpdateStatistics(); //mxd
		}
		
		#endregion
		
		#region ================== Changes

		// This moves the thing
		// NOTE: This does not update sector! (call DetermineSector)
		public void Move(Vector3D newpos)
		{
			BeforePropsChange();
			
			// Change position
			this.pos = newpos;
			
			General.Map.IsChanged = true;
		}

		// This moves the thing
		// NOTE: This does not update sector! (call DetermineSector)
		public void Move(Vector2D newpos)
		{
			BeforePropsChange();
			
			// Change position
			this.pos = new Vector3D(newpos.x, newpos.y, pos.z);
			
			General.Map.IsChanged = true;
		}

		// This moves the thing
		// NOTE: This does not update sector! (call DetermineSector)
		public void Move(float x, float y, float z)
		{
			BeforePropsChange();
			
			// Change position
			this.pos = new Vector3D(x, y, z);
			
			General.Map.IsChanged = true;
		}
		
		// This rotates the thing
		/*public void Rotate(float newangle)
		{
			BeforePropsChange();
			
			// Change angle
			this.anglerad = newangle;
			//this.anglebuild = Angle2D.RealToBuild(newangle);
			
			General.Map.IsChanged = true;
		}*/
		
		// This rotates the thing
		/*public void Rotate(int newangle)
		{
			BeforePropsChange();
			
			// Change angle
			this.anglerad = Angle2D.DegToRad(newangle);
			//this.anglebuild = newangle;
			
			General.Map.IsChanged = true;
		}*/
		
		// This updates all properties
		// NOTE: This does not update sector! (call DetermineSector)
		internal void Update(BuildSprite src)
		{
			// Apply changes
			this.flags = new Dictionary<string, bool>(src.Flags, StringComparer.Ordinal);
			this.tileindex = src.TileIndex;
			this.shade = src.Shade;
			this.paletteindex = src.PaletteIndex;

			this.clipdistance = src.ClipDistance;
			this.repeatx = src.RepeatX;
			this.repeaty = src.RepeatY;
			this.offsetx = src.OffsetX;
			this.offsety = src.OffsetY;
			this.status = src.Status;
			this.anglerad = Angle2D.BuildToRad(src.Angle);

			this.owner = src.Owner;
			this.Velocity = new Vector3D(src.VelX, src.VelY, src.VelZ);

			this.hitag = src.HiTag;
			this.lotag = src.LoTag;
			this.extra = src.Extra;

			this.Move(src.X, src.Y, src.Z);
			this.sector = src.Sector;
		}
		
		// This updates the settings from configuration
		public void UpdateConfiguration()
		{
			// Lookup settings
			SpriteInfo ti = General.Map.Data.GetSpriteInfo(tileindex);
			
			// Apply size
			size = ti.Radius;
			fixedsize = ti.FixedSize;
			
			// Color valid?
			if((ti.Color >= 0) && (ti.Color < ColorCollection.SpriteColorsCount))
			{
				// Apply color
				color = General.Colors.Colors[ti.Color + General.Colors.SpriteColorsOffset];
			}
			else
			{
				// Unknown thing color
				color = General.Colors.Colors[General.Colors.SpriteColorsOffset];
			}
			
			// Apply icon offset (arrow or dot)
			//iconoffset = (ti.Arrow ? 0f : 0.25f);
		}
		
		#endregion

		#region ================== Methods
		
		// This checks and returns a flag without creating it
		public bool IsFlagSet(string flagname)
		{
			return flags.ContainsKey(flagname) && flags[flagname];
		}

		// This sets a flag
		public void SetFlag(string flagname, bool value)
		{
			if(!flags.ContainsKey(flagname) || (IsFlagSet(flagname) != value))
			{
				BeforePropsChange();
				flags[flagname] = value;
			}
		}
		
		// This returns a copy of the flags dictionary
		public Dictionary<string, bool> GetFlags()
		{
			return new Dictionary<string, bool>(flags, StringComparer.Ordinal);
		}

		// This clears all flags
		public void ClearFlags()
		{
			BeforePropsChange();
			flags.Clear();
		}
		
		// This snaps the vertex to the grid
		public void SnapToGrid()
		{
			// Calculate nearest grid coordinates
			this.Move(General.Map.Grid.SnappedToGrid(pos));
		}

		// This snaps the vertex to the map format accuracy
		/*public void SnapToAccuracy()
		{
			// Round the coordinates
			Vector3D newpos = new Vector3D((float)Math.Round(pos.x, General.Map.FormatInterface.VertexDecimals),
										   (float)Math.Round(pos.y, General.Map.FormatInterface.VertexDecimals),
										   (float)Math.Round(pos.z, General.Map.FormatInterface.VertexDecimals));
			this.Move(newpos);
		}*/
		
		// This returns the distance from given coordinates
		public float DistanceToSq(Vector2D p)
		{
			return Vector2D.DistanceSq(p, pos);
		}

		// This returns the distance from given coordinates
		public float DistanceTo(Vector2D p)
		{
			return Vector2D.Distance(p, pos);
		}

		#endregion
	}
}
