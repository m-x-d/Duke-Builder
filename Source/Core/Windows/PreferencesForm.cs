
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
using System.Windows.Forms;
using mxd.DukeBuilder.Actions;
using mxd.DukeBuilder.Data;
using Action = mxd.DukeBuilder.Actions.Action;

#endregion

namespace mxd.DukeBuilder.Windows
{
	internal partial class PreferencesForm : DelayedForm
	{
		#region ================== Variables

		private PreferencesController controller;
		private bool allowapplycontrol;
		private bool disregardshift;
		private bool disregardcontrol;

		private bool reloadresources;
		
		#endregion

		#region ================== Properties

		public bool ReloadResources { get { return reloadresources; } }

		#endregion

		#region ================== Constructor

		// Constructor
		public PreferencesForm()
		{
			// Initialize
			InitializeComponent();
			
			// Interface
			imagebrightness.Value = General.Settings.ImageBrightness;
			doublesidedalpha.Value = (int)((1.0f - General.Settings.DoubleSidedAlpha) * 10.0f);
			defaultviewmode.SelectedIndex = General.Settings.DefaultViewMode;
			fieldofview.Value = General.Settings.VisualFOV / 10;
			mousespeed.Value = General.Settings.MouseSpeed / 100;
			movespeed.Value = General.Settings.MoveSpeed / 1600;
			viewdistance.Value = General.Clamp((int)(General.Settings.ViewDistance / 4000.0f), viewdistance.Minimum, viewdistance.Maximum);
			invertyaxis.Checked = General.Settings.InvertYAxis;
			autoscrollspeed.Value = General.Settings.AutoScrollSpeed;
			zoomfactor.Value = General.Settings.ZoomFactor;
			animatevisualselection.Checked = General.Settings.AnimateVisualSelection;
			dockersposition.SelectedIndex = General.Settings.DockersPosition;
			collapsedockers.Checked = General.Settings.CollapseDockers;
			toolbar_file.Checked = General.Settings.ToolbarFile;
			toolbar_undo.Checked = General.Settings.ToolbarUndo;
			toolbar_copy.Checked = General.Settings.ToolbarCopy;
			toolbar_filter.Checked = General.Settings.ToolbarFilter;
			toolbar_viewmodes.Checked = General.Settings.ToolbarViewModes;
			toolbar_geometry.Checked = General.Settings.ToolbarGeometry;
			toolbar_testing.Checked = General.Settings.ToolbarTesting;
			showtexturesizes.Checked = General.Settings.ShowImageSizes;
			
			// Fill actions list with categories
			foreach(KeyValuePair<string, string> c in General.Actions.Categories)
				listactions.Groups.Add(c.Key, c.Value);
			
			// Fill list of actions
			Action[] actions = General.Actions.GetAllActions();
			foreach(Action a in actions)
			{
				// Create item
				ListViewItem item = listactions.Items.Add(a.Name, a.Title, 0);
				item.SubItems.Add(Action.GetShortcutKeyDesc(a.ShortcutKey));
				item.SubItems[1].Tag = a.ShortcutKey;

				// Put in category, if the category exists
				if(General.Actions.Categories.ContainsKey(a.Category))
					item.Group = listactions.Groups[a.Category];
			}

			// Set the colors
			// TODO: Make this automated by using the collection
			colorbackcolor.Color = General.Colors.Background;
			colorvertices.Color = General.Colors.Vertices;
			colorlines.Color = General.Colors.LinesSingleSided;
			colorlinesdoublesided.Color = General.Colors.LinesDoubleSided;
			colorlinesinvalid.Color = General.Colors.LinesInvalid;
			colorhighlight.Color = General.Colors.Highlight;
			colorselection.Color = General.Colors.Selection;
			colorindication.Color = General.Colors.Indication;
			colorgrid.Color = General.Colors.Grid;
			colorgrid1024.Color = General.Colors.Grid1024;
			classicbilinear.Checked = General.Settings.ClassicBilinear;
			visualbilinear.Checked = General.Settings.VisualBilinear;
			
			// Paste options
			pasteoptions.Setup(General.Settings.PasteOptions.Copy());

			// Allow plugins to add tabs
			this.SuspendLayout();
			controller = new PreferencesController(this);
			controller.AllowAddTab = true;
			General.Plugins.OnShowPreferences(controller);
			controller.AllowAddTab = false;
			this.ResumeLayout(true);
			
			// Done
			allowapplycontrol = true;
		}

		#endregion

		#region ================== OK / Cancel

		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			// Let the plugins know
			controller.RaiseAccept();
			
