
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
using mxd.DukeBuilder.Data;

#endregion

namespace mxd.DukeBuilder.Config.ImageSets
{
	internal sealed class ResourceImageSet : ImageSet, IImageSet
	{
		#region ================== Variables

		// Matching images
		private Dictionary<int, ImageData> images;
		private DataLocation location;

		#endregion

		#region ================== Properties
		
		public ICollection<ImageData> Images { get { return images.Values; } }
		public DataLocation Location { get { return location; } }
		
		#endregion

		#region ================== Constructor / Destructor

		// New texture set constructor
		public ResourceImageSet(string name, DataLocation location)
		{
			this.name = name;
			this.location = location;
			this.images = new Dictionary<int, ImageData>();
		}
		
		#endregion

		#region ================== Methods

		// Add an image
		internal void AddImage(ImageData image)
		{
			if(images.ContainsKey(image.TileIndex))
				General.ErrorLogger.Add(ErrorType.Warning, "Image \"" + image.TileIndex + "\" is double defined in resource \"" + this.Location.location + "\".");
			images[image.TileIndex] = image;
		}

		// Check if this set has an image
		internal bool ImageExists(ImageData image)
		{
			return images.ContainsKey(image.TileIndex);
		}

		#endregion
	}
}
