//
//  FileScriptBase.cs
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
    public abstract class FileScriptBase : DesktopScriptBase
    {
        public delegate void ScriptProcessFileHandler (object sender, string file);

        #region Script params

        public string [] Files { get; set; }

        public IList<string> AllowedExtensions { get; set; } = new List<string> ();

        public bool ContinueOnErrors { get; set; } = false;

        #endregion

        protected FileScriptBase (string [] args) : base (args)
        {
        }

        protected override int Process ()
        {
            foreach (var file in Files ?? Enumerable.Empty<string> ()) {
                var result = 0;

                try {
                    var allowProcessFile = true;

                    if (AllowedExtensions.Count > 0) {
                        var ext = Path.GetExtension (file).ToLowerInvariant ();
                        allowProcessFile = null != AllowedExtensions.FirstOrDefault (e => e == ext);
                    }

                    if (!File.Exists (file)) {
                        throw new FileNotFoundException ("File not found.", file);
                    }

                    if (allowProcessFile) {
                        PreProcessFile (file);
                        result = ProcessFile (file);
                        PostProcessFile (file);
                    }
                }
                catch (Exception ex) {
                    ProcessFileCatch (ex);
                    result = 1;
                }

                if (result != 0 && !ContinueOnErrors) {
                    return 1;
                }
            }

            return 0;
        }

        public event ScriptProcessFileHandler OnPreProcessFile;

        public event ScriptProcessFileHandler OnPostProcessFile;

        public event ScriptProcessCatchHandler OnProcessFileCatch;

        public abstract int ProcessFile (string file);

        protected virtual void PreProcessFile (string file)
        {
            if (OnPreProcessFile != null) {
                OnPreProcessFile (this, file);
            }
        }

        protected virtual void PostProcessFile (string file)
        {
            if (OnPostProcessFile != null) {
                OnPostProcessFile (this, file);
            }
        }

        protected virtual void ProcessFileCatch (Exception ex)
        {
            if (OnProcessFileCatch != null) {
                OnProcessFileCatch (this, ex);
            }

            Log.WriteException (ex);
        }
    }
}

