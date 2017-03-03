
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

using System.Windows.Forms;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	// This doesn't do jack shit.
	internal class NullVisualEventReceiver : IVisualEventReceiver
	{
		public void OnSelectBegin() { }
		public void OnSelectEnd() { }
		public void OnEditBegin() { }
		public void OnEditEnd() { }
		public void OnMouseMove(MouseEventArgs e) { }
		public void OnChangeTargetHeight(int amount) { }
		public void OnSetFirstWall() { } //mxd
		public void OnChangeTargetShade(bool up) { }
		public void OnChangeTargetAngle(float amount) { } //mxd
		public void OnChangeImageOffset(int horizontal, int vertical) { }
		public void OnResetImageOffsets() { }
		public void OnSelectImage() { }
		public void OnCopyImage() { }
		public void OnPasteImage() { }
		public void OnCopyImageOffsets() { }
		public void OnPasteImageOffsets() { }
		public void OnCopyProperties() { }
		public void OnPasteProperties() { }
		public void OnImageAlign(bool alignx, bool aligny) { }
		public void OnImageFloodfill() { }
		public void OnToggleBottomAlignment() { }
		public void OnProcess(double deltatime) { }
		public void OnInsert() { }
		public void OnDelete() { }
		public void ApplyImage(int tileindex) { }
		public void ApplyBottomAlignment(bool set) { }
		public int GetImageIndex() { return 0; } //mxd
	}
}
