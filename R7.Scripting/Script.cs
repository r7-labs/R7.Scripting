//
//  Script.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015 Roman M. Yagodin
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
    public abstract class Script
    {
        public Log Log { get; protected set; }

        public string [] Args { get; protected set; }

        public int Result { get; protected set; }

        public string ScriptFile
        {
            get { return Path.GetFileNameWithoutExtension (Args [0]); }
        }

        protected Script (string [] args)
        {
            Args = args;
            Log = new Log (ScriptFile);
        }

        protected Script (string scriptFile)
        {
            Args = new [] { scriptFile };
            Log = new Log (ScriptFile);
        }

        public int Run ()
        {
            try
            {
                OnPreProcess ();
                Result = Process ();
                OnPostProcess ();

                return Result;
            }
            catch (Exception ex)
            {
                OnException (ex);
            }
            finally
            {
                OnFinish ();
            }

            return 1;
        }

        public virtual void OnPreProcess ()
        {}

        public abstract int Process ();

        public virtual void OnPostProcess ()
        {}

        public virtual void OnException (Exception ex)
        {
            Log.WriteLine (ex.Message);
        }

        public virtual void OnFinish ()
        {
            Log.Close ();
        }
    }
}
