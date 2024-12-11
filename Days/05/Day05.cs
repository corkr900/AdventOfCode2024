using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days._05 {
	internal class Day05 {

		private static string inputFile = "Input/05.txt";

		private static readonly List<Rule> rules = [];
		private static readonly List<Update> updates = [];

		public static void Do() {
			// Load input
			Load();

			long correctSum = 0, incorrectSum = 0;
			// For each update...
			foreach (Update update in updates) {
				// Check if correct
				if (IsCorrect(update)) {
					// add to the sum
					correctSum += update.MiddleValue;
				}
				else {
                    update.Fix(rules);
                    incorrectSum += update.MiddleValue;
				}
			}

			// Output
			//foreach (var rule in rules) {
			//             Console.WriteLine($"Rule: {rule.First} <> {rule.Second}");
			//         }
			//foreach (var update in updates) {
			//	Console.WriteLine($"Update: {update.DisplayVal}");
			//}
			Console.WriteLine($"Correct sum is: {correctSum}\nIncorrect sum is {incorrectSum}");
		}

		private static void Load() {
			rules.Clear();
			updates.Clear();

			foreach (string line in File.ReadLines(inputFile)) {
				string txt = line.Trim();
				if (string.IsNullOrEmpty(txt)) continue;  // empty line
				if (Regex.Match(txt, "\\d+\\|\\d+").Success) {
					// Is a rule
					int[] nums = txt.Split('|').Select(int.Parse).ToArray();
					rules.Add(new() { First = nums[0], Second = nums[1] });
				}
				else {
					// Assume is a update
					updates.Add(new Update() {
						Pages = new(txt.Split(',').Select(int.Parse))
					});
				}
			}
		}

		private static bool IsCorrect(Update update) {
			foreach (Rule rule in rules) {
				if (!update.Satisfies(rule)) return false;
			}
			return true;
		}

	}

	internal class Rule {
		public int First;
		public int Second;
	}

	internal class Update {
		public List<int> Pages = [];

		internal string DisplayVal => Pages.Aggregate("", (val, itm) => val + ", " + itm.ToString());

		public long MiddleValue {
			get {
				return Pages[Pages.Count / 2];
			}
		}

		internal bool Satisfies(Rule rule) {
			int idxFirst = Pages.IndexOf(rule.First);
			int idxSecond = Pages.IndexOf(rule.Second);
			if (idxFirst == -1 || idxSecond == -1) return true;  // rule is not applicable
			else return idxFirst < idxSecond;
		}

		internal void Fix(List<Rule> rules) {
			for (int i = 0; i < Pages.Count; i++) {
				for (int j = i + 1; j < Pages.Count; j++) {
					if (NeedSwap(Pages[i], Pages[j], rules)) {
						int tmp = Pages[j];
						Pages[j] = Pages[i];
						Pages[i] = tmp;
					}
				}
			}
		}

		private bool NeedSwap(int first, int second, List<Rule> rules) {
			for (int i = 0; i < rules.Count; i++) {
				if (first == rules[i].First && second == rules[i].Second) return false;
				else if (first == rules[i].Second && second == rules[i].First) return true;
			}
			return false;  // No rule
		}

	}
}
