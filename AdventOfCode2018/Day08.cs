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
    public class Day08
    {
        public const string puzzleInput = "";

        [Theory]
        [InlineData(0L, "")]
        [InlineData(-1L, puzzleInput)]
        public void Test_Solve1(long expected, string input)
        {
            Assert.Equal(expected, Solve1(input));
        }

        [Theory]
        [InlineData(0L, "")]
        [InlineData(-1L, puzzleInput)]
        public void Test_Solve2(long expected, string input)
        {
            Assert.Equal(expected, Solve2(input));
        }

        public long Solve1(string input)
        {
            var data = input.Split(",");
            return 0;
        }

        public long Solve2(string input)
        {
            var data = input.Split(",");
            return 0;
        }
    }
}
