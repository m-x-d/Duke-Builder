#region ================== Namespaces

using System.Collections.Generic;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.ChannelSector, "Sector Channel")]
	internal class ChannelSectorHandler : ChannelAnyHandler
	{
		#region ================== Methods

		// Collect LoTags from sectors
		protected override IEnumerable<int> GetLoTags()
		{
			HashSet<int> lotags = new HashSet<int> { 0 };
			foreach(Sector s in General.Map.Map.Sectors) if(s.LoTag > 0) lotags.Add(s.LoTag);
			return lotags;
		}

		#endregion
	}
}
