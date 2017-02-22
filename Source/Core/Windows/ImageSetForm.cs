
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
using System.Globalization;
using System.Windows.Forms;
using mxd.DukeBuilder.Config.ImageSets;
using mxd.DukeBuilder.Controls;
using mxd.DukeBuilder.Data;

#endregion

namespace mxd.DukeBuilder.Windows
{
	internal partial class ImageSetForm : DelayedForm
	{
		// Variables
		private DefinedImageSet textureset;
		
		// Constructor
		public ImageSetForm()
		{
			InitializeComponent();
			//matcheslist.ApplySettings();
			
			// Show/hide components
			matchesbutton.Visible = (General.Map != null);
			nomatchesbutton.Visible = (General.Map != null);
			matcheslist.Visible = (General.Map != null);
			noresultlabel.Visible = (General.Map == null);
		}
		
		// This initializes the set
		public void Setup(DefinedImageSet set)
		{
			// Keep reference
			textureset = set;
			
			// Set name
			name.Text = set.Name;
			
			// Fill filters list
			foreach(int i in set.Filters) filters.Items.Add(i.ToString());
		}

		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			// Apply name
			textureset.Name = name.Text;
			
			// Apply filters
			textureset.Filters.Clear();
			foreach(ListViewItem i in filters.Items)
			{
				int num;
				if(!int.TryParse(i.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
				{
					General.ShowErrorMessage("Unable to get image tile index from \"" + i.Text.Trim() + "\"", MessageBoxButtons.OK);
					return;
				}
				textureset.Filters.Add(num);
			}
			
			// Done
			matcheslist.CleanUp();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			// Be gone.
			matcheslist.CleanUp();
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		
		// Add image
		private void addfilter_Click(object sender, EventArgs e)
		{
			ListViewItem i = new ListViewItem("");
			filters.Items.Add(i);
			i.BeginEdit();
		}
		
		// Remove selected items
		private void removefilter_Click(object sender, EventArgs e)
		{
			foreach(ListViewItem i in filters.SelectedItems) i.Remove();
			
			// Run the timer
			filterstimer.Start();
		}
		
		// Items selected/deselected
		private void filters_SelectedIndexChanged(object sender, EventArgs e)
		{
			removefilter.Enabled = (filters.SelectedItems.Count > 0);
		}
		
		// Double clicking an item
		private void filters_DoubleClick(object sender, EventArgs e)
		{
			// Edit item
			if(filters.SelectedItems.Count == 1)
				filters.SelectedItems[0].BeginEdit();
		}
		
		// This removes empty items and makes others uppercase
		private void filterstimer_Tick(object sender, EventArgs e)
		{
			// Stop timer
			filterstimer.Stop();
			
			// Update labels
			for(int i = filters.Items.Count - 1; i >= 0; i--)
			{
				// Empty label?
				if((filters.Items[i].Text == null) ||
				   (filters.Items[i].Text.Trim().Length == 0))
				{
					// Remove it
					filters.Items.RemoveAt(i);
				}
				else
				{
					// Make uppercase
					filters.Items[i].Text = filters.Items[i].Text.ToUpperInvariant();
				}
			}
			
			// Show example results if when we can
			if(General.Map != null)
			{
				Cursor.Current = Cursors.AppStarting;
				
				// Make a set for comparing
				List<int> filterslist = new List<int>(filters.Items.Count);
				foreach(ListViewItem i in filters.Items)
				{
					int num;
					if(int.TryParse(i.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
						filterslist.Add(num);
				}
				MatchingImageSet set = new MatchingImageSet(filterslist);
				
				// Determine tooltip text
				string tooltiptext = null;
				if(nomatchesbutton.Checked) tooltiptext = "Doubleclick to include this texture";
				
				// Start adding
				matcheslist.PreventSelection = matchesbutton.Checked;
				matcheslist.BeginAdding(true);
				
				// Go for all textures
				foreach(ImageData img in General.Map.Data.Images)
				{
					bool ismatch = set.IsMatch(img);
					if((ismatch && matchesbutton.Checked) || (!ismatch && nomatchesbutton.Checked))
						matcheslist.AddItem(img, tooltiptext);
				}
				
				// Done adding
				matcheslist.EndAdding();
				Cursor.Current = Cursors.Default;
			}
		}
		
		// Done editing a filter
		private void filters_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			// Run the timer
			filterstimer.Start();
		}
		
		// When first shown
		private void TextureSetForm_Shown(object sender, EventArgs e)
		{
			// Run the timer
			filterstimer.Start();
		}
		
		// Show (not) matches clicked
		private void matchesbutton_Click(object sender, EventArgs e)
		{
			// Run the timer
			filterstimer.Start();
		}
		
		// Image doubleclicked
		private void matcheslist_SelectedItemDoubleClicked(ImageBrowserItem item)
		{
			// Add image index to the list
			if(item != null) filters.Items.Add(item.TileIndex.ToString());
			
			// Run the timer
			filterstimer.Start();
		}

		// Help
		private void TextureSetForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			General.ShowHelp("w_textureset.html");
			hlpevent.Handled = true;
		}
	}
}

