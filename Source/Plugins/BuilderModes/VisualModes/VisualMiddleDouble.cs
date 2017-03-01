
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
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	internal sealed class VisualMiddleDouble : BaseVisualGeometrySidedef
	{
		#region ================== Constructor / Setup

		// Constructor
		public VisualMiddleDouble(BaseVisualMode mode, VisualSector vs, Sidedef s) : base(mode, vs, s) { }
		
		// This builds the geometry. Returns false when no geometry created.
		public override bool Setup()
		{
			//mxd. Middle part exists only when either masked or maskedsolid flag is set
			bool masked = this.Sidedef.Masked; // wal->cstat & 16
			bool maskedsolid = this.Sidedef.MaskedSolid; // wal->cstat & 32

			if(masked || maskedsolid)
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
				Sector s = Sidedef.Sector;
				Sector os = Sidedef.Other.Sector;
				top = ((s.CeilingPlane.GetZ(vl) < os.CeilingPlane.GetZ(vl) && s.CeilingPlane.GetZ(vr) < os.CeilingPlane.GetZ(vr)) ? s.CeilingPlane : os.CeilingPlane);
				bottom = ((os.FloorPlane.GetZ(vl) > s.FloorPlane.GetZ(vl) && os.FloorPlane.GetZ(vr) > s.FloorPlane.GetZ(vr)) ? os.FloorPlane : s.FloorPlane);

				// Get ceiling and floor heights
				float cl = top.GetZ(vl);
				float cr = top.GetZ(vr);
				float fl = bottom.GetZ(vl);
				float fr = bottom.GetZ(vr);

				// Anything to see?
				if(cl > fl || cr > fr)
				{
					// Mostly the same as upper wall part
					if(masked && maskedsolid)
					{
						// Transparency and brightness
						int brightness;
						if(Sidedef.SemiTransparent)
						{
							brightness = MapElement.CalculateBrightness(Sidedef.Shade, (byte)(Sidedef.Transparent ? 85 : 170));
							this.RenderPass = RenderPass.Alpha;
						}
						else
						{
							brightness = MapElement.CalculateBrightness(Sidedef.Shade);
							this.RenderPass = RenderPass.Mask;
						}

						// Texture given?
						Texture = General.Map.Data.GetImageData(Sidedef.MaskedTileIndex);
						if(Texture is UnknownImage)
						{
							// Use missing texture
							//Texture = General.Map.Data.MissingTexture3D;
							setuponloadedimage = -1;
						}
						else if(!Texture.IsImageLoaded)
						{
							setuponloadedimage = Sidedef.MaskedTileIndex;
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

						int yref = (Sidedef.AlignImageToBottom ? 
							Math.Min(Sidedef.Sector.FloorHeight, Sidedef.Other.Sector.FloorHeight) :
							Math.Max(Sidedef.Sector.CeilingHeight, Sidedef.Other.Sector.CeilingHeight));

						float ypancoef = CalculateOffsetV(Sidedef.OffsetY, Texture, false);
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

						// Now we create triangles from the polygon. The polygon is convex and clockwise, so this is a piece of cake.
						List<WorldVertex> verts = new List<WorldVertex>(3);
						for(int k = 1; k < (poly.Count - 1); k++)
						{
							verts.Add(poly[0]);
							verts.Add(poly[k]);
							verts.Add(poly[k + 1]);
						}

						// Apply vertices
						SetVertices(verts);
						return true;
					}
					// Mostly the same as lower wall part
					else // masked || maskedsolid
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
						int yref = (!refwall.AlignImageToBottom ? refwall.Other.Sector.FloorHeight : refwall.Sector.CeilingHeight);
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

						// Now we create triangles from the polygon. The polygon is convex and clockwise, so this is a piece of cake.
						List<WorldVertex> verts = new List<WorldVertex>(3);
						for(int k = 1; k < (poly.Count - 1); k++)
						{
							verts.Add(poly[0]);
							verts.Add(poly[k]);
							verts.Add(poly[k + 1]);
						}

						// Apply vertices
						base.SetVertices(verts);
						return true;
					}
				}
			}
			
			// No geometry for invisible wall
			top = Sidedef.Sector.CeilingPlane;
			bottom = Sidedef.Sector.CeilingPlane;	// bottom same as top so that it has a height of 0 (otherwise it will still be picked up by object picking)
			SetVertices(new WorldVertex[0]);
			return false;
		}
		
		#endregion

		#region ================== Methods

		// Return texture name
		public override int GetImageIndex()
		{
			return (this.Sidedef.Masked && this.Sidedef.MaskedSolid ? this.Sidedef.MaskedTileIndex : this.Sidedef.TileIndex);
		}

		// This changes the texture
		protected override void SetTexture(int tileindex)
		{
			if(this.Sidedef.Masked && this.Sidedef.MaskedSolid)
			{
				this.Sidedef.MaskedTileIndex = tileindex;
			}
			else
			{
				this.Sidedef.TileIndex = tileindex;
			}
			
			General.Map.Data.UpdateUsedImages();
			this.Setup();
		}
		
		#endregion
	}
}
