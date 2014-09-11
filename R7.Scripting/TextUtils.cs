//
//  TextUtils.cs
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
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace R7.Utils
{
	public class TextUtils
	{
		public static string Translit (string s, string[,] translitTable)
		{
			// TODO: Use contracts
			if (translitTable.GetLength(1) != 2) 
				throw new Exception("Transliteration table must be [x,2] array");
			
			for (var i = 0; i < translitTable.GetLength(0); i++)
				s = Regex.Replace(s, translitTable[i,0], translitTable[i,1]);
			
			return s;
		}

		public static int LevenshteinDistance (string s, string t)
		{
			// border cases
			if (string.IsNullOrEmpty (s))
			{
				if (string.IsNullOrEmpty (t))
					return 0;
				return t.Length;
			}

			if (string.IsNullOrEmpty (t))
				return s.Length;

			var diff = 0;                       

			// create matrix
			var m = new int[s.Length + 1, t.Length + 1];

			// fill matrix
			for (var i=0; i <= s.Length; i++) m [i, 0] = i;
			for (var j=0; j <= t.Length; j++) m [0, j] = j;

			// main cycle
			for (var i=1; i<=s.Length; i++)
				for (var j=1; j<=t.Length; j++)
				{
					diff = (s [i - 1] == t [j - 1]) ? 0 : 1;
					m [i, j] = Math.Min (
						Math.Min (m [i - 1, j] + 1, m [i, j - 1] + 1), m [i - 1, j - 1] + diff);
				}
			
			return m [s.Length, t.Length];                
		}

		// CHECK: Need testing!
		public static int DamerauLevenshteinDistance (string s, string t)
		{
			// border cases
			if (string.IsNullOrEmpty (s))
			{
				if (string.IsNullOrEmpty (t))
					return 0;
				return t.Length;
			}
		
			if (string.IsNullOrEmpty (t))
				return s.Length;

			// create matrix
			var d = new int[s.Length + 2, t.Length + 2];

			// fill matrix
			d [0, 0] = int.MaxValue;
			for (var i = 0; i <= s.Length; i++)
			{
				d [i + 1, 1] = i;
				d [i + 1, 0] = int.MaxValue;
			}
			
			for (var j = 0; j <= t.Length; j++)
			{
				d[1, j + 1] = j;
				d[0, j + 1] = int.MaxValue;
			}

			// create dictionary from s and t chars
			var lastPosition = new Dictionary<char,int>();
			foreach (var letter in (s + t))
				if (!lastPosition.ContainsKey(letter))
					lastPosition.Add (letter, 0);

			// main cycle
			for (var i = 1; i <= s.Length; i++)
			{
				var last = 0;

				for (var j = 1; j <= t.Length; j++)
				{ 
					if (s[i-1] == t[j-1])
					{
						d[i + 1, j + 1] = d[i, j];
						last = j;
					}
					else
					{	
						var i2 = lastPosition[t[j-1]];
						var j2 = last;

						d[i + 1, j + 1] = 1 + Math.Min (Math.Min( d[i, j], d[i + 1, j]), d[i, j + 1]);
						d[i + 1, j + 1] = Math.Min (d[i + 1, j + 1], d[i2 + 1, j2 + 1] + (i - i2 - 1) + 1 + (j - j2 - 1));
						lastPosition[s[i-1]] = i;
					}
				}
			}
			return d[s.Length+1, t.Length+1];
		}  

		/*
		 int DamerauLevenshteinDistance(char S[1..M], char T[1..N])
   // Обработка крайних случаев
   if (S == "") then
      if (T == "") then
         return 0
      else
         return N
   else if (T == "") then
      return M
   int D[0..M + 1, 0..N + 1]          // Динамика
   int INF = M + N                    // Большая константа
    
   // База индукции
   D[0, 0] = INF;
   for i from 0 to M
      D[i + 1, 1] = i
      D[i + 1, 0] = INF
   for j from 0 to N
      D[1, j + 1] = j
      D[0, j + 1] = INF
    
   int lastPosition[0..количество различных символов в S и T]
   //для каждого элемента C алфавита задано значение lastPosition[C] 
    
   foreach (char Letter in (S + T))
      if Letter не содержится в lastPosition
         добавить Letter в lastPosition
         lastPosition[Letter] = 0
    
   for i from 1 to M
      int last = 0
      for j from 1 to N
         int i' = lastPosition[T[j]]
         int j' = last
         if S[i] == T[j] then
            D[i + 1, j + 1] = D[i, j]
            last = j
         else
            D[i + 1, j + 1] = minimum(D[i, j], D[i + 1, j], D[i, j + 1]) + 1
         D[i + 1, j + 1] = minimum(D[i + 1, j + 1], D[i' + 1, j' + 1] + (i - i' - 1) + 1 + (j - j' - 1))
      lastPosition[S[i]] = i
     
   return D[M + 1, N + 1]
		 * */


	} // class
} // namespace

