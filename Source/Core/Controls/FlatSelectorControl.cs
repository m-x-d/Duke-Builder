
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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using mxd.DukeBuilder.Actions;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Rendering;
using SlimDX.Direct3D9;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder.Controls
{
	public class FlatSelectorControl : ImageSelectorControl
	{
		// Setup
		public override void Initialize()
		{
			base.Initialize();
			
			// Fill autocomplete list
			name.AutoCompleteCustomSource.AddRange(General.Map.Data.FlatNames.ToArray());
		}
		
		// This finds the image we need for the given flat name
		protected override Image FindImage(string imagename)
		{
			// Check if name is a "none" texture
			if((imagename.Length < 1) || (imagename[0] == '-'))
			{
				// Flat required!
				return mxd.DukeBuilder.Properties.Resources.MissingTexture;
			}
			else
			{
				// Set the image
				return General.Map.Data.GetFlatImage(imagename).GetPreview();
			}
		}

		// This browses for a flat
		protected override string BrowseImage(string imagename)
		{
			string result;

			// Browse for texture
			result = FlatBrowserForm.Browse(this.ParentForm, imagename);
			if(result != null) return result; else return imagename;
		}
	}
}
