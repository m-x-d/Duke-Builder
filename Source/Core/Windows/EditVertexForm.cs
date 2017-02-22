
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

			// Get first vertex
			Vertex vc = General.GetFirst(vertices);

			// Position
			var pos = vc.Position;

			// Go for all vertices
			foreach(Vertex v in vertices)
			{
				// Position
				if(pos.x != v.Position.x) pos.x = VALUE_MISMATCH;
				if(pos.y != v.Position.y) pos.y = VALUE_MISMATCH;
				if(pos.x == VALUE_MISMATCH && pos.y == VALUE_MISMATCH) break;
			}

			// Update interface
			if(pos.x != VALUE_MISMATCH) positionx.Text = pos.x.ToString();
			if(pos.y != VALUE_MISMATCH) positiony.Text = (-pos.y).ToString();
		}
		
		#endregion
		
		#region ================== Events

		// OK clicked
		private void apply_Click(object sender, EventArgs e)
		{
			string undodesc = "vertex";

			// Verify the coordinates
			var pos = new Vector2D(positionx.GetResultFloat(0.0f), -positiony.GetResultFloat(0.0f));
			if((pos.x < General.Map.FormatInterface.MinCoordinate) || (pos.x > General.Map.FormatInterface.MaxCoordinate) ||
			   (pos.y < General.Map.FormatInterface.MinCoordinate) || (pos.y > General.Map.FormatInterface.MaxCoordinate))
			{
				General.ShowWarningMessage("Vertex coordinates must be between " + General.Map.FormatInterface.MinCoordinate + " and " + General.Map.FormatInterface.MaxCoordinate + ".", MessageBoxButtons.OK);
				return;
			}

			// Get coordinates again using invalid value as a default...
			pos = new Vector2D(positionx.GetResultFloat(VALUE_MISMATCH), positiony.GetResultFloat(VALUE_MISMATCH));

			// Skip when both fields have mixed values
			if(pos.x != VALUE_MISMATCH || pos.y != VALUE_MISMATCH)
			{
				// Make undo
				if(vertices.Count > 1) undodesc = vertices.Count + " vertices";
				General.Map.UndoRedo.CreateUndo("Edit " + undodesc);

				// Go for all vertices
				foreach(Vertex v in vertices)
				{
					// Apply position
					v.Move(new Vector2D((pos.x == VALUE_MISMATCH ? v.Position.x : pos.x),
										(pos.y == VALUE_MISMATCH ? v.Position.y : -pos.y)));
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