#region ================== Namespaces

using System.Collections.Generic;
using System.IO;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.IO
{
	internal static class ClipboardStreamWriter
	{
		#region ================== Writing

		public static void Write(MapSet map, Stream stream) 
		{
			Write(map.Vertices, map.Linedefs, map.Sidedefs, map.Sectors, map.Things, stream);
		}

		private static void Write(ICollection<Vertex> vertices, ICollection<Linedef> lines, ICollection<Sidedef> walls, 
						  ICollection<Sector> sectors, ICollection<Thing> sprites, Stream stream) 
		{
			// Create collections
			Dictionary<Vertex, int> vertexids = new Dictionary<Vertex, int>();
			Dictionary<Sidedef, int> wallids = new Dictionary<Sidedef, int>();
			Dictionary<Sector, int> sectorids = new Dictionary<Sector, int>();

			// Index the elements in the data structures
			foreach(Vertex v in vertices) vertexids.Add(v, vertexids.Count);
			foreach(Sidedef sd in walls) wallids.Add(sd, wallids.Count);
			foreach(Sector s in sectors) sectorids.Add(s, sectorids.Count);

			BinaryWriter writer = new BinaryWriter(stream);
			
			// Write the data structures to stream
			writer.Write(sectors.Count);
			writer.Write(walls.Count);
			writer.Write(sprites.Count);
			WriteVertices(vertices, writer);
			WriteSectors(sectors, writer, wallids);
			WriteWalls(walls, writer, sectorids);
			WriteLines(lines, writer, wallids, vertexids);
			WriteSprites(sprites, writer);
			writer.Flush();
		}

		private static void WriteVertices(ICollection<Vertex> vertices, BinaryWriter writer) 
		{
			writer.Write(vertices.Count);
			
			foreach(Vertex v in vertices) 
			{
				// Write "static" properties
				writer.Write(v.Position.x);
				writer.Write(v.Position.y);
			}
		}

		// This adds lines
		private static void WriteLines(ICollection<Linedef> lines, BinaryWriter writer, IDictionary<Sidedef, int> wallids, IDictionary<Vertex, int> vertexids) 
		{
			writer.Write(lines.Count);
			
			// Go for all lines
			foreach(Linedef l in lines) 
			{
				// Write "static" properties
				writer.Write(vertexids[l.Start]);
				writer.Write(vertexids[l.End]);

				// Walls
				writer.Write((l.Front != null && wallids.ContainsKey(l.Front)) ? wallids[l.Front] : -1);
				writer.Write((l.Back != null && wallids.ContainsKey(l.Back)) ? wallids[l.Back] : -1);
			}
		}

		// This adds walls
		private static void WriteWalls(ICollection<Sidedef> walls, BinaryWriter writer, IDictionary<Sector, int> sectorids) 
		{
			writer.Write(walls.Count);
			
			// Go for all walls
			foreach(Sidedef s in walls) 
			{
				// Write Build properties
				writer.Write(s.OffsetX);
				writer.Write(s.OffsetY);
				writer.Write(s.RepeatX);
				writer.Write(s.RepeatY);
				writer.Write(s.TileIndex);
				writer.Write(s.MaskedTileIndex);
				writer.Write(s.Shade);
				writer.Write(s.PaletteIndex);
				
				writer.Write(s.HiTag);
				writer.Write(s.LoTag);
				writer.Write(s.Extra);

				writer.Write(sectorids[s.Sector]);
				AddFlags(s.Flags, writer);
			}
		}

		// This adds sectors
		private static void WriteSectors(ICollection<Sector> sectors, BinaryWriter writer, IDictionary<Sidedef, int> wallids) 
		{
			writer.Write(sectors.Count);
			
			// Go for all sectors
			foreach(Sector s in sectors) 
			{
				// Write Build properties
				writer.Write(wallids[s.FirstWall]);
				writer.Write(s.CeilingHeight);
				writer.Write(s.FloorHeight);
				
				writer.Write(s.CeilingTileIndex);
				writer.Write(s.CeilingSlope);
				writer.Write(s.CeilingShade);
				writer.Write(s.CeilingPaletteIndex);
				writer.Write(s.CeilingOffsetX);
				writer.Write(s.CeilingOffsetY);

				writer.Write(s.FloorTileIndex);
				writer.Write(s.FloorSlope);
				writer.Write(s.FloorShade);
				writer.Write(s.FloorPaletteIndex);
				writer.Write(s.FloorOffsetX);
				writer.Write(s.FloorOffsetY);

				writer.Write(s.Visibility);
				
				writer.Write(s.HiTag);
				writer.Write(s.LoTag);
				writer.Write(s.Extra);

				AddFlags(s.CeilingFlags, writer);
				AddFlags(s.FloorFlags, writer);
			}
		}

		// This adds sprites
		private static void WriteSprites(ICollection<Thing> sprites, BinaryWriter writer) 
		{
			writer.Write(sprites.Count);
			
			// Go for all sprites
			foreach(Thing s in sprites) 
			{
				// Write Build properties
				writer.Write((int)s.Position.x);
				writer.Write((int)s.Position.y);
				writer.Write((int)s.Position.z);
				writer.Write(s.TileIndex);
				writer.Write(s.Shade);
				writer.Write(s.PaletteIndex);
				writer.Write(s.ClipDistance);
				writer.Write(s.RepeatX);
				writer.Write(s.RepeatY);
				writer.Write(s.OffsetX);
				writer.Write(s.OffsetY);
				writer.Write(s.Angle);
				
				writer.Write(s.Owner);
				writer.Write((int)s.Velocity.x);
				writer.Write((int)s.Velocity.y);
				writer.Write((int)s.Velocity.z);

				writer.Write(s.HiTag);
				writer.Write(s.LoTag);
				writer.Write(s.Extra);

				AddFlags(s.Flags, writer);
			}
		}

		private static void AddFlags(Dictionary<string, bool> flags, BinaryWriter writer) 
		{
			writer.Write(flags.Count);

			foreach(KeyValuePair<string, bool> group in flags) 
			{
				writer.Write(group.Key.Length);
				writer.Write(group.Key.ToCharArray());
				writer.Write(group.Value);
			}
		}

		#endregion
	}
}
