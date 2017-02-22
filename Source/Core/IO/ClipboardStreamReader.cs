#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder.IO
{
	internal static class ClipboardStreamReader
	{
		#region ================== Variables

		/*private struct SidedefData
		{
			public int OffsetX;
			public int OffsetY;
			public int SectorID;
			public string HighTexture;
			public string MiddleTexture;
			public string LowTexture;
			//public Dictionary<string, UniValue> Fields;
			public Dictionary<string, bool> Flags;
		}*/

		#endregion 

		#region ================== Reading

		// This reads from a stream
		public static bool Read(MapSet map, Stream stream) 
		{
			BinaryReader reader = new BinaryReader(stream);

			//mxd. Sanity checks
			int numsectors = reader.ReadInt32();
			if(map.Sectors.Count + numsectors >= General.Map.FormatInterface.MaxSectors)
			{
				General.Interface.DisplayStatus(StatusType.Warning, "Cannot paste: resulting number of sectors (" + (map.Sectors.Count + numsectors) + ") will exceed map format's maximum (" + General.Map.FormatInterface.MaxSectors + ").");
				return false;
			}

			int numwalls = reader.ReadInt32();
			if(map.Linedefs.Count + numwalls >= General.Map.FormatInterface.MaxWalls)
			{
				General.Interface.DisplayStatus(StatusType.Warning, "Cannot paste: resulting number of walls (" + (map.Linedefs.Count + numwalls) + ") will exceed map format's maximum (" + General.Map.FormatInterface.MaxWalls + ").");
				return false;
			}

			int numsprites = reader.ReadInt32();
			if(map.Things.Count + numsprites >= General.Map.FormatInterface.MaxSprites)
			{
				General.Interface.DisplayStatus(StatusType.Warning, "Cannot paste: resulting number of sprites (" + (map.Things.Count + numsprites) + ") will exceed map format's maximum (" + General.Map.FormatInterface.MaxSprites + ").");
				return false;
			}

			// Read the map
			Dictionary<int, Vertex> vertexlink = ReadVertices(map, reader);
			Dictionary<int, BuildSector> sectorlink = ReadSectors(map, reader);
			Dictionary<int, BuildWall> sidedeflink = ReadWalls(reader);
			ReadLines(map, reader, vertexlink, sectorlink, sidedeflink);
			ReadSprites(map, reader);

			// Done
			return true;
		}

		private static Dictionary<int, Vertex> ReadVertices(MapSet map, BinaryReader reader) 
		{
			int count = reader.ReadInt32();

			// Create lookup table
			Dictionary<int, Vertex> link = new Dictionary<int, Vertex>(count);

			// Go for all collections
			map.SetCapacity(map.Vertices.Count + count, 0, 0, 0, 0);
			for(int i = 0; i < count; i++) 
			{
				float x = reader.ReadSingle();
				float y = reader.ReadSingle();

				// Create new item
				Vertex v = map.CreateVertex(new Vector2D(x, y));

				// Add it to the lookup table
				if(v != null) link.Add(i, v);
			}

			// Return lookup table
			return link;
		}

		private static Dictionary<int, BuildSector> ReadSectors(MapSet map, BinaryReader reader) 
		{
			int count = reader.ReadInt32();

			// Create lookup table
			var link = new Dictionary<int, BuildSector>(count);

			// Go for all collections
			map.SetCapacity(0, 0, 0, map.Sectors.Count + count, 0);

			for(int i = 0; i < count; i++) 
			{
				BuildSector bs = new BuildSector();

				// Read Build properteis
				bs.FirstWallIndex = reader.ReadInt32();
				bs.CeilingHeight = reader.ReadInt32();
				bs.FloorHeight = reader.ReadInt32();

				bs.CeilingTileIndex = reader.ReadInt32();
				bs.CeilingSlope = reader.ReadInt32();
				bs.CeilingShade = reader.ReadInt32();
				bs.CeilingPaletteIndex = reader.ReadInt32();
				bs.CeilingOffsetX = reader.ReadInt32();
				bs.CeilingOffsetY = reader.ReadInt32();

				bs.FloorTileIndex = reader.ReadInt32();
				bs.FloorSlope = reader.ReadInt32();
				bs.FloorShade = reader.ReadInt32();
				bs.FloorPaletteIndex = reader.ReadInt32();
				bs.FloorOffsetX = reader.ReadInt32();
				bs.FloorOffsetY = reader.ReadInt32();
				
				bs.Visibility = reader.ReadInt32();
				
				bs.HiTag = reader.ReadInt32();
				bs.LoTag = reader.ReadInt32();
				bs.Extra = reader.ReadInt32();

				// Flags
				bs.CeilingFlags = ReadFlags(reader, General.Map.Config.SectorFlags.Keys);
				bs.FloorFlags = ReadFlags(reader, General.Map.Config.SectorFlags.Keys);

				// Create new item
				Sector s = map.CreateSector();
				if(s != null) 
				{
					// Set properties
					s.Update(bs);
					bs.Sector = s;

					// Add it to the lookup table
					link.Add(i, bs);
				}
			}

			// Return lookup table
			return link;
		}

		// This reads the linedefs and sidedefs
		private static void ReadLines(MapSet map, BinaryReader reader, Dictionary<int, Vertex> vertexlink, Dictionary<int, BuildSector> sectorlink, Dictionary<int, BuildWall> sidedeflink) 
		{
			int count = reader.ReadInt32();

			// Go for all lines
			map.SetCapacity(0, map.Linedefs.Count + count, map.Sidedefs.Count + sidedeflink.Count, 0, 0);
			for(int i = 0; i < count; i++) 
			{
				int v1 = reader.ReadInt32();
				int v2 = reader.ReadInt32();
				int s1 = reader.ReadInt32();
				int s2 = reader.ReadInt32();

				// Check if not zero-length
				if(Vector2D.ManhattanDistance(vertexlink[v1].Position, vertexlink[v2].Position) > 0.0001f) 
				{
					// Create new linedef
					Linedef l = map.CreateLinedef(vertexlink[v1], vertexlink[v2]);
					if(l != null) 
					{
						l.Update();
						l.UpdateCache();

						// Connect walls to the line
						if(s1 > -1) 
						{
							if(s1 < sidedeflink.Count)
								AddWall(map, sidedeflink[s1], l, true, sectorlink);
							else
								General.ErrorLogger.Add(ErrorType.Warning, "Line " + i + " references invalid front wall " + s1 + ". Wall has been removed.");
						}

						if(s2 > -1) 
						{
							if(s2 < sidedeflink.Count)
								AddWall(map, sidedeflink[s2], l, false, sectorlink);
							else
								General.ErrorLogger.Add(ErrorType.Warning, "Line " + i + " references invalid back wall " + s1 + ". Wall has been removed.");
						}
					}
				} 
				else 
				{
					General.ErrorLogger.Add(ErrorType.Warning, "Line " + i + " is zero-length. Line has been removed.");
				}
			}
		}

		private static void AddWall(MapSet map, BuildWall data, Linedef ld, bool front, Dictionary<int, BuildSector> sectorlink) 
		{
			// Create sidedef
			if(sectorlink.ContainsKey(data.SectorID))
			{
				Sidedef s = map.CreateSidedef(ld, front, sectorlink[data.SectorID].Sector);
				if(s != null) s.Update(data);
			} 
			else
			{
				General.ErrorLogger.Add(ErrorType.Warning, "Wall references invalid sector " + data.SectorID + ". Wall has been removed.");
			}
		}

		private static Dictionary<int, BuildWall> ReadWalls(BinaryReader reader) 
		{
			var sidedeflink = new Dictionary<int, BuildWall>();
			int count = reader.ReadInt32();

			for(int i = 0; i < count; i++) 
			{
				BuildWall bw = new BuildWall();

				// Read Build proeprties
				bw.OffsetX = reader.ReadInt32();
				bw.OffsetY = reader.ReadInt32();
				bw.RepeatX = reader.ReadInt32();
				bw.RepeatY = reader.ReadInt32();
				bw.TileIndex = reader.ReadInt32();
				bw.MaskedTileIndex = reader.ReadInt32();
				bw.Shade = reader.ReadInt32();
				bw.PaletteIndex = reader.ReadInt32();

				bw.HiTag = reader.ReadInt32();
				bw.LoTag = reader.ReadInt32();
				bw.Extra = reader.ReadInt32();

				// Read internal properties
				bw.SectorID = reader.ReadInt32();

				// Read flags
				bw.Flags = ReadFlags(reader, General.Map.Config.WallFlags.Keys);

				// Add to the lookup table
				sidedeflink.Add(i, bw);
			}

			return sidedeflink;
		}

		private static void ReadSprites(MapSet map, BinaryReader reader) 
		{
			int count = reader.ReadInt32();

			// Go for all sprites
			map.SetCapacity(0, 0, 0, 0, map.Things.Count + count);
			for(int i = 0; i < count; i++) 
			{
				BuildSprite bs = new BuildSprite();

				// Read Build properties
				bs.X = reader.ReadInt32();
				bs.Y = reader.ReadInt32();
				bs.Z = reader.ReadInt32();
				
				bs.TileIndex = reader.ReadInt32();
				bs.Shade = reader.ReadInt32();
				bs.PaletteIndex = reader.ReadInt32();
				bs.ClipDistance = reader.ReadInt32();
				bs.RepeatX = reader.ReadInt32();
				bs.RepeatY = reader.ReadInt32();
				bs.OffsetX = reader.ReadInt32();
				bs.OffsetY = reader.ReadInt32();
				bs.Angle = reader.ReadInt32();

				bs.Owner = reader.ReadInt32();
				bs.VelX = reader.ReadInt32();
				bs.VelY = reader.ReadInt32();
				bs.VelZ = reader.ReadInt32();
				
				bs.HiTag = reader.ReadInt32();
				bs.LoTag = reader.ReadInt32();
				bs.Extra = reader.ReadInt32();

				// Read flags
				bs.Flags = ReadFlags(reader, General.Map.Config.SpriteFlags.Keys);
				
				// Create new item
				Thing t = map.CreateThing();
				if(t != null) t.Update(bs);
			}
		}

		private static string ReadString(BinaryReader reader) 
		{
			int len = reader.ReadInt32();
			if(len == 0) return string.Empty;
			char[] chars = new char[len];
			for(int i = 0; i < len; ++i) chars[i] = reader.ReadChar();
			return new string(chars);
		}

		private static Dictionary<string, bool> ReadFlags(BinaryReader reader, IEnumerable<string> allflags)
		{
			var flags = new Dictionary<string, bool>(StringComparer.Ordinal);
			int numflags = reader.ReadInt32();
			for(int f = 0; f < numflags; f++) flags.Add(ReadString(reader), reader.ReadBoolean());

			// Add missing flags
			foreach(string flag in allflags)
			{
				if(!flags.ContainsKey(flag)) flags.Add(flag, false);
			}

			return flags;
		} 

		#endregion

	}
}
