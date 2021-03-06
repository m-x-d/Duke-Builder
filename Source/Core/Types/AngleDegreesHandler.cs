
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

using System.Drawing;
using System.Windows.Forms;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.AngleDegrees, "Angle (degrees)")]
	internal class AngleDegreesHandler : IntegerHandler
	{
		#region ================== Variables

		//private int value;

		#endregion

		#region ================== Properties

		public override bool IsBrowseable { get { return true; } }
		public override Image BrowseImage { get { return Properties.Resources.Angle; } }
		
		#endregion

		#region ================== Methods

		public override void Browse(IWin32Window parent)
		{
			value = AngleForm.ShowDialog(parent, value);
		}

		/*public override void SetValue(object value)
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
		}*/

		/*public override object GetValue()
		{
			return this.value;
		}*/

		/*public override int GetIntValue()
		{
			return this.value;
		}

		public override string GetStringValue()
		{
			return this.value.ToString();
		}*/

		#endregion
	}
}
