
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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using mxd.DukeBuilder.Actions;
using mxd.DukeBuilder.Windows;

#endregion

namespace mxd.DukeBuilder
{
	internal class Launcher : IDisposable
	{
		#region ================== Constants

		//private const string NUMBERS = "0123456789";

		#endregion

		#region ================== Variables

		private string tempmap;
		private bool isdisposed;
		
		#endregion

		#region ================== Properties

		public string TempWAD { get { return tempmap; } }
		
		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public Launcher()
		{
			// Initialize
			//CleanTempFile(manager);

			// Bind actions
			General.Actions.BindMethods(this);
		}

		// Disposer
		public void Dispose()
		{
			// Not yet disposed?
			if(!isdisposed)
			{
				// Unbind actions
				General.Actions.UnbindMethods(this);
				
				// Remove temporary file
				CleanTempFile();
				
				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Parameters

		// This takes the unconverted parameters (with placeholders) and converts it
		// to parameters with full paths, names and numbers where placeholders were put.
		// The tempfile must be the full path and filename to the PWAD file to test.
		public string ConvertParameters(string parameters, int skill)
		{
			//mxd. Supported wildcards:
			// %S - skill number
			// %MD - map directory
			// %MN - map filename
			// %NM - replaced with "No monsters" parameter
			
			string outp = parameters;
			string skillnumber = skill.ToString();
			string mapfolder = "";
			string mapfilename = Path.GetFileName(tempmap);
			string nomonsters = (!General.Settings.TestMonsters ? General.Map.Config.NoMonstersParameter : "");

			// Set map location if it's not in the engine folder
			if(General.Map.FilePath != Path.GetDirectoryName(General.Map.ConfigSettings.TestProgram))
			{
				mapfolder = General.Map.FilePath;
			}

			if(string.IsNullOrEmpty(mapfolder) || mapfolder.Contains(" "))
			{
				mapfolder = "\"" + mapfolder + "\"";
			}

			if(mapfilename.Contains(" "))
			{
				mapfilename = "\"" + mapfilename + "\"";
			}

			// Make sure all our placeholders are in uppercase
			outp = Regex.Replace(outp, "%S", "%S", RegexOptions.IgnoreCase);
			outp = Regex.Replace(outp, "%MD", "%MD", RegexOptions.IgnoreCase);
			outp = Regex.Replace(outp, "%MN", "%MN", RegexOptions.IgnoreCase);
			outp = Regex.Replace(outp, "%NM", "%NM", RegexOptions.IgnoreCase);

			// Replace placeholders with actual values
			outp = outp.Replace("%S", skillnumber);
			outp = outp.Replace("%MN", mapfilename);
			outp = outp.Replace("%MD", mapfolder);
			outp = outp.Replace("%NM", nomonsters);
			
			// Return result
			return outp;
		}

		#endregion

		#region ================== Test

		// This saves the map to a temporary file and launches a test
		[BeginAction("testmap")]
		public void Test()
		{
			TestAtSkill(General.Map.ConfigSettings.TestSkill);
		}
		
		// This saves the map to a temporary file and launches a test wit hthe given skill
		public void TestAtSkill(int skill)
		{
			Cursor oldcursor = Cursor.Current;

			// Check if configuration is OK
			if((General.Map.ConfigSettings.TestProgram == "") || !File.Exists(General.Map.ConfigSettings.TestProgram))
			{
				// Show message
				Cursor.Current = Cursors.Default;
				DialogResult result = General.ShowWarningMessage("Your game engine is not set for the current game configuration. Would you like to set up your game engine now?", MessageBoxButtons.YesNo);
				if(result == DialogResult.Yes)
				{
					// Show game configuration on the right page
					General.MainWindow.ShowConfigurationPage(0, General.Map.ConfigSettings);
				}
				return;
			}

			// No custom parameters?
			if(!General.Map.ConfigSettings.CustomParameters)
			{
				// Set parameters to the default ones
				General.Map.ConfigSettings.TestParameters = General.Map.Config.TestParameters;
				//General.Map.ConfigSettings.TestShortPaths = General.Map.Config.TestShortPaths;
			}
			
			// Remove old temporary file
			CleanTempFile();
			
			// Save map to temporary file
			Cursor.Current = Cursors.WaitCursor;
			tempmap = General.MakeTempFilename(General.Map.FilePath, "map");
			General.Plugins.OnMapSaveBegin(SavePurpose.Testing);
			if(General.Map.SaveMap(tempmap, SavePurpose.Testing))
			{
				// No compiler errors?
				//if(General.Map.Errors.Count == 0)
				//{
					// Make arguments
					string args = ConvertParameters(General.Map.ConfigSettings.TestParameters, skill);

					// Setup process info
					ProcessStartInfo processinfo = new ProcessStartInfo();
					processinfo.Arguments = args;
					processinfo.FileName = General.Map.ConfigSettings.TestProgram;
					processinfo.CreateNoWindow = false;
					processinfo.ErrorDialog = false;
					processinfo.UseShellExecute = true;
					processinfo.WindowStyle = ProcessWindowStyle.Normal;
					processinfo.WorkingDirectory = Path.GetDirectoryName(processinfo.FileName);

					// Output info
					General.WriteLogLine("Running test program: " + processinfo.FileName);
					General.WriteLogLine("Program parameters:  " + processinfo.Arguments);

					// Disable interface
					General.MainWindow.DisplayStatus(StatusType.Busy, "Waiting for game application to finish...");

					try
					{
						// Start the program
						Process process = Process.Start(processinfo);

						// Wait for program to complete
						while(!process.WaitForExit(10))
						{
							General.MainWindow.Update();
						}

						// Done
						TimeSpan deltatime = TimeSpan.FromTicks(process.ExitTime.Ticks - process.StartTime.Ticks);
						General.WriteLogLine("Test program has finished.");
						General.WriteLogLine("Run time: " + deltatime.TotalSeconds.ToString("###########0.00") + " seconds");
					}
					catch(Exception e)
					{
						// Unable to start the program
						General.ShowErrorMessage("Unable to start the test program, " + e.GetType().Name + ": " + e.Message, MessageBoxButtons.OK); ;
					}
					
					General.MainWindow.DisplayReady();
				/*}
				else
				{
					General.MainWindow.DisplayStatus(StatusType.Warning, "Unable to test the map due to script errors.");
				}*/
			}

			// Remove new temporary file
			CleanTempFile();
			
			// Done
			General.Map.Graphics.Reset();
			General.Plugins.OnMapSaveEnd(SavePurpose.Testing);
			General.MainWindow.RedrawDisplay();
			General.MainWindow.FocusDisplay();
			Cursor.Current = oldcursor;
		}
		
		// This deletes the current temp file
		private void CleanTempFile()
		{
			try { if(File.Exists(tempmap)) File.Delete(tempmap); } catch { }
		}

		#endregion
	}
}
