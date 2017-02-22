
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

namespace mxd.DukeBuilder.Types
{
	internal class NullHandler : TypeHandler
	{
		#region ================== Variables

		private object value = 0;

		#endregion

		#region ================== Methods

		public override void SetValue(object value)
		{
			this.value = (value ?? 0);
		}

		public override int GetIntValue()
		{
			int result;
			return (int.TryParse(this.value.ToString(), out result) ? result : 0);
		}
		
		public override string GetStringValue()
		{
			return this.value.ToString();
		}
		
		#endregion
	}
}
