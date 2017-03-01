
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
	internal interface IVisualEventReceiver
	{
		// The events that must be handled
		void OnSelectBegin();
		void OnSelectEnd();
		void OnEditBegin();
		void OnEditEnd();
		void OnMouseMove(MouseEventArgs e);
		void OnChangeTargetHeight(int amount);
		void OnChangeTargetShade(bool up);
		void OnChangeTargetAngle(float amount); //mxd
		void OnChangeImageOffset(int horizontal, int vertical);
		void OnResetImageOffsets();
		void OnSelectImage();
		void OnCopyImage();
		void OnPasteImage();
		void OnCopyImageOffsets();
		void OnPasteImageOffsets();
		void OnCopyProperties();
		void OnPasteProperties();
		void OnImageAlign(bool alignx, bool aligny);
		void OnImageFloodfill();
		//void OnToggleUpperUnpegged();
		void OnToggleBottomAlignment();
		void OnProcess(double deltatime);
		void OnInsert();
		void OnDelete();

		// Assist functions
		void ApplyImage(int tileindex);
		//void ApplyUpperUnpegged(bool set);
		void ApplyBottomAlignment(bool set);
		
		// Other methods
		int GetImageIndex();
	}
}
