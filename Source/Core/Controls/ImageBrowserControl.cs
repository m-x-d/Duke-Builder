
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
using mxd.DukeBuilder.Data;

#endregion

namespace mxd.DukeBuilder.Controls
{
	internal partial class ImageBrowserControl : UserControl
	{
		#region ================== Constants
		
		private static readonly HashSet<char> AllowedSpecialChars = new HashSet<char>("!@#$%^&*()-_=+<>,.?/'\"\\;:[]{}`~".ToCharArray()); //mxd

		#endregion
		
		#region ================== Delegates / Events

		public delegate void SelectedItemChangedDelegate(ImageBrowserItem item);
		public delegate void SelectedItemDoubleClickDelegate(ImageBrowserItem item);

		public event SelectedItemChangedDelegate SelectedItemChanged;
		public event SelectedItemDoubleClickDelegate SelectedItemDoubleClicked;
		
		#endregion

		#region ================== Variables
		
		// Properties
		private bool preventselection;
		
		// States
		private int keepselected;
		private bool blockupdate; //mxd
		
		//mxd. All items
		private List<ImageBrowserItem> items;

		// Filtered items
		private List<ImageBrowserItem> visibleitems;
		
		#endregion

		#region ================== Properties

		public bool PreventSelection { get { return preventselection; } set { preventselection = value; } }
		public bool HideInputBox { get { return splitter.Panel2Collapsed; } set { splitter.Panel2Collapsed = value; } }
		public List<ImageBrowserItem> SelectedItems { get { return list.SelectedItems; } } //mxd
		public ImageBrowserItem SelectedItem { get { return (list.SelectedItems.Count > 0 ? list.SelectedItems[0] : null); } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public ImageBrowserControl()
		{
			// Initialize
			InitializeComponent();
			items = new List<ImageBrowserItem>();

			//mxd
			StepsList sizes = new StepsList { 4, 8, 16, 32, 48, 64, 96, 128, 196, 256, 512, 1024 };
			filterWidth.StepValues = sizes;
			filterHeight.StepValues = sizes;

			//mxd. Looks like SplitterDistance is unaffected by DPI scaling. Let's fix that...
			/*if(MainForm.DPIScaler.Height != 1.0f)
			{
				splitter.SplitterDistance = splitter.Height - splitter.Panel2.Height - (int)Math.Round(splitter.SplitterWidth * MainForm.DPIScaler.Height);
			}*/

			//mxd
			list.SelectionChanged += list_SelectionChanged;
		}

		// This applies the application settings
		public void ApplySettings(string settingpath)
		{
			blockupdate = true;

			// Used images on top
			usedimagesfirst.Checked = General.Settings.ReadSetting(settingpath + "showusedimagesfirst", false);

			// Show image sizes
			showimagesizes.Checked = General.Settings.ReadSetting(settingpath + "showimagesizes", true);
			list.ShowImageSizes = showimagesizes.Checked;
			
			// Image size
			int imagesize = General.Settings.ReadSetting(settingpath + "imagesize", 128);
			sizecombo.Text = (imagesize == 0 ? sizecombo.Items[0].ToString() : imagesize.ToString());
			list.ImageSize = imagesize;

			// Po2 setting
			int po2mode = General.Settings.ReadSetting(settingpath + "po2mode", 0);
			po2combo.SelectedIndex = General.Clamp(po2mode, 0, po2combo.Items.Count);

			// Opacity setting
			int alphamode = General.Settings.ReadSetting(settingpath + "alphamode", 0);
			alphacombo.SelectedIndex = General.Clamp(alphamode, 0, alphacombo.Items.Count);

			blockupdate = false;
		}

		//mxd. Save settings
		public virtual void OnClose(string settingpath)
		{
			General.Settings.WriteSetting(settingpath + "showusedimagesfirst", usedimagesfirst.Checked);
			General.Settings.WriteSetting(settingpath + "showimagesizes", showimagesizes.Checked);
			General.Settings.WriteSetting(settingpath + "imagesize", list.ImageSize);
			General.Settings.WriteSetting(settingpath + "po2mode", po2combo.SelectedIndex);
			General.Settings.WriteSetting(settingpath + "alphamode", alphacombo.SelectedIndex);

			CleanUp();
		}

		// This cleans everything up
		public virtual void CleanUp()
		{
			// Stop refresh timer
			refreshtimer.Enabled = false;
		}

		#endregion

		#region ================== Rendering

		// Refresher
		private void refreshtimer_Tick(object sender, EventArgs e)
		{
			bool allpreviewsloaded = true;
			bool redrawneeded = false; //mxd
			
			// Go for all items
			foreach(ImageBrowserItem i in list.Items)
			{
				// Check if there are still previews that are not loaded
				allpreviewsloaded &= i.IsPreviewLoaded;

				//mxd. Item needs to be redrawn?
				redrawneeded |= i.CheckRedrawNeeded();
			}

			// If all previews were loaded, stop this timer
			if(allpreviewsloaded) refreshtimer.Stop();

			// Redraw the list if needed
			if(redrawneeded) list.Invalidate();
		}

		#endregion

		#region ================== Events

		// Name typed
		private void objectname_TextChanged(object sender, EventArgs e)
		{
			// Update list
			RefillList(false);

			// No item selected?
			if(list.SelectedItems.Count == 0)
			{
				// Select first
				SelectFirstItem();
			}
		}

		// Key pressed in textbox
		private void objectname_KeyDown(object sender, KeyEventArgs e)
		{
			// Toggle used items sorting
			if(e.KeyData == Keys.Tab)
			{
				usedimagesfirst.Checked = !usedimagesfirst.Checked;
				e.SuppressKeyPress = true;
			}
			//mxd. Clear text field instead of typing strange chars...
			else if(e.KeyData == (Keys.Back | Keys.Control))
			{
				if(objectname.Text.Length > 0) objectname.Clear();
				e.SuppressKeyPress = true;
			}
		}

		//mxd
		private void objectclear_Click(object sender, EventArgs e)
		{
			objectname.Clear();
			list.Focus();
		}

		//mxd
		private void filtersize_WhenTextChanged(object sender, EventArgs e) 
		{
			objectname_TextChanged(sender, e);
		}

		//mxd
		protected override bool ProcessTabKey(bool forward)
		{
			usedimagesfirst.Checked = !usedimagesfirst.Checked;
			return false;
		}
		
		// Selection changed
		private void list_SelectionChanged(object sender, List<ImageBrowserItem> selection)
		{
			// Prevent selecting?
			if(preventselection)
			{
				if(selection.Count > 0) list.ClearSelection(); //mxd
			}
			else
			{
				// Raise event
				if(SelectedItemChanged != null)
					SelectedItemChanged(list.SelectedItems.Count > 0 ? list.SelectedItems[0] : null);
			}
		}
		
		// Doublelicking an item
		private void list_ItemDoubleClicked(object sender, ImageBrowserItem item)
		{
			if(!preventselection && (list.SelectedItems.Count > 0))
				if(SelectedItemDoubleClicked != null) SelectedItemDoubleClicked(item);
		}

		//mxd. Transfer input to Filter textbox
		private void list_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == 8) // Backspace
			{
				if(objectname.Text.Length > 0)
				{
					if(objectname.SelectionLength > 0)
					{
						objectname.Text = objectname.Text.Substring(0, objectname.SelectionStart) +
							objectname.Text.Substring(objectname.SelectionStart + objectname.SelectionLength);
					}
					else
					{
						objectname.Text = objectname.Text.Substring(0, objectname.Text.Length - 1);
					}
				}
			}
			else if(e.KeyChar == 127) // Ctrl-Backspace
			{
				if(objectname.Text.Length > 0) objectname.Clear();
			}
			else if((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || AllowedSpecialChars.Contains(e.KeyChar))
			{
				if(objectname.SelectionLength > 0)
				{
					objectname.Text = objectname.Text.Substring(0, objectname.SelectionStart) +
										 e.KeyChar +
										 objectname.Text.Substring(objectname.SelectionStart + objectname.SelectionLength);
				}
				else
				{
					objectname.Text += e.KeyChar;
				}
			}
		}

