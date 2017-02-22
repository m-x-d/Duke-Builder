#region ================== Copyright (c) 2007 Pascal vd Heiden

using System;
using System.Runtime.InteropServices;

#endregion

namespace mxd.DukeBuilder
{
	public class Clock
	{
		#region ================== Declarations
		
		[DllImport("winmm.dll")]
		private static extern uint timeGetTime();

		#endregion
		
		#region ================== Constants

		#endregion

		#region ================== Variables

		// Settings
		private uint lasttime;
		private uint currenttime;
		
		// Disposing
		private bool isdisposed;

		#endregion

		#region ================== Properties

		// Settings
		public double CurrentTime { get { return currenttime; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public Clock()
		{
			GetCurrentTime();
			
			// We have no destructor
			GC.SuppressFinalize(this);
		}
		
		// Disposer
		public void Dispose()
		{
			// Not already disposed?
			if(!isdisposed)
			{
				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Methods

		// This returns the current time in milliseconds as a uint
		private static uint GetTime()
		{
			// Use Windows' timeGetTime
			return timeGetTime();
		}
		
		// This queries the system for the current time
		public double GetCurrentTime()
		{
			// Get the current system time
			uint nexttime = GetTime();

			// Determine delta time since previous update
			// (this takes care of time wrapping around to 0)
			uint deltatime;
			if(nexttime < lasttime)
				deltatime = (uint.MaxValue - lasttime) + nexttime;
			else
				deltatime = nexttime - lasttime;

			// Add the elapsed time to our internal time
			currenttime += deltatime;

			lasttime = nexttime;

			return currenttime;
		}
		
		#endregion
	}
}
