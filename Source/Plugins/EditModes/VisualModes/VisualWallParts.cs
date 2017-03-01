
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

namespace mxd.DukeBuilder.EditModes
{
	internal struct VisualWallParts
	{
		// Members
		public VisualUpper upper;
		public VisualLower lower;
		public VisualMiddleDouble middledouble;
		public VisualMiddleSingle middlesingle;

		// Constructor
		public VisualWallParts(VisualUpper u, VisualLower l, VisualMiddleDouble m)
		{
			this.upper = u;
			this.lower = l;
			this.middledouble = m;
			this.middlesingle = null;
		}

		// Constructor
		public VisualWallParts(VisualMiddleSingle m)
		{
			this.upper = null;
			this.lower = null;
			this.middledouble = null;
			this.middlesingle = m;
		}

		// This calls Setup() on all parts
		public void SetupAllParts()
		{
			if(lower != null) lower.Setup();
			if(middledouble != null) middledouble.Setup();
			if(middlesingle != null) middlesingle.Setup();
			if(upper != null) upper.Setup();
		}
	}
}
