
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
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Windows
{
	internal partial class EditLineForm : DelayedForm
	{
		// Variables
		private int numlines;
		private List<Sidedef> frontwalls;
		private List<Sidedef> backwalls;
		
		// Constructor
		public EditLineForm()
		{
			// Initialize
			InitializeComponent();
		}
		
		// This sets up the form to edit the given lines
		public bool Setup(ICollection<Linedef> lines)
		{
			numlines = lines.Count;
			this.Text = "Edit " + (numlines > 1 ? "lines (" + numlines + ")" : "line " + General.GetFirst(lines).Index);

			// Create walls collections
			frontwalls = new List<Sidedef>();
			backwalls = new List<Sidedef>();
			foreach(Linedef l in lines)
			{
				if(l.Front != null) frontwalls.Add(l.Front);
				if(l.Back != null) backwalls.Add(l.Back);
			}

			if(frontwalls.Count + backwalls.Count == 0) return false;

			// Setup or hide tabs
			if(frontwalls.Count > 0) front.Setup(frontwalls); else tabs.TabPages.Remove(tabfront);
			if(backwalls.Count > 0) back.Setup(backwalls); else tabs.TabPages.Remove(tabback);

			return true;
		}

		// This sets up the form to edit the given walls
		public bool Setup(ICollection<Sidedef> walls)
		{
			if(walls.Count == 0) return false;
			this.Text = "Edit " + (walls.Count > 1 ? "walls (" + walls.Count + ")" : "wall " + General.GetFirst(walls).Index);

			numlines = -1;
			frontwalls = new List<Sidedef>(walls);
			backwalls = new List<Sidedef>();
			front.Setup(frontwalls);
			tabs.TabPages.Remove(tabback);
			tabfront.Text = "Properties";

			return true;
		}

		// Apply clicked
		private void apply_Click(object sender, EventArgs e)
		{
			// Validate changes
			if(!front.IsValid())
			{
				tabs.SelectedTab = tabfront;
				return;
			}

			if(!back.IsValid())
			{
				tabs.SelectedTab = tabback;
				return;
			}
			
			// Make undo
			string undodesc;
			if(numlines == -1) undodesc = (frontwalls.Count > 1 ? frontwalls.Count + " walls" : "wall");
			else undodesc = (numlines > 1 ? numlines + " lines" : "line");

			General.Map.UndoRedo.CreateUndo("Edit " + undodesc);
			
			// Apply changes
			if(frontwalls.Count > 0) front.ApplyTo(frontwalls);
			if(backwalls.Count > 0) back.ApplyTo(backwalls);

			// Update the used images
			General.Map.Data.UpdateUsedImages();
			
			// Done
			General.Map.IsChanged = true;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			// Be gone
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// Help!
		private void LinedefEditForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			General.ShowHelp("w_linedefedit.html");
			hlpevent.Handled = true;
		}
	}
}
