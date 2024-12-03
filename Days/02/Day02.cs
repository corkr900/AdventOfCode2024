using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days._02 {
	internal class Day02 {

		private static string inputFile = "Input/02a.txt";

		public static void Do() {
			int safe = 0;
			// loop over each report (line)
			foreach (string line in File.ReadLines(inputFile)) {
				// for each report, check if safe
				if (IsSafe(line)) safe++;
			}
            Console.WriteLine($"Number of safe reports: {safe}");
        }

		private static bool IsSafe(string report) {
			//Console.WriteLine(report);
            string[] chunk = report.Split(' ');
			int[] nums = chunk.Select(x => int.Parse(x)).ToArray();
			return IsSafe(nums, true);
		}

		private static bool IsSafe(int[] nums, bool allowRemoving) {
			//Console.WriteLine($"EVALUATING: {nums.Aggregate("", (str, val) => str + val.ToString() + ", ")}");

			// First: check whether it's + or -
			int sign = Math.Sign(nums[1] - nums[0]);  // +1 if increasing or -1 if decreasing. 0 if they match.
			if (sign == 0) return TryRemove(nums, allowRemoving, 1);

			// loop over elements..
			for (int i = 1; i < nums.Length; i++) {
				// make sure it matches directions
				// make sure it's not too big a jump
				int step = sign * (nums[i] - nums[i - 1]);
				if (step < 1 || step > 3) return TryRemove(nums, allowRemoving, i);
			}

			return true;
		}

		private static bool TryRemove(IEnumerable<int> nums, bool allowRemoving, int upperpos) {
			// if can't recur, return false;
			if (!allowRemoving) return false;
			

			// 30 32 31 30    x
			// 30 32    30    x
			// 30    31 30    x
			//    32 31 30    correct




			// otherwise, try removing an element and reevaluate
			List<int> list = nums.ToList();
			list.RemoveAt(upperpos);
			if (IsSafe(list.ToArray(), false)) {
                //Console.WriteLine($"A ({upperpos}): {nums.Aggregate("", (str, val) => str + val.ToString() + ", ")}");
                return true;
			}

			list = nums.ToList();
			list.RemoveAt(upperpos - 1);
			if (IsSafe(list.ToArray(), false)) {
				//Console.WriteLine($"B ({upperpos}): {nums.Aggregate("", (str, val) => str + val.ToString() + ", ")}");
				return true;
			}

			if (upperpos > 1) {
				list = nums.ToList();
				list.RemoveAt(0);
				if (IsSafe(list.ToArray(), false)) {
					//Console.WriteLine($"B ({upperpos}): {nums.Aggregate("", (str, val) => str + val.ToString() + ", ")}");
					return true;
				}
			}

			if (upperpos == 2) Console.WriteLine($"FAILED: ({upperpos}): {nums.Aggregate("", (str, val) => str + val.ToString() + ", ")}");
			return false;
		}
	}
}
