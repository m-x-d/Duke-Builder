
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
using mxd.DukeBuilder.Windows;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;
using System.Drawing;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	//TODO: add FindSectorFlags, FindFloorFlags and FindCeilingFlags...
	[FindReplace("Sprite Flags", BrowseButton = true, Replacable = false)]
	internal class FindSpriteFlags : FindReplaceType
	{
		#region ================== Constants

		#endregion

		#region ================== Variables

		#endregion

		#region ================== Properties

		public override Presentation RenderPresentation { get { return Presentation.Things; } }
		public override Image BrowseImage { get { return Properties.Resources.List; } }

		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public FindSpriteFlags()
		{
			// Initialize

		}

		// Destructor
		~FindSpriteFlags()
		{
		}

		#endregion

		#region ================== Methods

		// This is called when the browse button is pressed
		public override string Browse(string initialvalue)
		{
			return FlagsForm.ShowDialog(Form.ActiveForm, initialvalue, General.Map.Config.SpriteFlags);
		}


		// This is called to perform a search (and replace)
		// Returns a list of items to show in the results list
		// replacewith is null when not replacing
		public override FindReplaceObject[] Find(string value, bool withinselection, string replacewith, bool keepselection)
		{
			List<FindReplaceObject> objs = new List<FindReplaceObject>();

			// Where to search?
			ICollection<Thing> list = (withinselection ? General.Map.Map.GetSelectedThings(true) : General.Map.Map.Things);

			//mxd. Prepare your flags...
			string[] flags = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			for(int i = 0; i < flags.Length; i++) flags[i] = flags[i].Trim();

			// Go for all things
			foreach(Thing t in list)
			{
				bool match = true;

				// Parse the value string...
				foreach(string flag in flags)
				{
					// ... and check if the flags don't match
					if(General.Map.Config.SpriteFlags.ContainsKey(flag) && !t.IsFlagSet(flag))
					{
						match = false;
						break;
					}
				}

				// Match?
				if(match)
				{
					// Add to list
					var ti = General.Map.Data.GetSpriteInfo(t.TileIndex);
					objs.Add(new FindReplaceObject(t, "Sprite " + t.Index + " (" + ti.Title + ")"));
				}
			}

			return objs.ToArray();
		}

		// This is called when a specific object is selected from the list
		public override void ObjectSelected(FindReplaceObject[] selection)
		{
			if(selection.Length == 1)
			{
				ZoomToSelection(selection);
				General.Interface.ShowInfo(selection[0].Thing);
			}
			else
				General.Interface.HideInfo();

			General.Map.Map.ClearAllSelected();
			foreach(FindReplaceObject obj in selection) obj.Thing.Selected = true;
		}

		// Render selection
		public override void RenderThingsSelection(IRenderer2D renderer, FindReplaceObject[] selection)
		{
			foreach(FindReplaceObject o in selection)
			{
				renderer.RenderThing(o.Thing, General.Colors.Selection, 1.0f);
			}
		}

		// Edit objects
		public override void EditObjects(FindReplaceObject[] selection)
		{
			List<Thing> things = new List<Thing>(selection.Length);
			foreach(FindReplaceObject o in selection) things.Add(o.Thing);
			General.Interface.ShowEditSprites(things);
		}

		#endregion
	}
}
