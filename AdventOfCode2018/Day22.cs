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

        [Fact] public void Solution_2_test_example_1() => Assert.Equal(45, Solve2(510, 10, 10));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(-1, Solve2(11109, 9, 731));

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

            OutputGrid(targetX, targetY, types);

            return riskLevel;
        }

        public int Solve2(int depth, int targetX, int targetY)
        {
            var types = new int[targetX + 1, targetY + 1];
            var geos = new int[targetX + 1, targetY + 1];
            var erosions = new int[targetX + 1, targetY + 1];

            var edges = new HashSet<Point> { new Point(0, 0) };

            var searchNodes = new HashSet<SearchNode>();

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

                    if (x < targetX) newEdges.Add(new Point(x + 1, y));
                    if (y < targetY) newEdges.Add(new Point(x, y + 1));
                    // TODO: "The fastest route might involve entering regions beyond the X or Y coordinate of the target."

                    var options = new HashSet<SearchNode>();
                    var point = new Point(x, y);

                    if (types[x, y] == 0)
                    {
                        options.Add(new SearchNode(point, Equipment.Gear));
                        options.Add(new SearchNode(point, Equipment.Torch));
                    }
                    else if (types[x, y] == 1)
                    {
                        options.Add(new SearchNode(point, Equipment.Gear));
                        options.Add(new SearchNode(point, Equipment.Neither));
                    }
                    else if (types[x, y] == 2)
                    {
                        options.Add(new SearchNode(point, Equipment.Torch));
                        options.Add(new SearchNode(point, Equipment.Neither));
                    }

                    foreach (var option in options)
                    {
                        searchNodes.Add(option);
                        var otherOption = options.Single(o => o != option);
                        option.ConnectionsWithCost.Add(otherOption, 7);

                        var left = point.Left();
                        var up = point.Up();

                        var candidates = searchNodes.Where(n => n.Id.Item1 == left || n.Id.Item1 == up);

                        foreach (var candidate in candidates)
                        {
                            if (candidate.Id.Item2 == option.Id.Item2)
                            {
                                option.ConnectionsWithCost.Add(candidate, 1);
                                candidate.ConnectionsWithCost.Add(option, 1);
                            }
                        }
                    }
                }

                edges = newEdges;
            }

            OutputGrid(targetX, targetY, types);

            var originPoint = new Point(0, 0);
            var origin = searchNodes.Single(n => n.Id.Item1 == originPoint && n.Id.Item2 == Equipment.Torch);

            var targetPoint = new Point(targetX, targetY);
            var target = searchNodes.Single(n => n.Id.Item1 == targetPoint && n.Id.Item2 == Equipment.Torch);

            return GetMinDistance(origin, target, new HashSet<SearchNode>());
        }

        public int GetMinDistance(SearchNode current, SearchNode target, HashSet<SearchNode> visited, int distance = 0, int recur = 0)
        {
            if (recur > 10) return int.MaxValue;

            if (current == target)
            {
                return 0;
            }

            var dist = int.MaxValue;

            foreach (var option in current.ConnectionsWithCost)
            {
                if (visited.Contains(option.Key)) continue;
                
                if (option.Key.Id.Item1 != current.Id.Item1 && visited.Any(o => o.Id.Item1 == option.Key.Id.Item1)) continue;
                
                var set = new HashSet<SearchNode>(visited);
                set.Add(option.Key);
                dist = Math.Min(dist, GetMinDistance(option.Key, target, set, option.Value + distance, recur + 1));
            }

            return dist;
        }

        public class SearchNode
        {
            public SearchNode(Point p, Equipment e)
            {
                Id = new ValueTuple<Point, Equipment>(p, e);
            }

            public ValueTuple<Point, Equipment> Id { get; set; }
            public IDictionary<SearchNode, int> ConnectionsWithCost { get; } = new Dictionary<SearchNode, int>();

            public override string ToString()
            {
                return $"({Id.Item1.X}, {Id.Item1.Y}) with {Id.Item2} and {ConnectionsWithCost.Count()} connections";
            }
        }

        public enum Equipment
        {
            Torch,
            Gear,
            Neither,
        }

        private void OutputGrid(int targetX, int targetY, int[,] types)
        {
            for (int y = 0; y <= targetY; y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x <= targetX; x++)
                {
                    if (x == 0 && y == 0) sb.Append('X');
                    else if (x == targetX && y == targetY) sb.Append('T');
                    else if (types[x, y] == 0) sb.Append('.');
                    else if (types[x, y] == 1) sb.Append('=');
                    else if (types[x, y] == 2) sb.Append('|');
                }
                output.WriteLine(sb.ToString());
            }
        }

        [Fact]
        public void Sanity_check_on_value_tuple()
        {
            var tuple1 = new ValueTuple<Point, Equipment>(new Point(1, 1), Equipment.Gear);
            var tuple2 = new ValueTuple<Point, Equipment>(new Point(1, 1), Equipment.Gear);
            var tuple3 = new ValueTuple<Point, Equipment>(new Point(1, 1), Equipment.Neither);
            var tuple4 = new ValueTuple<Point, Equipment>(new Point(1, 2), Equipment.Gear);

            Assert.Equal(tuple1, tuple2);
            Assert.NotEqual(tuple1, tuple3);
            Assert.NotEqual(tuple1, tuple4);
            Assert.NotEqual(tuple3, tuple4);

            Assert.True(tuple1.Equals(tuple2));
            Assert.False(tuple1.Equals(tuple3));
            Assert.False(tuple1.Equals(tuple4));
            Assert.False(tuple3.Equals(tuple4));
        }
    }
}