			// Check if we need to reload the resources
			reloadresources |= (General.Settings.ImageBrightness != imagebrightness.Value);

			// Apply interface
			General.Settings.ImageBrightness = imagebrightness.Value;
			General.Settings.DoubleSidedAlpha = 1.0f - (doublesidedalpha.Value * 0.1f);
			General.Settings.DefaultViewMode = defaultviewmode.SelectedIndex;
			General.Settings.VisualFOV = fieldofview.Value * 10;
			General.Settings.MouseSpeed = mousespeed.Value * 100;
			General.Settings.MoveSpeed = movespeed.Value * 1600;
			General.Settings.ViewDistance = viewdistance.Value * 4000.0f;
			General.Settings.InvertYAxis = invertyaxis.Checked;
			General.Settings.AutoScrollSpeed = autoscrollspeed.Value;
			General.Settings.ZoomFactor = zoomfactor.Value;
			General.Settings.AnimateVisualSelection = animatevisualselection.Checked;
			General.Settings.DockersPosition = dockersposition.SelectedIndex;
			General.Settings.CollapseDockers = collapsedockers.Checked;
			General.Settings.ToolbarFile = toolbar_file.Checked;
			General.Settings.ToolbarUndo = toolbar_undo.Checked;
			General.Settings.ToolbarCopy = toolbar_copy.Checked;
			General.Settings.ToolbarFilter = toolbar_filter.Checked;
			General.Settings.ToolbarViewModes = toolbar_viewmodes.Checked;
			General.Settings.ToolbarGeometry = toolbar_geometry.Checked;
			General.Settings.ToolbarTesting = toolbar_testing.Checked;
			General.Settings.ShowImageSizes = showtexturesizes.Checked;
			
			// Apply control keys to actions
			foreach(ListViewItem item in listactions.Items)
				General.Actions[item.Name].SetShortcutKey((int)item.SubItems[1].Tag);

			// Apply the colors
			// TODO: Make this automated by using the collection
			General.Colors.Background = colorbackcolor.Color;
			General.Colors.Vertices = colorvertices.Color;
			General.Colors.LinesSingleSided = colorlines.Color;
			General.Colors.LinesDoubleSided = colorlinesdoublesided.Color;
			General.Colors.LinesInvalid = colorlinesinvalid.Color;
			General.Colors.Highlight = colorhighlight.Color;
			General.Colors.Selection = colorselection.Color;
			General.Colors.Indication = colorindication.Color;
			General.Colors.Grid = colorgrid.Color;
			General.Colors.Grid1024 = colorgrid1024.Color;
			General.Colors.CreateAssistColors();
			General.Settings.ClassicBilinear = classicbilinear.Checked;
			General.Settings.VisualBilinear = visualbilinear.Checked;
			
			// Paste options
			General.Settings.PasteOptions = pasteoptions.GetOptions();
			
			// Let the plugins know we're closing
			General.Plugins.OnClosePreferences(controller);
			
			// Close
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			// Let the plugins know
			controller.RaiseCancel();

			// Let the plugins know we're closing
			General.Plugins.OnClosePreferences(controller);

			// Close
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion

		#region ================== Tabs

		// This adds a tab page
		public void AddTabPage(TabPage tab)
		{
			tabs.TabPages.Add(tab);
		}

		// Tab changes
		private void tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Enable/disable stuff with tabs
			if(tabs.SelectedTab != tabkeys)
			{
				this.AcceptButton = apply;
				this.CancelButton = cancel;
				apply.TabStop = true;
				cancel.TabStop = true;
				tabs.TabStop = true;
			}
			else
			{
				this.AcceptButton = null;
				this.CancelButton = null;
				apply.TabStop = false;
				cancel.TabStop = false;
				tabs.TabStop = false;
			}
		}

		#endregion

		#region ================== Interface Panel
		
		private void fieldofview_ValueChanged(object sender, EventArgs e)
		{
			int value = fieldofview.Value * 10;
			fieldofviewlabel.Text = value.ToString() + (char)176;
		}

		private void mousespeed_ValueChanged(object sender, EventArgs e)
		{
			int value = mousespeed.Value * 100;
			mousespeedlabel.Text = value.ToString();
		}

		private void movespeed_ValueChanged(object sender, EventArgs e)
		{
			int value = movespeed.Value * 1600;
			movespeedlabel.Text = value.ToString();
		}

		private void viewdistance_ValueChanged(object sender, EventArgs e)
		{
			int value = viewdistance.Value * 4000;
			viewdistancelabel.Text = value + " mu";
		}

