
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
using mxd.DukeBuilder.Data;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.Types;
using SlimDX;

#endregion

namespace mxd.DukeBuilder.Geometry
{
	/// <summary>
	/// Tools to work with geometry.
	/// </summary>
	public static class Tools
	{
		#region ================== Structures

		private class SidedefSettings
		{
			public int TileIndex = -1;
			public int Shade = -1;
		}

		private struct SidedefAlignJob
		{
			public Sidedef sidedef;
			
			public int offsetx;

			// When this is true, the previous sidedef was on the left of
			// this one and the texture X offset of this sidedef can be set
			// directly. When this is false, the length of this sidedef
			// must be subtracted from the X offset first.
			public bool forward;
		}

		private struct SidedefFillJob
		{
			public Sidedef sidedef;

			// Moving forward along the sidedef?
			public bool forward;
		}

		#endregion
		
		#region ================== Constants
		
		#endregion

		#region ================== Polygons and Triangles

		// Point inside the polygon?
		// See: http://local.wasp.uwa.edu.au/~pbourke/geometry/insidepoly/
		public static bool PointInPolygon(ICollection<Vector2D> polygon, Vector2D point)
		{
			Vector2D v1 = General.GetByIndex(polygon, polygon.Count - 1);
			uint c = 0;

			// Go for all vertices
			foreach(Vector2D v2 in polygon)
			{
				// Determine min/max values
				float miny = Math.Min(v1.y, v2.y);
				float maxy = Math.Max(v1.y, v2.y);
				float maxx = Math.Max(v1.x, v2.x);

				// Check for intersection
				if((point.y > miny) && (point.y <= maxy))
				{
					if(point.x <= maxx)
					{
						if(v1.y != v2.y)
						{
							float xint = (point.y - v1.y) * (v2.x - v1.x) / (v2.y - v1.y) + v1.x;
							if((v1.x == v2.x) || (point.x <= xint)) c++;
						}
					}
				}

				// Move to next
				v1 = v2;
			}

			// Inside this polygon?
			return (c & 0x00000001UL) != 0;
		}
		
		#endregion
		
		#region ================== Pathfinding

		/// <summary>
		/// This finds a potential sector at the given coordinates,
		/// or returns null when a sector is not possible there.
		/// </summary>
		public static List<LinedefSide> FindPotentialSectorAt(Vector2D pos)
		{
			// Find the nearest line and determine side, then use the other method to create the sector
			Linedef l = General.Map.Map.NearestLinedef(pos);
			return FindPotentialSectorAt(l, (l.SideOfLine(pos) <= 0));
		}

		/// <summary>
		/// This finds a potential sector starting at the given line and side,
		/// or returns null when sector is not possible.
		/// </summary>
		public static List<LinedefSide> FindPotentialSectorAt(Linedef line, bool front)
		{
			List<LinedefSide> alllines = new List<LinedefSide>();
			
			// Find the outer lines
			EarClipPolygon p = FindOuterLines(line, front, alllines, General.Map.Map.Linedefs);
			if(p != null)
			{
				// Find the inner lines
				FindInnerLines(p, alllines, General.Map.Map.Vertices, false);
				return alllines;
			}

			return null;
		}

		//mxd. Finds outer and inner wall loops for map saving
		internal static List<List<Sidedef>> GetSortedSectorSides(Sector target)
		{
			List<LinedefSide> alllines = new List<LinedefSide>();
			List<Linedef> sectorlines = new List<Linedef>();
			foreach(Sidedef side in target.Sidedefs) sectorlines.Add(side.Line);

			// Find the outer lines
			Sidedef start = General.GetFirst(target.Sidedefs);
			EarClipPolygon p = FindOuterLines(start.Line, start.IsFront, alllines, sectorlines);
			if(p == null) return null;


			HashSet<Vertex> sectorverts = new HashSet<Vertex>();
			foreach(Linedef l in sectorlines)
			{
				sectorverts.Add(l.Start);
				sectorverts.Add(l.End);
			}
				
			// Find the inner lines
			var sideloops = FindInnerLines(p, alllines, sectorverts, true);

			// First loop must be the one with sector's FirstWall, and should start with it...
			var result = new List<List<Sidedef>>(sideloops.Count);

			// Find and add the FirstWall loop
			for(int i = 0; i < sideloops.Count; i++)
			{
				int pos = sideloops[i].IndexOf(target.FirstWall);
				if(pos != -1)
				{
					var firstwallloop = new List<Sidedef>(sideloops[i].Count);
					
					// Add walls starting with FirstWall
					for(int j = pos; j < sideloops[i].Count; j++) firstwallloop.Add(sideloops[i][j]);

					// Add walls before FirstWall
					for(int j = 0; j < pos; j++) firstwallloop.Add(sideloops[i][j]);

					// Add to result, remove from found loops
					result.Add(firstwallloop);
					sideloops.RemoveAt(i);
					break;
				}
			}

			// Add the rest of the loops
			result.AddRange(sideloops);
			return result;
		}

		// This finds the inner lines of the sector and adds them to the sector polygon
		private static List<List<Sidedef>> FindInnerLines(EarClipPolygon p, List<LinedefSide> alllines, ICollection<Vertex> searchverts, bool returnsidelists)
		{
			bool findmore;
			float foundangle = 0f;
			RectangleF bbox = p.CreateBBox();

			//mxd. alllines is supposed to contain outer lines only
			List<List<Sidedef>> sidelists = new List<List<Sidedef>>();
			if(returnsidelists)
			{
				List<Sidedef> outersides = new List<Sidedef>(alllines.Count);
				foreach(LinedefSide ls in alllines)
					outersides.Add(ls.Front ? ls.Line.Front : ls.Line.Back);

				sidelists.Add(outersides);
			}
			
			do
			{
				findmore = false;

				// Go for all vertices to find the right-most vertex inside the polygon
				Vertex foundv = null;
				foreach(Vertex v in searchverts)
				{
					// Inside the polygon bounding box?
					if((v.Position.x >= bbox.Left) && (v.Position.x <= bbox.Right) &&
					   (v.Position.y >= bbox.Top) && (v.Position.y <= bbox.Bottom))
					{
						// More to the right?
						if((foundv == null) || (v.Position.x >= foundv.Position.x))
						{
							// Vertex is inside the polygon?
							if(p.Intersect(v.Position))
							{
								// Vertex has lines attached?
								if(v.Linedefs.Count > 0)
								{
									// Go for all lines to see if the vertex is not of the polygon itsself
									bool vvalid = true;
									foreach(LinedefSide ls in alllines)
									{
										if((ls.Line.Start == v) || (ls.Line.End == v))
										{
											vvalid = false;
											break;
										}
									}

									// Valid vertex?
									if(vvalid) foundv = v;
								}
							}
						}
					}
				}

				// Found a vertex inside the polygon?
				if(foundv != null)
				{
					// Find the attached linedef with the smallest angle to the right
					Linedef foundline = null;
					foreach(Linedef l in foundv.Linedefs)
					{
						// We need an angle unrelated to line direction, so correct for that
						float lineangle = l.Angle;
						if(l.End == foundv) lineangle += Angle2D.PI;

						// Better result?
						float deltaangle = Angle2D.Difference(Angle2D.PIHALF, lineangle);
						if((foundline == null) || (deltaangle < foundangle))
						{
							foundline = l;
							foundangle = deltaangle;
						}
					}

					// We already know that each linedef will go from this vertex
					// to the left, because this is the right-most vertex in this area.
					// If the line would go to the right, that means the other vertex of
					// that line must lie outside this area and the mapper made an error.
					// Should I check for this error and fail to create a sector in
					// that case or ignore it and create a malformed sector (possibly
					// breaking another sector also)?

					// Find the side at which to start pathfinding
					Vector2D testpos = new Vector2D(100.0f, 0.0f);
					bool foundlinefront = (foundline.SideOfLine(foundv.Position + testpos) < 0.0f);

					// Find inner path
					List<LinedefSide> innerlines = FindClosestPath(foundline, foundlinefront, true);
					if(innerlines != null)
					{
						// Make polygon
						LinedefTracePath tracepath = new LinedefTracePath(innerlines);
						EarClipPolygon innerpoly = tracepath.MakePolygon(true);

						// Check if the front of the line is outside the polygon
						if(!innerpoly.Intersect(foundline.GetSidePoint(foundlinefront)))
						{
							// Valid hole found!
							alllines.AddRange(innerlines);
							p.InsertChild(innerpoly);
							findmore = true;

							//mxd. Also store found inner lines as a separate collection...
							if(returnsidelists)
							{
								List<Sidedef> innersides = new List<Sidedef>(innerlines.Count);
								foreach(LinedefSide ls in innerlines)
									innersides.Add(ls.Front ? ls.Line.Front : ls.Line.Back);

								sidelists.Add(innersides);
							}
						}
					}
				}
			}
			// Continue until no more holes found
			while(findmore);

			//mxd
			return sidelists;
		}

