
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

using System.IO;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.Data
{
	public sealed class Playpal
	{
		#region ================== Variables

		private PixelColor[] colors;

		#endregion

		#region ================== Properties

		public PixelColor this[int index] { get { return colors[index]; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public Playpal()
		{
			// Create array
			colors = new PixelColor[256];

			// Set all palette entries
			for(int i = 0; i < 256; i++)
			{
				// Set colors to gray
				colors[i].r = 127;
				colors[i].g = 127;
				colors[i].b = 127;
				colors[i].a = 255;
			}
		}

		// Constructor
		public Playpal(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			
			// Create array
			colors = new PixelColor[256];

			// The colors are based on the VGA 262,144 color palette.  The values range from
			// 0-63, so if you want to convert it to a windows palette you will have to multiply each byte by 4.

			// Read all palette entries
			stream.Seek(0, SeekOrigin.Begin);
			for(int i = 0; i < 256; i++)
			{
				// Read colors
				colors[i].r = (byte)(reader.ReadByte() * 4);
				colors[i].g = (byte)(reader.ReadByte() * 4);
				colors[i].b = (byte)(reader.ReadByte() * 4);
				colors[i].a = 255;
			}

			//dbg
			/*System.Drawing.Bitmap pal = new System.Drawing.Bitmap(16, 16);
			int counter = 0;
			for(int w = 0; w < 16; w++)
			{
				for(int h = 0; h < 16; h++)
				{
					PixelColor c = colors[counter++];
					PixelColor pc = new PixelColor(c.a, (byte)(c.r / 4), (byte)(c.g / 4), (byte)(c.b / 4));
					pal.SetPixel(w, h, pc.ToColor());
				}
			}
			pal.Save("palette.png");*/

			// Last palette index is used as transparent color
			colors[255].a = 0;
		}

		#endregion
	}
}
