
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

#endregion

namespace mxd.DukeBuilder.Map
{
	public sealed class Linedef : SelectableElement
	{
		#region ================== Constants

		public const float SIDE_POINT_DISTANCE = 0.01f;
		
		#endregion

		#region ================== Variables

		// Map
		private MapSet map;

		// List items
		private LinkedListNode<Linedef> startvertexlistitem;
		private LinkedListNode<Linedef> endvertexlistitem;
		private LinkedListNode<Linedef> selecteditem;
		
		// Vertices
		private Vertex start;
		private Vertex end;
		
		// Sidedefs
		private Sidedef front;
		private Sidedef back;

		// Cache
		private bool updateneeded;
		private float lengthsq;
		private float lengthsqinv;
		private float length;
		private float lengthinv;
		private float angle;
		private RectangleF bbox;

		private bool isblocking;
		private bool isdoublesided;
		private bool isinvalid;
		
		// Properties
		private bool frontinterior;		// for drawing only

		// Clone
		private int serializedindex;
		
		#endregion

		#region ================== Properties

		public MapSet Map { get { return map; } }
		public Vertex Start { get { return start; } }
		public Vertex End { get { return end; } }
		public Sidedef Front { get { return front; } }
		public Sidedef Back { get { return back; } }
		public Line2D Line { get { return new Line2D(start.Position, end.Position); } }
		public float LengthSq { get { return lengthsq; } }
		public float Length { get { return length; } }
		public float LengthInv { get { return lengthinv; } }
		public float Angle { get { return angle; } }
		public int AngleDeg { get { return (int)(angle * Angle2D.PIDEG); } }
		public RectangleF BBox { get { return bbox; } }
		internal int SerializedIndex { get { return serializedindex; } set { serializedindex = value; } }
		internal bool FrontInterior { get { return frontinterior; } set { frontinterior = value; } }

		//mxd. Cached flags
		public bool IsDoubleSided { get { return isdoublesided; } }
		public bool IsBlocking { get { return isblocking; } }
		public bool IsInvalid { get { return isinvalid; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		internal Linedef(MapSet map, int listindex, Vertex start, Vertex end)
		{
			// Initialize
			this.map = map;
			this.listindex = listindex;
			this.updateneeded = true;
			
			// Attach to vertices
			this.start = start;
			this.startvertexlistitem = start.AttachLinedefP(this);
			this.end = end;
			this.endvertexlistitem = end.AttachLinedefP(this);
			
			if(map == General.Map.Map) General.Map.UndoRedo.RecAddLinedef(this);
		}
		
		// Disposer
		public override void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Already set isdisposed so that changes can be prohibited
				isdisposed = true;

				// Dispose sidedefs
				if((front != null) && map.AutoRemove) front.Dispose(); else AttachFrontP(null);
				if((back != null) && map.AutoRemove) back.Dispose(); else AttachBackP(null);
				
				if(map == General.Map.Map) General.Map.UndoRedo.RecRemLinedef(this);

				// Remove from main list
				map.RemoveLinedef(listindex);
				
				// Detach from vertices
				if(startvertexlistitem != null) start.DetachLinedefP(startvertexlistitem);
				startvertexlistitem = null;
				start = null;
				if(endvertexlistitem != null) end.DetachLinedefP(endvertexlistitem);
				endvertexlistitem = null;
				end = null;
				
				// Clean up
				start = null;
				end = null;
				front = null;
				back = null;
				map = null;

				// Clean up base
				base.Dispose();
			}
		}

		#endregion

		#region ================== Management
		
		// Call this before changing properties
		protected override void BeforePropsChange()
		{
			if(map == General.Map.Map) General.Map.UndoRedo.RecPrpLinedef(this);
		}
		
		// Serialize / deserialize (passive: doesn't record)
		internal void ReadWrite(IReadWriteStream s)
		{
			if(!s.IsWriting)
			{
				BeforePropsChange();
				updateneeded = true;
			}
		}

		// This sets new start vertex
		public void SetStartVertex(Vertex v)
		{
			if(map == General.Map.Map)
				General.Map.UndoRedo.RecRefLinedefStart(this);
			
			// Change start
			if(startvertexlistitem != null) start.DetachLinedefP(startvertexlistitem);
			startvertexlistitem = null;
			start = v;
			if(start != null) startvertexlistitem = start.AttachLinedefP(this);
			this.updateneeded = true;
		}

