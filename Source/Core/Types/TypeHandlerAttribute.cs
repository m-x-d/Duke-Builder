
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

namespace mxd.DukeBuilder.Types
{
	internal class TypeHandlerAttribute : Attribute
	{
		#region ================== Variables

		private PropertyType propertytype;
		private string name;
		private Type type;
		
		#endregion

		#region ================== Properties

		public PropertyType PropertyType { get { return propertytype; } }
		public string Name { get { return name; } }
		public Type Type { get { return type; } set { type = value; } }
		
		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public TypeHandlerAttribute(PropertyType type, string name)
		{
			// Initialize
			this.propertytype = type;
			this.name = name;
		}

		#endregion

		#region ================== Methods

		// String representation
		public override string ToString()
		{
			return name;
		}
		
		#endregion
	}
}
