#region ================== Namespaces

using System.Collections.Generic;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.IO
{
	#region ================== BuildWall

	public class BuildWall
	{
		// Build properties
		public Dictionary<string, bool> Flags;

		public int StartX;
		public int StartY;

		public int NextWallIndex; // Index to next wall on the right (always in the same sector) 
		public int OtherWallIndex; // Index to wall on other side of wall (-1 if there is no sector there)
		public int OtherSectorIndex; // Index to sector on other side of wall (-1 if there is no sector)
		
		public int OffsetX;
		public int OffsetY;
		public int RepeatX;
		public int RepeatY;
		public int TileIndex;
		public int MaskedTileIndex;
		public int Shade;
		public int PaletteIndex;

		public int HiTag;
		public int LoTag;
		public int Extra;

		// Internal properties
		public Vertex Start;
		public Vertex End;
		public int SectorID;
		public Sidedef Side;

		public BuildWall()
		{
			Flags = new Dictionary<string, bool>();
			Extra = -1;
		}

		public BuildWall(Sidedef src)
		{
			Flags = new Dictionary<string, bool>(src.Flags);

			var v = (src.IsFront ? src.Line.Start : src.Line.End);
			StartX = (int)v.Position.x;
			StartY = (int)v.Position.y;

			// These must be set separately...
			NextWallIndex = -1;
			OtherWallIndex = -1;
			OtherSectorIndex = -1;

			OffsetX = src.OffsetX;
			OffsetY = src.OffsetY;
			RepeatX = src.RepeatX;
			RepeatY = src.RepeatY;
			TileIndex = src.TileIndex;
			MaskedTileIndex = src.MaskedTileIndex;
			Shade = src.Shade;
			PaletteIndex = src.PaletteIndex;

			HiTag = src.HiTag;
			LoTag = src.LoTag;
			Extra = src.Extra;
		}

		public void ApplyTo(Sidedef s)
		{
			s.BeginPropertiesChange();
			
			s.ClearFlags();
			foreach(var group in Flags)
				s.SetFlag(group.Key, group.Value);

			s.OffsetX = OffsetX;
			s.OffsetY = OffsetY;
			s.RepeatX = RepeatX;
			s.RepeatY = RepeatY;
			s.TileIndex = TileIndex;
			s.MaskedTileIndex = MaskedTileIndex;
			s.Shade = Shade;
			s.PaletteIndex = PaletteIndex;

			s.HiTag = HiTag;
			s.LoTag = LoTag;
			s.Extra = Extra;

			s.EndPropertiesChange();
		}
	}

	#endregion

	#region ================== BuildSprite

	public class BuildSprite
	{
		// Build properties
		public Dictionary<string, bool> Flags;

		public int X;
		public int Y;
		public int Z;
		public int TileIndex;		// Texture index into ART file 
		public int Shade;			// Shade offset
		public int PaletteIndex;	// Palette lookup table number (0 = standard colours) 
		public int ClipDistance;	// Size of the movement clipping square (face sprites only) 
		public int RepeatX;			// Change pixel size to stretch/shrink textures
		public int RepeatY;
		public int OffsetX;			// Centre sprite animations 
		public int OffsetY;
		public int SectorIndex;		// Current sector of sprite's position
		public int Status;			// Current status of sprite (inactive, monster, bullet, etc.) (???)
		public int Angle;			// Build Angle 

		public int Owner; // ???
		public int VelX;
		public int VelY;
		public int VelZ;
		
		public int HiTag;
		public int LoTag;
		public int Extra;

		// Internal properties
		public Sector Sector;

		public BuildSprite()
		{
			Flags = new Dictionary<string, bool>();
			Owner = -1;
			Extra = -1;
		}

		public BuildSprite(Thing src)
		{
			Flags = new Dictionary<string, bool>(src.Flags);

			TileIndex = src.TileIndex;
			Shade = src.Shade;
			PaletteIndex = src.PaletteIndex;

			ClipDistance = src.ClipDistance;
			RepeatX = src.RepeatX;
			RepeatY = src.RepeatY;
			OffsetX = src.OffsetX;
			OffsetY = src.OffsetY;
			Status = src.Status;
			Angle = src.AngleDeg; // In degrees!

			Owner = src.Owner;
			VelX = (int)src.Velocity.x;
			VelY = (int)src.Velocity.y;
			VelZ = (int)src.Velocity.z;

			HiTag = src.HiTag;
			LoTag = src.LoTag;
			Extra = src.Extra;

			X = (int)src.Position.x;
			Y = (int)src.Position.y;
			Z = (int)src.Position.z;
		}

		public void ApplyTo(Thing s)
		{
			s.BeginPropertiesChange();
			
			s.Move(new Vector3D(X, Y, Z));

			s.ClearFlags();
			foreach(var group in Flags)
				s.SetFlag(group.Key, group.Value);

			s.TileIndex = TileIndex;
			s.Shade = Shade;
			s.PaletteIndex = PaletteIndex;

			s.ClipDistance = ClipDistance;
			s.RepeatX = RepeatX;
			s.RepeatY = RepeatY;
			s.OffsetX = OffsetX;
			s.OffsetY = OffsetY;
			s.Status = Status;
			s.AngleDeg = Angle;

			s.Owner = Owner;
			s.Velocity = new Vector3D(VelX, VelY, VelZ);

			s.HiTag = HiTag;
			s.LoTag = LoTag;
			s.Extra = Extra;

			s.EndPropertiesChange();
		}
	}

	#endregion

	#region ================== BuildSector

	public class BuildSector
	{
		// Build properties
		public int FirstWallIndex;
		public int WallsCount;
		public int CeilingHeight;
		public int FloorHeight;

		public Dictionary<string, bool> FloorFlags;
		public Dictionary<string, bool> CeilingFlags;

		public int CeilingTileIndex;
		public float CeilingSlope; // in radians!
		public int CeilingShade;
		public int CeilingPaletteIndex;
		public int CeilingOffsetX;
		public int CeilingOffsetY;

		public int FloorTileIndex;
		public float FloorSlope; // in radians!
		public int FloorShade;
		public int FloorPaletteIndex;
		public int FloorOffsetX;
		public int FloorOffsetY;

		public int Visibility;
		
		public int HiTag;
		public int LoTag;
		public int Extra;

		// Internal properties
		public Sector Sector;

		public BuildSector()
		{
			FloorFlags = new Dictionary<string, bool>();
			CeilingFlags = new Dictionary<string, bool>();
			Extra = -1;
		}

		public BuildSector(Sector src)
		{
			// FirstWallIndex must be set separately!
			WallsCount = src.Sidedefs.Count;
			CeilingHeight = src.CeilingHeight;
			FloorHeight = src.FloorHeight;

			FloorFlags = new Dictionary<string, bool>(src.FloorFlags);
			CeilingFlags = new Dictionary<string, bool>(src.CeilingFlags);

			CeilingTileIndex = src.CeilingTileIndex;
			CeilingSlope = src.CeilingSlope;
			CeilingShade = src.CeilingShade;
			CeilingPaletteIndex = src.CeilingPaletteIndex;
			CeilingOffsetX = src.CeilingOffsetX;
			CeilingOffsetY = src.CeilingOffsetY;

			FloorTileIndex = src.FloorTileIndex;
			FloorSlope = src.FloorSlope;
			FloorShade = src.FloorShade;
			FloorPaletteIndex = src.FloorPaletteIndex;
			FloorOffsetX = src.FloorOffsetX;
			FloorOffsetY = src.FloorOffsetY;

			Visibility = src.Visibility;

			HiTag = src.HiTag;
			LoTag = src.LoTag;
			Extra = src.Extra;
		}

		public void ApplyTo(Sector s)
		{
			s.BeginPropertiesChange();
			
			s.CeilingHeight = CeilingHeight;
			s.FloorHeight = FloorHeight;

			s.ClearFlags(false);
			foreach(var group in CeilingFlags)
				s.SetFlag(group.Key, group.Value, false);

			s.ClearFlags(true);
			foreach(var group in FloorFlags)
				s.SetFlag(group.Key, group.Value, true);

			s.CeilingTileIndex = CeilingTileIndex;
			s.CeilingSlope = CeilingSlope;
			s.CeilingShade = CeilingShade;
			s.CeilingPaletteIndex = CeilingPaletteIndex;
			s.CeilingOffsetX = CeilingOffsetX;
			s.CeilingOffsetY = CeilingOffsetY;

			s.FloorTileIndex = FloorTileIndex;
			s.FloorSlope = FloorSlope;
			s.FloorShade = FloorShade;
			s.FloorPaletteIndex = FloorPaletteIndex;
			s.FloorOffsetX = FloorOffsetX;
			s.FloorOffsetY = FloorOffsetY;

			s.Visibility = Visibility;

			s.HiTag = HiTag;
			s.LoTag = LoTag;
			s.Extra = Extra;

			s.EndPropertiesChange();
		}
	}

	#endregion
}
