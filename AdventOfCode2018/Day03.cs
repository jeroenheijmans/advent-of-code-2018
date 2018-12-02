using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day03
    {
        public const string puzzleInput = "";

        [Theory]
        [InlineData("1", 0L, "")]
        [InlineData("Actual", 0L, puzzleInput)]
        public void Test_Solve1(string nr, long expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve1(input));
        }

        [Theory]
        [InlineData("1", 0L, "")]
        [InlineData("Actual", 0L, puzzleInput)]
        public void Test_Solve2(string nr, long expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve2(input));
        }

        public long Solve1(string input)
        {
            var data = input.Split(",");

            var result = 0;

            return result;
        }

        public long Solve2(string input)
        {
            var data = input.Split(",");

            var result = 0;

            return result;
        }
    }
}