		// This sets new end vertex
		public void SetEndVertex(Vertex v)
		{
			if(map == General.Map.Map)
				General.Map.UndoRedo.RecRefLinedefEnd(this);

			// Change end
			if(endvertexlistitem != null) end.DetachLinedefP(endvertexlistitem);
			endvertexlistitem = null;
			end = v;
			if(end != null) endvertexlistitem = end.AttachLinedefP(this);
			this.updateneeded = true;
		}

		// This detaches a vertex
		internal void DetachVertexP(Vertex v)
		{
			if(v == start)
			{
				if(startvertexlistitem != null) start.DetachLinedefP(startvertexlistitem);
				startvertexlistitem = null;
				start = null;
			}
			else if(v == end)
			{
				if(endvertexlistitem != null) end.DetachLinedefP(endvertexlistitem);
				endvertexlistitem = null;
				end = null;
			}
			else 
				throw new Exception("Specified Vertex is not attached to this Line.");
		}
		
		// This copies all properties to another line
		public void CopyPropertiesTo(Linedef l)
		{
			l.BeforePropsChange();
			l.updateneeded = true;
			base.CopyPropertiesTo(l);
		}
		
		// This attaches a sidedef on the front
		internal void AttachFront(Sidedef s)
		{
			if(map == General.Map.Map) General.Map.UndoRedo.RecRefLinedefFront(this);
			
			// Attach and recalculate
			AttachFrontP(s);
		}
		
		// Passive version, does not record the change
		internal void AttachFrontP(Sidedef s)
		{
			// Attach and recalculate
			front = s;
			if(front != null) front.SetLinedefP(this);
			updateneeded = true;
		}

		// This attaches a sidedef on the back
		internal void AttachBack(Sidedef s)
		{
			if(map == General.Map.Map) General.Map.UndoRedo.RecRefLinedefBack(this);

			// Attach and recalculate
			AttachBackP(s);
		}
		
		// Passive version, does not record the change
		internal void AttachBackP(Sidedef s)
		{
			// Attach and recalculate
			back = s;
			if(back != null) back.SetLinedefP(this);
			updateneeded = true;
		}

		// This detaches a sidedef from the front
		internal void DetachSidedefP(Sidedef s)
		{
			// Sidedef is on the front?
			if(front == s)
			{
				// Remove sidedef reference
				if(front != null) front.SetLinedefP(null);
				front = null;
				updateneeded = true;
			}
			// Sidedef is on the back?
			else if(back == s)
			{
				// Remove sidedef reference
				if(back != null) back.SetLinedefP(null);
				back = null;
				updateneeded = true;
			}
		}
		
		// This updates the line when changes have been made
		public void UpdateCache()
		{
			// Update if needed
			if(updateneeded)
			{
				// Delta vector
				Vector2D delta = end.Position - start.Position;

				// Recalculate values
				lengthsq = delta.GetLengthSq();
				length = (float)Math.Sqrt(lengthsq);
				if(length > 0f) lengthinv = 1f / length; else lengthinv = 1f / 0.0000000001f;
				if(lengthsq > 0f) lengthsqinv = 1f / lengthsq; else lengthsqinv = 1f / 0.0000000001f;
				angle = delta.GetAngle();
				float l = Math.Min(start.Position.x, end.Position.x);
				float t = Math.Min(start.Position.y, end.Position.y);
				float r = Math.Max(start.Position.x, end.Position.x);
				float b = Math.Max(start.Position.y, end.Position.y);
				bbox = new RectangleF(l, t, r - l, b - t);

				// Cached flags
				isdoublesided = (front != null && back != null);
				isinvalid = ((front == null && back == null) || (isdoublesided && front.Sector == back.Sector));
				isblocking = (!isdoublesided || isinvalid || front.BlockHitscan || front.BlockMove || back.BlockHitscan || back.BlockMove);

				//mxd. Update wall repeats
				if(front != null) front.UpdateRepeats();
				if(back != null) back.UpdateRepeats();

				// Updated
				updateneeded = false;
			}
		}

		// This flags the line needs an update because it moved
		public void NeedUpdate()
		{
			// Update this line
			updateneeded = true;

			// Update sectors as well
			if(front != null) front.Sector.UpdateNeeded = true;
			if(back != null) back.Sector.UpdateNeeded = true;
		}

