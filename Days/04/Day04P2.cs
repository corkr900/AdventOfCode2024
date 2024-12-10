using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days._04 {
	internal class Day04P2 {

		private static string inputFile = "Input/04.txt";

		public static void Do() {
			// Read the file
			string[] lines = File.ReadAllText(inputFile).Trim().Split('\n');
			// Translate into a 2D array of chars
			char[][] input = lines.Select(str => str.Trim().ToCharArray()).ToArray();

			// Search for every instance of the FIRST letter... and for each one:
			int sum = 0;
			for (int x = 1; x < input.Length - 1; x++) {
				for (int y = 1; y < input[x].Length - 1; y++) {
					// Call a function to count any matches, and add to total
					if (Match(input, x, y)) ++sum;
				}
			}

			Console.WriteLine($"The number of matches is: {sum}");
		}

		public static bool Match(char[][] input, int x, int y) {
			if (input[x][y] != 'A') return false;
			char[] arr = [input[x - 1][y - 1], input[x + 1][y + 1], input[x - 1][y + 1], input[x + 1][y - 1]];
			string composed = new(arr);
			bool ret = Regex.Match(composed, "((MS)|(SM))((MS)|(SM))").Success;
			if (ret) {
                //Console.WriteLine($"Matched '{composed}' at {x}, {y}");
            }
			return ret;
		}
	}
}
