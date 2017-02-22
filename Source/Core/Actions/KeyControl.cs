
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

using System.Windows.Forms;

#endregion

namespace mxd.DukeBuilder.Actions
{
	internal struct KeyControl
	{
		#region ================== Variables

		public int Key;
		public string Name;

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public KeyControl(Keys key, string name)
		{
			// Initialize
			Key = (int)key;
			Name = name;
		}

		// Constructor
		public KeyControl(SpecialKeys key, string name)
		{
			// Initialize
			Key = (int)key;
			Name = name;
		}

		// Constructor
		public KeyControl(int key, string name)
		{
			// Initialize
			Key = key;
			Name = name;
		}

		#endregion

		#region ================== Methods

		// Returns name
		public override string ToString()
		{
			return Name;
		}
		
		#endregion
	}
}
