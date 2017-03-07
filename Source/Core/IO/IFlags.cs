namespace mxd.DukeBuilder.IO
{
	public interface ISectorFlags
	{
		string TextureExpansion { get; } // 1 texture pixel == 16 mu when unset, 8 mu when set
		string RelativeAlignment { get; } // When set, texture will be aligned to the first wall
		string Sloped { get; }
		string Parallaxed { get; }
		string SwapXY { get; }
		string FlipX { get; }
		string FlipY { get; }
	}

	public interface IWallFlags
	{
		string BlockHitscan { get; }
		string BlockMove { get; }
		string SwapBottomImage { get; }
		string AlignImageToBottom { get; }
		string FlipX { get; }
		string FlipY { get; }
		string Masked { get; }
		string MaskedSolid { get; }
		string SemiTransparent { get; }
		string Transparent { get; }
	}

	public interface ISpriteFlags
	{
		string WallAligned { get; }
		string FlatAligned { get; }
		string TrueCentered { get; }
		string OneSided { get; }
		string SemiTransparent { get; }
		string Transparent { get; }
		string FlipX { get; }
		string FlipY { get; }
	}
}
