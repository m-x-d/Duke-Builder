
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

#endregion

namespace mxd.DukeBuilder.Data
{
	public enum DataLocationType //mxd
	{
		RESOURCE_GRP,
		RESOURCE_ART,
		RESOURCE_DIRECTORY,
		RESOURCE_ZIP,
	}
	
	public struct DataLocation : IComparable<DataLocation>, IComparable, IEquatable<DataLocation>
	{
		// Members
		public DataLocationType type;
		public string location;
		
		// Constructor
		public DataLocation(DataLocationType type, string location)
		{
			// Initialize
			this.type = type;
			this.location = location;
		}

		// This displays the struct as string
		public override string ToString()
		{
			// Simply show location
			return location;
		}

		// This compares two locations
		public int CompareTo(DataLocation other)
		{
			return string.Compare(this.location, other.location, true);
		}
		
		// This compares two locations
		public int CompareTo(object obj)
		{
			return string.Compare(this.location, ((DataLocation)obj).location, true);
		}
		
		// This compares two locations
		public bool Equals(DataLocation other)
		{
			return (this.CompareTo(other) == 0);
		}
	}
}
