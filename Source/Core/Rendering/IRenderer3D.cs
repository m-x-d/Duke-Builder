
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

using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.VisualModes;

#endregion

namespace mxd.DukeBuilder.Rendering
{
	public interface IRenderer3D
	{
		// Properties
		ProjectedFrustum2D Frustum2D { get; }
		bool DrawThingCages { get; set; }
		bool FullBrightness { get; set; }
		bool ShowSelection { get; set; }
		bool ShowHighlight { get; set; }
		
		// General methods
		void PositionAndLookAt(Vector3D pos, Vector3D lookat);
		
		// Presenting methods
		void Finish();
		bool Start();
		void StartGeometry();
		void FinishGeometry();

		// Rendering methods
		//int CalculateBrightness(int level);
		void SetHighlightedObject(IVisualPickable obj);
		void AddSectorGeometry(VisualGeometry g);
		void AddThingGeometry(VisualThing t);
		void RenderCrosshair();
		void SetFogMode(bool usefog);
		void SetCrosshairBusy(bool busy);
	}
}
