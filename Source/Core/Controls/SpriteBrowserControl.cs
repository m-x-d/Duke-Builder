
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
using mxd.DukeBuilder.Config;

#endregion

namespace mxd.DukeBuilder.Controls
{
	public partial class SpriteBrowserControl : UserControl
	{
		#region ================== Events

		public delegate void TypeChangedDeletegate(int tileindex);
		public delegate void TypeDoubleClickDeletegate(int tileindex);

		public event TypeChangedDeletegate OnTypeChanged;
		public event TypeDoubleClickDeletegate OnTypeDoubleClicked;

		#endregion

		#region ================== Variables

		private List<TreeNode> nodes;
		private bool preventchanges;
		
		#endregion

		#region ================== Constructor

		// Constructor
		public SpriteBrowserControl()
		{
			InitializeComponent();
		}

		// This sets up the control
		public void Setup()
		{
			// Go for all predefined categories
			typelist.Nodes.Clear();
			nodes = new List<TreeNode>();
			foreach(SpriteCategory tc in General.Map.Data.SpriteCategories)
			{
				// Create category
				TreeNode cn = typelist.Nodes.Add(tc.Name, tc.Title);
				if(tc.Color > -1 && tc.Color < thingimages.Images.Count) cn.ImageIndex = tc.Color;
				cn.SelectedImageIndex = cn.ImageIndex;
				foreach(SpriteInfo ti in tc.Sprites)
				{
					// Create thing
					TreeNode n = cn.Nodes.Add(ti.Title);
					if(ti.Color > -1 && ti.Color < thingimages.Images.Count) n.ImageIndex = ti.Color;
					n.SelectedImageIndex = n.ImageIndex;
					n.Tag = ti;
					nodes.Add(n);
				}
			}
		}

		#endregion
		
		#region ================== Methods

		// Select a type
		public void SelectType(int type)
		{
			preventchanges = true;

			typelist.SelectedNodes.Clear();
			foreach(TreeNode n in nodes)
			{
				// Matching node?
				if((n.Tag as SpriteInfo).Index == type)
				{
					// Select this
					n.Parent.Expand();
					typelist.SelectedNodes.Add(n);
					n.EnsureVisible();
					break;
				}
			}

			//mxd. Collapse nodes?
			if(typelist.SelectedNodes.Count == 0)
			{
				foreach(TreeNode n in nodes)
					if(n.Parent.IsExpanded) n.Parent.Collapse();
			}

			preventchanges = false;
		}

		// This clears the type
		/*public void ClearSelectedType()
		{
			preventchanges = true;

			// Clear selection
			typelist.SelectedNode = null;

			// Collapse nodes
			foreach(TreeNode n in nodes)
				if(n.Parent.IsExpanded) n.Parent.Collapse();
			
			preventchanges = false;
		}*/

		// Result
		public int GetResult(int original)
		{
			// Anything selected?
			if(typelist.SelectedNodes.Count == 1)
			{
				TreeNode n = typelist.SelectedNodes[0];
				if(n.Nodes.Count == 0 && n.Tag is SpriteInfo)
				{
					return (n.Tag as SpriteInfo).Index;
				}
			}

			return original;
		}

		#endregion

		#region ================== Events

		// List double-clicked
		private void typelist_DoubleClick(object sender, EventArgs e)
		{
			if(typelist.SelectedNodes.Count == 1)
			{
				// Node is a child node?
				TreeNode n = typelist.SelectedNodes[0];
				if(n.Nodes.Count == 0 && n.Tag is SpriteInfo && (OnTypeDoubleClicked != null))
					OnTypeDoubleClicked((n.Tag as SpriteInfo).Index);
			}
		}
		
		// Thing type selection changed
		private void typelist_AfterSelect(object sender, TreeViewEventArgs e)
		{
			// Anything selected?
			if(!preventchanges && typelist.SelectedNodes.Count == 1)
			{
				TreeNode n = typelist.SelectedNodes[0];

				// Raise event when node is a child node
				if(n.Nodes.Count == 0 && n.Tag is SpriteInfo && OnTypeChanged != null)
					OnTypeChanged((n.Tag as SpriteInfo).Index);
			}
		}
		
		#endregion
	}
}
