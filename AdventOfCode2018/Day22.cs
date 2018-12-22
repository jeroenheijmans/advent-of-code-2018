using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day22
    {
        private readonly ITestOutputHelper output;

        public Day22(ITestOutputHelper output)
        {
            this.output = output;
        }
        
        [Fact] public void Solution_1_test_example_1() => Assert.Equal(114, Solve1(510, 10, 10));

        [Fact] public void Solution_1_test_real_input() => Assert.Equal(7299, Solve1(11109, 9, 731));

        public int Solve1(int depth, int targetX, int targetY)
        {
            var riskLevel = 0;

            var types = new int[targetX + 1, targetY + 1];
            var geos = new int[targetX + 1, targetY + 1];
            var erosions = new int[targetX + 1, targetY + 1];

            var edges = new HashSet<Point> { new Point(0, 0) };

            while (edges.Any())
            {
                var newEdges = new HashSet<Point>();

                foreach (var edge in edges)
                {
                    var x = edge.X;
                    var y = edge.Y;
                    var geoidx = 0;

                    if (x == 0 && y == 0) geoidx = 0;
                    else if (y == 0) geoidx = x * 16807;
                    else if (x == 0) geoidx = y * 48271;
                    else if (x == targetX && y == targetY) geoidx = 0;
                    else
                    {
                        geoidx = erosions[x - 1, y] * erosions[x, y - 1];
                    }

                    geos[x, y] = geoidx;
                    erosions[x, y] = (geoidx + depth) % 20183;
                    types[x, y] = erosions[x, y] % 3;

                    riskLevel += types[x, y];

                    if (x < targetX) newEdges.Add(new Point(x + 1, y));
                    if (y < targetY) newEdges.Add(new Point(x, y + 1));
                }

                edges = newEdges;
            }

            return riskLevel;
        }
    }
}
