//  
//  Command.cs
//  
//  Author:
//       R7 <>
// 
//  Copyright (c) 2012 R7
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

namespace R7.Scripting
{
	public class Command
	{
		private static int defaultWaitTime = 120000;

		public static int DefaultWaitTime
		{
			get { return defaultWaitTime; }
			set { defaultWaitTime = value; }
		}

		public static int Run (string command, string arguments, int waitms = -1)//, bool shellExecute = true)
		{
			var exitCode = 1;

			if (waitms < 0) waitms = defaultWaitTime;

			var process = new Process ();
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arguments;
			// process.StartInfo.UseShellExecute = shellExecute;
			process.Start ();

			if (process.WaitForExit (waitms))
				exitCode = process.ExitCode;

			process.Close ();

			return exitCode;
		}
		
		public static void RunNoWait (string command, string arguments = "")
		{
			var process = new Process ();
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arguments;
			process.Start ();
		}

		public static string RunToFile (string command, string arguments, string file, int waitms =-1, bool createOrAppend = true)
		{	
			var result = string.Empty;
			if (waitms < 0) waitms = defaultWaitTime;

			var process = new Process ();
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;

			if (process.Start ())
			{
				process.WaitForExit (waitms);
			
				if (!process.StandardOutput.EndOfStream)
						result = process.StandardOutput.ReadToEnd ();

				if (createOrAppend)
					File.AppendAllText(file,result);
				else
					File.WriteAllText(file, result);				
				
				/*
				var fs = new FileStream (file, (createOrAppend)? FileMode.Create : FileMode.Append, FileAccess.Write, FileShare.None);
				var sw = new StreamWriter(fs);
				sw.Write(result);
				sw.Close ();
				fs.Close ();
				*/
			}
			process.Close ();

			return result;
		}


		public static string RunToString (string command, string arguments, int waitms =-1)
		{	
			var result = string.Empty;
			if (waitms < 0) waitms = defaultWaitTime;

			var process = new Process ();
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			
			if (process.Start ())
			{
				process.WaitForExit (waitms);
				
				if (!process.StandardOutput.EndOfStream)
					result = process.StandardOutput.ReadToEnd ();

			}
			process.Close ();
			
			return result;
		}


	} // class 
} // namespace

