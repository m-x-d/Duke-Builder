
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
using System.Windows.Forms;
using mxd.DukeBuilder.Windows;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Actions;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Geometry;

#endregion

namespace mxd.DukeBuilder.Editing
{
	public class GridSetup : IDisposable
	{
		#region ================== Constants

		private const int DEFAULT_GRID_SIZE = 1024;

		#endregion

		#region ================== Variables

		// Grid
		private int gridsize;
		private float gridsizef;
		private float gridsizefinv;

		// Background
		private string background = "";
		private ImageData backimage = new UnknownImage(null);
		private int backoffsetx, backoffsety;
		private float backscalex, backscaley;

		// Disposing
		private bool isdisposed;
		
		#endregion

		#region ================== Properties

		public int GridSize { get { return gridsize; } }
		public float GridSizeF { get { return gridsizef; } }
		internal string BackgroundName { get { return background; } }
		internal ImageData Background { get { return backimage; } }
		internal int BackgroundX { get { return backoffsetx; } }
		internal int BackgroundY { get { return backoffsety; } }
		internal float BackgroundScaleX { get { return backscalex; } }
		internal float BackgroundScaleY { get { return backscaley; } }
		internal bool Disposed { get { return isdisposed; } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal GridSetup()
		{
			// Initialize
			SetGridSize(DEFAULT_GRID_SIZE);
			backscalex = 1.0f;
			backscaley = 1.0f;
			
			// Register actions
			General.Actions.BindMethods(this);
			
			// We have no destructor
			GC.SuppressFinalize(this);
		}

		// Disposer
		public void Dispose()
		{
			if(!isdisposed)
			{
				// Dispose image if needed
				if(backimage is FileImage) (backimage as FileImage).Dispose();
				
				// Clean up
				backimage = null;

				// Unregister actions
				General.Actions.UnbindMethods(this);

				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Methods

		// Write settings to configuration
		internal void WriteToConfig(Configuration cfg, string path)
		{
			// Write settings
			cfg.WriteSetting(path + ".background", background);
			cfg.WriteSetting(path + ".backoffsetx", backoffsetx);
			cfg.WriteSetting(path + ".backoffsety", backoffsety);
			cfg.WriteSetting(path + ".backscalex", (int)(backscalex * 100.0f));
			cfg.WriteSetting(path + ".backscaley", (int)(backscaley * 100.0f));
			cfg.WriteSetting(path + ".gridsize", gridsize);
		}

		// Read settings from configuration
		internal void ReadFromConfig(Configuration cfg, string path)
		{
			// Read settings
			background = cfg.ReadSetting(path + ".background", "");
			backoffsetx = cfg.ReadSetting(path + ".backoffsetx", 0);
			backoffsety = cfg.ReadSetting(path + ".backoffsety", 0);
			backscalex = cfg.ReadSetting(path + ".backscalex", 100) / 100.0f;
			backscaley = cfg.ReadSetting(path + ".backscaley", 100) / 100.0f;
			gridsize = cfg.ReadSetting(path + ".gridsize", DEFAULT_GRID_SIZE);

			// Setup
			SetGridSize(gridsize);
			LinkBackground();
		}

		// This sets the grid size
		internal void SetGridSize(int size)
		{
			// Change grid
			this.gridsize = size;
			this.gridsizef = gridsize;
			this.gridsizefinv = 1f / gridsizef;

			// Update in main window
			General.MainWindow.UpdateGrid(gridsize);
		}

		// This sets the background
		internal void SetBackground(string name)
		{
			// Set background
			if(name == null) name = "";
			this.background = name;

			// Find this image
			LinkBackground();
		}

		// This sets the background view
		internal void SetBackgroundView(int offsetx, int offsety, float scalex, float scaley)
		{
			// Set background offset
			this.backoffsetx = offsetx;
			this.backoffsety = offsety;
			this.backscalex = scalex;
			this.backscaley = scaley;
		}
		
		// This finds and links the background image
		internal void LinkBackground()
		{
			// Dispose image if needed
			if(backimage is FileImage)
			{
				backimage.Dispose();
				backimage = null;
			}

			// Load background image
			if(!string.IsNullOrEmpty(background))
			{
				backimage = new FileImage(-1, background);

				// Make sure it is loaded
				backimage.LoadImage();
				backimage.CreateTexture();
			}
		}
		
		// This returns the next higher coordinate
		public float GetHigher(float offset)
		{
			return (float)Math.Round((offset + (gridsizef * 0.5f)) * gridsizefinv) * gridsizef;
		}

		// This returns the next lower coordinate
		public float GetLower(float offset)
		{
			return (float)Math.Round((offset - (gridsizef * 0.5f)) * gridsizefinv) * gridsizef;
		}
		
		// This snaps to the nearest grid coordinate
		public Vector2D SnappedToGrid(Vector2D v)
		{
			return GridSetup.SnappedToGrid(v, gridsizef, gridsizefinv);
		}

		// This snaps to the nearest grid coordinate
		public static Vector2D SnappedToGrid(Vector2D v, float gridsize, float gridsizeinv)
		{
			Vector2D sv = new Vector2D((float)Math.Round(v.x * gridsizeinv) * gridsize,
									   (float)Math.Round(v.y * gridsizeinv) * gridsize);

			sv.x = General.Clamp(sv.x, General.Map.FormatInterface.LeftBoundary, General.Map.FormatInterface.RightBoundary);
			sv.y = General.Clamp(sv.y, General.Map.FormatInterface.BottomBoundary, General.Map.FormatInterface.TopBoundary);

			return sv;
		}

		#endregion

		#region ================== Actions

		// This shows the grid setup dialog
		internal void ShowGridSetup()
		{
			// Show preferences dialog
			GridSetupForm gridform = new GridSetupForm();
			if(gridform.ShowDialog(General.MainWindow) == DialogResult.OK)
			{
				// Redraw display
				General.MainWindow.RedrawDisplay();
			}

			// Done
			gridform.Dispose();
		}
		
		// This changes grid size
		// Note: these were incorrectly swapped before, hence the wrong action name
		[BeginAction("gridinc")]
		internal void DecreaseGrid()
		{
			// Not lower than 1
			if(gridsize >= 2)
			{
				// Change grid
				SetGridSize(gridsize >> 1);
				
				// Redraw display
				General.MainWindow.RedrawDisplay();
			}
		}

		// This changes grid size
		// Note: these were incorrectly swapped before, hence the wrong action name
		[BeginAction("griddec")]
		internal void IncreaseGrid()
		{
			// Not higher than 1024
			if(gridsize <= 512)
			{
				// Change grid
				SetGridSize(gridsize << 1);

				// Redraw display
				General.MainWindow.RedrawDisplay();
			}
		}

		#endregion
	}
}
