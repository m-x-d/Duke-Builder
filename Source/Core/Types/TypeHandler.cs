
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
using System.Windows.Forms;
using mxd.DukeBuilder.Config;

#endregion

namespace mxd.DukeBuilder.Types
{
	/// <summary>
	/// Type Handler base class. A Type Handler takes care of editing, validating and
	/// displaying values of different types for UDMF fields and hexen arguments.
	/// </summary>
	internal abstract class TypeHandler
	{
		#region ================== Variables

		protected PropertyType propertytype;
		protected string typename;
		protected PropertyInfo propinfo;
		protected TypeHandlerAttribute attribute;
		
		#endregion

		#region ================== Properties

		public PropertyType PropertyType { get { return propertytype; } }
		public string TypeName { get { return typename; } }
		public TypeHandlerAttribute Attribute { get { return attribute; } }

		public virtual bool IsBrowseable { get { return false; } }
		public virtual bool IsEnumerable { get { return false; } }
		public virtual bool IsLimitedToEnums { get { return false; } }
		
		public virtual Image BrowseImage { get { return null; } }

		public virtual int MinValue { get { return int.MinValue; } }
		public virtual int MaxValue { get { return int.MaxValue; } }

		#endregion

		#region ================== Constructor

		// This sets up the handler for arguments
		public virtual void SetupProperty(TypeHandlerAttribute attr, PropertyInfo propinfo)
		{
			// Setup
			this.propinfo = propinfo;
			if(attr != null)
			{
				// Set attributes
				this.attribute = attr;
				this.propertytype = attr.PropertyType;
				this.typename = attr.Name;
			}
			else
			{
				// Indexless
				this.attribute = null;
				this.propertytype = PropertyType.Integer;
				this.typename = "Unknown";
			}
		}

		// This sets up the handler for arguments
		/*public virtual void SetupField(TypeHandlerAttribute attr, UniversalFieldInfo fieldinfo)
		{
			// Setup
			this.forargument = false;
			this.arginfo = arginfo;
			if(attr != null)
			{
				// Set attributes
				this.attribute = attr;
				this.index = attr.Index;
				this.typename = attr.Name;
				this.customusable = attr.IsCustomUsable;
			}
			else
			{
				// Indexless
				this.attribute = null;
				this.index = -1;
				this.typename = "Unknown";
				this.customusable = false;
			}
		}*/
		
		#endregion

		#region ================== Methods

		// This must set the value
		// How the value is actually validated and stored is up to the implementation
		public abstract void SetValue(object value);

		// This must return the value as one of the primitive data types
		// supported by UDMF: int, string, float or bool
		//public abstract object GetValue();
		
		// This must return the value as integer (for arguments)
		public virtual int GetIntValue()
		{
			throw new NotSupportedException("Override this method to support it as integer for arguments");
		}

		// This must return the value as a string for displaying
		public abstract string GetStringValue();

		// This is called when the user presses the browse button
		public virtual void Browse(IWin32Window parent) { }
		
		// This must returns an enum list when IsEnumerable is true
		public virtual EnumList GetEnumList()
		{
			return null;
		}
		
		// String representation
		public override string ToString()
		{
			return this.GetStringValue();
		}

		// This returns the type to display for fixed fields
		// Must be a custom usable type
		/*public virtual TypeHandlerAttribute GetDisplayType()
		{
			return this.attribute;
		}*/
		
		#endregion
	}
}
