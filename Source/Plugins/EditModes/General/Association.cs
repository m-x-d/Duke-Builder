
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

using mxd.DukeBuilder.Types;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	public struct Association
	{
		public int tag;
		public PropertyType type;

		// This sets up the association
		public Association(int tag, int type)
		{
			this.tag = tag;
			this.type = (PropertyType)type;
		}

		// This sets up the association
		public Association(int tag, PropertyType type)
		{
			this.tag = tag;
			this.type = type;
		}

		// This sets up the association
		public void Set(int tag, int type)
		{
			this.tag = tag;
			this.type = (PropertyType)type;
		}

		// This sets up the association
		public void Set(int tag, PropertyType type)
		{
			this.tag = tag;
			this.type = type;
		}

		// This compares an association
		public static bool operator ==(Association a, Association b)
		{
			return (a.tag == b.tag) && (a.type == b.type);
		}

		// This compares an association
		public static bool operator !=(Association a, Association b)
		{
			return (a.tag != b.tag) || (a.type != b.type);
		}

		//mxd 
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		//mxd
		public override bool Equals(object obj)
		{
			if(!(obj is Association)) return false;

			Association b = (Association)obj;
			return (type == b.type && tag == b.tag);
		}
	}
}
