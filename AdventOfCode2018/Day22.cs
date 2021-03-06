﻿using System;
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
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(1008, Solve2(11109, 9, 731));

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

            OutputGrid(targetX + 1, targetY + 1, targetX, targetY, types);

            return riskLevel;
        }

        public int Solve2(int depth, int targetX, int targetY)
        {
            // > The fastest route might involve entering regions beyond 
            // > the X or Y coordinate of the target.
            //
            // Let's just take a wide margin?
            var maxx = targetX + 30;
            var maxy = targetY + 30;

            var types = new int[maxx, maxy];
            var geos = new int[maxx, maxy];
            var erosions = new int[maxx, maxy];

            var edges = new HashSet<Point> { new Point(0, 0) };

            var searchNodes = new HashSet<SearchNode>();
            var lookup = new Dictionary<ValueTuple<Point, Equipment>, SearchNode>();

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

                    if (x < maxx - 1) newEdges.Add(new Point(x + 1, y));
                    if (y < maxy - 1) newEdges.Add(new Point(x, y + 1));

                    var options = new HashSet<SearchNode>();
                    var point = new Point(x, y);

                    if (x == targetX && y == targetY && types[x, y] != 0) throw new Exception("Target should be rock according to AoC");
                    if (x == 0 && y == 0 && types[x, y] != 0) throw new Exception("Origin should be rock according to AoC");

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
                        var id = new ValueTuple<Point, Equipment>(option.Point, option.Equipment);
                        lookup.Add(id, option);

                        searchNodes.Add(option);
                        var otherOption = options.Single(o => o != option);
                        option.ConnectionsWithCost.Add(otherOption, 7);

                        var left = new ValueTuple<Point, Equipment>(point.Left(), option.Equipment);
                        var up = new ValueTuple<Point, Equipment>(point.Up(), option.Equipment);

                        if (lookup.TryGetValue(left, out var reach1))
                        {
                            option.ConnectionsWithCost.Add(reach1, 1);
                            reach1.ConnectionsWithCost.Add(option, 1);
                        }

                        if (lookup.TryGetValue(up, out var reach2))
                        {
                            option.ConnectionsWithCost.Add(reach2, 1);
                            reach2.ConnectionsWithCost.Add(option, 1);
                        }
                    }
                }

                edges = newEdges;
            }

            OutputGrid(maxx, maxy, targetX, targetY, types);
            
            return GetMinDistance(searchNodes, new Point(0, 0), new Point(targetX, targetY));
        }

        public int GetMinDistance(ISet<SearchNode> searchNodes, Point originPoint, Point targetPoint)
        {
            var origin = searchNodes.Single(n => n.Point == originPoint && n.Equipment == Equipment.Torch);
            var target = searchNodes.Single(n => n.Point == targetPoint && n.Equipment == Equipment.Torch);

            var visited = new HashSet<SearchNode>();
            var edges = new HashSet<SearchNode> { origin };
            var distances = new Dictionary<SearchNode, int> { { origin, 0 } };
            var discoveryTimers = new List<DiscoveryTimer>();

            while (edges.Any() || discoveryTimers.Any())
            {
                var newEdges = new HashSet<SearchNode>();

                for (int i = discoveryTimers.Count - 1; i >= 0; i--)
                {
                    var timer = discoveryTimers[i];

                    timer.Ticks--;

                    if (timer.Ticks == 0)
                    {
                        distances[timer.SearchNode] = Math.Min(timer.TargetCost, distances.GetOrMaxInt(timer.SearchNode));
                        newEdges.Add(timer.SearchNode);
                        discoveryTimers.RemoveAt(i);
                    }
                }

                foreach (var edge in edges)
                {
                    if (visited.Contains(edge)) continue;

                    visited.Add(edge);

                    foreach (var option in edge.ConnectionsWithCost)
                    {
                        if (option.Value == 1)
                        {
                            distances[option.Key] = Math.Min(distances[edge] + 1, distances.GetOrMaxInt(option.Key));
                            newEdges.Add(option.Key);
                        }
                        else
                        {
                            discoveryTimers.Add(new DiscoveryTimer { SearchNode = option.Key, TargetCost = distances[edge] + 7 });
                        }
                    }
                }

                edges = newEdges;
            }

            return distances[target];
        }

        public class DiscoveryTimer
        {
            public SearchNode SearchNode { get; set; }
            public int TargetCost { get; set; }
            public int Ticks { get; set; } = 6;
        }

        public class SearchNode
        {
            public SearchNode(Point p, Equipment e)
            {
                Point = p;
                Equipment = e;
            }

            public Point Point { get; }
            public Equipment Equipment { get; }
            public IDictionary<SearchNode, int> ConnectionsWithCost { get; } = new Dictionary<SearchNode, int>();

            public override string ToString()
            {
                return $"({Point.X}, {Point.Y}) with {Equipment} and {ConnectionsWithCost.Count()} connections";
            }
        }

        public enum Equipment
        {
            Torch,
            Gear,
            Neither,
        }

        private void OutputGrid(int maxx, int maxy, int targetX, int targetY, int[,] types)
        {
            for (int y = 0; y < maxy; y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < maxx; x++)
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