		// This finds the outer lines of the sector as a polygon
		// Returns null when no valid outer polygon can be found
		private static EarClipPolygon FindOuterLines(Linedef line, bool front, List<LinedefSide> alllines, ICollection<Linedef> searchlines)
		{
			Linedef scanline = line;
			bool scanfront = front;
			Vector2D sidepoint = line.GetSidePoint(front); //mxd

			do
			{
				// Find closest path
				List<LinedefSide> pathlines = FindClosestPath(scanline, scanfront, true);
				if(pathlines != null)
				{
					// Make polygon
					LinedefTracePath tracepath = new LinedefTracePath(pathlines);
					EarClipPolygon poly = tracepath.MakePolygon(true);

					//mxd. Check bbox first...
					RectangleF bbox = poly.CreateBBox();
					bool outsidebbox = (sidepoint.x < bbox.Left || sidepoint.x > bbox.Right || sidepoint.y < bbox.Top || sidepoint.y > bbox.Bottom);

					// Check if the front of the line is inside the polygon
					if(!outsidebbox && poly.Intersect(sidepoint))
					{
						// Outer lines found!
						alllines.AddRange(pathlines);
						return poly;
					}
					else
					{
						// Inner lines found. This is not what we need, we want the outer lines.
						// Find the right-most vertex to start a scan from there towards the outer lines.
						Vertex foundv = null;
						foreach(LinedefSide ls in pathlines)
						{
							if((foundv == null) || (ls.Line.Start.Position.x > foundv.Position.x))
								foundv = ls.Line.Start;
							
							if((foundv == null) || (ls.Line.End.Position.x > foundv.Position.x))
								foundv = ls.Line.End;
						}

						// If foundv is null then something is horribly wrong with the
						// path we received from FindClosestPath!
						if(foundv == null) throw new Exception("FAIL!");
						
						// From the right-most vertex trace outward to the right to find the next closest linedef,
						// this is based on the idea that all sectors are closed.

						//mxd. Intersection test is bounded, so extend end position x to the right map boundary
						Line2D testline = new Line2D(foundv.Position, new Vector2D(General.Map.FormatInterface.RightBoundary, foundv.Position.y));
						scanline = null;
						float foundu = float.MaxValue;

						float px = foundv.Position.x; //mxd
						float py = foundv.Position.y; //mxd

						foreach(Linedef ld in searchlines)
						{
							// Line to the right of start point?
							if(ld.Start.Position.x > px || ld.End.Position.x > px)
							{
								// Line intersecting the y axis?
								if((ld.Start.Position.y >= py && ld.End.Position.y <= py)
									|| (ld.Start.Position.y <= py && ld.End.Position.y >= py))
								{
									// Check if this linedef intersects our test line at a closer range
									float thisu;
									ld.Line.GetIntersection(testline, out thisu);
									if(!float.IsNaN(thisu) && thisu > 0.00001f)
									{
										if(thisu < foundu)
										{
											scanline = ld;
											foundu = thisu;
										}
										//mxd. Special cases: when foundv.y matches ld's start or end y, 
										// prefer the line, which is clser to being parallel to the x axis
										else if(scanline != null && Math.Round(thisu, 4) == Math.Round(foundu, 4))
										{
											float ldanglerel, scanlineanglerel;
											if(GetRelativeAngle(ld, foundv.Position, out ldanglerel)
												&& GetRelativeAngle(scanline, foundv.Position, out scanlineanglerel)
												&& (ldanglerel < scanlineanglerel))
											{
												scanline = ld; // foundu already matches
											}
										}
									}
								}
							}
						}

						// Did we meet another line?
						if(scanline != null)
						{
							// Determine on which side we should start the next pathfind
							scanfront = (scanline.SideOfLine(foundv.Position) < 0.0f);
						}
						else
						{
							// Appearently we reached the end of the map, no sector possible here
							return null;
						}
					}
				}
				else
				{
					// Can't find a path
					return null;
				}
			}
			while(true);
		}

		//mxd. Gets angle between pos and l when pos.y matches l.Start.Position.y or l.End.Position.y
		private static bool GetRelativeAngle(Linedef l, Vector2D pos, out float result)
		{
			if(l.Start.Position.y == pos.y)
			{
				result = Angle2D.GetAngle(pos, l.Start.Position, l.End.Position);
				return true;
			}

			if(l.End.Position.y == pos.y)
			{
				result = Angle2D.GetAngle(pos, l.End.Position, l.Start.Position);
				return true;
			}

			// We just don't know...
			result = float.MaxValue;
			return false;
		}

		/// <summary>
		/// This finds the closest path from the beginning of a line to the end of the line.
		/// When turnatends is true, the algorithm will continue at the other side of the
		/// line when a dead end has been reached. Returns null when no path could be found.
		/// </summary>
		public static List<LinedefSide> FindClosestPath(Linedef startline, bool startfront, bool turnatends)
		{
			return FindClosestPath(startline, startfront, startline, startfront, turnatends);
		}
		
		/// <summary>
		/// This finds the closest path from the beginning of a line to the end of the line.
		/// When turnatends is true, the algorithm will continue at the other side of the
		/// line when a dead end has been reached. Returns null when no path could be found.
		/// </summary>
		public static List<LinedefSide> FindClosestPath(Linedef startline, bool startfront, Linedef endline, bool endfront, bool turnatends)
		{
			List<LinedefSide> path = new List<LinedefSide>();
			Dictionary<Linedef, int> tracecount = new Dictionary<Linedef, int>();
			Linedef nextline = startline;
			bool nextfront = startfront;

			do
			{
				// Add line to path
				path.Add(new LinedefSide(nextline, nextfront));

				// Determine next vertex to use
				Vertex v = nextfront ? nextline.End : nextline.Start;

				// Get list of linedefs and sort by angle
				List<Linedef> lines = new List<Linedef>(v.Linedefs);
				LinedefAngleSorter sorter = new LinedefAngleSorter(nextline, nextfront, v);
				lines.Sort(sorter);

				// Source line is the only one?
				if(lines.Count == 1)
				{
					// Are we allowed to trace along this line again?
					if(turnatends && (!tracecount.ContainsKey(nextline) || (tracecount[nextline] < 3)))
					{
						// Turn around and go back along the other side of the line
						nextfront = !nextfront;
					}
					else
					{
						// No more lines, trace ends here
						path = null;
					}
				}
				else
				{
					// Trace along the next line
					Linedef prevline = nextline;
					nextline = (lines[0] == nextline ? lines[1] : lines[0]);

					//mxd. Try to pick a line with lower tracecount, otherwise we will just walk the same path trise
					int curcount = (!tracecount.ContainsKey(nextline) ? 0 : tracecount[nextline]);

					//mxd. Don't pick a different line for start and end lines, otherwise the path can go away from it instead of closing the path
					//mxd. Also don't pick a different line for marked lines (these are newly drawn lines, and we don't want to skip them)
					if(curcount > 0 && !nextline.Marked && nextline != startline && nextline != endline)
					{
						foreach(Linedef l in lines)
						{
							if(l != nextline && l != prevline && (!tracecount.ContainsKey(l) || tracecount[l] < curcount))
							{
								nextline = l;
								break;
							}
						}
					}

					// Are we allowed to trace this line again?
					if(!tracecount.ContainsKey(nextline) || (tracecount[nextline] < 3))
					{
						// Check if front side changes
						if((prevline.Start == nextline.Start) ||
						   (prevline.End == nextline.End)) nextfront = !nextfront;
					}
					else
					{
						// No more lines, trace ends here
						path = null;
					}
				}

				//mxd. Increase trace count
				if(!tracecount.ContainsKey(nextline)) tracecount.Add(nextline, 1); else tracecount[nextline]++;
			}
			// Continue as long as we have not reached the start yet
			// or we have no next line to trace
			while((path != null) && ((nextline != endline) || (nextfront != endfront)));

			// If start and front are not the same, add the end to the list also
			if((path != null) && ((startline != endline) || (startfront != endfront)))
				path.Add(new LinedefSide(endline, endfront));
			
			// Return path (null when trace failed)
			return path;
		}

