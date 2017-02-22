
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
using mxd.DukeBuilder.Config.ImageSets;
using mxd.DukeBuilder.Controls;
using mxd.DukeBuilder.Data;

#endregion

namespace mxd.DukeBuilder.Windows
{
	internal partial class ImageBrowserForm : DelayedForm
	{
		//mxd. Constants
		private const string ALL_IMAGES = "[All]";
		
		//mxd. Structs
		private struct TreeNodeData
		{
			public IImageSet Set;
			public string FolderName;
		}
		
		// Variables
		private int selectedtile;
		private int initialtile;
		private TreeNode selectedset; //mxd
		private int selecttileonfill; //mxd
		
		// Properties
		public int SelectedTile { get { return selectedtile; } }
		
		// Constructor
		public ImageBrowserForm(int selecttile)
		{
			Cursor.Current = Cursors.WaitCursor;
			General.Interface.DisableProcessing(); //mxd

			TreeNode item; //mxd
			selectedset = null; //mxd
			
			// Initialize
			InitializeComponent();

			// Setup texture browser
			browser.ApplySettings(configpath);
			
			// Update the used textures
			General.Map.Data.UpdateUsedImages();

			tvTextureSets.BeginUpdate(); //mxd

			//mxd. Texture longname to select when list is filled
			selecttileonfill = selecttile;
			initialtile = selecttile;

			//mxd. Fill texture sets list with normal texture sets
			foreach(IImageSet ts in General.Map.Data.ImageSets) 
			{
				if(ts.Images.Count == 0) continue;

				item = tvTextureSets.Nodes.Add(ts.Name + " [" + ts.Images.Count + "]");
				item.Name = ts.Name;
				item.Tag = new TreeNodeData { Set = ts, FolderName = ts.Name };
				item.ImageIndex = 0;
			}

			//mxd. Add container-specific texture sets
			foreach(ResourceImageSet ts in General.Map.Data.ResourceImageSets)
			{
				if(ts.Images.Count == 0) continue;

				item = tvTextureSets.Nodes.Add(ts.Name);
				item.Name = ts.Name;
				item.Tag = new TreeNodeData { Set = ts, FolderName = ts.Name };
				item.ImageIndex = 2 + (int)ts.Location.type;
				item.SelectedImageIndex = item.ImageIndex;

				CreateNodes(item);
				item.Expand();
			}

			//mxd. Add "All" texture set
			int count = General.Map.Data.AllImageSet.Images.Count;
			item = tvTextureSets.Nodes.Add(General.Map.Data.AllImageSet.Name + " [" + count + "]");
			item.Name = General.Map.Data.AllImageSet.Name;
			item.Tag = new TreeNodeData { Set = General.Map.Data.AllImageSet, FolderName = General.Map.Data.AllImageSet.Name };
			item.ImageIndex = 1;
			item.SelectedImageIndex = item.ImageIndex;

			//mxd. Get the previously selected texture set
			string prevtextureset = General.Settings.ReadSetting(configpath + "textureset", "");
			TreeNode match;

			// When texture set name is empty, select "All" texture set
			if(string.IsNullOrEmpty(prevtextureset))
			{
				match = tvTextureSets.Nodes[tvTextureSets.Nodes.Count - 1];
			}
			else
			{
				match = FindNodeByName(tvTextureSets.Nodes, prevtextureset);
			}

			if(match != null)
			{
				IImageSet set = ((TreeNodeData)match.Tag).Set;
				foreach(ImageData img in set.Images)
				{
					if(img.TileIndex == selecttile)
					{
						selectedset = match;
						break;
					}
				}
			}

			//mxd. If the selected texture was not found in the last-selected set, try finding it in the other sets
			if(selectedset == null)
			{
				foreach(TreeNode n in tvTextureSets.Nodes)
				{
					selectedset = FindImageSet(n, selecttile);
					if(selectedset != null) break;
				}
			}

			//mxd. Texture still not found? Then just select the last used set
			if(selectedset == null && match != null) selectedset = match;

			//mxd. Select the found set or "All", if none were found
			if(tvTextureSets.Nodes.Count > 0)
			{
				if(selectedset == null) selectedset = tvTextureSets.Nodes[tvTextureSets.Nodes.Count - 1];
				tvTextureSets.SelectedNodes.Clear();
				tvTextureSets.SelectedNodes.Add(selectedset);
				selectedset.EnsureVisible();
			}

			tvTextureSets.EndUpdate(); //mxd

			//mxd. Set splitter position and state (doesn't work when layout is suspended)
			if(General.Settings.ReadSetting(configpath + "splittercollapsed", false))
				splitter.IsCollapsed = true;

			//mxd. Looks like SplitterDistance is unaffected by DPI scaling. Let's fix that...
			int splitterdistance = General.Settings.ReadSetting(configpath + "splitterdistance", int.MinValue);
			if(splitterdistance == int.MinValue)
			{
				splitterdistance = 210;
				/*if(MainForm.DPIScaler.Width != 1.0f)
				{
					splitterdistance = (int)Math.Round(splitterdistance * MainForm.DPIScaler.Width);
				}*/
			}

			splitter.SplitPosition = splitterdistance;
		}

