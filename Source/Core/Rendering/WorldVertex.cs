
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

using mxd.DukeBuilder.Geometry;

namespace mxd.DukeBuilder.Rendering
{
	// WorldVertex
	public struct WorldVertex
	{
		// Vertex format
		public const int Stride = 6 * 4;

		// Members
		public float x;
		public float y;
		public float z;
		public int c;
		public float u;
		public float v;

		// Constructor
		public WorldVertex(float x, float y, float z, int c, float u, float v)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.c = c;
			this.u = u;
			this.v = v;
		}

		// Constructor
		public WorldVertex(Vector3D pos, Vector2D uv, int color)
		{
			this.x = pos.x;
			this.y = pos.y;
			this.z = pos.z;
			this.c = color;
			this.u = uv.x;
			this.v = uv.y;
		}

		// Constructor
		public WorldVertex(FlatVertex v, float z)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = z;
			this.c = v.c;
			this.u = v.u;
			this.v = v.v;
		}
	}
}
