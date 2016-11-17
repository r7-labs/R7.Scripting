﻿//
//  GenericFileProcessingScript.cs
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

namespace R7.Scripting
{
    public sealed class GenericFileProcessingScript: FileProcessingScriptBase
    {
        private readonly Func<string, int> processFileCallback;

        public GenericFileProcessingScript (string [] args, Func<string, int> processFileCallback): base (args)
        {
            this.processFileCallback = processFileCallback;
        }

        public override int ProcessFile (string file)
        {
            return processFileCallback (file);
        }
    }
}