		//mxd
		private static int SortImageData(ImageData img1, ImageData img2) 
		{
			if(img1.TileIndex > img2.TileIndex) return 1;
			if(img1.TileIndex < img2.TileIndex) return -1;
			return 0;
		}

		//mxd
		private static int SortTreeNodes(TreeNode n1, TreeNode n2)
		{
			return String.Compare(n1.Text, n2.Text, StringComparison.InvariantCultureIgnoreCase);
		}

		//mxd
		private static TreeNode FindImageSet(TreeNode node, int tileindex) 
		{
			if(node.Name == ALL_IMAGES) return null; // Skip "All images" set
			
			// First search in child nodes
			foreach(TreeNode n in node.Nodes)
			{
				TreeNode match = FindImageSet(n, tileindex);
				if(match != null) return match;
			}

			// Then - in current node
			IImageSet set = ((TreeNodeData)node.Tag).Set;

			foreach(ImageData img in set.Images)
				if(img.TileIndex == tileindex) return node;

			return null;
		}

		//mxd
		private static TreeNode FindNodeByName(TreeNodeCollection nodes, string selectname) 
		{
			foreach(TreeNode n in nodes) 
			{
				if(n.Name == selectname) return n;

				TreeNode match = FindNodeByName(n.Nodes, selectname);
				if(match != null) return match;
			}
			return null;
		}

		//mxd
		private void CreateNodes(TreeNode root)
		{
			TreeNodeData rootdata = (TreeNodeData)root.Tag;
			ResourceImageSet set = rootdata.Set as ResourceImageSet;
			if(set == null) 
			{
				General.ErrorLogger.Add(ErrorType.Error, "Resource " + root.Name + " doesn't have ImageSet!");
				return;
			}

			int imageIndex = (int)set.Location.type + 5;
			//char[] separator = { Path.AltDirectorySeparatorChar };
			
			ImageData[] images = new ImageData[set.Images.Count];
			set.Images.CopyTo(images, 0);

			Array.Sort(images, SortImageData);

			List<ImageData> rootimages = new List<ImageData>();
			foreach(ImageData image in images) 
			{
				rootimages.Add(image);
				
				//TODO
				/*string[] parts = image.VirtualName.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				TreeNode curNode = root;

				if(parts.Length == 1)
				{
					rootimages.Add(image);
					continue;
				}

				int localindex = ((parts[0] == "[TEXTURES]" || image is TEXTURESImage) ? 8 : imageIndex);
				string category = set.Name;
				for(int i = 0; i < parts.Length - 1; i++) 
				{
					category += (Path.DirectorySeparatorChar + parts[i]);
					
					//already got such category?
					if(curNode.Nodes.Count > 0 && curNode.Nodes.ContainsKey(category)) 
					{
						curNode = curNode.Nodes[category];
					} 
					else //create a new one
					{
						TreeNode n = new TreeNode(parts[i]) { Name = category, ImageIndex = localindex, SelectedImageIndex = localindex };
						curNode.Nodes.Add(n);
						curNode = n;
						curNode.Tag = new TreeNodeData { Set = new ResourceTextureSet(category, set.Location), FolderName = parts[i] };
					}

					// Add to current node
					if(i == parts.Length - 2) 
					{
						ResourceTextureSet curTs = ((TreeNodeData)curNode.Tag).Set as ResourceTextureSet;
						if(image.IsFlat)
							curTs.AddFlat(image);
						else
							curTs.AddTexture(image);
					}
				}*/
			}

			// Shift the tree up when only single child node was added
			if(root.Nodes.Count == 1 && root.Nodes[0].Nodes.Count > 0) 
			{
				TreeNode[] children = new TreeNode[root.Nodes[0].Nodes.Count];
				root.Nodes[0].Nodes.CopyTo(children, 0);
				root.Nodes.Clear();
				root.Nodes.AddRange(children);
			}

			// Add "All set textures" node
			if(root.Nodes.Count > 1)
			{
				TreeNode allnode = new TreeNode(ALL_IMAGES)
				{
					Name = ALL_IMAGES,
					ImageIndex = imageIndex,
					SelectedImageIndex = imageIndex,
					Tag = new TreeNodeData { Set = set, FolderName = ALL_IMAGES }
				};
				root.Nodes.Add(allnode);
			}

			// Sort immediate child nodes...
			TreeNode[] rootnodes = new TreeNode[root.Nodes.Count];
			root.Nodes.CopyTo(rootnodes, 0);
			Array.Sort(rootnodes, SortTreeNodes);
			root.Nodes.Clear();
			root.Nodes.AddRange(rootnodes);

			// Re-add root images
			ResourceImageSet rootset = new ResourceImageSet(set.Name, set.Location);
			foreach(ImageData data in rootimages) rootset.AddImage(data);

			// Store root data
			rootdata.Set = rootset;
			root.Tag = rootdata;

			// Set root images count
			if(rootset.Images.Count > 0) root.Text += " [" + rootset.Images.Count + "]";

			// Add image count to node titles
			foreach(TreeNode n in root.Nodes) SetItemsCount(n);
		}

