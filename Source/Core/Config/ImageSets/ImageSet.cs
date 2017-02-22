
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

using System;
using System.Collections.Generic;

#endregion

namespace mxd.DukeBuilder.Config.ImageSets
{
	public abstract class ImageSet : IComparable<ImageSet>
	{
		#region ================== Variables

		protected string name;
		protected HashSet<int> filters;
		
		#endregion
		
		#region ================== Properties
		
		public string Name { get { return name; } set { name = value; } }
		internal HashSet<int> Filters { get { return filters; } }
		
		#endregion
		
		#region ================== Constructor / Destructor
		
		public ImageSet()
		{
			this.name = "Unnamed Set";
			this.filters = new HashSet<int>();
		}
		
		#endregion
		
		#region ================== Methods
		
		// This returns the name
		public override string ToString()
		{
			return name;
		}
		
		// Comparer for sorting alphabetically
		public int CompareTo(ImageSet other)
		{
			return string.Compare(this.name, other.name);
		}
		
		#endregion
	}
}
