
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
using System.Globalization;
using mxd.DukeBuilder.Config;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.Enum, "Setting")]
	internal class EnumOptionHandler : TypeHandler
	{
		#region ================== Variables

		protected EnumList list;
		private EnumItem value;
		
		#endregion

		#region ================== Properties

		//public override bool IsBrowseable { get { return true; } }
		public override bool IsEnumerable { get { return true; } }
		
		#endregion

		#region ================== Constructor

		// When set up for an argument
		public override void SetupProperty(TypeHandlerAttribute attr, PropertyInfo arginfo)
		{
			base.SetupProperty(attr, arginfo);

			// Keep enum list reference
			list = arginfo.Enum;
		}

		#endregion
		
		#region ================== Methods
		
		public override void SetValue(object value)
		{
			this.value = null;

			// Input null?
			if(value == null)
			{
				this.value = new EnumItem("0", "NULL");
			}
			else
			{
				// Compatible type?
				if((value is int) || (value is float) || (value is bool))
				{
					int intvalue = Convert.ToInt32(value);

					// First try to match the value against the enum values
					foreach(EnumItem item in list)
					{
						// Matching value?
						if(item.GetIntValue() == intvalue)
						{
							// Set this value
							this.value = item;
						}
					}
				}

				// No match found yet?
				if(this.value == null)
				{
					// First try to match the value against the enum values
					foreach(EnumItem item in list)
					{
						// Matching value?
						if(item.Value == value.ToString())
						{
							// Set this value
							this.value = item;
						}
					}
				}

				// No match found yet?
				if(this.value == null)
				{
					// Try to match against the titles
					foreach(EnumItem item in list)
					{
						// Matching value?
						if(item.Title.ToLowerInvariant() == value.ToString().ToLowerInvariant())
						{
							// Set this value
							this.value = item;
						}
					}
				}

				// Still no match found?
				if(this.value == null)
				{
					// Make a dummy value
					this.value = new EnumItem(value.ToString(), value.ToString());
					this.value = new EnumItem(this.value.GetIntValue().ToString(CultureInfo.InvariantCulture), value.ToString());
				}
			}
		}

		/*public override object GetValue()
		{
			return GetIntValue();
		}*/
		
		public override int GetIntValue()
		{
			if(this.value == null) return 0;
			
			// Parse the value to integer
			int result;
			return (int.TryParse(this.value.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result) ? result : 0);
		}
		
		public override string GetStringValue()
		{
			return (this.value != null ? this.value.Title : "NULL");
		}

		// This returns an enum list
		public override EnumList GetEnumList()
		{
			return list;
		}

		// This returns the type to display for fixed fields
		// Must be a custom usable type
		/*public override TypeHandlerAttribute GetDisplayType()
		{
			return General.Types.GetAttribute(PropertyType.Integer);
		}*/
		
		#endregion
	}
}