		private void autoscrollspeed_ValueChanged(object sender, EventArgs e)
		{
			if(autoscrollspeed.Value == 0)
				autoscrollspeedlabel.Text = "Off";
			else
				autoscrollspeedlabel.Text = autoscrollspeed.Value + "x";
		}

		private void zoomfactor_ValueChanged(object sender, EventArgs e)
		{
			zoomfactorlabel.Text = (zoomfactor.Value * 10) + "%";
		}

		#endregion
		
		#region ================== Controls Panel
		
		// This updates the used keys info
		private void UpdateKeyUsedActions()
		{
			List<string> usedactions = new List<string>();
			
			// Anything selected?
			if(listactions.SelectedItems.Count > 0)
			{
				// Get info
				int thiskey = (int)listactions.SelectedItems[0].SubItems[1].Tag;
				if(thiskey != 0)
				{
					// Find actions with same key
					foreach(ListViewItem item in listactions.Items)
					{
						// Don't count the selected action
						if(item != listactions.SelectedItems[0])
						{
							Actions.Action a = General.Actions[item.Name];
							int akey = (int)item.SubItems[1].Tag;

							// Check if the key combination matches
							if((thiskey & a.ShortcutMask) == (akey & a.ShortcutMask))
								usedactions.Add(a.Title + "  (" + General.Actions.Categories[a.Category] + ")");
						}
					}
				}
			}
			
			// Update info
			if(usedactions.Count == 0)
			{
				keyusedlabel.Visible = false;
				keyusedlist.Visible = false;
				keyusedlist.Items.Clear();
			}
			else
			{
				keyusedlist.Items.Clear();
				foreach(string a in usedactions) keyusedlist.Items.Add(a);
				keyusedlabel.Visible = true;
				keyusedlist.Visible = true;
			}
		}
		
