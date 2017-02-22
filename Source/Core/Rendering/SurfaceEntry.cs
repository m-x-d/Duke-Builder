
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

using System.Drawing;

#endregion

namespace mxd.DukeBuilder.Rendering
{
	// This is an entry is the surface manager and contains the information
	// needed for a sector to place it's ceiling and floor surface geometry
	// in a vertexbuffer. Sectors keep a reference to this entry to tell the
	// surface manager to remove them if needed.
	internal class SurfaceEntry
	{
		// Number of vertices in the geometry and index of the buffer
		// This tells the surface manager which vertexbuffer this is in.
		public int VerticesCount;
		public int BufferIndex;
		
		// Bounding box for fast culling
		public RectangleF BoundingBox;
		
		// Offset in the buffer (in number of vertices)
		public int VertexOffset;
		
		// Sector geometry (local copy used to quickly refill buffers)
		// The sector must set these!
		public FlatVertex[] FloorVertices;
		public FlatVertex[] CeilingVertices;
		
		// Sector images
		// The sector must set these!
		public int FloorTileIndex;
		public int CeilingTileIndex;
		
		// Constructor
		internal SurfaceEntry(int verticescount, int bufferindex, int vertexoffset)
		{
			this.VerticesCount = verticescount;
			this.BufferIndex = bufferindex;
			this.VertexOffset = vertexoffset;
		}

		// Constructor that copies the entry, but does not copy the vertices
		internal SurfaceEntry(SurfaceEntry oldentry)
		{
			this.VerticesCount = oldentry.VerticesCount;
			this.BufferIndex = oldentry.BufferIndex;
			this.VertexOffset = oldentry.VertexOffset;
		}

		// This calculates the bounding box from the vertices
		public void UpdateBBox()
		{
			float left = float.MaxValue;
			float right = float.MinValue;
			float top = float.MaxValue;
			float bottom = float.MinValue;
			
			for(int i = 0; i < FloorVertices.Length; i++)
			{
				if(FloorVertices[i].x < left) left = FloorVertices[i].x;
				if(FloorVertices[i].x > right) right = FloorVertices[i].x;
				if(FloorVertices[i].y < top) top = FloorVertices[i].y;
				if(FloorVertices[i].y > bottom) bottom = FloorVertices[i].y;
			}

			BoundingBox = new RectangleF(left, top, right - left, bottom - top);
		}
	}
}
