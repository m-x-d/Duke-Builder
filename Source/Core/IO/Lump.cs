
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

using System.IO;

#endregion

namespace mxd.DukeBuilder.IO
{
	public class Lump
	{
		#region ================== Variables

		// Data stream
		private ClippedStream stream;
		
		// Data info
		private string name;
		private int offset;
		private int length;

		// Disposing
		private bool isdisposed;

		#endregion

		#region ================== Properties

		internal string Name { get { return name; } }
		internal int Offset { get { return offset; } }
		internal int Length { get { return length; } }
		internal ClippedStream Stream { get { return stream; } }
		internal bool IsDisposed { get { return isdisposed; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal Lump(Stream data, string name, int offset, int length)
		{
			// Initialize
			this.stream = new ClippedStream(data, offset, length);
			this.offset = offset;
			this.length = length;

			// Make name
			this.name = name.ToUpperInvariant();
		}

		// Disposer
		internal void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Clean up
				stream.Dispose();

				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Methods

		// This copies lump data to another lump
		internal void CopyTo(Lump lump)
		{
			// Create a reader
			BinaryReader reader = new BinaryReader(stream);

			// Copy bytes over
			stream.Seek(0, SeekOrigin.Begin);
			lump.Stream.Write(reader.ReadBytes((int)stream.Length), 0, (int)stream.Length);
		}
		
		// String representation
		public override string ToString()
		{
			return name;
		}
		
		#endregion
	}
}
