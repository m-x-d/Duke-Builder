
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
using mxd.DukeBuilder.Data;

#endregion

namespace mxd.DukeBuilder.Config.ImageSets
{
	internal sealed class MatchingImageSet : ImageSet, IImageSet, IComparable<MatchingImageSet>
	{
		#region ================== Variables
		
		// Matching images
		private List<ImageData> images;
		
		#endregion

		#region ================== Properties

		public ICollection<ImageData> Images { get { return images; } }

		#endregion
		
		#region ================== Constructor / Destructor
		
		// New texture set for quick matching
		public MatchingImageSet(IEnumerable<int> filters)
		{
			this.filters = new HashSet<int>(filters);
			this.images = new List<ImageData>();
		}
		
		// Texture set from defined set
		public MatchingImageSet(DefinedImageSet definedset)
		{
			// Copy the name
			this.name = definedset.Name;
			
			// Copy the filters
			this.filters = new HashSet<int>(definedset.Filters);
			this.images = new List<ImageData>();
		}
		
		#endregion
		
		#region ================== Methods
		
		// This matches a name against the regex and adds a texture to
		// the list if it matches. Returns true when matched and added.
		internal bool AddImage(ImageData image)
		{
			// Check against regex
			if(filters.Contains(image.TileIndex))
			{
				// Matches! Add it.
				images.Add(image);
				return true;
			}

			// Doesn't match
			return false;
		}
		
		// This only checks if the given image is a match
		internal bool IsMatch(ImageData image)
		{
			return filters.Contains(image.TileIndex);
		}

		// This compares it for sorting
		public int CompareTo(MatchingImageSet other)
		{
			return string.Compare(this.name, other.name);
		}
		
		#endregion
	}
}
