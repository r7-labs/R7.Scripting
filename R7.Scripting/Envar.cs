//
//  Envar.cs
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

namespace R7.Scripting
{
	public class Envar
	{
		public string Name { get; set; }

		public string Value
		{ 
			get { return Environment.GetEnvironmentVariable (Name); }
			set { Environment.SetEnvironmentVariable (Name, value); }
		}

		public bool IsEmpty
		{
			get { return string.IsNullOrWhiteSpace (Environment.GetEnvironmentVariable (Name)); }
		}

		public static implicit operator string (Envar evar) 
		{
			return Environment.GetEnvironmentVariable (evar.Name);
		}

		public Envar (string name)
		{
			Name = name;
		}

		public Envar (string name, string value)
		{
			Name = name;
			Value = value;
		}
	}

	public class Env
	{
		public static bool IsEmpty (string name)
		{
			return string.IsNullOrWhiteSpace (Environment.GetEnvironmentVariable (name));
		}
		
		public static bool IsEmpty (Envar evar)
		{
			return string.IsNullOrWhiteSpace (Environment.GetEnvironmentVariable (evar.Name));
		}

		public static string Get (string name)
		{
			return Environment.GetEnvironmentVariable (name);
		}
		
		public static void Set (string name, string _value)
		{
			Environment.SetEnvironmentVariable (name, _value);
		}

	}
}

