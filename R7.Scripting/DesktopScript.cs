//
//  DesktopScript.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using Notifications;

namespace R7.Scripting
{
    public abstract class DesktopScript : Script
    {
        #region Script params

        public int LongOperationTimeout { get; set; } = 10;

        public int ExpireTimeoutBase { get; set; } = 15;

        public bool DisableNotifications { get; set; } = false;

        #endregion

        protected DesktopScript (string [] args): base (args)
        {
        }

        private Urgency GetUrgencyByResult (int result)
        {
            if (result == 0) {
                return Urgency.Low;
            }

            return Urgency.Normal;
        }

        private string GetDialogIconByResult (int result)
        {
            if (result == 0) {
                return "dialog-information";
            }

            return "dialog-warning";
        }

        DateTime startTime;

        protected override void PreProcess ()
        {
            base.PreProcess ();

            startTime = DateTime.Now;
        }

        protected override void PostProcess ()
        {
            base.PostProcess ();
            
            var scriptExecutionTime = DateTime.Now - startTime;
            if (scriptExecutionTime.TotalSeconds > LongOperationTimeout) {
                if (!DisableNotifications) {
                    new Notification {
                        Summary = ScriptFile + " finished.",
                        Body = "",
                        IconName = GetDialogIconByResult (Result),
                        Urgency = GetUrgencyByResult (Result),
                        Timeout = 1000 * ExpireTimeoutBase * ((Result == 0) ? 1 : 2),
                    }.Show ();
                }
            }
        }

        protected override void ProcessCatch (Exception ex)
        {
            base.ProcessCatch (ex);
            
            if (!DisableNotifications) {
                new Notification {
                    Summary = ScriptFile + ": " + ex.Message,
                    Body = ex.StackTrace,
                    IconName = "dialog-error",
                    Urgency = Urgency.Critical,
                    Timeout = 1000 * ExpireTimeoutBase * 4
                }.Show ();
            }
        }
    }
}
