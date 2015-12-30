//
//  Command.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace R7.Scripting
{
	public static class Command
	{
        [Obsolete]
        public static int DefaultWaitTime { get; set; }

        public static int Run (string command, string arguments = "", int waitms = int.MaxValue)
		{
			var exitCode = 1;

            using (var process = new Process ()) {
    			process.StartInfo.FileName = command;
    			process.StartInfo.Arguments = arguments;
    			process.StartInfo.UseShellExecute = false;
    			
                if (process.Start ()) {
                    if (process.WaitForExit (waitms)) {
                        exitCode = process.ExitCode;
                    }
                }
            }

			return exitCode;
		}
		
		public static void RunNoWait (string command, string arguments = "")
		{
            Process.Start (command, arguments);
		}

        public static string RunToFile (string command, string arguments, string file, int waitms = int.MaxValue, bool createOrAppend = true)
		{	
			var result = string.Empty;
			
            using (var process = new Process ()) {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                if (process.Start ()) {
                    if (process.WaitForExit (waitms)) {
                        if (!process.StandardOutput.EndOfStream) {
                            result = process.StandardOutput.ReadToEnd ();
                        }

                        if (createOrAppend) {
                            File.AppendAllText (file, result);
                        }
                        else {
                            File.WriteAllText (file, result);				
                        }
                    }
                }
            }

			return result;
		}

        public static string RunToString (string command, string arguments, int waitms = int.MaxValue)
		{	
			var result = string.Empty;
			
            using (var process = new Process ()) {
    			process.StartInfo.FileName = command;
    			process.StartInfo.Arguments = arguments;
    			process.StartInfo.UseShellExecute = false;
    			process.StartInfo.RedirectStandardOutput = true;
    			
                if (process.Start ()) {
    			    if (process.WaitForExit (waitms)) {
                        if (!process.StandardOutput.EndOfStream) {
                            result = process.StandardOutput.ReadToEnd ();
                        }
                    }
                    process.Close ();
    			}
            }

			return result;
		}

        public static List<string> RunToLines (string command, string arguments, int waitms = int.MaxValue)
		{	
			var result = new List<string> ();
			
            using (var process = new Process ()) {
                process.StartInfo.FileName = command;
    			process.StartInfo.Arguments = arguments;
    			process.StartInfo.UseShellExecute = false;
    			process.StartInfo.RedirectStandardOutput = true;

			    if (process.Start ()) {
                    if	(process.WaitForExit (waitms)) {
                        while (!process.StandardOutput.EndOfStream) {
    						result.Add (process.StandardOutput.ReadLine ());
                        }
    				}
                }
			}
			
			return result;
		}
	}
}
    