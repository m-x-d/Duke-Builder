
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
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	internal sealed class VisualUpper : BaseVisualGeometrySidedef
	{
		#region ================== Constructor / Setup

		// Constructor
		public VisualUpper(BaseVisualMode mode, VisualSector vs, Sidedef s) : base(mode, vs, s) { }

		// This builds the geometry. Returns false when no geometry created.
		public override bool Setup()
		{
			Vector2D vl, vr;

			// Left and right vertices for this sidedef
			if(Sidedef.IsFront)
			{
				vl = new Vector2D(Sidedef.Line.Start.Position);
				vr = new Vector2D(Sidedef.Line.End.Position);
			}
			else
			{
				vl = new Vector2D(Sidedef.Line.End.Position);
				vr = new Vector2D(Sidedef.Line.Start.Position);
			}

			// Keep top and bottom planes for intersection testing
			top = Sidedef.Sector.CeilingPlane;

			//mxd. Our sector's floor can be higher than the other sector's ceiling!
			Sector s = Sidedef.Sector;
			Sector os = Sidedef.Other.Sector;
			bottom = ((os.CeilingPlane.GetZ(vl) < s.FloorPlane.GetZ(vl) && os.CeilingPlane.GetZ(vr) < s.FloorPlane.GetZ(vr)) ? s.FloorPlane : os.CeilingPlane);

			// Get ceiling and floor heights
			float cl = top.GetZ(vl);
			float cr = top.GetZ(vr);
			float fl = bottom.GetZ(vl);
			float fr = bottom.GetZ(vr);

			// Anything to see?
			if(cl > fl || cr > fr)
			{
				int brightness = MapElement.CalculateBrightness(Sidedef.Shade);

				// Texture given?
				Texture = General.Map.Data.GetImageData(Sidedef.TileIndex);
				if(Texture is UnknownImage)
				{
					// Use missing texture
					//Texture = General.Map.Data.MissingTexture3D;
					setuponloadedimage = -1;
				}
				else if(!Texture.IsImageLoaded)
				{
					setuponloadedimage = Sidedef.TileIndex;
				}

				// Create initial polygon, which is just a quad between floor and ceiling
				var poly = new List<WorldVertex>
	                       {
		                       new WorldVertex(vl.x, vl.y, fl, brightness, 0, 0), 
							   new WorldVertex(vl.x, vl.y, cl, brightness, 0, 0), 
							   new WorldVertex(vr.x, vr.y, cr, brightness, 0, 0), 
							   new WorldVertex(vr.x, vr.y, fr, brightness, 0, 0)
	                       };

				//mxd. Set UV coords the Build way...
				int xref = (!Sidedef.ImageFlipX ? 1 : 0);
				int xrefinv = 1 - xref;
				int yref = (Sidedef.AlignImageToBottom ? Sidedef.Sector.CeilingHeight : Sidedef.Other.Sector.CeilingHeight);
				float ypancoef = CalculateOffsetV(Sidedef.OffsetY, Texture, !Sidedef.AlignImageToBottom);
				float scaledtexrepeaty = ((Texture.Height * 128f) / Sidedef.RepeatY);

				for(int i = 0; i < poly.Count; i++)
				{
					var p = poly[i];
					float dist = ((i == 2 || i == 3) ? xref : xrefinv);

					p.u = ((dist * 8.0f * Sidedef.RepeatX) + Sidedef.OffsetX) / Texture.Width;
					p.v = ((yref + (-p.z)) / scaledtexrepeaty) - ypancoef;
					if(Sidedef.ImageFlipY) p.v *= -1;
					
					poly[i] = p;
				}

				// Cut off the part below the other ceiling
				CropPoly(ref poly, bottom, false);

				if(poly.Count > 2)
				{
					// Now we create triangles from the polygon. The polygon is convex and clockwise, so this is a piece of cake.
					List<WorldVertex> verts = new List<WorldVertex>(3);
					for(int k = 1; k < (poly.Count - 1); k++)
					{
						verts.Add(poly[0]);
						verts.Add(poly[k]);
						verts.Add(poly[k + 1]);
					}

#if DEBUG
					//mxd. We should have either 3 or 6 verts, right?
					if(verts.Count != 3 && verts.Count != 6) throw new NotImplementedException("Unexpected number of vertices!");
#endif

					// Apply vertices
					base.SetVertices(verts);
					return true;
				}
			}

			// No geometry for invisible wall
			base.SetVertices(new WorldVertex[0]);
			return false;
		}
		
		#endregion

		#region ================== Methods

		// Return texture name
		public override int GetImageIndex()
		{
			return this.Sidedef.TileIndex;
		}

		// This changes the texture
		protected override void SetTexture(int tileindex)
		{
			this.Sidedef.TileIndex = tileindex;
			General.Map.Data.UpdateUsedImages();
			this.Setup();
		}
		
		#endregion
	}
}