		#endregion
		
		#region ================== Sector Making

		// This makes the sector from the given lines and sides
		// If nearbylines is not null, then this method will find the default
		// properties from the nearest line in this collection when the
		// default properties can't be found in the alllines collection.
		// Return null when no new sector could be made.
		public static Sector MakeSector(List<LinedefSide> alllines, List<Linedef> nearbylines)
		{
			Sector sourcesector = null;
			SidedefSettings sourceside = new SidedefSettings();
			bool foundsidedefaults = false;

			if(General.Map.Map.Sectors.Count >= General.Map.FormatInterface.MaxSectors)
				return null;

			Sector newsector = General.Map.Map.CreateSector();
			if(newsector == null) return null;
			
			// Check if any of the sides already has a sidedef
			// Then we use information from that sidedef to make the others
			foreach(LinedefSide ls in alllines)
			{
				if(ls.Front)
				{
					if(ls.Line.Front != null)
					{
						// Copy sidedef information if not already found
						sourcesector = ls.Line.Front.Sector;
						TakeSidedefSettings(ref sourceside, ls.Line.Front);
						foundsidedefaults = true;
						break;
					}
				}
				else
				{
					if(ls.Line.Back != null)
					{
						// Copy sidedef information if not already found
						sourcesector = ls.Line.Back.Sector;
						TakeSidedefSettings(ref sourceside, ls.Line.Back);
						foundsidedefaults = true;
						break;
					}
				}
			}
			
			// Now do the same for the other sides
			// Note how information is only copied when not already found
			// so this won't override information from the sides searched above
			foreach(LinedefSide ls in alllines)
			{
				if(ls.Front)
				{
					if(ls.Line.Back != null)
					{
						// Copy sidedef information if not already found
						if(sourcesector == null) sourcesector = ls.Line.Back.Sector;
						TakeSidedefSettings(ref sourceside, ls.Line.Back);
						foundsidedefaults = true;
						break;
					}
				}
				else
				{
					if(ls.Line.Front != null)
					{
						// Copy sidedef information if not already found
						if(sourcesector == null) sourcesector = ls.Line.Front.Sector;
						TakeSidedefSettings(ref sourceside, ls.Line.Front);
						foundsidedefaults = true;
						break;
					}
				}
			}
			
			// Use default settings from neares linedef, if settings have been found yet
			if( (nearbylines != null) && (alllines.Count > 0) && (!foundsidedefaults || (sourcesector == null)) )
			{
				Vector2D testpoint = alllines[0].Line.GetSidePoint(alllines[0].Front);
				Linedef nearest = MapSet.NearestLinedef(nearbylines, testpoint);
				if(nearest != null)
				{
					Sidedef defaultside;
					float side = nearest.SideOfLine(testpoint);
					if(side < 0.0f)
						defaultside = nearest.Front;
					else
						defaultside = nearest.Back;

					if(defaultside != null)
					{
						if(sourcesector == null) sourcesector = defaultside.Sector;
						TakeSidedefSettings(ref sourceside, defaultside);
					}
				}
			}
			
			// Use defaults where no settings could be found
			TakeSidedefDefaults(ref sourceside);
			
			// Found a source sector?
			if(sourcesector != null)
			{
				// Copy properties from source to new sector
				sourcesector.CopyPropertiesTo(newsector);
			}
			else
			{
				// No source sector, apply default sector properties
				ApplyDefaultsToSector(newsector);
			}

			// Go for all sides to make sidedefs
			foreach(LinedefSide ls in alllines)
			{
				// We may only remove a useless middle texture when
				// the line was previously singlesided
				//bool wassinglesided = (ls.Line.Back == null) || (ls.Line.Front == null);
				
				if(ls.Front)
				{
					// Create sidedef is needed and ensure it points to the new sector
					if(ls.Line.Front == null) General.Map.Map.CreateSidedef(ls.Line, true, newsector);
					if(ls.Line.Front == null) return null;
					if(ls.Line.Front.Sector != newsector) ls.Line.Front.SetSector(newsector);
					ApplyDefaultsToSidedef(ls.Line.Front, sourceside);
				}
				else
				{
					// Create sidedef is needed and ensure it points to the new sector
					if(ls.Line.Back == null) General.Map.Map.CreateSidedef(ls.Line, false, newsector);
					if(ls.Line.Back == null) return null;
					if(ls.Line.Back.Sector != newsector) ls.Line.Back.SetSector(newsector);
					ApplyDefaultsToSidedef(ls.Line.Back, sourceside);
				}

				// Update line
				//if(ls.Line.Front != null) ls.Line.Front.RemoveUnneededTextures(wassinglesided);
				//if(ls.Line.Back != null) ls.Line.Back.RemoveUnneededTextures(wassinglesided);

				// Apply single/double sided flags if the double-sided-ness changed
				/*if( (wassinglesided && ((ls.Line.Front != null) && (ls.Line.Back != null))) ||
					(!wassinglesided && ((ls.Line.Front == null) || (ls.Line.Back == null))))
					ls.Line.ApplySidedFlags();*/
			}

			// Return the new sector
			return newsector;
		}


		// This joins a sector with the given lines and sides. Returns null when operation could not be completed.
		public static Sector JoinSector(List<LinedefSide> alllines, Sidedef original)
		{
			SidedefSettings sourceside = new SidedefSettings();
			
			// Take settings from the original side
			TakeSidedefSettings(ref sourceside, original);

			// Use defaults where no settings could be found
			TakeSidedefDefaults(ref sourceside);

			// Go for all sides to make sidedefs
			foreach(LinedefSide ls in alllines)
			{
				if(ls.Front)
				{
					// Create sidedef if needed
					if(ls.Line.Front == null)
					{
						Sidedef sd = General.Map.Map.CreateSidedef(ls.Line, true, original.Sector);
						if(sd == null) return null;
						ApplyDefaultsToSidedef(ls.Line.Front, sourceside);
						//ls.Line.ApplySidedFlags();
						
						// We must remove the (now useless) middle texture on the other side
						//if(ls.Line.Back != null) ls.Line.Back.RemoveUnneededTextures(true, true);
					}
					// Added 23-9-08, can we do this or will it break things?
					else
					{
						// Link to the new sector
						ls.Line.Front.SetSector(original.Sector);
					}
				}
				else
				{
					// Create sidedef if needed
					if(ls.Line.Back == null)
					{
						Sidedef sd = General.Map.Map.CreateSidedef(ls.Line, false, original.Sector);
						if(sd == null) return null;
						ApplyDefaultsToSidedef(ls.Line.Back, sourceside);
						//ls.Line.ApplySidedFlags();

						// We must remove the (now useless) middle texture on the other side
						//if(ls.Line.Front != null) ls.Line.Front.RemoveUnneededTextures(true, true);
					}
					// Added 23-9-08, can we do this or will it break things?
					else
					{
						// Link to the new sector
						ls.Line.Back.SetSector(original.Sector);
					}
				}
			}

			// Return the new sector
			return original.Sector;
		}

		// This takes default settings if not taken yet
		private static void TakeSidedefDefaults(ref SidedefSettings settings)
		{
			// Use defaults where no settings could be found
			if(settings.TileIndex == -1) settings.TileIndex = General.Map.Config.DefaultWallTile;
			if(settings.Shade == -1) settings.Shade = General.Map.Config.DefaultShade;
		}

		// This takes sidedef settings if not taken yet
		private static void TakeSidedefSettings(ref SidedefSettings settings, Sidedef side)
		{
			if(settings.TileIndex == -1) settings.TileIndex = side.TileIndex;
			if(settings.Shade == -1) settings.Shade = side.Shade;
		}
		
		// This applies defaults to a sidedef
		private static void ApplyDefaultsToSidedef(Sidedef sd, SidedefSettings defaults)
		{
			if(sd.Other == null || sd.HighRequired() || sd.LowRequired()) sd.TileIndex = defaults.TileIndex;
			if(defaults.Shade != -1) sd.Shade = defaults.Shade;
		}

