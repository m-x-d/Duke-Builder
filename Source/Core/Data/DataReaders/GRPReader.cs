#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using mxd.DukeBuilder.IO;

#endregion

namespace mxd.DukeBuilder.Data.DataReaders
{
	internal sealed class GRPReader : DataReader
	{
		#region ================== Constants

		private const string GRP_ID = "KenSilverman";
		private const int GRP_FILENAME_LENGTH = 12;
		private static Regex artfile = new Regex(@"^[a-z0-9]{5}\d{3}\.ART$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		#endregion

		#region ================== Structs

		private struct LumpInfo
		{
			public string Name;
			public int Length;

			public LumpInfo(string name, int length)
			{
				Name = name;
				Length = length;
			}
		}

		#endregion

		#region ================== Variables

		// Lumps
		private List<Lump> lumps;
		private Dictionary<int, string> confiles; // <lump index, name>
		private List<ART> artfiles;

		// File objects
		private FileStream file;

		// Encoder
		private static readonly Encoding ENCODING = Encoding.ASCII;

		#endregion

		#region ================== Constructor

		public GRPReader(DataLocation dl) : base(dl)
		{
			General.WriteLogLine("Opening GRP resource \"" + location.location + "\"");

			if(!File.Exists(location.location))
				throw new FileNotFoundException("Could not find the file \"" + location.location + "\"", location.location);

			Open(location.location);
		}

		// Destructor
		~GRPReader()
		{
			// Make sure everything is disposed
			this.Dispose();
		}
		
		// Disposer
		public override void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				General.WriteLogLine("Closing GRP resource \"" + location.location + "\"");
				
				// Clean up
				if(lumps != null) foreach(Lump l in lumps) l.Dispose();
				//if(artfiles != null) foreach(ART a in artfiles) a.Dispose();
				//if(reader != null) reader.Close();
				if(file != null) file.Dispose();

				// Done
				base.Dispose();
			}
		}

		#endregion

		#region ================== Methods

		private void Open(string path)
		{
			// Open in read-only mode
			FileAccess access = FileAccess.Read;
			FileShare share = FileShare.ReadWrite;

			// Open the file stream
			file = File.Open(path, FileMode.OpenOrCreate, access, share);

			// Create file handling tools
			var reader = new BinaryReader(file, ENCODING);

			// Create quick access collections
			confiles = new Dictionary<int, string>();
			artfiles = new List<ART>();

			// Read information from file
			ReadHeaders(reader);
		}

		private void ReadHeaders(BinaryReader reader)
		{
			// Seek to beginning
			file.Seek(0, SeekOrigin.Begin);

			// Check GRP header
			if(ENCODING.GetString(reader.ReadBytes(GRP_ID.Length)) != GRP_ID) throw new IOException("Invalid GRP file header.");

			// Number of lumps
			int numlumps = reader.ReadInt32();
			if(numlumps < 0) throw new IOException("Invalid number of lumps in GRP file.");

			// Dispose old lumps and create new list
			if(lumps != null) foreach(Lump l in lumps) l.Dispose();
			lumps = new List<Lump>(numlumps);

			// Go for all lumps
			List<LumpInfo> lumpinfos = new List<LumpInfo>(numlumps);
			for(int i = 0; i < numlumps; i++)
			{
				// Read lump information
				string name = ENCODING.GetString(reader.ReadBytes(GRP_FILENAME_LENGTH)).TrimEnd('\0');
				int length = reader.ReadInt32();

				// Create lump info
				lumpinfos.Add(new LumpInfo(name, length));
			}

			// Create lumps
			int offset = (int)reader.BaseStream.Position;
			for(int i = 0; i < numlumps; i++)
			{
				LumpInfo info = lumpinfos[i];
				Lump l = new Lump(file, info.Name, offset, info.Length);
				lumps.Add(l);
				offset += info.Length;

				// Also collect ???.CON files
				if(l.Name.EndsWith(".CON")) confiles.Add(i, l.Name);
			}
		}

		public override Playpal LoadPalette()
		{
			// Error when suspended
			if(issuspended) throw new Exception("Data reader is suspended");
			
			// Find PALETTE.DAT
			foreach(var l in lumps) if(l.Name.ToUpperInvariant() == "PALETTE.DAT") return new Playpal(l.Stream);
			return null;
		}

		public override Stream GetImageStream(int tileindex)
		{
			// Error when suspended
			if(issuspended) throw new Exception("Data reader is suspended");

			// Search in ART files
			foreach(var art in artfiles)
			{
				if(art.Tiles.ContainsKey(tileindex)) return art.Tiles[tileindex].Stream;
			}

			return null;
		}

		public override ICollection<ImageData> LoadImages()
		{
			// Error when suspended
			if(issuspended) throw new Exception("Data reader is suspended");

			List<ImageData> images = new List<ImageData>();

			// Find TILES###.ART lumps
			Dictionary<int, Lump> artlumps = new Dictionary<int, Lump>();
			List<int> artlumpnums = new List<int>();
			foreach(var l in lumps)
			{
				if(artfile.IsMatch(l.Name))
				{
					// Get art number
					int num;
					int.TryParse(l.Name.Substring(5, 3), NumberStyles.Integer, CultureInfo.InvariantCulture, out num);

					if(artlumps.ContainsKey(num))
					{
						General.ErrorLogger.Add(ErrorType.Error, "ART file \"" + l.Name + "\" is double-defined in resource \"" + location.location + "\"");
					}
					else
					{
						artlumps.Add(num, l);
						artlumpnums.Add(num);
					}
				}
			}

			// No ART lumps?
			if(artlumps.Count == 0) return images;

			// Check sequentiality
			artlumpnums.Sort();
			List<int> validartlumpnums = new List<int>();
			for(int i = 0; i < artlumpnums.Count - 1; i++)
			{
				validartlumpnums.Add(artlumpnums[i]);
				if(artlumpnums[i + 1] - 1 != artlumpnums[i])
				{
					General.ErrorLogger.Add(ErrorType.Error, "GRP file \"" + location.location + "\" contains inconsistently named ART files (\"TILES" + artlumpnums[i].ToString("D3") + ".ART\" file is missing)");
					break;
				}
			}

			// Create Art readers
			foreach(int i in validartlumpnums)
			{
				ART art = new ART(artlumps[i].Name);
				if(art.Create(artlumps[i].Stream)) artfiles.Add(art);
			}

			// Load textures
			foreach(ART art in artfiles)
			{
				foreach(Tile t in art.Tiles.Values)
				{
					ARTImage img = new ARTImage(t);
					images.Add(img);
				}
			}

			// Add images to the container-specific texture set
			foreach(ImageData img in images) textureset.AddImage(img);

			// Done
			return images;
		}

		// Return a short name for this data location
		protected override string GetTitle()
		{
			return Path.GetFileName(location.location);
		}

		#endregion
	}
}
