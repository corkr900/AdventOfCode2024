using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days._03 {
	internal class Day03 {
		private const string inputFile = "Input/03a.txt";
		//private const string inputFile = "Input/example.txt";

		public static void Do() {
			// Load input into a variable
			string input = File.ReadAllText(inputFile);

			// Do a regex match for valid instructions
			Regex regex_any = new Regex("(mul\\([0-9]+,[0-9]+\\))|(do\\(\\))|(don't\\(\\))");

			// Match on any
			MatchCollection matches = regex_any.Matches(input);

			bool enabled = true;
			long sum = 0;
			// For each match...
			foreach (Match match in matches) {
				string val = match.Value;
				// Determine which one it matched
				// Handle do/don't
				if (val == "do()") {
					enabled = true;
                    continue;
				}
				if (val == "don't()") {
					enabled = false;
					continue;
				}
				if (!enabled) {
                    continue;
				}
				// Do the parsing and multiplication if necessary
				val = match.Value.Trim('m', 'u', 'l', '(', ')');  // -> 123,456
				string[] split = val.Split(',');  // guaranteed to have 2 elements
				int res = int.Parse(split[0]) * int.Parse(split[1]);
				sum += res;
			}

			// Output result
			Console.WriteLine($"Sum is: {sum}");
        }
	}
}
