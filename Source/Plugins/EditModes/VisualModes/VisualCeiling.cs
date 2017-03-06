
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
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	internal sealed class VisualCeiling : BaseVisualGeometrySector
	{
		#region ================== Constructor / Setup

		// Constructor
		public VisualCeiling(BaseVisualMode mode, VisualSector vs) : base(mode, vs) { }

		// This builds the geometry. Returns false when no geometry created.
		public override bool Setup()
		{
			Sector s = base.Sector.Sector;
			
			// Load ceiling image
			Texture = General.Map.Data.GetImageData(s.CeilingTileIndex);
			if(Texture is UnknownImage)
			{
				// Use missing texture
				//Texture = General.Map.Data.MissingTexture3D;
				setuponloadedtexture = -1;
			}
			else if(!Texture.IsImageLoaded)
			{
				setuponloadedtexture = s.CeilingTileIndex;
			}

			// Make vertices
			WorldVertex[] verts = new WorldVertex[s.FloorVertices.Length];

			if(s.CeilingSloped)
			{
				for(int i = 0; i < s.CeilingVertices.Length; i++)
				{
					var v = s.CeilingVertices[i];
					verts[i] = new WorldVertex(v, s.CeilingPlane.GetZ(v.x, v.y));
				}
			}
			else
			{
				for(int i = 0; i < s.CeilingVertices.Length; i++)
					verts[i] = new WorldVertex(s.CeilingVertices[i], s.CeilingHeight);
			}

			// The sector triangulation created clockwise triangles that
			// are right up for the floor. For the ceiling we must flip the triangles upside down.
			for(int i = 0; i < verts.Length; i += 3)
			{
				// Swap some vertices to flip the triangle
				WorldVertex v = verts[i];
				verts[i] = verts[i + 1];
				verts[i + 1] = v;
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
			if(BuilderPlug.Me.CopiedImageIndex  > -1)
			{
				mode.CreateUndo("Paste ceiling image " + BuilderPlug.Me.CopiedImageIndex);
				mode.SetActionResult("Pasted image " + BuilderPlug.Me.CopiedImageIndex + " on ceiling.");
				SetImage(BuilderPlug.Me.CopiedImageIndex);
				this.Setup();
			}
		}
		
		// This changes the height
		protected override void ChangeHeight(int amount)
		{
			mode.CreateUndo("Change ceiling height", UndoGroup.CeilingHeightChange, this.Sector.Sector.Index);
			this.Sector.Sector.CeilingHeight += amount;
			mode.SetActionResult("Changed ceiling height to " + Sector.Sector.CeilingHeight + ".");
		}

		//mxd. This changes shade
		protected override void ChangeShade(bool up)
		{
			mode.CreateUndo("Change ceiling shade", UndoGroup.ShadeChange, this.Sector.Sector.Index);
			this.Sector.Sector.CeilingShade = General.Clamp(this.Sector.Sector.CeilingShade + (up ? -1 : 1), General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
			mode.SetActionResult("Changed ceiling shade to " + this.Sector.Sector.CeilingShade + ".");
		}

		//mxd. This changes slope angle. Amount is in radians!
		protected override void ChangeAngle(float amount)
		{
			mode.CreateUndo("Change ceiling slope angle", UndoGroup.AngleChange, Sector.Sector.Index);
			Sector.Sector.SetFlag(General.Map.FormatInterface.SectorSlopeFlag, true, false);
			Sector.Sector.CeilingSlope = General.Clamp(Sector.Sector.CeilingSlope - amount, General.Map.FormatInterface.MinSlope, General.Map.FormatInterface.MaxSlope);
			mode.SetActionResult("Changed ceiling slope angle to " + Math.Round(Angle2D.RadToDeg(Sector.Sector.CeilingSlope) - 90, 1) + ".");
		}

		//mxd. Image offset change
		protected override void ChangeImageOffset(int horizontal, int vertical)
		{
			var s = this.Sector.Sector;
			mode.CreateUndo("Change ceiling image offsets", UndoGroup.ImageOffsetChange, s.Index);

			s.CeilingOffsetX = General.Wrap(s.CeilingOffsetX + horizontal, General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
			s.CeilingOffsetY = General.Wrap(s.CeilingOffsetY + vertical, General.Map.FormatInterface.MinImageOffset, General.Map.FormatInterface.MaxImageOffset);
			
			s.UpdateCeilingSurface(); // Surface update needed
			mode.SetActionResult("Changed ceiling image offsets to " + s.CeilingOffsetX + ", " + s.CeilingOffsetY + ".");
		}
		
		// This performs a fast test in object picking
		public override bool PickFastReject(Vector3D from, Vector3D to, Vector3D dir)
		{
			//TODO: is there enough performance difference to keep non-plane version?
			if(Sector.Sector.CeilingSloped)
			{
				// Check if our ray starts at the correct side of the plane
				var plane = Sector.Sector.CeilingPlane;
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
				float planez = Sector.Sector.CeilingHeight;

				// Check if line crosses the z height
				if((from.z < planez) && (to.z > planez))
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
			return (((side <= 0.0f) && sd.IsFront) || ((side > 0.0f) && !sd.IsFront));
		}

		// Return image name
		public override int GetImageIndex()
		{
			return this.Sector.Sector.CeilingTileIndex;
		}

		// This changes the image
		protected override void SetImage(int tileindex)
		{
			this.Sector.Sector.CeilingTileIndex = tileindex;
			General.Map.Data.UpdateUsedImages();
			this.Setup();
		}
		
		#endregion
	}
}
