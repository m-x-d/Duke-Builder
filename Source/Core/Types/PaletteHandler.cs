#region ================== Namespaces

using mxd.DukeBuilder.Config;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.Palette, "Palette Index")]
	internal class PaletteHandler : EnumOptionHandler
	{
		#region ================== Methods

		// When set up for an argument
		public override void SetupProperty(TypeHandlerAttribute attr, PropertyInfo arginfo)
		{
			base.SetupProperty(attr, arginfo);

			// Keep enum list reference
			list = General.Map.Config.PalettesList;
		}

		#endregion
	}
}