		// This applies defaults to a sector
		private static void ApplyDefaultsToSector(Sector s)
		{
			s.FloorTileIndex = General.Map.Config.DefaultFloorTile;
			s.CeilingTileIndex = General.Map.Config.DefaultCeilingTile;
			s.FloorHeight = General.Map.Config.DefaultFloorHeight;
			s.CeilingHeight = General.Map.Config.DefaultCeilingHeight;
			s.CeilingShade = General.Map.Config.DefaultShade;
			s.FloorShade = General.Map.Config.DefaultShade;
			s.Visibility = General.Map.Config.DefaultVisibility;
		}
		
		#endregion
		
		#region ================== Sector Labels
		
		// This finds the ideal label positions for a sector
		/*public static List<LabelPositionInfo> FindLabelPositions(Sector s)
		{
			List<LabelPositionInfo> positions = new List<LabelPositionInfo>(2);
			int islandoffset = 0;
			
			// Do we have a triangulation?
			Triangulation triangles = s.Triangles;
			if(triangles != null)
			{
				// Go for all islands
				for(int i = 0; i < triangles.IslandVertices.Count; i++)
				{
					Dictionary<Sidedef, Linedef> sides = new Dictionary<Sidedef, Linedef>(triangles.IslandVertices[i] >> 1);
					List<Vector2D> candidatepositions = new List<Vector2D>(triangles.IslandVertices[i] >> 1);
					float founddistance = float.MinValue;
					Vector2D foundposition = new Vector2D();
					float minx = float.MaxValue;
					float miny = float.MaxValue;
					float maxx = float.MinValue;
					float maxy = float.MinValue;
					
					// Make candidate lines that are not along sidedefs
					// We do this before testing the candidate against the sidedefs so that
					// we can collect the relevant sidedefs first in the same run
					for(int t = 0; t < triangles.IslandVertices[i]; t += 3)
					{
						int triangleoffset = islandoffset + t;
						Vector2D v1 = triangles.Vertices[triangleoffset + 2];
						Sidedef sd = triangles.Sidedefs[triangleoffset + 2];
						for(int v = 0; v < 3; v++)
						{
							Vector2D v2 = triangles.Vertices[triangleoffset + v];
							
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
							sd = triangles.Sidedefs[triangleoffset + v];
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
						positions.Add(new LabelPositionInfo(foundposition, (float)Math.Sqrt(founddistance)));
					}
					else
					{
						// No candidate lines found.
						
						// Check to see if the island is a triangle
						if(triangles.IslandVertices[i] == 3)
						{
							// Use the center of the triangle
							// TODO: Use the 'incenter' instead, see http://mathworld.wolfram.com/Incenter.html
							Vector2D v = (triangles.Vertices[islandoffset] + triangles.Vertices[islandoffset + 1] + triangles.Vertices[islandoffset + 2]) / 3.0f;
							float d = Line2D.GetDistanceToLineSq(triangles.Vertices[islandoffset], triangles.Vertices[islandoffset + 1], v, false);
							d = Math.Min(d, Line2D.GetDistanceToLineSq(triangles.Vertices[islandoffset + 1], triangles.Vertices[islandoffset + 2], v, false));
							d = Math.Min(d, Line2D.GetDistanceToLineSq(triangles.Vertices[islandoffset + 2], triangles.Vertices[islandoffset], v, false));
							positions.Add(new LabelPositionInfo(v, (float)Math.Sqrt(d)));
						}
						else
						{
							// Use the center of this island.
							float d = Math.Min((maxx - minx) * 0.5f, (maxy - miny) * 0.5f);
							positions.Add(new LabelPositionInfo(new Vector2D(minx + (maxx - minx) * 0.5f, miny + (maxy - miny) * 0.5f), d));
						}
					}
					
					// Done with this island
					islandoffset += triangles.IslandVertices[i];
				}
			}
			else
			{
				// No triangulation was made. FAIL!
				General.Fail("No triangulation exists for sector " + s + " Triangulation is required to create label positions for a sector.");
			}
			
			// Done
			return positions;
		}*/
		
		#endregion

		#region ================== Drawing
		
