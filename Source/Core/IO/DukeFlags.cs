namespace mxd.DukeBuilder.IO
{
	public class DukeSectorFlags : ISectorFlags
	{
		public string TextureExpansion { get { return "8"; } }
		public string RelativeAlignment { get { return "64"; } }
		public string Sloped { get { return "2"; } }
		public string Parallaxed { get { return "1"; } }
		public string SwapXY { get { return "4"; } }
		public string FlipX { get { return "16"; } }
		public string FlipY { get { return "32"; } }
	}

	public class DukeWallFlags : IWallFlags
	{
		public string BlockHitscan { get { return "64"; } }
		public string BlockMove { get { return "1"; } }
		public string SwapBottomImage { get { return "2"; } }
		public string AlignImageToBottom { get { return "4"; } }
		public string FlipX { get { return "8"; } }
		public string FlipY { get { return "256"; } }
		public string Masked { get { return "16"; } }
		public string MaskedSolid { get { return "32"; } }
		public string SemiTransparent { get { return "128"; } }
		public string Transparent { get { return "512"; } }
	}

	public class DukeSpriteFlags : ISpriteFlags
	{
		public string WallAligned { get { return "16"; } }
		public string FlatAligned { get { return "32"; } }
		public string TrueCentered { get { return "128"; } }
		public string OneSided { get { return "64"; } }
		public string SemiTransparent { get { return "2"; } }
		public string Transparent { get { return "512"; } }
		public string FlipX { get { return "4"; } }
		public string FlipY { get { return "8"; } }
	}
}
