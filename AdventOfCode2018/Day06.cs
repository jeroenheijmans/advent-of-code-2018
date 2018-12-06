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
    public class Day06
    {
        public const string puzzleInput = "311,74|240,84|54,241|99,336|53,244|269,353|175,75|119,271|267,301|251,248|216,259|327,50|120,248|56,162|42,278|309,269|176,74|305,86|93,359|311,189|85,111|255,106|286,108|233,228|105,211|66,256|213,291|67,53|308,190|320,131|254,179|338,44|88,70|296,113|278,75|92,316|274,92|147,121|71,181|113,268|317,53|188,180|42,267|251,98|278,85|268,266|334,337|74,69|102,227|194,239";

        [Theory]
        [InlineData("1", 17, "1,1|1,6|8,3|3,4|5,5|8,9")]
        [InlineData("1", 5, "1,1|6,1|4,4|1,6|6,6")]
        [InlineData("Actual", -1L, puzzleInput)]
        public void Test_Solve1(string nr, int expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve1(input));
        }

        [Theory]
        [InlineData("1", -1L, "")]
        [InlineData("Actual", -1L, puzzleInput)]
        public void Test_Solve2(string nr, int expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve2(input));
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

            var innerPoints = data.Where(p =>
                p.X == minX || p.X == maxX || p.Y == minY || p.Y == maxY
            ).ToArray();

            Dictionary<Point, Point> pointOwners = GetPointOwnersBetween(data, minX, maxX, minY, maxY);
            Dictionary<Point, Point> outliers = GetPointOwnersBetween(data, 0, maxX+100, 0, maxY+100);

            // NOT: 5035 (only between min/max)
            // NOT: 5475 (just go from 0, instead of minX/maxX)

            var two = outliers
                .GroupBy(kvp => kvp.Value /*owner*/)
                .ToDictionary(x => x.Key, x => x.Count());

            var one = pointOwners
                .GroupBy(kvp => kvp.Value /*owner*/)
                .ToDictionary(x => x.Key, x => x.Count());

            return one
                .Where(grp => two[grp.Key] == grp.Value)
                .Select(grp => grp.Value)
                .Max();
        }

        private static Dictionary<Point, Point> GetPointOwnersBetween(Point[] data, int minX, int maxX, int minY, int maxY)
        {
            var pointOwners = new Dictionary<Point, Point>();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var dists = data
                        .Select(p => new KeyValuePair<Point, int>(p, Math.Abs(p.X - x) + Math.Abs(p.Y - y)))
                        .ToArray();

                    var minDist = dists.Min(kvp => kvp.Value);

                    var closestPoints = dists.Where(kvp => kvp.Value == minDist);

                    if (closestPoints.Count() == 1)
                    {
                        pointOwners.Add(new Point(x, y), closestPoints.Single().Key);
                    }
                    // else: multiple ownwers, so no ownwer
                }
            }

            return pointOwners;
        }

        public int Solve2(string input)
        {
            var data = input.Split(",");
            return 0;
        }
    }
}
