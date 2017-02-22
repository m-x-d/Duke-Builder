
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
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.EnumBits, "Options")]
	internal class EnumBitsHandler : TypeHandler
	{
		#region ================== Variables

		private EnumList list;
		private int value;

		#endregion

		#region ================== Properties

		public override bool IsBrowseable { get { return true; } }
		public override Image BrowseImage { get { return Properties.Resources.List; } }
		
		#endregion
		
		#region ================== Methods

		// When set up for an argument
		public override void SetupProperty(TypeHandlerAttribute attr, PropertyInfo arginfo)
		{
			base.SetupProperty(attr, arginfo);

			// Keep enum list reference
			list = arginfo.Enum;
		}

		public override void Browse(IWin32Window parent)
		{
			value = BitFlagsForm.ShowDialog(parent, list, value);
		}

		public override void SetValue(object value)
		{
			// Null?
			if(value == null)
			{
				this.value = 0;
			}
			// Compatible type?
			else if((value is int) || (value is float) || (value is bool))
			{
				// Set directly
				this.value = Convert.ToInt32(value);
			}
			else
			{
				// Try parsing as string
				int result;
				this.value = (int.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.CurrentCulture, out result) ? result : 0);
			}
		}

		/*public override object GetValue()
		{
			return this.value;
		}*/

		public override int GetIntValue()
		{
			return this.value;
		}

		public override string GetStringValue()
		{
			return this.value.ToString();
		}

		#endregion
	}
}
