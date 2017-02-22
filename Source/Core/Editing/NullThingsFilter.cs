
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

namespace mxd.DukeBuilder.Editing
{
	internal class NullThingsFilter : ThingsFilter
	{
		#region ================== Constructor / Disposer

		// Constructor
		internal NullThingsFilter()
		{
			this.name = "(show all)";
		}

		#endregion

		#region ================== Methods

		// This updates the lists
		public override void Update()
		{
			// Make lists
			visiblethings = new List<Thing>(General.Map.Map.Things);
			hiddenthings = new List<Thing>(0);
			thingsvisiblestate = new Dictionary<Thing, bool>(General.Map.Map.Things.Count);
			foreach(Thing t in visiblethings) thingsvisiblestate.Add(t, true);
		}
		
		#endregion
	}
}
