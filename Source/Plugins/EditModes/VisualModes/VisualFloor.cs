	
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
using System.Drawing;
using System.Linq;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	internal sealed class VisualFloor : BaseVisualGeometrySector
	{
		#region ================== Constructor / Setup

		// Constructor
		public VisualFloor(BaseVisualMode mode, VisualSector vs) : base(mode, vs) { }

		// This builds the geometry. Returns false when no geometry created.
		public override bool Setup()
		{
			Sector s = base.Sector.Sector;
			
			// Load floor image
			Texture = General.Map.Data.GetImageData(s.FloorTileIndex);
			if(Texture is UnknownImage)
			{
				// Use missing texture
				//Texture = General.Map.Data.MissingTexture3D;
				setuponloadedtexture = -1;
			}
			else if(!Texture.IsImageLoaded)
			{
				setuponloadedtexture = s.FloorTileIndex;
			}
			
			// Make vertices
			WorldVertex[] verts = new WorldVertex[s.FloorVertices.Length];
			
			if(s.FloorSloped)
			{
				for(int i = 0; i < s.FloorVertices.Length; i++)
				{
					var v = s.FloorVertices[i];
					verts[i] = new WorldVertex(v, s.FloorPlane.GetZ(v.x, v.y));
				}
			}
			else
			{
				for(int i = 0; i < s.FloorVertices.Length; i++)
					verts[i] = new WorldVertex(s.FloorVertices[i], s.FloorHeight);
			}
			
			// Apply vertices
			base.SetVertices(verts);
			return (verts.Length > 0);
		}
		
		#endregion
		
		#region ================== Methods
		
		// Paste texture
		public override void OnPasteImage()
		{
			if(BuilderPlug.Me.CopiedImageIndex > -1)
			{
				mode.CreateUndo("Paste floor image " + BuilderPlug.Me.CopiedImageIndex);
				mode.SetActionResult("Pasted image " + BuilderPlug.Me.CopiedImageIndex + " on floor.");
				SetImage(BuilderPlug.Me.CopiedImageIndex);
				this.Setup();
			}
		}

		// This changes the height
		protected override void ChangeHeight(int amount)
		{
			mode.CreateUndo("Change floor height", UndoGroup.FloorHeightChange, this.Sector.Sector.Index);
			this.Sector.Sector.FloorHeight += amount;
			mode.SetActionResult("Changed floor height to " + Sector.Sector.FloorHeight + ".");
		}

		//mxd. This changes shade
		protected override void ChangeShade(bool up)
		{
			mode.CreateUndo("Change floor shade", UndoGroup.ShadeChange, Sector.Sector.Index);
			Sector.Sector.FloorShade = General.Clamp(Sector.Sector.FloorShade + (up ? -1 : 1), General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
			mode.SetActionResult("Changed floor shade to " + Sector.Sector.FloorShade + ".");
		}

		//mxd. This changes slope angle. Amount is in radians!
		protected override void ChangeAngle(float amount)
		{
			mode.CreateUndo("Change floor slope angle", UndoGroup.AngleChange, Sector.Sector.Index);
			Sector.Sector.SetFlag(General.Map.FormatInterface.SectorSlopeFlag, true, true);
			Sector.Sector.FloorSlope = General.Clamp(Sector.Sector.FloorSlope + amount, General.Map.FormatInterface.MinSlope, General.Map.FormatInterface.MaxSlope);
			mode.SetActionResult("Changed floor slope angle to " + Math.Round(Angle2D.RadToDeg(Sector.Sector.FloorSlope) - 90, 3) + ".");
		}

		//mxd. Image offset change
		protected override void ChangeImageOffset(int horizontal, int vertical)
		{
			var s = Sector.Sector;
			mode.CreateUndo("Change floor image offsets", UndoGroup.ImageOffsetChange, s.Index);

			s.FloorOffsetX = General.Wrap(s.FloorOffsetX + horizontal, General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
			s.FloorOffsetY = General.Wrap(s.FloorOffsetY + vertical, General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
			
			s.UpdateFloorSurface(); // Surface update needed
			mode.SetActionResult("Changed floor image offsets to " + s.FloorOffsetX + ", " + s.FloorOffsetY + ".");
		}

		// This performs a fast test in object picking
		public override bool PickFastReject(Vector3D from, Vector3D to, Vector3D dir)
		{
			if(Sector.Sector.FloorSloped)
			{
				// Check if our ray starts at the correct side of the plane
				var plane = Sector.Sector.FloorPlane;
				if(plane.Distance(from) > 0.0f)
				{
					// Calculate the intersection
					if(plane.GetIntersection(from, to, ref pickrayu) && pickrayu > 0.0f)
					{
						pickintersect = from + (to - from) * pickrayu;

						// Intersection point within bbox?
						RectangleF bbox = Sector.Sector.BBox;
						return ((pickintersect.x >= bbox.Left) && (pickintersect.x <= bbox.Right) &&
								(pickintersect.y >= bbox.Top) && (pickintersect.y <= bbox.Bottom));
					}
				}
			}
			else
			{
				float planez = Sector.Sector.FloorHeight;

				// Check if line crosses the z height
				if((from.z > planez) && (to.z < planez))
				{
					// Calculate intersection point using the z height
					pickrayu = (planez - from.z) / (to.z - from.z);
					pickintersect = from + (to - from) * pickrayu;

					// Intersection point within bbox?
					RectangleF bbox = Sector.Sector.BBox;
					return ((pickintersect.x >= bbox.Left) && (pickintersect.x <= bbox.Right) &&
							(pickintersect.y >= bbox.Top) && (pickintersect.y <= bbox.Bottom));
				}
			}

			// Not even crossing the z height (or not in the right direction)
			return false;
		}
		
		// This performs an accurate test for object picking
		public override bool PickAccurate(Vector3D from, Vector3D to, Vector3D dir, ref float u_ray)
		{
			u_ray = pickrayu;
			
			// Check on which side of the nearest sidedef we are
			Sidedef sd = MapSet.NearestSidedef(Sector.Sector.Sidedefs, pickintersect);
			float side = sd.Line.SideOfLine(pickintersect);
			return ( (side <= 0.0f && sd.IsFront) || (side > 0.0f && !sd.IsFront) );
		}
		
		// Return image index
		public override int GetImageIndex()
		{
			return this.Sector.Sector.FloorTileIndex;
		}

		// This changes the texture
		protected override void SetImage(int tileindex)
		{
			this.Sector.Sector.FloorTileIndex = tileindex;
			General.Map.Data.UpdateUsedImages();
			this.Setup();
		}
		
		#endregion
	}
}
