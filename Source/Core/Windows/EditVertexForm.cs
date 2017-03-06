
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
using mxd.DukeBuilder.Controls;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Windows
{
	public partial class EditVertexForm : DelayedForm
	{
		#region ================== Constants

		private const int VALUE_MISMATCH = int.MinValue;

		#endregion
		
		#region ================== Variables

		private ICollection<Vertex> vertices;

		#endregion
		
		#region ================== Constructor

		// Constructor
		public EditVertexForm()
		{
			InitializeComponent();
		}

		#endregion
		
		#region ================== Methods

		// This sets up the form to edit the given vertices
		public void Setup(ICollection<Vertex> vertices)
		{
			// Keep this list
			this.vertices = vertices;
			if(vertices.Count > 1) this.Text = "Edit vertices (" + vertices.Count + ")";

			// Get first vertex position
			var pos = General.GetFirst(vertices).Position;

			// Go for all vertices
			foreach(Vertex v in vertices)
			{
				// Position
				if(pos.x != v.Position.x) pos.x = VALUE_MISMATCH;
				if(pos.y != v.Position.y) pos.y = VALUE_MISMATCH;
				if(pos.x == VALUE_MISMATCH && pos.y == VALUE_MISMATCH) break;
			}

			// Update interface
			if(pos.x != VALUE_MISMATCH) posx.Text = pos.x.ToString();
			if(pos.y != VALUE_MISMATCH) posy.Text = (-pos.y).ToString();
		}
		
		#endregion
		
		#region ================== Events

		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			string undodesc = "vertex";

			// Verify the coordinates
			int px = posx.GetResult(0);
			int py = -posy.GetResult(0);
			if((px < General.Map.FormatInterface.MinCoordinate) || (px > General.Map.FormatInterface.MaxCoordinate) ||
			   (py < General.Map.FormatInterface.MinCoordinate) || (py > General.Map.FormatInterface.MaxCoordinate))
			{
				General.ShowWarningMessage("Vertex coordinates must be between " + General.Map.FormatInterface.MinCoordinate + " and " + General.Map.FormatInterface.MaxCoordinate + ".", MessageBoxButtons.OK);
				return;
			}

			// Skip when both fields have mixed values
			if(posx.ApplyMode != NumericTextboxApplyMode.NO_VALUE || posy.ApplyMode != NumericTextboxApplyMode.NO_VALUE)
			{
				// Make undo
				if(vertices.Count > 1) undodesc = vertices.Count + " vertices";
				General.Map.UndoRedo.CreateUndo("Edit " + undodesc);

				// Go for all vertices
				foreach(Vertex v in vertices)
				{
					// Apply position
					v.Move(new Vector2D(posx.GetResultFloat(v.Position.x), -posy.GetResultFloat(-v.Position.y)));
				}

				// Done
				General.Map.IsChanged = true;
			}
			
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e)
		{
			// Just close
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// Help requested
		private void VertexEditForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			General.ShowHelp("w_vertexeditor.html");
			hlpevent.Handled = true;
		}

		#endregion
	}
}