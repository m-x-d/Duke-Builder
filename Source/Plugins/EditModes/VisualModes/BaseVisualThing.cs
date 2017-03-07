
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
using System.Windows.Forms;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;
using mxd.DukeBuilder.Geometry;
using mxd.DukeBuilder.VisualModes;
using mxd.DukeBuilder.Data;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	internal class BaseVisualThing : VisualThing, IVisualEventReceiver
	{
		private const float MINIMUM_RADIUS = 32f;
		private const float MINIMUM_HEIGHT = 64f;
		
		#region ================== Variables

		protected BaseVisualMode mode;
		
		//private SpriteInfo info;
		private bool isloaded;
		private ImageData sprite;
		private float cageradius2;
		private Vector2D pos2d;
		private Vector3D boxp1;
		private Vector3D boxp2;
		
		// Undo/redo
		private int undoticket;

		// If this is set to true, the thing will be rebuilt after the action is performed.
		protected bool changed;

		#endregion
		
		#region ================== Properties

		public bool Changed { get { return changed; } set { changed |= value; } }
		
		#endregion
		
		#region ================== Constructor / Setup
		
		// Constructor
		public BaseVisualThing(BaseVisualMode mode, Thing t) : base(t)
		{
			this.mode = mode;

			// Find thing information
			//info = General.Map.Data.GetSpriteInfo(Thing.TileIndex);

			// Find sprite texture
			sprite = General.Map.Data.GetImageData(Thing.TileIndex);
			if(sprite != null) sprite.AddReference();

			// We have no destructor
			GC.SuppressFinalize(this);
		}
		
		// This builds the thing geometry. Returns false when nothing was created.
		public virtual bool Setup()
		{
			// Must have a width and height!
			float radius, height;

			// Find the sector in which the thing resides
			Thing.DetermineSector(mode.BlockMap);

			if(sprite != null)
			{
				//mxd. Transparency and brightness
				int brightness;
				if(Thing.SemiTransparent)
				{
					brightness = MapElement.CalculateBrightness(Thing.Shade, (byte)(Thing.Transparent ? 85 : 170));
					this.RenderPass = RenderPass.Alpha;
				}
				else
				{
					brightness = MapElement.CalculateBrightness(Thing.Shade);
					this.RenderPass = RenderPass.Mask;
				}

				// Check if the texture is loaded
				sprite.LoadImage();
				isloaded = sprite.IsImageLoaded;

				if(isloaded)
				{
					base.Texture = sprite;

					// Determine sprite size and offsets
					bool flooraligned = Thing.FlatAligned;
					bool wallaligned = Thing.WallAligned;
					bool truecentered = Thing.TrueCentered;

					float xratio = Thing.RepeatX * ((flooraligned && truecentered) ? 0.2f : 0.25f);
					float yratio = Thing.RepeatY * 0.25f;

					int xsize = (int)(sprite.Width * xratio);
					int ysize = (int)(sprite.Height * yratio);

					int tilexoff = Thing.OffsetX + sprite.OffsetX;
					int tileyoff = Thing.OffsetY + sprite.OffsetY;

					int xoff = (int)(tilexoff * xratio);
					int yoff = (int)(tileyoff * yratio);

					if(truecentered && !flooraligned) yoff -= ysize / 2; // Seems that centeryoff shenanigans in polymer_updatesprite.cpp do exactly the same thing...

					bool xflip = Thing.FlipX;
					bool yflip = Thing.FlipY;

					// Initially set flipu and flipv.
					bool flipu = (xflip ^ flooraligned);
					bool flipv = (yflip && !flooraligned);

					if(flipu) xoff = -xoff;
					if(yflip && (flooraligned || wallaligned)) yoff = -yoff;

					radius = xsize * 0.5f;
					height = ysize;

					// Make vertices
					WorldVertex[] verts = new WorldVertex[6];
					verts[0] = new WorldVertex(-radius + xoff, 0.0f, yoff, brightness, 0.0f, 1.0f);
					verts[1] = new WorldVertex(-radius + xoff, 0.0f, height + yoff, brightness, 0.0f, 0.0f);
					verts[2] = new WorldVertex(radius + xoff, 0.0f, height + yoff, brightness, 1.0f, 0.0f);
					verts[3] = verts[0];
					verts[4] = verts[2];
					verts[5] = new WorldVertex(radius + xoff, 0.0f, yoff, brightness, 1.0f, 1.0f);

					// Update UVs if needed
					if(flipu || flipv)
					{
						for(int i = 0; i < 6; i++)
						{
							if(flipu) verts[i].u = (verts[i].u - 1.0f) * -1.0f;
							if(flipv) verts[i].v = (verts[i].v - 1.0f) * -1.0f;
						}
					}

					SetVertices(verts);
				}
				else
				{
					base.Texture = General.Map.Data.Hourglass3D;

					// Determine sprite size
					radius = Math.Min(Texture.Width / 2f, Texture.Height / 2f);
					height = Math.Min(Texture.Width, Texture.Height);

					// Make vertices
					WorldVertex[] verts = new WorldVertex[6];
					verts[0] = new WorldVertex(-radius, 0.0f, 0.0f, brightness, 0.0f, 1.0f);
					verts[1] = new WorldVertex(-radius, 0.0f, height, brightness, 0.0f, 0.0f);
					verts[2] = new WorldVertex(+radius, 0.0f, height, brightness, 1.0f, 0.0f);
					verts[3] = verts[0];
					verts[4] = verts[2];
					verts[5] = new WorldVertex(+radius, 0.0f, 0.0f, brightness, 1.0f, 1.0f);
					SetVertices(verts);
				}
			}
			else
			{
				// Use some default values...
				radius = MINIMUM_RADIUS;
				height = MINIMUM_HEIGHT;

				SetVertices(new WorldVertex[0]);
			}
			
			// Determine position
			Vector3D pos = Thing.Position;
			
			if(Thing.Sector != null) pos.z = Thing.Sector.FloorPlane.GetZ(Thing.Position.x, Thing.Position.y);
			if(Thing.Position.z > 0) pos.z += Thing.Position.z;
				
			// Check if above ceiling
			if((Thing.Sector != null) && ((pos.z + height) > Thing.Sector.CeilingHeight))
			{
				// Put thing against ceiling
				pos.z = Thing.Sector.CeilingHeight - height;
			}
			
			// Apply settings
			SetPosition(pos);
			SetCageSize(Math.Max(radius, MINIMUM_RADIUS), Math.Max(height, MINIMUM_HEIGHT));
			SetCageColor(Thing.Color);

			// Keep info for object picking
			cageradius2 = radius * Angle2D.SQRT2;
			cageradius2 = cageradius2 * cageradius2;
			pos2d = pos;
			boxp1 = new Vector3D(pos.x - radius, pos.y - radius, pos.z);
			boxp2 = new Vector3D(pos.x + radius, pos.y + radius, pos.z + height);
			
			// Done
			changed = false;
			return true;
		}
		
		// Disposing
		public override void Dispose()
		{
			if(!IsDisposed)
			{
				if(sprite != null)
				{
					sprite.RemoveReference();
					sprite = null;
				}
			}
			
			base.Dispose();
		}
		
		#endregion
		
		#region ================== Methods

		// This forces to rebuild the whole sprite
		public void Rebuild()
		{
			// Find thing information
			//info = General.Map.Data.GetSpriteInfo(Thing.TileIndex);

			// Find sprite image
			sprite = General.Map.Data.GetImageData(Thing.TileIndex);
			if(sprite != null) sprite.AddReference();
			
			// Setup sprite
			Setup();
		}

		// This updates the sprite when needed
		public override void Update()
		{
			// Rebuild sprite geometry when sprite is loaded
			if(!isloaded && sprite.IsImageLoaded) Setup();
			
			// Let the base update
			base.Update();
		}

		// This performs a fast test in object picking
		public override bool PickFastReject(Vector3D from, Vector3D to, Vector3D dir)
		{
			float distance2 = Line2D.GetDistanceToLineSq(from, to, pos2d, false);
			return (distance2 <= cageradius2);
		}

		// This performs an accurate test for object picking
		public override bool PickAccurate(Vector3D from, Vector3D to, Vector3D dir, ref float u_ray)
		{
			Vector3D delta = to - from;
			float tfar = float.MaxValue;
			float tnear = float.MinValue;
			
			// Ray-Box intersection code
			// See http://www.masm32.com/board/index.php?topic=9941.0
			
			// Check X slab
			if(delta.x == 0.0f)
			{
				if(from.x > boxp2.x || from.x < boxp1.x)
				{
					// Ray is parallel to the planes & outside slab
					return false;
				}
			}
			else
			{
				float tmp = 1.0f / delta.x;
				float t1 = (boxp1.x - from.x) * tmp;
				float t2 = (boxp2.x - from.x) * tmp;
				if(t1 > t2) General.Swap(ref t1, ref t2);
				if(t1 > tnear) tnear = t1;
				if(t2 < tfar) tfar = t2;
				if(tnear > tfar || tfar < 0.0f)
				{
					// Ray missed box or box is behind ray
					return false;
				}
			}
			
			// Check Y slab
			if(delta.y == 0.0f)
			{
				if(from.y > boxp2.y || from.y < boxp1.y)
				{
					// Ray is parallel to the planes & outside slab
					return false;
				}
			}
			else
			{
				float tmp = 1.0f / delta.y;
				float t1 = (boxp1.y - from.y) * tmp;
				float t2 = (boxp2.y - from.y) * tmp;
				if(t1 > t2) General.Swap(ref t1, ref t2);
				if(t1 > tnear) tnear = t1;
				if(t2 < tfar) tfar = t2;
				if(tnear > tfar || tfar < 0.0f)
				{
					// Ray missed box or box is behind ray
					return false;
				}
			}
			
			// Check Z slab
			if(delta.z == 0.0f)
			{
				if(from.z > boxp2.z || from.z < boxp1.z)
				{
					// Ray is parallel to the planes & outside slab
					return false;
				}
			}
			else
			{
				float tmp = 1.0f / delta.z;
				float t1 = (boxp1.z - from.z) * tmp;
				float t2 = (boxp2.z - from.z) * tmp;
				if(t1 > t2) General.Swap(ref t1, ref t2);
				if(t1 > tnear) tnear = t1;
				if(t2 < tfar) tfar = t2;
				if(tnear > tfar || tfar < 0.0f)
				{
					// Ray missed box or box is behind ray
					return false;
				}
			}
			
			// Set interpolation point
			u_ray = (tnear > 0.0f) ? tnear : tfar;
			return true;
		}
		
		#endregion

		#region ================== Events

		// Unused
		public virtual void OnSelectBegin() { }
		public virtual void OnEditBegin() { }
		public virtual void OnMouseMove(MouseEventArgs e) { }
		public virtual void OnSetFirstWall() { } //mxd
		public virtual void OnChangeImageOffset(int horizontal, int vertical) { }
		public virtual void OnSelectImage() { }
		public virtual void OnCopyImage() { }
		public virtual void OnPasteImage() { }
		public virtual void OnCopyImageOffsets() { }
		public virtual void OnPasteImageOffsets() { }
		public virtual void OnImageAlign(bool alignx, bool aligny) { }
		public virtual void OnToggleBottomAlignment() { }
		public virtual void OnResetImageOffsets() { }
		public virtual void OnProcess(double deltatime) { }
		public virtual void OnImageFloodfill() { }
		public virtual void OnInsert() { }
		public virtual void OnDelete() { }
		public virtual void ApplyImage(int tileindex) { }
		public virtual void ApplyBottomAlignment(bool set) { }
		
		// Return image index
		public virtual int GetImageIndex() { return Thing.TileIndex; }

		// Select or deselect
		public virtual void OnSelectEnd()
		{
			if(this.selected)
			{
				this.selected = false;
				mode.RemoveSelectedObject(this);
			}
			else
			{
				this.selected = true;
				mode.AddSelectedObject(this);
			}
		}
		
		// Copy properties
		public virtual void OnCopyProperties()
		{
			BuilderPlug.Me.CopiedSpriteProps = new BuildSprite(Thing);
			mode.SetActionResult("Copied sprite properties.");
		}
		
		// Paste properties
		public virtual void OnPasteProperties()
		{
			if(BuilderPlug.Me.CopiedSpriteProps != null)
			{
				mode.CreateUndo("Paste sprite properties");
				mode.SetActionResult("Pasted sprite properties.");
				BuilderPlug.Me.CopiedSpriteProps.ApplyTo(Thing);
				Thing.UpdateConfiguration();
				this.Rebuild();
				mode.ShowTargetInfo();
			}
		}
		
		// Edit button released
		public virtual void OnEditEnd()
		{
			if(General.Interface.IsActiveWindow)
			{
				List<Thing> things = mode.GetSelectedSprites();
				DialogResult result = General.Interface.ShowEditSprites(things);
				if(result == DialogResult.OK)
				{
					foreach(Thing t in things)
					{
						VisualThing vt = mode.GetVisualSprite(t);
						if(vt != null) (vt as BaseVisualThing).Changed = true;
					}
				}
			}
		}
		
		// Raise/lower thing
		public virtual void OnChangeTargetHeight(int amount)
		{
			if((General.Map.UndoRedo.NextUndo == null) || (General.Map.UndoRedo.NextUndo.TicketID != undoticket))
				undoticket = mode.CreateUndo("Change sprite height");

			Thing.Move(Thing.Position + new Vector3D(0.0f, 0.0f, amount));
			mode.SetActionResult("Changed sprite height to " + Thing.Position.z + ".");
			this.Changed = true;
		}

		//mxd. Change shade
		public virtual void OnChangeTargetShade(bool up)
		{
			if((General.Map.UndoRedo.NextUndo == null) || (General.Map.UndoRedo.NextUndo.TicketID != undoticket))
				undoticket = mode.CreateUndo("Change sprite shade");

			Thing.Shade = General.Clamp(Thing.Shade + (up ? 1 : -1), General.Map.FormatInterface.MinShade, General.Map.FormatInterface.MaxShade);
			mode.SetActionResult("Changed sprite shade to " + Thing.Shade + ".");
			this.Changed = true;
		}

		//mxd. Rotate sprite
		public virtual void OnChangeTargetAngle(float amount)
		{
			if((General.Map.UndoRedo.NextUndo == null) || (General.Map.UndoRedo.NextUndo.TicketID != undoticket))
				undoticket = mode.CreateUndo("Change sprite angle");

			this.Thing.Angle += amount;
			mode.SetActionResult("Changed sprite angle to " + Thing.AngleDeg + ".");
			this.Changed = true;
		}
		
		#endregion
	}
}