		/// <summary>
		/// This draws lines with the given points. Note that this tool removes any existing geometry
		/// marks and marks the new lines and vertices when done. Also marks the sectors that were added.
		/// Returns false when the drawing failed.
		/// </summary>
		public static bool DrawLines(IList<DrawnVertex> points)
		{
			List<Vertex> newverts = new List<Vertex>();
			List<Vertex> intersectverts = new List<Vertex>();
			List<Linedef> newlines = new List<Linedef>();
			List<Linedef> oldlines = new List<Linedef>(General.Map.Map.Linedefs);
			List<Sidedef> insidesides = new List<Sidedef>();
			List<Vertex> mergeverts = new List<Vertex>();
			List<Vertex> nonmergeverts = new List<Vertex>(General.Map.Map.Vertices);
			MapSet map = General.Map.Map;

			General.Map.Map.ClearAllMarks(false);
			
			// Any points to do?
			if(points.Count > 0)
			{
				/***************************************************\
					Create the drawing
				\***************************************************/

				// Make first vertex
				Vertex v1 = map.CreateVertex(points[0].pos);
				if(v1 == null) return false;
				v1.Marked = true;

				// Keep references
				newverts.Add(v1);
				if(points[0].stitch) mergeverts.Add(v1); else nonmergeverts.Add(v1);

				// Go for all other points
				for(int i = 1; i < points.Count; i++)
				{
					// Create vertex for point
					Vertex v2 = map.CreateVertex(points[i].pos);
					if(v2 == null) return false;
					v2.Marked = true;

					// Keep references
					newverts.Add(v2);
					if(points[i].stitch) mergeverts.Add(v2); else nonmergeverts.Add(v2);

					// Create line between point and previous
					Linedef ld = map.CreateLinedef(v1, v2);
					if(ld == null) return false;
					ld.Marked = true;
					//ld.ApplySidedFlags();
					ld.UpdateCache();
					newlines.Add(ld);

					// Should we split this line to merge with intersecting lines?
					if(points[i - 1].stitchline && points[i].stitchline)
					{
						// Check if any other lines intersect this line
						List<float> intersections = new List<float>();
						Line2D measureline = ld.Line;
						foreach(Linedef ld2 in map.Linedefs)
						{
							// Intersecting?
							// We only keep the unit length from the start of the line and
							// do the real splitting later, when all intersections are known
							float u;
							if(ld2.Line.GetIntersection(measureline, out u))
							{
								if(!float.IsNaN(u) && (u > 0.0f) && (u < 1.0f) && (ld2 != ld))
									intersections.Add(u);
							}
						}

						// Sort the intersections
						intersections.Sort();

						// Go for all found intersections
						Linedef splitline = ld;
						foreach(float u in intersections)
						{
							// Calculate exact coordinates where to split
							// We use measureline for this, because the original line
							// may already have changed in length due to a previous split
							Vector2D splitpoint = measureline.GetCoordinatesAt(u);

							// Make the vertex
							Vertex splitvertex = map.CreateVertex(splitpoint);
							if(splitvertex == null) return false;
							splitvertex.Marked = true;
							newverts.Add(splitvertex);
							mergeverts.Add(splitvertex);			// <-- add to merge?
							intersectverts.Add(splitvertex);
							
							// The Split method ties the end of the original line to the given
							// vertex and starts a new line at the given vertex, so continue
							// splitting with the new line, because the intersections are sorted
							// from low to high (beginning at the original line start)
							splitline = splitline.Split(splitvertex);
							if(splitline == null) return false;
							//splitline.ApplySidedFlags();
							newlines.Add(splitline);
						}
					}

					// Next
					v1 = v2;
				}

				// Join merge vertices so that overlapping vertices in the draw become one.
				map.BeginAddRemove();
				MapSet.JoinVertices(mergeverts, mergeverts, false, MapSet.STITCH_DISTANCE);
				map.EndAddRemove();
				
				/***************************************************\
					Find a way to close the drawing
				\***************************************************/

				// We prefer a closed polygon, because then we can determine the interior properly
				// Check if the two ends of the polygon are closed
				bool splittingonly = false;
				if(newlines.Count > 0)
				{
					Linedef firstline = newlines[0];
					Linedef lastline = newlines[newlines.Count - 1];
					bool drawingclosed = (firstline.Start == lastline.End);
					if(!drawingclosed)
					{
						// When not closed, we will try to find a path to close it.
						// But first we check if any of our new lines are inside existing sectors, because
						// if they are then we are splitting sectors and cannot accurately find a closed path
						// to close our polygon. In that case, we want to do sector splits only.
						foreach(Linedef ld in newlines)
						{
							Vector2D ldcp = ld.GetCenterPoint();
							Linedef nld = MapSet.NearestLinedef(oldlines, ldcp);
							if(nld != null)
							{
								float ldside = nld.SideOfLine(ldcp);
								if(ldside < 0.0f)
								{
									if(nld.Front != null)
									{
										splittingonly = true;
										break;
									}
								}
								else if(ldside > 0.0f)
								{
									if(nld.Back != null)
									{
										splittingonly = true;
										break;
									}
								}
							}
						}

						// Not splitting only?
						if(!splittingonly)
						{
							// First and last vertex stitch with geometry?
							if(points[0].stitch && points[points.Count - 1].stitch)
							{
								List<LinedefSide> startpoints = new List<LinedefSide>();
								List<LinedefSide> endpoints = new List<LinedefSide>();

								// Find out where the start will stitch and create test points
								Linedef l1 = MapSet.NearestLinedefRange(oldlines, firstline.Start.Position, MapSet.STITCH_DISTANCE);
								Vertex vv1 = null;
								if(l1 != null)
								{
									startpoints.Add(new LinedefSide(l1, true));
									startpoints.Add(new LinedefSide(l1, false));
								}
								else
								{
									// Not stitched with a linedef, so check if it will stitch with a vertex
									vv1 = MapSet.NearestVertexSquareRange(nonmergeverts, firstline.Start.Position, MapSet.STITCH_DISTANCE);
									if((vv1 != null) && (vv1.Linedefs.Count > 0))
									{
										// Now we take the two linedefs with adjacent angles to the drawn line
										List<Linedef> lines = new List<Linedef>(vv1.Linedefs);
										lines.Sort(new LinedefAngleSorter(firstline, true, firstline.Start));
										startpoints.Add(new LinedefSide(lines[0], true));
										startpoints.Add(new LinedefSide(lines[0], false));
										lines.Sort(new LinedefAngleSorter(firstline, false, firstline.Start));
										startpoints.Add(new LinedefSide(lines[0], true));
										startpoints.Add(new LinedefSide(lines[0], false));
									}
								}

								// Find out where the end will stitch and create test points
								Linedef l2 = MapSet.NearestLinedefRange(oldlines, lastline.End.Position, MapSet.STITCH_DISTANCE);
								Vertex vv2 = null;
								if(l2 != null)
								{
									endpoints.Add(new LinedefSide(l2, true));
									endpoints.Add(new LinedefSide(l2, false));
								}
								else
								{
									// Not stitched with a linedef, so check if it will stitch with a vertex
									vv2 = MapSet.NearestVertexSquareRange(nonmergeverts, lastline.End.Position, MapSet.STITCH_DISTANCE);
									if((vv2 != null) && (vv2.Linedefs.Count > 0))
									{
										// Now we take the two linedefs with adjacent angles to the drawn line
										List<Linedef> lines = new List<Linedef>(vv2.Linedefs);
										lines.Sort(new LinedefAngleSorter(firstline, true, lastline.End));
										endpoints.Add(new LinedefSide(lines[0], true));
										endpoints.Add(new LinedefSide(lines[0], false));
										lines.Sort(new LinedefAngleSorter(firstline, false, lastline.End));
										endpoints.Add(new LinedefSide(lines[0], true));
										endpoints.Add(new LinedefSide(lines[0], false));
									}
								}

								// Found any start and end points?
								if((startpoints.Count > 0) && (endpoints.Count > 0))
								{
									List<LinedefSide> shortestpath = null;

									// Both stitched to the same line?
									if((l1 == l2) && (l1 != null))
									{
										// Then just connect the two
										shortestpath = new List<LinedefSide> { new LinedefSide(l1, true) };
									}
									// One stitched to a line and the other to a vertex of that line?
									else if((l1 != null) && (vv2 != null) && ((l1.Start == vv2) || (l1.End == vv2)))
									{
										// Then just connect the two
										shortestpath = new List<LinedefSide> { new LinedefSide(l1, true) };
									}
									// The other stitched to a line and the first to a vertex of that line?
									else if((l2 != null) && (vv1 != null) && ((l2.Start == vv1) || (l2.End == vv1)))
									{
										// Then just connect the two
										shortestpath = new List<LinedefSide> { new LinedefSide(l2, true) };
									}
									else
									{
										// Find the shortest, closest path between start and end points
										foreach(LinedefSide startp in startpoints)
										{
											foreach(LinedefSide endp in endpoints)
											{
												List<LinedefSide> p;
												p = FindClosestPath(startp.Line, startp.Front, endp.Line, endp.Front, true);
												if((p != null) && ((shortestpath == null) || (p.Count < shortestpath.Count))) shortestpath = p;
												p = FindClosestPath(endp.Line, endp.Front, startp.Line, startp.Front, true);
												if((p != null) && ((shortestpath == null) || (p.Count < shortestpath.Count))) shortestpath = p;
											}
										}
									}

									// Found a path?
									if(shortestpath != null)
									{
										// Check which direction the path goes in
										bool pathforward = false;
										foreach(LinedefSide startp in startpoints)
										{
											if(shortestpath[0].Line == startp.Line)
											{
												pathforward = true;
												break;
											}
										}

										// TEST
										/*
										General.Map.Renderer2D.StartOverlay(true);
										foreach(LinedefSide lsd in shortestpath)
										{
											General.Map.Renderer2D.RenderLine(lsd.Line.Start.Position, lsd.Line.End.Position, 2, new PixelColor(255, 0, 255, 0), true);
										}
										General.Map.Renderer2D.Finish();
										General.Map.Renderer2D.Present();
										Thread.Sleep(1000);
										*/
										
										// Begin at first vertex in path
										v1 = (pathforward ? firstline.Start : lastline.End);

										// Go for all vertices in the path to make additional lines
										for(int i = 1; i < shortestpath.Count; i++)
										{
											// Get the next position
											Vector2D v2pos = (shortestpath[i].Front ? shortestpath[i].Line.Start.Position : shortestpath[i].Line.End.Position);

											// Make the new vertex
											Vertex v2 = map.CreateVertex(v2pos);
											if(v2 == null) return false;
											v2.Marked = true;
											mergeverts.Add(v2);

											// Make the line
											Linedef ld = map.CreateLinedef(v1, v2);
											if(ld == null) return false;
											ld.Marked = true;
											//ld.ApplySidedFlags();
											ld.UpdateCache();
											newlines.Add(ld);

											// Next
											v1 = v2;
										}

										// Make the final line
										Linedef lld = map.CreateLinedef(v1, (pathforward ? lastline.End : firstline.Start));
										if(lld == null) return false;
										
										// Setup line
										lld.Marked = true;
										//lld.ApplySidedFlags();
										lld.UpdateCache();
										newlines.Add(lld);

										// Join merge vertices so that overlapping vertices in the draw become one.
										MapSet.JoinVertices(mergeverts, mergeverts, false, MapSet.STITCH_DISTANCE);
									}
								}
							}
						}
					}
				}

				// Merge intersetion vertices with the new lines. This completes the
				// self intersections for which splits were made above.
				map.Update(true, false);
				map.BeginAddRemove();
				MapSet.SplitLinesByVertices(newlines, intersectverts, MapSet.STITCH_DISTANCE, null);
				MapSet.SplitLinesByVertices(newlines, mergeverts, MapSet.STITCH_DISTANCE, null);
				map.EndAddRemove();
				
				/***************************************************\
					Determine drawing interior
				\***************************************************/

				// In step 3 we will make sectors on the interior sides and join sectors on the
				// exterior sides, but because the user could have drawn counterclockwise or just
				// some weird polygon. The following code figures out the interior side of all
				// new lines.
				map.Update(true, false);
				foreach(Linedef ld in newlines)
				{
					// Find closest path starting with the front of this linedef
					List<LinedefSide> pathlines = FindClosestPath(ld, true, true);
					if(pathlines != null)
					{
						// Make polygon
						LinedefTracePath tracepath = new LinedefTracePath(pathlines);
						EarClipPolygon pathpoly = tracepath.MakePolygon(true);
						
						// Check if the front of the line is outside the polygon
						if((pathpoly.CalculateArea() > 0.001f) && !pathpoly.Intersect(ld.GetSidePoint(true)))
						{
							// Now trace from the back side of the line to see if
							// the back side lies in the interior. I don't want to
							// flip the line if it is not helping.

							// Find closest path starting with the back of this linedef
							pathlines = FindClosestPath(ld, false, true);
							if(pathlines != null)
							{
								// Make polygon
								tracepath = new LinedefTracePath(pathlines);
								pathpoly = tracepath.MakePolygon(true);

								// Check if the front of the line is inside the polygon
								ld.FrontInterior = (pathpoly.CalculateArea() < 0.001f) || pathpoly.Intersect(ld.GetSidePoint(true));
							}
							else
							{
								ld.FrontInterior = true;
							}
						}
						else
						{
							ld.FrontInterior = true;
						}
					}
					else
					{
						ld.FrontInterior = true;
					}
				}

				/***************************************************\
					Merge the new geometry
				\***************************************************/

				// Mark only the vertices that should be merged
				map.ClearMarkedVertices(false);
				foreach(Vertex v in mergeverts) v.Marked = true;

				// Before this point, the new geometry is not linked with the existing geometry.
				// Now perform standard geometry stitching to merge the new geometry with the rest
				// of the map. The marked vertices indicate the new geometry.
				map.StitchGeometry();
				map.Update(true, false);

				// Find our new lines again, because they have been merged with the other geometry
				// but their Marked property is copied where they have joined.
				newlines = map.GetMarkedLinedefs(true);
				
				// Remove any disposed old lines
				List<Linedef> prevoldlines = oldlines;
				oldlines = new List<Linedef>(prevoldlines.Count);
				foreach(Linedef ld in prevoldlines)
					if(!ld.IsDisposed) oldlines.Add(ld);
				
				/***************************************************\
					Join and create new sectors
				\***************************************************/

				// The code below atempts to create sectors on the interior sides of the drawn
				// geometry and joins sectors on the other sides of the drawn geometry.
				// This code does not change any geometry, it only makes/updates sidedefs.
				bool sidescreated = false;
				bool[] frontsdone = new bool[newlines.Count];
				bool[] backsdone = new bool[newlines.Count];
				for(int i = 0; i < newlines.Count; i++)
				{
					Linedef ld = newlines[i];

					// Interior not done yet?
					if((ld.FrontInterior && !frontsdone[i]) || (!ld.FrontInterior && !backsdone[i]))
					{
						// Find a way to create a sector here
						List<LinedefSide> sectorlines = FindPotentialSectorAt(ld, ld.FrontInterior);
						if(sectorlines != null)
						{
							sidescreated = true;

							// When none of the linedef sides exist yet, this is a true new
							// sector that will be created out of the void!
							bool istruenewsector = true;
							foreach(LinedefSide ls in sectorlines)
							{
								if((ls.Front && (ls.Line.Front != null)) ||
								   (!ls.Front && (ls.Line.Back != null)))
								{
									istruenewsector = false;
									break;
								}
							}

							// But we don't want to create sectors out of the void when we
							// decided that we only want to split sectors.
							if(!istruenewsector || !splittingonly)
							{
								// Make the new sector
								Sector newsector = MakeSector(sectorlines, oldlines);
								if(newsector == null) return false;

								if(istruenewsector) newsector.Marked = true;

								// Go for all sidedefs in this new sector
								foreach(Sidedef sd in newsector.Sidedefs)
								{
									// Keep list of sides inside created sectors
									insidesides.Add(sd);

									// Side matches with a side of our new lines?
									int lineindex = newlines.IndexOf(sd.Line);
									if(lineindex > -1)
									{
										// Mark this side as done
										if(sd.IsFront)
											frontsdone[lineindex] = true;
										else
											backsdone[lineindex] = true;
									}
								}
							}
						}
					}

					// Exterior not done yet?
					if((ld.FrontInterior && !backsdone[i]) || (!ld.FrontInterior && !frontsdone[i]))
					{
						// Find a way to create a sector here
						List<LinedefSide> sectorlines = FindPotentialSectorAt(ld, !ld.FrontInterior);
						if(sectorlines != null)
						{
							// Check if any of the surrounding lines originally have sidedefs we can join
							Sidedef joinsidedef = null;
							foreach(LinedefSide ls in sectorlines)
							{
								if(ls.Front && (ls.Line.Front != null))
								{
									joinsidedef = ls.Line.Front;
									break;
								}

								if(!ls.Front && (ls.Line.Back != null))
								{
									joinsidedef = ls.Line.Back;
									break;
								}
							}

							// Join?
							if(joinsidedef != null)
							{
								sidescreated = true;

								// We only want to modify our new lines when joining a sector
								// (or it may break nearby self-referencing sectors)
								List<LinedefSide> newsectorlines = new List<LinedefSide>(sectorlines.Count);
								foreach(LinedefSide sd in sectorlines)
								{
									// Side matches with a side of our new lines?
									int lineindex = newlines.IndexOf(sd.Line);
									if(lineindex > -1)
									{
										// Add to list
										newsectorlines.Add(sd);
										
										// Mark this side as done
										if(sd.Front)
											frontsdone[lineindex] = true;
										else
											backsdone[lineindex] = true;
									}
								}
								
								// Have our new lines join the existing sector
								if(JoinSector(newsectorlines, joinsidedef) == null)
									return false;
							}
						}
					}
				}

				/***************************************************\
					Corrections and clean up
				\***************************************************/

				// Make corrections for backward linedefs
				MapSet.FlipBackwardLinedefs(newlines);

				// Check if any of our new lines have sides
				if(sidescreated)
				{
					// Then remove the lines which have no sides at all
					for(int i = newlines.Count - 1; i >= 0; i--)
					{
						// Remove the line if it has no sides
						if((newlines[i].Front == null) && (newlines[i].Back == null)) newlines[i].Dispose();
					}

					//mxd. Split multipart sectors...
					SplitMultipartSectors(newlines);
				}

				// Mark new geometry only
				General.Map.Map.ClearMarkedLinedefs(false);
				General.Map.Map.ClearMarkedVertices(false);
				foreach(Vertex v in newverts) v.Marked = true;
				foreach(Linedef l in newlines) l.Marked = true;
			}

			return true;
		}

