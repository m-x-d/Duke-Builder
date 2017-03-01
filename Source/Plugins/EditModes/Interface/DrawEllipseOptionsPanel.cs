using System;
using System.Windows.Forms;

namespace mxd.DukeBuilder.EditModes
{
	internal partial class DrawEllipseOptionsPanel : UserControl
	{
		public event EventHandler OnValueChanged;

		private bool blockevents;

		public int Subdivisions { get { return (int)subdivs.Value; } set { blockevents = true; subdivs.Value = value; blockevents = false; } }
		public int Angle { get { return (int)angle.Value; } set { blockevents = true; angle.Value = value; blockevents = false; } }
		public int MaxSubdivisions { get { return (int)subdivs.Maximum; } set { subdivs.Maximum = value; } }
		public int MinSubdivisions { get { return (int)subdivs.Minimum;  } set { subdivs.Minimum = value; } }
		
		public DrawEllipseOptionsPanel() 
		{
			InitializeComponent();
		}

		public void Register() 
		{
			subdivs.ValueChanged += ValueChanged;
			angle.ValueChanged += ValueChanged;

			//General.Interface.BeginToolbarUpdate();
			General.Interface.AddButton(subdivslabel);
			General.Interface.AddButton(subdivs);
			General.Interface.AddButton(anglelabel);
			General.Interface.AddButton(angle);
			General.Interface.AddButton(reset);
			//General.Interface.EndToolbarUpdate();
		}

		public void Unregister() 
		{
			//General.Interface.BeginToolbarUpdate();
			General.Interface.RemoveButton(reset);
			General.Interface.RemoveButton(angle);
			General.Interface.RemoveButton(anglelabel);
			General.Interface.RemoveButton(subdivs);
			General.Interface.RemoveButton(subdivslabel);
			//General.Interface.EndToolbarUpdate();
		}

		private void ValueChanged(object sender, EventArgs e) 
		{
			if(!blockevents && OnValueChanged != null) OnValueChanged(this, EventArgs.Empty);
		}

		private void reset_Click(object sender, EventArgs e) 
		{
			// Reset values
			blockevents = true;
			angle.Value = 0;
			subdivs.Value = 6;
			blockevents = false;

			// Dispatch event
			OnValueChanged(this, EventArgs.Empty);
		}
	}
}
