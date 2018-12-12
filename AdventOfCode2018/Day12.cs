using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day12
    {
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
        public const string puzzleInput = "";

        [Fact] public void Solution_1_test_example() => Assert.Equal(325, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(-1, Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example() => Assert.Equal(0, Solve2(testInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(0, Solve2(puzzleInput));

        public class Pot
        {
            public int Index { get; set; }
            public bool HasPlant { get; set; }
        }

        public class Rule
        {
            public bool[] Pattern { get; set; }
            public bool Target { get; set; }

            public bool Matches(LinkedListNode<Pot> current)
            {
                return
                    (current.Previous?.Previous?.Value?.HasPlant ?? false) == Pattern[0]
                    && (current.Previous?.Value?.HasPlant ?? false) == Pattern[1]
                    && current.Value.HasPlant == Pattern[2]
                    && (current.Next?.Value?.HasPlant ?? false) == Pattern[3]
                    && (current.Next?.Next?.Value?.HasPlant ?? false) == Pattern[4];
            }
        }

        [Fact]
        public void RuleTest1()
        {
            var list = new LinkedList<Pot>(new[] { new Pot { HasPlant = true } });
            var rule = new Rule { Pattern = new[] { false, false, true, false, false } };
            Assert.True(rule.Matches(list.First));
            list.First.Value.HasPlant = false;
            Assert.False(rule.Matches(list.First));
        }

        [Fact]
        public void RuleTest2()
        {
            var list = new LinkedList<Pot>(new[] { new Pot { HasPlant = true }, new Pot { HasPlant = true } });
            var rule = new Rule { Pattern = new[] { false, false, true, true, false } };
            Assert.True(rule.Matches(list.First));
            Assert.False(rule.Matches(list.Last));
        }

        [Fact]
        public void RuleTest3()
        {
            var list = new LinkedList<Pot>(new[] { new Pot { HasPlant = true }, new Pot { HasPlant = true }, new Pot { HasPlant = true } });
            var rule = new Rule { Pattern = new[] { false, true, true, true, false } };
            Assert.False(rule.Matches(list.First));
            Assert.True(rule.Matches(list.First.Next));
            Assert.False(rule.Matches(list.Last));
        }

        public int Solve1(string input)
        {
            var data = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            var initialState = data.First().Replace("initial state: ", "").Select(c => c == '#').ToArray();

            var rules = data.Skip(2).Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Replace(" => ", ""))
                .Select(r => new Rule
                {
                    Pattern = r.Take(5).Select(c => c == '#').ToArray(),
                    Target = r.Last() == '#'
                })
                .ToArray();

            var linkedList = new LinkedList<Pot>();
            for (int i = 0; i < initialState.Length; i++)
            {
                linkedList.AddLast(new Pot { Index = i, HasPlant = initialState[i] });
            }
            
            for (int i = 0; i < 20; i++)
            {
                var newList = new LinkedList<Pot>(linkedList);

                var current = linkedList.First;
                while (current.Next != null)
                {
                    var newTarget = rules.FirstOrDefault(r => r.Matches(current))?.Target ?? false;
                    newList.AddLast(new Pot { Index = current.Value.Index, HasPlant = newTarget });
                    current = current.Next;
                }

                linkedList = newList;
            }

            var result = 0;
            var node = linkedList.First;
            while (node != null)
            {
                if (node.Value.HasPlant) result += node.Value.Index;
                node = node.Next;
            }
            
            return result;
        }

        public int Solve2(string input)
        {
            var data = input.Split(" ");

            return -1;
        }
    }
}
