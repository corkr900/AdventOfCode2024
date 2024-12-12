using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days._06 {
	internal class Day06 {

		private static string inputFile = "Input/06.txt";

		public static Location[,] map = null;

		public static void Do() {
			// Load initial state
			map = Load(out Position initialPosition, out Direction initialDirection);
			initialPosition.In(map).Visit(initialDirection);


			int viableLocations = 0;
			// for each position ...
			for (int x = 0; x < map.GetLength(0); x++) {
				for (int y = 0 ; y < map.GetLength(1); y++) {
					Location loc = map[x, y];
					// is valid?
					if (loc.Obstructed || (initialPosition.X == x && initialPosition.Y == y)) continue;

					// place an obstacle
					loc.Obstructed = true;

					// Do the evaluation
					Position pos = initialPosition;
					Direction direction = initialDirection;
					StepResult res;
					ResetMap();
					while ((res = Step(ref pos, ref direction)) == StepResult.Normal) { }  // position logging happens in step so nothing to do in loop other than repeat the check

					// Undo the new obstruction
					loc.Obstructed = false;

					// Check exit condition for loop and add to count
					if (res == StepResult.Loop) {
						viableLocations++;
                    }
				}
			}

			// Count up the visited spots
			long sum = 0;
			foreach (Location item in map) {
				if (item.Visited) sum++;
			}
            Console.WriteLine($"Spots visited: {sum}. Viable locations for the obstacle: {viableLocations}");
        }

		private static void ResetMap() {
			foreach (Location item in map) {
				item.VisitedWithDirection.Clear();
			}
		}

		public static Location[,] Load(out Position initPos, out Direction initDir) {
			string[] lines = File.ReadAllText(inputFile).Trim().Split('\n').Select(s => s.Trim()).ToArray();
			int numLines = lines.Count();
			int width = lines[0].Length;

			Location[,] ret = new Location[width, numLines];

			initPos = new() { X = 0, Y = 0 };
			initDir = Direction.Up;

			for (int y = 0; y < lines.Length; y++) {
				for (int x = 0; x < lines[y].Length; x++) {
					char c = lines[y].ElementAt(x);
					ret[x, y] = new(c);
					Direction? dir = c switch {
						'^' => Direction.Up,
						'V' => Direction.Down,
						'<' => Direction.Left,
						'>' => Direction.Right,
						_ => null,
					};
					if (dir != null) {
						initDir = dir.Value;
						initPos = new() { X = x, Y = y };
					}
				}
			}

			return ret;
		}

		public static StepResult Step(ref Position pos, ref Direction dir) {
			Location next = null;
			Position nxtPos = new();
			for (int i = 0; i < 4; i++) {  // limit iterations as safeguard
				nxtPos = pos.Next(dir);
				next = nxtPos.In(map);
				if (next == null) return StepResult.LeaveArea;  // Outside map
				if (next.IsPreviouslyVisited(dir)) return StepResult.Loop;  // Completed a loop
				if (!next.Obstructed) continue;  // moving this way is valid
				dir = Rotate(dir);
				pos.In(map).Visit(dir);  // Add every step of the rotation to catch as many cases as possible
			}
			if (next.Obstructed) return StepResult.Loop;  // sanity check

			next.Visit(dir);
			pos = nxtPos;
			return StepResult.Normal;
		}

		public static Direction Rotate(Direction d) {
			return d switch {
				Direction.Up => Direction.Right,
				Direction.Right => Direction.Down,
				Direction.Down => Direction.Left,
				Direction.Left => Direction.Up,
				_ => Direction.Up
			};
		}

	}

	public struct Position {
		public int X, Y;

		public Position Next(Direction dir) {
			return dir switch {
				Direction.Up => new() { X = this.X, Y = this.Y - 1 },
				Direction.Down => new() { X = this.X, Y = this.Y + 1 },
				Direction.Left => new() { X = this.X - 1, Y = this.Y },
				Direction.Right => new() { X = this.X + 1, Y = this.Y },
				_ => new() { X = this.X, Y = this.Y }
			};
		}

		public Location In(Location[,] map) {
			if (map == null || X < 0 || Y < 0 || X >= map.GetLength(0) || Y >= map.GetLength(1)) return null;  // Outside map
			return map[X, Y];
		}
	}

	public class Location {
		public bool Obstructed;
		public List<Direction> VisitedWithDirection;
		public bool Visited => VisitedWithDirection?.Count > 0;

		public Location(char c) {
			Obstructed = c == '#';
			VisitedWithDirection = new();
		}

		internal void Visit(Direction dir) {
			if (!IsPreviouslyVisited(dir)) VisitedWithDirection.Add(dir);
		}

		public bool IsPreviouslyVisited(Direction dir) => VisitedWithDirection.Contains(dir);
	}

	public enum Direction {
		Up, Down, Left, Right
	}

	public enum StepResult {
		Loop,
		LeaveArea,
		Normal,
	}

}
