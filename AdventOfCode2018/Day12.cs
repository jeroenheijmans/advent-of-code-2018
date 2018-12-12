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
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(0, Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example() => Assert.Equal(0, Solve2(testInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(0, Solve2(puzzleInput));

        public int Solve1(string input)
        {
            var data = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            var initialState = data.First().Replace("initial state: ", "").Select(c => c == '#' ? true : false).ToArray();
            var rules = data.Skip(2).Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Replace(" => #", "").Select(c => c == '#' ? true : false).ToArray()).ToArray();

            var linkedList = new LinkedList<bool>(initialState);
            
            for (int i = 0; i < 20; i++)
            {
                if (linkedList.First.Value == false && linkedList.First.Next.Value == true) { linkedList.AddFirst(false); }
                if (linkedList.First.Value == true) { linkedList.AddFirst(false); linkedList.AddFirst(false); }

                var newList = new LinkedList<bool>(linkedList);

                foreach (var rule in rules)
                {
                    var current = linkedList.First;
                    while (current.Next != null)
                    {
                        if (
                            (current.Previous?.Previous?.Value ?? false)== rule[0]
                            && (current.Previous?.Value ?? false) == rule[1]
                            && current.Value == rule[2]
                            && (current.Next?.Value ?? false) == rule[3]
                            && (current.Next?.Next?.Value ?? false) == rule[4]
                            )
                        {
                            newList.AddLast(!current.Value);
                        }

                        current = current.Next;
                    }
                }

                linkedList = newList;
            }

            var low = 0;
            var node = linkedList.First;
            while (node != null && !node.Value)
            {
                low++;
                node = node.Next;
            }
            node = linkedList.Last;
            var hi = 0;
            while (node != null && !node.Value)
            {
                hi++;
                node = node.Previous;
            }
            
            // Oh no I lost the pot 0 spot so this won't do
            return linkedList.Count() - hi - low;
        }

        public int Solve2(string input)
        {
            var data = input.Split(" ");

            return -1;
        }
    }
}
