
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
using System.Collections.Generic;
using mxd.DukeBuilder.IO;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.Map
{
	public abstract class MapElement : IDisposable
	{
		#region ================== Variables
		
		// List index
		protected int listindex;
		
		// Marking
		protected bool marked;
		
		// Disposing
		protected bool isdisposed;

		//mxd. Hashing
		private static int hashcounter;
		private readonly int hashcode;
		
		#endregion
		
		#region ================== Properties

		public int Index { get { return listindex; } internal set { listindex = value; } }
		public bool Marked { get { return marked; } set { marked = value; } }
		public bool IsDisposed { get { return isdisposed; } }
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		protected MapElement()
		{
			// Initialize
			hashcode = hashcounter++; //mxd
		}

		// Disposer
		public virtual void Dispose()
		{
			// Done
			isdisposed = true;
		}

		#endregion

		#region ================== Methods

		protected static void ReadWrite(IReadWriteStream s, ref Dictionary<string, bool> flags)
		{
			if(s.IsWriting)
			{
				s.wInt(flags.Count);
				foreach(KeyValuePair<string, bool> f in flags)
				{
					s.wString(f.Key);
					s.wBool(f.Value);
				}
			}
			else
			{
				int c;
				s.rInt(out c);

				flags = new Dictionary<string, bool>(c, StringComparer.Ordinal);
				for(int i = 0; i < c; i++)
				{
					string t;
					s.rString(out t);
					bool b;
					s.rBool(out b);
					flags.Add(t, b);
				}
			}
		}
		
		// This must implement the call to the undo system to record the change of properties
		protected abstract void BeforePropsChange();

		public static int CalculateBrightness(int shade) { return CalculateBrightness(shade, 255); }
		public static int CalculateBrightness(int shade, byte alpha)
		{
			//TODO: more correct shade implementation? Also calculate fog density factor?
			int level = 256 - General.Clamp(shade, 0, 32) * 8;
			
			//float flevel = level;

			// Simulat doom light levels
			//if((level < 192) && General.Map.Config.DoomLightLevels)
				//flevel = (192.0f - (float)(192 - level) * 1.5f);

			byte blevel = (byte)General.Clamp(level, 0, 255);
			PixelColor c = new PixelColor(alpha, blevel, blevel, blevel);
			return c.ToInt();
		}

		//mxd. This greatly speeds up Dictionary lookups
		public override int GetHashCode()
		{
			return hashcode;
		}
		
		#endregion
	}
}
