
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
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	internal abstract class BaseVisualGeometrySidedef : VisualGeometry, IVisualEventReceiver
	{
		#region ================== Constants
		
		private const float DRAG_ANGLE_TOLERANCE = 0.06f;
		
		#endregion

		#region ================== Variables

		protected BaseVisualMode mode;

		protected int setuponloadedimage = -1;
		
		// UV dragging
		private float dragstartanglexy;
		private float dragstartanglez;
		private Vector3D dragorigin;
		private Vector3D deltaxy;
		private Vector3D deltaz;
		private int startoffsetx;
		private int startoffsety;
		protected bool uvdragging;
		private int prevoffsetx;		// We have to provide delta offsets, but I don't
		private int prevoffsety;		// want to calculate with delta offsets to prevent
										// inaccuracy in the dragging.
		// Undo/redo
		private int undoticket;
		
		#endregion
		
		#region ================== Properties
		
		public bool IsDraggingUV { get { return uvdragging; } }
		new public BaseVisualSector Sector { get { return (BaseVisualSector)base.Sector; } }
		
		#endregion
		
		#region ================== Constructor / Destructor
		
		// Constructor for sidedefs
		public BaseVisualGeometrySidedef(BaseVisualMode mode, VisualSector vs, Sidedef sd) : base(vs, sd)
		{
			this.mode = mode;
			this.deltaz = new Vector3D(0.0f, 0.0f, 1.0f);
			this.deltaxy = (sd.Line.End.Position - sd.Line.Start.Position) * sd.Line.LengthInv;
			if(!sd.IsFront) this.deltaxy = -this.deltaxy;
		}
		
		#endregion

		#region ================== Methods

		// This performs a fast test in object picking
		public override bool PickFastReject(Vector3D from, Vector3D to, Vector3D dir)
		{
			// Check if intersection point is between top and bottom
			return (pickintersect.z >= Sidedef.Sector.FloorPlane.GetZ(pickintersect)) && (pickintersect.z <= Sidedef.Sector.CeilingPlane.GetZ(pickintersect));
		}

		// This performs an accurate test for object picking
		public override bool PickAccurate(Vector3D from, Vector3D to, Vector3D dir, ref float u_ray)
		{
			// The fast reject pass is already as accurate as it gets,
			// so we just return the intersection distance here
			u_ray = pickrayu;
			return true;
		}
		
		#endregion

		#region ================== Events
		
		// Unused
		public virtual void OnEditBegin() { }
		public virtual void OnChangeTargetAngle(float amount) { } //mxd
		protected virtual void SetTexture(int tileindex) { }
		public abstract bool Setup();
		
		// Insert middle texture
		public virtual void OnInsert()
		{
			// No middle texture yet?
			/*if(!Sidedef.MiddleRequired() && (string.IsNullOrEmpty(Sidedef.MiddleTexture) || (Sidedef.MiddleTexture[0] == '-')))
			{
				// Make it now
				mode.CreateUndo("Create middle texture");
				mode.SetActionResult("Created middle texture.");
				General.Map.Config.FindDefaultDrawSettings();
				Sidedef.SetTextureMid(General.Settings.DefaultTexture);

				// Update
				Sector.Changed = true;
				
				// Other side as well
				if(string.IsNullOrEmpty(Sidedef.Other.MiddleTexture) || (Sidedef.Other.MiddleTexture[0] == '-'))
				{
					Sidedef.Other.SetTextureMid(General.Settings.DefaultTexture);

					// Update
					VisualSector othersector = mode.GetVisualSector(Sidedef.Other.Sector);
					if(othersector is BaseVisualSector) (othersector as BaseVisualSector).Changed = true;
				}
			}*/
		}

		// Delete texture
		public virtual void OnDelete()
		{
			// Remove texture
			/*mode.CreateUndo("Delete image");
			mode.SetActionResult("Deleted a texture.");
			SetTexture("-");

			// Update
			Sector.Changed = true;*/
		}
		
		// Processing
		public virtual void OnProcess(double deltatime)
		{
			// If the texture was not loaded, but is loaded now, then re-setup geometry
			if(setuponloadedimage > -1)
			{
				ImageData t = General.Map.Data.GetImageData(setuponloadedimage);
				if(t != null && t.IsImageLoaded)
				{
					setuponloadedimage = -1;
					Setup();
				}
			}
		}
		
		// Change target height
		public virtual void OnChangeTargetHeight(int amount)
		{
			switch(BuilderPlug.Me.ChangeHeightBySidedef)
			{
				// Change ceiling
				case 1:
					if(!this.Sector.Ceiling.Changed)
						this.Sector.Ceiling.OnChangeTargetHeight(amount);
					break;

				// Change floor
				case 2:
					if(!this.Sector.Floor.Changed)
						this.Sector.Floor.OnChangeTargetHeight(amount);
					break;

				// Change both
				case 3:
					if(!this.Sector.Floor.Changed)
						this.Sector.Floor.OnChangeTargetHeight(amount);
					if(!this.Sector.Ceiling.Changed)
						this.Sector.Ceiling.OnChangeTargetHeight(amount);
					break;
			}
		}
		
		// Reset texture offsets
		public virtual void OnResetImageOffsets()
		{
			mode.CreateUndo("Reset image offsets");
			mode.SetActionResult("Texture image reset.");

			// Apply offsets
			Sidedef.OffsetX = 0;
			Sidedef.OffsetY = 0;

			// Update sidedef geometry
			Sector.GetSidedefParts(Sidedef).SetupAllParts();
		}

		// Toggle lower-unpegged
		public virtual void OnToggleBottomAlignment()
		{
			mode.ApplyBottomAlignment(!this.Sidedef.IsFlagSet(General.Map.FormatInterface.WallAlignImageToBottomFlag));
		}

		// This sets the Lower Unpegged flag
		public virtual void ApplyBottomAlignment(bool set)
		{
			if(!set)
			{
				// Remove flag
				mode.CreateUndo("Remove bottom-aligned setting");
				mode.SetActionResult("Removed bottom-aligned setting.");
				Sidedef.SetFlag(General.Map.FormatInterface.WallAlignImageToBottomFlag, false);
			}
			else
			{
				// Add flag
				mode.CreateUndo("Set bottom-aligned setting");
				mode.SetActionResult("Set bottom-aligned setting.");
				Sidedef.SetFlag(General.Map.FormatInterface.WallAlignImageToBottomFlag, true);
			}

			// Update sidedef geometry
			VisualWallParts parts = Sector.GetSidedefParts(Sidedef);
			parts.SetupAllParts();

			// Update other sidedef geometry
			/*if(Sidedef.Other != null)
			{
				BaseVisualSector othersector = (BaseVisualSector)mode.GetVisualSector(Sidedef.Other.Sector);
				parts = othersector.GetSidedefParts(Sidedef.Other);
				parts.SetupAllParts();
			}*/
		}

		// Flood-fill textures
		public virtual void OnImageFloodfill()
		{
			if(BuilderPlug.Me.CopiedImageIndex <= -1) return;

			int oldtile = GetImageIndex();
			int newtile = BuilderPlug.Me.CopiedImageIndex;
			if(newtile == oldtile) return;

			mode.CreateUndo("Flood-fill images with image " + newtile);
			mode.SetActionResult("Flood-filled images with " + newtile + ".");
					
			mode.Renderer.SetCrosshairBusy(true);
			General.Interface.RedrawDisplay();

			// Get the image
			ImageData newtextureimage = General.Map.Data.GetImageData(newtile);
			if(newtextureimage == null) return;

			if(mode.IsSingleSelection)
			{
				// Clear all marks, this will align everything it can
				General.Map.Map.ClearMarkedSidedefs(false);
			}
			else
			{
				// Limit the alignment to selection only
				General.Map.Map.ClearMarkedSidedefs(true);
				List<Sidedef> sides = mode.GetSelectedWalls();
				foreach(Sidedef sd in sides) sd.Marked = false;
			}
						
			// Do the alignment
			Tools.FloodfillWallImages(this.Sidedef, oldtile, newtile, false);

			// Get the changed sidedefs
			List<Sidedef> changes = General.Map.Map.GetMarkedSidedefs(true);
			foreach(Sidedef sd in changes)
			{
				// Update the parts for this sidedef!
				if(mode.VisualSectorExists(sd.Sector))
				{
					BaseVisualSector vs = (mode.GetVisualSector(sd.Sector) as BaseVisualSector);
					VisualWallParts parts = vs.GetSidedefParts(sd);
					parts.SetupAllParts();
				}
			}

			General.Map.Data.UpdateUsedImages();
			mode.Renderer.SetCrosshairBusy(false);
			mode.ShowTargetInfo();
		}
		
		// Auto-align texture X offsets
		public virtual void OnImageAlign(bool alignx, bool aligny)
		{
			mode.CreateUndo("Auto-align images");
			mode.SetActionResult("Auto-aligned images.");
			
			// Make sure the texture is loaded (we need the texture size)
			if(!base.Texture.IsImageLoaded) base.Texture.LoadImage();
			
			if(mode.IsSingleSelection)
			{
				// Clear all marks, this will align everything it can
				General.Map.Map.ClearMarkedSidedefs(false);
			}
			else
			{
				// Limit the alignment to selection only
				General.Map.Map.ClearMarkedSidedefs(true);
				List<Sidedef> sides = mode.GetSelectedWalls();
				foreach(Sidedef sd in sides) sd.Marked = false;
			}
			
			// Do the alignment
			Tools.AutoAlignImages(this.Sidedef, base.Texture, alignx, aligny, false);

			// Get the changed sidedefs
			List<Sidedef> changes = General.Map.Map.GetMarkedSidedefs(true);
			foreach(Sidedef sd in changes)
			{
				// Update the parts for this sidedef!
				if(mode.VisualSectorExists(sd.Sector))
				{
					BaseVisualSector vs = (mode.GetVisualSector(sd.Sector) as BaseVisualSector);
					VisualWallParts parts = vs.GetSidedefParts(sd);
					parts.SetupAllParts();
				}
			}
		}
		
		// Select texture
		public virtual void OnSelectImage()
		{
			if(General.Interface.IsActiveWindow)
			{
				int oldtile = GetImageIndex();
				int newtile = General.Interface.BrowseImage(General.Interface, oldtile);
				if(newtile != oldtile) mode.ApplySelectImage(newtile);
			}
		}

		// Apply Texture
		public virtual void ApplyImage(int tileindex)
		{
			mode.CreateUndo("Change image " + tileindex);
			SetTexture(tileindex);
		}
		
		// Paste texture
		public virtual void OnPasteImage()
		{
			if(BuilderPlug.Me.CopiedImageIndex > -1)
			{
				mode.CreateUndo("Paste image " + BuilderPlug.Me.CopiedImageIndex);
				mode.SetActionResult("Pasted image " + BuilderPlug.Me.CopiedImageIndex + ".");
				SetTexture(BuilderPlug.Me.CopiedImageIndex);
			}
		}
		
		// Paste texture offsets
		public virtual void OnPasteImageOffsets()
		{
			mode.CreateUndo("Paste image offsets");
			Sidedef.OffsetX = BuilderPlug.Me.CopiedOffsets.X;
			Sidedef.OffsetY = BuilderPlug.Me.CopiedOffsets.Y;
			mode.SetActionResult("Pasted image offsets " + Sidedef.OffsetX + ", " + Sidedef.OffsetY + ".");
			
			// Update sidedef geometry
			VisualWallParts parts = Sector.GetSidedefParts(Sidedef);
			parts.SetupAllParts();
		}
		
		// Copy texture
		public virtual void OnCopyImage()
		{
			BuilderPlug.Me.CopiedImageIndex = GetImageIndex();
			mode.SetActionResult("Copied image " + GetImageIndex() + ".");
		}
		
		// Copy texture offsets
		public virtual void OnCopyImageOffsets()
		{
			BuilderPlug.Me.CopiedOffsets = new Point(Sidedef.OffsetX, Sidedef.OffsetY);
			mode.SetActionResult("Copied image offsets " + Sidedef.OffsetX + ", " + Sidedef.OffsetY + ".");
		}

		// Copy properties
		public virtual void OnCopyProperties()
		{
			BuilderPlug.Me.CopiedWallProps = new BuildWall(Sidedef);
			mode.SetActionResult("Copied wall properties.");
		}

		// Paste properties
		public virtual void OnPasteProperties()
		{
			if(BuilderPlug.Me.CopiedWallProps != null)
			{
				mode.CreateUndo("Paste wall properties");
				mode.SetActionResult("Pasted wall properties.");
				BuilderPlug.Me.CopiedWallProps.ApplyTo(Sidedef);
				
				// Update sectors on both sides
				BaseVisualSector front = (BaseVisualSector)mode.GetVisualSector(Sidedef.Sector);
				if(front != null) front.Changed = true;
				if(Sidedef.Other != null)
				{
					BaseVisualSector back = (BaseVisualSector)mode.GetVisualSector(Sidedef.Other.Sector);
					if(back != null) back.Changed = true;
				}
				mode.ShowTargetInfo();
			}
		}
		
		// Return image index
		public virtual int GetImageIndex() { return -1; }
		
		// Select button pressed
		public virtual void OnSelectBegin()
		{
			mode.LockTarget();
			dragstartanglexy = General.Map.VisualCamera.AngleXY;
			dragstartanglez = General.Map.VisualCamera.AngleZ;
			dragorigin = pickintersect;
			startoffsetx = Sidedef.OffsetX;
			startoffsety = Sidedef.OffsetY;
			prevoffsetx = Sidedef.OffsetX;
			prevoffsety = Sidedef.OffsetY;
		}
		
		// Select button released
		public virtual void OnSelectEnd()
		{
			mode.UnlockTarget();
			
			// Was dragging?
			if(uvdragging)
			{
				// Dragging stops now
				uvdragging = false;
			}
			else
			{
				// Add/remove selection
				if(this.selected)
				{
					this.selected = false;
					mode.RemoveSelectedObject(this);
				}
				else
				{
					this.selected = true;
					mode.AddSelectedObject(this);
				}
			}
		}
		
		// Edit button released
		public virtual void OnEditEnd()
		{
			if(!General.Interface.IsActiveWindow) return;

			List<Sidedef> walls = mode.GetSelectedWalls();
			DialogResult result = General.Interface.ShowEditWalls(walls);
			if(result == DialogResult.OK)
			{
				foreach(Sidedef w in walls)
				{
					VisualSector vs = mode.GetVisualSector(w.Sector);
					if(vs != null) (vs as BaseVisualSector).Changed = true;
				}
			}
		}
		
		// Mouse moves
		public virtual void OnMouseMove(MouseEventArgs e)
		{
			// Dragging UV?
			if(uvdragging)
			{
				UpdateDragUV();
			}
			else
			{
				// Select button pressed?
				if(General.Actions.CheckActionActive(General.ThisAssembly, "visualselect"))
				{
					// Check if tolerance is exceeded to start UV dragging
					float deltaxy = General.Map.VisualCamera.AngleXY - dragstartanglexy;
					float deltaz = General.Map.VisualCamera.AngleZ - dragstartanglez;
					if((Math.Abs(deltaxy) + Math.Abs(deltaz)) > DRAG_ANGLE_TOLERANCE)
					{
						mode.PreAction(UndoGroup.ImageOffsetChange);
						mode.CreateUndo("Change image offsets");

						// Start drag now
						uvdragging = true;
						mode.Renderer.ShowSelection = false;
						mode.Renderer.ShowHighlight = false;
						UpdateDragUV();
					}
				}
			}
		}
		
		// This is called to update UV dragging
		protected virtual void UpdateDragUV()
		{
			float u_ray;
			
			// Calculate intersection position
			Line2D ray = new Line2D(General.Map.VisualCamera.Position, General.Map.VisualCamera.Target);
			Sidedef.Line.Line.GetIntersection(ray, out u_ray);
			Vector3D intersect = General.Map.VisualCamera.Position + (General.Map.VisualCamera.Target - General.Map.VisualCamera.Position) * u_ray;
			
			// Calculate offsets
			Vector3D dragdelta = intersect - dragorigin;
			Vector3D dragdeltaxy = dragdelta * deltaxy;
			Vector3D dragdeltaz = dragdelta * deltaz;
			float offsetx = dragdeltaxy.GetLength();
			float offsety = dragdeltaz.GetLength();
			if((Math.Sign(dragdeltaxy.x) < 0) || (Math.Sign(dragdeltaxy.y) < 0) || (Math.Sign(dragdeltaxy.z) < 0)) offsetx = -offsetx;
			if((Math.Sign(dragdeltaz.x) < 0) || (Math.Sign(dragdeltaz.y) < 0) || (Math.Sign(dragdeltaz.z) < 0)) offsety = -offsety;
			
			// Apply offsets
			int newoffsetx = startoffsetx - (int)Math.Round(offsetx);
			int newoffsety = startoffsety + (int)Math.Round(offsety);
			mode.ApplyImageOffsetChange(prevoffsetx - newoffsetx, prevoffsety - newoffsety);
			prevoffsetx = newoffsetx;
			prevoffsety = newoffsety;
			
			mode.ShowTargetInfo();
		}
		
		// Sector brightness change
		public virtual void OnChangeTargetShade(bool up)
		{
			// Change shade
			if((General.Map.UndoRedo.NextUndo == null) || (General.Map.UndoRedo.NextUndo.TicketID != undoticket))
				undoticket = mode.CreateUndo("Change wall shade");

			// Apply shade
			Sidedef.Shade = General.Clamp(Sidedef.Shade + (up ? -1 : 1), General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);

			mode.SetActionResult("Changed wall shade to " + Sidedef.Shade + ".");

			// Update wall geometry
			VisualWallParts parts = Sector.GetSidedefParts(Sidedef);
			parts.SetupAllParts();
		}
		
		// Image offset change
		public virtual void OnChangeImageOffset(int horizontal, int vertical)
		{
			if((General.Map.UndoRedo.NextUndo == null) || (General.Map.UndoRedo.NextUndo.TicketID != undoticket))
				undoticket = mode.CreateUndo("Change image offsets");
			
			// Apply offsets
			Sidedef.OffsetX -= horizontal;
			Sidedef.OffsetY -= vertical;

			mode.SetActionResult("Changed image offsets to " + Sidedef.OffsetX + ", " + Sidedef.OffsetY + ".");
			
			// Update wall geometry
			VisualWallParts parts = Sector.GetSidedefParts(Sidedef);
			parts.SetupAllParts();
		}
		
		#endregion
	}
}
