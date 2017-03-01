
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
	internal sealed class VisualLower : BaseVisualGeometrySidedef
	{
		#region ================== Constructor / Setup

		// Constructor
		public VisualLower(BaseVisualMode mode, VisualSector vs, Sidedef s) : base(mode, vs, s) { }

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
			//mxd. Our sector's ceiling can be lower than the other sector's floor!
			Sector s = Sidedef.Sector;
			Sector os = Sidedef.Other.Sector;
			top = ((s.CeilingPlane.GetZ(vl) < os.FloorPlane.GetZ(vl) && s.CeilingPlane.GetZ(vr) < os.FloorPlane.GetZ(vr)) ? s.CeilingPlane : os.FloorPlane);
			bottom = Sidedef.Sector.FloorPlane;

			// Get ceiling and floor heights
			float cl = top.GetZ(vl);
			float cr = top.GetZ(vr);
			float fl = bottom.GetZ(vl);
			float fr = bottom.GetZ(vr);

			// Anything to see?
			if(cl > fl || cr > fr)
			{
				//INFO: used refwall props: picnum, palette, shade, xpanning, ypanning
				var refwall = (Sidedef.SwapBottomImage ? GetNextWall() : Sidedef);
				int brightness = MapElement.CalculateBrightness(refwall.Shade);

				// Texture given?
				Texture = General.Map.Data.GetImageData(refwall.TileIndex);
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
				int yref = (!refwall.AlignImageToBottom ? Sidedef.Other.Sector.FloorHeight : Sidedef.Sector.CeilingHeight);
				float ypancoef = CalculateOffsetV(refwall.OffsetY, Texture, !refwall.AlignImageToBottom);
				float scaledtexrepeaty = ((Texture.Height * 128f) / Sidedef.RepeatY);
				
				// (!(wal->cstat & 2) && (wal->cstat & 256)) || ((wal->cstat & 2) && (wall[nwallnum].cstat & 256))
				bool flipy = (!Sidedef.SwapBottomImage && Sidedef.ImageFlipY) || (Sidedef.SwapBottomImage && refwall.ImageFlipY);

				for(int i = 0; i < poly.Count; i++)
				{
					var p = poly[i];
					float dist = ((i == 2 || i == 3) ? xref : xrefinv);

					p.u = ((dist * 8.0f * Sidedef.RepeatX) + refwall.OffsetX) / Texture.Width;
					p.v = ((yref + (-p.z)) / scaledtexrepeaty) - ypancoef;
					if(flipy) p.v *= -1;

					poly[i] = p;
				}

				// Cut off the part above the other floor
				CropPoly(ref poly, top, false);

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
			return (Sidedef.SwapBottomImage ? GetNextWall().TileIndex : Sidedef.TileIndex);
		}

		// This changes the texture
		protected override void SetTexture(int tileindex)
		{
			if(Sidedef.SwapBottomImage)
			{
				var nextwall = GetNextWall();
				nextwall.TileIndex = tileindex;
				foreach(var geo in mode.GetVisualSector(nextwall.Sector).GetSidedefGeometry(nextwall))
				{
					((BaseVisualGeometrySidedef)geo).Setup();
				}
			}
			else
			{
				Sidedef.TileIndex = tileindex;
			}

			General.Map.Data.UpdateUsedImages();
			this.Setup();
		}
		
		#endregion
	}
}
