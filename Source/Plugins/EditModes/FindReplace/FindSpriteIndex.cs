
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

using System.Collections.Generic;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	[FindReplace("Sprite Index", BrowseButton = false, Replacable = false)]
	internal class FindSpriteIndex : FindReplaceType
	{
		#region ================== Constants

		#endregion

		#region ================== Variables

		#endregion

		#region ================== Properties

		public override Presentation RenderPresentation { get { return Presentation.Things; } }

		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public FindSpriteIndex()
		{
			// Initialize

		}

		// Destructor
		~FindSpriteIndex()
		{
		}

		#endregion

		#region ================== Methods

		// This is called when the browse button is pressed
		public override string Browse(string initialvalue)
		{
			return "";
		}


		// This is called to perform a search (and replace)
		// Returns a list of items to show in the results list
		// replacewith is null when not replacing
		public override FindReplaceObject[] Find(string value, bool withinselection, string replacewith, bool keepselection)
		{
			List<FindReplaceObject> objs = new List<FindReplaceObject>();

			// Interpret the number given
			int index;
			if(int.TryParse(value, out index))
			{
				Thing t = General.Map.Map.GetThingByIndex(index);
				if(t != null)
				{
					var ti = General.Map.Data.GetSpriteInfo(t.TileIndex);
					objs.Add(new FindReplaceObject(t, "Thing " + index + " (" + ti.Title + ")"));
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
		public override void  RenderThingsSelection(IRenderer2D renderer, FindReplaceObject[] selection)
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