		// This fills the list of available controls for the specified action
		private void FillControlsList(Actions.Action a)
		{
			actioncontrol.Items.Clear();
			
			// Fill combobox with special controls
			if(a.AllowMouse)
			{
				actioncontrol.Items.Add(new KeyControl(Keys.LButton, "LButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.MButton, "MButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.RButton, "RButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.XButton1, "XButton1"));
				actioncontrol.Items.Add(new KeyControl(Keys.XButton2, "XButton2"));
			}
			if(a.AllowScroll)
			{
				actioncontrol.Items.Add(new KeyControl(SpecialKeys.MScrollUp, "ScrollUp"));
				actioncontrol.Items.Add(new KeyControl(SpecialKeys.MScrollDown, "ScrollDown"));
			}
			if(a.AllowMouse && !a.DisregardShift)
			{
				actioncontrol.Items.Add(new KeyControl(Keys.LButton | Keys.Shift, "Shift+LButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.MButton | Keys.Shift, "Shift+MButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.RButton | Keys.Shift, "Shift+RButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.XButton1 | Keys.Shift, "Shift+XButton1"));
				actioncontrol.Items.Add(new KeyControl(Keys.XButton2 | Keys.Shift, "Shift+XButton2"));
			}
			if(a.AllowScroll && !a.DisregardShift)
			{
				actioncontrol.Items.Add(new KeyControl((int)SpecialKeys.MScrollUp | (int)Keys.Shift, "Shift+ScrollUp"));
				actioncontrol.Items.Add(new KeyControl((int)SpecialKeys.MScrollDown | (int)Keys.Shift, "Shift+ScrollDown"));
			}
			if(a.AllowMouse && !a.DisregardControl)
			{
				actioncontrol.Items.Add(new KeyControl(Keys.LButton | Keys.Control, "Ctrl+LButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.MButton | Keys.Control, "Ctrl+MButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.RButton | Keys.Control, "Ctrl+RButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.XButton1 | Keys.Control, "Ctrl+XButton1"));
				actioncontrol.Items.Add(new KeyControl(Keys.XButton2 | Keys.Control, "Ctrl+XButton2"));
			}
			if(a.AllowScroll && !a.DisregardControl)
			{
				actioncontrol.Items.Add(new KeyControl((int)SpecialKeys.MScrollUp | (int)Keys.Control, "Ctrl+ScrollUp"));
				actioncontrol.Items.Add(new KeyControl((int)SpecialKeys.MScrollDown | (int)Keys.Control, "Ctrl+ScrollDown"));
			}
			if(a.AllowMouse && !a.DisregardShift && !a.DisregardControl)
			{
				actioncontrol.Items.Add(new KeyControl(Keys.LButton | Keys.Shift | Keys.Control, "Ctrl+Shift+LButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.MButton | Keys.Shift | Keys.Control, "Ctrl+Shift+MButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.RButton | Keys.Shift | Keys.Control, "Ctrl+Shift+RButton"));
				actioncontrol.Items.Add(new KeyControl(Keys.XButton1 | Keys.Shift | Keys.Control, "Ctrl+Shift+XButton1"));
				actioncontrol.Items.Add(new KeyControl(Keys.XButton2 | Keys.Shift | Keys.Control, "Ctrl+Shift+XButton2"));
			}
			if(a.AllowScroll && !a.DisregardShift && !a.DisregardControl)
			{
				actioncontrol.Items.Add(new KeyControl((int)SpecialKeys.MScrollUp | (int)Keys.Shift | (int)Keys.Control, "Ctrl+Shift+ScrollUp"));
				actioncontrol.Items.Add(new KeyControl((int)SpecialKeys.MScrollDown | (int)Keys.Shift | (int)Keys.Control, "Ctrl+Shift+ScrollDown"));
			}
		}
		
		// Item selected
		private void listactions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			Actions.Action action;
			KeyControl keycontrol;
			string disregardkeys = "";
			int key;

			// Anything selected?
			if(listactions.SelectedItems.Count > 0)
			{
				// Begin updating
				allowapplycontrol = false;

				// Get the selected action
				action = General.Actions[listactions.SelectedItems[0].Name];
				key = (int)listactions.SelectedItems[0].SubItems[1].Tag;
				disregardshift = action.DisregardShift;
				disregardcontrol = action.DisregardControl;
				
				// Enable panel
				actioncontrolpanel.Enabled = true;
				actiontitle.Text = action.Title;
				actiondescription.Text = action.Description;
				actioncontrol.SelectedIndex = -1;
				actionkey.Text = "";
				
				if(disregardshift && disregardcontrol)
					disregardkeys = "Shift and Control";
				else if(disregardshift)
					disregardkeys = "Shift";
				else if(disregardcontrol)
					disregardkeys = "Control";

				disregardshiftlabel.Text = disregardshiftlabel.Tag.ToString().Replace("%s", disregardkeys);
				disregardshiftlabel.Visible = disregardshift | disregardcontrol;
				
				// Fill special controls list
				FillControlsList(action);
				
				// See if the key is in the combobox
				for(int i = 0; i < actioncontrol.Items.Count; i++)
				{
					// Select it when the key is found here
					keycontrol = (KeyControl)actioncontrol.Items[i];
					if(keycontrol.Key == key) actioncontrol.SelectedIndex = i;
				}

				// Otherwise display the key in the textbox
				if(actioncontrol.SelectedIndex == -1)
					actionkey.Text = Actions.Action.GetShortcutKeyDesc(key);
				
				// Show actions with same key
				UpdateKeyUsedActions();
				
				// Focus to the input box
				actionkey.Focus();

				// Done
				allowapplycontrol = true;
			}
		}

		// Key released
		private void listactions_KeyUp(object sender, KeyEventArgs e)
		{
			// Nothing selected?
			if(listactions.SelectedItems.Count == 0)
			{
				// Disable panel
				actioncontrolpanel.Enabled = false;
				actiontitle.Text = "(select an action from the list)";
				actiondescription.Text = "";
				actionkey.Text = "";
				actioncontrol.SelectedIndex = -1;
				disregardshiftlabel.Visible = false;
			}
			
			// Show actions with same key
			UpdateKeyUsedActions();
		}

		// Mouse released
		private void listactions_MouseUp(object sender, MouseEventArgs e)
		{
			listactions_KeyUp(sender, new KeyEventArgs(Keys.None));

			// Focus to the input box
			actionkey.Focus();
		}

		// Key combination pressed
		private void actionkey_KeyDown(object sender, KeyEventArgs e)
		{
			int key = (int)e.KeyData;
			e.SuppressKeyPress = true;

			// Leave when not allowed to update
			if(!allowapplycontrol) return;

			// Anything selected?
			if(listactions.SelectedItems.Count > 0)
			{
				// Begin updating
				allowapplycontrol = false;
				
				// Remove modifier keys from the key if needed
				if(disregardshift) key &= ~(int)Keys.Shift;
				if(disregardcontrol) key &= ~(int)Keys.Control;
				
				// Deselect anything from the combobox
				actioncontrol.SelectedIndex = -1;
				
				// Apply the key combination
				listactions.SelectedItems[0].SubItems[1].Text = Actions.Action.GetShortcutKeyDesc(key);
				listactions.SelectedItems[0].SubItems[1].Tag = key;
				actionkey.Text = Actions.Action.GetShortcutKeyDesc(key);
				
				// Show actions with same key
				UpdateKeyUsedActions();
				
				// Done
				allowapplycontrol = true;
			}
		}

		// Key combination displayed
		private void actionkey_TextChanged(object sender, EventArgs e)
		{
			// Cursor to the end
			actionkey.SelectionStart = actionkey.Text.Length;
			actionkey.SelectionLength = 0;
		}

		// Special key selected
		private void actioncontrol_SelectedIndexChanged(object sender, EventArgs e)
		{
			KeyControl key;

			// Leave when not allowed to update
			if(!allowapplycontrol) return;

			// Anything selected?
			if((actioncontrol.SelectedIndex > -1) && (listactions.SelectedItems.Count > 0))
			{
				// Begin updating
				allowapplycontrol = false;

				// Remove text from textbox
				actionkey.Text = "";

				// Get the key control
				key = (KeyControl)actioncontrol.SelectedItem;

				// Apply the key combination
				listactions.SelectedItems[0].SubItems[1].Text = Actions.Action.GetShortcutKeyDesc(key.Key);
				listactions.SelectedItems[0].SubItems[1].Tag = key.Key;
				
				// Show actions with same key
				UpdateKeyUsedActions();
				
				// Focus to the input box
				actionkey.Focus();

				// Done
				allowapplycontrol = true;
			}
		}

		// Clear clicked
		private void actioncontrolclear_Click(object sender, EventArgs e)
		{
			// Begin updating
			allowapplycontrol = false;

			// Clear textbox and combobox
			actionkey.Text = "";
			actioncontrol.SelectedIndex = -1;

			// Apply the key combination
			listactions.SelectedItems[0].SubItems[1].Text = "";
			listactions.SelectedItems[0].SubItems[1].Tag = 0;
			
			// Show actions with same key
			UpdateKeyUsedActions();
			
			// Focus to the input box
			actionkey.Focus();

			// Done
			allowapplycontrol = true;
		}

		#endregion

		#region ================== Colors Panel

		private void imagebrightness_ValueChanged(object sender, EventArgs e)
		{
			imagebrightnesslabel.Text = "+ " + imagebrightness.Value.ToString() + " y";
		}

		private void doublesidedalpha_ValueChanged(object sender, EventArgs e)
		{
			int percent = doublesidedalpha.Value * 10;
			doublesidedalphalabel.Text = percent.ToString() + "%";
		}

		#endregion

		// Help
		private void PreferencesForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			if(!actionkey.Focused)
			{
				General.ShowHelp("w_preferences.html");
				hlpevent.Handled = true;
			}
		}
		
		/*
		// This writes all action help files using a template and some basic info from the actions.
		// Also writes actioncontents.txt with all files to be inserted into Contents.hhc.
		// Only used during development. Actual button to call this has been removed.
		private void gobutton_Click(object sender, EventArgs e)
		{
			string template = File.ReadAllText(Path.Combine(General.AppPath, "..\\Help\\a_template.html"));
			StringBuilder contents = new StringBuilder("\t<UL>\r\n");
			string filename;
			
			// Go for all actions
			Action[] actions = General.Actions.GetAllActions();
			foreach(Action a in actions)
			{
				StringBuilder actionhtml = new StringBuilder(template);
				actionhtml.Replace("ACTIONTITLE", a.Title);
				actionhtml.Replace("ACTIONDESCRIPTION", a.Description);
				actionhtml.Replace("ACTIONCATEGORY", General.Actions.Categories[a.Category]);
				filename = Path.Combine(General.AppPath, "..\\Help\\a_" + a.Name + ".html");
				File.WriteAllText(filename, actionhtml.ToString());
				
				contents.Append("\t\t<LI> <OBJECT type=\"text/sitemap\">\r\n");
				contents.Append("\t\t\t<param name=\"Name\" value=\"" + a.Title + "\">\r\n");
				contents.Append("\t\t\t<param name=\"Local\" value=\"a_" + a.Name + ".html\">\r\n");
				contents.Append("\t\t\t</OBJECT>\r\n");
			}
			
			contents.Append("\t</UL>\r\n");
			filename = Path.Combine(General.AppPath, "..\\Help\\actioncontents.txt");
			File.WriteAllText(filename, contents.ToString());
		}
		*/
	}
}