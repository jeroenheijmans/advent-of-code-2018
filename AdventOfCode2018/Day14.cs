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
        [Fact] public void Solution_1_test_real_input() => Assert.Equal("5371393113", Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example_1() => Assert.Equal(9, Solve2("51589"));
        [Fact] public void Solution_2_test_example_2() => Assert.Equal(5, Solve2("01245"));
        [Fact] public void Solution_2_test_example_3() => Assert.Equal(18, Solve2("92510"));
        [Fact] public void Solution_2_test_example_4() => Assert.Equal(2018, Solve2("59414"));
        [Fact] public void Solution_2_my_example_1() => Assert.Equal(6, Solve2("124"));
        [Fact] public void Solution_2_my_example_2() => Assert.Equal(13, Solve2("916779"));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(20286858, Solve2(puzzleInput.ToString()));

        public string Solve1(long input)
        {
            var board = new List<int> { 3, 7 };
            int elf1 = 0, elf2 = 1;
            int toSkip = 0;

            for (long i = 0; i < input + 10; i++)
            {
                var fresh = board[elf1] + board[elf2];

                if (toSkip == 0 && board.Count() >= input) toSkip = board.Count();
                if (fresh > 9) board.Add(1);
                if (toSkip == 0 && board.Count() >= input) toSkip = board.Count();
                board.Add(fresh % 10);

                elf1 = (elf1 + board[elf1] + 1) % board.Count();
                elf2 = (elf2 + board[elf2] + 1) % board.Count();
            }

            return board.Skip(toSkip).Take(10).Select(x => x.ToString()).JoinAsStrings();
        }

        public int Solve2(string input)
        {
            int len = input.Length;
            int[] digits = input.Select(x => int.Parse(x.ToString())).ToArray();

            var board = new List<int> { 3, 7 };
            int elf1 = 0, elf2 = 1;

            for (long i = 0; i < 1_000_000_000; i++)
            {
                var fresh = board[elf1] + board[elf2];

                if (fresh > 9) board.Add(1);

                var boardLength2 = board.Count();
                var offset2 = boardLength2 - len;
                for (int n = len - 1; n >= 0; n--)
                {
                    if (board[n + offset2] != digits[n]) break;
                    if (n == 0) return offset2;
                }

                board.Add(fresh % 10);

                var boardLength = board.Count();
                var offset = boardLength - len;
                for (int n = len - 1; n >= 0; n--)
                {
                    if (board[n + offset] != digits[n]) break;
                    if (n == 0) return offset;
                }

                elf1 = (elf1 + board[elf1] + 1) % boardLength;
                elf2 = (elf2 + board[elf2] + 1) % boardLength;
            }

            throw new Exception("Not found");
        }
    }
}
