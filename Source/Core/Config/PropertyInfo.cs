
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

using System.Collections;
using System.Collections.Generic;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Types;

#endregion

namespace mxd.DukeBuilder.Config
{
	public class PropertyInfo
	{
		#region ================== Variables

		private const string DEFAULT_TITLE = "Generic property";

		#endregion

		#region ================== Variables

		private string title;
		private string tooltip;
		private bool used;
		private PropertyType type;
		private EnumList enumlist;
		private HashSet<int> channeltargets; // Sprite types compatible with Channel

		#endregion

		#region ================== Properties

		public string Title { get { return title; } }
		public string ToolTip { get { return tooltip; } }
		public bool Used { get { return used; } }
		public PropertyType Type { get { return type; } }
		public EnumList Enum { get { return enumlist; } }
		public HashSet<int> ChannelTargets { get { return channeltargets; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor for property info from configuration
		internal PropertyInfo(Configuration cfg, string propertypath, PropertyType defaultpropertytype, IDictionary<string, EnumList> enums)
		{
			// Read
			this.used = cfg.SettingExists(propertypath);
			if(!this.used)
			{
				this.title = DEFAULT_TITLE;
				this.type = defaultpropertytype;
				this.enumlist = new EnumList();
				this.channeltargets = new HashSet<int>();
				return;
			}

			this.title = cfg.ReadSetting(propertypath + "title", DEFAULT_TITLE);
			this.tooltip = cfg.ReadSetting(propertypath + "tooltip", string.Empty);
			
			string typestr = cfg.ReadSetting(propertypath + "type", "Integer");
			this.type = General.Types.PropertyTypeFromName(typestr);
			if(this.type == PropertyType.Unknown)
			{
				General.ErrorLogger.Add(ErrorType.Warning, "\"" + propertypath + ".type\" references unknown property type \"" + typestr + "\"");
				this.type = PropertyType.Integer;
			}

			// Determine enum type
			IDictionary argdic = cfg.ReadSetting(propertypath, new Hashtable());
			if(argdic.Contains("enum"))
			{
				// Enum fully specified?
				if(argdic["enum"] is IDictionary)
				{
					// Create anonymous enum
					this.enumlist = new EnumList(argdic["enum"] as IDictionary);
				}
				else
				{
					// Check if referenced enum exists
					string enumname = argdic["enum"].ToString();
					if((enumname.Length > 0) && enums.ContainsKey(enumname))
					{
						// Get the enum list
						this.enumlist = enums[enumname];
					}
					else
					{
						General.ErrorLogger.Add(ErrorType.Warning, "\"" + propertypath + ".enum\" references unknown enumeration \"" + enumname + "\"");
					}
				}
			}

			// Read channeltargets
			string targetsstr = cfg.ReadSetting(propertypath + "channeltargets", string.Empty);
			if(!string.IsNullOrEmpty(targetsstr) && !General.GetNumbersFromString(targetsstr, channeltargets))
			{
				General.ErrorLogger.Add(ErrorType.Warning, "Unable to get Channel target from string \"" + targetsstr + "\" while parsing \"" + propertypath + ".channeltargets\"");
			}

			//mxd
			if(this.enumlist == null) this.enumlist = new EnumList();
		}

		// Constructor for generic property info
		/*internal PropertyInfo()
		{
			this.used = false;
			this.title = DEFAULT_TITLE;
			this.type = PropertyType.Integer;
			this.enumlist = new EnumList();
		}*/

		#endregion

		#region ================== Methods

		// This gets the description for an argument value
		/*public string GetValueDescription(int value)
		{
			// TODO: Use the registered type editor to get the description!

			return value.ToString();
		}*/

		#endregion
	}
}
