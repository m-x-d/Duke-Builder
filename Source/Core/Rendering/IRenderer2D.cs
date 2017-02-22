
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
using System.Drawing;
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Rendering
{
	public interface IRenderer2D
	{
		// Properties
		float OffsetX { get; }
		float OffsetY { get; }
		float TranslateX { get; }
		float TranslateY { get; }
		float Scale { get; }
		int VertexSize { get; }
		ViewMode ViewMode { get; }
		
		// View methods
		Vector2D DisplayToMap(Vector2D mousepos);
		Vector2D MapToDisplay(Vector2D mappos);
		
		// Color methods
		PixelColor DetermineLinedefColor(Linedef l);
		PixelColor DetermineThingColor(Thing t);
		EditorColor DetermineVertexColor(Vertex v);
		//int CalculateBrightness(int level);
		
		// Rendering management methods
		bool StartPlotter(bool clear);
		bool StartThings(bool clear);
		bool StartOverlay(bool clear);
		void Finish();
		void SetPresentation(Presentation present);
		void Present();

		// Drawing methods
		void PlotLine(Vector2D start, Vector2D end, PixelColor c);
		void PlotLinedef(Linedef l, PixelColor c);
		void PlotLinedefSet(ICollection<Linedef> linedefs);
		void PlotSector(Sector s);
		void PlotSector(Sector s, PixelColor c);
		void PlotVertex(Vertex v, EditorColor color);
		void PlotVertexAt(Vector2D v, EditorColor color);
		void PlotVerticesSet(ICollection<Vertex> vertices);
		void RenderThing(Thing t, PixelColor c, float alpha);
		void RenderThingSet(ICollection<Thing> things, float alpha);
		void RenderRectangle(RectangleF rect, float bordersize, PixelColor c, bool transformrect);
		void RenderRectangleFilled(RectangleF rect, PixelColor c, bool transformrect);
		void RenderRectangleFilled(RectangleF rect, PixelColor c, bool transformrect, ImageData texture);
		void RenderLine(Vector2D start, Vector2D end, float thickness, PixelColor c, bool transformcoords);
		void RenderText(TextLabel text);
		//void RenderText(IList<TextLabel> labels); //mxd
		void RenderGeometry(FlatVertex[] vertices, ImageData texture, bool transformcoords);
		void RedrawSurface();
	}
}
