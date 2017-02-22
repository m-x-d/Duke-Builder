
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

using System.Collections.Generic;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Geometry
{
	public sealed class SidedefsTracePath : List<Sidedef>
	{
		#region ================== Constructor / Disposer

		// Constructor
		public SidedefsTracePath()
		{
			// Initialize
		}

		// Constructor
		public SidedefsTracePath(SidedefsTracePath p, Sidedef add) : base(p)
		{
			// Initialize
			base.Add(add);
		}

		#endregion

		#region ================== Methods

		// This checks if the polygon is closed
		public bool CheckIsClosed()
		{
			// There must be at least 2 sidedefs
			if(Count > 1)
			{
				// The end sidedef must share a vertex with the first
				return (base[0].Line.Start == base[Count - 1].Line.Start) ||
					   (base[0].Line.Start == base[Count - 1].Line.End) ||
					   (base[0].Line.End == base[Count - 1].Line.Start) ||
					   (base[0].Line.End == base[Count - 1].Line.End);
			}

			// Not closed
			return false;
		}
		
		// This makes a polygon from the path
		public EarClipPolygon MakePolygon()
		{
			EarClipPolygon p = new EarClipPolygon();
			
			// Any sides at all?
			if(Count > 0)
			{
				// Add all sides
				for(int i = 0; i < Count; i++)
				{
					// On front or back?
					if(base[i].IsFront)
						p.AddLast(new EarClipVertex(base[i].Line.End.Position, base[i]));
					else
						p.AddLast(new EarClipVertex(base[i].Line.Start.Position, base[i]));
				}
			}
			
			return p;
		}

		#endregion
	}
}
