
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
	internal sealed class AllImageSet : ImageSet, IImageSet
	{
		#region ================== Constants

		private const string NAME = "All";

		#endregion

		#region ================== Variables

		// Matching textures
		private List<ImageData> images;

		#endregion

		#region ================== Properties

		public ICollection<ImageData> Images { get { return images; } }

		#endregion

		#region ================== Constructor / Destructor

		// New texture set constructor
		public AllImageSet()
		{
			this.name = NAME;
			this.images = new List<ImageData>();
		}

		#endregion

		#region ================== Methods

		internal void AddImage(ImageData image)
		{
			images.Add(image);
		}

		#endregion
	}
}
