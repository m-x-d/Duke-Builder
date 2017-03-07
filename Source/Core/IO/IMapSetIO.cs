
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

namespace mxd.DukeBuilder.IO
{
	public interface IMapSetIO
	{
		int Version { get; }

		int MaxWalls { get; }
		int MaxSectors { get; }
		int MaxSprites { get; }
		float MaxSlope { get; } // In radians
		float MinSlope { get; } // In radians
		int MaxImageOffset { get; }
		int MinImageOffset { get; }
		int MaxImageRepeat { get; }
		int MinImageRepeat { get; }
		int MaxSpriteOffset { get; }
		int MinSpriteOffset { get; }
		int MaxSpriteRepeat { get; }
		int MinSpriteRepeat { get; }
		int MaxTag { get; }
		int MinTag { get; }
		int MaxExtra { get; }
		int MinExtra { get; }
		int MaxShade { get; }
		int MinShade { get; }
		int MaxVisibility { get; }
		int MinVisibility { get; }
		int MaxTileIndex { get; }
		int MinTileIndex { get; }
		int MaxCoordinate { get; }
		int MinCoordinate { get; }
		int MaxSpriteAngle { get; }
		int MinSpriteAngle { get; }

		int LeftBoundary { get; }
		int RightBoundary { get; }
		int TopBoundary { get; }
		int BottomBoundary { get; }

		ISectorFlags SectorFlags { get; }
		IWallFlags WallFlags { get; }
		ISpriteFlags SpriteFlags { get; }
	}
}
