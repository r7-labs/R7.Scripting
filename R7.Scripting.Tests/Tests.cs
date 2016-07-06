//
//  Tests.cs
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
using NUnit.Framework;
using R7.Scripting;

namespace R7.Scripting.Tests
{
	[TestFixture()]
	public class Tests
	{
		[Test()]
		public void TranslitMachine ()
		{
			Assert.AreEqual ("Ivanov_Ivan", FileHelper.TranslitMachine("Иванов Иван"));
			Assert.AreEqual ("Kharitonov_Pyotr", FileHelper.TranslitMachine("Хaритонов Пётр"));
			Assert.AreEqual ("some_file-name", FileHelper.TranslitMachine("some__-file--_name"));
			Assert.AreEqual ("trimmed", FileHelper.TranslitMachine("__-_trimmed--_-"));
			Assert.AreEqual ("Novy", FileHelper.TranslitMachine("Новый"));
		}

		[Test()]
		public void Damerau ()
		{
			// TODO: must use some well-known samples here
			Assert.AreEqual (0, TextUtils.DamerauLevenshteinDistance("", ""));
			Assert.AreEqual (0, TextUtils.DamerauLevenshteinDistance("mandragora", "mandragora"));
			Assert.AreEqual (3, TextUtils.DamerauLevenshteinDistance("cat", ""));
			Assert.AreEqual (6, TextUtils.DamerauLevenshteinDistance("", "hotdog"));
			Assert.AreEqual (1, TextUtils.DamerauLevenshteinDistance("man", "men"));
			// letter interchange counted as +2?
			Assert.AreEqual (2, TextUtils.DamerauLevenshteinDistance("carbonium", "cabronium"));
		}
	}
}

