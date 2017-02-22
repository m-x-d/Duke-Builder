
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
using mxd.DukeBuilder.Config;

#endregion

namespace mxd.DukeBuilder.Types
{
	internal class TypesManager : IDisposable
	{
		#region ================== Variables

		// List of handler types
		private Dictionary<PropertyType, TypeHandlerAttribute> handlertypes;

		//mxd. Property types as strings
		private Dictionary<string, PropertyType> knownpropertytypes;
		
		// Disposing
		private bool isdisposed;

		#endregion

		#region ================== Properties

		public bool IsDisposed { get { return isdisposed; } }
		//internal Dictionary<string, PropertyType> KnownPropertyTypes { get { return knownpropertytypes; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public TypesManager()
		{
			// Initialize
			handlertypes = new Dictionary<PropertyType, TypeHandlerAttribute>();

			// Go for all types in this assembly
			Type[] types = General.ThisAssembly.GetTypes();
			foreach(Type tp in types)
			{
				// Check if this type is a class
				if(tp.IsClass && !tp.IsAbstract && !tp.IsArray)
				{
					// Check if class has an TypeHandler attribute
					if(Attribute.IsDefined(tp, typeof(TypeHandlerAttribute), false))
					{
						// Add the type to the list
						object[] attribs = tp.GetCustomAttributes(typeof(TypeHandlerAttribute), false);
						TypeHandlerAttribute attr = (attribs[0] as TypeHandlerAttribute);
						attr.Type = tp;
						handlertypes.Add(attr.PropertyType, attr);
					}
				}
			}

			//mxd
			knownpropertytypes = new Dictionary<string, PropertyType>();
			string[] names = Enum.GetNames(typeof(PropertyType));
			Array values = Enum.GetValues(typeof(PropertyType));
			for(int i = 0; i < names.Length; i++)
			{
				knownpropertytypes[names[i]] = (PropertyType)values.GetValue(i);
			}
			
			// We have no destructor
			GC.SuppressFinalize(this);
		}

		// Disposer
		public void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Clean up
				handlertypes.Clear();
				
				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Methods

		// This returns the type handler for the given argument
		public TypeHandler GetPropertyHandler(PropertyInfo propinfo)
		{
			Type t = typeof(NullHandler);
			TypeHandlerAttribute ta = null;
			
			// Do we have a handler type for this?
			if(handlertypes.ContainsKey(propinfo.Type))
			{
				ta = handlertypes[propinfo.Type];
				t = ta.Type;
			}

			// Create instance
			TypeHandler th = (TypeHandler)General.ThisAssembly.CreateInstance(t.FullName);
			th.SetupProperty(ta, propinfo);
			return th;
		}

		// This returns the type handler for a custom universal field
		/*public TypeHandler GetFieldHandler(int type, object defaultsetting)
		{
			Type t = typeof(NullHandler);
			TypeHandlerAttribute ta = null;

			// Do we have a handler type for this?
			if(handlertypes.ContainsKey(type))
			{
				ta = handlertypes[type];
				t = ta.Type;
			}

			// Create instance
			TypeHandler th = (TypeHandler)General.ThisAssembly.CreateInstance(t.FullName);
			th.SetupField(ta, null);
			th.SetValue(defaultsetting);
			return th;
		}*/

		// This returns the type handler for a given universal field
		/*public TypeHandler GetFieldHandler(UniversalFieldInfo fieldinfo)
		{
			Type t = typeof(NullHandler);
			TypeHandlerAttribute ta = null;

			// Do we have a handler type for this?
			if(handlertypes.ContainsKey(fieldinfo.Type))
			{
				ta = handlertypes[fieldinfo.Type];
				t = ta.Type;
			}

			// Create instance
			TypeHandler th = (TypeHandler)General.ThisAssembly.CreateInstance(t.FullName);
			th.SetupField(ta, fieldinfo);
			th.SetValue(fieldinfo.Default);
			return th;
		}*/
		
		// This returns all custom attributes
		/*public TypeHandlerAttribute[] GetCustomUseAttributes()
		{
			List<TypeHandlerAttribute> attribs = new List<TypeHandlerAttribute>();
			foreach(KeyValuePair<int, TypeHandlerAttribute> ta in handlertypes)
				if(ta.Value.IsCustomUsable) attribs.Add(ta.Value);
			return attribs.ToArray();
		}*/

		// This returns the attribute with the give name
		public TypeHandlerAttribute GetNamedAttribute(string name)
		{
			foreach(var ta in handlertypes)
			{
				if(ta.Value.Name == name) return ta.Value;
			}

			// Nothing found
			return null;
		}

		// This returns the attribute with the give type
		public TypeHandlerAttribute GetAttribute(PropertyType type)
		{
			// Do we have a handler type for this?
			return (handlertypes.ContainsKey(type) ? handlertypes[type] : null);
		}

		public PropertyType PropertyTypeFromName(string name)
		{
			return (knownpropertytypes.ContainsKey(name) ? knownpropertytypes[name] : PropertyType.Unknown);
		}

		#endregion
	}
}
