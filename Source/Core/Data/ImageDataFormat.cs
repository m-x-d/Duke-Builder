
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
using mxd.DukeBuilder.IO;

#endregion

namespace mxd.DukeBuilder.Data
{
	// Input guess formats
	internal enum SupportedImageFormat //mxd
	{
		UNKNOWN, // No clue
		ART,	 // Should be Build ART image
	}
	
	internal static class ImageDataFormat
	{
		// File format signatures
		//TODO: eduke supports PNG, JPG, DDS, TGA, BMP, GIF and PCX images
		private static readonly int[] PNG_SIGNATURE = { 137, 80, 78, 71, 13, 10, 26, 10 };
		private static readonly int[] GIF_SIGNATURE = { 71, 73, 70 };
		private static readonly int[] BMP_SIGNATURE = { 66, 77 };
		private static readonly int[] DDS_SIGNATURE = { 68, 68, 83, 32 };

		// This check image data and returns the appropriate image reader
		public static IImageReader GetImageReader(Stream data, SupportedImageFormat guessformat, Playpal palette)
		{
			//BinaryReader bindata = new BinaryReader(data);

			// This can ONLY be an ART file, right?
			if(guessformat == SupportedImageFormat.ART && data.Length > 0)
			{
				return new ARTImageReader(palette);
			}
			
			// First check the formats that provide the means to 'ensure' that
			// it actually is that format. Then guess the Doom image format.

			// Data long enough to check for signatures?
			/*if(data.Length > 10)
			{
				// Check for PNG signature
				data.Seek(0, SeekOrigin.Begin);
				if(CheckSignature(data, PNG_SIGNATURE)) return new FileImageReader();

				// Check for DDS signature
				data.Seek(0, SeekOrigin.Begin);
				if(CheckSignature(data, DDS_SIGNATURE)) return new FileImageReader();

				// Check for GIF signature
				data.Seek(0, SeekOrigin.Begin);
				if(CheckSignature(data, GIF_SIGNATURE)) return new FileImageReader();

				// Check for BMP signature
				data.Seek(0, SeekOrigin.Begin);
				if(CheckSignature(data, BMP_SIGNATURE))
				{
					// Check if data size matches the size specified in the data
					if(bindata.ReadUInt32() <= data.Length) return new FileImageReader();
				}
			}*/
			
			// Format not supported
			return new UnknownImageReader();
		}

		// This checks a signature as byte array
		// NOTE: Expects the stream position to be at the start of the
		// signature, and expects the stream to be long enough.
		private static bool CheckSignature(Stream data, int[] sig)
		{
			// Go for all bytes
			foreach(int t in sig)
			{
				// When byte doesnt match the signature, leave
				if(data.ReadByte() != t) return false;
			}

			// Signature matches
			return true;
		}
	}
}
