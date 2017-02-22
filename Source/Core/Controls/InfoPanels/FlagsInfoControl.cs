#region ================== Namespaces

using System.Collections.Generic;
using System.Windows.Forms;

#endregion

namespace mxd.DukeBuilder.Controls
{
	public partial class FlagsInfoControl : UserControl
	{
		#region ================== Properties

		public string Title { get { return groupbox.Text; } set { groupbox.Text = value; } }

		#endregion

		#region ================== Constructor / Setup

		public FlagsInfoControl()
		{
			InitializeComponent();
		}

		public void Setup(IEnumerable<string> flags)
		{
			this.SuspendLayout();
			
			list.Items.Clear();
			
			// Fill the list
			foreach(string flag in flags)
			{
				var item = list.Items.Add(flag);
				item.Checked = true;
			}

			// Resize control
			if(list.Items.Count > 0)
			{
				int startx = list.Items[list.Items.Count - 1].Bounds.Left;
				int maxx = 0;
				for(int i = list.Items.Count - 1; i > -1; i--)
				{
					if(list.Items[i].Bounds.Left < startx) break;
					if(list.Items[i].Bounds.Right > maxx) maxx = list.Items[i].Bounds.Right;
				}

				if(maxx > 0)
				{
					groupbox.Width = list.Left + groupbox.Padding.Right + maxx;
					this.Width = groupbox.Right + groupbox.Margin.Right;
					panel.Width = maxx - panel.Left;
					list.Width = panel.Width;
				}
			}

			// Show control if needed
			this.Visible = list.Items.Count > 0;

			this.ResumeLayout();
		}

		#endregion
	}
}
