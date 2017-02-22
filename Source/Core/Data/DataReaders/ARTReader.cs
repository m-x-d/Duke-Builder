#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using mxd.DukeBuilder.IO;

#endregion

namespace mxd.DukeBuilder.Data.DataReaders
{
	internal sealed class ARTReader : DataReader
	{
		// File objects
		private ART artfile;
		private FileStream file;
		
		#region ================== Constructor

		// Constructor for loading loose ART files
		public ARTReader(DataLocation dl) : base(dl)
		{
			General.WriteLogLine("Opening ART resource \"" + location.location + "\"");

			if(!File.Exists(location.location))
				throw new FileNotFoundException("Could not find the file \"" + location.location + "\"", location.location);

			Open(location.location);
		}

		// Destructor
		~ARTReader()
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
				General.WriteLogLine("Closing ART resource \"" + location.location + "\"");
				
				// Clean up
				//if(lumps != null) foreach(Lump l in lumps) l.Dispose();
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
			//var reader = new BinaryReader(file, GRPReader.ENCODING);

			// Create quick access collections
			//confiles = new Dictionary<int, string>();
			//artfiles = new List<ART>();

			artfile = new ART(Path.GetFileName(path));
			artfile.Create(file);
		}

		protected override string GetTitle()
		{
			return Path.GetFileName(location.location);
		}

		public override Stream GetImageStream(int tileindex)
		{
			// Error when suspended
			if(issuspended) throw new Exception("Data reader is suspended");
			
			// Return tile stream
			return (artfile.Tiles.ContainsKey(tileindex) ? artfile.Tiles[tileindex].Stream : null);
		}

		public override ICollection<ImageData> LoadImages()
		{
			// Error when suspended
			if(issuspended) throw new Exception("Data reader is suspended");

			List<ImageData> images = new List<ImageData>();

			foreach(Tile t in artfile.Tiles.Values)
			{
				ARTImage img = new ARTImage(t);
				images.Add(img);
			}

			// Add images to the container-specific texture set
			foreach(ImageData img in images) textureset.AddImage(img);

			// Done
			return images;
		}

		#endregion
	}
}
