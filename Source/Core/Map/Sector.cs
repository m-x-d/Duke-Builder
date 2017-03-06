
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
using System.Drawing;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.Map
{
	public sealed class Sector : SelectableElement
	{
		#region ================== Variables

		// Map
		private MapSet map;

		// List items
		private LinkedListNode<Sector> selecteditem;
		
		// Sidedefs
		private LinkedList<Sidedef> sidedefs;

		// Build Properties
		private Sidedef firstwall;
		private int ceilingheight;
		private int floorheight;

		private Dictionary<string, bool> ceilingflags;
		private Dictionary<string, bool> floorflags;

		private int ceilingtileindex;
		private float ceilingslope; // in radians!
		private int ceilingshade;
		private int ceilingpaletteindex;
		private int ceilingoffsetx;
		private int ceilingoffsety;

		private int floortileindex;
		private float floorslope; // in radians!
		private int floorshade;
		private int floorpaletteindex;
		private int flooroffsetx;
		private int flooroffsety;

		private int visibility;

		private int hitag;			// Significance is game-specific
		private int lotag;
		private int extra;

		// Cloning
		private Sector clone;
		private int serializedindex;
		
		// Triangulation
		private bool updateneeded;
		private bool triangulationneeded;
		private RectangleF bbox;
		private Triangulation triangles;
		private FlatVertex[] floorvertices;
		private FlatVertex[] ceilingvertices;
		private LabelPositionInfo label;
		private SurfaceEntryCollection surfaceentries;

		//mxd. Planes
		private Plane ceilingplane;
		private Plane floorplane;

		private bool ceilingplaneupdateneeded;
		private bool floorplaneupdateneeded;

		//mxd. Cached flags
		private bool ceilingsloped;
		private bool floorsloped;
		
		#endregion

		#region ================== Properties

		public MapSet Map { get { return map; } }
		public ICollection<Sidedef> Sidedefs { get { return sidedefs; } }
		
		// Build properties
		public Sidedef FirstWall
		{ 
			get { return firstwall; }
			set
			{
				BeforePropsChange();

				firstwall = value;
				updateneeded = true;
				ceilingplaneupdateneeded = true;
				floorplaneupdateneeded = true;
				General.Map.IsChanged = true; 
			} 
		}

		public int CeilingHeight { get { return ceilingheight; } set { BeforePropsChange(); ceilingheight = value; ceilingplaneupdateneeded = true; } }
		public int FloorHeight { get { return floorheight; } set { BeforePropsChange(); floorheight = value; floorplaneupdateneeded = true; } }

		internal Dictionary<string, bool> CeilingFlags { get { return ceilingflags; } }
		internal Dictionary<string, bool> FloorFlags { get { return floorflags; } }

		public int CeilingTileIndex { get { return ceilingtileindex; } set { BeforePropsChange(); ceilingtileindex = value; General.Map.IsChanged = true; } }
		public float CeilingSlope { get { return ceilingslope; } set { BeforePropsChange(); ceilingslope = value; ceilingplaneupdateneeded = true; updateneeded = true; } }
		public int CeilingShade { get { return ceilingshade; } set { BeforePropsChange(); ceilingshade = value; } }
		public int CeilingPaletteIndex { get { return ceilingpaletteindex; } set { BeforePropsChange(); ceilingpaletteindex = value; } }
		public int CeilingOffsetX { get { return ceilingoffsetx; } set { BeforePropsChange(); ceilingoffsetx = value; } }
		public int CeilingOffsetY { get { return ceilingoffsety; } set { BeforePropsChange(); ceilingoffsety = value; } }

		public int FloorTileIndex { get { return floortileindex; } set { BeforePropsChange(); floortileindex = value; General.Map.IsChanged = true; } }
		public float FloorSlope { get { return floorslope; } set { BeforePropsChange(); floorslope = value; floorplaneupdateneeded = true; updateneeded = true; } }
		public int FloorShade { get { return floorshade; } set { BeforePropsChange(); floorshade = value; updateneeded = true; } }
		public int FloorPaletteIndex { get { return floorpaletteindex; } set { BeforePropsChange(); floorpaletteindex = value; } }
		public int FloorOffsetX { get { return flooroffsetx; } set { BeforePropsChange(); flooroffsetx = value; } }
		public int FloorOffsetY { get { return flooroffsety; } set { BeforePropsChange(); flooroffsety = value; } }

		public int Visibility { get { return visibility; } set { BeforePropsChange(); visibility = value; updateneeded = true; } }

		// Identification
		public int HiTag { get { return hitag; } set { BeforePropsChange(); hitag = value; if((hitag < General.Map.FormatInterface.MinTag) || (hitag > General.Map.FormatInterface.MaxTag)) throw new ArgumentOutOfRangeException("HiTag", "Invalid hitag number"); } }
		public int LoTag { get { return lotag; } set { BeforePropsChange(); lotag = value; if((lotag < General.Map.FormatInterface.MinTag) || (lotag > General.Map.FormatInterface.MaxTag)) throw new ArgumentOutOfRangeException("LoTag", "Invalid lotag number"); } }
		public int Extra { get { return extra; } set { BeforePropsChange(); extra = value; if((extra < General.Map.FormatInterface.MinExtra) || (extra > General.Map.FormatInterface.MaxExtra)) throw new ArgumentOutOfRangeException("Extra", "Invalid extra number"); } }

		// Properties
		public bool UpdateNeeded { get { return updateneeded; } set { updateneeded |= value; triangulationneeded |= value; } }
		public RectangleF BBox { get { return bbox; } }
		internal Sector Clone { get { return clone; } set { clone = value; } }
		internal int SerializedIndex { get { return serializedindex; } set { serializedindex = value; } }
		public Triangulation Triangles { get { return triangles; } }
		public FlatVertex[] FloorVertices { get { return floorvertices; } }
		public FlatVertex[] CeilingVertices { get { return ceilingvertices; } }
		public LabelPositionInfo Label { get { return label; } }

		//mxd. Planes
		public Plane CeilingPlane { get { UpdateCeilingPlane(); return ceilingplane; } }
		public Plane FloorPlane { get { UpdateFloorPlane(); return floorplane; } }

		//mxd. Cached flags
		public bool CeilingParallaxed { get { return CheckCeilingFlag(General.Map.FormatInterface.SectorParallaxedFlag); } }
		public bool CeilingSloped { get { UpdateCeilingPlane(); return ceilingsloped; } }
		public bool CeilingSwapXY { get { return CheckCeilingFlag(General.Map.FormatInterface.SectorSwapXYFlag); } }
		public bool CeilingFlipX { get { return CheckCeilingFlag(General.Map.FormatInterface.SectorFlipXFlag); } }
		public bool CeilingFlipY { get { return CheckCeilingFlag(General.Map.FormatInterface.SectorFlipYFlag); } }
		public bool CeilingRelativeAlignment { get { return CheckCeilingFlag(General.Map.FormatInterface.SectorRelativeAlignmentFlag); } }
		public bool CeilingTextureExpansion { get { return CheckCeilingFlag(General.Map.FormatInterface.SectorTextureExpansionFlag); } }

		public bool FloorParallaxed { get { return CheckFloorFlag(General.Map.FormatInterface.SectorParallaxedFlag); } }
		public bool FloorSloped { get { UpdateFloorPlane(); return floorsloped; } }
		public bool FloorSwapXY { get { return CheckFloorFlag(General.Map.FormatInterface.SectorSwapXYFlag); } }
		public bool FloorFlipX { get { return CheckFloorFlag(General.Map.FormatInterface.SectorFlipXFlag); } }
		public bool FloorFlipY { get { return CheckFloorFlag(General.Map.FormatInterface.SectorFlipYFlag); } }
		public bool FloorRelativeAlignment { get { return CheckFloorFlag(General.Map.FormatInterface.SectorRelativeAlignmentFlag); } }
		public bool FloorTextureExpansion { get { return CheckFloorFlag(General.Map.FormatInterface.SectorTextureExpansionFlag); } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal Sector(MapSet map, int listindex)
		{
			// Initialize
			this.map = map;
			this.listindex = listindex;
			this.sidedefs = new LinkedList<Sidedef>();
			this.updateneeded = true;
			this.ceilingplaneupdateneeded = true;
			this.floorplaneupdateneeded = true;
			this.triangulationneeded = true;
			this.surfaceentries = new SurfaceEntryCollection();
			this.floorflags = new Dictionary<string, bool>(StringComparer.Ordinal);
			this.ceilingflags = new Dictionary<string, bool>(StringComparer.Ordinal);
			this.firstwall = null;
			this.extra = -1;
			this.floorslope = Angle2D.PIHALF; //mxd
			this.ceilingslope = Angle2D.PIHALF; //mxd

			if(map == General.Map.Map) General.Map.UndoRedo.RecAddSector(this);
		}

		// Disposer
		public override void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Already set isdisposed so that changes can be prohibited
				isdisposed = true;
				
				// Dispose the sidedefs that are attached to this sector
				// because a sidedef cannot exist without reference to its sector.
				if(map.AutoRemove)
					foreach(Sidedef sd in sidedefs) sd.Dispose();
				else
					foreach(Sidedef sd in sidedefs) sd.SetSectorP(null);
				
				if(map == General.Map.Map)
					General.Map.UndoRedo.RecRemSector(this);

				// Remove from main list
				map.RemoveSector(listindex);
				
				// Free surface entry
				General.Map.CRenderer2D.Surfaces.FreeSurfaces(surfaceentries);

				// Clean up
				firstwall = null;
				sidedefs = null;
				map = null;
				
				// Dispose base
				base.Dispose();
			}
		}

		#endregion

		#region ================== Management

		// Call this before changing properties
		protected override void BeforePropsChange()
		{
			if(map == General.Map.Map) General.Map.UndoRedo.RecPrpSector(this);
		}

		// Serialize / deserialize (passive: this doesn't record)
		internal void ReadWrite(IReadWriteStream s)
		{
			if(!s.IsWriting)
			{
				BeforePropsChange();
				updateneeded = true;
				ceilingplaneupdateneeded = true;
				floorplaneupdateneeded = true;
			}

			ReadWrite(s, ref floorflags);
			ReadWrite(s, ref ceilingflags);

			if(s.IsWriting)
			{
				int firstwallindex = (firstwall == null ? -1 : firstwall.Index);
				s.rwInt(ref firstwallindex);
			}
			else
			{
				int firstwallindex = -1;
				s.rwInt(ref firstwallindex);

				if(firstwallindex != -1)
				{
					foreach(var wall in sidedefs)
					{
						if(wall.Index == firstwallindex)
						{
							firstwall = wall;
							break;
						}
					}

					if(firstwall == null) throw new Exception("Failed to restore FirstWall!");
				}
			}

			s.rwInt(ref ceilingheight);
			s.rwInt(ref floorheight);

			s.rwInt(ref ceilingtileindex);
			s.rwFloat(ref ceilingslope);
			s.rwInt(ref ceilingshade);
			s.rwInt(ref ceilingpaletteindex);
			s.rwInt(ref ceilingoffsetx);
			s.rwInt(ref ceilingoffsety);

			s.rwInt(ref floortileindex);
			s.rwFloat(ref floorslope);
			s.rwInt(ref floorshade);
			s.rwInt(ref floorpaletteindex);
			s.rwInt(ref flooroffsetx);
			s.rwInt(ref flooroffsety);

			s.rwInt(ref visibility);

			s.rwInt(ref hitag);
			s.rwInt(ref lotag);
			s.rwInt(ref extra);
		}
		
		// After deserialization
		internal void PostDeserialize(MapSet map)
		{
			triangles.PostDeserialize(map);
			updateneeded = true;
			triangulationneeded = true;
			ceilingplaneupdateneeded = true;
			floorplaneupdateneeded = true;
		}
		
		// This copies all properties to another sector
		public void CopyPropertiesTo(Sector s)
		{
			s.BeforePropsChange();

			// Copy build properties
			s.ceilingheight = ceilingheight;
			s.floorheight = floorheight;

			s.ceilingflags = new Dictionary<string, bool>(ceilingflags, StringComparer.Ordinal);
			s.floorflags = new Dictionary<string, bool>(floorflags, StringComparer.Ordinal);

			s.ceilingtileindex = ceilingtileindex;
			s.ceilingslope = ceilingslope;
			s.ceilingshade = ceilingshade;
			s.ceilingpaletteindex = ceilingpaletteindex;
			s.ceilingoffsetx = ceilingoffsetx;
			s.ceilingoffsety = ceilingoffsety;

			s.floortileindex = floortileindex;
			s.floorslope = floorslope;
			s.floorshade = floorshade;
			s.floorpaletteindex = floorpaletteindex;
			s.flooroffsetx = flooroffsetx;
			s.flooroffsety = flooroffsety;

			s.visibility = visibility;

			s.hitag = hitag;
			s.lotag = lotag;
			s.extra = extra;

			// Copy internal properties
			s.updateneeded = true;
			s.ceilingplaneupdateneeded = true;
			s.floorplaneupdateneeded = true;
			base.CopyPropertiesTo(s);
		}

		// This attaches a sidedef and returns the listitem
		internal LinkedListNode<Sidedef> AttachSidedefP(Sidedef sd)
		{
			//mxd. Set first wall?
			if(firstwall == null) firstwall = sd;
			
			updateneeded = true;
			triangulationneeded = true;
			ceilingplaneupdateneeded = true;
			floorplaneupdateneeded = true;
			return sidedefs.AddLast(sd);
		}

		// This detaches a sidedef
		internal void DetachSidedefP(LinkedListNode<Sidedef> l)
		{
			// Not disposing?
			if(!isdisposed)
			{
				// Remove sidedef
				updateneeded = true;
				ceilingplaneupdateneeded = true;
				floorplaneupdateneeded = true;
				triangulationneeded = true;
				sidedefs.Remove(l);

				// No more sidedefs left?
				if(sidedefs.Count == 0 && map.AutoRemove)
				{
					// This sector is now useless, dispose it
					this.Dispose();
				}
				//mxd. FirstWall was detached?
				else if(l.Value == firstwall)
				{
					firstwall = (sidedefs.Count > 0 ? sidedefs.First.Value : null);
				}
			}
		}
		
		// This triangulates the sector geometry
		internal void Triangulate()
		{
			if(updateneeded)
			{
				// Triangulate again?
				if(triangulationneeded || (triangles == null))
				{
					// Triangulate sector
					triangles = Triangulation.Create(this);
					triangulationneeded = false;
					updateneeded = true;
					
					//mxd. Update label position
					label = FindLabelPosition(this);
					
					// Number of vertices changed?
					if(triangles.Vertices.Count != surfaceentries.TotalVertices)
						General.Map.CRenderer2D.Surfaces.FreeSurfaces(surfaceentries);
				}
			}
		}
		
		// This makes new vertices as well as floor and ceiling surfaces
		internal void CreateSurfaces()
		{
			if(updateneeded)
			{
				// Floor color
				int color = CalculateBrightness(floorshade);

				// Make floor vertices
				floorvertices = new FlatVertex[triangles.Vertices.Count];
				for(int i = 0; i < triangles.Vertices.Count; i++)
				{
					floorvertices[i].x = triangles.Vertices[i].x;
					floorvertices[i].y = triangles.Vertices[i].y;
					floorvertices[i].z = 1.0f;
					floorvertices[i].c = color;
				}

				// Create bounding box
				bbox = CreateBBox();
				
				// Make update info (this lets the plugin fill in texture coordinates and such)
				SurfaceUpdate updateinfo = new SurfaceUpdate(floorvertices.Length, true, true);

				General.Plugins.OnSectorFloorSurfaceUpdate(this, ref floorvertices);
				floorvertices.CopyTo(updateinfo.FloorVertices, 0);

				// Ceiling color
				color = CalculateBrightness(ceilingshade);

				// Make ceiling vertices
				ceilingvertices = new FlatVertex[triangles.Vertices.Count];
				for(int i = 0; i < triangles.Vertices.Count; i++)
				{
					ceilingvertices[i].x = triangles.Vertices[i].x;
					ceilingvertices[i].y = triangles.Vertices[i].y;
					ceilingvertices[i].z = 1.0f;
					ceilingvertices[i].c = color;
				}

				General.Plugins.OnSectorCeilingSurfaceUpdate(this, ref ceilingvertices);
				ceilingvertices.CopyTo(updateinfo.CeilingVertices, 0);
				
				updateinfo.FloorTileIndex = floortileindex;
				updateinfo.CeilingTileIndex = ceilingtileindex;

				// Update surfaces
				General.Map.CRenderer2D.Surfaces.UpdateSurfaces(surfaceentries, updateinfo);

				// Updated
				updateneeded = false;
			}
		}

		// This updates the floor surface
		public void UpdateFloorSurface()
		{
			if(floorvertices == null) return;
			
			// Create floor vertices
			SurfaceUpdate updateinfo = new SurfaceUpdate(floorvertices.Length, true, false);
			General.Plugins.OnSectorFloorSurfaceUpdate(this, ref floorvertices);
			floorvertices.CopyTo(updateinfo.FloorVertices, 0);
			updateinfo.FloorTileIndex = floortileindex;
			
			// Update entry
			General.Map.CRenderer2D.Surfaces.UpdateSurfaces(surfaceentries, updateinfo);
			General.Map.CRenderer2D.Surfaces.UnlockBuffers();
		}

		// This updates the ceiling surface
		public void UpdateCeilingSurface()
		{
			if(floorvertices == null) return;

			// Create ceiling vertices
			SurfaceUpdate updateinfo = new SurfaceUpdate(floorvertices.Length, false, true);
			General.Plugins.OnSectorCeilingSurfaceUpdate(this, ref ceilingvertices);
			ceilingvertices.CopyTo(updateinfo.CeilingVertices, 0);
			updateinfo.CeilingTileIndex = ceilingtileindex;
			
			// Update entry
			General.Map.CRenderer2D.Surfaces.UpdateSurfaces(surfaceentries, updateinfo);
			General.Map.CRenderer2D.Surfaces.UnlockBuffers();
		}
		
		// This updates the sector when changes have been made
		public void UpdateCache()
		{
			// Update if needed
			if(updateneeded)
			{
				Triangulate();
				CreateSurfaces();
				General.Map.CRenderer2D.Surfaces.UnlockBuffers();
			}
		}

		// Selected
		protected override void DoSelect()
		{
			base.DoSelect();
			selecteditem = map.SelectedSectors.AddLast(this);
			General.MainWindow.UpdateStatistics(); //mxd
		}

		// Deselect
		protected override void DoUnselect()
		{
			base.DoUnselect();
			if(selecteditem.List != null) selecteditem.List.Remove(selecteditem);
			selecteditem = null;
			General.MainWindow.UpdateStatistics(); //mxd
		}
		
		#endregion
		
		#region ================== Methods

		// This checks and returns a flag without creating it
		public bool IsFlagSet(string flagname, bool floor)
		{
			if(floor) return floorflags.ContainsKey(flagname) && floorflags[flagname];
			return ceilingflags.ContainsKey(flagname) && ceilingflags[flagname];
		}

		// This sets a flag
		public void SetFlag(string flagname, bool value, bool floor)
		{
			var flags = (floor ? floorflags : ceilingflags);
			if(!flags.ContainsKey(flagname) || (IsFlagSet(flagname, floor) != value))
			{
				BeforePropsChange();
				flags[flagname] = value;
				updateneeded = true; //mxd. FlipXY flags require surface update
			}
		}

		// This returns a copy of the flags dictionary
		public Dictionary<string, bool> GetFlags(bool floor)
		{
			return new Dictionary<string, bool>(floor ? floorflags : ceilingflags, StringComparer.Ordinal);
		}

		// This clears all flags
		public void ClearFlags(bool floor)
		{
			BeforePropsChange();
			if(floor) floorflags.Clear(); else ceilingflags.Clear();
		}
		
		// This checks if the given point is inside the sector polygon
		// See: http://paulbourke.net/geometry/polygonmesh/index.html#insidepoly
		public bool Intersect(Vector2D p)
		{
			//mxd. Check bounding box first
			if(p.x < bbox.Left || p.x > bbox.Right || p.y < bbox.Top || p.y > bbox.Bottom) return false;

			uint c = 0;
			Vector2D v1, v2;

			// Go for all sidedefs
			foreach(Sidedef sd in sidedefs)
			{
				// Get vertices
				v1 = sd.Line.Start.Position;
				v2 = sd.Line.End.Position;

				//mxd. On top of a vertex?
				if(p == v1 || p == v2) return true;

				// Check for intersection
				if(v1.y != v2.y //mxd. If line is not horizontal...
				  && p.y > (v1.y < v2.y ? v1.y : v2.y) //mxd. ...And test point y intersects with the line y bounds...
				  && p.y <= (v1.y > v2.y ? v1.y : v2.y) //mxd
				  && (p.x < (v1.x < v2.x ? v1.x : v2.x) || (p.x <= (v1.x > v2.x ? v1.x : v2.x) //mxd. ...And test point x is to the left of the line, or is inside line x bounds and intersects it
						&& (v1.x == v2.x || p.x <= ((p.y - v1.y) * (v2.x - v1.x) / (v2.y - v1.y) + v1.x)))))
					c++; //mxd. ...Count the line as crossed
			}

			// Inside this polygon when we crossed odd number of polygon lines
			return (c % 2 != 0);
		}
		
		// This creates a bounding box rectangle
		// This requires the sector triangulation to be up-to-date!
		private RectangleF CreateBBox()
		{
			// Setup
			float left = float.MaxValue;
			float top = float.MaxValue;
			float right = float.MinValue;
			float bottom = float.MinValue;
			
			// Go for vertices
			foreach(Vector2D v in triangles.Vertices)
			{
				// Update rect
				if(v.x < left) left = v.x;
				if(v.y < top) top = v.y;
				if(v.x > right) right = v.x;
				if(v.y > bottom) bottom = v.y;
			}
			
			// Return rectangle
			return new RectangleF(left, top, right - left, bottom - top);
		}
		
		// This joins the sector with another sector
		// This sector will be disposed
		public void Join(Sector other)
		{
			// Any sidedefs to move?
			if(sidedefs.Count > 0)
			{
				// Change secter reference on my sidedefs
				// This automatically disposes this sector
				while(sidedefs != null)
					sidedefs.First.Value.SetSector(other);
			}
			else
			{
				// No sidedefs attached
				// Dispose manually
				this.Dispose();
			}
			
			General.Map.IsChanged = true;
		}

		//mxd. Moved here from Tools. This finds the ideal label position for a sector
		private static LabelPositionInfo FindLabelPosition(Sector s)
		{
			// Do we have a triangulation?
			Triangulation triangles = s.Triangles;
			if(triangles != null)
			{
				// Go for all islands
				foreach(int iv in triangles.IslandVertices)
				{
					Dictionary<Sidedef, Linedef> sides = new Dictionary<Sidedef, Linedef>(iv >> 1);
					List<Vector2D> candidatepositions = new List<Vector2D>(iv >> 1);
					float founddistance = float.MinValue;
					Vector2D foundposition = new Vector2D();
					float minx = float.MaxValue;
					float miny = float.MaxValue;
					float maxx = float.MinValue;
					float maxy = float.MinValue;

					// Make candidate lines that are not along sidedefs
					// We do this before testing the candidate against the sidedefs so that
					// we can collect the relevant sidedefs first in the same run
					for(int t = 0; t < iv; t += 3)
					{
						Vector2D v1 = triangles.Vertices[t + 2];
						Sidedef sd = triangles.Sidedefs[t + 2];
						for(int v = 0; v < 3; v++)
						{
							Vector2D v2 = triangles.Vertices[t + v];

							// Not along a sidedef? Then this line is across the sector
							// and guaranteed to be inside the sector!
							if(sd == null)
							{
								// Make the line
								candidatepositions.Add(v1 + (v2 - v1) * 0.5f);
							}
							else
							{
								// This sidedefs is part of this island and must be checked
								// so add it to the dictionary
								sides[sd] = sd.Line;
							}

							// Make bbox of this island
							minx = Math.Min(minx, v1.x);
							miny = Math.Min(miny, v1.y);
							maxx = Math.Max(maxx, v1.x);
							maxy = Math.Max(maxy, v1.y);

							// Next
							sd = triangles.Sidedefs[t + v];
							v1 = v2;
						}
					}

					// Any candidate lines found at all?
					if(candidatepositions.Count > 0)
					{
						// Start with the first line
						foreach(Vector2D candidatepos in candidatepositions)
						{
							// Check distance against other lines
							float smallestdist = int.MaxValue;
							foreach(KeyValuePair<Sidedef, Linedef> sd in sides)
							{
								// Check the distance
								float distance = sd.Value.DistanceToSq(candidatepos, true);
								smallestdist = Math.Min(smallestdist, distance);
							}

							// Keep this candidate if it is better than previous
							if(smallestdist > founddistance)
							{
								foundposition = candidatepos;
								founddistance = smallestdist;
							}
						}

						// No cceptable line found, just use the first!
						return new LabelPositionInfo(foundposition, (float)Math.Sqrt(founddistance));
					}

					// No candidate lines found.
					// Check to see if the island is a triangle
					if(iv == 3)
					{
						// Use the center of the triangle
						// TODO: Use the 'incenter' instead, see http://mathworld.wolfram.com/Incenter.html
						Vector2D v = (triangles.Vertices[0] + triangles.Vertices[1] + triangles.Vertices[2]) / 3.0f;
						float d = Line2D.GetDistanceToLineSq(triangles.Vertices[0], triangles.Vertices[1], v, false);
						d = Math.Min(d, Line2D.GetDistanceToLineSq(triangles.Vertices[1], triangles.Vertices[2], v, false));
						d = Math.Min(d, Line2D.GetDistanceToLineSq(triangles.Vertices[2], triangles.Vertices[0], v, false));
						return new LabelPositionInfo(v, (float)Math.Sqrt(d));
					}
					else
					{
						// Use the center of this island.
						float d = Math.Min((maxx - minx) * 0.5f, (maxy - miny) * 0.5f);
						return new LabelPositionInfo(new Vector2D(minx + (maxx - minx) * 0.5f, miny + (maxy - miny) * 0.5f), d);
					}
				}
			}
			else
			{
				// No triangulation was made. FAIL!
				General.Fail("No triangulation exists for sector " + s + " Triangulation is required to create label positions for a sector.");
			}

			// Nothing found...
			return new LabelPositionInfo();
		}

		//mxd
		private bool CheckFloorFlag(string flagname)
		{
			return (!string.IsNullOrEmpty(flagname) && floorflags.ContainsKey(flagname) && floorflags[flagname]);
		}

		//mxd
		private bool CheckCeilingFlag(string flagname)
		{
			return (!string.IsNullOrEmpty(flagname) && ceilingflags.ContainsKey(flagname) && ceilingflags[flagname]);
		}

		// String representation
		public override string ToString()
		{
			return "Sector " + listindex;
		}
		
		#endregion

		#region ================== Changes

		// This updates all properties
		internal void Update(BuildSector src)
		{
			BeforePropsChange();
			
			// Apply changes
			// FirstWall needs to be set separately!
			this.ceilingheight = src.CeilingHeight;
			this.floorheight = src.FloorHeight;

			this.ceilingflags = new Dictionary<string, bool>(src.CeilingFlags, StringComparer.Ordinal);
			this.floorflags = new Dictionary<string, bool>(src.FloorFlags, StringComparer.Ordinal);

			this.ceilingtileindex = src.CeilingTileIndex;
			this.ceilingslope = src.CeilingSlope;
			this.ceilingshade = src.CeilingShade;
			this.ceilingpaletteindex = src.CeilingPaletteIndex;
			this.ceilingoffsetx = src.CeilingOffsetX;
			this.ceilingoffsety = src.CeilingOffsetY;

			this.floortileindex = src.FloorTileIndex;
			this.floorslope = src.FloorSlope;
			this.floorshade = src.FloorShade;
			this.floorpaletteindex = src.FloorPaletteIndex;
			this.flooroffsetx = src.FloorOffsetX;
			this.flooroffsety = src.FloorOffsetY;

			this.visibility = src.Visibility;

			this.hitag = src.HiTag;
			this.lotag = src.LoTag;
			this.extra = src.Extra;

			updateneeded = true;
			ceilingplaneupdateneeded = true;
			floorplaneupdateneeded = true;
			General.Map.IsChanged = true;
		}

		//mxd
		private void UpdateCeilingPlane()
		{
			if(ceilingplaneupdateneeded)
			{
				ceilingsloped = (CheckCeilingFlag(General.Map.FormatInterface.SectorSlopeFlag) && ceilingslope != 0);
				ceilingplane = (ceilingsloped ? GetSlopePlane(ceilingslope, ceilingheight, false) : new Plane(new Vector3D(0, 0, -1), ceilingheight));
				ceilingplaneupdateneeded = false;

				// "Align to first wall" flag + slope needs UV recalculation...
				UpdateCeilingSurface();
			}
		}

		//mxd
		private void UpdateFloorPlane()
		{
			if(floorplaneupdateneeded)
			{
				floorsloped = (CheckFloorFlag(General.Map.FormatInterface.SectorSlopeFlag) && floorslope != 0);
				floorplane = (floorsloped ? GetSlopePlane(floorslope, floorheight, true) : new Plane(new Vector3D(0, 0, 1), -floorheight));
				floorplaneupdateneeded = false;

				// "Align to first wall" flag + slope needs UV recalculation...
				UpdateFloorSurface();
			}
		}

		//mxd
		private Plane GetSlopePlane(float slopeangle, int planeheight, bool up)
		{
			Vector3D center = new Vector3D(firstwall.Line.GetCenterPoint(), planeheight);
			float anglexy = (up ? firstwall.Line.Angle - Angle2D.PIHALF : firstwall.Line.Angle + Angle2D.PIHALF);
			float anglez = (up ? -slopeangle : slopeangle);
			return new Plane(center, anglexy, anglez, up);
		}
		
		#endregion
	}
}
