
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
using mxd.DukeBuilder.EditModes.VisualModes;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	internal sealed class VisualMiddleSingle : BaseVisualGeometrySidedef
	{
		#region ================== Constants

		#endregion

		#region ================== Variables

		#endregion

		#region ================== Properties

		#endregion

		#region ================== Constructor / Setup

		// Constructor
		public VisualMiddleSingle(BaseVisualMode mode, VisualSector vs, Sidedef s) : base(mode, vs, s) { }

		// This builds the geometry. Returns false when no geometry created.
		public override bool Setup()
		{
			// Left and right vertices for this wall
			Vector2D vl, vr;
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

			// Get ceiling and floor heights
			Sector s = Sidedef.Sector;
			float fl = s.FloorPlane.GetZ(vl);
			float fr = s.FloorPlane.GetZ(vr);
			float cl = s.CeilingPlane.GetZ(vl);
			float cr = s.CeilingPlane.GetZ(vr);

			// Anything to see?
			if(cl - fl > 0.01f || cr - fr > 0.01f)
			{
				int brightness = Sidedef.CalculateBrightness(Sidedef.Shade);

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

				// Get texture scaled size
				Vector2D texscale = new Vector2D(1.0f / Texture.Width, 1.0f / Texture.Height);

				// Get texture offsets. Offset is based on image size (with 255 being equal to texture width or height)
				Vector2D offset = new Vector2D(Sidedef.OffsetX * Texture.Width, Sidedef.OffsetY * Texture.Height) * BuilderPlug.IMAGE_TO_MU_SCALER;
				Vector2D scale = new Vector2D(Sidedef.ImageFlipX ? -BuilderPlug.IMAGE_TO_MU_SCALER : BuilderPlug.IMAGE_TO_MU_SCALER, Sidedef.ImageFlipY ? -BuilderPlug.IMAGE_TO_MU_SCALER : BuilderPlug.IMAGE_TO_MU_SCALER);

				// Determine texture coordinates
				TexturePlane tp = new TexturePlane();
				float floorbias = (Sidedef.Sector.CeilingHeight == Sidedef.Sector.FloorHeight) ? 1.0f : 0.0f;
				
				tp.TextureBottomRight.x = (float)Math.Round(Sidedef.Line.Length);
				tp.TextureBottomRight.y = (Sidedef.Sector.CeilingHeight - (Sidedef.Sector.FloorHeight + floorbias));

				// Apply texture offset
				tp.TextureTopLeft = (tp.TextureTopLeft + offset) * scale * texscale;
				tp.TextureBottomRight = (tp.TextureBottomRight + offset) * scale * texscale;

				// Transform pixel coordinates to texture coordinates
				//tp.TextureTopLeft /= texsize;
				//tp.TextureBottomRight /= texsize;

				// Left top and right bottom of the geometry
				tp.VertTopLeft = new Vector3D(vl.x, vl.y, Sidedef.Sector.CeilingHeight);
				tp.VertBottomRight = new Vector3D(vr.x, vr.y, Sidedef.Sector.FloorHeight + floorbias);

				// Make the right-top coordinates
				tp.TextureTopRight = new Vector2D(tp.TextureBottomRight.x, tp.TextureTopLeft.y);
				tp.VertTopRight = new Vector3D(tp.VertBottomRight.x, tp.VertBottomRight.y, tp.VertTopLeft.z);

				WorldVertex[] verts = new WorldVertex[6];
				/*verts[0] = new WorldVertex(vl.x, vl.y, fl, brightness, 0, 0);
				verts[1] = new WorldVertex(vl.x, vl.y, cl, brightness, 0, 0);
				verts[2] = new WorldVertex(vr.x, vr.y, cr, brightness, 0, 0);
				verts[3] = verts[0];
				verts[4] = verts[2];
				verts[5] = new WorldVertex(vr.x, vr.y, fr, brightness, 0, 0);*/

				Vector3D v;
				
				v = new Vector3D(vl.x, vl.y, fl);
				verts[0] = new WorldVertex(v, tp.GetTextureCoordsAt(v), brightness);
				
				v = new Vector3D(vl.x, vl.y, cl);
				verts[1] = new WorldVertex(v, tp.GetTextureCoordsAt(v), brightness);

				v = new Vector3D(vr.x, vr.y, cr);
				verts[2] = new WorldVertex(v, tp.GetTextureCoordsAt(v), brightness);
				
				verts[3] = verts[0];
				verts[4] = verts[2];

				v = new Vector3D(vr.x, vr.y, fr);
				verts[5] = new WorldVertex(v, tp.GetTextureCoordsAt(v), brightness);
				
				// Determine texture coordinates
				// See http://doom.wikia.com/wiki/Texture_alignment
				// We just use pixels for coordinates for now
				//if(Sidedef.Line.IsFlagSet(General.Map.Config.LowerUnpeggedFlag))
				//{
					// When lower unpegged is set, the middle texture is bound to the bottom
					//t1.y = tsz.y - geoheight;
				//}
				//t2.x = t1.x + Sidedef.Line.Length;
				//t2.y = t1.y + geoheight;

				// Apply texture offset
				/*if(General.Map.Config.ScaledTextureOffsets && !base.Texture.WorldPanning)
				{
					t1 += new Vector2D(Sidedef.OffsetX * base.Texture.Scale.x, Sidedef.OffsetY * base.Texture.Scale.y);
					t2 += new Vector2D(Sidedef.OffsetX * base.Texture.Scale.x, Sidedef.OffsetY * base.Texture.Scale.y);
				}
				else
				{*/
					//t1 += new Vector2D(Sidedef.OffsetX, Sidedef.OffsetY);
					//t2 += new Vector2D(Sidedef.OffsetX, Sidedef.OffsetY);
				//}

				// Transform pixel coordinates to texture coordinates
				//t1 /= tsz;
				//t2 /= tsz;

				// Get world coordinates for geometry
				/*Vector2D v1, v2;
				if(Sidedef.IsFront)
				{
					v1 = Sidedef.Line.Start.Position;
					v2 = Sidedef.Line.End.Position;
				}
				else
				{
					v1 = Sidedef.Line.End.Position;
					v2 = Sidedef.Line.Start.Position;
				}*/

				// Make vertices
				/*WorldVertex[] verts = new WorldVertex[6];
				verts[0] = new WorldVertex(v1.x, v1.y, geobottom, brightness, t1.x, t2.y);
				verts[1] = new WorldVertex(v1.x, v1.y, geotop, brightness, t1.x, t1.y);
				verts[2] = new WorldVertex(v2.x, v2.y, geotop, brightness, t2.x, t1.y);
				verts[3] = verts[0];
				verts[4] = verts[2];
				verts[5] = new WorldVertex(v2.x, v2.y, geobottom, brightness, t2.x, t2.y);*/

				// Keep properties
				//base.top = geotop;
				//base.bottom = geobottom;
				
				// Apply vertices
				SetVertices(verts);
				return true;
			}

			// No geometry for invisible wall
			SetVertices(new WorldVertex[0]);
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
