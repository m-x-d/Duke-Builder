
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
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Geometry;

#endregion

namespace mxd.DukeBuilder.Map
{
	public sealed class Sidedef : MapElement
	{
		#region ================== Variables

		// Map
		private MapSet map;

		// List items
		private LinkedListNode<Sidedef> sectorlistitem;

		// Owner
		private Linedef linedef;
		
		// Sector
		private Sector sector;

		// Build Properties
		private Dictionary<string, bool> flags; // cstat

		private int offsetx;		// Offset for aligning textures 
		private int offsety;
		private int repeatx;		// Change pixel size to stretch/shrink textures 
		private int repeaty;
		private int tileindex;		// Texture index into ART file 
		private int maskedtileindex;// Texture index into ART file for masked/one-way walls 
		private int shade;			// Shade offset
		private int paletteindex;	// Palette lookup table number (0 = standard colours)
		
		private int hitag;			// Significance is game-specific
		private int lotag;
		private int extra;

		#endregion

		#region ================== Properties

		public MapSet Map { get { return map; } }
		public bool IsFront { get { return (linedef != null) && (this == linedef.Front); } }
		public Linedef Line { get { return linedef; } }
		public Sidedef Other { get { return (this == linedef.Front ? linedef.Back : linedef.Front); } }
		public Sector Sector { get { return sector; } }
		public float Angle { get { return (IsFront ? linedef.Angle : Angle2D.Normalized(linedef.Angle + Angle2D.PI)); } }
		
		// Build properties
		internal Dictionary<string, bool> Flags { get { return flags; } }

		public int OffsetX { get { return offsetx; } set { BeforePropsChange(); offsetx = value; } }
		public int OffsetY { get { return offsety; } set { BeforePropsChange(); offsety = value; } }
		public int RepeatX { get { return repeatx; } set { BeforePropsChange(); repeatx = value; } }
		public int RepeatY { get { return repeaty; } set { BeforePropsChange(); repeaty = value; } }
		public int TileIndex { get { return tileindex; } set { BeforePropsChange(); tileindex = value; General.Map.IsChanged = true; } }
		public int MaskedTileIndex { get { return maskedtileindex; } set { BeforePropsChange(); maskedtileindex = value; General.Map.IsChanged = true; } }
		public int Shade { get { return shade; } set { BeforePropsChange(); shade = value; } }
		public int PaletteIndex { get { return paletteindex; } set { BeforePropsChange(); paletteindex = value; } }

		// Identification
		public int HiTag { get { return hitag; } set { BeforePropsChange(); hitag = value; if((hitag < General.Map.FormatInterface.MinTag) || (hitag > General.Map.FormatInterface.MaxTag)) throw new ArgumentOutOfRangeException("HiTag", "Invalid hitag number"); } }
		public int LoTag { get { return lotag; } set { BeforePropsChange(); lotag = value; if((lotag < General.Map.FormatInterface.MinTag) || (lotag > General.Map.FormatInterface.MaxTag)) throw new ArgumentOutOfRangeException("LoTag", "Invalid lotag number"); } }
		public int Extra { get { return extra; } set { BeforePropsChange(); extra = value; if((extra < General.Map.FormatInterface.MinExtra) || (extra > General.Map.FormatInterface.MaxExtra)) throw new ArgumentOutOfRangeException("Extra", "Invalid extra number"); } }

		//mxd. Quick access flags
		public bool BlockHitscan { get { return CheckFlag(General.Map.FormatInterface.WallBlockHitscanFlag); } }
		public bool BlockMove { get { return CheckFlag(General.Map.FormatInterface.WallBlockMoveFlag); } }
		public bool ImageFlipX { get { return CheckFlag(General.Map.FormatInterface.WallFlipXFlag); } }
		public bool ImageFlipY { get { return CheckFlag(General.Map.FormatInterface.WallFlipYFlag); } }
		public bool SwapBottomImage { get { return CheckFlag(General.Map.FormatInterface.WallSwapBottomImageFlag); } }
		public bool AlignImageToBottom { get { return CheckFlag(General.Map.FormatInterface.WallAlignImageToBottomFlag); } }
		public bool Masked { get { return CheckFlag(General.Map.FormatInterface.WallMasked); } }
		public bool MaskedSolid { get { return CheckFlag(General.Map.FormatInterface.WallMaskedSolid); } }

