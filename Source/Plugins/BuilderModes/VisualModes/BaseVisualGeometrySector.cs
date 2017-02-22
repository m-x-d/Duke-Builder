
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
using System.Windows.Forms;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	internal abstract class BaseVisualGeometrySector : VisualGeometry, IVisualEventReceiver
	{
		#region ================== Variables

		protected BaseVisualMode mode;
		protected int setuponloadedtexture = -1;

		// This is only used to see if this object has already received a change
		// in a multiselection. The Changed property on the BaseVisualSector is
		// used to indicate a rebuild is needed.
		protected bool changed;
		
		#endregion

		#region ================== Properties
		
		new public BaseVisualSector Sector { get { return (BaseVisualSector)base.Sector; } }
		public bool Changed { get { return changed; } set { changed = value; } }

		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public BaseVisualGeometrySector(BaseVisualMode mode, VisualSector vs) : base(vs)
		{
			this.mode = mode;
		}

		#endregion

		#region ================== Methods

		// This changes the height
		protected abstract void ChangeHeight(int amount);

		//mxd. This changes the shade
		protected abstract void ChangeShade(bool up);

		//mxd. This changes the slope angle
		protected abstract void ChangeAngle(float amount);

		//mxd. This changes the image offsets
		protected abstract void ChangeImageOffset(int horizontal, int vertical);
		
		#endregion

		#region ================== Events

		// Unused
		public abstract bool Setup();
		public virtual void OnSelectBegin(){ }
		public virtual void OnEditBegin() { }
		public virtual void OnMouseMove(MouseEventArgs e) { }
		public virtual void OnImageAlign(bool alignx, bool aligny) { }
		public virtual void OnToggleBottomAlignment() { }
		public virtual void OnResetImageOffsets() { }
		public virtual void OnCopyImageOffsets() { }
		public virtual void OnPasteImageOffsets() { }
		public virtual void OnInsert() { }
		public virtual void OnDelete() { }
		protected virtual void SetImage(int tileindex) { }
		public virtual void ApplyBottomAlignment(bool set) { }

		// Select or deselect
		public virtual void OnSelectEnd()
		{
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
		
		// Processing
		public virtual void OnProcess(double deltatime)
		{
			// If the texture was not loaded, but is loaded now, then re-setup geometry
			if(setuponloadedtexture > -1)
			{
				ImageData t = General.Map.Data.GetImageData(setuponloadedtexture);
				if(t != null && t.IsImageLoaded)
				{
					setuponloadedtexture = -1;
					Setup();
				}
			}
		}

		// Flood-fill textures
		public virtual void OnImageFloodfill()
		{
			if(BuilderPlug.Me.CopiedImageIndex <= -1) return;

			int oldtile = GetImageIndex();
			int newtile = BuilderPlug.Me.CopiedImageIndex;
			if(oldtile == newtile) return;

			// Get the image
			ImageData newtextureimage = General.Map.Data.GetImageData(newtile);
			if(newtextureimage == null) return;

			bool fillceilings = (this is VisualCeiling);
			string elementname = (fillceilings ? "ceilings" : "floors");
			mode.CreateUndo("Flood-fill " + elementname + " with image " + newtile);
			mode.SetActionResult("Flood-filled " + elementname + " with image " + newtile + ".");

			mode.Renderer.SetCrosshairBusy(true);
			General.Interface.RedrawDisplay();

			if(mode.IsSingleSelection)
			{
				// Clear all marks, this will align everything it can
				General.Map.Map.ClearMarkedSectors(false);
			}
			else
			{
				// Limit the alignment to selection only
				General.Map.Map.ClearMarkedSectors(true);
				List<Sector> sectors = mode.GetSelectedSectors();
				foreach(Sector s in sectors) s.Marked = false;
			}
						
			// Do the fill
			Tools.FloodfillSectorImages(this.Sector.Sector, fillceilings, oldtile, newtile, false);

			// Get the changed sectors
			List<Sector> changes = General.Map.Map.GetMarkedSectors(true);
			foreach(Sector s in changes)
			{
				// Update the visual sector
				if(mode.VisualSectorExists(s))
				{
					BaseVisualSector vs = (mode.GetVisualSector(s) as BaseVisualSector);
					if(fillceilings)
						vs.Ceiling.Setup();
					else
						vs.Floor.Setup();
				}
			}

			General.Map.Data.UpdateUsedImages();
			mode.Renderer.SetCrosshairBusy(false);
			mode.ShowTargetInfo();
		}
		
		// Copy properties
		public virtual void OnCopyProperties()
		{
			BuilderPlug.Me.CopiedSectorProps = new BuildSector(Sector.Sector);
			mode.SetActionResult("Copied sector properties.");
		}
		
		// Paste properties
		public virtual void OnPasteProperties()
		{
			if(BuilderPlug.Me.CopiedSectorProps != null)
			{
				mode.CreateUndo("Paste sector properties");
				mode.SetActionResult("Pasted sector properties.");
				BuilderPlug.Me.CopiedSectorProps.ApplyTo(Sector.Sector);
				Sector.UpdateSectorGeometry(true);
				mode.ShowTargetInfo();
			}
		}
		
		// Select texture
		public virtual void OnSelectImage()
		{
			if(General.Interface.IsActiveWindow)
			{
				int oldtile = GetImageIndex();
				int newtile = General.Interface.BrowseImage(General.Interface, oldtile);
				if(oldtile != newtile) mode.ApplySelectImage(newtile);
			}
		}

		// Apply Texture
		public virtual void ApplyImage(int tileindex)
		{
			mode.CreateUndo("Change image " + tileindex);
			SetImage(tileindex);
		}
		
		// Copy texture
		public virtual void OnCopyImage()
		{
			BuilderPlug.Me.CopiedImageIndex = GetImageIndex();
			mode.SetActionResult("Copied image " + BuilderPlug.Me.CopiedImageIndex + ".");
		}
		
		public virtual void OnPasteImage() { }

		// Return image index
		public virtual int GetImageIndex() { return -1; }
		
		// Edit button released
		public virtual void OnEditEnd()
		{
			if(General.Interface.IsActiveWindow)
			{
				List<Sector> sectors = mode.GetSelectedSectors();
				DialogResult result = General.Interface.ShowEditSectors(sectors);
				if(result == DialogResult.OK)
				{
					// Rebuild sector
					foreach(Sector s in sectors)
					{
						VisualSector vs = mode.GetVisualSector(s);
						if(vs != null)
							(vs as BaseVisualSector).UpdateSectorGeometry(true);
					}
				}
			}
		}

		// Sector height change
		public virtual void OnChangeTargetHeight(int amount)
		{
			changed = true;
			ChangeHeight(amount);

			// Rebuild sector
			Sector.UpdateSectorGeometry(true);
		}
		
		//mxd. Sector brightness change
		public virtual void OnChangeTargetShade(bool up)
		{
			changed = true;
			ChangeShade(up);
			Sector.Sector.UpdateCache();

			// Rebuild sector
			Sector.UpdateSectorGeometry(false);
		}

		//mxd. Floor/ceiling slope change. Amount is in radians
		public virtual void OnChangeTargetAngle(float amount)
		{
			changed = true;
			ChangeAngle(amount);
			//Sector.Sector.UpdateCache();

			// Rebuild sector
			Sector.UpdateSectorGeometry(false);
		}

		//mxd. Image offset change
		public virtual void OnChangeImageOffset(int horizontal, int vertical)
		{
			changed = true;
			ChangeImageOffset(horizontal, vertical);
			Sector.Sector.UpdateCache();

			// Rebuild sector
			Sector.UpdateSectorGeometry(false);
		}
		
		#endregion
	}
}