		// Selected
		protected override void DoSelect()
		{
			base.DoSelect();
			selecteditem = map.SelectedLinedefs.AddLast(this);
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
		
		// This flips the linedef's vertex attachments
		public void FlipVertices()
		{
			// make sure the start/end vertices are not automatically
			// deleted if they do not belong to any other line
			General.Map.Map.AutoRemove = false;

			// Flip vertices
			Vertex oldstart = start;
			Vertex oldend = end;
			SetStartVertex(oldend);
			SetEndVertex(oldstart);

			General.Map.Map.AutoRemove = true;
			
			// For drawing, the interior now lies on the other side
			frontinterior = !frontinterior;

			// Update required (angle changed)
			NeedUpdate();
			General.Map.IsChanged = true;
		}

		// This flips the sidedefs
		public void FlipSidedefs()
		{
			//mxd. FirstWalls may need updating
			if(front != null && front == front.Sector.FirstWall)
				front.Sector.FirstWall = (back ?? General.GetByIndex(front.Sector.Sidedefs, 0));
			if(back != null && back == back.Sector.FirstWall)
				back.Sector.FirstWall = (front ?? General.GetByIndex(back.Sector.Sidedefs, 0));
			
			// Flip sidedefs
			Sidedef oldfront = front;
			Sidedef oldback = back;
			AttachFront(oldback);
			AttachBack(oldfront);
			
			General.Map.IsChanged = true;
		}
		
		// This returns a point for testing on one side
		public Vector2D GetSidePoint(bool front)
		{
			Vector2D delta = end.Position - start.Position;
			Vector2D n = new Vector2D { x = delta.x * lengthinv * SIDE_POINT_DISTANCE, 
										y = delta.y * lengthinv * SIDE_POINT_DISTANCE};
			if(front) n *= -1;

			return new Vector2D { x = start.Position.x + delta.x * 0.5f - n.y, 
								  y = start.Position.y + delta.y * 0.5f + n.x };
		}

		// This returns a point in the middle of the line
		public Vector2D GetCenterPoint()
		{
			return start.Position + (end.Position - start.Position) * 0.5f;
		}

		// This returns all points at which the line intersects with the grid
		public List<Vector2D> GetGridIntersections()
		{
			List<Vector2D> coords = new List<Vector2D>();
			Vector2D v = new Vector2D();
			float minx, maxx, miny, maxy;
			bool reversex, reversey;
			
			if(start.Position.x > end.Position.x)
			{
				minx = end.Position.x;
				maxx = start.Position.x;
				reversex = true;
			}
			else
			{
				minx = start.Position.x;
				maxx = end.Position.x;
				reversex = false;
			}

			if(start.Position.y > end.Position.y)
			{
				miny = end.Position.y;
				maxy = start.Position.y;
				reversey = true;
			}
			else
			{
				miny = start.Position.y;
				maxy = end.Position.y;
				reversey = false;
			}

			// Go for all vertical grid lines in between line start and end
			float gx = General.Map.Grid.GetHigher(minx);
			if(gx < maxx)
			{
				for(; gx < maxx; gx += General.Map.Grid.GridSizeF)
				{
					// Add intersection point at this x coordinate
					float u = (gx - minx) / (maxx - minx);
					if(reversex) u = 1.0f - u;
					v.x = gx;
					v.y = start.Position.y + (end.Position.y - start.Position.y) * u;
					coords.Add(v);
				}
			}
			
			// Go for all horizontal grid lines in between line start and end
			float gy = General.Map.Grid.GetHigher(miny);
			if(gy < maxy)
			{
				for(; gy < maxy; gy += General.Map.Grid.GridSizeF)
				{
					// Add intersection point at this y coordinate
					float u = (gy - miny) / (maxy - miny);
					if(reversey) u = 1.0f - u;
					v.x = start.Position.x + (end.Position.x - start.Position.x) * u;
					v.y = gy;
					coords.Add(v);
				}
			}
			
			// Profit
			return coords;
		}
		
		// This returns the closest coordinates ON the line
		public Vector2D NearestOnLine(Vector2D pos)
		{
			float u = Line2D.GetNearestOnLine(start.Position, end.Position, pos);
			if(u < 0f) u = 0f; else if(u > 1f) u = 1f;
			return Line2D.GetCoordinatesAt(start.Position, end.Position, u);
		}

		// This returns the shortest distance from given coordinates to line
		public float SafeDistanceToSq(Vector2D p, bool bounded)
		{
			Vector2D v1 = start.Position;
			Vector2D v2 = end.Position;

			// Calculate intersection offset
			float u = ((p.x - v1.x) * (v2.x - v1.x) + (p.y - v1.y) * (v2.y - v1.y)) * lengthsqinv;

			// Limit intersection offset to the line
			if(bounded) if(u < lengthinv) u = lengthinv; else if(u > (1f - lengthinv)) u = 1f - lengthinv;

			// Calculate intersection point
			Vector2D i = v1 + u * (v2 - v1);

			// Return distance between intersection and point which is the shortest distance to the line
			float ldx = p.x - i.x;
			float ldy = p.y - i.y;
			return ldx * ldx + ldy * ldy;
		}

		// This returns the shortest distance from given coordinates to line
		public float SafeDistanceTo(Vector2D p, bool bounded)
		{
			return (float)Math.Sqrt(SafeDistanceToSq(p, bounded));
		}

		// This returns the shortest distance from given coordinates to line
		public float DistanceToSq(Vector2D p, bool bounded)
		{
			Vector2D v1 = start.Position;
			Vector2D v2 = end.Position;
			
			// Calculate intersection offset
			float u = ((p.x - v1.x) * (v2.x - v1.x) + (p.y - v1.y) * (v2.y - v1.y)) * lengthsqinv;

			// Limit intersection offset to the line
			if(bounded) if(u < 0f) u = 0f; else if(u > 1f) u = 1f;
			
			// Calculate intersection point
			Vector2D i = v1 + u * (v2 - v1);

			// Return distance between intersection and point which is the shortest distance to the line
			float ldx = p.x - i.x;
			float ldy = p.y - i.y;
			return ldx * ldx + ldy * ldy;
		}

		// This returns the shortest distance from given coordinates to line
		public float DistanceTo(Vector2D p, bool bounded)
		{
			return (float)Math.Sqrt(DistanceToSq(p, bounded));
		}

		// This tests on which side of the line the given coordinates are
		// returns < 0 for front (right) side, > 0 for back (left) side and 0 if on the line
		public float SideOfLine(Vector2D p)
		{
			Vector2D v1 = start.Position;
			Vector2D v2 = end.Position;
			
			// Calculate and return side information
			return (p.y - v1.y) * (v2.x - v1.x) - (p.x - v1.x) * (v2.y - v1.y);
		}

		// This splits this line by vertex v
		// Returns the new line resulting from the split, or null when it failed
		public Linedef Split(Vertex v)
		{
			// Copy linedef and change vertices
			Linedef nl = map.CreateLinedef(v, end);
			if(nl == null) return null;
			CopyPropertiesTo(nl);
			SetEndVertex(v);
			nl.Selected = this.Selected;
			nl.marked = this.marked;
			
			// Copy front wall if exists
			if(front != null)
			{
				Sidedef nsd = map.CreateSidedef(nl, true, front.Sector);
				if(nsd == null) return null;
				front.CopyPropertiesTo(nsd);
				nsd.Marked = front.Marked;

				// Make texture offset adjustments
				//TODO: works differently in Build...
				nsd.OffsetX += (int)Vector2D.Distance(this.start.Position, this.end.Position);
			}

			// Copy back wall if exists
			if(back != null)
			{
				Sidedef nsd = map.CreateSidedef(nl, false, back.Sector);
				if(nsd == null) return null;
				back.CopyPropertiesTo(nsd);
				nsd.Marked = back.Marked;
				
				// Make texture offset adjustments
				//TODO: works differently in Build...
				back.OffsetX += (int)Vector2D.Distance(nl.start.Position, nl.end.Position);
			}

			// Return result
			General.Map.IsChanged = true;
			return nl;
		}
		
		// This joins the line with another line
		// This line will be disposed
		// Returns false when the operation could not be completed
		public bool Join(Linedef other)
		{
			// Check which lines were 2 sided
			//bool otherwas2s = ((other.Front != null) && (other.Back != null));
			//bool thiswas2s = ((this.Front != null) && (this.Back != null));

			// Get sector references
			Sector otherfs = (other.front != null ? other.front.Sector : null);
			Sector otherbs = (other.back != null ? other.back.Sector : null);
			Sector thisfs = (this.front != null ? this.front.Sector : null);
			Sector thisbs = (this.back != null ? this.back.Sector : null);

			// This line has no sidedefs?
			if((thisfs == null) && (thisbs == null))
			{
				// We have no sidedefs, so we have no influence
				// Nothing to change on the other line
			}
			// Other line has no sidedefs?
			else if((otherfs == null) && (otherbs == null))
			{
				// The other has no sidedefs, so it has no influence
				// Copy my sidedefs to the other
				if(this.Start == other.Start)
				{
					if(!JoinChangeSidedefs(other, true, front)) return false;
					if(!JoinChangeSidedefs(other, false, back)) return false;
				}
				else
				{
					if(!JoinChangeSidedefs(other, false, front)) return false;
					if(!JoinChangeSidedefs(other, true, back)) return false;
				}

				// Copy my properties to the other
				this.CopyPropertiesTo(other);
			}
			else
			{
				// Compare front sectors
				if((otherfs != null) && (otherfs == thisfs))
				{
					// Copy textures
					if(other.front != null) other.front.AddTexturesTo(this.back);
					if(this.front != null) this.front.AddTexturesTo(other.back);

					// Change sidedefs?
					if(!JoinChangeSidedefs(other, true, back)) return false;
				}
				// Compare back sectors
				else if((otherbs != null) && (otherbs == thisbs))
				{
					// Copy textures
					if(other.back != null) other.back.AddTexturesTo(this.front);
					if(this.back != null) this.back.AddTexturesTo(other.front);

					// Change sidedefs?
					if(!JoinChangeSidedefs(other, false, front)) return false;
				}
				// Compare front and back
				else if((otherfs != null) && (otherfs == thisbs))
				{
					// Copy textures
					if(other.front != null) other.front.AddTexturesTo(this.front);
					if(this.back != null) this.back.AddTexturesTo(other.back);

					// Change sidedefs?
					if(!JoinChangeSidedefs(other, true, front)) return false;
				}
				// Compare back and front
				else if((otherbs != null) && (otherbs == thisfs))
				{
					// Copy textures
					if(other.back != null) other.back.AddTexturesTo(this.back);
					if(this.front != null) this.front.AddTexturesTo(other.front);

					// Change sidedefs?
					if(!JoinChangeSidedefs(other, false, back)) return false;
				}
				else
				{
					// Other line single sided?
					if(other.back == null)
					{
						// This line with its back to the other?
						if(this.start == other.end)
						{
							// Copy textures
							if(this.back != null) this.back.AddTexturesTo(other.front);

							// Change sidedefs?
							if(!JoinChangeSidedefs(other, false, front)) return false;
						}
						else
						{
							// Copy textures
							if(this.front != null) this.front.AddTexturesTo(other.front);

							// Change sidedefs?
							if(!JoinChangeSidedefs(other, false, back)) return false;
						}
					}
					// This line single sided?
					else if(this.back == null)
					{
						// Other line with its back to this?
						if(other.start == this.end)
						{
							if(otherbs == null)
							{
								// Copy textures
								if(other.back != null) other.back.AddTexturesTo(this.front);

								// Change sidedefs
								if(!JoinChangeSidedefs(other, false, front)) return false;
							}
						}
						else
						{
							if(otherfs == null)
							{
								// Copy textures
								if(other.front != null) other.front.AddTexturesTo(this.front);

								// Change sidedefs
								if(!JoinChangeSidedefs(other, true, front)) return false;
							}
						}
					}
					else
					{
						// This line with its back to the other?
						if(this.start == other.end)
						{
							// Copy textures
							if(other.back != null) other.back.AddTexturesTo(this.front);
							if(this.back != null) this.back.AddTexturesTo(other.front);

							// Change sidedefs
							if(!JoinChangeSidedefs(other, false, front)) return false;
						}
						// Both lines face the same way
						else
						{
							// Copy textures
							if(other.back != null) other.back.AddTexturesTo(this.back);
							if(this.front != null) this.front.AddTexturesTo(other.front);

							// Change sidedefs
							if(!JoinChangeSidedefs(other, false, back)) return false;
						}
					}
				}
			}
			
			// If either of the two lines was selected, keep the other selected
			if(this.Selected) other.Selected = true;
			if(this.marked) other.marked = true;
			
			// I got killed by the other.
			this.Dispose();
			General.Map.IsChanged = true;
			return true;
		}
		
		// This changes sidedefs (used for joining lines)
		// target:		The linedef on which to remove or create a new sidedef
		// front:		Side on which to remove or create the sidedef (true for front side)
		// newside:		The side from which to copy the properties to the new sidedef.
		//				If this is null, no sidedef will be created (only removed)
		// Returns false when the operation could not be completed.
		private bool JoinChangeSidedefs(Linedef target, bool front, Sidedef newside)
		{
			// Change sidedefs
			if(front)
			{
				if(target.front != null) target.front.Dispose();
			}
			else
			{
				if(target.back != null) target.back.Dispose();
			}
			
			if(newside != null)
			{
				Sidedef sd = map.CreateSidedef(target, front, newside.Sector);
				if(sd == null) return false;
				newside.CopyPropertiesTo(sd);
				sd.Marked = newside.Marked;
			}

			return true;
		}

		// String representation
		public override string ToString()
		{
			return "Line " + listindex;
		}
		
		#endregion

		#region ================== Changes
		
		// This updates all properties
		public void Update()
		{
			BeforePropsChange();
			this.updateneeded = true;
		}

		#endregion
	}
}