		// Clone
		internal int SerializedIndex;

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal Sidedef(MapSet map, int listindex, Linedef l, bool front, Sector s)
		{
			// Initialize
			this.map = map;
			this.listindex = listindex;
			this.flags = new Dictionary<string, bool>();
			this.extra = -1;
			
			// Attach linedef
			this.linedef = l;
			if(l != null)
			{
				if(front)
					l.AttachFrontP(this);
				else
					l.AttachBackP(this);
			}
			
			// Attach sector
			SetSectorP(s);
			
			if(map == General.Map.Map) General.Map.UndoRedo.RecAddSidedef(this);
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
					General.Map.UndoRedo.RecRemSidedef(this);

				// Remove from main list
				map.RemoveSidedef(listindex);

				// Detach from linedef
				if(linedef != null) linedef.DetachSidedefP(this);
				
				// Detach from sector
				SetSectorP(null);

				// Clean up
				sectorlistitem = null;
				linedef = null;
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
			if(!blockpropchange && map == General.Map.Map) General.Map.UndoRedo.RecPrpSidedef(this);
		}
		
		// Serialize / deserialize (passive: this doesn't record)
		internal void ReadWrite(IReadWriteStream s)
		{
			if(!s.IsWriting) BeforePropsChange();
			
			ReadWrite(s, flags);

			s.rwInt(ref offsetx);
			s.rwInt(ref offsety);
			s.rwInt(ref repeatx);
			s.rwInt(ref repeaty);
			s.rwInt(ref tileindex);
			s.rwInt(ref maskedtileindex);
			s.rwInt(ref shade);
			s.rwInt(ref paletteindex);

			s.rwInt(ref hitag);
			s.rwInt(ref lotag);
			s.rwInt(ref extra);
		}
		
		// This copies all properties to another sidedef
		public void CopyPropertiesTo(Sidedef s)
		{
			s.BeforePropsChange();
			
			// Copy build properties
			s.flags = new Dictionary<string, bool>(flags);
			s.offsetx = offsetx;
			s.offsety = offsety;
			s.repeatx = repeatx;
			s.repeaty = repeaty;
			s.tileindex = tileindex;
			s.maskedtileindex = maskedtileindex;
			s.shade = shade;
			s.paletteindex = paletteindex;

			s.hitag = hitag;
			s.lotag = lotag;
			s.extra = extra;
		}

		// This changes sector
		public void SetSector(Sector newsector)
		{
			if(map == General.Map.Map)
				General.Map.UndoRedo.RecRefSidedefSector(this);
			
			// Change sector
			SetSectorP(newsector);
		}

		internal void SetSectorP(Sector newsector)
		{
			// Detach from sector
			if(sector != null) sector.DetachSidedefP(sectorlistitem);

			// Change sector
			sector = newsector;

			// Attach to sector
			if(sector != null)
				sectorlistitem = sector.AttachSidedefP(this);

			General.Map.IsChanged = true;
		}

		// This sets the linedef
		public void SetLinedef(Linedef ld, bool front)
		{
			if(linedef != null) linedef.DetachSidedefP(this);
			
			if(ld != null)
			{
				if(front)
					ld.AttachFront(this);
				else
					ld.AttachBack(this);
			}
		}

		// This sets the linedef (passive: this doesn't tell the linedef and doesn't record)
		internal void SetLinedefP(Linedef ld)
		{
			linedef = ld;
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
			return new Dictionary<string, bool>(flags);
		}

		// This clears all flags
		public void ClearFlags()
		{
			BeforePropsChange();
			flags.Clear();
		}

		//mxd
		private bool CheckFlag(string flagname)
		{
			return (!string.IsNullOrEmpty(flagname) && flags.ContainsKey(flagname) && flags[flagname]);
		}
		
		// This removes textures that are not required
		/*public void RemoveUnneededTextures(bool removemiddle)
		{
			RemoveUnneededTextures(removemiddle, false);
		}*/
		
		// This removes textures that are not required
		/*public void RemoveUnneededTextures(bool removemiddle, bool force)
		{
			BeforePropsChange();
			
			// The middle texture can be removed regardless of any sector tag or linedef action
			if(!MiddleRequired() && removemiddle)
			{
				this.texnamemid = "-";
				this.longtexnamemid = MapSet.EmptyLongName;
				General.Map.IsChanged = true;
			}

			// Check if the line or sectors have no action or tags because
			// if they do, any texture on this side could be needed
			if(((linedef.Tag <= 0) && (linedef.Action == 0) && (sector.Tag <= 0) &&
			    ((Other == null) || (Other.sector.Tag <= 0))) ||
			   force)
			{
				if(!HighRequired())
				{
					this.texnamehigh = "-";
					this.longtexnamehigh = MapSet.EmptyLongName;
					General.Map.IsChanged = true;
				}

				if(!LowRequired())
				{
					this.texnamelow = "-";
					this.longtexnamelow = MapSet.EmptyLongName;
					General.Map.IsChanged = true;
				}
			}
		}*/
		
