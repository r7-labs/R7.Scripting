//
//  NotifyScript.cs
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
    public abstract class NotifyScript: Script
    {
        public int NotifyAfterSeconds { get; protected set; }

        protected NotifyScript (string [] args): base (args)
        {
            NotifyAfterSeconds = 10;
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

        public override int Run ()
        {
            try {
                var startTime = DateTime.Now;

                PreProcess ();
                var result = Process ();
                PostProcess ();

                var scriptExecutionTime = DateTime.Now - startTime;
                if (scriptExecutionTime.TotalSeconds > NotifyAfterSeconds) {
                    var notification = new Notification ();
                    notification.Summary = ScriptFile + " finished.";
                    notification.Body = "";
                    notification.IconName = GetDialogIconByResult (result);
                    notification.Urgency = GetUrgencyByResult (result);
                    notification.Show ();

                }

                return result;
            } catch (Exception ex) {
                var notification = new Notification ();
                notification.Summary = ScriptFile + ": " + ex.Message;
                notification.Body = ex.StackTrace;
                notification.IconName = "dialog-error";
                notification.Urgency = Urgency.Critical;
                notification.Show ();

                Log.WriteLine (ex.Message);
            } finally {
                Log.Close ();
            }

            return 1;
        }
    }
}
