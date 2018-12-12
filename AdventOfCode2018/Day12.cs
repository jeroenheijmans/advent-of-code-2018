using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day12
    {
        private readonly ITestOutputHelper output;

        public Day12(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string testInput = @"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #";
        public const string puzzleInput = @"initial state: ..#..###...#####.#.#...####.#..####..###.##.#.#.##.#....#....#.####...#....###.###..##.#....#######

..### => .
.##.# => #
#..#. => .
#.#.# => #
###.. => #
.#..# => .
##..# => #
.###. => #
..#.. => .
..... => .
##### => .
.#... => #
...#. => #
#...# => #
####. => .
.#### => .
##.## => #
...## => .
..##. => .
#.##. => .
#.... => .
.#.#. => .
..#.# => #
#.#.. => #
##... => #
##.#. => .
#..## => .
.##.. => .
#.### => .
....# => .
.#.## => #
###.# => #";

        [Fact] public void Solution_1_test_example() => Assert.Equal(325, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(2767, Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example() => Assert.Equal(0, Solve2(testInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(0, Solve2(puzzleInput));
        
        public int Solve1(string input)
        {
            var data = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            var state = data
                .First()
                .Replace("initial state: ", "")
                .Select(c => c == '#')
                .ToList();

            var rules = data
                .Skip(2)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Replace(" => ", ""))
                .Select(r => new Rule
                {
                    Pattern = r.Take(5).Select(c => c == '#').ToArray(),
                    Target = r.Last() == '#'
                })
                .ToArray();

            var potZeroIndex = 0;

            for (int n = 0; n < 20; n++)
            {
                // output.WriteLine(String.Join("", state.Select(b => b ? '#' : '.')));

                var newState = new List<bool>();

                var matchesLeftLeft = rules.SingleOrDefault(r => r.Matches(state, -2))?.Target ?? false;
                var matchesLeft = rules.SingleOrDefault(r => r.Matches(state, -1))?.Target ?? false;
                if (matchesLeftLeft)
                {
                    potZeroIndex += 2;
                    newState.Add(true);
                    newState.Add(matchesLeft);
                }
                else if (matchesLeft)
                {
                    potZeroIndex += 1;
                    newState.Add(true);
                }

                for (int i = 0; i < state.Count(); i++)
                {
                    var target = rules.SingleOrDefault(r => r.Matches(state, i))?.Target ?? false;
                    newState.Add(target);
                }

                if (rules.SingleOrDefault(r => r.Matches(state, state.Count() + 0))?.Target ?? false) newState.Add(true);
                if (rules.SingleOrDefault(r => r.Matches(state, state.Count() + 1))?.Target ?? false) newState.Add(true);

                state = newState;
            }

            var result = 0;
            for (int n = 0; n < state.Count(); n++)
            {
                if (state[n]) result += n - potZeroIndex;
            }

            return result;
        }

        public int Solve2(string input)
        {
            var data = input.Split(" ");

            return -1;
        }

        public class Rule
        {
            public bool[] Pattern { get; set; }
            public bool Target { get; set; }

            public bool Matches(IList<bool> state, int i)
            {
                var stateLength = state.Count();

                var ll = i-2 < 0 || i-2 >= stateLength ? false : state[i-2];
                var l = i-1 < 0 || i-1 >= stateLength ? false : state[i-1];
                var c = i < 0 || i >= stateLength ? false : state[i];
                var r = i+1 < 0 || i+1 >= stateLength ? false : state[i+1];
                var rr = i+2 < 0 || i+2 >= stateLength ? false : state[i+2];

                return ll == Pattern[0]
                    && l == Pattern[1]
                    && c == Pattern[2]
                    && r == Pattern[3]
                    && rr == Pattern[4];
            }
        }

        [Fact]
        public void Rule_test_1()
        {
            var rule = new Rule { Pattern = new[] { false, false, true, false, false } };
            Assert.True(rule.Matches(new[] { true }, 0));
            Assert.False(rule.Matches(new[] { false }, 0));
        }

        [Fact]
        public void Rule_test_2()
        {
            var rule = new Rule { Pattern = new[] { true, true, true, true, true, } };
            Assert.False(rule.Matches(new[] { true, true, true, true, true, true, }, 0));
            Assert.False(rule.Matches(new[] { true, true, true, true, true, true, }, 1));
            Assert.True(rule.Matches(new[] { true, true, true, true, true, true, }, 2));
            Assert.True(rule.Matches(new[] { true, true, true, true, true, true, }, 3));
            Assert.False(rule.Matches(new[] { true, true, true, true, true, true, }, 4));
            Assert.False(rule.Matches(new[] { true, true, true, true, true, true, }, 5));
        }

        [Fact]
        public void Rule_test_3()
        {
            var rule = new Rule { Pattern = new[] { false, false, false, false, true } };
            Assert.True(rule.Matches(new[] { true }, -2));
        }
    }
}
