
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace mxd.DukeBuilder.Windows
{
	public class DelayedForm : Form
	{
		// Variables
		protected readonly string configpath; //mxd

		//mxd. Stored window size and location. Tracks location and size of FormWindowState.Normal window 
		private Size windowsize = Size.Empty;
		private Point windowlocation = Point.Empty;

		// Constructor
		protected DelayedForm()
		{
			//mxd. Only when running (this.DesignMode doesn't seem to cut it here,
			// probably because not DelayedForm, but a child class is in design mode...)
			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime)
				configpath = "windows." + this.GetType().Name.ToLowerInvariant() + ".";
		}

		//mxd
		protected override void OnLoad(EventArgs e)
		{
			// Let the base class know
			base.OnLoad(e);

			if(this.DesignMode) return;

			// Restore location and size
			this.SuspendLayout();

			// Restore windowstate
			if(this.MaximizeBox)
			{
				this.WindowState = (FormWindowState)General.Settings.ReadSetting(configpath + "windowstate", (int)FormWindowState.Normal);
			}

			// Form size matters?
			if(this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
			{
				this.Size = new Size(General.Settings.ReadSetting(configpath + "sizewidth", this.Size.Width),
									 General.Settings.ReadSetting(configpath + "sizeheight", this.Size.Height));
			}

			// Restore location
			Point validlocation = Point.Empty;
			Point location = new Point(General.Settings.ReadSetting(configpath + "positionx", int.MaxValue),
									   General.Settings.ReadSetting(configpath + "positiony", int.MaxValue));

			if(location.X < int.MaxValue && location.Y < int.MaxValue)
			{
				// Location withing screen bounds?
				Rectangle bounds = new Rectangle(location, this.Size);
				bounds.Inflate(16, 16); // Add some safety padding
				if(SystemInformation.VirtualScreen.IntersectsWith(bounds))
					validlocation = location;
			}

			if(validlocation == Point.Empty && !(this is MainForm))
			{
				// Do the manual CenterParent...
				validlocation = new Point(General.MainWindow.Location.X + General.MainWindow.Width / 2 - this.Width / 2,
										  General.MainWindow.Location.Y + General.MainWindow.Height / 2 - this.Height / 2);
			}

			// Apply location
			if(validlocation == Point.Empty)
			{
				this.StartPosition = FormStartPosition.CenterParent;
			}
			else
			{
				this.StartPosition = FormStartPosition.Manual;
				this.Location = validlocation;
			}

			// Show the form if needed
			if(this.Opacity < 1.0) this.Opacity = 1.0;

			this.ResumeLayout();
		}

		//mxd. When form is closing
		protected override void OnClosing(CancelEventArgs e)
		{
			if(e.Cancel) return;

			// Let the base class know
			base.OnClosing(e);

			// Determine window state to save
			if(this.MaximizeBox)
			{
				int windowstate;
				if(this.WindowState != FormWindowState.Minimized)
					windowstate = (int)this.WindowState;
				else
					windowstate = (int)FormWindowState.Normal;

				General.Settings.WriteSetting(configpath + "windowstate", windowstate);
			}

			// Form size matters?
			if(this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
			{
				Size size = ((windowsize.IsEmpty && this.WindowState == FormWindowState.Normal) ? this.Size : windowsize); // Prefer stored size if it was set
				if(!size.IsEmpty)
				{
					General.Settings.WriteSetting(configpath + "sizewidth", size.Width);
					General.Settings.WriteSetting(configpath + "sizeheight", size.Height);
				}
			}

			// Save location
			Point location = ((windowlocation.IsEmpty && this.WindowState == FormWindowState.Normal) ? this.Location : windowlocation); // Prefer stored location if it was set
			if(!location.IsEmpty)
			{
				General.Settings.WriteSetting(configpath + "positionx", location.X);
				General.Settings.WriteSetting(configpath + "positiony", location.Y);
			}
		}

		//mxd. Also triggered when the window is dragged.
		protected override void OnResizeEnd(EventArgs e)
		{
			// Store location and size when window is not minimized or maximized
			if(this.WindowState == FormWindowState.Normal)
			{
				// Form size matters?
				if(this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
					windowsize = this.Size;
				windowlocation = this.Location;
			}

			base.OnResizeEnd(e);
		}
	}
}
