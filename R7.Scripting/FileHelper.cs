//
//  FileHelper.cs
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
using System.Text;
using System.Text.RegularExpressions;

using R7.Utils;

namespace R7.Scripting
{
	public enum FileSource  
	{ 
		CurrentDirectory, 
		Directory, 
		CommandLine, 
		Nautilus, 
		NautilusSelection, 
		NautilusCurrentDirectory, 
		EnvironmentVariable 
	}; 

	/// <summary>
	/// 
	/// </summary>
	public class FileHelper
	{

		public static string [] GetFiles (FileSource source, string path = "")
		{
			var files = new string[0];

			if (source == FileSource.Directory)
				files = Directory.GetFiles (path);
			else if (source == FileSource.CurrentDirectory)
				files = Directory.GetFiles (Directory.GetCurrentDirectory());
			else if (source == FileSource.EnvironmentVariable)
				files = Directory.GetFiles (Environment.GetEnvironmentVariable (path));
			else if (source == FileSource.NautilusSelection)
				files = NauHelper.SelectedFiles;
			else if (source == FileSource.NautilusCurrentDirectory)
				files = Directory.GetFiles (NauHelper.CurrentDirectory);
			else if (source == FileSource.Nautilus)
				files = (NauHelper.IsNothingSelected)? Directory.GetFiles (NauHelper.CurrentDirectory) : NauHelper.SelectedFiles;

			// TODO: Realize commandline

			return files;
		}

		public static bool IsDirectory (string file)
		{
			return (File.GetAttributes (file) & FileAttributes.Directory) != 0;
		}

		#region Translit table
		private static string [,] ruTranslitMachine = { 
			// apply some filename rules
			{@"[^0-9^a-z^а-я^A-Z^А-Я^\-^ё^Ё]", "_"},
			{@"_+", "_"},
			{@"\-+", "-"},
			{@"\A[_\-]+", ""},
			{@"[_\-]+\z", ""},
			{@"([_\-])[_\-]+", "$1"},
			// custom rules
			{@"\bх", "kh" },
			{@"\bХ", "Kh" },
			{@"\Bый", "y"},
			{"ье", "iye" },
			{"ья", "iya" },
			{"ью", "iyu" },
			// main rules
			{"а", "a"}, 
			{"б", "b"}, 
			{"в", "v"},
			{"г", "g"},
			{"д", "d"},
			{"е", "e"},
			{"ё", "yo"},
			{"ж", "zh"},
			{"з", "z"},
			{"и", "i"},
			{"й", "y"},
			{"к", "k"},
			{"л", "l"},
			{"м", "m"},
			{"н", "n"},
			{"о", "o"},
			{"п", "p"},
			{"р", "r"},
			{"с", "s"},
			{"т", "t"},
			{"у", "u"},
			{"ф", "f"},
			{"х", "h"},
			{"ц", "c"},
			{"ч", "ch"},
			{"ш", "sh"},
			{"щ", "sch"},
			{"ъ", ""},
			{"ы", "y"},
			{"ь", ""},
			{"э", "e"},
			{"ю", "yu"},
			{"я", "ya"},
			{"А", "A"},
			{"Б", "B"},
			{"В", "V"},
			{"Г", "G"},
			{"Д", "D"},
			{"Е", "E"},
			{"Ё", "YO"},
			{"Ж", "Zh"},
			{"З", "Z"},
			{"И", "I"},
			{"Й", "Y"},
			{"К", "K"},
			{"Л", "L"},
			{"М", "M"},
			{"Н", "N"},
			{"О", "O"},
			{"П", "P"},
			{"Р", "R"},
			{"С", "S"},
			{"Т", "T"},
			{"У", "U"},
			{"Ф", "F"},
			{"Х", "H"},
			{"Ц", "C"},
			{"Ч", "Ch"},
			{"Ш", "Sh"},
			{"Щ", "Sch"},
			{"Ъ", ""},
			{"Ы", "Y"},
			{"Ь", ""},
			{"Э", "E"},
			{"Ю", "YU"},
			{"Я", "YA"}
		};
		#endregion

		/// <summary>
		/// Translits the filename
		/// </summary>
		/// <returns>Translitted filename</returns>
		/// <param name="filename">filename w/o extension</param>
		public static string TranslitMachine (string filename)
		{
			return TextUtils.Translit(filename, ruTranslitMachine);
		}

		private static bool CopyDirectory (string SourcePath, string DestinationPath, bool overwriteexisting)
		{
			bool ret = false;
			try
			{
				SourcePath = SourcePath.EndsWith (@"\") ? SourcePath : SourcePath + @"\";
				DestinationPath = DestinationPath.EndsWith (@"\") ? DestinationPath : DestinationPath + @"\";
		    
				if (Directory.Exists (SourcePath))
				{
					if (!Directory.Exists (DestinationPath))
						Directory.CreateDirectory (DestinationPath);
		    
					foreach (var fls in Directory.GetFiles (SourcePath))
					{
						FileInfo flinfo = new FileInfo (fls);
						flinfo.CopyTo (DestinationPath + flinfo.Name, overwriteexisting);
					}
					foreach (var drs in Directory.GetDirectories (SourcePath))
					{
						DirectoryInfo drinfo = new DirectoryInfo (drs);
						if (!CopyDirectory (drs, DestinationPath + drinfo.Name, overwriteexisting))
							ret = false;
					}
				}
				ret = true;
			}
			catch 
			{
				ret = false;
			}
			return ret;
		}  
		
		// CHECK: need testing
		public static void CopyDirectory (string source, string target)
		{
			CopyDirectoryWave (new DirectoryInfo (source), new DirectoryInfo (target));
		}
		
		// CHECK: need testing
		private static void CopyDirectoryWave (DirectoryInfo source, DirectoryInfo target)
		{
			foreach (DirectoryInfo dir in source.GetDirectories ())
				CopyDirectoryWave (dir, target.CreateSubdirectory (dir.Name));
			
			foreach (FileInfo file in source.GetFiles ())
				file.CopyTo (Path.Combine (target.FullName, file.Name), true);
		}

	} // class
} // namespace

