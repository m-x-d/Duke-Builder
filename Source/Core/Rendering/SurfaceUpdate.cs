
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

namespace mxd.DukeBuilder.Rendering
{
	// This contains information to update surface entries with. This may exceed the maximum number
	// of sector vertices, the surface manager will take care of splitting it up in several SurfaceEntries.
	internal class SurfaceUpdate
	{
		public int VerticesCount;
		
		// Sector geometry (local copy used to quickly refill buffers)
		// The sector must set these!
		public FlatVertex[] FloorVertices;
		public FlatVertex[] CeilingVertices;
		
		// Sector images
		// The sector must set these!
		public int FloorTileIndex;
		public int CeilingTileIndex;
		
		// Constructor
		internal SurfaceUpdate(int numvertices, bool updatefloor, bool updateceiling)
		{
			VerticesCount = numvertices;
			FloorTileIndex = 0;
			CeilingTileIndex = 0;
			
			FloorVertices = (updatefloor ? new FlatVertex[numvertices] : null);
			CeilingVertices = (updateceiling ? new FlatVertex[numvertices] : null);
		}
	}
}
