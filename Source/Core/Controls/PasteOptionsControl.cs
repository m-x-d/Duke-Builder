
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
using mxd.DukeBuilder.Config;

#endregion

namespace mxd.DukeBuilder.Controls
{
	public partial class PasteOptionsControl : UserControl
	{
		#region ================== Variables

		#endregion
		
		#region ================== Properties

		#endregion
		
		#region ================== Constructor
		
		// Constructor
		public PasteOptionsControl()
		{
			InitializeComponent();
		}
		
		#endregion
		
		#region ================== Methods
		
		// This sets the options from the given PasteOptions
		public void Setup(PasteOptions options)
		{
			// Setup controls
			keeptags.Checked = (options.ChangeTags == 0);
			renumbertags.Checked = (options.ChangeTags == 1);
			removetags.Checked = (options.ChangeTags == 2);
			adjustheights.Checked = options.AdjustHeights;
		}
		
		// This returns the options as set by the user
		public PasteOptions GetOptions()
		{
			PasteOptions options = new PasteOptions();
			
			// Collect settings
			if(keeptags.Checked)
				options.ChangeTags = 0;
			else if(renumbertags.Checked)
				options.ChangeTags = 1;
			else if(removetags.Checked)
				options.ChangeTags = 2;
			options.AdjustHeights = adjustheights.Checked;
			
			return options;
		}
		
		#endregion
	}
}
