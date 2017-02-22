#region ================== Namespaces

using System.IO;

#endregion

namespace mxd.DukeBuilder.IO
{
	// Similar to Lump
	internal class Tile
	{
		#region ================== Variables

		// Tile stream
		private ClippedStream stream;

		// Tile info
		private int width;
		private int height;
		private int offsetx;
		private int offsety;
		private int index;

		// Disposing
		//private bool isdisposed;

		#endregion

		#region ================== Properties

		public int Index { get { return index; } }
		public int Width { get { return width; } }
		public int Height { get { return height; } }
		public int OffsetX { get { return offsetx; } }
		public int OffsetY { get { return offsety; } }
		public Stream Stream { get { return stream; } }

		#endregion

		#region ================== Constructor / Disposer

		internal Tile(Stream data, int index, int width, int height, int offsetx, int offsety, int dataoffset, int datalength)
		{
			// Initialize
			this.index = index;
			this.width = width;
			this.height = height;
			this.offsetx = offsetx;
			this.offsety = offsety;
			this.stream = new ClippedStream(data, dataoffset, datalength);
		}

		// Disposer
		/*internal void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Clean up
				stream.Dispose();

				// Done
				isdisposed = true;
			}
		}*/

		#endregion

		#region ================== Methods

		// String representation
		public override string ToString()
		{
			return "Tile" + index;
		}

		#endregion
	}
}
