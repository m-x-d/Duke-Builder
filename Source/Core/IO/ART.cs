#region ================== Namespaces

using System.Collections.Generic;
using System.Drawing;
using System.IO;

#endregion

namespace mxd.DukeBuilder.IO
{
	internal class ART
	{
		#region ================== Variables

		private string name;
		private int starttile;
		private int endtile;
		private Dictionary<int, Tile> tiles;
		//private bool isdisposed;
		//private Size tilesizes;

		#endregion

		#region ================== Properties

		public string Name { get { return name; } }
		public Dictionary<int, Tile> Tiles { get { return tiles; } }
		//public long StartTile { get { return starttile; } }
		//public long EndTile { get { return endtile; } }

		#endregion

		#region ================== Constructor

		internal ART(string name)
		{
			this.name = name;
			this.tiles = new Dictionary<int, Tile>(256);
		}

		#endregion

		#region ================== Methods

		internal bool Create(Stream source)
		{
			// Read header
			BinaryReader reader = new BinaryReader(source);

			// long artversion
			int artversion = reader.ReadInt32();
			if(artversion != 1)
			{
				General.ErrorLogger.Add(ErrorType.Error, "Unable to load \"" + name + "\": unsupported ART file version (expected 1, got " + artversion + ")");
				return false;
			}

			// long numtiles. Unused
			reader.ReadInt32();

			// long localtilestart
			starttile = reader.ReadInt32();

			// long localtileend
			endtile = reader.ReadInt32();

			// Just in case...
			if(starttile >= endtile || starttile < 0) return false;

			// Log it...
			General.WriteLogLine("Loading tile range " + starttile + " - " + endtile + " from \"" + name + "\"");

			int numtiles = endtile - starttile + 1;
			Size[] tilesizes = new Size[numtiles];
			Point[] tileoffsets = new Point[numtiles];
			
			// short tilesizx[localtileend-localtilestart+1];
			for(int i = 0; i < numtiles; i++) tilesizes[i].Width = reader.ReadInt16();

			// short tilesizy[localtileend-localtilestart+1];
			for(int i = 0; i < numtiles; i++) tilesizes[i].Height = reader.ReadInt16();

			// long picanm[localtileend-localtilestart+1];
			for(int i = 0; i < numtiles; i++)
			{
				reader.BaseStream.Position += 1;		// BYTE Animation type
				tileoffsets[i].X = reader.ReadSByte();	// SBYTE OffsetX
				tileoffsets[i].Y = reader.ReadSByte();	// SBYTE OffsetY
				reader.BaseStream.Position += 1;		// BYTE Animation speed
			}

			// Create tiles
			int offset = (int)reader.BaseStream.Position;
			for(int i = 0; i < numtiles; i++)
			{
				int tilesize = tilesizes[i].Width * tilesizes[i].Height;
				if(tilesize > 0)
				{
					int tileindex = starttile + i;
					tiles.Add(tileindex, new Tile(source, tileindex, 
						tilesizes[i].Width, tilesizes[i].Height, 
						tileoffsets[i].X, tileoffsets[i].Y, 
						offset, tilesize));
				}

				offset += tilesize;
			}

			// Done
			return tiles.Count > 0;
		}

		/*public void Dispose()
		{
			if(!isdisposed)
			{
				foreach(Tile t in tiles.Values) t.Dispose();
				isdisposed = true;
			}
		}*/

		public override string ToString()
		{
			return name;
		}

		#endregion
	}
}
