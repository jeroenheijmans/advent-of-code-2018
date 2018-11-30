using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2018
{
    public class Day01
    {
        [Theory]
        [InlineData(@"", "0")]
        public void Test_Solve1(string input, object expected)
        {
            Assert.Equal(expected, Solve1(input));
        }

        [Theory]
        [InlineData(@"", "0")]
        public void Test_Solve2(string input, object expected)
        {
            Assert.Equal(expected, Solve2(input));
        }

        public object Solve1(string input)
        {


            return 0;
        }

        public object Solve2(string input)
        {


            return 0;
        }
    }
}
