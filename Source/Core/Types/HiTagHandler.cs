#region ================== Namespaces

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.HiTag, "HiTag")]
	internal class HiTagHandler : IntegerHandler
	{
		#region ================== Properties

		public override bool IsBrowseable { get { return true; } }
		public override Image BrowseImage { get { return Properties.Resources.NewTag; } }

		public override int MinValue { get { return General.Map.FormatInterface.MinTag; } }
		public override int MaxValue { get { return General.Map.FormatInterface.MaxTag; } }

		#endregion

		#region ================== Methods

		public override void Browse(IWin32Window parent)
		{
			//TODO: find unused hitag
			throw new NotImplementedException();
			
			//value = AngleForm.ShowDialog(parent, value);
		}

		#endregion
	}
}
