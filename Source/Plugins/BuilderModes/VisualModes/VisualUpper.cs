
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
			int brightness = Sidedef.CalculateBrightness(Sidedef.Shade);

			// Calculate size of this wall part
			float geotop = Sidedef.Sector.CeilingHeight;
			float geobottom = Sidedef.Other.Sector.CeilingHeight;
			float geoheight = geotop - geobottom;
			if(geoheight > 0.001f)
			{
				Vector2D t1 = new Vector2D();
				Vector2D t2 = new Vector2D();

				// Texture given?
				int tileindex = Sidedef.TileIndex;
				Texture = General.Map.Data.GetImageData(tileindex);
				if(Texture is UnknownImage)
				{
					// Use missing texture
					Texture = General.Map.Data.MissingTexture3D;
					setuponloadedimage = -1;
				}
				else if(!Texture.IsImageLoaded)
				{
					setuponloadedimage = tileindex;
				}

				// Get texture scaled size
				Vector2D tsz = new Vector2D(Texture.Width, Texture.Height);

				// Determine texture coordinates
				// See http://doom.wikia.com/wiki/Texture_alignment
				// We just use pixels for coordinates for now
				//if(!Sidedef.Line.IsFlagSet(General.Map.Config.UpperUnpeggedFlag))
				//{
					// When upper unpegged is NOT set, the upper texture is bound to the bottom
					//t1.y = tsz.y - geoheight;
				//}
				t2.x = t1.x + Sidedef.Line.Length;
				t2.y = t1.y + geoheight;

				// Apply texture offset
				/*if(General.Map.Config.ScaledTextureOffsets && !base.Texture.WorldPanning)
				{
					t1 += new Vector2D(Sidedef.OffsetX * base.Texture.Scale.x, Sidedef.OffsetY * base.Texture.Scale.y);
					t2 += new Vector2D(Sidedef.OffsetX * base.Texture.Scale.x, Sidedef.OffsetY * base.Texture.Scale.y);
				}
				else
				{*/
					t1 += new Vector2D(Sidedef.OffsetX, Sidedef.OffsetY);
					t2 += new Vector2D(Sidedef.OffsetX, Sidedef.OffsetY);
				//}

				// Transform pixel coordinates to texture coordinates
				t1 /= tsz;
				t2 /= tsz;

				// Get world coordinates for geometry
				Vector2D v1, v2;
				if(Sidedef.IsFront)
				{
					v1 = Sidedef.Line.Start.Position;
					v2 = Sidedef.Line.End.Position;
				}
				else
				{
					v1 = Sidedef.Line.End.Position;
					v2 = Sidedef.Line.Start.Position;
				}

				// Make vertices
				WorldVertex[] verts = new WorldVertex[6];
				verts[0] = new WorldVertex(v1.x, v1.y, geobottom, brightness, t1.x, t2.y);
				verts[1] = new WorldVertex(v1.x, v1.y, geotop, brightness, t1.x, t1.y);
				verts[2] = new WorldVertex(v2.x, v2.y, geotop, brightness, t2.x, t1.y);
				verts[3] = verts[0];
				verts[4] = verts[2];
				verts[5] = new WorldVertex(v2.x, v2.y, geobottom, brightness, t2.x, t2.y);
				
				// Keep properties
				//base.top = geotop;
				//base.bottom = geobottom;
				
				// Apply vertices
				base.SetVertices(verts);
				return true;
			}

			// No geometry for invisible wall
			//base.top = geotop;
			//base.bottom = geobottom;
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
