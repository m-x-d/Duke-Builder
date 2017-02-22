
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
using System.IO;
using System.Windows.Forms;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Config.ImageSets;
using mxd.DukeBuilder.Editing;

#endregion

namespace mxd.DukeBuilder.Windows
{
	internal partial class ConfigForm : DelayedForm
	{
		#region ================== Variables

		private GameConfiguration gameconfig;
		private ConfigurationInfo configinfo;
		private List<DefinedImageSet> copiedsets;
		private bool preventchanges;
		private bool reloadresources;

		#endregion

		#region ================== Properties

		public bool ReloadResources { get { return reloadresources; } }

		#endregion

		#region ================== Constructor

		public ConfigForm()
		{
			// Initialize
			InitializeComponent();
			
			// Make list column header full width
			columnname.Width = listconfigs.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - 2;
			
			// Fill list of configurations
			foreach(ConfigurationInfo ci in General.Configs)
			{
				// Add a copy
				ListViewItem lvi = listconfigs.Items.Add(ci.Name);
				lvi.Tag = ci.Clone();

				// This is the current configuration?
				if((General.Map != null) && (General.Map.ConfigSettings.Filename == ci.Filename))
					lvi.Selected = true;
			}
			
			// No skill
			//skill.Text = "0";
			
			// Fill list of editing modes
			foreach(EditModeInfo emi in General.Editing.ModesInfo)
			{
				// Is this mode selectable by the user?
				if(emi.IsOptional)
				{
					ListViewItem lvi = listmodes.Items.Add(emi.Attributes.DisplayName);
					lvi.Tag = emi;
					lvi.SubItems.Add(emi.Plugin.Plug.Name);
				}
			}
		}

		#endregion

		// This shows a specific page
		public void ShowTab(int index)
		{
			tabs.SelectedIndex = index;
		}

		//mxd. This selects a specific Game Configuration
		public void SelectConfiguration(ConfigurationInfo info)
		{
			foreach(ListViewItem item in listconfigs.Items)
			{
				var curinfo = item.Tag as ConfigurationInfo;
				if(curinfo != null && curinfo.Name == info.Name)
				{
					item.Selected = true;
					break;
				}
			}
		}

		// Configuration item selected
		private void listconfigs_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Item selected?
			if(listconfigs.SelectedItems.Count > 0)
			{
				// Enable panels
				tabs.Enabled = true;
				preventchanges = true;
				
				// Get config info of selected item
				configinfo = listconfigs.SelectedItems[0].Tag as ConfigurationInfo;
				
				// Load the game configuration
				gameconfig = new GameConfiguration(General.LoadGameConfiguration(configinfo.Filename));

				// Set defaults
				configinfo.ApplyDefaults(gameconfig);
				
				// Fill skills list
				//skill.ClearInfo();
				//skill.AddInfo(gameconfig.Skills.ToArray());
				skill.Items.Clear();
				skill.Items.AddRange(gameconfig.Skills.ToArray());
				
				// Set test application and parameters
				if(!configinfo.CustomParameters) configinfo.TestParameters = gameconfig.TestParameters;

				testapplication.Text = configinfo.TestProgram;
				testparameters.Text = configinfo.TestParameters;
				int skilllevel = configinfo.TestSkill;
				//skill.Value = skilllevel - 1;
				//skill.Value = skilllevel;
				skill.SelectedIndex = skilllevel;
				customparameters.Checked = configinfo.CustomParameters;
				
				// Fill texture sets list
				listtextures.Items.Clear();
				foreach(DefinedImageSet ts in configinfo.TextureSets)
				{
					ListViewItem item = listtextures.Items.Add(ts.Name);
					item.Tag = ts;
					item.ImageIndex = 0;
				}
				listtextures.Sort();
				
				// Go for all the editing modes in the list
				foreach(ListViewItem lvi in listmodes.Items)
				{
					EditModeInfo emi = (lvi.Tag as EditModeInfo);
					lvi.Checked = (configinfo.EditModes.ContainsKey(emi.Type.FullName) && configinfo.EditModes[emi.Type.FullName]);
				}
				
				// Fill start modes
				RefillStartModes();

				// Done
				preventchanges = false;
			}
		}
		
		// Key released
		private void listconfigs_KeyUp(object sender, KeyEventArgs e)
		{
			// Nothing selected?
			if(listconfigs.SelectedItems.Count == 0)
			{
				// Disable panels
				gameconfig = null;
				configinfo = null;
				testapplication.Text = "";
				testparameters.Text = "";
				//skill.Value = 0;
				//skill.ClearInfo();
				skill.Items.Clear();
				customparameters.Checked = false;
				tabs.Enabled = false;
				listtextures.Items.Clear();
			}
		}
		
