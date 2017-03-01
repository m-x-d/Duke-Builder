
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
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;
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

			// Keep top and bottom planes for intersection testing
			top = Sidedef.Sector.CeilingPlane;
			bottom = Sidedef.Sector.FloorPlane;

			// Get ceiling and floor heights
			float cl = top.GetZ(vl);
			float cr = top.GetZ(vr);
			float fl = bottom.GetZ(vl);
			float fr = bottom.GetZ(vr);

			// Anything to see?
			if(cl - fl > 0.01f || cr - fr > 0.01f)
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

				WorldVertex[] verts = new WorldVertex[6];
				verts[0] = new WorldVertex(vl.x, vl.y, fl, brightness, 0, 0);
				verts[1] = new WorldVertex(vl.x, vl.y, cl, brightness, 0, 0);
				verts[2] = new WorldVertex(vr.x, vr.y, cr, brightness, 0, 0);
				verts[3] = verts[0];
				verts[4] = verts[2];
				verts[5] = new WorldVertex(vr.x, vr.y, fr, brightness, 0, 0);

				//mxd. Set UV coords the Build way...
				int xref = (!Sidedef.ImageFlipX ? 1 : 0);
				int xrefinv = 1 - xref;
				int yref = (Sidedef.AlignImageToBottom ? Sidedef.Sector.FloorHeight : Sidedef.Sector.CeilingHeight);
				float ypancoef = CalculateOffsetV(Sidedef.OffsetY, Texture, !Sidedef.AlignImageToBottom);
				float scaledtexrepeaty = ((Texture.Height * 128f) / Sidedef.RepeatY);

				for(int i = 0; i < 6; i++)
				{
					float dist = ((i == 2 || i == 4 ||i == 5) ? xref : xrefinv);

					verts[i].u = ((dist * 8.0f * Sidedef.RepeatX) + Sidedef.OffsetX) / Texture.Width;
					// w->wall.buffer[i].v = (-(float)(yref + (w->wall.buffer[i].y * 16)) / ((tilesiz[curpicnum].y * 2048.0f) / (float)(wal->yrepeat))) + ypancoef;
					verts[i].v = ((yref + (-verts[i].z)) / scaledtexrepeaty) - ypancoef;
					if(Sidedef.ImageFlipY) verts[i].v *= -1;
				}
				
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
