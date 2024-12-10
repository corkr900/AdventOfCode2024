using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days._04 {
	internal class Day04 {

		private static string inputFile = "Input/04.txt";
		private static string query = "XMAS";

		private static int[][] directions = [
			[ 1, 0 ],  // right
			[ 1, 1 ],  // downright
			[ 0, 1 ],  // down
			[ -1, 1 ],  // downleft
			[ -1, 0 ],  // left
			[ -1, -1 ],  // upleft
			[ 0, -1 ],  // up
			[ 1, -1 ],  // upright
		];

		public static void Do() {
			// Read the file
			string[] lines = File.ReadAllText(inputFile).Trim().Split('\n');
			// Translate into a 2D array of chars
			char[][] input = lines.Select(str => str.ToCharArray()).ToArray();

			//for (int x = 0; x < input.Length; x++) {
			//	for (int y = 0; y < input[x].Length; y++) {
			//		Console.Write(input[x][y]);
			//	}
			//	Console.WriteLine();
			//}

			// Search for every instance of the FIRST letter... and for each one:
			int sum = 0;
			for (int x = 0; x < input.Length; x++) {
				for (int y = 0; y < input[x].Length; y++ ) {
					// Call a function to count any matches, and add to total
					sum += Match(input, x, y);
				}
			}

            Console.WriteLine($"The number of matches is: {sum}");
        }

		public static int Match(char[][] input, int x, int y) {
			if (input[x][y] != query[0]) return 0;
			int matches = 0;
			// For each of the 8 directions...
			foreach (int[] dir in directions) {
				// Check if it's a match and add 1 to result
				if (MatchDir(input, x, y, dir)) {
					++matches;
				}
			}

			// This will return 0-8 depending on how many matches start at that position.
			return matches;
		}

		public static bool MatchDir(char[][] input, int x, int y, int[] dir) {
			// Check bounds
			int xEnd = x + dir[0] * (query.Length - 1);
			int yEnd = y + dir[1] * (query.Length - 1);
			if (xEnd < 0 || xEnd >= input.Length || yEnd < 0 || yEnd > input[x].Length) {
				return false;
			}

			// iterate over the characters and check they all match
			for (int i = 1; i < query.Length; i++) {
				if (input[x + i * dir[0]][y + i * dir[1]] != query[i]) {
					return false;
				}
			}

            return true;
		}

	}
}