		// Mouse released
		private void listconfigs_MouseUp(object sender, MouseEventArgs e)
		{
			listconfigs_KeyUp(sender, new KeyEventArgs(Keys.None));
		}
		
		// Test application changed
		private void testapplication_TextChanged(object sender, EventArgs e)
		{
			// Leave when no configuration selected
			if(configinfo == null) return;

			// Apply to selected configuration
			configinfo.TestProgram = testapplication.Text;
		}

		// Test parameters changed
		private void testparameters_TextChanged(object sender, EventArgs e)
		{
			// Leave when no configuration selected
			if(configinfo == null) return;

			// Apply to selected configuration
			configinfo = listconfigs.SelectedItems[0].Tag as ConfigurationInfo;
			configinfo.TestParameters = testparameters.Text;

			// Show example result
			CreateParametersExample();
		}
		
		// This creates a new parameters example
		private void CreateParametersExample()
		{
			// Map loaded?
			if(General.Map != null && configinfo != null)
			{
				// Make converted parameters
				testresult.Text = General.Map.Launcher.ConvertParameters(testparameters.Text, configinfo.TestSkill);
			}
		}
		
		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			// Apply configuration items
			foreach(ListViewItem lvi in listconfigs.Items)
			{
				// Get configuration item
				ConfigurationInfo ci = lvi.Tag as ConfigurationInfo;

				// Find same configuration info in originals
				foreach(ConfigurationInfo oci in General.Configs)
				{
					// Apply settings when they match
					if(string.Compare(ci.Filename, oci.Filename) == 0) oci.Apply(ci);
				}
			}

			// Close
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			// Close
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// Browse test program
		private void browsetestprogram_Click(object sender, EventArgs e)
		{
			// Set initial directory
			if(testapplication.Text.Length > 0)
			{
				try { testprogramdialog.InitialDirectory = Path.GetDirectoryName(testapplication.Text); }
				catch(Exception) { }
			}
			
			// Browse for test program
			if(testprogramdialog.ShowDialog() == DialogResult.OK)
			{
				// Apply
				testapplication.Text = testprogramdialog.FileName;
			}
		}

		// Customize parameters (un)checked
		private void customparameters_CheckedChanged(object sender, EventArgs e)
		{
			// Leave when no configuration selected
			if(configinfo == null) return;

			// Apply to selected configuration
			configinfo.CustomParameters = customparameters.Checked;

			// Update interface
			labelparameters.Visible = customparameters.Checked;
			testparameters.Visible = customparameters.Checked;

			// Check if a map is loaded
			if(General.Map != null)
			{
				// Show parameters example result
				labelresult.Visible = customparameters.Checked;
				testresult.Visible = customparameters.Checked;
				noresultlabel.Visible = false;
			}
			else
			{
				// Cannot show parameters example result
				labelresult.Visible = false;
				testresult.Visible = false;
				noresultlabel.Visible = customparameters.Checked;
			}
		}
		
		// Skill changes
		private void skill_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Leave when no configuration selected
			if(configinfo == null) return;
			
			// Apply to selected configuration
			configinfo.TestSkill = ((SkillInfo)skill.SelectedItem).Index;
			
