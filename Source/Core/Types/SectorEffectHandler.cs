
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

using mxd.DukeBuilder.Config;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.SectorEffect, "Sector Effect")]
	internal class SectorEffectHandler : EnumOptionHandler
	{
		#region ================== Properties

		public override int MinValue { get { return 0; } }

		#endregion

		#region ================== Methods

		// When set up for an argument
		public override void SetupProperty(TypeHandlerAttribute attr, PropertyInfo arginfo)
		{
			base.SetupProperty(attr, arginfo);

			// Keep enum list reference
			list = General.Map.Config.SectorEffectsList;
		}

		#endregion
	}
}