		//mxd
		private static void SplitMultipartSectors(IEnumerable<Linedef> drawnlines)
		{
			Dictionary<Sector, HashSet<Sidedef>> sectorsidesref = new Dictionary<Sector, HashSet<Sidedef>>();
			HashSet<Sidedef> drawnsides = new HashSet<Sidedef>();

			// Create drawn lines per sector collection
			foreach(Linedef l in drawnlines)
			{
				if(l.Front != null && (l.Front.Sector != null && !SectorWasInvalid(l.Front.Sector)))
				{
					// Add only multipart sectors
					if(l.Front.Sector.Triangles.IslandVertices.Count > 1)
					{
						if(!sectorsidesref.ContainsKey(l.Front.Sector))
							sectorsidesref[l.Front.Sector] = new HashSet<Sidedef>();
						sectorsidesref[l.Front.Sector].Add(l.Front);
					}
					drawnsides.Add(l.Front);
				}

				if(l.Back != null && (l.Back.Sector != null && !SectorWasInvalid(l.Back.Sector)))
				{
					// Add only multipart sectors
					if(l.Back.Sector.Triangles.IslandVertices.Count > 1)
					{
						if(!sectorsidesref.ContainsKey(l.Back.Sector))
							sectorsidesref[l.Back.Sector] = new HashSet<Sidedef>();
						sectorsidesref[l.Back.Sector].Add(l.Back);
					}
					drawnsides.Add(l.Back);
				}
			}

			// Split sectors
			foreach(KeyValuePair<Sector, HashSet<Sidedef>> group in sectorsidesref)
			{
				// Sector has all sides selected?
				if(group.Key.Sidedefs.Count == group.Value.Count)
				{
					group.Key.Marked = true; // Sometimes those are not marked...
					continue;
				}

				// Process all sides
				foreach(Sidedef side in group.Value)
				{
					// Sector was already split?
					if(side.Sector != group.Key) continue;

					// Find drawing interior
					List<LinedefSide> linedefsides = FindPotentialSectorAt(side.Line, side.IsFront);

					// Number of potential sides fewer than the sector has?
					if(linedefsides != null && linedefsides.Count > 0 && linedefsides.Count < group.Key.Sidedefs.Count)
					{
						// Collect sidedefs from new sector shape...
						HashSet<Sidedef> newsectorsides = new HashSet<Sidedef>();
						foreach(LinedefSide ls in linedefsides)
						{
							Sidedef s = (ls.Front ? ls.Line.Front : ls.Line.Back);
							if(s != null) newsectorsides.Add(s);
						}

						// Make new sector only if one of the remaining sector sides was also drawn...
						foreach(Sidedef s in group.Key.Sidedefs)
						{
							if(newsectorsides.Contains(s)) continue;
							if(drawnsides.Contains(s))
							{
								Sector newsector = MakeSector(linedefsides, null);
								if(newsector != null)
								{
									newsector.UpdateCache();
									group.Key.UpdateCache();
								}

								// Existing sector may've become invalid
								SectorWasInvalid(group.Key);

								break;
							}
						}
					}
				}
			}
		}