			CreateParametersExample();
		}

		// Make new texture set
		private void addtextureset_Click(object sender, EventArgs e)
		{
			DefinedImageSet s = new DefinedImageSet("New Image Set");
			ImageSetForm form = new ImageSetForm();
			form.Setup(s);
			if(form.ShowDialog(this) == DialogResult.OK)
			{
				// Add to texture sets
				configinfo.TextureSets.Add(s);
				ListViewItem item = listtextures.Items.Add(s.Name);
				item.Tag = s;
				item.ImageIndex = 0;
				listtextures.Sort();
				reloadresources = true;
			}
		}

		// Edit texture set
		private void edittextureset_Click(object sender, EventArgs e)
		{
			// Texture Set selected?
			if(listtextures.SelectedItems.Count > 0)
			{
				DefinedImageSet s = (listtextures.SelectedItems[0].Tag as DefinedImageSet);
				ImageSetForm form = new ImageSetForm();
				form.Setup(s);
				form.ShowDialog(this);
				listtextures.SelectedItems[0].Text = s.Name;
				listtextures.Sort();
				reloadresources = true;
			}
		}
		
		// Remove texture set
		private void removetextureset_Click(object sender, EventArgs e)
		{
			// Texture Set selected?
			while(listtextures.SelectedItems.Count > 0)
			{
				// Remove from config info and list
				DefinedImageSet s = (listtextures.SelectedItems[0].Tag as DefinedImageSet);
				configinfo.TextureSets.Remove(s);
				listtextures.SelectedItems[0].Remove();
				reloadresources = true;
			}
		}
		
		// Texture Set selected/deselected
		private void listtextures_SelectedIndexChanged(object sender, EventArgs e)
		{
			edittextureset.Enabled = (listtextures.SelectedItems.Count > 0);
			removetextureset.Enabled = (listtextures.SelectedItems.Count > 0);
			copytexturesets.Enabled = (listtextures.SelectedItems.Count > 0);
		}
		
		// Doubleclicking a texture set
		private void listtextures_DoubleClick(object sender, EventArgs e)
		{
			edittextureset_Click(sender, e);
		}
		
		// Copy selected texture sets
		private void copytexturesets_Click(object sender, EventArgs e)
		{
			// Make copies
			copiedsets = new List<DefinedImageSet>();
			foreach(ListViewItem item in listtextures.SelectedItems)
			{
				DefinedImageSet s = (item.Tag as DefinedImageSet);
				copiedsets.Add(s.Copy());
			}
			
			// Enable button
			pastetexturesets.Enabled = true;
		}
		
		// Paste copied texture sets
		private void pastetexturesets_Click(object sender, EventArgs e)
		{
			if(copiedsets != null)
			{
				// Add copies
				foreach(DefinedImageSet ts in copiedsets)
				{
					DefinedImageSet s = ts.Copy();
					ListViewItem item = listtextures.Items.Add(s.Name);
					item.Tag = s;
					item.ImageIndex = 0;
					configinfo.TextureSets.Add(s);
				}
				listtextures.Sort();
				reloadresources = true;
			}
		}
		
		// This will add the default sets from game configuration
		private void restoretexturesets_Click(object sender, EventArgs e)
		{
			// Ask nicely first
			if(MessageBox.Show(this, "This will add the default Image Sets from the Game Configuration. Do you want to continue?",
				"Add Default Sets", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				// Add copies
				foreach(DefinedImageSet ts in gameconfig.ImageSets)
				{
					DefinedImageSet s = ts.Copy();
					ListViewItem item = listtextures.Items.Add(s.Name);
					item.Tag = s;
					item.ImageIndex = 0;
					configinfo.TextureSets.Add(s);
				}
				listtextures.Sort();
				reloadresources = true;
			}
		}
		
		// This is called when an editing mode item is checked or unchecked
		private void listmodes_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			// Leave when no configuration selected
			if(configinfo == null) return;
			
			// Apply changes
			EditModeInfo emi = (e.Item.Tag as EditModeInfo);
			bool currentstate = (configinfo.EditModes.ContainsKey(emi.Type.FullName) && configinfo.EditModes[emi.Type.FullName]);
			if(e.Item.Checked && !currentstate)
			{
				// Add
				configinfo.EditModes[emi.Type.FullName] = true;
			}
			else if(!e.Item.Checked && currentstate)
			{
				// Remove
				configinfo.EditModes[emi.Type.FullName] = false;
			}
			
			preventchanges = true;
			RefillStartModes();
			preventchanges = false;
		}

		// Help requested
		private void ConfigForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			General.ShowHelp("w_gameconfigurations.html");
			hlpevent.Handled = true;
		}
		
		// This refills the start mode cobobox
		private void RefillStartModes()
		{
			// Refill the startmode combobox
			startmode.Items.Clear();
			foreach(ListViewItem item in listmodes.Items)
			{
				if(item.Checked)
				{
					EditModeInfo emi = (item.Tag as EditModeInfo);
					if(emi.Attributes.SafeStartMode)
					{
						int newindex = startmode.Items.Add(emi);
						if(emi.Type.Name == configinfo.StartMode) startmode.SelectedIndex = newindex;
					}
				}
			}
			
			// Select the first in the combobox if none are selected
			if((startmode.SelectedItem == null) && (startmode.Items.Count > 0))
			{
				startmode.SelectedIndex = 0;
				EditModeInfo emi = (startmode.SelectedItem as EditModeInfo);
				configinfo.StartMode = emi.Type.Name;
			}
		}
		
		// Start mode combobox changed
		private void startmode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(preventchanges || (configinfo == null)) return;
			
			// Apply start mode
			if(startmode.SelectedItem != null)
			{
				EditModeInfo emi = (startmode.SelectedItem as EditModeInfo);
				configinfo.StartMode = emi.Type.Name;
			}
		}
	}
}
