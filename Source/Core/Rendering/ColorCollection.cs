
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

using System;
using System.Globalization;
using System.Drawing;
using SlimDX;
using Configuration = mxd.DukeBuilder.IO.Configuration;

#endregion

namespace mxd.DukeBuilder.Rendering
{
	public enum EditorColor //mxd
	{
		// Sprite colors
		SPRITECOLOR00,
		SPRITECOLOR01,
		SPRITECOLOR02,
		SPRITECOLOR03,
		SPRITECOLOR04,
		SPRITECOLOR05,
		SPRITECOLOR06,
		SPRITECOLOR07,
		SPRITECOLOR08,
		SPRITECOLOR09,
		SPRITECOLOR10,
		SPRITECOLOR11,
		SPRITECOLOR12,
		SPRITECOLOR13,
		SPRITECOLOR14,
		SPRITECOLOR15,
		SPRITECOLOR16,
		SPRITECOLOR17,
		SPRITECOLOR18,
		SPRITECOLOR19,
		// General editing colors
		BACKGROUND,
		VERTICES,
		LINEDEFS_SINGLE,
		LINEDEFS_DOUBLE,
		LINEDEFS_INVALID,
		HIGHLIGHT,
		SELECTION,
		INDICATION,
		GRID,
		GRID1024,
	}
	
	public sealed class ColorCollection
	{
		#region ================== Constants

		// Assist color creation
		private const float BRIGHT_MULTIPLIER = 1.0f;
		private const float BRIGHT_ADDITION = 0.4f;
		private const float DARK_MULTIPLIER = 0.9f;
		private const float DARK_ADDITION = -0.2f;
		
		#endregion

		#region ================== Variables

		// Colors
		private PixelColor[] colors;
		private PixelColor[] brightcolors;
		private PixelColor[] darkcolors;

		// Palette size
		private int colorscount;
		private int thingcolorsoffset;
		
		// Color-correction table
		private byte[] correctiontable;
		
		#endregion
		
		#region ================== Properties
		
		public PixelColor[] Colors { get { return colors; } }
		public PixelColor[] BrightColors { get { return brightcolors; } }
		public PixelColor[] DarkColors { get { return darkcolors; } }

		// Palette size
		public const int SpriteColorsCount = 20;
		public int SpriteColorsOffset { get { return thingcolorsoffset; } }

