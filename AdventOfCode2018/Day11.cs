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
    public class Day11
    {
        public const long puzzleInput = 6878;

        public const int gridSize = 300;

        [Fact] public void Solution_1_test_example_1() => Assert.Equal(29, Solve1(18));
        [Fact] public void Solution_1_test_example_2() => Assert.Equal(30, Solve1(42));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(0, Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example() => Assert.Equal(0, Solve2(0));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(0, Solve2(0));

        public long Solve1(long serialNr)
        {
            Dictionary<Point, long> energyLevels = GetEnergyLevelsGrid(serialNr);

            var sums = new Dictionary<Point, long>();

            for (int y = 1; y <= gridSize - 3; y++)
            {
                for (int x = 1; x <= gridSize - 3; x++)
                {
                    long newSum = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            newSum += energyLevels[new Point(x + i, y + j)];
                        }
                    }

                    sums.Add(new Point(x, y), newSum);
                }
            }

            // Not 30
            return sums.Select(x => x.Value).Max();
        }

        private static Dictionary<Point, long> GetEnergyLevelsGrid(long serialNr)
        {
            var energyLevels = new Dictionary<Point, long>();

            for (int y = 1; y <= gridSize; y++)
            {
                for (int x = 1; x <= gridSize; x++)
                {
                    long level = GetLevelFor(serialNr, y, x);

                    energyLevels.Add(new Point(x, y), level);
                }
            }

            return energyLevels;
        }

        [Fact] public void GetLevelFor_given_example1_should_return_result() => Assert.Equal(4, GetLevelFor(8, y: 5, x: 3));
        [Fact] public void GetLevelFor_given_example2_should_return_result() => Assert.Equal(-5, GetLevelFor(57, y: 79, x: 122));
        [Fact] public void GetLevelFor_given_example3_should_return_result() => Assert.Equal(0, GetLevelFor(39, y: 196, x: 217));
        [Fact] public void GetLevelFor_given_example4_should_return_result() => Assert.Equal(4, GetLevelFor(71, y: 153, x: 101));

        private static long GetLevelFor(long serialNr, int y, int x)
        {
            long rackId = x + 10;
            long levelStart = rackId * y;
            long withSerial = levelStart + serialNr;
            long multiplied = withSerial * rackId;
            long hundreth = (multiplied / 100) % 10;
            long level = hundreth - 5;

            return level;
        }

        public long Solve2(long serialNr)
        {

            return -1;
        }
    }
}
