
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
using mxd.DukeBuilder.Actions;
using mxd.DukeBuilder.Editing;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.VisualModes;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	[EditMode(DisplayName = "Visual Mode",
			  SwitchAction = "visualmode",		// Action name used to switch to this mode
			  ButtonImage = "VisualMode.png",	// Image resource name for the button
			  ButtonOrder = 0,					// Position of the button (lower is more to the left)
			  ButtonGroup = "001_visual",
			  UseByDefault = true)]

	public class BaseVisualMode : VisualMode
	{
		#region ================== Constants
		
		// Object picking
		private const double PICK_INTERVAL = 80.0d;
		private const float PICK_RANGE = 0.98f;

		// Gravity
		private const float GRAVITY = -0.06f;
		
		#endregion
		
		#region ================== Variables

		// Gravity
		private Vector3D gravity;
		private float cameraflooroffset = 41f;		// same as in doom
		private float cameraceilingoffset = 10f;
		
		// Object picking
		private VisualPickResult target;
		private double lastpicktime;
		private bool locktarget;
		
		// This is true when a selection was made because the action is performed
		// on an object that was not selected. In this case the previous selection
		// is cleared and the targeted object is temporarely selected to perform
		// the action on. After the action is completed, the object is deselected.
		private bool singleselection;
		
		// We keep these to determine if we need to make a new undo level
		private bool selectionchanged;
		private UndoGroup lastundogroup;
		private VisualActionResult actionresult;
		private bool undocreated;

		// List of selected objects when an action is performed
		private List<IVisualEventReceiver> selectedobjects;
		
		#endregion
		
		#region ================== Properties

		public override object HighlightedObject
		{
			get
			{
				// Geometry picked?
				if(target.picked is VisualGeometry)
				{
					VisualGeometry pickedgeo = (target.picked as VisualGeometry);

					if(pickedgeo.Sidedef != null)
						return pickedgeo.Sidedef;
					else if(pickedgeo.Sector != null)
						return pickedgeo.Sector;
					else
						return null;
				}
				// Thing picked?
				else if(target.picked is VisualThing)
				{
					VisualThing pickedthing = (target.picked as VisualThing);
					return pickedthing.Thing;
				}
				else
				{
					return null;
				}
			}
		}

		//public IRenderer3D Renderer { get { return renderer; } }
		
		public bool IsSingleSelection { get { return singleselection; } }
		public bool SelectionChanged { get { return selectionchanged; } set { selectionchanged |= value; } }

		#endregion
		
		#region ================== Constructor / Disposer

		// Constructor
		public BaseVisualMode()
		{
			// Initialize
			this.gravity = new Vector3D(0.0f, 0.0f, 0.0f);
			this.selectedobjects = new List<IVisualEventReceiver>();
			
			// We have no destructor
			GC.SuppressFinalize(this);
		}

		// Disposer
		public override void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Clean up
				
				// Done
				base.Dispose();
			}
		}

		#endregion
		
		#region ================== Methods
		
		// This calculates brightness level
		/*internal int CalculateBrightness(int level)
		{
			return renderer.CalculateBrightness(level);
		}*/
		
		// This adds a selected object
		internal void AddSelectedObject(IVisualEventReceiver obj)
		{
			selectedobjects.Add(obj);
			selectionchanged = true;
		}
		
		// This removes a selected object
		internal void RemoveSelectedObject(IVisualEventReceiver obj)
		{
			selectedobjects.Remove(obj);
			selectionchanged = true;
		}
		
		// This is called before an action is performed
		public void PreAction(UndoGroup multiselectionundogroup)
		{
			actionresult = new VisualActionResult();
			
			PickTargetUnlocked();
			
			// If the action is not performed on a selected object, clear the
			// current selection and make a temporary selection for the target.
			if((target.picked != null) && !target.picked.Selected && (BuilderPlug.Me.VisualModeClearSelection || (selectedobjects.Count == 0)))
			{
				// Single object, no selection
				singleselection = true;
				ClearSelection();
				undocreated = false;
			}
			else
			{
				singleselection = false;
				
				// Check if we should make a new undo level
				// We don't want to do this if this is the same action with the same
				// selection and the action wants to group the undo levels
				if((lastundogroup != multiselectionundogroup) || (lastundogroup == UndoGroup.None) ||
				   (multiselectionundogroup == UndoGroup.None) || selectionchanged)
				{
					// We want to create a new undo level, but not just yet
					lastundogroup = multiselectionundogroup;
					undocreated = false;
				}
				else
				{
					// We don't want to make a new undo level (changes will be combined)
					undocreated = true;
				}
			}
		}

		// Called before an action is performed. This does not make an undo level
		private void PreActionNoChange()
		{
			actionresult = new VisualActionResult();
			singleselection = false;
			undocreated = false;
		}
		
		// This is called after an action is performed
		private void PostAction()
		{
			if(!string.IsNullOrEmpty(actionresult.displaystatus))
				General.Interface.DisplayStatus(StatusType.Action, actionresult.displaystatus);

			// Reset changed flags
			foreach(KeyValuePair<Sector, VisualSector> vs in allsectors)
			{
				BaseVisualSector bvs = (vs.Value as BaseVisualSector);
				bvs.Floor.Changed = false;
				bvs.Ceiling.Changed = false;
			}
			
			selectionchanged = false;
			
			if(singleselection) ClearSelection();
			
			UpdateChangedObjects();
			ShowTargetInfo();
		}
		
		// This sets the result for an action
		public void SetActionResult(VisualActionResult result)
		{
			actionresult = result;
		}

		// This sets the result for an action
		public void SetActionResult(string displaystatus)
		{
			actionresult = new VisualActionResult { displaystatus = displaystatus };
		}
		
		// This creates an undo, when only a single selection is made
		// When a multi-selection is made, the undo is created by the PreAction function
		public int CreateUndo(string description, UndoGroup group, int grouptag)
		{
			if(!undocreated)
			{
				undocreated = true;

				if(singleselection)
					return General.Map.UndoRedo.CreateUndo(description, this, (int)group, grouptag);
				else
					return General.Map.UndoRedo.CreateUndo(description, this, (int)UndoGroup.None, 0);
			}

			return 0;
		}

		// This creates an undo, when only a single selection is made
		// When a multi-selection is made, the undo is created by the PreAction function
		public int CreateUndo(string description)
		{
			return CreateUndo(description, UndoGroup.None, 0);
		}

		// This makes a list of the selected object
		private void RebuildSelectedObjectsList()
		{
			// Make list of selected objects
			selectedobjects = new List<IVisualEventReceiver>();
			foreach(KeyValuePair<Sector, VisualSector> vs in allsectors)
			{
				if(vs.Value != null)
				{
					BaseVisualSector bvs = (BaseVisualSector)vs.Value;
					if((bvs.Floor != null) && bvs.Floor.Selected) selectedobjects.Add(bvs.Floor);
					if((bvs.Ceiling != null) && bvs.Ceiling.Selected) selectedobjects.Add(bvs.Ceiling);
					foreach(Sidedef sd in vs.Key.Sidedefs)
					{
						List<VisualGeometry> sidedefgeos = bvs.GetSidedefGeometry(sd);
						foreach(VisualGeometry sdg in sidedefgeos)
						{
							if(sdg.Selected) selectedobjects.Add((sdg as IVisualEventReceiver));
						}
					}
				}
			}

			foreach(KeyValuePair<Thing, VisualThing> vt in allthings)
			{
				if(vt.Value != null)
				{
					BaseVisualThing bvt = (BaseVisualThing)vt.Value;
					if(bvt.Selected) selectedobjects.Add(bvt);
				}
			}
		}

		// This creates a visual sector
		protected override VisualSector CreateVisualSector(Sector s)
		{
			BaseVisualSector vs = new BaseVisualSector(this, s);
			return vs;
		}
		
		// This creates a visual thing
		protected override VisualThing CreateVisualThing(Thing t)
		{
			BaseVisualThing vt = new BaseVisualThing(this, t);
			return vt.Setup() ? vt : null;
		}

		// This locks the target so that it isn't changed until unlocked
		public void LockTarget()
		{
			locktarget = true;
		}
		
		// This unlocks the target so that is changes to the aimed geometry again
		public void UnlockTarget()
		{
			locktarget = false;
		}
		
		// This picks a new target, if not locked
		private void PickTargetUnlocked()
		{
			if(!locktarget) PickTarget();
		}
		
		// This picks a new target
		private void PickTarget()
		{
			// Find the object we are aiming at
			Vector3D start = General.Map.VisualCamera.Position;
			Vector3D delta = General.Map.VisualCamera.Target - General.Map.VisualCamera.Position;
			delta = delta.GetFixedLength(General.Settings.ViewDistance * PICK_RANGE);
			VisualPickResult newtarget = PickObject(start, start + delta);
			
			// Should we update the info on panels?
			bool updateinfo = (newtarget.picked != target.picked);
			
			// Apply new target
			target = newtarget;

			// Show target info
			if(updateinfo) ShowTargetInfo();
		}

		// This shows the picked target information
		public void ShowTargetInfo()
		{
			// Any result?
			if(target.picked != null)
			{
				// Geometry picked?
				if(target.picked is VisualGeometry)
				{
					VisualGeometry pickedgeo = (target.picked as VisualGeometry);

					if(pickedgeo.Sidedef != null)
						General.Interface.ShowInfo(pickedgeo.Sidedef);
					else if(pickedgeo.Sidedef == null)
						General.Interface.ShowInfo(pickedgeo.Sector.Sector);
					else
						General.Interface.HideInfo();
				}
				// Thing picked?
				if(target.picked is VisualThing)
				{
					VisualThing pickedthing = (target.picked as VisualThing);
					General.Interface.ShowInfo(pickedthing.Thing);
				}
			}
			else
			{
				General.Interface.HideInfo();
			}
		}
		
		// This updates the VisualSectors and VisualThings that have their Changed property set
		private void UpdateChangedObjects()
		{
			foreach(KeyValuePair<Sector, VisualSector> vs in allsectors)
			{
				if(vs.Value != null)
				{
					BaseVisualSector bvs = (BaseVisualSector)vs.Value;
					if(bvs.Changed) bvs.Rebuild();
				}
			}

			foreach(KeyValuePair<Thing, VisualThing> vt in allthings)
			{
				if(vt.Value != null)
				{
					BaseVisualThing bvt = (BaseVisualThing)vt.Value;
					if(bvt.Changed) bvt.Rebuild();
				}
			}
		}
		
		#endregion
		
		#region ================== Events
		
		// Help!
		public override void OnHelp()
		{
			General.ShowHelp("e_visual.html");
		}

		// When entering this mode
		public override void OnEngage()
		{
			base.OnEngage();
			
			// Read settings
			cameraflooroffset = General.Map.Config.ReadSetting("cameraflooroffset", cameraflooroffset);
			cameraceilingoffset = General.Map.Config.ReadSetting("cameraceilingoffset", cameraceilingoffset);
		}

		// When returning to another mode
		public override void OnDisengage()
		{
			base.OnDisengage();
			General.Map.Map.Update();
		}
		
		// Processing
		public override void OnProcess(double deltatime)
		{
			// Process things?
			base.ProcessThings = (BuilderPlug.Me.ShowVisualThings != 0);
			
			// Setup the move multiplier depending on gravity
			Vector3D movemultiplier = new Vector3D(1.0f, 1.0f, 1.0f);
			if(BuilderPlug.Me.UseGravity) movemultiplier.z = 0.0f;
			General.Map.VisualCamera.MoveMultiplier = movemultiplier;
			
			// Apply gravity?
			if(BuilderPlug.Me.UseGravity && (General.Map.VisualCamera.Sector != null))
			{
				// Camera below floor level?
				if(General.Map.VisualCamera.Position.z <= (General.Map.VisualCamera.Sector.FloorHeight + cameraflooroffset + 0.1f))
				{
					// Stay above floor
					gravity = new Vector3D(0.0f, 0.0f, 0.0f);
					General.Map.VisualCamera.Position = new Vector3D(General.Map.VisualCamera.Position.x,
																	 General.Map.VisualCamera.Position.y,
																	 General.Map.VisualCamera.Sector.FloorHeight + cameraflooroffset);
				}
				else
				{
					// Fall down
					gravity += new Vector3D(0.0f, 0.0f, (float)(GRAVITY * deltatime));
					General.Map.VisualCamera.Position += gravity;
				}
				
				// Camera above ceiling level?
				if(General.Map.VisualCamera.Position.z >= (General.Map.VisualCamera.Sector.CeilingHeight - cameraceilingoffset - 0.1f))
				{
					// Stay below ceiling
					General.Map.VisualCamera.Position = new Vector3D(General.Map.VisualCamera.Position.x,
																	 General.Map.VisualCamera.Position.y,
																	 General.Map.VisualCamera.Sector.CeilingHeight - cameraceilingoffset);
				}
			}
			else
			{
				gravity = new Vector3D(0.0f, 0.0f, 0.0f);
			}
			
			// Do processing
			base.OnProcess(deltatime);

			// Process visible geometry
			foreach(IVisualEventReceiver g in visiblegeometry)
			{
				g.OnProcess(deltatime);
			}
			
			// Time to pick a new target?
			if(General.Clock.CurrentTime > (lastpicktime + PICK_INTERVAL))
			{
				PickTargetUnlocked();
				lastpicktime = General.Clock.CurrentTime;
			}
			
			// The mouse is always in motion
			MouseEventArgs args = new MouseEventArgs(General.Interface.MouseButtons, 0, 0, 0, 0);
			OnMouseMove(args);
		}
		
		// This draws a frame
		public override void OnRedrawDisplay()
		{
			// Start drawing
			if(renderer.Start())
			{
				// Use fog!
				renderer.SetFogMode(true);

				// Set target for highlighting
				if(BuilderPlug.Me.UseHighlight)
					renderer.SetHighlightedObject(target.picked);
				
				// Begin with geometry
				renderer.StartGeometry();

				// Render all visible sectors
				foreach(VisualGeometry g in visiblegeometry)
					renderer.AddSectorGeometry(g);

				if(BuilderPlug.Me.ShowVisualThings != 0)
				{
					// Render things in cages?
					renderer.DrawThingCages = ((BuilderPlug.Me.ShowVisualThings & 2) != 0);
					
					// Render all visible things
					foreach(VisualThing t in visiblethings)
						renderer.AddThingGeometry(t);
				}
				
				// Done rendering geometry
				renderer.FinishGeometry();
				
				// Render crosshair
				renderer.RenderCrosshair();
				
				// Present!
				renderer.Finish();
			}
		}
		
		// After resources were reloaded
		protected override void ResourcesReloaded()
		{
			base.ResourcesReloaded();
			PickTarget();
		}
		
		// This usually happens when geometry is changed by undo, redo, cut or paste actions
		// and uses the marks to check what needs to be reloaded.
		protected override void ResourcesReloadedPartial()
		{
			bool sectorsmarked = false;
			
			if(General.Map.UndoRedo.GeometryChanged)
			{
				// Let the core do this (it will just dispose the sectors that were changed)
				base.ResourcesReloadedPartial();
			}
			else
			{
				// Neighbour sectors must be updated as well
				foreach(Sector s in General.Map.Map.Sectors)
				{
					if(s.Marked)
					{
						sectorsmarked = true;
						foreach(Sidedef sd in s.Sidedefs)
						{
							sd.Marked = true;
							if(sd.Other != null) sd.Other.Marked = true;
						}
					}
				}
				
				// Go for all sidedefs to update
				foreach(Sidedef sd in General.Map.Map.Sidedefs)
				{
					if(sd.Marked && VisualSectorExists(sd.Sector))
					{
						BaseVisualSector vs = (BaseVisualSector)GetVisualSector(sd.Sector);
						VisualWallParts parts = vs.GetSidedefParts(sd);
						parts.SetupAllParts();
					}
				}
				
				// Go for all sectors to update
				foreach(Sector s in General.Map.Map.Sectors)
				{
					if(s.Marked && VisualSectorExists(s))
					{
						BaseVisualSector vs = (BaseVisualSector)GetVisualSector(s);
						vs.Floor.Setup();
						vs.Ceiling.Setup();
					}
				}
				
				if(!sectorsmarked)
				{
					// No sectors or geometry changed. So we only have
					// to update things when they have changed.
					foreach(KeyValuePair<Thing, VisualThing> vt in allthings)
						if((vt.Value != null) && vt.Key.Marked) (vt.Value as BaseVisualThing).Rebuild();
				}
				else
				{
					// Things depend on the sector they are in and because we can't
					// easily determine which ones changed, we dispose all things
					foreach(KeyValuePair<Thing, VisualThing> vt in allthings)
						if(vt.Value != null) vt.Value.Dispose();
					
					// Apply new lists
					allthings = new Dictionary<Thing, VisualThing>(allthings.Count);
				}
				
				// Clear visibility collections
				visiblesectors.Clear();
				visibleblocks.Clear();
				visiblegeometry.Clear();
				visiblethings.Clear();
				
				// Make new blockmap
				if(sectorsmarked || General.Map.UndoRedo.PopulationChanged)
					FillBlockMap();
				
				// Visibility culling (this re-creates the needed resources)
				DoCulling();
			}
			
			// Determine what we're aiming at now
			PickTarget();
		}
		
		// Mouse moves
		public override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			GetTargetEventReceiver(true).OnMouseMove(e);
		}
		
		// Undo performed
		public override void OnUndoEnd()
		{
			base.OnUndoEnd();
			RebuildSelectedObjectsList();
			
			// We can't group with this undo level anymore
			lastundogroup = UndoGroup.None;
		}
		
		// Redo performed
		public override void OnRedoEnd()
		{
			base.OnRedoEnd();
			RebuildSelectedObjectsList();
		}
		
		#endregion

		#region ================== Action Assist

		// Because some actions can only be called on a single (the targeted) object because
		// they show a dialog window or something, these functions help applying the result
		// to all compatible selected objects.
		
		// Apply texture offsets
		public void ApplyImageOffsetChange(int dx, int dy)
		{
			Dictionary<Sidedef, int> donesides = new Dictionary<Sidedef, int>(selectedobjects.Count);
			List<IVisualEventReceiver> objs = GetSelectedObjects(false, true, false);
			foreach(IVisualEventReceiver i in objs)
			{
				if(i is BaseVisualGeometrySidedef)
				{
					if(!donesides.ContainsKey((i as BaseVisualGeometrySidedef).Sidedef))
					{
						i.OnChangeImageOffset(dx, dy);
						donesides.Add((i as BaseVisualGeometrySidedef).Sidedef, 0);
					}
				}
			}
		}

		// Apply lower unpegged flag
		public void ApplyBottomAlignment(bool set)
		{
			foreach(IVisualEventReceiver i in GetSelectedObjects(true, true, true))
				i.ApplyBottomAlignment(set);
		}

		// Apply texture change
		public void ApplySelectImage(int tileindex)
		{
			// Apply on all compatible types
			foreach(IVisualEventReceiver i in GetSelectedObjects(true, true, false))
				i.ApplyImage(tileindex);
		}

		// This returns all selected objects
		internal List<IVisualEventReceiver> GetSelectedObjects(bool includesectors, bool includesidedefs, bool includethings)
		{
			List<IVisualEventReceiver> objs = new List<IVisualEventReceiver>();
			foreach(IVisualEventReceiver i in selectedobjects)
			{
				if((i is BaseVisualGeometrySector) && includesectors) objs.Add(i);
				else if((i is BaseVisualGeometrySidedef) && includesidedefs) objs.Add(i);
				else if((i is BaseVisualThing) && includethings) objs.Add(i);
			}

			// Add highlight?
			if(selectedobjects.Count == 0)
			{
				IVisualEventReceiver i = (target.picked as IVisualEventReceiver);
				if((i is BaseVisualGeometrySector) && includesectors) objs.Add(i);
				else if((i is BaseVisualGeometrySidedef) && includesidedefs) objs.Add(i);
				else if((i is BaseVisualThing) && includethings) objs.Add(i);
			}

			return objs;
		}

		// This returns all selected sectors, no doubles
		public List<Sector> GetSelectedSectors()
		{
			HashSet<Sector> added = new HashSet<Sector>();
			List<Sector> sectors = new List<Sector>();
			foreach(IVisualEventReceiver i in selectedobjects)
			{
				if(i is BaseVisualGeometrySector)
				{
					Sector s = (i as BaseVisualGeometrySector).Sector.Sector;
					if(!added.Contains(s))
					{
						sectors.Add(s);
						added.Add(s);
					}
				}
			}

			// Add highlight?
			if((selectedobjects.Count == 0) && (target.picked is BaseVisualGeometrySector))
			{
				Sector s = (target.picked as BaseVisualGeometrySector).Sector.Sector;
				if(!added.Contains(s)) sectors.Add(s);
			}
			
			return sectors;
		}

		// This returns all selected walls, no doubles
		public List<Linedef> GetSelectedLinedefs()
		{
			HashSet<Linedef> added = new HashSet<Linedef>();
			List<Linedef> linedefs = new List<Linedef>();
			foreach(IVisualEventReceiver i in selectedobjects)
			{
				if(i is BaseVisualGeometrySidedef)
				{
					Linedef l = (i as BaseVisualGeometrySidedef).Sidedef.Line;
					if(!added.Contains(l))
					{
						linedefs.Add(l);
						added.Add(l);
					}
				}
			}

			// Add highlight?
			if((selectedobjects.Count == 0) && (target.picked is BaseVisualGeometrySidedef))
			{
				Linedef l = (target.picked as BaseVisualGeometrySidedef).Sidedef.Line;
				if(!added.Contains(l)) linedefs.Add(l);
			}

			return linedefs;
		}

		// This returns all selected sidedefs, no doubles
		public List<Sidedef> GetSelectedWalls()
		{
			HashSet<Sidedef> added = new HashSet<Sidedef>();
			List<Sidedef> sidedefs = new List<Sidedef>();
			foreach(IVisualEventReceiver i in selectedobjects)
			{
				if(i is BaseVisualGeometrySidedef)
				{
					Sidedef sd = (i as BaseVisualGeometrySidedef).Sidedef;
					if(!added.Contains(sd))
					{
						sidedefs.Add(sd);
						added.Add(sd);
					}
				}
			}

			// Add highlight?
			if((selectedobjects.Count == 0) && (target.picked is BaseVisualGeometrySidedef))
			{
				Sidedef sd = (target.picked as BaseVisualGeometrySidedef).Sidedef;
				if(!added.Contains(sd)) sidedefs.Add(sd);
			}

			return sidedefs;
		}

		// This returns all selected things, no doubles
		public List<Thing> GetSelectedSprites()
		{
			HashSet<Thing> added = new HashSet<Thing>();
			List<Thing> sprites = new List<Thing>();
			foreach(IVisualEventReceiver i in selectedobjects)
			{
				if(i is BaseVisualThing)
				{
					Thing t = (i as BaseVisualThing).Thing;
					if(!added.Contains(t))
					{
						sprites.Add(t);
						added.Add(t);
					}
				}
			}

			// Add highlight?
			if((selectedobjects.Count == 0) && (target.picked is BaseVisualThing))
			{
				Thing t = (target.picked as BaseVisualThing).Thing;
				if(!added.Contains(t)) sprites.Add(t);
			}

			return sprites;
		}
		
		// This returns the IVisualEventReceiver on which the action must be performed
		private IVisualEventReceiver GetTargetEventReceiver(bool targetonly)
		{
			if(target.picked == null) return new NullVisualEventReceiver();
			if(singleselection || target.picked.Selected || targetonly) return (IVisualEventReceiver)target.picked;
			if(selectedobjects.Count > 0) return selectedobjects[0];
			return (IVisualEventReceiver)target.picked;
		}
		
		#endregion

		#region ================== Actions

		[BeginAction("clearselection", BaseAction = true)]
		public void ClearSelection()
		{
			selectedobjects = new List<IVisualEventReceiver>();
			
			foreach(KeyValuePair<Sector, VisualSector> vs in allsectors)
			{
				if(vs.Value != null)
				{
					BaseVisualSector bvs = (BaseVisualSector)vs.Value;
					if(bvs.Floor != null) bvs.Floor.Selected = false;
					if(bvs.Ceiling != null) bvs.Ceiling.Selected = false;
					foreach(Sidedef sd in vs.Key.Sidedefs)
					{
						List<VisualGeometry> sidedefgeos = bvs.GetSidedefGeometry(sd);
						foreach(VisualGeometry sdg in sidedefgeos)
						{
							sdg.Selected = false;
						}
					}
				}
			}

			foreach(KeyValuePair<Thing, VisualThing> vt in allthings)
			{
				if(vt.Value != null)
				{
					BaseVisualThing bvt = (BaseVisualThing)vt.Value;
					bvt.Selected = false;
				}
			}
		}

		[BeginAction("visualselect", BaseAction = true)]
		public void BeginSelect()
		{
			PreActionNoChange();
			PickTargetUnlocked();
			GetTargetEventReceiver(true).OnSelectBegin();
			PostAction();
		}

		[EndAction("visualselect", BaseAction = true)]
		public void EndSelect()
		{
			//PreActionNoChange();
			GetTargetEventReceiver(true).OnSelectEnd();
			Renderer.ShowSelection = true;
			Renderer.ShowHighlight = true;
			PostAction();
		}

		[BeginAction("visualedit", BaseAction = true)]
		public void BeginEdit()
		{
			PreAction(UndoGroup.None);
			GetTargetEventReceiver(false).OnEditBegin();
			PostAction();
		}

		[EndAction("visualedit", BaseAction = true)]
		public void EndEdit()
		{
			PreActionNoChange();
			GetTargetEventReceiver(false).OnEditEnd();
			PostAction();
		}

		[BeginAction("raisesector8")]
		public void RaiseSector8()
		{
			PreAction(UndoGroup.SectorHeightChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeTargetHeight(64);
			PostAction();
		}

		[BeginAction("lowersector8")]
		public void LowerSector8()
		{
			PreAction(UndoGroup.SectorHeightChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeTargetHeight(-64);
			PostAction();
		}

		[BeginAction("raisesector1")]
		public void RaiseSector1()
		{
			PreAction(UndoGroup.SectorHeightChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeTargetHeight(8);
			PostAction();
		}
		
		[BeginAction("lowersector1")]
		public void LowerSector1()
		{
			PreAction(UndoGroup.SectorHeightChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeTargetHeight(-8);
			PostAction();
		}

		[BeginAction("showvisualthings")]
		public void ShowVisualThings()
		{
			BuilderPlug.Me.ShowVisualThings++;
			if(BuilderPlug.Me.ShowVisualThings > 2) BuilderPlug.Me.ShowVisualThings = 0;
		}

		[BeginAction("raisebrightness1")]
		public void RaiseBrightness8()
		{
			PreAction(UndoGroup.ShadeChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeTargetShade(false);
			PostAction();
		}

		[BeginAction("lowerbrightness1")]
		public void LowerBrightness8()
		{
			PreAction(UndoGroup.ShadeChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeTargetShade(true);
			PostAction();
		}

		[BeginAction("increaseangle")] //mxd
		public void IncreaseAngle() { ChangeAngle(5); }

		[BeginAction("decreaseangle")] //mxd
		public void DecreaseAngle() { ChangeAngle(-5); }

		//mxd
		private void ChangeAngle(int increment)
		{
			PreAction(UndoGroup.AngleChange);
			float incrementrad = Angle2D.DegToRad(increment);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, false, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeTargetAngle(incrementrad);
			PostAction();
		}

		[BeginAction("moveimageleft")]
		public void MoveImageLeft1()
		{
			PreAction(UndoGroup.ImageOffsetChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeImageOffset(-1, 0);
			PostAction();
		}

		[BeginAction("moveimageright")]
		public void MoveImageRight1()
		{
			PreAction(UndoGroup.ImageOffsetChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeImageOffset(1, 0);
			PostAction();
		}

		[BeginAction("moveimageup")]
		public void MoveImageUp1()
		{
			PreAction(UndoGroup.ImageOffsetChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeImageOffset(0, -1);
			PostAction();
		}

		[BeginAction("moveimagedown")]
		public void MoveImageDown1()
		{
			PreAction(UndoGroup.ImageOffsetChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeImageOffset(0, 1);
			PostAction();
		}

		[BeginAction("moveimageleft8")]
		public void MoveImageLeft8()
		{
			PreAction(UndoGroup.ImageOffsetChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeImageOffset(-8, 0);
			PostAction();
		}

		[BeginAction("moveimageright8")]
		public void MoveImageRight8()
		{
			PreAction(UndoGroup.ImageOffsetChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeImageOffset(8, 0);
			PostAction();
		}

		[BeginAction("moveimageup8")]
		public void MoveImageUp8()
		{
			PreAction(UndoGroup.ImageOffsetChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeImageOffset(0, -8);
			PostAction();
		}

		[BeginAction("moveimagedown8")]
		public void MoveImageDown8()
		{
			PreAction(UndoGroup.ImageOffsetChange);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnChangeImageOffset(0, 8);
			PostAction();
		}

		[BeginAction("selectimage")]
		public void TextureSelect()
		{
			PreAction(UndoGroup.None);
			renderer.SetCrosshairBusy(true);
			General.Interface.RedrawDisplay();
			GetTargetEventReceiver(false).OnSelectImage();
			UpdateChangedObjects();
			renderer.SetCrosshairBusy(false);
			PostAction();
		}

		[BeginAction("copyimage")]
		public void CopyImage()
		{
			PreActionNoChange();
			GetTargetEventReceiver(false).OnCopyImage();
			PostAction();
		}

		[BeginAction("pasteimage")]
		public void PasteImage()
		{
			PreAction(UndoGroup.None);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnPasteImage();
			PostAction();
		}

		[BeginAction("visualautoalignx")]
		public void TextureAutoAlignX()
		{
			PreAction(UndoGroup.None);
			renderer.SetCrosshairBusy(true);
			General.Interface.RedrawDisplay();
			GetTargetEventReceiver(false).OnImageAlign(true, false);
			UpdateChangedObjects();
			renderer.SetCrosshairBusy(false);
			PostAction();
		}

		[BeginAction("visualautoaligny")]
		public void TextureAutoAlignY()
		{
			PreAction(UndoGroup.None);
			renderer.SetCrosshairBusy(true);
			General.Interface.RedrawDisplay();
			GetTargetEventReceiver(false).OnImageAlign(false, true);
			UpdateChangedObjects();
			renderer.SetCrosshairBusy(false);
			PostAction();
		}

		/*[BeginAction("toggleupperunpegged")]
		public void ToggleUpperUnpegged()
		{
			PreAction(UndoGroup.None);
			GetTargetEventReceiver(false).OnToggleUpperUnpegged();
			PostAction();
		}*/

		[BeginAction("togglebottomalignment")]
		public void ToggleBottomAlignment()
		{
			PreAction(UndoGroup.None);
			GetTargetEventReceiver(false).OnToggleBottomAlignment();
			PostAction();
		}

		[BeginAction("togglegravity")]
		public void ToggleGravity()
		{
			BuilderPlug.Me.UseGravity = !BuilderPlug.Me.UseGravity;
			string onoff = BuilderPlug.Me.UseGravity ? "ON" : "OFF";
			General.Interface.DisplayStatus(StatusType.Action, "Gravity is now " + onoff + ".");
		}

		[BeginAction("togglebrightness")]
		public void ToggleBrightness()
		{
			renderer.FullBrightness = !renderer.FullBrightness;
			string onoff = renderer.FullBrightness ? "ON" : "OFF";
			General.Interface.DisplayStatus(StatusType.Action, "Full Brightness is now " + onoff + ".");
		}
		
		[BeginAction("togglehighlight")]
		public void ToggleHighlight()
		{
			BuilderPlug.Me.UseHighlight = !BuilderPlug.Me.UseHighlight;
			string onoff = BuilderPlug.Me.UseHighlight ? "ON" : "OFF";
			General.Interface.DisplayStatus(StatusType.Action, "Highlight is now " + onoff + ".");
		}
		
		[BeginAction("resetimageoffsets")]
		public void ResetImageOffsets()
		{
			PreAction(UndoGroup.None);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnResetImageOffsets();
			PostAction();
		}

		[BeginAction("floodfillimages")]
		public void FloodfillImages()
		{
			PreAction(UndoGroup.None);
			GetTargetEventReceiver(false).OnImageFloodfill();
			PostAction();
		}

		[BeginAction("copyimageoffsets")]
		public void CopyImageOffsets()
		{
			PreActionNoChange();
			GetTargetEventReceiver(false).OnCopyImageOffsets();
			PostAction();
		}

		[BeginAction("pasteimageoffsets")]
		public void PasteImageOffsets()
		{
			PreAction(UndoGroup.None);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnPasteImageOffsets();
			PostAction();
		}

		[BeginAction("copyproperties")]
		public void CopyProperties()
		{
			PreActionNoChange();
			GetTargetEventReceiver(false).OnCopyProperties();
			PostAction();
		}

		[BeginAction("pasteproperties")]
		public void PasteProperties()
		{
			PreAction(UndoGroup.None);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnPasteProperties();
			PostAction();
		}
		
		[BeginAction("insertitem", BaseAction = true)]
		public void Insert()
		{
			PreAction(UndoGroup.None);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnInsert();
			PostAction();
		}

		[BeginAction("deleteitem", BaseAction = true)]
		public void Delete()
		{
			PreAction(UndoGroup.None);
			List<IVisualEventReceiver> objs = GetSelectedObjects(true, true, true);
			foreach(IVisualEventReceiver i in objs) i.OnDelete();
			PostAction();
		}
		
		#endregion
	}
}