		//mxd
		private static bool SectorWasInvalid(Sector s)
		{
			s.UpdateCache();
			
			if(s.Sidedefs == null || s.Sidedefs.Count < 3 || s.FloorVertices.Length < 3)
			{
				// Collect changed lines
				HashSet<Linedef> changedlines = new HashSet<Linedef>();
				if(s.Sidedefs != null)
				{
					foreach(Sidedef side in s.Sidedefs) changedlines.Add(side.Line);
				}

				// Delete sector
				s.Dispose();

				// Correct lines
				foreach(Linedef l in changedlines)
				{
					if(l.Front == null)
					{
						l.FlipVertices();
						l.FlipSidedefs();
					}
				}

				return true;
			}

			return false;
		}
		
		#endregion

		#region ================== Sector Images Floodfill

		// This performs flat floodfill over sector floors or ceilings that match with the same flat
		// NOTE: This method uses the sectors marking to indicate which sides have been filled
		// When resetsectormarks is set to true, all sectors will first be marked false (not aligned).
		// Setting resetsectormarks to false is usefull to fill only within a specific selection
		// (set the marked property to true for the sectors outside the selection)
		public static void FloodfillSectorImages(Sector start, bool fillceilings, int originalflat, int fillflat, bool resetsectormarks)
		{
			Stack<Sector> todo = new Stack<Sector>(50);

			// Mark all sectors false (they will be marked true when the flat is modified)
			if(resetsectormarks) General.Map.Map.ClearMarkedSectors(false);
			
			// Begin with first sector
			if(((start.FloorTileIndex == originalflat) && !fillceilings) ||
			   ((start.CeilingTileIndex == originalflat) && fillceilings))
			{
				todo.Push(start);
			}

			// Continue until nothing more to align
			while(todo.Count > 0)
			{
				// Get the sector to do
				Sector s = todo.Pop();
				
				// Apply new flat
				if(fillceilings) s.CeilingTileIndex = fillflat;
				else s.FloorTileIndex = fillflat;
				s.Marked = true;
				
				// Go for all sidedefs to add neighbouring sectors
				foreach(Sidedef sd in s.Sidedefs)
				{
					// Sector on the other side of the line that we haven't checked yet?
					if((sd.Other != null) && !sd.Other.Sector.Marked)
					{
						Sector os = sd.Other.Sector;
						
						// Check if texture matches
						if(((os.FloorTileIndex == originalflat) && !fillceilings) ||
						   ((os.CeilingTileIndex == originalflat) && fillceilings))
						{
							todo.Push(os);
						}
					}
				}
			}
		}

		#endregion

		#region ================== Wall Images Floodfill

		// This performs texture floodfill along all walls that match with the same texture
		// NOTE: This method uses the sidedefs marking to indicate which sides have been filled
		// When resetsidemarks is set to true, all sidedefs will first be marked false (not aligned).
		// Setting resetsidemarks to false is usefull to fill only within a specific selection
		// (set the marked property to true for the sidedefs outside the selection)
		public static void FloodfillWallImages(Sidedef start, int originaltexture, int filltexture, bool resetsidemarks)
		{
			Stack<SidedefFillJob> todo = new Stack<SidedefFillJob>(50);

			// Mark all sidedefs false (they will be marked true when the texture is aligned)
			if(resetsidemarks) General.Map.Map.ClearMarkedSidedefs(false);
			
			// Begin with first sidedef
			if(SidedefTextureMatch(start, originaltexture))
				todo.Push(new SidedefFillJob { sidedef = start, forward = true });
			
			// Continue until nothing more to align
			while(todo.Count > 0)
			{
				// Get the align job to do
				SidedefFillJob j = todo.Pop();

				// Apply texturing
				if(j.sidedef.TileIndex == originaltexture) j.sidedef.TileIndex = filltexture;
				if(j.sidedef.MaskedTileIndex == originaltexture) j.sidedef.MaskedTileIndex = filltexture;
				j.sidedef.Marked = true;
				
				if(j.forward)
				{
					// Add sidedefs forward (connected to the right vertex)
					Vertex v = j.sidedef.IsFront ? j.sidedef.Line.End : j.sidedef.Line.Start;
					AddSidedefsForFloodfill(todo, v, true, originaltexture);

					// Add sidedefs backward (connected to the left vertex)
					v = j.sidedef.IsFront ? j.sidedef.Line.Start : j.sidedef.Line.End;
					AddSidedefsForFloodfill(todo, v, false, originaltexture);
				}
				else
				{
					// Add sidedefs backward (connected to the left vertex)
					Vertex v = j.sidedef.IsFront ? j.sidedef.Line.Start : j.sidedef.Line.End;
					AddSidedefsForFloodfill(todo, v, false, originaltexture);

					// Add sidedefs forward (connected to the right vertex)
					v = j.sidedef.IsFront ? j.sidedef.Line.End : j.sidedef.Line.Start;
					AddSidedefsForFloodfill(todo, v, true, originaltexture);
				}
			}
		}

		// This adds the matching, unmarked sidedefs from a vertex for texture alignment
		private static void AddSidedefsForFloodfill(Stack<SidedefFillJob> stack, Vertex v, bool forward, int tileindex)
		{
			foreach(Linedef ld in v.Linedefs)
			{
				Sidedef side1 = (forward ? ld.Front : ld.Back);
				Sidedef side2 = (forward ? ld.Back : ld.Front);
				if(ld.Start == v && side1 != null && !side1.Marked)
				{
					if(SidedefTextureMatch(side1, tileindex))
					{
						stack.Push(new SidedefFillJob { forward = forward, sidedef = side1 });
					}
				}
				else if(ld.End == v && side2 != null && !side2.Marked)
				{
					if(SidedefTextureMatch(side2, tileindex))
					{
						stack.Push(new SidedefFillJob { forward = forward, sidedef = side2 });
					}
				}
			}
		}

		#endregion

		#region ================== Texture Alignment