		public PixelColor Background { get { return colors[(int)EditorColor.BACKGROUND]; } internal set { colors[(int)EditorColor.BACKGROUND] = value; } }
		public PixelColor Vertices { get { return colors[(int)EditorColor.VERTICES]; } internal set { colors[(int)EditorColor.VERTICES] = value; } }
		public PixelColor LinesSingleSided { get { return colors[(int)EditorColor.LINEDEFS_SINGLE]; } internal set { colors[(int)EditorColor.LINEDEFS_SINGLE] = value; } }
		public PixelColor LinesDoubleSided { get { return colors[(int)EditorColor.LINEDEFS_DOUBLE]; } internal set { colors[(int)EditorColor.LINEDEFS_DOUBLE] = value; } }
		public PixelColor LinesInvalid { get { return colors[(int)EditorColor.LINEDEFS_INVALID]; } internal set { colors[(int)EditorColor.LINEDEFS_INVALID] = value; } }
		public PixelColor Highlight { get { return colors[(int)EditorColor.HIGHLIGHT]; } internal set { colors[(int)EditorColor.HIGHLIGHT] = value; } }
		public PixelColor Selection { get { return colors[(int)EditorColor.SELECTION]; } internal set { colors[(int)EditorColor.SELECTION] = value; } }
		public PixelColor Indication { get { return colors[(int)EditorColor.INDICATION]; } internal set { colors[(int)EditorColor.INDICATION] = value; } }
		public PixelColor Grid { get { return colors[(int)EditorColor.GRID]; } internal set { colors[(int)EditorColor.GRID] = value; } }
		public PixelColor Grid1024 { get { return colors[(int)EditorColor.GRID1024]; } internal set { colors[(int)EditorColor.GRID1024] = value; } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor for settings from configuration
		internal ColorCollection(Configuration cfg)
		{
			// Initialize
			colorscount = Enum.GetValues(typeof(EditorColor)).Length;
			thingcolorsoffset = colorscount - SpriteColorsCount;
			colors = new PixelColor[colorscount];
			brightcolors = new PixelColor[colorscount];
			darkcolors = new PixelColor[colorscount];
			
			// Read all colors from config
			for(int i = 0; i < colorscount; i++)
			{
				// Read color
				colors[i] = PixelColor.FromInt(cfg.ReadSetting("colors.color" + i.ToString(CultureInfo.InvariantCulture), 0));
			}

			// Set new colors
			if(colors[(int)EditorColor.SPRITECOLOR00].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR00] = PixelColor.FromColor(Color.DimGray);
			if(colors[(int)EditorColor.SPRITECOLOR01].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR01] = PixelColor.FromColor(Color.RoyalBlue);
			if(colors[(int)EditorColor.SPRITECOLOR02].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR02] = PixelColor.FromColor(Color.ForestGreen);
			if(colors[(int)EditorColor.SPRITECOLOR03].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR03] = PixelColor.FromColor(Color.LightSeaGreen);
			if(colors[(int)EditorColor.SPRITECOLOR04].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR04] = PixelColor.FromColor(Color.Firebrick);
			if(colors[(int)EditorColor.SPRITECOLOR05].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR05] = PixelColor.FromColor(Color.DarkViolet);
			if(colors[(int)EditorColor.SPRITECOLOR06].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR06] = PixelColor.FromColor(Color.DarkGoldenrod);
			if(colors[(int)EditorColor.SPRITECOLOR07].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR07] = PixelColor.FromColor(Color.Silver);
			if(colors[(int)EditorColor.SPRITECOLOR08].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR08] = PixelColor.FromColor(Color.Gray);
			if(colors[(int)EditorColor.SPRITECOLOR09].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR09] = PixelColor.FromColor(Color.DeepSkyBlue);
			if(colors[(int)EditorColor.SPRITECOLOR10].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR10] = PixelColor.FromColor(Color.LimeGreen);
			if(colors[(int)EditorColor.SPRITECOLOR11].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR11] = PixelColor.FromColor(Color.PaleTurquoise);
			if(colors[(int)EditorColor.SPRITECOLOR12].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR12] = PixelColor.FromColor(Color.Tomato);
			if(colors[(int)EditorColor.SPRITECOLOR13].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR13] = PixelColor.FromColor(Color.Violet);
			if(colors[(int)EditorColor.SPRITECOLOR14].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR14] = PixelColor.FromColor(Color.Yellow);
			if(colors[(int)EditorColor.SPRITECOLOR15].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR15] = PixelColor.FromColor(Color.WhiteSmoke);
			if(colors[(int)EditorColor.SPRITECOLOR16].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR16] = PixelColor.FromColor(Color.LightPink);
			if(colors[(int)EditorColor.SPRITECOLOR17].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR17] = PixelColor.FromColor(Color.DarkOrange);
			if(colors[(int)EditorColor.SPRITECOLOR18].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR18] = PixelColor.FromColor(Color.DarkKhaki);
			if(colors[(int)EditorColor.SPRITECOLOR19].ToInt() == 0) colors[(int)EditorColor.SPRITECOLOR19] = PixelColor.FromColor(Color.Goldenrod);
			
			// Create assist colors
			CreateAssistColors();
			
			// Create color correction table
			CreateCorrectionTable();
		}

		#endregion

		#region ================== Methods
		
		// This generates a color-correction table
		internal void CreateCorrectionTable()
		{
			// Determine amounts
			float gamma = (General.Settings.ImageBrightness + 10) * 0.1f;
			float bright = General.Settings.ImageBrightness * 5f;
			
			// Make table
			correctiontable = new byte[256];
			
			// Fill table
			for(int i = 0; i < 256; i++)
			{
				byte b;
				float a = i * gamma + bright;
				if(a < 0f) b = 0; else if(a > 255f) b = 255; else b = (byte)a;
				correctiontable[i] = b;
			}
		}
		
		// This applies color-correction over a block of pixel data
		internal unsafe void ApplyColorCorrection(PixelColor* pixels, int numpixels)
		{
			for(PixelColor* cp = pixels + numpixels - 1; cp >= pixels; cp--)
			{
				cp->r = correctiontable[cp->r];
				cp->g = correctiontable[cp->g];
				cp->b = correctiontable[cp->b];
			}
		}
		
		// This clamps a value between 0 and 1
		private static float Saturate(float v)
		{
			if(v < 0f) return 0f; 
			if(v > 1f) return 1f; 
			return v;
		}

		// This creates assist colors
		internal void CreateAssistColors()
		{
			// Go for all colors
			for(int i = 0; i < colorscount; i++)
			{
				// Create assist colors
				brightcolors[i] = CreateBrightVariant(colors[i]);
				darkcolors[i] = CreateDarkVariant(colors[i]);
			}
		}
		
		// This creates a brighter color
		public PixelColor CreateBrightVariant(PixelColor pc)
		{
			Color4 o = pc.ToColorValue();
			Color4 c = new Color4(1f, 0f, 0f, 0f);
						
			// Create brighter color
			c.Red = Saturate(o.Red * BRIGHT_MULTIPLIER + BRIGHT_ADDITION);
			c.Green = Saturate(o.Green * BRIGHT_MULTIPLIER + BRIGHT_ADDITION);
			c.Blue = Saturate(o.Blue * BRIGHT_MULTIPLIER + BRIGHT_ADDITION);
			return PixelColor.FromInt(c.ToArgb());
		}

		// This creates a darker color
		public PixelColor CreateDarkVariant(PixelColor pc)
		{
			Color4 o = pc.ToColorValue();
			Color4 c = new Color4(1f, 0f, 0f, 0f);

			// Create darker color
			c.Red = Saturate(o.Red * DARK_MULTIPLIER + DARK_ADDITION);
			c.Green = Saturate(o.Green * DARK_MULTIPLIER + DARK_ADDITION);
			c.Blue = Saturate(o.Blue * DARK_MULTIPLIER + DARK_ADDITION);
			return PixelColor.FromInt(c.ToArgb());
		}

		// This saves colors to configuration
		internal void SaveColors(Configuration cfg)
		{
			// Write all colors to config
			for(int i = 0; i < colorscount; i++)
			{
				// Write color
				cfg.WriteSetting("colors.color" + i.ToString(CultureInfo.InvariantCulture), colors[i].ToInt());
			}
		}
		
		#endregion
	}
}
