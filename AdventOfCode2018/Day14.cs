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
    public class Day14
    {
        private readonly ITestOutputHelper output;

        public Day14(ITestOutputHelper output)
        {
            this.output = output;
        }

        public long puzzleInput = 513401;

        [Fact] public void Solution_1_test_example_1() => Assert.Equal("5158916779", Solve1(9));
        [Fact] public void Solution_1_test_example_2() => Assert.Equal("0124515891", Solve1(5));
        [Fact] public void Solution_1_test_example_3() => Assert.Equal("9251071085", Solve1(18));
        [Fact] public void Solution_1_test_example_4() => Assert.Equal("5941429882", Solve1(2018));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal("", Solve1(puzzleInput));

        //[Fact] public void Solution_2_test_example() => Assert.Equal(0, Solve2(exampleInput));
        //[Fact] public void Solution_2_test_real_input() => Assert.Equal(0, Solve2(puzzleInput));

        public string Solve1(long input)
        {
            var board = new List<int> { 3, 7 };
            int elf1 = 0, elf2 = 1;
            int toSkip = 0;

            for (long i = 0; i < input + 10; i++)
            {
                //var line = board.Select((r, idx) => idx == elf1 ? $"({r})" : (idx == elf2 ? $"[{r}]" : $" {r} ")).JoinAsStrings();
                //output.WriteLine(line);

                var fresh = board[elf1] + board[elf2];

                if (toSkip == 0 && board.Count() >= input) toSkip = board.Count();
                if (fresh > 9) board.Add(1);
                if (toSkip == 0 && board.Count() >= input) toSkip = board.Count();
                board.Add(fresh % 10);
                if (toSkip == 0 && board.Count() >= input) toSkip = board.Count();

                elf1 = (elf1 + board[elf1] + 1) % board.Count();
                elf2 = (elf2 + board[elf2] + 1) % board.Count();
            }

            return board.Skip(toSkip).Take(10).Select(x => x.ToString()).JoinAsStrings();
        }
    }
}
