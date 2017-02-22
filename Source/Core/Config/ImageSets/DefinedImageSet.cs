
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

using System.Collections.Generic;
using mxd.DukeBuilder.IO;

#endregion

namespace mxd.DukeBuilder.Config.ImageSets
{
	internal sealed class DefinedImageSet : ImageSet
	{
		#region ================== Constructor / Destructor
		
		// Texture set from configuration
		public DefinedImageSet(Configuration cfg, string path)
		{
			// Read the name
			name = cfg.ReadSetting(path + ".name", "Unnamed Set");
			
			// Read the filter
			string filtersstr = cfg.ReadSetting(path + ".filter", string.Empty);
			if(!General.GetNumbersFromString(filtersstr, filters))
			{
				General.ErrorLogger.Add(ErrorType.Error, "Failed to get filter from string \"" + filtersstr + "\" while parsing image filter \"" + name + "\"");
			}
		}
		
		// New texture set constructor
		public DefinedImageSet(string name)
		{
			this.name = name;
			this.filters = new HashSet<int>();
		}
		
		#endregion
		
		#region ================== Methods
		
		// This writes the image set to configuration
		internal void WriteToConfig(Configuration cfg, string path)
		{
			// Save name
			cfg.WriteSetting(path + ".name", name);

			// Save filters
			int counter = 0;
			string[] arr = new string[filters.Count];
			foreach(int i in filters)
			{
				arr[counter++] = i.ToString();
			}
			cfg.WriteSetting(path + ".filter", string.Join(" ", arr));
		}
		
		// Duplication
		internal DefinedImageSet Copy()
		{
			// Make a copy
			return new DefinedImageSet(this.name) { filters = new HashSet<int>(this.filters) };
		}
		
		// This applies the filters and name of one set to this one
		/*internal void Apply(TextureSet set)
		{
			this.name = set.Name;
			this.filters = new List<string>(set.Filters);
		}*/
		
		#endregion
	}
}
