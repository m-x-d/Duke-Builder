#region ================== Namespaces

using System.Collections.Generic;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.ChannelSprite, "Sprite Channel")]
	internal class ChannelSpriteHandler : ChannelAnyHandler
	{
		// Collect LoTags from sprites
		protected override IEnumerable<int> GetLoTags()
		{
			HashSet<int> lotags = new HashSet<int> { 0 };

			if(propinfo.ChannelTargets.Count > 0)
			{
				foreach(Thing s in General.Map.Map.Things)
				{
					if(s.LoTag > 0 && propinfo.ChannelTargets.Contains(s.TileIndex))
						lotags.Add(s.LoTag);
				}
			}
			else
			{
				foreach(Thing s in General.Map.Map.Things) if(s.LoTag > 0) lotags.Add(s.LoTag);
			}

			return lotags;
		}
	}
}