		// This performs texture alignment along all walls that match with the same texture
		// NOTE: This method uses the sidedefs marking to indicate which sides have been aligned
		// When resetsidemarks is set to true, all sidedefs will first be marked false (not aligned).
		// Setting resetsidemarks to false is usefull to align only within a specific selection
		// (set the marked property to true for the sidedefs outside the selection)
		public static void AutoAlignImages(Sidedef start, ImageData texture, bool alignx, bool aligny, bool resetsidemarks)
		{
			throw new NotImplementedException("Not done yet...");
			
			/*Stack<SidedefAlignJob> todo = new Stack<SidedefAlignJob>(50);
			float scalex = (General.Map.Config.ScaledTextureOffsets && !texture.WorldPanning) ? texture.Scale.x : 1.0f;
			float scaley = (General.Map.Config.ScaledTextureOffsets && !texture.WorldPanning) ? texture.Scale.y : 1.0f;
			
			// Mark all sidedefs false (they will be marked true when the texture is aligned)
			if(resetsidemarks) General.Map.Map.ClearMarkedSidedefs(false);
			
			// Begin with first sidedef
			SidedefAlignJob first = new SidedefAlignJob();
			first.sidedef = start;
			first.offsetx = start.OffsetX;

			first.forward = true;
			todo.Push(first);
			
			// Continue until nothing more to align
			while(todo.Count > 0)
			{
				// Get the align job to do
				SidedefAlignJob j = todo.Pop();
				
				if(j.forward)
				{
					// Apply alignment
					if(alignx) j.sidedef.OffsetX = j.offsetx;
					if(aligny) j.sidedef.OffsetY = (int)Math.Round((start.Sector.CeilHeight - j.sidedef.Sector.CeilHeight) / scaley) + start.OffsetY;
					int forwardoffset = j.offsetx + (int)Math.Round(j.sidedef.Line.Length / scalex);
					int backwardoffset = j.offsetx;
					
					j.sidedef.Marked = true;
					
					// Wrap the value within the width of the texture (to prevent ridiculous values)
					// NOTE: We don't use ScaledWidth here because the texture offset is in pixels, not mappixels
					if(texture.IsImageLoaded)
					{
						if(alignx) j.sidedef.OffsetX %= texture.Width;
						if(aligny) j.sidedef.OffsetY %= texture.Height;
					}
					
					// Add sidedefs forward (connected to the right vertex)
					Vertex v = j.sidedef.IsFront ? j.sidedef.Line.End : j.sidedef.Line.Start;
					AddSidedefsForAlignment(todo, v, true, forwardoffset, texture.LongName);

					// Add sidedefs backward (connected to the left vertex)
					v = j.sidedef.IsFront ? j.sidedef.Line.Start : j.sidedef.Line.End;
					AddSidedefsForAlignment(todo, v, false, backwardoffset, texture.LongName);
				}
				else
				{
					// Apply alignment
					if(alignx) j.sidedef.OffsetX = j.offsetx - (int)Math.Round(j.sidedef.Line.Length / scalex);
					if(aligny) j.sidedef.OffsetY = (int)Math.Round((start.Sector.CeilHeight - j.sidedef.Sector.CeilHeight) / scaley) + start.OffsetY;
					int forwardoffset = j.offsetx;
					int backwardoffset = j.offsetx - (int)Math.Round(j.sidedef.Line.Length / scalex);
					
					j.sidedef.Marked = true;

					// Wrap the value within the width of the texture (to prevent ridiculous values)
					// NOTE: We don't use ScaledWidth here because the texture offset is in pixels, not mappixels
					if(texture.IsImageLoaded)
					{
						if(alignx) j.sidedef.OffsetX %= texture.Width;
						if(aligny) j.sidedef.OffsetY %= texture.Height;
					}

					// Add sidedefs backward (connected to the left vertex)
					Vertex v = j.sidedef.IsFront ? j.sidedef.Line.Start : j.sidedef.Line.End;
					AddSidedefsForAlignment(todo, v, false, backwardoffset, texture.LongName);

					// Add sidedefs forward (connected to the right vertex)
					v = j.sidedef.IsFront ? j.sidedef.Line.End : j.sidedef.Line.Start;
					AddSidedefsForAlignment(todo, v, true, forwardoffset, texture.LongName);
				}
			}*/
		}

		// This adds the matching, unmarked sidedefs from a vertex for texture alignment
		private static void AddSidedefsForAlignment(Stack<SidedefAlignJob> stack, Vertex v, bool forward, int offsetx, int tileindex)
		{
			foreach(Linedef ld in v.Linedefs)
			{
				Sidedef side1 = forward ? ld.Front : ld.Back;
				Sidedef side2 = forward ? ld.Back : ld.Front;
				if((ld.Start == v) && (side1 != null) && !side1.Marked)
				{
					if(SidedefTextureMatch(side1, tileindex))
					{
						stack.Push(new SidedefAlignJob { forward = forward, offsetx = offsetx, sidedef = side1 });
					}
				}
				else if((ld.End == v) && (side2 != null) && !side2.Marked)
				{
					if(SidedefTextureMatch(side2, tileindex))
					{
						stack.Push(new SidedefAlignJob { forward = forward, offsetx = offsetx, sidedef = side2 });
					}
				}
			}
		}
		
		// This checks if any of the sidedef texture match the given texture
		private static bool SidedefTextureMatch(Sidedef sd, int tileindex)
		{
			return (sd.TileIndex == tileindex || (sd.Other != null && sd.MaskedTileIndex == tileindex));

			//return ((sd.LongHighTexture == texturelongname) && sd.HighRequired()) ||
			// ((sd.LongLowTexture == texturelongname) && sd.LowRequired()) ||
			//((sd.LongMiddleTexture == texturelongname) && (sd.MiddleRequired() || ((sd.MiddleTexture.Length > 0) && (sd.MiddleTexture[0] != '-')))) ;
		}
		
		#endregion
		
		#region ================== Tags and Actions
		
		/// <summary>
		/// This removes all tags on the marked geometry.
		/// </summary>
		public static void RemoveMarkedTags()
		{
			General.Map.Map.ForAllTags<object>(RemoveTagHandler, true, null);
		}
		
		// This removes tags
		private static void RemoveTagHandler(MapElement element, bool actionargument, PropertyType type, ref int value, object obj)
		{
			value = 0;
		}
		
		/// <summary>
		/// This renumbers all tags on the marked geometry.
		/// </summary>
		public static void RenumberMarkedTags()
		{
			Dictionary<int, int> tagsmap = new Dictionary<int, int>();
			
			// Collect the tag numbers used in the marked geometry
			General.Map.Map.ForAllTags(CollectTagNumbersHandler, true, tagsmap);
			
			// Get new tags that are unique within unmarked geometry
			List<int> newtags = General.Map.Map.GetMultipleNewTags(tagsmap.Count, false);
			
			// Map the old tags with the new tags
			int index = 0;
			List<int> oldkeys = new List<int>(tagsmap.Keys);
			foreach(int ot in oldkeys) tagsmap[ot] = newtags[index++];
			
			// Now renumber the old tags with the new ones
			General.Map.Map.ForAllTags(RenumberTagsHandler, true, tagsmap);
		}
		
		// This collects tags in a dictionary
		private static void CollectTagNumbersHandler(MapElement element, bool actionargument, PropertyType type, ref int value, Dictionary<int, int> tagsmap)
		{
			if(value != 0) tagsmap[value] = value;
		}

		// This remaps tags from a dictionary
		private static void RenumberTagsHandler(MapElement element, bool actionargument, PropertyType type, ref int value, Dictionary<int, int> tagsmap)
		{
			if(value != 0) value = tagsmap[value];
		}
		
		/// <summary>
		/// This removes all actions on the marked geometry.
		/// </summary>
		/*public static void RemoveMarkedActions()
		{
			// Remove actions from things
			foreach(Thing t in General.Map.Map.Things)
			{
				if(t.Marked)
				{
					t.Action = 0;
					for(int i = 0; i < Thing.NUM_ARGS; i++) t.Args[i] = 0;
				}
			}
			
			// Remove actions from linedefs
			foreach(Linedef l in General.Map.Map.Linedefs)
			{
				if(l.Marked)
				{
					l.Action = 0;
					for(int i = 0; i < Linedef.NUM_ARGS; i++) l.Args[i] = 0;
				}
			}
		}*/
		
		#endregion
		
		#region ================== Misc Exported Functions

		/// <summary>
		/// This performs a Hermite spline interpolation and returns the result position.
		/// Where u (0 - 1) is the wanted position on the curve between p1 (using tangent t1) and p2 (using tangent t2).
		/// </summary>
		public static Vector2D HermiteSpline(Vector2D p1, Vector2D t1, Vector2D p2, Vector2D t2, float u)
		{
			return D3DDevice.V2D(Vector2.Hermite(D3DDevice.V2(p1), D3DDevice.V2(t1), D3DDevice.V2(p2), D3DDevice.V2(t2), u));
		}

		/// <summary>
		/// This performs a Hermite spline interpolation and returns the result position.
		/// Where u (0 - 1) is the wanted position on the curve between p1 (using tangent t1) and p2 (using tangent t2).
		/// </summary>
		public static Vector3D HermiteSpline(Vector3D p1, Vector3D t1, Vector3D p2, Vector3D t2, float u)
		{
			return D3DDevice.V3D(Vector3.Hermite(D3DDevice.V3(p1), D3DDevice.V3(t1), D3DDevice.V3(p2), D3DDevice.V3(t2), u));
		}

		#endregion
	}
}