		/// <summary>
		/// This checks if a texture is required
		/// </summary>
		public bool HighRequired()
		{
			// Doublesided?
			if(Other != null)
			{
				// Texture is required when ceiling of other side is lower
				return (Other.sector.CeilingHeight < this.sector.CeilingHeight);
			}

			return false;
		}

		/// <summary>
		/// This checks if a texture is required
		/// </summary>
		public bool MiddleRequired()
		{
			// Texture is required when the line is singlesided
			return (Other == null);
		}

		/// <summary>
		/// This checks if a texture is required
		/// </summary>
		public bool LowRequired()
		{
			// Doublesided?
			if(Other != null)
			{
				// Texture is required when floor of other side is higher
				return (Other.sector.FloorHeight > this.sector.FloorHeight);
			}

			return false;
		}

		/// <summary>
		/// This returns the height of the upper wall part. Returns 0 when no upper part exists.
		/// </summary>
		/*public int GetHighHeight()
		{
			Sidedef other = this.Other;
			if(other != null)
			{
				int top = this.sector.CeilHeight;
				int bottom = other.sector.CeilHeight;
				int height = top - bottom;
				return (height > 0) ? height : 0;
			}
			else
			{
				return 0;
			}
		}*/

		/// <summary>
		/// This returns the height of the middle wall part.
		/// </summary>
		/*public int GetMiddleHeight()
		{
			Sidedef other = this.Other;
			if(other != null)
			{
				int top = Math.Min(this.Sector.CeilHeight, other.Sector.CeilHeight);
				int bottom = Math.Max(this.Sector.FloorHeight, other.Sector.FloorHeight);
				int height = top - bottom;
				return (height > 0) ? height : 0;
			}
			else
			{
				int top = this.Sector.CeilHeight;
				int bottom = this.Sector.FloorHeight;
				int height = top - bottom;
				return (height > 0) ? height : 0;
			}
		}*/

		/// <summary>
		/// This returns the height of the lower wall part. Returns 0 when no lower part exists.
		/// </summary>
		/*public int GetLowHeight()
		{
			Sidedef other = this.Other;
			if(other != null)
			{
				int top = other.sector.FloorHeight;
				int bottom = this.sector.FloorHeight;
				int height = top - bottom;
				return (height > 0) ? height : 0;
			}
			else
			{
				return 0;
			}
		}*/

		// This copies textures to another sidedef
		// And possibly also the offsets
		public void AddTexturesTo(Sidedef s)
		{
			// s cannot be null
			if(s == null) return;

			s.BeforePropsChange();
			bool copyoffsets = false;

			//TODO: investigate "masking wall" / "bottoms of invisible walls swapped" wall flags
			// Texture set?
			if(tileindex > 0)
			{
				s.tileindex = tileindex;
				copyoffsets = true;
			}

			// Masked texture set?
			if(maskedtileindex > 0 && s.Other != null)
			{
				s.maskedtileindex = maskedtileindex;
				copyoffsets = true;
			}

			// Copy offsets?
			if(copyoffsets)
			{
				s.offsetx = offsetx;
				s.offsety = offsety;
			}

			General.Map.IsChanged = true;
		}
		
		#endregion

		#region ================== Changes

		// This updates all properties
		internal void Update(BuildWall src)
		{
			BeforePropsChange();
			
			// Apply changes
			this.flags = src.Flags;

			this.offsetx = src.OffsetX;
			this.offsety = src.OffsetY;
			this.repeatx = src.RepeatX;
			this.repeaty = src.RepeatY;
			this.tileindex = src.TileIndex;
			this.maskedtileindex = src.MaskedTileIndex;
			this.shade = src.Shade;
			this.paletteindex = src.PaletteIndex;
			
			this.hitag = src.HiTag;
			this.lotag = src.LoTag;
			this.extra = src.Extra;
		}

		// This sets texture
		/*public void SetTextureHigh(string name)
		{
			BeforePropsChange();
			
			texnamehigh = name;
			longtexnamehigh = Lump.MakeLongName(name);
			General.Map.IsChanged = true;
		}

		// This sets texture
		public void SetTextureMid(string name)
		{
			BeforePropsChange();
			
			texnamemid = name;
			longtexnamemid = Lump.MakeLongName(name);
			General.Map.IsChanged = true;
		}

		// This sets texture
		public void SetTextureLow(string name)
		{
			BeforePropsChange();
			
			texnamelow = name;
			longtexnamelow = Lump.MakeLongName(name);
			General.Map.IsChanged = true;
		}*/
		
		#endregion
	}
}
