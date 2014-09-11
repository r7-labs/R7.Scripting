//  
//  DateTimeHelper.cs
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

namespace R7.Scripting
{
	/// <summary>
	/// 
	/// </summary>
	public class DateTimeHelper
	{
		public static string IsoToday
		{
			get { return DateTime.Now.ToString ("yyMMdd"); }
		}
		
		public static bool IsIsoDate (string date)
		{
			if (date.Length != 6)
				return false;
			
			int a;
			if (!int.TryParse (date, out a))
				return false;
			
			int day = int.Parse (date.Substring (4, 2));
			if (day > 31 || day < 1)
				return false;
			
			int month = int.Parse (date.Substring (2, 2));
			if (month > 12 || month < 1)
				return false;
			
			return true;
		}
	}

}

