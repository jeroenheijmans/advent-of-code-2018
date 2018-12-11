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

        [Fact] public void Solution_1_test_example_1() => Assert.Equal("33,45", Solve1(18));
        [Fact] public void Solution_1_test_example_2() => Assert.Equal("21,61", Solve1(42));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal("20,34", Solve1(puzzleInput));
        
        [Fact] public void Solution_2_test_real_input() => Assert.Equal("90,57,15", Solve2(puzzleInput));

        public string Solve1(long serialNr)
        {
            var levels = GetEnergyLevelsGrid(serialNr);
            var (targetPoint, _) = GetLargestSquareWithValue(levels, size: 3);
            return $"{targetPoint.X},{targetPoint.Y}";
        }

        public string Solve2(long serialNr)
        {
            var energyLevels = GetEnergyLevelsGrid(serialNr);

            int largestSize = 3;
            long largestSum = long.MinValue;
            var origin = new Point(0, 0);

            for (int size = 3; size < 30; size++) // Guessing size no larger than this... :'(
            {
                var (targetPoint, targetValue) = GetLargestSquareWithValue(energyLevels, size);

                if (targetValue > largestSum)
                {
                    largestSum = targetValue;
                    largestSize = size;
                    origin = targetPoint;
                }
            }
            
            return $"{origin.X},{origin.Y},{largestSize}";
        }

        private static (Point, long) GetLargestSquareWithValue(long[,] energyLevels, int size)
        {
            long largest = long.MinValue;
            Point origin = new Point(0, 0);

            for (int y = 0; y < gridSize - size; y++)
            {
                for (int x = 0; x < gridSize - size; x++)
                {
                    long newSum = 0;

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            newSum += energyLevels[x + i, y + j];
                        }
                    }

                    if (newSum > largest)
                    {
                        origin = new Point(x + 1, y + 1); // +1 as answers should be 1-based
                        largest = newSum;
                    }

                    // TODO: Apply same trick for moving down a notch
                    while (x++ < gridSize - size)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            newSum -= energyLevels[x - 1, y + j]; // Take off old left side
                            newSum += energyLevels[x + size - 1, y + j]; // Add new row on the right
                        }

                        if (newSum > largest)
                        {
                            origin = new Point(x + 1, y + 1); // +1 as answers should be 1-based
                            largest = newSum;
                        }
                    }
                }
            }

            return (origin, largest);
        }

        private static long[,] GetEnergyLevelsGrid(long serialNr)
        {
            var rows = new long[gridSize, gridSize];

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    rows[x, y] = GetLevelFor(serialNr, x + 1, y + 1); // +1 as answers should be 1-based
                }
            }

            return rows;
        }

        [Fact] public void GetLevelFor_given_example1_should_return_result() => Assert.Equal(4, GetLevelFor(8, x: 3, y: 5));
        [Fact] public void GetLevelFor_given_example2_should_return_result() => Assert.Equal(-5, GetLevelFor(57, x: 122, y: 79));
        [Fact] public void GetLevelFor_given_example3_should_return_result() => Assert.Equal(0, GetLevelFor(39, x: 217, y: 196));
        [Fact] public void GetLevelFor_given_example4_should_return_result() => Assert.Equal(4, GetLevelFor(71, x: 101, y: 153));

        private static long GetLevelFor(long serialNr, int x, int y)
        {
            long rackId = x + 10;
            long multiple = (rackId * y + serialNr) * rackId;
            long hundredth = (multiple / 100) % 10;
            return hundredth - 5;
        }
    }
}
