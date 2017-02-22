#region ================== Namespaces

using System.Collections.Generic;
using mxd.DukeBuilder.Config;
using mxd.DukeBuilder.Map;

#endregion

namespace mxd.DukeBuilder.Types
{
	[TypeHandler(PropertyType.ChannelAny, "Any Channel")]
	internal class ChannelAnyHandler : EnumOptionHandler
	{
		#region ================== Properties

		public override int MinValue { get { return General.Map.FormatInterface.MinTag; } }
		public override int MaxValue { get { return General.Map.FormatInterface.MaxTag; } }

		#endregion

		#region ================== Methods

		// When set up for an argument
		public override void SetupProperty(TypeHandlerAttribute attr, PropertyInfo propinfo)
		{
			base.SetupProperty(attr, propinfo);
			
			// Sort in descending order
			List<int> lotagslist = new List<int>(GetLoTags());
			lotagslist.Sort((a, b) => -1 * a.CompareTo(b));

			// Create enums list
			list = new EnumList();
			foreach(int i in lotagslist)
			{
				list.Add(new EnumItem(i.ToString(), i.ToString()));
			}
		}

		// Collect LoTags from all map elements
		protected virtual IEnumerable<int> GetLoTags()
		{
			HashSet<int> lotags = new HashSet<int> { 0 };
			foreach(Sidedef s in General.Map.Map.Sidedefs) if(s.LoTag > 0) lotags.Add(s.LoTag);
			foreach(Sector s in General.Map.Map.Sectors)   if(s.LoTag > 0) lotags.Add(s.LoTag);
			foreach(Thing s in General.Map.Map.Things)     if(s.LoTag > 0) lotags.Add(s.LoTag);
			return lotags;
		}

		#endregion
	}
}
