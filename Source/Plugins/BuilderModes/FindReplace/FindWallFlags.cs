
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
using System.Drawing;
using System.Windows.Forms;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	[FindReplace("Wall Flags", BrowseButton = true, Replacable = false)]
	internal class FindWallFlags : FindReplaceType
	{
		#region ================== Properties

		public override Image BrowseImage { get { return Properties.Resources.List; } }

		#endregion

		#region ================== Methods

		// This is called when the browse button is pressed
		public override string Browse(string initialvalue)
		{
			return FlagsForm.ShowDialog(Form.ActiveForm, initialvalue, General.Map.Config.WallFlags);
		}


		// This is called to perform a search (and replace)
		// Returns a list of items to show in the results list replacewith is null when not replacing
		public override FindReplaceObject[] Find(string value, bool withinselection, string replacewith, bool keepselection)
		{
			List<FindReplaceObject> objs = new List<FindReplaceObject>();

			// Where to search?
			ICollection<Linedef> list = (withinselection ? General.Map.Map.GetSelectedLinedefs(true) : General.Map.Map.Linedefs);

			//mxd. Prepare your flags...
			string[] flags = value.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
			for(int i = 0; i < flags.Length; i++) flags[i] = flags[i].Trim();

			// Go for all linedefs
			foreach(Linedef l in list)
			{
				if(l.Front != null && WallFlagsMatch(l.Front, flags))
				{
					objs.Add(new FindReplaceObject(l.Front, "Wall " + l.Front.Index));
				}
				
				if(l.Back != null && WallFlagsMatch(l.Back, flags))
				{
					objs.Add(new FindReplaceObject(l.Back, "Wall " + l.Back.Index));
				}
				
				//bool match = true;

				// Parse the value string...
				/*foreach(string flag in parts)
				{
					string str = s.Trim();

					// ... and check if the flags match
					if(General.Map.Config.WallFlags.ContainsKey(str) && !l.IsFlagSet(str))
					{
						match = false;
						break;
					}
				}*/

				// Flags matches?
				/*if(match)
				{
					// Add to list
					LinedefActionInfo info = General.Map.Config.GetLinedefActionInfo(l.Action);
					if(!info.IsNull)
						objs.Add(new FindReplaceObject(l, "Linedef " + l.Index + " (" + info.Title + ")"));
					else
						objs.Add(new FindReplaceObject(l, "Linedef " + l.Index));
				}*/
			}

			return objs.ToArray();
		}

		//mxd
		private bool WallFlagsMatch(Sidedef wall, IEnumerable<string> flags)
		{
			foreach(string flag in flags)
				if(General.Map.Config.WallFlags.ContainsKey(flag) && !wall.IsFlagSet(flag)) return false;

			return true;
		}

		// This is called when a specific object is selected from the list
		public override void ObjectSelected(FindReplaceObject[] selection)
		{
			if(selection.Length == 1)
			{
				ZoomToSelection(selection);
				General.Interface.ShowInfo(selection[0].Linedef);
			}
			else
				General.Interface.HideInfo();

			General.Map.Map.ClearAllSelected();
			foreach(FindReplaceObject obj in selection) obj.Linedef.Selected = true;
		}

		// Render selection
		public override void PlotSelection(IRenderer2D renderer, FindReplaceObject[] selection)
		{
			foreach(FindReplaceObject o in selection)
			{
				renderer.PlotLinedef(o.Linedef, General.Colors.Selection);
			}
		}

		// Edit objects
		public override void EditObjects(FindReplaceObject[] selection)
		{
			List<Sidedef> walls = new List<Sidedef>(selection.Length);
			foreach(FindReplaceObject o in selection) walls.Add(o.Sidedef);
			General.Interface.ShowEditWalls(walls);
		}

		#endregion
	}
}
