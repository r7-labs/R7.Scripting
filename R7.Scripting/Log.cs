//
//  Log.cs
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

namespace R7.Scripting
{
	public class Log
	{
		public Log (string filename, bool echo = true)
		{
            if (!Directory.Exists ("~log"))
            {
                Directory.CreateDirectory ("~log");
            }

			fileName = Path.Combine("~log", filename);

			var fs = new FileStream (fileName, FileMode.Create, FileAccess.Write, FileShare.None);
			log = new StreamWriter(fs);

			Echo = echo;
		}

		public bool Echo { get; set; } 

		private bool empty = true;
		private string fileName;

		protected StreamWriter log = null;
	
		public void WriteLine (string s)
		{
			empty = false;

			log.WriteLine("{0}: {1}", DateTime.Now.ToShortTimeString(), s);	

			if (Echo) 
				Console.WriteLine("{0}: {1}", DateTime.Now.ToShortTimeString(), s);
		}

		public void WriteException (Exception ex)
		{
			empty = false;

			log.WriteLine (string.Format("Error: {0}\nStack Trace: {1}", ex.Message, ex.StackTrace));

            if (Echo)
            {
                Console.WriteLine ("Error: {0}\nStack Trace: {1}", ex.Message, ex.StackTrace);
            }
		}

		public void Close ()
		{
			if (log != null)
			{
				log.Close ();

				// delete log if empty
				if (empty)
				{
					File.Delete (fileName);
					Directory.Delete ("~log");
				}
			}
		}
		
	}
}

