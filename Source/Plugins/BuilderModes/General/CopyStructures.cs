
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

using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	// Line
	public class LineProperties
	{
		private BuildWall front;
		private BuildWall back;

		public LineProperties(Linedef l)
		{
			front = (l.Front != null ? new BuildWall(l.Front) : null);
			back = (l.Back != null ? new BuildWall(l.Back) : null);
		}
		
		public void Apply(Linedef l)
		{
			if(front != null && l.Front != null) front.ApplyTo(l.Front);
			if(back != null && l.Back != null) back.ApplyTo(l.Back);
		}
	}
}
