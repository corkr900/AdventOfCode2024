using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days._01 {
	internal class Day01 {
		public static void Do() {
			const string inputFile = "Input/01a.txt";

			// Load data
			List<int> left = new();
			List<int> right = new();
			foreach (string line in File.ReadLines(inputFile)) {
				if (string.IsNullOrEmpty(line)) continue;
				string[] chunks = line.Split(' ');
				if (!int.TryParse(chunks[0], out int l)) continue;
				for (int i = 1; i < chunks.Length; i++) {
					if (string.IsNullOrEmpty(chunks[i])) continue;
					if (!int.TryParse(chunks[i], out int r)) continue;
					left.Add(l);
					right.Add(r);
					break;
				}
			}

			// sort it
			left.Sort();
			right.Sort();

			// sum up differences
			long sum = 0;
			long similarity = 0;
			for (int i = 0;  i < Math.Min(left.Count, right.Count); i++) {
				sum += Math.Abs(left[i] - right[i]);
				int numOccur = right.FindAll(x => x == left[i]).Count();
				long additionalScore = left[i] * numOccur;
				similarity += additionalScore;
			}

			Console.WriteLine($"Sum is: {sum}\nSimilarity is {similarity}");
		}
	}
}