		//mxd
		private void sizecombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(blockupdate) return;

			switch(sizecombo.SelectedIndex)
			{
				case 0: list.ImageSize = 0; break;
				case 1: list.ImageSize = -1; break;
				default: list.ImageSize = Convert.ToInt32(sizecombo.SelectedItem); break;
			}

			list.Focus();
		}

		//mxd
		private void alphacombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!blockupdate)
			{
				RefillList(false);
				list.Focus();
			}
		}

		//mxd
		private void po2combo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!blockupdate)
			{
				RefillList(false);
				list.Focus();
			}
		}

		//mxd
		private void usedimagesfirst_CheckedChanged(object sender, EventArgs e)
		{
			if(!blockupdate)
			{
				RefillList(false);
				list.Focus();
			}
		}

		//mxd
		private void showimagesizes_CheckedChanged(object sender, EventArgs e)
		{
			if(!blockupdate)
			{
				list.ShowImageSizes = showimagesizes.Checked;
				list.Refresh();
				list.Focus();
			}
		}
		
		#endregion

		#region ================== Methods

		// This selects an item by tile index
		public void SelectItem(int tileindex)
		{
			// Not when selecting is prevented
			if(preventselection) return;

			// Search for item
			ImageBrowserItem target = null; //mxd
			foreach(ImageBrowserItem item in items)
			{
				if(item.Icon.TileIndex == tileindex) //mxd
				{
					target = item;
					break;
				}
			}
			
			if(target != null)
			{
				// Select the item
				list.SetSelectedItem(target);
			}
		}
		
		// This selectes the first item
		private void SelectFirstItem()
		{
			// Not when selecting is prevented
			if(preventselection) return;
			
			// Select first
			if(list.Items.Count > 0) list.SetSelectedItem(list.Items[0]);
		}
		
		// This begins adding items
		public void BeginAdding(bool keepselectedindex)
		{
			if(keepselectedindex && (list.SelectedItems.Count > 0))
				keepselected = list.Items.IndexOf(list.SelectedItems[0]);
			else
				keepselected = -1;
			
			// Clean list
			items.Clear();
			
			// Stop updating
			refreshtimer.Enabled = false;
		}

		// This ends adding items
		public void EndAdding()
		{
			// Fill list with items
			RefillList(true);

			// Start updating
			refreshtimer.Enabled = true;
		}
		
		// This adds an item
		public void AddItem(ImageData image, string tooltip)
		{
			items.Add(new ImageBrowserItem(image, tooltip));
		}

		public void AddItem(ImageData image)
		{
			items.Add(new ImageBrowserItem(image, string.Empty));
		}

		// This fills the list based on the objectname filter
		private void RefillList(bool selectfirst)
		{
			visibleitems = new List<ImageBrowserItem>();

			//mxd. Store info about currently selected item
			string selectedname = string.Empty;
			if(!selectfirst && keepselected == -1 && list.SelectedItems.Count > 0)
			{
				selectedname = list.SelectedItems[0].TileName;
			}

			// Clear list first
			list.Clear();

			//mxd. Anything to do?
			if(items.Count == 0) return;

			//mxd. Filtering by texture size?
			int w = filterWidth.GetResult(-1);
			int h = filterHeight.GetResult(-1);
			
			// Go for all items
			ImageBrowserItem previtem = null; //mxd
			for(int i = items.Count - 1; i > -1; i--)
			{
				// Add item if valid
				if(ValidateItem(items[i], previtem) && ValidateItemSize(items[i], w, h))
				{
					visibleitems.Add(items[i]);
					previtem = items[i];
				}
			}
			
			// Fill list
			visibleitems.Sort(SortItems);
			list.SetItems(visibleitems);

			// Update Filter group title
			if(visibleitems.Count != items.Count)
			{
				filtergroup.Text = " Filtering (" + visibleitems.Count + " out of " + items.Count + " images shown) ";
			}
			else
			{
				filtergroup.Text = " Filtering ";
			}
			
			// Make selection?
			if(!preventselection && list.Items.Count > 0)
			{
				// Select specific item?
				if(keepselected > -1)
				{
					list.SetSelectedItem(list.Items[keepselected]);
				}
				// Select first item?
				else if(selectfirst)
				{
					SelectFirstItem();
				}
				//mxd. Try reselecting the same/next closest item
				else if(!string.IsNullOrEmpty(selectedname))
				{
					ImageBrowserItem bestmatch = null;
					int charsmatched = 1;
					foreach(ImageBrowserItem item in list.Items)
					{
						if(item.TileName[0] == selectedname[0])
						{
							if(item.TileName == selectedname)
							{
								bestmatch = item;
								break;
							}

							for(int i = 1; i < Math.Min(item.TileName.Length, selectedname.Length); i++)
							{
								if(item.TileName[i] != selectedname[i])
								{
									if(i > charsmatched)
									{
										bestmatch = item;
										charsmatched = i;
									}
									break;
								}
							}
						}
					}

					// Select found item
					if(bestmatch != null)
					{
						list.SetSelectedItem(bestmatch);
					}
					else
					{
						SelectFirstItem();
					}
				}
			}
			
			// Raise event
			if((SelectedItemChanged != null) && !preventselection)
				SelectedItemChanged(list.SelectedItems.Count > 0 ? list.SelectedItems[0] : null);
		}

		// This validates an item
		private bool ValidateItem(ImageBrowserItem item, ImageBrowserItem previtem)
		{
			//mxd. Don't show duplicate items
			if(previtem != null && item.TileName == previtem.TileName) return false; //mxd

			//mxd. Filter by Po2 setting. 0 - Any, 1 - only Po2, 2 - only nPo2
			switch(po2combo.SelectedIndex)
			{
				case 1: if(!item.IsPowerOf2) return false; break;
				case 2: if(item.IsPowerOf2) return false; break;
			}

			//mxd. Filter by Opacity setting. 0 - Any, 1 - only with alpha, 2 - only without alpha
			switch(alphacombo.SelectedIndex)
			{
				case 1: if(!item.HasAlpha) return false; break;
				case 2: if(item.HasAlpha) return false; break;
			}

			// Filter by name
			return item.TileName.ToUpperInvariant().Contains(objectname.Text.ToUpperInvariant());
		}

		//mxd. This validates an item's texture size
		private static bool ValidateItemSize(ImageBrowserItem i, int w, int h) 
		{
			if(!i.Icon.IsPreviewLoaded) return true;
			if(w > 0 && i.Icon.Width != w) return false;
			if(h > 0 && i.Icon.Height != h) return false;
			return true;
		}

		//mxd
		private int SortItems(ImageBrowserItem item1, ImageBrowserItem item2)
		{
			if(usedimagesfirst.Checked && item1.Icon.UsedInMap != item2.Icon.UsedInMap)
			{
				// Push used items to the top
				return (item1.Icon.UsedInMap ? -1 : 1);
			}

			return item1.CompareTo(item2);
		}
		
		//mxd. This sends the focus to the textures list
		public void FocusList()
		{
			list.Focus();
		}
		
		#endregion
	}
}
