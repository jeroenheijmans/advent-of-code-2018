using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2018
{
    public class Day02
    {
        public const string puzzleInput = "";

        [Theory]
        [InlineData("1", 0L, "")]
        [InlineData("2", 0L, "")]
        [InlineData("3", 0L, "")]
        [InlineData("4", 0L, "")]
        [InlineData("Actual", -1L, puzzleInput)]
        public void Test_Solve1(string nr, long expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve1(input));
        }

        [Theory]
        [InlineData("1", 0L, "")]
        [InlineData("2", 0L, "")]
        [InlineData("3", 0L, "")]
        [InlineData("4", 0L, "")]
        [InlineData("Actual", -1L, puzzleInput)]
        public void Test_Solve2(string nr, long expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve2(input));
        }

        public long Solve1(string input)
        {
            var data = input.Split(",").Select(long.Parse).ToArray();
            long result = data.Sum();

            return result;
        }

        public long Solve2(string input)
        {
            var data = input.Split(",").Select(long.Parse).ToArray();
            long result = data.Sum();

            return result;
        }
    }
}
