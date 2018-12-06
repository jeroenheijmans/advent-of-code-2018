﻿using System;
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
    public class Day06
    {
        public const string puzzleInput = "311,74|240,84|54,241|99,336|53,244|269,353|175,75|119,271|267,301|251,248|216,259|327,50|120,248|56,162|42,278|309,269|176,74|305,86|93,359|311,189|85,111|255,106|286,108|233,228|105,211|66,256|213,291|67,53|308,190|320,131|254,179|338,44|88,70|296,113|278,75|92,316|274,92|147,121|71,181|113,268|317,53|188,180|42,267|251,98|278,85|268,266|334,337|74,69|102,227|194,239";

        [Theory]
        [InlineData("1", 17, "1,1|1,6|8,3|3,4|5,5|8,9")]
        [InlineData("1", 5, "2,2|6,2|4,4|2,6|6,6")]
        [InlineData("1", 6, "2,2|6,2|4,4|2,6|6,7")]
        [InlineData("1", 1, "4,4|2,3|5,3|3,5|5,6")]
        [InlineData("Actual", 5035, puzzleInput)]
        public void Test_Solve1(string nr, int expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve1(input));
        }

        [Theory]
        [InlineData("1", 16, 32, "1,1|1,6|8,3|3,4|5,5|8,9")]
        [InlineData("Actual", 35294, 10000, puzzleInput)]
        public void Test_Solve2(string nr, int expected, int safety, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve2(input, safety));
        }

        public int Solve1(string input)
        {
            var data = input
                .Split("|")
                .Select(x => new Point(int.Parse(x.Split(",")[0]), int.Parse(x.Split(",")[1])))
                .ToArray();

            var minX = data.Select(p => p.X).Min();
            var maxX = data.Select(p => p.X).Max();
            var minY = data.Select(p => p.Y).Min();
            var maxY = data.Select(p => p.Y).Max();

            var pointOwners = new Dictionary<Point, Point>();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var dists = data
                        .Select(p => new KeyValuePair<Point, int>(p, Math.Abs(p.X - x) + Math.Abs(p.Y - y)))
                        .ToArray();

                    var minDist = dists.Min(kvp => kvp.Value);

                    var closestPoints = dists.Where(kvp => kvp.Value == minDist).ToArray();

                    if (closestPoints.Count() == 1)
                    {
                        pointOwners.Add(new Point(x, y), closestPoints.Single().Key);
                    }
                    // else: multiple ownwers, so no ownwer
                }
            }

            var edgeOwners = pointOwners
                .Where(p => p.Key.X == minX || p.Key.X == maxX || p.Key.Y == minY || p.Key.Y == maxY)
                .Select(po => po.Value)
                .Distinct()
                .ToArray();
            
            return pointOwners
                .Where(po => !edgeOwners.Contains(po.Value /* owner */))
                .GroupBy(po => po.Value /* owner */)
                .Select(grp => grp.Count())
                .Max();
        }

        public int Solve2(string input, int safety)
        {
            var data = input
                .Split("|")
                .Select(x => new Point(int.Parse(x.Split(",")[0]), int.Parse(x.Split(",")[1])))
                .ToArray();

            var minX = data.Select(p => p.X).Min();
            var maxX = data.Select(p => p.X).Max();
            var minY = data.Select(p => p.Y).Min();
            var maxY = data.Select(p => p.Y).Max();

            int result = 0;

            for (int x = 0; x <= maxX; x++)
            {
                for (int y = 0; y <= maxY; y++)
                {
                    var totalDistance = data
                        .Select(p => Math.Abs(p.X - x) + Math.Abs(p.Y - y))
                        .Sum();

                    if (totalDistance < safety)
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}
