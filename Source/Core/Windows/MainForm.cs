
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
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using mxd.DukeBuilder.Actions;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Controls;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Editing;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Plugins;
using mxd.DukeBuilder.Properties;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.Windows
{
	public partial class MainForm : DelayedForm, IMainForm
	{
		#region ================== Constants
		
		// Recent files
		private const int MAX_RECENT_FILES = 8;
		private const int MAX_RECENT_FILES_PIXELS = 250;
		
		// Status bar
		private const string STATUS_READY_TEXT = "Ready.";
		private const string STATUS_LOADING_TEXT = "Loading resources...";
		private const int WARNING_FLASH_COUNT = 10;
		private const int WARNING_FLASH_INTERVAL = 100;
		private const int WARNING_RESET_DELAY = 5000;
		private const int INFO_RESET_DELAY = 5000;
		private const int ACTION_FLASH_COUNT = 3;
		private const int ACTION_FLASH_INTERVAL = 50;
		private const int ACTION_RESET_DELAY = 5000;
		private const int DISPLAYED_ZOOM_SCALER = 25; //mxd
		
		private readonly Image[,] STATUS_IMAGES = 
		{
			// Normal versions
			{
			  Resources.Status0, Resources.Status1,
			  Resources.Status2, Resources.Warning
			},
			
			// Flashing versions
			{
			  Resources.Status10, Resources.Status11,
			  Resources.Status12, Resources.WarningOff
			}
		};
		
		// Message pump
		public enum ThreadMessages
		{
			// Sent by the background threat to update the status
			UpdateStatus = General.WM_USER + 1,
			
			// This is sent by the background thread when images are loaded
			// but only when first loaded or when dimensions were changed
			ImageDataLoaded = General.WM_USER + 2
		}
		
		#endregion

		#region ================== Variables

		// Position/size
		private bool displayresized = true;
		private bool windowactive;
		
		// Mouse in display
		private bool mouseinside;
		
		// Input
		private bool shift, ctrl, alt;
		private MouseButtons mousebuttons;
		private MouseInput mouseinput;
		private Rectangle originalclip;
		private bool mouseexclusive;
		private int mouseexclusivebreaklevel;
		
		// Skills
		private ToolStripItem[] skills;
		
		// Recent files
		private ToolStripMenuItem[] recentitems;
		
		// View modes
		private ToolStripButton[] viewmodesbuttons;
		private ToolStripMenuItem[] viewmodesitems;
		
		// Edit modes
		private List<ToolStripItem> editmodeitems;
		
		// Toolbar
		private List<PluginToolbarButton> pluginbuttons;
		private EventHandler buttonvisiblechangedhandler;
		private bool preventupdateseperators;
		private bool updatingfilters;
		
		// Statusbar
		private StatusInfo status;
		private int statusflashcount;
		private bool statusflashicon;
		
		// Properties
		private IntPtr windowptr;
		
		// Processing
		private int processingcount;
		private double lastupdatetime;

		// Updating
		private int lockupdatecount;

		//mxd. Misc drawing
		private Graphics graphics;
		
		#endregion

		#region ================== Properties

		public bool ShiftState { get { return shift; } }
		public bool CtrlState { get { return ctrl; } }
		public bool AltState { get { return alt; } }
		public new MouseButtons MouseButtons { get { return mousebuttons; } }
		public bool MouseInDisplay { get { return mouseinside; } }
		public RenderTargetControl Display { get { return display; } }
		public bool SnapToGrid { get { return buttonsnaptogrid.Checked; } }
		public bool AutoMerge { get { return buttonautomerge.Checked; } }
		public bool MouseExclusive { get { return mouseexclusive; } }
		new public IntPtr Handle { get { return windowptr; } }
		public bool IsActiveWindow { get { return windowactive; } }
		public StatusInfo Status { get { return status; } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal MainForm()
		{
			// Setup controls
			InitializeComponent();
			pluginbuttons = new List<PluginToolbarButton>();
			editmodeitems = new List<ToolStripItem>();
			
			// Fetch pointer
			windowptr = base.Handle;

			//mxd. Graphics
			graphics = Graphics.FromHwndInternal(windowptr);
			
			// Make array for view modes
			viewmodesbuttons = new ToolStripButton[Renderer2D.NUM_VIEW_MODES];
			viewmodesbuttons[(int)ViewMode.Normal] = buttonviewnormal;
			viewmodesbuttons[(int)ViewMode.Brightness] = buttonviewbrightness;
			viewmodesbuttons[(int)ViewMode.FloorTextures] = buttonviewfloors;
			viewmodesbuttons[(int)ViewMode.CeilingTextures] = buttonviewceilings;
			viewmodesitems = new ToolStripMenuItem[Renderer2D.NUM_VIEW_MODES];
			viewmodesitems[(int)ViewMode.Normal] = itemviewnormal;
			viewmodesitems[(int)ViewMode.Brightness] = itemviewbrightness;
			viewmodesitems[(int)ViewMode.FloorTextures] = itemviewfloors;
			viewmodesitems[(int)ViewMode.CeilingTextures] = itemviewceilings;
			
			// Visual Studio IDE doesn't let me set these in the designer :(
			buttonzoom.Font = menufile.Font;
			buttonzoom.DropDownDirection = ToolStripDropDownDirection.AboveLeft;
			buttongrid.Font = menufile.Font;
			buttongrid.DropDownDirection = ToolStripDropDownDirection.AboveLeft;

			// Event handlers
			buttonvisiblechangedhandler = ToolbarButtonVisibleChanged;
			
			// Bind any methods
			General.Actions.BindMethods(this);
			
			// Apply shortcut keys
			ApplyShortcutKeys();
			
			// Make recent items list
			CreateRecentFiles();
			
			// Show splash
			ShowSplashDisplay();
		}
		
		#endregion
		
		#region ================== General

		// Editing mode changed!
		internal void EditModeChanged()
		{
			// Check appropriate button on interface
			// And show the mode name
			if(General.Editing.Mode != null)
			{
				CheckEditModeButton(General.Editing.Mode.EditModeButtonName);
				infopanel.DisplayModeName(General.Editing.Mode.Attributes.DisplayName);
			}
			else
			{
				CheckEditModeButton("");
				infopanel.DisplayModeName("");
			}

			// View mode only matters in classic editing modes
			for(int i = 0; i < Renderer2D.NUM_VIEW_MODES; i++)
			{
				viewmodesitems[i].Enabled = (General.Editing.Mode is ClassicMode);
				viewmodesbuttons[i].Enabled = (General.Editing.Mode is ClassicMode);
			}

			UpdateEditMenu();
		}

		// This makes a beep sound
		public void MessageBeep(MessageBeepType type)
		{
			General.MessageBeep(type);
		}

		// This sets up the interface
		internal void SetupInterface()
		{
			// Setup docker
			if(General.Settings.DockersPosition != 2 && General.Map != null)
			{
				LockUpdate();
				dockerspanel.Visible = true;
				dockersspace.Visible = true;

				// We can't place the docker easily when collapsed
				dockerspanel.Expand();

				// Setup docker width
				int collapsedwidth = dockerspanel.GetCollapsedWidth();
				if(General.Settings.DockersWidth < collapsedwidth) General.Settings.DockersWidth = collapsedwidth;

				// Determine fixed space required
				dockersspace.Width = (General.Settings.CollapseDockers ? collapsedwidth : General.Settings.DockersWidth);

				// Setup docker
				if(General.Settings.DockersPosition == 0)
				{
					dockersspace.Dock = DockStyle.Left;
					dockerspanel.Setup(false);
					dockerspanel.Location = dockersspace.Location;
					dockerspanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
				}
				else
				{
					dockersspace.Dock = DockStyle.Right;
					dockerspanel.Setup(true);
					dockerspanel.Location = new Point(dockersspace.Right - General.Settings.DockersWidth, dockersspace.Top);
					dockerspanel.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
				}

				//mxd. dockersspace display index gets messed up while re-docking. This fixes it...
				int targetindex = this.Controls.IndexOf(display) + 1;
				while(this.Controls.IndexOf(dockersspace) != targetindex)
				{
					this.Controls.SetChildIndex(dockersspace, targetindex);
				}

				dockerspanel.Width = General.Settings.DockersWidth;
				dockerspanel.Height = dockersspace.Height;
				dockerspanel.BringToFront();

				if(General.Settings.CollapseDockers) dockerspanel.Collapse();
				
				UnlockUpdate();
			}
			else
			{
				dockerspanel.Visible = false;
				dockersspace.Visible = false;
			}
		}
		
		// This updates all menus for the current status
		internal void UpdateInterface()
		{
			//mxd. Update title
			UpdateTitle();

			// Update the status bar
			UpdateStatusbar();
			
			// Update menus and toolbar icons
			UpdateFileMenu();
			UpdateEditMenu();
			UpdateViewMenu();
			UpdateModeMenu();
			UpdateToolsMenu();
			UpdateToolbar();
			UpdateSkills();
			UpdateHelpMenu();
		}

		//mxd
		internal void UpdateTitle()
		{
			// Map opened?
			if(General.Map != null)
			{
				// Show map name and filename in caption
				bool mapchanged = (General.Map != null && General.Map.IsChanged);
				this.Text = (mapchanged ? "\u25CF " : "") + General.Map.FileName + " - " + Application.ProductName;
			}
			else
			{
				// Show normal caption
#if DEBUG
				this.Text = Application.ProductName + " - DEVBUILD";
#else
				this.Text = Application.ProductName + " R" + General.ThisAssembly.GetName().Version.Revision;
#endif
			}
		}
		
		// Generic event that invokes the tagged action
		public void InvokeTaggedAction(object sender, EventArgs e)
		{
			//this.Update();
			if(sender is ToolStripItem)
				General.Actions.InvokeAction((sender as ToolStripItem).Tag.ToString());
			else if(sender is Control)
				General.Actions.InvokeAction((sender as Control).Tag.ToString());
			else
				General.Fail("InvokeTaggedAction used on an unexpected control.");
			
			this.Update();
		}
		
		#endregion
		
		#region ================== Window
		
		// This locks the window for updating
		internal void LockUpdate()
		{
			lockupdatecount++;
			if(lockupdatecount == 1) General.LockWindowUpdate(this.Handle);
		}

		// This unlocks for updating
		internal void UnlockUpdate()
		{
			lockupdatecount--;
			if(lockupdatecount == 0) General.LockWindowUpdate(IntPtr.Zero);
			if(lockupdatecount < 0) lockupdatecount = 0;
		}

		// This unlocks for updating
		/*internal void ForceUnlockUpdate()
		{
			if(lockupdatecount > 0) General.LockWindowUpdate(IntPtr.Zero);
			lockupdatecount = 0;
		}*/
		
		// This sets the focus on the display for correct key input
		public bool FocusDisplay()
		{
			return display.Focus();
		}

		// Window is loaded
		private void MainForm_Load(object sender, EventArgs e)
		{
			// Info panel state?
			bool expandedpanel = General.Settings.ReadSetting(configpath + "expandedinfopanel", true);
			if(expandedpanel != infopanel.IsExpanded) ToggleInfoPanel();
		}

		// Window receives focus
		private void MainForm_Activated(object sender, EventArgs e)
		{
			windowactive = true;

			UpdateInterface();
			ResumeExclusiveMouseInput();
			ReleaseAllKeys();
			FocusDisplay();
		}
		
		// Window loses focus
		private void MainForm_Deactivate(object sender, EventArgs e)
		{
			windowactive = false;
			
			BreakExclusiveMouseInput();
			ReleaseAllKeys();
		}

		//mxd. Looks like in some cases StartMouseExclusive is called before app aquires the mouse
		// which results in setting Cursor.Clip not taking effect.
		private void MainForm_MouseCaptureChanged(object sender, EventArgs e)
		{
			if(mouseexclusive && windowactive && mouseinside)
				Cursor.Clip = display.RectangleToScreen(display.ClientRectangle);
		}

		//mxd
		private void MainForm_ResizeEnd(object sender, EventArgs eventArgs)
		{
			// Separators may need updating
			UpdateSeparators();
		}

		// Window is being closed
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason != CloseReason.ApplicationExitCall)
			{
				// Close the map
				if(General.CloseMap())
				{
					General.WriteLogLine("Closing main interface window...");
					
					// Stop timers
					redrawtimer.Stop(); //mxd
					statusflasher.Stop();
					statusresetter.Stop();

					// Stop exclusive mode, if any is active
					StopExclusiveMouseInput();
					StopProcessing();

					// Unbind methods
					General.Actions.UnbindMethods(this);

					// Save window settings
					General.Settings.WriteSetting(configpath + "expandedinfopanel", infopanel.IsExpanded);

					// Save recent files
					SaveRecentFiles();

					// Terminate the program
					General.Terminate(true);
				}
				else
				{
					// Cancel the close
					e.Cancel = true;
				}
			}
		}

		#endregion
		
		#region ================== Statusbar
		
		// This updates the status bar
		private void UpdateStatusbar()
		{
			// Map open?
			if(General.Map != null)
			{
				// Enable items
				xposlabel.Enabled = true;
				yposlabel.Enabled = true;
				poscommalabel.Enabled = true;
				zoomlabel.Enabled = true;
				buttonzoom.Enabled = true;
				gridlabel.Enabled = true;
				buttongrid.Enabled = true;
				configlabel.Text = General.Map.Config.Name;
			}
			else
			{
				// Disable items
				xposlabel.Text = "--";
				yposlabel.Text = "--";
				xposlabel.Enabled = false;
				yposlabel.Enabled = false;
				poscommalabel.Enabled = false;
				zoomlabel.Enabled = false;
				buttonzoom.Enabled = false;
				gridlabel.Enabled = false;
				buttongrid.Enabled = false;
				configlabel.Text = "";
			}
			
			UpdateStatusIcon();
		}
		
		// This flashes the status icon
		private void statusflasher_Tick(object sender, EventArgs e)
		{
			statusflashicon = !statusflashicon;
			UpdateStatusIcon();
			statusflashcount--;
			if(statusflashcount == 0) statusflasher.Stop();
		}
		
		// This resets the status to ready
		private void statusresetter_Tick(object sender, EventArgs e)
		{
			DisplayReady();
		}
		
		// This changes status text
		public void DisplayStatus(StatusType type, string message) { DisplayStatus(new StatusInfo(type, message)); }
		public void DisplayStatus(StatusInfo newstatus)
		{
			// Stop timers
			if(!newstatus.displayed)
			{
				statusresetter.Stop();
				statusflasher.Stop();
				statusflashicon = false;
			}
			
			// Determine what to do specifically for this status type
			switch(newstatus.type)
			{
				// When no particular information is to be displayed.
				// The messages displayed depends on running background processes.
				case StatusType.Ready:
					if((General.Map != null) && (General.Map.Data != null) && General.Map.Data.IsLoading)
						newstatus.message = STATUS_LOADING_TEXT;
					else
						newstatus.message = STATUS_READY_TEXT;
					break;

				// Shows information without flashing the icon.
				case StatusType.Info:
					if(!newstatus.displayed)
					{
						statusresetter.Interval = INFO_RESET_DELAY;
						statusresetter.Start();
					}
					break;
					
				// Shows action information and flashes up the status icon once.	
				case StatusType.Action:
					if(!newstatus.displayed)
					{
						statusflashicon = true;
						statusflasher.Interval = ACTION_FLASH_INTERVAL;
						statusflashcount = ACTION_FLASH_COUNT;
						statusflasher.Start();
						statusresetter.Interval = ACTION_RESET_DELAY;
						statusresetter.Start();
					}
					break;
					
				// Shows a warning, makes a warning sound and flashes a warning icon.
				case StatusType.Warning:
					if(!newstatus.displayed)
					{
						MessageBeep(MessageBeepType.Warning);
						statusflasher.Interval = WARNING_FLASH_INTERVAL;
						statusflashcount = WARNING_FLASH_COUNT;
						statusflasher.Start();
						statusresetter.Interval = WARNING_RESET_DELAY;
						statusresetter.Start();
					}
					break;
			}
			
			// Update status description
			status = newstatus;
			status.displayed = true;
			if(statuslabel.Text != status.message)
				statuslabel.Text = status.message;
			
			// Update icon as well
			UpdateStatusIcon();
			
			// Refresh
			statusbar.Invalidate();
			this.Update();
		}
		
		// This changes status text to Ready
		public void DisplayReady()
		{
			DisplayStatus(StatusType.Ready, null);
		}
		
		// This updates the status icon
		internal void UpdateStatusIcon()
		{
			int statusicon = 0;
			int statusflashindex = statusflashicon ? 1 : 0;
			
			// Loading icon?
			if((General.Map != null) && (General.Map.Data != null) && General.Map.Data.IsLoading)
				statusicon = 1;
			
			// Status type
			switch(status.type)
			{
				case StatusType.Ready:
				case StatusType.Info:
				case StatusType.Action:
					statuslabel.Image = STATUS_IMAGES[statusflashindex, statusicon];
					break;
				
				case StatusType.Busy:
					statuslabel.Image = STATUS_IMAGES[statusflashindex, 2];
					break;
					
				case StatusType.Warning:
					statuslabel.Image = STATUS_IMAGES[statusflashindex, 3];
					break;
			}
		}
		
		// This changes coordinates display
		public void UpdateCoordinates(Vector2D coords)
		{
			if(!coords.IsFinite())
			{
				xposlabel.Text = "--";
				yposlabel.Text = "--";
			}
			else
			{
				coords = General.Map.Grid.SnappedToGrid(coords);
				xposlabel.Text = coords.x.ToString("####0");
				yposlabel.Text = (-coords.y).ToString("####0");
			}
		}

		// This changes zoom display
		internal void UpdateZoom(float scale)
		{
			// Update scale label
			if(float.IsNaN(scale))
				zoomlabel.Text = "--";
			else
			{
				scale *= DISPLAYED_ZOOM_SCALER;
				zoomlabel.Text = scale.ToString("##0") + "%";
			}
		}

		// Zoom to a specified level
		private void itemzoomto_Click(object sender, EventArgs e)
		{
			if(General.Map == null) return;

			// In classic mode?
			if(General.Editing.Mode is ClassicMode)
			{
				// Requested from menu?
				if(sender is ToolStripMenuItem)
				{
					// Get integral zoom level
					int zoom = int.Parse((sender as ToolStripMenuItem).Tag.ToString(), CultureInfo.InvariantCulture);

					// Zoom now
					(General.Editing.Mode as ClassicMode).SetZoom((float)zoom / DISPLAYED_ZOOM_SCALER);
				}
			}
		}

		// Zoom to fit in screen
		private void itemzoomfittoscreen_Click(object sender, EventArgs e)
		{
			if(General.Map == null) return;
			
			// In classic mode?
			if(General.Editing.Mode is ClassicMode)
				(General.Editing.Mode as ClassicMode).CenterInScreen();
		}

		// This changes grid display
		internal void UpdateGrid(int gridsize)
		{
			// Update grid label
			if(gridsize == 0)
				gridlabel.Text = "--";
			else
				gridlabel.Text = gridsize.ToString("###0") + " mp";
		}

		// Set grid to a specified size
		private void itemgridsize_Click(object sender, EventArgs e)
		{
			if(General.Map == null) return;

			// In classic mode?
			if(General.Editing.Mode is ClassicMode)
			{
				// Requested from menu?
				if(sender is ToolStripMenuItem)
				{
					// Get integral zoom level
					int size = int.Parse((sender as ToolStripMenuItem).Tag.ToString(), CultureInfo.InvariantCulture);

					// Change grid size
					General.Map.Grid.SetGridSize(size);
					
					// Redraw display
					RedrawDisplay();
				}
			}
		}

		// Show grid setup
		private void itemgridcustom_Click(object sender, EventArgs e)
		{
			if(General.Map == null) return;
			General.Map.Grid.ShowGridSetup();
		}
		
		#endregion

		#region ================== Display

		// This shows the splash screen on display
		internal void ShowSplashDisplay()
		{
			// Change display to show splash logo
			display.SetSplashLogoDisplay();
			display.Cursor = Cursors.Default;
			this.Update();
		}
		
		// This clears the display
		internal void ClearDisplay()
		{
			// Clear the display
			display.SetManualRendering();
			this.Update();
		}

		// This sets the display cursor
		public void SetCursor(Cursor cursor)
		{
			// Only when a map is open
			if(General.Map != null) display.Cursor = cursor;
		}

		// This redraws the display on the next paint event
		public void RedrawDisplay()
		{
			if((General.Map != null) && (General.Editing.Mode != null))
			{
				General.Plugins.OnEditRedrawDisplayBegin();
				General.Editing.Mode.OnRedrawDisplay();
				General.Plugins.OnEditRedrawDisplayEnd();
				infopanel.UpdateStatistics(); //mxd
			}
			else
			{
				display.Invalidate();
			}
		}

		// This event is called when a repaint is needed
		private void display_Paint(object sender, PaintEventArgs e)
		{
			if(General.Map != null)
			{
				if(General.Editing.Mode != null)
				{
					if(!displayresized) General.Editing.Mode.OnPresentDisplay();
				}
				else
				{
					if(General.Colors != null)
						e.Graphics.Clear(Color.FromArgb(General.Colors.Background.ToInt()));
					else
						e.Graphics.Clear(SystemColors.ControlDarkDark);
				}
			}
		}
		
		// Redraw requested
		private void redrawtimer_Tick(object sender, EventArgs e)
		{
			// Disable timer (only redraw once)
			redrawtimer.Enabled = false;

			// Map opened?
			if(General.Map != null && !General.Map.IsDisposed)
			{
				// Display was resized?
				if(displayresized)
				{
					// Reset graphics to match changes
					General.Map.Graphics.Reset();

					//mxd. Aspect ratio may've been changed
					General.Map.CRenderer3D.CreateProjection();
				}

				// This is a dirty trick to give the display a new mousemove event with correct arguments
				if(mouseinside)
				{
					Point mousepos = Cursor.Position;
					Cursor.Position = new Point(mousepos.X + 1, mousepos.Y + 1);
					Cursor.Position = mousepos;
				}
				
				// Redraw now
				RedrawDisplay();
			}

			// Display resize is done
			displayresized = false;
		}

		// Display size changes
		private void display_Resize(object sender, EventArgs e)
		{
			// Resizing
			displayresized = true;
			
			// Request redraw
			if(!redrawtimer.Enabled) redrawtimer.Enabled = true;
		}
		
		// This requests a delayed redraw
		public void DelayedRedraw()
		{
			// Request redraw
			if(!redrawtimer.Enabled) redrawtimer.Enabled = true;
		}
		
		// Mouse click
		private void display_MouseClick(object sender, MouseEventArgs e)
		{
			if((General.Map != null) && (General.Editing.Mode != null))
			{
				General.Plugins.OnEditMouseClick(e);
				General.Editing.Mode.OnMouseClick(e);
			}
		}

		// Mouse doubleclick
		private void display_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if((General.Map != null) && (General.Editing.Mode != null))
			{
				General.Plugins.OnEditMouseDoubleClick(e);
				General.Editing.Mode.OnMouseDoubleClick(e);
			}
		}

		// Mouse down
		private void display_MouseDown(object sender, MouseEventArgs e)
		{
			int key = 0;
			
			LoseFocus(this, EventArgs.Empty);
			
			int mod = 0;
			if(alt) mod |= (int)Keys.Alt;
			if(shift) mod |= (int)Keys.Shift;
			if(ctrl) mod |= (int)Keys.Control;
			
			// Apply button
			mousebuttons |= e.Button;
			
			// Create key
			switch(e.Button)
			{
				case MouseButtons.Left: key = (int)Keys.LButton; break;
				case MouseButtons.Middle: key = (int)Keys.MButton; break;
				case MouseButtons.Right: key = (int)Keys.RButton; break;
				case MouseButtons.XButton1: key = (int)Keys.XButton1; break;
				case MouseButtons.XButton2: key = (int)Keys.XButton2; break;
			}
			
			// Invoke any actions associated with this key
			General.Actions.KeyPressed(key | mod);
			
			// Invoke on editing mode
			if((General.Map != null) && (General.Editing.Mode != null))
			{
				General.Plugins.OnEditMouseDown(e);
				General.Editing.Mode.OnMouseDown(e);
			}
		}

		// Mouse enters
		private void display_MouseEnter(object sender, EventArgs e)
		{
			mouseinside = true;
			if((General.Map != null) && (mouseinput == null) && (General.Editing.Mode != null))
			{
				General.Plugins.OnEditMouseEnter(e);
				General.Editing.Mode.OnMouseEnter(e);
			}
		}

		// Mouse leaves
		private void display_MouseLeave(object sender, EventArgs e)
		{
			mouseinside = false;
			if((General.Map != null) && (mouseinput == null) && (General.Editing.Mode != null))
			{
				General.Plugins.OnEditMouseLeave(e);
				General.Editing.Mode.OnMouseLeave(e);
			}
		}

		// Mouse moves
		private void display_MouseMove(object sender, MouseEventArgs e)
		{
			if((General.Map != null) && (mouseinput == null) && (General.Editing.Mode != null))
			{
				General.Plugins.OnEditMouseMove(e);
				General.Editing.Mode.OnMouseMove(e);
			}
		}

		// Mouse up
		private void display_MouseUp(object sender, MouseEventArgs e)
		{
			int key = 0;
			
			int mod = 0;
			if(alt) mod |= (int)Keys.Alt;
			if(shift) mod |= (int)Keys.Shift;
			if(ctrl) mod |= (int)Keys.Control;
			
			// Apply button
			mousebuttons &= ~e.Button;
			
			// Create key
			switch(e.Button)
			{
				case MouseButtons.Left: key = (int)Keys.LButton; break;
				case MouseButtons.Middle: key = (int)Keys.MButton; break;
				case MouseButtons.Right: key = (int)Keys.RButton; break;
				case MouseButtons.XButton1: key = (int)Keys.XButton1; break;
				case MouseButtons.XButton2: key = (int)Keys.XButton2; break;
			}
			
			// Invoke any actions associated with this key
			General.Actions.KeyReleased(key | mod);

			// Invoke on editing mode
			if((General.Map != null) && (General.Editing.Mode != null))
			{
				General.Plugins.OnEditMouseUp(e);
				General.Editing.Mode.OnMouseUp(e);
			}
		}
		
		#endregion

		#region ================== Input
		
		// This is a tool to lock the mouse in exclusive mode
		private void StartMouseExclusive()
		{
			// Not already locked?
			if(mouseinput == null)
			{
				// Start special input device
				mouseinput = new MouseInput(this);

				// Lock and hide the mouse in window
				originalclip = Cursor.Clip;
				Cursor.Clip = display.RectangleToScreen(display.ClientRectangle);
				Cursor.Hide();
			}
		}

		// This is a tool to unlock the mouse
		private void StopMouseExclusive()
		{
			// Locked?
			if(mouseinput != null)
			{
				// Stop special input device
				mouseinput.Dispose();
				mouseinput = null;

				// Release and show the mouse
				Cursor.Clip = originalclip;
				Cursor.Position = display.PointToScreen(new Point(display.ClientSize.Width / 2, display.ClientSize.Height / 2));
				Cursor.Show();
			}
		}
		
		// This requests exclusive mouse input
		public void StartExclusiveMouseInput()
		{
			// Only when not already in exclusive mode
			if(!mouseexclusive)
			{
				General.WriteLogLine("Starting exclusive mouse input mode...");
				
				// Start special input device
				StartMouseExclusive();
				mouseexclusive = true;
				mouseexclusivebreaklevel = 0;
			}
		}
		
		// This stops exclusive mouse input
		public void StopExclusiveMouseInput()
		{
			// Only when in exclusive mode
			if(mouseexclusive)
			{
				General.WriteLogLine("Stopping exclusive mouse input mode...");

				// Stop special input device
				StopMouseExclusive();
				mouseexclusive = false;
				mouseexclusivebreaklevel = 0;
			}
		}

		// This temporarely breaks exclusive mode and counts the break level
		public void BreakExclusiveMouseInput()
		{
			// Only when in exclusive mode
			if(mouseexclusive)
			{
				// Stop special input device
				StopMouseExclusive();
				
				// Count the break level
				mouseexclusivebreaklevel++;
			}
		}

		// This resumes exclusive mode from a break when all breaks have been called to resume
		public void ResumeExclusiveMouseInput()
		{
			// Only when in exclusive mode
			if(mouseexclusive && (mouseexclusivebreaklevel > 0))
			{
				// Decrease break level
				mouseexclusivebreaklevel--;

				// All break levels resumed? Then lock the mouse again.
				if(mouseexclusivebreaklevel == 0)
					StartMouseExclusive();
			}
		}

		// This releases all keys
		internal void ReleaseAllKeys()
		{
			General.Actions.ReleaseAllKeys();
			mousebuttons = MouseButtons.None;
			shift = false;
			ctrl = false;
			alt = false;
		}
		
		// When the mouse wheel is changed
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			int mod = 0;
			if(alt) mod |= (int)Keys.Alt;
			if(shift) mod |= (int)Keys.Shift;
			if(ctrl) mod |= (int)Keys.Control;
			
			// Scrollwheel up?
			if(e.Delta > 0)
			{
				// Invoke actions for scrollwheel
				General.Actions.KeyPressed((int)SpecialKeys.MScrollUp | mod);
				General.Actions.KeyReleased((int)SpecialKeys.MScrollUp | mod);
			}
			// Scrollwheel down?
			else if(e.Delta < 0)
			{
				// Invoke actions for scrollwheel
				General.Actions.KeyPressed((int)SpecialKeys.MScrollDown | mod);
				General.Actions.KeyReleased((int)SpecialKeys.MScrollDown | mod);
			}
			
			// Let the base know
			base.OnMouseWheel(e);
		}
		
		// When a key is pressed
		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			int mod = 0;
			
			// Keep key modifiers
			alt = e.Alt;
			shift = e.Shift;
			ctrl = e.Control;
			if(alt) mod |= (int)Keys.Alt;
			if(shift) mod |= (int)Keys.Shift;
			if(ctrl) mod |= (int)Keys.Control;
			
			// Don't process any keys when they are meant for other input controls
			if((ActiveControl == null) || (ActiveControl == display))
			{
				// Invoke any actions associated with this key
				General.Actions.UpdateModifiers(mod);
				e.Handled = General.Actions.KeyPressed((int)e.KeyData);
				
				// Invoke on editing mode
				if((General.Map != null) && (General.Editing.Mode != null))
				{
					General.Plugins.OnEditKeyDown(e);
					General.Editing.Mode.OnKeyDown(e);
				}

				// Handled
				if(e.Handled) e.SuppressKeyPress = true;
			}
			
			// F1 pressed?
			if((e.KeyCode == Keys.F1) && (e.Modifiers == Keys.None))
			{
				// No action bound to F1?
				Actions.Action[] f1actions = General.Actions.GetActionsByKey((int)e.KeyData);
				if(f1actions.Length == 0)
				{
					// If we don't have any map open, show the Main Window help
					// otherwise, give the help request to the editing mode so it
					// can open the appropriate help file.
					if((General.Map == null) || (General.Editing.Mode == null))
					{
						General.ShowHelp("introduction.html");
					}
					else
					{
						General.Editing.Mode.OnHelp();
					}
				}
			}
		}

		// When a key is released
		private void MainForm_KeyUp(object sender, KeyEventArgs e)
		{
			int mod = 0;
			
			// Keep key modifiers
			alt = e.Alt;
			shift = e.Shift;
			ctrl = e.Control;
			if(alt) mod |= (int)Keys.Alt;
			if(shift) mod |= (int)Keys.Shift;
			if(ctrl) mod |= (int)Keys.Control;
			
			// Don't process any keys when they are meant for other input controls
			if((ActiveControl == null) || (ActiveControl == display))
			{
				// Invoke any actions associated with this key
				General.Actions.UpdateModifiers(mod);
				e.Handled = General.Actions.KeyReleased((int)e.KeyData);
				
				// Invoke on editing mode
				if((General.Map != null) && (General.Editing.Mode != null))
				{
					General.Plugins.OnEditKeyUp(e);
					General.Editing.Mode.OnKeyUp(e);
				}

				// Handled
				if(e.Handled) e.SuppressKeyPress = true;
			}
		}
		
		// These prevent focus changes by way of TAB or Arrow keys
		protected override bool IsInputChar(char charCode) { return false; }
		protected override bool IsInputKey(Keys keyData) { return false; }
		protected override bool ProcessKeyPreview(ref Message m) { return false; }
		protected override bool ProcessDialogKey(Keys keyData) { return false; }
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) { return false; }
		
		// This fixes some odd input behaviour
		private void display_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if((ActiveControl == null) || (ActiveControl == display))
			{
				LoseFocus(this, EventArgs.Empty);
				KeyEventArgs ea = new KeyEventArgs(e.KeyData);
				MainForm_KeyDown(sender, ea);
			}
		}

		//mxd. This fixes some odd input behaviour
		private void display_KeyUp(object sender, KeyEventArgs e)
		{
			if((ActiveControl == null) || (ActiveControl == display))
			{
				LoseFocus(this, EventArgs.Empty);
				MainForm_KeyUp(sender, e);
			}
		}
		
		#endregion

		#region ================== Toolbar
		
		// This updates the skills list
		private void UpdateSkills()
		{
			// Clear list
			buttontest.DropDownItems.Clear();
			
			// Map loaded?
			if(General.Map != null)
			{
				// Make the new skills list
				skills = new ToolStripItem[(General.Map.Config.Skills.Count * 2) + 1];
				int addindex = 0;
				
				// Positive skills are with monsters
				foreach(SkillInfo si in General.Map.Config.Skills)
				{
					ToolStripMenuItem menuitem = new ToolStripMenuItem(si.ToString()) { Image = Resources.Monster2 };
					menuitem.Click += TestSkill_Click;
					menuitem.Tag = si.Index;
					menuitem.Checked = (General.Settings.TestMonsters && (General.Map.ConfigSettings.TestSkill == si.Index));
					skills[addindex++] = menuitem;
				}

				// Add seperator
				skills[addindex] = new ToolStripSeparator { Padding = new Padding(0, 3, 0, 3) };
				addindex++;

				// Negative skills are without monsters
				foreach(SkillInfo si in General.Map.Config.Skills)
				{
					ToolStripMenuItem menuitem = new ToolStripMenuItem(si.ToString()) { Image = Resources.Monster3 };
					menuitem.Click += TestSkill_Click;
					menuitem.Tag = -si.Index;
					menuitem.Checked = (!General.Settings.TestMonsters && (General.Map.ConfigSettings.TestSkill == si.Index));
					skills[addindex++] = menuitem;
				}
				
				// Add to list
				buttontest.DropDownItems.AddRange(skills);
			}
		}
		
		// Event handler for testing at a specific skill
		private void TestSkill_Click(object sender, EventArgs e)
		{
			int skill = (int)((sender as ToolStripMenuItem).Tag);
			General.Settings.TestMonsters = (skill > 0);
			General.Map.ConfigSettings.TestSkill = Math.Abs(skill);
			General.Map.Launcher.TestAtSkill(Math.Abs(skill));
			UpdateSkills();
		}
		
		// This loses focus
		private void LoseFocus(object sender, EventArgs e)
		{
			// Lose focus!
			try { display.Focus(); } catch(Exception) { }
			this.ActiveControl = null;
		}

		// Things filter selected
		private void thingfilters_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Only possible when a map is open
			if((General.Map != null) && !updatingfilters)
			{
				updatingfilters = true;
				
				// Change filter
				General.Map.ChangeThingFilter(thingfilters.SelectedItem as ThingsFilter);

				updatingfilters = false;
			}
			
			// Lose focus
			if(!thingfilters.DroppedDown) LoseFocus(sender, e);
		}
		
		// This updates the things filter on the toolbar
		internal void UpdateThingsFilters()
		{
			// Only possible to list filters when a map is open
			if(General.Map != null)
			{
				ThingsFilter oldfilter = null;
				if(thingfilters.SelectedIndex > -1)
					oldfilter = thingfilters.SelectedItem as ThingsFilter;
				
				updatingfilters = true;

				// Clear the list
				thingfilters.Items.Clear();

				// Add null filter
				if(General.Map.ThingsFilter is NullThingsFilter)
					thingfilters.Items.Add(General.Map.ThingsFilter);
				else
					thingfilters.Items.Add(new NullThingsFilter());

				// Add all filters
				foreach(ThingsFilter f in General.Map.ConfigSettings.ThingsFilters)
					thingfilters.Items.Add(f);

				// Select current filter
				foreach(ThingsFilter f in thingfilters.Items)
					if(f == General.Map.ThingsFilter) thingfilters.SelectedItem = f;

				updatingfilters = false;
				
				// No filter selected?
				if(thingfilters.SelectedIndex == -1)
				{
					// Select the first and update
					thingfilters.SelectedIndex = 0;
				}
				// Another filter got selected?
				else if(oldfilter != (thingfilters.SelectedItem as ThingsFilter))
				{
					// Update!
					thingfilters_SelectedIndexChanged(this, EventArgs.Empty);
				}
			}
			else
			{
				// Clear the list
				thingfilters.Items.Clear();
			}
		}

		// This selects the things filter based on the filter set on the map manager
		internal void ReflectThingsFilter()
		{
			if(!updatingfilters)
			{
				updatingfilters = true;
				
				// Select current filter
				bool selecteditemfound = false;
				foreach(ThingsFilter f in thingfilters.Items)
				{
					if(f == General.Map.ThingsFilter)
					{
						thingfilters.SelectedItem = f;
						selecteditemfound = true;
					}
				}

				// Not in the list?
				if(!selecteditemfound)
				{
					// Select nothing
					thingfilters.SelectedIndex = -1;
				}

				updatingfilters = false;
			}
		}
		
		// This adds a button to the toolbar
		public void AddButton(ToolStripItem button) { AddButton(button, ToolbarSection.Custom, General.Plugins.FindPluginByAssembly(Assembly.GetCallingAssembly())); }
		public void AddButton(ToolStripItem button, ToolbarSection section) { AddButton(button, section, General.Plugins.FindPluginByAssembly(Assembly.GetCallingAssembly())); }
		private void AddButton(ToolStripItem button, ToolbarSection section, Plugin plugin)
		{
			// Fix tags to full action names
			ToolStripItemCollection items = new ToolStripItemCollection(toolbar, new ToolStripItem[0]);
			items.Add(button);
			RenameTagsToFullActions(items, plugin);

			// Add to the list so we can update it as needed
			PluginToolbarButton buttoninfo = new PluginToolbarButton();
			buttoninfo.button = button;
			buttoninfo.section = section;
			pluginbuttons.Add(buttoninfo);
			
			// Bind visible changed event
			if(!(button is ToolStripSeparator)) button.VisibleChanged += buttonvisiblechangedhandler;
			
			// Insert the button in the right section
			switch(section)
			{
				case ToolbarSection.File: toolbar.Items.Insert(toolbar.Items.IndexOf(seperatorfile), button); break;
				case ToolbarSection.UndoRedo: toolbar.Items.Insert(toolbar.Items.IndexOf(seperatorundo), button); break;
				case ToolbarSection.CopyPaste: toolbar.Items.Insert(toolbar.Items.IndexOf(seperatorcopypaste), button); break;
				case ToolbarSection.Sprites: toolbar.Items.Insert(toolbar.Items.IndexOf(buttonviewnormal), button); break;
				case ToolbarSection.Views: toolbar.Items.Insert(toolbar.Items.IndexOf(seperatorviews), button); break;
				case ToolbarSection.Geometry: toolbar.Items.Insert(toolbar.Items.IndexOf(seperatorgeometry), button); break;
				case ToolbarSection.Testing: toolbar.Items.Insert(toolbar.Items.IndexOf(seperatortesting), button); break;
				case ToolbarSection.Custom: toolbar.Items.Add(button); break;
			}
			
			UpdateToolbar();
		}

		// Removes a button
		public void RemoveButton(ToolStripItem button)
		{
			// Find in the list and remove it
			PluginToolbarButton buttoninfo = new PluginToolbarButton();
			for(int i = 0; i < pluginbuttons.Count; i++)
			{
				if(pluginbuttons[i].button == button)
				{
					buttoninfo = pluginbuttons[i];
					pluginbuttons.RemoveAt(i);
					break;
				}
			}

			if(buttoninfo.button != null)
			{
				// Unbind visible changed event
				if(!(button is ToolStripSeparator)) button.VisibleChanged -= buttonvisiblechangedhandler;

				// Remove button from toolbar
				toolbar.Items.Remove(button);
				UpdateSeparators();
			}
		}

		// This handle visibility changes in the toolbar buttons
		private void ToolbarButtonVisibleChanged(object sender, EventArgs e)
		{
			if(!preventupdateseperators)
			{
				// Update the seeprators
				UpdateSeparators();
			}
		}

		// This hides redundant seperators and shows single seperators
		internal void UpdateSeparators()
		{
			UpdateToolStripSeparators(toolbar.Items, false);
			UpdateToolStripSeparators(menumode.DropDownItems, true);
		}
		
		// This updates the seperators
		// Hides redundant seperators and shows single seperators
		private void UpdateToolStripSeparators(ToolStripItemCollection items, bool defaultvisible)
		{
			ToolStripItem pvi = null;
			foreach(ToolStripItem i in items)
			{
				bool separatorvisible = false;
				
				// This is a seperator?
				if(i is ToolStripSeparator)
				{
					// Make visible when previous item was not a seperator
					separatorvisible = !(pvi is ToolStripSeparator) && (pvi != null);
					i.Visible = separatorvisible;
				}
				
				// Keep as previous visible item
				if(i.Visible || separatorvisible || (defaultvisible && !(i is ToolStripSeparator))) pvi = i;
			}
			
			// Hide last item if it is a seperator
			if(pvi is ToolStripSeparator) pvi.Visible = false;
		}
		
		// This enables or disables all editing mode items and toolbar buttons
		private void UpdateToolbar()
		{
			preventupdateseperators = true;

			// Show/hide items based on preferences
			bool maploaded = (General.Map != null); //mxd
			buttonnewmap.Visible = General.Settings.ToolbarFile;
			buttonopenmap.Visible = General.Settings.ToolbarFile;
			buttonsavemap.Visible = General.Settings.ToolbarFile;
			buttonundo.Visible = General.Settings.ToolbarUndo && maploaded;
			buttonredo.Visible = General.Settings.ToolbarUndo && maploaded;
			buttoncut.Visible = General.Settings.ToolbarCopy && maploaded;
			buttoncopy.Visible = General.Settings.ToolbarCopy && maploaded;
			buttonpaste.Visible = General.Settings.ToolbarCopy && maploaded;
			buttonthingsfilter.Visible = General.Settings.ToolbarFilter && maploaded;
			thingfilters.Visible = General.Settings.ToolbarFilter && maploaded;
			buttonviewbrightness.Visible = General.Settings.ToolbarViewModes && maploaded;
			buttonviewceilings.Visible = General.Settings.ToolbarViewModes && maploaded;
			buttonviewfloors.Visible = General.Settings.ToolbarViewModes && maploaded;
			buttonviewnormal.Visible = General.Settings.ToolbarViewModes && maploaded;
			buttonsnaptogrid.Visible = General.Settings.ToolbarGeometry && maploaded;
			buttonautomerge.Visible = General.Settings.ToolbarGeometry && maploaded;
			buttontest.Visible = General.Settings.ToolbarTesting && maploaded;

			//mxd. Show/hide additional panels
			infopanel.Visible = maploaded;

			// Update plugin buttons
			foreach(PluginToolbarButton p in pluginbuttons)
			{
				switch(p.section)
				{
					case ToolbarSection.File: p.button.Visible = General.Settings.ToolbarFile; break;
					case ToolbarSection.UndoRedo: p.button.Visible = General.Settings.ToolbarUndo; break;
					case ToolbarSection.CopyPaste: p.button.Visible = General.Settings.ToolbarCopy; break;
					case ToolbarSection.Sprites: p.button.Visible = General.Settings.ToolbarFilter; break;
					case ToolbarSection.Views: p.button.Visible = General.Settings.ToolbarViewModes; break;
					case ToolbarSection.Geometry: p.button.Visible = General.Settings.ToolbarGeometry; break;
					case ToolbarSection.Testing: p.button.Visible = General.Settings.ToolbarTesting; break;
				}
			}
			
			// Show/hide items based on preferences
			/*buttonnewmap.Visible = General.Settings.ToolbarFile;
			buttonopenmap.Visible = General.Settings.ToolbarFile;
			buttonsavemap.Visible = General.Settings.ToolbarFile;
			buttonundo.Visible = General.Settings.ToolbarUndo;
			buttonredo.Visible = General.Settings.ToolbarUndo;
			buttoncut.Visible = General.Settings.ToolbarCopy;
			buttoncopy.Visible = General.Settings.ToolbarCopy;
			buttonpaste.Visible = General.Settings.ToolbarCopy;
			buttonthingsfilter.Visible = General.Settings.ToolbarFilter;
			thingfilters.Visible = General.Settings.ToolbarFilter;
			buttonviewbrightness.Visible = General.Settings.ToolbarViewModes;
			buttonviewceilings.Visible = General.Settings.ToolbarViewModes;
			buttonviewfloors.Visible = General.Settings.ToolbarViewModes;
			buttonviewnormal.Visible = General.Settings.ToolbarViewModes;
			buttonsnaptogrid.Visible = General.Settings.ToolbarGeometry;
			buttonautomerge.Visible = General.Settings.ToolbarGeometry;
			buttontest.Visible = General.Settings.ToolbarTesting;

			// Enable/disable all edit mode items
			foreach(ToolStripItem i in editmodeitems) i.Enabled = (General.Map != null);

			// Update plugin buttons
			foreach(PluginToolbarButton p in pluginbuttons)
			{
				switch(p.section)
				{
					case ToolbarSection.File: p.button.Visible = General.Settings.ToolbarFile; break;
					case ToolbarSection.UndoRedo: p.button.Visible = General.Settings.ToolbarUndo; break;
					case ToolbarSection.CopyPaste: p.button.Visible = General.Settings.ToolbarCopy; break;
					case ToolbarSection.Sprites: p.button.Visible = General.Settings.ToolbarFilter; break;
					case ToolbarSection.Views: p.button.Visible = General.Settings.ToolbarViewModes; break;
					case ToolbarSection.Geometry: p.button.Visible = General.Settings.ToolbarGeometry; break;
					case ToolbarSection.Testing: p.button.Visible = General.Settings.ToolbarTesting; break;
				}
			}*/

			preventupdateseperators = false;

			UpdateSeparators();
		}

		// This checks one of the edit mode items (and unchecks all others)
		internal void CheckEditModeButton(string modeclassname)
		{
			// Go for all items
			foreach(ToolStripItem i in editmodeitems)
			{
				// Check what type it is
				if(i is ToolStripMenuItem)
				{
					// Check if mode type matches with given name
					(i as ToolStripMenuItem).Checked = ((i.Tag as EditModeInfo).Type.Name == modeclassname);
				}
				else if(i is ToolStripButton)
				{
					// Check if mode type matches with given name
					(i as ToolStripButton).Checked = ((i.Tag as EditModeInfo).Type.Name == modeclassname);
				}
			}
		}
		
		// This removes the config-specific editing mode buttons
		internal void RemoveEditModeButtons()
		{
			// Go for all items
			foreach(ToolStripItem i in editmodeitems)
			{
				// Remove it and restart
				toolbar.Items.Remove(i);
				menumode.DropDownItems.Remove(i);
				i.Dispose();
			}
			
			// Done
			editmodeitems.Clear();
			UpdateSeparators();
		}
		
		// This adds an editing mode seperator on the toolbar and menu
		internal void AddEditModeSeperator()
		{
			ToolStripSeparator item;
			int index;
			
			// Create a button
			index = toolbar.Items.IndexOf(seperatormodes);
			item = new ToolStripSeparator();
			item.Margin = new Padding(6, 0, 6, 0);
			toolbar.Items.Insert(index, item);
			editmodeitems.Add(item);
			
			// Create menu item
			index = menumode.DropDownItems.Count;
			item = new ToolStripSeparator();
			item.Margin = new Padding(0, 3, 0, 3);
			menumode.DropDownItems.Insert(index, item);
			editmodeitems.Add(item);
			
			UpdateSeparators();
		}
		
		// This adds an editing mode button to the toolbar and edit menu
		internal void AddEditModeButton(EditModeInfo modeinfo)
		{
			ToolStripItem item;
			int index;

			string controlname = modeinfo.ButtonDesc.Replace("&", "&&");
			
			// Create a button
			index = toolbar.Items.IndexOf(seperatormodes);
			item = new ToolStripButton(modeinfo.ButtonDesc, modeinfo.ButtonImage, new EventHandler(EditModeButtonHandler));
			item.DisplayStyle = ToolStripItemDisplayStyle.Image;
			item.Tag = modeinfo;
			toolbar.Items.Insert(index, item);
			editmodeitems.Add(item);
			
			// Create menu item
			index = menumode.DropDownItems.Count;
			item = new ToolStripMenuItem(controlname, modeinfo.ButtonImage, new EventHandler(EditModeButtonHandler));
			item.Tag = modeinfo;
			menumode.DropDownItems.Insert(index, item);
			editmodeitems.Add(item);
			item.Visible = true;
			
			ApplyShortcutKeys(menumode.DropDownItems);
			UpdateSeparators();
		}

		// This handles edit mode button clicks
		private void EditModeButtonHandler(object sender, EventArgs e)
		{
			EditModeInfo modeinfo;
			
			this.Update();
			modeinfo = (EditModeInfo)((sender as ToolStripItem).Tag);
			General.Actions.InvokeAction(modeinfo.SwitchAction.GetFullActionName(modeinfo.Plugin.Assembly));
			this.Update();
		}

		#endregion

		#region ================== Menus

		// This adds a menu to the menus bar
		public void AddMenu(ToolStripMenuItem menu) { AddMenu(menu, MenuSection.Top, General.Plugins.FindPluginByAssembly(Assembly.GetCallingAssembly())); }
		public void AddMenu(ToolStripMenuItem menu, MenuSection section) { AddMenu(menu, section, General.Plugins.FindPluginByAssembly(Assembly.GetCallingAssembly())); }
		private void AddMenu(ToolStripMenuItem menu, MenuSection section, Plugin plugin)
		{
			// Fix tags to full action names
			ToolStripItemCollection items = new ToolStripItemCollection(this.menumain, new ToolStripItem[0]);
			items.Add(menu);
			RenameTagsToFullActions(items, plugin);
			
			// Insert the menu in the right location
			switch(section)
			{
				case MenuSection.FileNewOpenClose: menufile.DropDownItems.Insert(menufile.DropDownItems.IndexOf(seperatorfileopen), menu); break;
				case MenuSection.FileSave: menufile.DropDownItems.Insert(menufile.DropDownItems.IndexOf(seperatorfilesave), menu); break;
				case MenuSection.FileRecent: menufile.DropDownItems.Insert(menufile.DropDownItems.IndexOf(seperatorfilerecent), menu); break;
				case MenuSection.FileExit: menufile.DropDownItems.Insert(menufile.DropDownItems.IndexOf(itemexit), menu); break;
				case MenuSection.EditUndoRedo: menuedit.DropDownItems.Insert(menuedit.DropDownItems.IndexOf(seperatoreditundo), menu); break;
				case MenuSection.EditCopyPaste: menuedit.DropDownItems.Insert(menuedit.DropDownItems.IndexOf(seperatoreditcopypaste), menu); break;
				case MenuSection.EditGeometry: menuedit.DropDownItems.Insert(menuedit.DropDownItems.IndexOf(seperatoreditgeometry), menu); break;
				case MenuSection.EditGrid: menuedit.DropDownItems.Insert(menuedit.DropDownItems.IndexOf(seperatoreditgrid), menu); break;
				case MenuSection.EditMapOptions: menuedit.DropDownItems.Add(menu); break;
				case MenuSection.ViewThings: menuview.DropDownItems.Insert(menuview.DropDownItems.IndexOf(seperatorviewthings), menu); break;
				case MenuSection.ViewViews: menuview.DropDownItems.Insert(menuview.DropDownItems.IndexOf(seperatorviewviews), menu); break;
				case MenuSection.ViewZoom: menuview.DropDownItems.Insert(menuview.DropDownItems.IndexOf(seperatorviewzoom), menu); break;
				case MenuSection.ViewScriptEdit: menuview.DropDownItems.Add(menu); break;
				case MenuSection.ToolsResources: menutools.DropDownItems.Insert(menutools.DropDownItems.IndexOf(seperatortoolsresources), menu); break;
				case MenuSection.ToolsConfiguration: menutools.DropDownItems.Insert(menutools.DropDownItems.IndexOf(seperatortoolsconfig), menu); break;
				case MenuSection.ToolsTesting: menutools.DropDownItems.Add(menu); break;
				case MenuSection.HelpManual: menuhelp.DropDownItems.Insert(menuhelp.DropDownItems.IndexOf(seperatorhelpmanual), menu); break;
				case MenuSection.HelpAbout: menuhelp.DropDownItems.Add(menu); break;
				case MenuSection.Top: menumain.Items.Insert(menumain.Items.IndexOf(menutools), menu); break;
			}
			
			ApplyShortcutKeys(items);
		}
		
		// Removes a menu
		public void RemoveMenu(ToolStripMenuItem menu)
		{
			// We actually have no idea in which menu this item is,
			// so try removing from all menus and the top strip
			menufile.DropDownItems.Remove(menu);
			menuedit.DropDownItems.Remove(menu);
			menuview.DropDownItems.Remove(menu);
			menutools.DropDownItems.Remove(menu);
			menuhelp.DropDownItems.Remove(menu);
			menumain.Items.Remove(menu);
		}
		
		// Public method to apply shortcut keys
		internal void ApplyShortcutKeys()
		{
			// Apply shortcut keys to menus
			ApplyShortcutKeys(menumain.Items);
		}
		
		// This sets the shortcut keys on menu items
		private static void ApplyShortcutKeys(ToolStripItemCollection items)
		{
			// Go for all controls to find menu items
			foreach(ToolStripItem item in items)
			{
				// This is a menu item?
				if(item is ToolStripMenuItem)
				{
					// Get the item in proper type
					ToolStripMenuItem menuitem = (item as ToolStripMenuItem);

					// Tag set for this item?
					if(menuitem.Tag is string)
					{
						// Action with this name available?
						string actionname = menuitem.Tag.ToString();
						if(General.Actions.Exists(actionname))
						{
							// Put the action shortcut key on the menu item
							menuitem.ShortcutKeyDisplayString = Actions.Action.GetShortcutKeyDesc(General.Actions[actionname].ShortcutKey);
						}
					}
					// Edit mode info set for this item?
					else if(menuitem.Tag is EditModeInfo)
					{
						// Action with this name available?
						EditModeInfo modeinfo = (menuitem.Tag as EditModeInfo);
						string actionname = modeinfo.SwitchAction.GetFullActionName(modeinfo.Plugin.Assembly);
						if(General.Actions.Exists(actionname))
						{
							// Put the action shortcut key on the menu item
							menuitem.ShortcutKeyDisplayString = Actions.Action.GetShortcutKeyDesc(General.Actions[actionname].ShortcutKey);
						}
					}

					// Recursively apply shortcut keys to child menu items as well
					ApplyShortcutKeys(menuitem.DropDownItems);
				}
			}
		}

		// This fixes short action names to fully qualified action names on menu item tags
		private static void RenameTagsToFullActions(ToolStripItemCollection items, Plugin plugin)
		{
			// Go for all controls to find menu items
			foreach(ToolStripItem item in items)
			{
				// Tag set for this item?
				if(item.Tag is string)
				{
					// Get the action name
					string actionname = item.Tag.ToString();

					// Check if the tag doe not already begin with the assembly name
					if(!actionname.StartsWith(plugin.Name + "_", StringComparison.InvariantCultureIgnoreCase))
					{
						// Change the tag to a fully qualified action name
						item.Tag = plugin.Name.ToLowerInvariant() + "_" + actionname;
					}
				}

				// This is a menu item?
				if(item is ToolStripMenuItem)
				{
					// Get the item in proper type
					ToolStripMenuItem menuitem = (item as ToolStripMenuItem);
					
					// Recursively perform operation on child menu items
					RenameTagsToFullActions(menuitem.DropDownItems, plugin);
				}
			}
		}
		
		#endregion

		#region ================== File Menu

		// This sets up the file menu
		private void UpdateFileMenu()
		{
			//mxd. Show/hide items
			bool show = (General.Map != null);
			itemclosemap.Visible = show;
			itemsavemap.Visible = show;
			itemsavemapas.Visible = show;
			itemsavemapinto.Visible = show;
			//seperatorfileopen.Visible = show; //mxd
			seperatorfilesave.Visible = show; //mxd

			// Toolbar icons
			buttonsavemap.Visible = show;
			buttonsavemap.Enabled = show;
		}

		// This sets the recent files from configuration
		private void CreateRecentFiles()
		{
			bool anyitems = false;

			// Where to insert
			int insertindex = menufile.DropDownItems.IndexOf(itemnorecent);
			
			// Create all items
			recentitems = new ToolStripMenuItem[MAX_RECENT_FILES];
			for(int i = 0; i < MAX_RECENT_FILES; i++)
			{
				// Create item
				recentitems[i] = new ToolStripMenuItem("");
				recentitems[i].Tag = "";
				recentitems[i].Click += recentitem_Click;
				menufile.DropDownItems.Insert(insertindex + i, recentitems[i]);

				// Get configuration setting
				string filename = General.Settings.ReadSetting("recentfiles.file" + i, "");
				if(filename != "")
				{
					// Set up item
					int number = i + 1;
					recentitems[i].Text = "&" + number + "  " + GetDisplayFilename(filename);
					recentitems[i].Tag = filename;
					recentitems[i].Visible = true;
					anyitems = true;
				}
				else
				{
					// Hide item
					recentitems[i].Visible = false;
				}
			}

			// Hide the no recent item when there are items
			itemnorecent.Visible = !anyitems;
		}
		
		// This saves the recent files list
		private void SaveRecentFiles()
		{
			// Go for all items
			for(int i = 0; i < MAX_RECENT_FILES; i++)
			{
				// Recent file set?
				if(recentitems[i].Text != "")
				{
					// Save to configuration
					General.Settings.WriteSetting("recentfiles.file" + i, recentitems[i].Tag.ToString());
				}
			}
		}
		
		// This adds a recent file to the list
		internal void AddRecentFile(string filename)
		{
			int movedownto = MAX_RECENT_FILES - 1;
			
			// Check if this file is already in the list
			for(int i = 0; i < MAX_RECENT_FILES; i++)
			{
				// File same as this item?
				if(string.Compare(filename, recentitems[i].Tag.ToString(), true) == 0)
				{
					// Move down to here so that this item will disappear
					movedownto = i;
					break;
				}
			}
			
			// Go for all items, except the last one, backwards
			for(int i = movedownto - 1; i >= 0; i--)
			{
				// Move recent file down the list
				int number = i + 2;
				recentitems[i + 1].Text = "&" + number + "  " + GetDisplayFilename(recentitems[i].Tag.ToString());
				recentitems[i + 1].Tag = recentitems[i].Tag.ToString();
				recentitems[i + 1].Visible = (recentitems[i].Tag.ToString() != "");
			}

			// Add new file at the top
			recentitems[0].Text = "&1  " + GetDisplayFilename(filename);
			recentitems[0].Tag = filename;
			recentitems[0].Visible = true;

			// Hide the no recent item
			itemnorecent.Visible = false;
		}

		// This returns the trimmed file/path string
		private string GetDisplayFilename(string filename)
		{
			// String doesnt fit?
			if(MeasureString(filename, this.Font).Width > MAX_RECENT_FILES_PIXELS)
			{
				// Start chopping off characters
				for(int i = filename.Length - 6; i >= 0; i--)
				{
					// Does it fit now?
					string newname = filename.Substring(0, 3) + "..." + filename.Substring(filename.Length - i, i);
					if(MeasureString(newname, this.Font).Width <= MAX_RECENT_FILES_PIXELS)
						return newname;
				}

				// Cant find anything that fits (most unlikely!)
				return "wtf?!";
			}

			// The whole string fits
			return filename;
		}
		
		// Exit clicked
		private void itemexit_Click(object sender, EventArgs e) { this.Close(); }

		// Recent item clicked
		private void recentitem_Click(object sender, EventArgs e)
		{
			// Get the item that was clicked
			ToolStripItem item = (sender as ToolStripItem);

			// Open this file
			General.OpenMapFile(item.Tag.ToString());
		}
		
		#endregion

		#region ================== Edit Menu

		// This sets up the edit menu
		private void UpdateEditMenu()
		{
			// No edit menu when no map open
			menuedit.Visible = (General.Map != null);
			
			// Enable/disable items
			itemundo.Enabled = (General.Map != null) && (General.Map.UndoRedo.NextUndo != null);
			itemredo.Enabled = (General.Map != null) && (General.Map.UndoRedo.NextRedo != null);
			itemcut.Enabled = (General.Map != null) && (General.Editing.Mode != null) && General.Editing.Mode.Attributes.AllowCopyPaste;
			itemcopy.Enabled = (General.Map != null) && (General.Editing.Mode != null) && General.Editing.Mode.Attributes.AllowCopyPaste;
			itempaste.Enabled = (General.Map != null) && (General.Editing.Mode != null) && General.Editing.Mode.Attributes.AllowCopyPaste;
			itempastespecial.Enabled = (General.Map != null) && (General.Editing.Mode != null) && General.Editing.Mode.Attributes.AllowCopyPaste;
			itemmapoptions.Enabled = (General.Map != null);
			itemsnaptogrid.Enabled = (General.Map != null);
			itemautomerge.Enabled = (General.Map != null);
			itemgridsetup.Enabled = (General.Map != null);
			itemgridinc.Enabled = (General.Map != null);
			itemgriddec.Enabled = (General.Map != null);

			// Determine undo description
			if(itemundo.Enabled)
				itemundo.Text = "Undo " + General.Map.UndoRedo.NextUndo.Description;
			else
				itemundo.Text = "Undo";

			// Determine redo description
			if(itemredo.Enabled)
				itemredo.Text = "Redo " + General.Map.UndoRedo.NextRedo.Description;
			else
				itemredo.Text = "Redo";
			
			// Toolbar icons
			buttonundo.Enabled = itemundo.Enabled;
			buttonredo.Enabled = itemredo.Enabled;
			buttonundo.ToolTipText = itemundo.Text;
			buttonredo.ToolTipText = itemredo.Text;
			buttonsnaptogrid.Enabled = (General.Map != null);
			buttonautomerge.Enabled = (General.Map != null);
			buttoncut.Enabled = itemcut.Enabled;
			buttoncopy.Enabled = itemcopy.Enabled;
			buttonpaste.Enabled = itempaste.Enabled;
		}

		// Action to toggle snap to grid
		[BeginAction("togglesnap")]
		internal void ToggleSnapToGrid()
		{
			buttonsnaptogrid.Checked = !buttonsnaptogrid.Checked;
			itemsnaptogrid.Checked = buttonsnaptogrid.Checked;
			string onoff = buttonsnaptogrid.Checked ? "ON" : "OFF";
			DisplayStatus(StatusType.Action, "Snap to grid is now " + onoff + " by default.");
		}

		// Action to toggle auto merge
		[BeginAction("toggleautomerge")]
		internal void ToggleAutoMerge()
		{
			buttonautomerge.Checked = !buttonautomerge.Checked;
			itemautomerge.Checked = buttonautomerge.Checked;
			string onoff = buttonautomerge.Checked ? "ON" : "OFF";
			DisplayStatus(StatusType.Action, "Snap to geometry is now " + onoff + " by default.");
		}
		
		#endregion

		#region ================== View Menu

		// This sets up the modes menu
		private void UpdateViewMenu()
		{
			menuview.Visible = (General.Map != null); //mxd
			
			// Menu items
			itemthingsfilter.Enabled = (General.Map != null);
			itemscripteditor.Enabled = (General.Map != null);
			itemfittoscreen.Enabled = (General.Map != null);
			menuzoom.Enabled = (General.Map != null);
			itemtoggleinfo.Checked = infopanel.IsExpanded;
			
			// View mode items
			for(int i = 0; i < Renderer2D.NUM_VIEW_MODES; i++)
			{
				// NOTE: We only disable them when no map is loaded, because they may
				// need to be disabled for non-classic modes
				if(General.Map == null)
				{
					viewmodesbuttons[i].Enabled = false;
					viewmodesbuttons[i].Checked = false;
					viewmodesitems[i].Enabled = false;
					viewmodesitems[i].Checked = false;
				}
				else
				{
					// Check the correct item
					viewmodesbuttons[i].Checked = (i == (int)General.Map.CRenderer2D.ViewMode);
					viewmodesitems[i].Checked = (i == (int)General.Map.CRenderer2D.ViewMode);
				}
			}
			
			// Toolbar icons
			thingfilters.Enabled = (General.Map != null);
			buttonthingsfilter.Enabled = (General.Map != null);
		}

		#endregion

		#region ================== Mode Menu

		// This sets up the modes menu
		private void UpdateModeMenu()
		{
			menumode.Visible = (General.Map != null);
		}
		
		#endregion

		#region ================== Help Menu
		
		// This sets up the help menu
		private void UpdateHelpMenu()
		{
			itemhelpeditmode.Visible = (General.Map != null); //mxd
			itemhelpeditmode.Enabled = (General.Map != null && General.Editing.Mode != null);
		}
		
		// About clicked
		private void itemhelpabout_Click(object sender, EventArgs e)
		{
			// Show about dialog
			var aboutform = new AboutForm();
			aboutform.ShowDialog(this);
		}

		// Reference Manual clicked
		private void itemhelprefmanual_Click(object sender, EventArgs e)
		{
			General.ShowHelp("introduction.html");
		}

		// About this Editing Mode
		private void itemhelpeditmode_Click(object sender, EventArgs e)
		{
			if((General.Map != null) && (General.Editing.Mode != null))
				General.Editing.Mode.OnHelp();
		}
		
		#endregion

		#region ================== Tools Menu

		// This sets up the tools menu
		private void UpdateToolsMenu()
		{
			//mxd. Enable/disable items
			bool enabled = (General.Map != null);
			itemreloadresources.Visible = enabled;
			seperatortoolsconfig.Visible = enabled;
			itemtestmap.Visible = enabled;
		}
		
		// Errors and Warnings
		[BeginAction("showerrors")]
		internal void ShowErrors()
		{
			ErrorsForm errform = new ErrorsForm();
			errform.ShowDialog(this);
			errform.Dispose();
		}
		
		// Game Configuration action
		[BeginAction("configuration")]
		internal void ShowConfiguration()
		{
			// Show configuration dialog
			ShowConfigurationPage(-1, null);
		}

		// This shows the configuration on a specific page
		internal void ShowConfigurationPage(int pageindex, ConfigurationInfo info)
		{
			// Show configuration dialog
			ConfigForm cfgform = new ConfigForm();
			if(info != null) cfgform.SelectConfiguration(info); //mxd
			if(pageindex > -1) cfgform.ShowTab(pageindex);
			if(cfgform.ShowDialog(this) == DialogResult.OK)
			{
				// Update stuff
				SetupInterface();
				UpdateInterface();
				General.Editing.UpdateCurrentEditModes();
				General.Plugins.ProgramReconfigure();
				
				// Reload resources if a map is open
				if((General.Map != null) && cfgform.ReloadResources) General.Actions.InvokeAction("dukebuilder_reloadresources");
				
				// Redraw display
				RedrawDisplay();
			}

			// Done
			cfgform.Dispose();
		}

		// Preferences action
		[BeginAction("preferences")]
		internal void ShowPreferences()
		{
			// Show preferences dialog
			PreferencesForm prefform = new PreferencesForm();
			if(prefform.ShowDialog(this) == DialogResult.OK)
			{
				// Update stuff
				SetupInterface();
				UpdateInterface();
				ApplyShortcutKeys();
				General.Colors.CreateCorrectionTable();
				General.Plugins.ProgramReconfigure();
				
				// Map opened?
				if(General.Map != null)
				{
					// Reload resources!
					General.Map.Graphics.SetupSettings();
					General.Map.UpdateConfiguration();
					if(prefform.ReloadResources) General.Actions.InvokeAction("dukebuilder_reloadresources");
				}
				
				// Redraw display
				RedrawDisplay();
			}

			// Done
			prefform.Dispose();
		}
		
		#endregion
		
		#region ================== Info Panels

		// This toggles the panel expanded / collapsed
		[BeginAction("toggleinfopanel")]
		internal void ToggleInfoPanel()
		{
			infopanel.Toggle();
			itemtoggleinfo.Checked = infopanel.IsExpanded;
			FocusDisplay();
		}

		// Show map element info
		public void ShowInfo(MapElement e) { infopanel.ShowInfo(e); }
		
		// This hides all info panels
		public void HideInfo() { infopanel.HideInfo(); }
		
		// This refreshes info
		public void RefreshInfo() { infopanel.RefreshInfo(); }

		//mxd. This updates statistics
		public void UpdateStatistics() { infopanel.UpdateStatistics(); }

		#endregion

		#region ================== Dialogs

		// This browses for a texture
		// Returns the new texture name or the same texture name when cancelled
		public int BrowseImage(IWin32Window owner, int initialvalue)
		{
			return ImageBrowserForm.Browse(owner, initialvalue);
		}
		
		// This browses the lindef types
		// Returns the new action or the same action when cancelled
		/*public int BrowseLinedefActions(IWin32Window owner, int initialvalue)
		{
			return ActionBrowserForm.BrowseAction(owner, initialvalue);
		}

		// This browses sector effects
		// Returns the new effect or the same effect when cancelled
		public int BrowseSectorEffect(IWin32Window owner, int initialvalue)
		{
			return EffectBrowserForm.BrowseEffect(owner, initialvalue);
		}*/

		// This browses thing types
		// Returns the new thing type or the same thing type when cancelled
		public int BrowseSpriteType(IWin32Window owner, int initialvalue)
		{
			return SpriteBrowserForm.BrowseSprite(owner, initialvalue);
		}

		// This shows the dialog to edit vertices
		public DialogResult ShowEditVertices(ICollection<Vertex> vertices)
		{
			// Show sector edit dialog
			EditVertexForm f = new EditVertexForm();
			f.Setup(vertices);
			DialogResult result = f.ShowDialog(this);
			f.Dispose();

			return result;
		}
		
		// This shows the dialog to edit lines
		public DialogResult ShowEditLines(ICollection<Linedef> lines)
		{
			// Show line edit dialog
			EditLineForm f = new EditLineForm();
			if(f.Setup(lines))
			{
				DialogResult result = f.ShowDialog(this);
				f.Dispose();
				return result;
			}
			DisplayStatus(StatusType.Warning, "Line has no editable properties");
			return DialogResult.Cancel; 
		}

		// This shows the dialog to edit lines
		public DialogResult ShowEditWalls(ICollection<Sidedef> walls)
		{
			// Show line edit dialog
			EditLineForm f = new EditLineForm();
			f.Setup(walls);
			DialogResult result = f.ShowDialog(this);
			f.Dispose();
			return result;
		}

		// This shows the dialog to edit sectors
		public DialogResult ShowEditSectors(ICollection<Sector> sectors)
		{
			// Show sector edit dialog
			EditSectorForm f = new EditSectorForm();
			f.Setup(sectors);
			DialogResult result = f.ShowDialog(this);
			f.Dispose();

			return result;
		}

		// This shows the dialog to edit things
		public DialogResult ShowEditSprites(ICollection<Thing> things)
		{
			// Show thing edit dialog
			EditSpriteForm f = new EditSpriteForm();
			f.Setup(things);
			DialogResult result = f.ShowDialog(this);
			f.Dispose();
			
			return result;
		}

		#endregion

		#region ================== Message Pump
		
		// This handles messages
		protected override void WndProc(ref Message m)
		{
			// Notify message?
			switch(m.Msg)
			{
				case (int)ThreadMessages.UpdateStatus:
					DisplayStatus(status);
					break;
					
				case (int)ThreadMessages.ImageDataLoaded:
					//string imagename = Marshal.PtrToStringAuto(m.WParam);
					//Marshal.FreeCoTaskMem(m.WParam);
					if((General.Map != null) && (General.Map.Data != null))
					{
						int tileindex = (int)m.WParam;
						ImageData img = General.Map.Data.GetImageData(tileindex);
						if(img != null) ImageDataLoaded(img);
					}
					break;

				case General.WM_SYSCOMMAND:
					// We don't want to open a menu when ALT is pressed
					if(m.WParam.ToInt32() != General.SC_KEYMENU)
					{
						base.WndProc(ref m);
					}
					break;
					
				default:
					// Let the base handle the message
					base.WndProc(ref m);
					break;
			}
		}
		
		#endregion
		
		#region ================== Processing
		
		// This is called from the background thread when images are loaded
		// but only when first loaded or when dimensions were changed
		private void ImageDataLoaded(ImageData img)
		{
			// Image is used in the map?
			if((img != null) && img.UsedInMap && !img.IsDisposed)
			{
				// Go for all setors
				bool updated = false;
				foreach(Sector s in General.Map.Map.Sectors)
				{
					// Update floor buffer if needed
					if(s.FloorTileIndex == img.TileIndex)
					{
						s.UpdateFloorSurface();
						updated = true;
					}
					
					// Update ceiling buffer if needed
					if(s.CeilingTileIndex == img.TileIndex)
					{
						s.UpdateCeilingSurface();
						updated = true;
					}
				}
				
				// If we made updates, redraw the screen
				if(updated) DelayedRedraw();
			}
		}

		public void EnableProcessing()
		{
			// Increase count
			processingcount++;

			// If not already enabled, enable processing now
			if(!processor.Enabled)
			{
				processor.Enabled = true;
				lastupdatetime = General.Clock.GetCurrentTime();
			}
		}

		public void DisableProcessing()
		{
			// Increase count
			processingcount--;
			if(processingcount < 0) processingcount = 0;
			
			// Turn off
			if(processor.Enabled && (processingcount == 0))
				processor.Enabled = false;
		}

		internal void StopProcessing()
		{
			// Turn off
			processingcount = 0;
			processor.Enabled = false;
		}
		
		// Processor event
		private void processor_Tick(object sender, EventArgs e)
		{
			double curtime = General.Clock.GetCurrentTime();
			double deltatime = curtime - lastupdatetime;
			lastupdatetime = curtime;
			
			// In exclusive mouse mode?
			if(mouseinput != null)
			{
				// Process mouse input
				Vector2D deltamouse = mouseinput.Process();
				if((General.Map != null) && (General.Editing.Mode != null))
				{
					General.Plugins.OnEditMouseInput(deltamouse);
					General.Editing.Mode.OnMouseInput(deltamouse);
				}
			}

			// Process signal
			if((General.Map != null) && (General.Editing.Mode != null))
				General.Editing.Mode.OnProcess(deltatime);
		}

		#endregion

		#region ================== Dockers
		
		// This adds a docker
		public void AddDocker(Docker d)
		{
			// Make sure the full name is set with the plugin name as prefix
			Plugin plugin = General.Plugins.FindPluginByAssembly(Assembly.GetCallingAssembly());
			d.MakeFullName(plugin.Name.ToLowerInvariant());
			
			dockerspanel.Add(d);
		}
		
		// This removes a docker
		public bool RemoveDocker(Docker d)
		{
			// Make sure the full name is set with the plugin name as prefix
			Plugin plugin = General.Plugins.FindPluginByAssembly(Assembly.GetCallingAssembly());
			d.MakeFullName(plugin.Name.ToLowerInvariant());
			
			// We must release all keys because the focus may be stolen when
			// this was the selected docker (the previous docker is automatically selected)
			ReleaseAllKeys();
			
			return dockerspanel.Remove(d);
		}
		
		// This selects a docker
		public bool SelectDocker(Docker d)
		{
			// Make sure the full name is set with the plugin name as prefix
			Plugin plugin = General.Plugins.FindPluginByAssembly(Assembly.GetCallingAssembly());
			d.MakeFullName(plugin.Name.ToLowerInvariant());
			
			// We must release all keys because the focus will be stolen
			ReleaseAllKeys();
			
			return dockerspanel.SelectDocker(d);
		}
		
		// This selects the previous selected docker
		public void SelectPreviousDocker()
		{
			// We must release all keys because the focus will be stolen
			ReleaseAllKeys();
			
			dockerspanel.SelectPrevious();
		}
		
		// Mouse enters dockers window
		private void dockerspanel_MouseContainerEnter(object sender, EventArgs e)
		{
			if(General.Settings.CollapseDockers)
				dockerscollapser.Start();
			
			dockerspanel.Expand();
		}
		
		// Automatic collapsing
		private void dockerscollapser_Tick(object sender, EventArgs e)
		{
			if(General.Settings.CollapseDockers)
			{
				if(!dockerspanel.IsFocused)
				{
					Point p = this.PointToClient(Cursor.Position);
					Rectangle r = new Rectangle(dockerspanel.Location, dockerspanel.Size);
					if(!r.IntersectsWith(new Rectangle(p, Size.Empty)))
					{
						dockerspanel.Collapse();
						dockerscollapser.Stop();
					}
				}
			}
			else
			{
				dockerscollapser.Stop();
			}
		}
		
		// User resizes the docker
		private void dockerspanel_UserResize(object sender, EventArgs e)
		{
			General.Settings.DockersWidth = dockerspanel.Width;

			if(!General.Settings.CollapseDockers)
			{
				dockersspace.Width = dockerspanel.Width;
				dockerspanel.Left = dockersspace.Left;
			}
		}
		
		#endregion

		#region ================== Graphics (mxd)

		public SizeF MeasureString(string text, Font font)
		{
			return graphics.MeasureString(text, font);
		}

		public SizeF MeasureString(string text, Font font, int width, StringFormat format)
		{
			return graphics.MeasureString(text, font, width, format);
		}

		#endregion
	}
}