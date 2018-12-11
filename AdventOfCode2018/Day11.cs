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

        [Fact] public void Solution_1_test_example_1() => Assert.Equal(new Point(33, 45), Solve1(18));
        [Fact] public void Solution_1_test_example_2() => Assert.Equal(new Point(21,61), Solve1(42));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(new Point(20,34), Solve1(puzzleInput));

        //[Fact] public void Solution_2_test_example() => Assert.Equal("", Solve2(0));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal("", Solve2(puzzleInput));

        public Point Solve1(long serialNr)
        {
            int size = 3;

            Dictionary<Point, long> energyLevels = GetEnergyLevelsGrid(serialNr);

            KeyValuePair<Point, long> getLargestSquareWithValue = GetLargestSquareWithValue(size, energyLevels);

            return getLargestSquareWithValue.Key;
        }

        public string Solve2(long serialNr)
        {
            Dictionary<Point, long> energyLevels = GetEnergyLevelsGrid(serialNr);

            int largestSize = 3;
            long largestSum = long.MinValue;
            Point origin = new Point(0, 0);

            for (int size = 299; size > 150; size--)
            {
                KeyValuePair<Point, long> target = GetLargestSquareWithValue(size, energyLevels);

                if (target.Value > largestSum)
                {
                    largestSum = target.Value;
                    largestSize = size;
                    origin = target.Key;
                }
            }

            // Not 1,1,299
            return $"{origin},{largestSize}";
        }

        private static KeyValuePair<Point, long> GetLargestSquareWithValue(int size, Dictionary<Point, long> energyLevels)
        {
            long largest = long.MinValue;
            Point origin = new Point(0, 0);

            for (int y = 1; y <= gridSize - size; y++)
            {
                for (int x = 1; x <= gridSize - size; x++)
                {
                    long newSum = 0;

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            newSum += energyLevels[new Point(x + i, y + j)];
                        }
                    }

                    if (newSum > largest)
                    {
                        origin = new Point(x, y);
                        largest = newSum;
                    }
                }
            }

            return new KeyValuePair<Point, long>(origin, largest);
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
            return ((((rackId * y + serialNr) * rackId) / 100) % 10) - 5;
        }
    }
}
