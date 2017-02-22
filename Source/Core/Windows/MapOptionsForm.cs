
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
using System.IO;
using System.Windows.Forms;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Windows
{
	internal partial class MapOptionsForm : DelayedForm
	{
		// Variables
		private MapOptions options;
		private bool newmap;
		
		// Properties
		public MapOptions Options { get { return options; } }
		public bool IsForNewMap { get { return newmap; } set { newmap = value; } }
		
		// Constructor
		public MapOptionsForm(MapOptions options)
		{
			// Initialize
			InitializeComponent();

			// Keep settings
			this.options = options;

			// Go for all configurations
			foreach(ConfigurationInfo t in General.Configs)
			{
				// Add config name to list
				int index = config.Items.Add(t);

				// Is this configuration currently selected?
				if(string.Compare(t.Filename, options.ConfigFile, true) == 0)
				{
					// Select this item
					config.SelectedIndex = index;
				}
			}

			//mxd. Select the first config if none are selected...
			if(config.SelectedIndex == -1 && config.Items.Count > 0) config.SelectedIndex = 0;
		}

		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			// Configuration selected?
			/*if(config.SelectedIndex == -1)
			{
				// Select a configuration!
				MessageBox.Show(this, "Please select a game configuration to use for editing your map.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				config.Focus();
				return;
			}*/

			// Collect information
			ConfigurationInfo configinfo = General.Configs[config.SelectedIndex];
			if((configinfo.TestProgram == "") || !File.Exists(configinfo.TestProgram))
			{
				switch(MessageBox.Show(this, "Your game engine is not set for the current game configuration. Would you like to set up your game engine now?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
				{
					// Show Preferences...
					case DialogResult.Yes:
						General.MainWindow.ShowConfigurationPage(0, configinfo);
						break;

					case DialogResult.No: return;
					default: return;
				}
			}


			/*DataLocationList locations = datalocations.GetResources();
			
			// When making a new map, check if we should warn the user for missing resources
			if(newmap && (locations.Count == 0) && (configinfo.Resources.Count == 0))
			{
				if(MessageBox.Show(this, "You are about to make a map without selecting any resources. Textures, flats and " +
										 "sprites may not be shown correctly or may not show up at all. Do you want to continue?", Application.ProductName,
										 MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
				{
					return;
				}
			}*/

			// Next checks are only for maps that are already opened
			if(!newmap)
			{
				// When the user changed the configuration to one that has a different read/write interface,
				// we have to warn the user that the map may not be compatible.
				
				// Configuration changed?
				if((options.ConfigFile != "") && (General.Configs[config.SelectedIndex].Filename != options.ConfigFile))
				{
					// Load the new cfg file
					Configuration newcfg = General.LoadGameConfiguration(General.Configs[config.SelectedIndex].Filename);
					if(newcfg == null) return;

					// Check if the config uses a different IO interface
					if(newcfg.ReadSetting("formatinterface", "") != General.Map.Config.FormatInterface)
					{
						// Warn the user about IO interface change
						if(General.ShowWarningMessage("The game configuration you selected uses a different file format than your current map. Because your map was not designed for this format it may cause the map to work incorrectly in the game. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) == DialogResult.No)
						{
							// Reset to old configuration
							for(int i = 0; i < config.Items.Count; i++)
							{
								// Is this configuration the old config?
								if(string.Compare(General.Configs[i].Filename, options.ConfigFile, true) == 0)
								{
									// Select this item
									config.SelectedIndex = i;
								}
							}
							return;
						}
					}
				}
			}
			
			// Apply changes
			options.ConfigFile = General.Configs[config.SelectedIndex].Filename;

			// Reset default drawing textures
			//General.Settings.DefaultTexture = null;
			//General.Settings.DefaultFloorTexture = null;
			//General.Settings.DefaultCeilingTexture = null;
			
			// Hide window
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			// Just hide window
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// Help
		private void MapOptionsForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			General.ShowHelp("w_mapoptions.html");
			hlpevent.Handled = true;
		}
	}
}