#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.IO
{
	internal class DukeMapSetIO : MapSetIO
	{
		#region ================== Constants

		private const int BUILD_Z_MULTIPLIER = -16;
		private const float BUILD_ANGLE_MULTIPLIER = 16383.0f;

		#endregion

		#region ================== Variables

		private float maxslope = Angle2D.DegToRad(165);
		private float minslope = Angle2D.DegToRad(15);

		#endregion

		#region ================== Properties

		public override int Version { get { return 7; } }

		public override int MaxWalls { get { return 8192; } }
		public override int MaxSectors { get { return 1024; } }
		public override int MaxSprites { get { return 4096; } }
		public override float MaxSlope { get { return maxslope; } }
		public override float MinSlope { get { return minslope; } }
		//TODO: is that correct? 
		public override int MaxImageOffset { get { return byte.MaxValue; } }
		public override int MinImageOffset { get { return byte.MinValue; } }
		public override int MaxImageRepeat { get { return byte.MaxValue; } }
		public override int MinImageRepeat { get { return byte.MinValue; } }
		public override int MaxSpriteOffset { get { return sbyte.MaxValue; } }
		public override int MinSpriteOffset { get { return sbyte.MinValue; } }
		public override int MaxSpriteRepeat { get { return byte.MaxValue; } }
		public override int MinSpriteRepeat { get { return byte.MinValue; } }
		public override int MaxTag { get { return short.MaxValue; } }
		public override int MinTag { get { return short.MinValue; } }
		public override int MaxExtra { get { return short.MaxValue; } }
		public override int MinExtra { get { return short.MinValue; } }
		public override int MaxShade { get { return sbyte.MaxValue; } }
		public override int MinShade { get { return sbyte.MinValue; } }
		public override int MaxVisibility { get { return byte.MaxValue; } }
		public override int MinVisibility { get { return 0; } }
		public override int MaxTileIndex { get { return 32767; } } // Not sure, but noone would reach this anyway
		public override int MinTileIndex { get { return 0; } }
		public override int MaxCoordinate { get { return 131072; } } // According to mapster32
		public override int MinCoordinate { get { return -131072; } }
		public override int MaxSpriteAngle { get { return 2047; } }
		public override int MinSpriteAngle { get { return 0; } }

		#endregion

		#region ================== Constructor

		public DukeMapSetIO(MapManager manager) : base(manager)
		{
			sectorflags = new DukeSectorFlags();
			wallflags = new DukeWallFlags();
			spriteflags = new DukeSpriteFlags();
		}

		#endregion

		#region ================== Reading

		public override void Read(MapSet map, Stream mapdata)
		{
			using(BinaryReader reader = new BinaryReader(mapdata))
			{
				// Read version
				int version = reader.ReadInt32();
				if(version != Version) throw new NotSupportedException("Unsupported map format version: expected " + Version + ", got " + version);

				// Read player start position
				var playersprite = new BuildSprite();
				playersprite.X = reader.ReadInt32();
				playersprite.Y = -reader.ReadInt32(); // Transform to match DB2 coords...
				playersprite.Z = GetEditorHeight(reader.ReadInt32()); // Transform to match DB2 model/XY map units

				if( playersprite.X < MinCoordinate || playersprite.X > MaxCoordinate ||
					playersprite.Y < MinCoordinate || playersprite.Y > MaxCoordinate ||
					playersprite.Z < MinCoordinate || playersprite.Z > MaxCoordinate)
					throw new NotSupportedException("Invalid player start position: " + playersprite.X + " " + playersprite.Y + " " + playersprite.Z);

				// Read player angle
				playersprite.Angle = reader.ReadUInt16();
				if(playersprite.Angle < MinSpriteAngle || playersprite.Angle > MaxSpriteAngle)
					throw new NotSupportedException("Invalid player start angle: " + playersprite.Angle);

				// Read player start sector
				playersprite.SectorIndex = reader.ReadUInt16();

				// Set player tile index
				playersprite.TileIndex = General.Map.Config.PlayerStartTileIndex;
			
				// Read sectors
				List<BuildSector> buildsectors = ReadSectors(reader);

				// Read walls
				List<BuildWall> buildwalls = ReadWalls(reader);

				// Add player start, because I REALLY don't want to manage it separately...
				List<BuildSprite> buildsprites = new List<BuildSprite> { playersprite };
				// Read sprites
				buildsprites.AddRange(ReadSprites(reader));

				// Create vertices
				CreateVertices(map, buildwalls);

				// Create sectors
				CreateSectors(map, buildsectors);

				// Create lines and walls
				CreateWalls(map, buildwalls, buildsectors);

				// Create sprites
				CreateSprites(map, buildsprites, buildsectors);
			}
		}

		private static void CreateVertices(MapSet map, List<BuildWall> walls)
		{
			// Count how many verts we will actually create...
			int numverts = walls.Count;
			Dictionary<int, BuildWall> prevwalls = new Dictionary<int, BuildWall>();
			foreach(var w in walls)
			{
				if(w.OtherWallIndex > -1) numverts--;

#if DEBUG
				if(prevwalls.ContainsKey(w.NextWallIndex)) throw new NotSupportedException("Invalid OtherWallIndex!");
#endif

				prevwalls[w.NextWallIndex] = w;
			}

			// Always expect the impossible...
			if(numverts < 1) throw new NotSupportedException("Map has no vertices!");

			// Create vertices
			map.SetCapacity(map.Vertices.Count + numverts, 0, 0, 0, 0);
			for(int i = 0; i < walls.Count; i++)
			{
				var w = walls[i];

				// Create or set start vertex?
				if(w.Start == null)
				{
					var prev = prevwalls[i];
					
					// The other side of previous wall already created?
					if(prev.End == null && prev.OtherWallIndex > -1 && walls[prev.OtherWallIndex].Start != null)
					{
						w.Start = walls[prev.OtherWallIndex].Start;
					}
					// Previous wall already created?
					else if(prev.End != null)
					{
						w.Start = prevwalls[i].End;
					}
					// Create new vertex
					else
					{
						Vertex v = map.CreateVertex(new Vector2D(w.StartX, w.StartY));
						prev.End = v;
						w.Start = v;
					}
				}
				
				// Create or set end vertex?
				if(w.End == null)
				{
					var next = walls[w.NextWallIndex];

					// The other side of next wall already created?
					if(next.Start == null && next.OtherWallIndex > -1 && walls[next.OtherWallIndex].End != null)
					{
						w.End = walls[next.OtherWallIndex].End;
					}
					// Next wall already created?
					else if(next.Start != null)
					{
						w.End = next.Start;
					}
					// Create new vertex
					else
					{
						Vertex v = map.CreateVertex(new Vector2D(next.StartX, next.StartY));
						next.Start = v;
						w.End = v;
					}
				}

				// Transfer verts to other side?
				if(w.OtherWallIndex > -1)
				{
					var other = walls[w.OtherWallIndex];
					other.End = w.Start;
					other.Start = w.End;

					// Also assign to other's prev/next walls (and their backsides)...
					var otherprev = walls[other.NextWallIndex];
					otherprev.Start = other.End;
					if(otherprev.OtherWallIndex > -1) walls[otherprev.OtherWallIndex].End = other.End;

					var othernext = prevwalls[w.OtherWallIndex];
					othernext.End = other.Start;
					if(othernext.OtherWallIndex > -1) walls[othernext.OtherWallIndex].Start = other.Start;
				}
			}

			// Integrity checks...
			for(int i = 0; i < walls.Count; i++)
			{
				var w = walls[i];
				
				// Check verts existance
				if(w.Start == null) throw new InvalidDataException("Wall is missing start vertex!");
				if(w.End == null) throw new InvalidDataException("Wall is missing end vertex!");

				// Check length
				if(w.Start.Position == w.End.Position)
				{
					// Find the wall connected to this one...
					foreach(BuildWall o in walls)
					{
						// Re-connect it to the next one...
						if(o != null && o.NextWallIndex == i)
						{
							o.NextWallIndex = w.NextWallIndex;
							break;
						}
					}

					// Remove without altering the array size...
					walls[i] = null;

					// Log warning
					General.ErrorLogger.Add(ErrorType.Warning, "Zero-length wall " + i + " was removed.");
				}
			}
		}

		private static void CreateWalls(MapSet map, List<BuildWall> walls, List<BuildSector> sectors)
		{
			// Count how many lines and walls we will actually create...
			int numwalls = walls.Count;
			int numdoublesidedwalls = 0;

			foreach(BuildWall w in walls)
			{
				if(w == null) numwalls--;
				else if(w.OtherWallIndex > -1) numdoublesidedwalls++;
			}

			int numlines = numwalls - (numdoublesidedwalls / 2);
			
			// Walls are stored per-sector
			map.SetCapacity(0, map.Linedefs.Count + numlines, map.Sidedefs.Count + numwalls, 0, 0);
			Dictionary<int, Linedef> newlines = new Dictionary<int, Linedef>(); // <wall index, line>

			int walloffset = 0;
			foreach(BuildSector bs in sectors)
			{
				for(int i = walloffset; i < walloffset + bs.WallsCount; i++)
				{
					BuildWall w = walls[i];
					if(w == null) continue;

					// Line already created?
					if(w.OtherWallIndex > -1 && newlines.ContainsKey(w.OtherWallIndex))
					{
						// Add back side
						Sidedef back = map.CreateSidedef(newlines[w.OtherWallIndex], false, bs.Sector);
						back.Update(w);
						w.Side = back;
					}
					else
					{
						// Create line
						Linedef l = map.CreateLinedef(w.Start, w.End);
						l.Update();
						l.UpdateCache();

						// Add front side
						Sidedef front = map.CreateSidedef(l, true, bs.Sector);
						front.Update(w);
						w.Side = front;

						// Add to collection
						newlines[i] = l;
					}
				}

				walloffset += bs.WallsCount;
			}

			// Set Sector FirstWalls
			for(int i = 0; i < sectors.Count; i++)
			{
				int wallindex = sectors[i].FirstWallIndex;
				if(wallindex < 0 || wallindex >= walls.Count)
				{
					General.ErrorLogger.Add(ErrorType.Error, "Failed to set First Wall " + wallindex + " of sector " + i + ": corresponding wall does not exist!");
				}
				else
				{
					sectors[i].Sector.FirstWall = walls[wallindex].Side;
				}
			}
		}

		private static void CreateSectors(MapSet map, List<BuildSector> buildsectors)
		{
			// Create sectors
			map.SetCapacity(0, 0, 0, map.Sectors.Count + buildsectors.Count, 0);
			foreach(var bs in buildsectors)
			{
				// Create new item
				Sector s = map.CreateSector();
				s.Update(bs);

				// Add it to the lookup table
				bs.Sector = s;
			}
		}

		private static void CreateSprites(MapSet map, List<BuildSprite> buildsprites, List<BuildSector> sectors)
		{
			// Create sprites
			map.SetCapacity(0, 0, 0, 0, map.Things.Count + buildsprites.Count);

			for(int i = 0; i < buildsprites.Count; i++)
			{
				var bs = buildsprites[i];

				// Link sector
				if(bs.SectorIndex < sectors.Count)
					bs.Sector = sectors[bs.SectorIndex].Sector;
				else
					General.ErrorLogger.Add(ErrorType.Warning, "Sprite " + i + " references non-existing sector " + bs.SectorIndex);

				// Create new item
				Thing t = map.CreateThing();
				t.Update(bs);
			}
		}

		private List<BuildSector> ReadSectors(BinaryReader reader)
		{
			// Read sectors count
			int num = reader.ReadUInt16();

			// Prepare collection
			var result = new List<BuildSector>(num);

			// Read sectors
			for(int i = 0; i < num; i++)
			{
				BuildSector s = new BuildSector();
				
				// Read properties from stream
				s.FirstWallIndex = reader.ReadInt16();
				s.WallsCount = reader.ReadInt16(); // Number of walls in sector

				// Transform to match DB2 model/XY map units
				s.CeilingHeight = GetEditorHeight(reader.ReadInt32()); // Z-coordinate (height) of ceiling at first point of sector
				s.FloorHeight = GetEditorHeight(reader.ReadInt32());   // Z-coordinate (height) of floor at first point of sector

				// Make string flags
				int ceilingstat = reader.ReadUInt16();
				int floorstat = reader.ReadUInt16();
				foreach(int flag in manager.Config.SortedSectorFlags)
				{
					s.CeilingFlags[flag.ToString()] = ((ceilingstat & flag) == flag);
					s.FloorFlags[flag.ToString()] = ((floorstat & flag) == flag);
				}

				s.CeilingTileIndex = reader.ReadUInt16();
				s.CeilingSlope = BuildSlopeToRadians(reader.ReadInt16());
				s.CeilingShade = reader.ReadSByte();
				s.CeilingPaletteIndex = reader.ReadByte();
				s.CeilingOffsetX = reader.ReadByte();
				s.CeilingOffsetY = reader.ReadByte();

				s.FloorTileIndex = reader.ReadUInt16();
				s.FloorSlope = BuildSlopeToRadians(reader.ReadInt16());
				s.FloorShade = reader.ReadSByte();
				s.FloorPaletteIndex = reader.ReadByte();
				s.FloorOffsetX = reader.ReadByte();
				s.FloorOffsetY = reader.ReadByte();

				s.Visibility = reader.ReadByte();

				reader.BaseStream.Position += 1; // Skip padding byte 

				s.LoTag = reader.ReadInt16();
				s.HiTag = reader.ReadInt16();
				s.Extra = reader.ReadInt16();

				// Add to collection
				result.Add(s);
			}

			return result;
		}

		private List<BuildWall> ReadWalls(BinaryReader reader)
		{
			// Read walls count
			int num = reader.ReadUInt16();

			// Prepare collection
			var result = new List<BuildWall>(num);

			// Read walls
			for(int i = 0; i < num; i++)
			{
				// Read properties from stream
				BuildWall w = new BuildWall();

				// Start vertex
				w.StartX = reader.ReadInt32();
				w.StartY = -reader.ReadInt32(); // Transform to match DB2 coords...

				w.NextWallIndex = reader.ReadInt16();
				w.OtherWallIndex = reader.ReadInt16();
				w.OtherSectorIndex = reader.ReadInt16();

				// Make string flags
				int cstat = reader.ReadUInt16();
				foreach(int flag in manager.Config.SortedWallFlags)
				{
					w.Flags[flag.ToString()] = ((cstat & flag) == flag);
				}

				w.TileIndex = reader.ReadInt16();
				w.MaskedTileIndex = reader.ReadInt16();
				w.Shade = reader.ReadSByte();
				w.PaletteIndex = reader.ReadByte();
				w.RepeatX = reader.ReadByte();
				w.RepeatY = reader.ReadByte();
				w.OffsetX = reader.ReadByte();
				w.OffsetY = reader.ReadByte();

				w.LoTag = reader.ReadInt16();
				w.HiTag = reader.ReadInt16();
				w.Extra = reader.ReadInt16();

				// Add to collection
				result.Add(w);
			}

			return result;
		}

		private List<BuildSprite> ReadSprites(BinaryReader reader)
		{
			// Read sprites count
			int num = reader.ReadUInt16();

			// Prepare collection
			var result = new List<BuildSprite>(num);

			// Read sprites
			for(int i = 0; i < num; i++)
			{
				// Read properties from stream
				BuildSprite s = new BuildSprite();

				s.X = reader.ReadInt32();
				s.Y = -reader.ReadInt32(); // Transform to match DB2 coords...
				s.Z = GetEditorHeight(reader.ReadInt32()); // Transform to match DB2 model/XY map units

				// Make string flags
				int cstat = reader.ReadUInt16();
				foreach(int flag in manager.Config.SortedSpriteFlags)
				{
					s.Flags[flag.ToString()] = ((cstat & flag) == flag);
				}

				s.TileIndex = reader.ReadInt16();
				s.Shade = reader.ReadSByte();
				s.PaletteIndex = reader.ReadByte();
				s.ClipDistance = reader.ReadByte();

				reader.BaseStream.Position += 1; // Skip filler byte

				s.RepeatX = reader.ReadByte();
				s.RepeatY = reader.ReadByte();
				s.OffsetX = reader.ReadByte();
				s.OffsetY = reader.ReadByte();

				s.SectorIndex = reader.ReadInt16();
				s.Status = reader.ReadInt16();
				s.Angle = reader.ReadInt16(); // Stored in radians!
				s.Owner = reader.ReadInt16();
				
				s.VelX = reader.ReadInt16();
				s.VelY = reader.ReadInt16();
				s.VelZ = reader.ReadInt16();

				s.LoTag = reader.ReadInt16();
				s.HiTag = reader.ReadInt16();
				s.Extra = reader.ReadInt16();

				// Add to collection
				result.Add(s);
			}

			return result;
		}

		#endregion

		#region ================== Writing

		public override void Write(MapSet map, Stream mapdata)
		{
			// Make collections
			var buildwallids = new Dictionary<int, BuildWall>(map.Sidedefs.Count);
			var wallids = new Dictionary<Sidedef, int>(map.Sidedefs.Count);
			var sectorids = new Dictionary<Sector, int>(map.Sectors.Count);
			
			// Walls must be stored on per-sector basis...
			foreach(Sector s in map.Sectors)
			{
				if(s.Sidedefs == null || s.Sidedefs.Count < 3) continue;

				var sectorsidelists = Tools.GetSortedSectorSides(s);
				if(sectorsidelists.Count == 0) continue;

				foreach(List<Sidedef> sideslist in sectorsidelists)
				{
					// Add to lookup table
					for(int i = 0; i < sideslist.Count; i++)
					{
						wallids.Add(sideslist[i], wallids.Count);
					}
					
					// Create BuildWalls
					for(int i = 0; i < sideslist.Count; i++)
					{
						var bw = new BuildWall(sideslist[i]);
						int next = (i < sideslist.Count - 1 ? i + 1 : 0); // Connect last wall to the first one
						bw.NextWallIndex = wallids[sideslist[next]];
						bw.Side = sideslist[i];
						buildwallids.Add(buildwallids.Count, bw);
					}
				}

				sectorids.Add(s, sectorids.Count);
			}

			// Fill in OtherWallIndex and OtherSectorIndex
			foreach(var group in wallids)
			{
				if(group.Key.Other != null)
				{
					buildwallids[group.Value].OtherWallIndex = wallids[group.Key.Other];
					buildwallids[group.Value].OtherSectorIndex = sectorids[group.Key.Other.Sector];
				}
			}

			// Write map structures
			using(BinaryWriter writer = new BinaryWriter(mapdata))
			{
				// Write map header
				writer.Write(Version);

				// First sprite must be player start
				Thing playerstart = map.GetThingByIndex(0);
				
				//TODO: check this BEFORE saving the map!
				if(playerstart == null) throw new InvalidDataException("Map has no Player start!");
				if(playerstart.Sector == null || playerstart.Sector.IsDisposed) playerstart.DetermineSector();
				if(playerstart.Sector == null) throw new InvalidDataException("Player start must be inside a sector!");

				// Write player start point
				writer.Write((int)playerstart.Position.x);
				writer.Write((int)-playerstart.Position.y); // Transform to match Build coords...
				writer.Write(GetBuildHeight((int)playerstart.Position.z));  // Transform to match Build coords...

				// Write player starting angle
				writer.Write((ushort)Angle2D.RadToBuild(playerstart.Angle));

				// Write sector number containing the start point
				writer.Write((ushort)sectorids[playerstart.Sector]);

				// Write sectors
				WriteSectors(writer, sectorids.Keys);

				// Write walls
				WriteWalls(writer, buildwallids.Values);

				// Write sprites
				WriteSprites(writer, map.Things, sectorids);
			}
		}

		private static void WriteSectors(BinaryWriter writer, ICollection<Sector> sectors)
		{
			// UINT16LE Number of sectors in the map 
			writer.Write((ushort)sectors.Count);

			int firstwall = 0;
			foreach(Sector s in sectors)
			{
				// INT16LE Index to first wall in sector. FirstWall is always written first, so...
				writer.Write((short)firstwall);
				firstwall += s.Sidedefs.Count;

				// INT16LE Number of walls in sector
				writer.Write((short)s.Sidedefs.Count);

				// INT32LE ceilingz and floorz
				writer.Write(GetBuildHeight(s.CeilingHeight)); // Transform to match Build coords...
				writer.Write(GetBuildHeight(s.FloorHeight));   // Transform to match Build coords...

				// INT16LE ceilingstat and floorcstat
				writer.Write((ushort)FlagsToInt(s.CeilingFlags));
				writer.Write((ushort)FlagsToInt(s.FloorFlags));

				// Ceiling props
				writer.Write((short)s.CeilingTileIndex);
				writer.Write(RadiansToBuildSlope(s.CeilingSlope));
				writer.Write((sbyte)s.CeilingShade);
				writer.Write((byte)s.CeilingPaletteIndex);
				writer.Write((byte)s.CeilingOffsetX);
				writer.Write((byte)s.CeilingOffsetY);

				// Floor props
				writer.Write((short)s.FloorTileIndex);
				writer.Write(RadiansToBuildSlope(s.FloorSlope));
				writer.Write((sbyte)s.FloorShade);
				writer.Write((byte)s.FloorPaletteIndex);
				writer.Write((byte)s.FloorOffsetX);
				writer.Write((byte)s.FloorOffsetY);

				// UINT8 Visibility
				writer.Write((byte)s.Visibility);
				writer.Write((byte)0); // Padding byte

				// INT16LE lotag, hitag, extra
				writer.Write((short)s.LoTag);
				writer.Write((short)s.HiTag);
				writer.Write((short)s.Extra);
			}
		}

		private static void WriteWalls(BinaryWriter writer, ICollection<BuildWall> walls)
		{
			// UINT16LE Number of walls in the map 
			writer.Write((ushort)walls.Count);

			// Write walls, those should be already in per-sector order and with all Build properties set
			foreach(var w in walls)
			{
				// INT32LE x, y
				Vector2D start = (w.Side.IsFront ? w.Side.Line.Start : w.Side.Line.End).Position;
				writer.Write((int)start.x);
				writer.Write((int)-start.y); // Transform to match Build coords...

				// INT16LE point2, nextwall, nextsector
				writer.Write((short)w.NextWallIndex);
				writer.Write((short)w.OtherWallIndex);
				writer.Write((short)w.OtherSectorIndex);

				// INT16LE cstat
				writer.Write((ushort)FlagsToInt(w.Flags));

				// INT16LE picnum, overpicnum
				writer.Write((short)w.TileIndex);
				writer.Write((short)w.MaskedTileIndex);

				// INT8 shade, UINT8 pal
				writer.Write((sbyte)w.Shade);
				writer.Write((byte)w.PaletteIndex);

				// UINT8 xrepeat, yrepeat, xpanning, ypanning
				writer.Write((byte)w.RepeatX);
				writer.Write((byte)w.RepeatY);
				writer.Write((byte)w.OffsetX);
				writer.Write((byte)w.OffsetY);

				// INT16LE lotag, hitag, extra
				writer.Write((short)w.LoTag);
				writer.Write((short)w.HiTag);
				writer.Write((short)w.Extra);
			}
		}

		private static void WriteSprites(BinaryWriter writer, ICollection<Thing> sprites, Dictionary<Sector, int> sectorids)
		{
			List<Thing> validsprites = new List<Thing>(sprites.Count);

			bool first = true;
			foreach(var s in sprites)
			{
				// Skip player start sprite...
				if(first)
				{
					first = false;
					continue;
				}

				// Sprites must be inside an existing sector
				if(s.Sector == null || s.Sector.IsDisposed) s.DetermineSector();
				if(s.Sector == null) continue;
				validsprites.Add(s);
			}
			
			// UINT16LE Number of sprites in the map 
			writer.Write((ushort)validsprites.Count);

			foreach(Thing s in validsprites)
			{
				// INT32LE x, y, z
				writer.Write((int)s.Position.x);
				writer.Write((int)-s.Position.y); // Transform to match Build coords...
				writer.Write(GetBuildHeight((int)s.Position.z)); // Transform to match Build coords...

				// INT16LE cstat
				writer.Write((ushort)FlagsToInt(s.Flags));

				// INT16LE picnum
				writer.Write((short)s.TileIndex);

				// INT8 shade, UINT8 pal
				writer.Write((sbyte)s.Shade);
				writer.Write((byte)s.PaletteIndex);

				// UINT8 clipdist, filler
				writer.Write((byte)s.ClipDistance);
				writer.Write((byte)0);

				// UINT8 xrepeat, yrepeat, xpanning, ypanning
				writer.Write((byte)s.RepeatX);
				writer.Write((byte)s.RepeatY);
				writer.Write((byte)s.OffsetX);
				writer.Write((byte)s.OffsetY);

				// INT16LE sectnum, statnum, ang, owner
				writer.Write((short)sectorids[s.Sector]);
				writer.Write((short)s.Status);
				writer.Write((short)Angle2D.RadToBuild(s.Angle));
				writer.Write((short)s.Owner);

				// INT16LE xvel, yvel, zvel
				writer.Write((short)s.Velocity.x);
				writer.Write((short)s.Velocity.y);
				writer.Write((short)s.Velocity.z);

				// INT16LE lotag, hitag, extra
				writer.Write((short)s.LoTag);
				writer.Write((short)s.HiTag);
				writer.Write((short)s.Extra);
			}
		}

		private static int FlagsToInt(Dictionary<string, bool> flags)
		{
			// Convert flags
			int intflags = 0;
			foreach(KeyValuePair<string, bool> f in flags)
			{
				int fnum;
				if(f.Value && int.TryParse(f.Key, out fnum)) intflags |= fnum;
			}

			return intflags;
		}

		private static int GetEditorHeight(int buildheight)
		{
			return (int)Math.Round((float)buildheight / BUILD_Z_MULTIPLIER);
		}

		private static int GetBuildHeight(int height)
		{
			return height * BUILD_Z_MULTIPLIER;
		}

		// Converts build slope angle [-8192 .. 8192] to radians (0 .. PI)
		private static float BuildSlopeToRadians(int anglebuild)
		{
			return anglebuild * Angle2D.PI / BUILD_ANGLE_MULTIPLIER + Angle2D.PIHALF;
		}

		// Converts radians (0 .. PI) to build slope angle [8192 .. -8192]
		private static short RadiansToBuildSlope(float anglerad)
		{
			return (short)Math.Round((anglerad - Angle2D.PIHALF) * BUILD_ANGLE_MULTIPLIER / Angle2D.PI);
		}

		#endregion
	}
}