		//mxd
		private static void SetItemsCount(TreeNode node) 
		{
			ResourceImageSet ts = ((TreeNodeData)node.Tag).Set as ResourceImageSet;
			if(ts == null) throw new Exception("Expected ResourceImageSet, but got null...");
			if(ts.Images.Count > 0) node.Text += " [" + ts.Images.Count + "]";
			foreach(TreeNode child in node.Nodes) SetItemsCount(child);
		}

		// Selection changed
		private void browser_SelectedItemChanged(ImageBrowserItem item)
		{
			apply.Enabled = (item != null);
		}

		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			// Set selected name and close
			if(browser.SelectedItem != null)
			{
				selectedtile = browser.SelectedItem.TileIndex;
				DialogResult = DialogResult.OK;
			}
			else
			{
				selectedtile = initialtile;
				DialogResult = DialogResult.Cancel;
			}
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			// No selection, close
			selectedtile = initialtile;
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// Activated
		private void TextureBrowserForm_Activated(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.Default;
			General.Interface.EnableProcessing(); //mxd
		}

		// Closing
		private void TextureBrowserForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Save window settings
			General.Settings.WriteSetting(configpath + "splitterdistance", splitter.SplitPosition); //mxd
			General.Settings.WriteSetting(configpath + "splittercollapsed", splitter.IsCollapsed); //mxd

			//mxd. Save last selected texture set
			if(this.DialogResult == DialogResult.OK && tvTextureSets.SelectedNodes.Count > 0)
				General.Settings.WriteSetting(configpath + "textureset", tvTextureSets.SelectedNodes[0].Name);
			
			// Clean up
			browser.OnClose(configpath);
		}

		// Static method to browse for an image.
		public static int Browse(IWin32Window parent, int selecttile)
		{
			ImageBrowserForm browser = new ImageBrowserForm(selecttile);
			return (browser.ShowDialog(parent) == DialogResult.OK ? browser.SelectedTile : selecttile);
		}

		// Item double clicked
		private void browser_SelectedItemDoubleClicked(ImageBrowserItem item)
		{
			if(item == null) return;
			if(selectedset == null) throw new NotSupportedException("selectedset required!");
			if(apply.Enabled) apply_Click(this, EventArgs.Empty);
		}

		// This fills the list of textures, depending on the selected texture set
		private void FillImagesList()
		{
			// Get the selected texture set
			IImageSet set = ((TreeNodeData)selectedset.Tag).Set;

			// Start adding
			browser.BeginAdding(false);

			// Add all available images
			foreach(ImageData img in set.Images) browser.AddItem(img);
			
			// Done adding
			browser.EndAdding();
		}

		// Help
		private void TextureBrowserForm_HelpRequested(object sender, HelpEventArgs e)
		{
			General.ShowHelp("w_imagesbrowser.html");
			e.Handled = true;
		}

		private void TextureBrowserForm_Shown(object sender, EventArgs e)
		{
			//mxd. Calling FillImagesList() from constructor results in TERRIBLE load times. Why? I have no sodding idea...
			if(selectedset != null) FillImagesList();
			
			// Select texture
			if(selecttileonfill != 0)
			{
				browser.SelectItem(selecttileonfill);
				selecttileonfill = 0;
			}

			//mxd. Focus the textures list. Calling this from TextureBrowserForm_Activated (like it's done in DB2) fails when the form is maximized. Again, I've no idea why...
			browser.FocusList();
		}

		//mxd
		private void tvTextureSets_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) 
		{
			selectedset = e.Node;
			FillImagesList();
		}

		//mxd
		private void tvTextureSets_KeyUp(object sender, KeyEventArgs e) 
		{
			if(tvTextureSets.SelectedNodes.Count > 0 && tvTextureSets.SelectedNodes[0] != selectedset) 
			{
				selectedset = tvTextureSets.SelectedNodes[0];
				FillImagesList();
			}
		}
	}
}