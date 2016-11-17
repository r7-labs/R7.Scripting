//
//  FileProcessingScriptBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using System.Linq;
using System.Collections.Generic;

namespace R7.Scripting
{
    public abstract class FileProcessingScriptBase : DesktopScriptBase
    {
        #region Script params

        public string [] Files { get; set; }

        public IList<string> AllowedExtensions { get; set; } = new List<string> ();

        public bool ContinueOnErrors { get; set; } = false;

        #endregion

        protected FileProcessingScriptBase (string [] args) : base (args)
        {
        }

        protected override int Process ()
        {
            // FIXME: If Files == null, method will crash
            foreach (var file in Files) {
                var result = 0;

                try {
                    var allowProcessFile = true;

                    if (AllowedExtensions.Count > 0) {
                        var ext = Path.GetExtension (file).ToLowerInvariant ();
                        allowProcessFile = null != AllowedExtensions.FirstOrDefault (e => e == ext);
                    }

                    if (allowProcessFile) {
                        result = ProcessFile (file);
                    }
                }
                catch (Exception ex) {
                    Log.WriteLine ("Error: " + ex.Message);
                    result = 1;
                }

                if (result != 0 && !ContinueOnErrors) {
                    return 1;
                }
            }

            return 0;
        }

        public abstract int ProcessFile (string file);
    }
}

