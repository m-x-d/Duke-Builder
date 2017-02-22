
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
		#region ================== Constants

		#endregion

		#region ================== Variables

		#endregion

		#region ================== Properties

		#endregion

		#region ================== Constructor / Setup

		// Constructor
		public VisualMiddleDouble(BaseVisualMode mode, VisualSector vs, Sidedef s) : base(mode, vs, s)
		{
			// Set render pass
			this.RenderPass = RenderPass.Mask;
		}
		
		// This builds the geometry. Returns false when no geometry created.
		public override bool Setup()
		{
			int brightness = Sidedef.CalculateBrightness(Sidedef.Shade);
			
			// Calculate size of this wall part
			float geotop = Math.Min(Sidedef.Sector.CeilingHeight, Sidedef.Other.Sector.CeilingHeight);
			float geobottom = Math.Max(Sidedef.Sector.FloorHeight, Sidedef.Other.Sector.FloorHeight);
			float geoheight = geotop - geobottom;
			if(geoheight > 0.001f)
			{
				// Masked image used?
				if(Sidedef.Masked || Sidedef.MaskedSolid)
				{
					Vector2D t1 = new Vector2D();
					Vector2D t2 = new Vector2D();
					float textop, texbottom;
					float cliptop = 0.0f;
					float clipbottom = 0.0f;
					
					// Load texture
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

					/*base.Texture = General.Map.Data.GetImageData(Sidedef.MaskedTileIndex);
					if(base.Texture == null)
					{
						base.Texture = General.Map.Data.MissingTexture3D;
						setuponloadedimage = Sidedef.LongMiddleTexture;
					}
					else
					{
						if(!base.Texture.IsImageLoaded)
							setuponloadedimage = Sidedef.LongMiddleTexture;
					}*/

					// Get texture scaled size
					Vector2D tsz = new Vector2D(base.Texture.Width, base.Texture.Height);

					// Because the middle texture on a double sided line does not repeat vertically,
					// we first determine the visible portion of the texture
					/*if(Sidedef.Line.IsFlagSet(General.Map.Config.LowerUnpeggedFlag))
						textop = geobottom + tsz.y;
					else*/
						textop = geotop;

					// Apply texture offset
					/*if(General.Map.Config.ScaledTextureOffsets)
					{
						textop += Sidedef.OffsetY * base.Texture.Scale.y;
					}
					else
					{*/
						textop += Sidedef.OffsetY;
					//}

					
					// Calculate texture portion bottom
					texbottom = textop - tsz.y;

					// Clip texture portion by geometry
					if(geotop < textop) { cliptop = textop - geotop; textop = geotop; }
					if(geobottom > texbottom) { clipbottom = geobottom - texbottom; texbottom = geobottom; }
					
					// Check if anything is still visible
					if((textop - texbottom) > 0.001f)
					{
						// Determine texture coordinatess
						t1.y = cliptop;
						t2.y = tsz.y - clipbottom;

						/*if(General.Map.Config.ScaledTextureOffsets && !base.Texture.WorldPanning)
						{
							t1.x = Sidedef.OffsetX * base.Texture.Scale.x;
						}
						else
						{*/
							t1.x = Sidedef.OffsetX;
						//} 

						t2.x = t1.x + Sidedef.Line.Length;
						
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
						verts[0] = new WorldVertex(v1.x, v1.y, texbottom, brightness, t1.x, t2.y);
						verts[1] = new WorldVertex(v1.x, v1.y, textop, brightness, t1.x, t1.y);
						verts[2] = new WorldVertex(v2.x, v2.y, textop, brightness, t2.x, t1.y);
						verts[3] = verts[0];
						verts[4] = verts[2];
						verts[5] = new WorldVertex(v2.x, v2.y, texbottom, brightness, t2.x, t2.y);
						
						// Keep properties
						//base.top = textop;
						//base.bottom = texbottom;
						
						// Apply vertices
						base.SetVertices(verts);
						return true;
					}
				}
			}
			
			// No geometry for invisible wall
			//base.top = geotop;
			//base.bottom = geotop;	// bottom same as top so that it has a height of 0 (otherwise it will still be picked up by object picking)
			base.SetVertices(new WorldVertex[0]);
			return false;
		}
		
		#endregion

		#region ================== Methods

		// Return texture name
		public override int GetImageIndex()
		{
			return this.Sidedef.MaskedTileIndex;
		}

		// This changes the texture
		protected override void SetTexture(int tileindex)
		{
			this.Sidedef.MaskedTileIndex = tileindex;
			General.Map.Data.UpdateUsedImages();
			this.Setup();
		}
		
		#endregion
	}
}
