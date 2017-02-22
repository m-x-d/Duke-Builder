#region ================== Namespaces

using System;
using System.Collections.Generic;

#endregion

namespace mxd.DukeBuilder.Config
{
	public class SectorEffectInfo: INumberedTitle, IComparable<SectorEffectInfo>
	{
		#region ================== Variables

		// Properties
		private readonly int index;
		private readonly string title;
		private HashSet<int> setags; // Associated Sector Effector lotags

		#endregion

		#region ================== Properties

		public int Index { get { return index; } }
		public string Title { get { return title; } }
		public HashSet<int> SectorEffectorLotags { get { return setags; } }

		#endregion

		#region ================== Constructor

		internal SectorEffectInfo(int index, string title, HashSet<int> setags)
		{
			this.index = index;
			this.title = title;
			this.setags = setags;
		}

		internal SectorEffectInfo(int index, string title)
		{
			this.index = index;
			this.title = title;
			this.setags = new HashSet<int>();
		}

		#endregion

		#region ================== Methods

		public override string ToString()
		{
			return index + " - " + title;
		}

		public int CompareTo(SectorEffectInfo other)
		{
			if(this.index < other.index) return -1;
			if(this.index > other.index) return 1;
			return 0;
		}

		#endregion
	}
}
