
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

#endregion

namespace mxd.DukeBuilder.Data
{
	public sealed class UnknownImage : ImageData
	{
		#region ================== Constructor / Disposer

		// Constructor
		public UnknownImage(Bitmap image)
		{
			// Initialize
			if(image != null)
			{
				this.bitmap = new Bitmap(image);
				this.width = image.Width;
				this.height = image.Height;
			}
			this.ImageState = ImageLoadState.Ready;
			this.AllowUnload = false;
			
			// We have no destructor
			GC.SuppressFinalize(this);
		}

		#endregion

		#region ================== Methods

		// This returns a preview image
		public override Image GetPreview()
		{
			lock(this)
			{
				// Make a copy
				return (bitmap != null ? new Bitmap(bitmap) : null);
			}
		}

		#endregion
	}
}
