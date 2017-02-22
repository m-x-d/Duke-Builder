
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
using System.IO;
using mxd.DukeBuilder.Config.ImageSets;

#endregion

namespace mxd.DukeBuilder.Data.DataReaders
{
	internal abstract class DataReader
	{
		#region ================== Variables

		protected DataLocation location;
		protected bool issuspended;
		protected bool isdisposed;
		protected ResourceImageSet textureset;

		#endregion

		#region ================== Properties

		public DataLocation Location { get { return location; } }
		public bool IsDisposed { get { return isdisposed; } }
		public bool IsSuspended { get { return issuspended; } }
		public ResourceImageSet ImageSet { get { return textureset; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public DataReader(DataLocation dl)
		{
			// Keep information
			location = dl;
			textureset = new ResourceImageSet(GetTitle(), dl);
		}

		// Disposer
		public virtual void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Done
				textureset = null;
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Management

		// This returns a short name
		protected abstract string GetTitle();

		// This suspends use of this resource
		public virtual void Suspend()
		{
			issuspended = true;
		}

		// This resumes use of this resource
		public virtual void Resume()
		{
			issuspended = false;
		}

		#endregion

		#region ================== Palette

		// When implemented, this should find and load a PLAYPAL palette
		public virtual Playpal LoadPalette() { return null; }
		
		#endregion

		#region ================== Images
		
		// When implemented, this returns the texture lump
		public virtual Stream GetImageStream(int name) { return null; }

		// When implemented, this loads the textures
		public virtual ICollection<ImageData> LoadImages() { return null; }
		
		#endregion
	}
}
