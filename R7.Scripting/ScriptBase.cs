//
//  ScriptBase.cs
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

namespace R7.Scripting
{
    #region Delegates for script events

    public delegate void ScriptHandler (object sender);

    public delegate void ScriptProcessCatchHandler (object sender, Exception ex);

    #endregion

    public abstract class ScriptBase
    {
        public Log Log { get; protected set; }

        public string [] Args { get; protected set; }

        public int Result { get; protected set; }

        public string ScriptFile {
            get { return Path.GetFileNameWithoutExtension (Args [0]); }
        }

        protected ScriptBase (string [] args)
        {
            Args = args;
            Log = new Log (ScriptFile);
        }

        protected ScriptBase (string scriptFile)
        {
            Args = new [] { scriptFile };
            Log = new Log (ScriptFile);
        }

        public int Run ()
        {
            try {
                PreProcess ();
                Result = Process ();
                PostProcess ();

                return Result;
            } 
            catch (Exception ex) {
                ProcessCatch (ex);
            } 
            finally {
                ProcessFinally ();
            }

            return 1;
        }

        public event ScriptHandler OnPreProcess;

        public event ScriptHandler OnPostProcess;

        public event ScriptProcessCatchHandler OnProcessCatch;

        public event ScriptHandler OnProcessFinally;

        protected virtual void PreProcess ()
        {
            if (OnPreProcess != null) {
                OnPreProcess (this);
            }
        }

        protected abstract int Process ();

        protected virtual void PostProcess ()
        {
            if (OnPostProcess != null) {
                OnPostProcess (this);
            }
        }

        protected virtual void ProcessCatch (Exception ex)
        {
            if (OnProcessCatch != null) {
                OnProcessCatch (this, ex);
            }

            Log.WriteException (ex);
        }

        protected virtual void ProcessFinally ()
        {
            if (OnProcessFinally != null) {
                OnProcessFinally (this);
            }

            Log.Close ();
        }
    }
}
