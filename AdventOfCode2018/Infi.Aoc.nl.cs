using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2018
{
    public class Infi
    {
        public const string puzzleInput = @"
╔╦╦╝═╗╔╣╗╦╔╔╔═║╔╩═╚╗
║═║║║╦╣║╣╬╠╚║╦╦╦║╦╠╣
╚╗╬╦║═╦╦╚╩╚═╗╝║╣║╗║═
╝║╩╩╬║╩╗╗╚╦╗╔╗║╚═╗╬╠
╚╝╗╝╠╗╠╗╗╝╝╬╚╗╚╝╚╣╚╦
╚╔═╚╗╬╦╦║╣╠═║╔═╠═╔╝║
╠╣╚╠╚╗╦╚═╦╝═╠╝╝║═║╔╚
╦║╩╝╗╚║╚╦╠╝╣╝╦╦╗╗═║═
╣═╔║╠║╦╚╦╚═╣╗╦║╝║╗╚║
╩═╝═╚╝╩╚═╚╗╬╩══╗╩╚╝╔
╗╔╔╝╔═╗═║╠╠╬╚╗╣╩╔╗╚╣
║╝╦╔╩╝╠╩║║║║╩╣║╦╗╣╚╠
╠╩╠╣╦╠╦╝╬╝╩║╠║╠║╚╚╔╦
║║╣╣║╦╠╔║╣╔╚╦╚╦╝║╠╝═
╗╚╔╔╣╠║║╔╩╝║╚╗╚╔═╦╗╣
╣╝╔╣╬╠╦═╣╩╬╚╔║╠║╦╦╣╬
╚═╦╔╝═╠╠╚╔╬╠╣╩═╦╬╝╦╣
║╦╣═╣╗╚══║╚╠╗╝╠╠╠╬╬║
╝╩╦═╔╝╔╗╬╔╔╠╗╗╣╗╗╝╝║
╠╝╗╩╚╗╦╦╚╣╗╠║╦╔╗╚╗═╬";

        [Fact] public void Solve_puzzle_1() => Assert.Equal(40, Solve1(puzzleInput));
        [Fact] public void Solve_puzzle_2() => Assert.Equal(0, Solve2(puzzleInput));

        public class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Char { get; set; }
            public ISet<Node> ConntectedNodes { get; } = new HashSet<Node>();

            public void ConnectTo(Node other)
            {
                other.ConntectedNodes.Add(this);
                this.ConntectedNodes.Add(other);
            }

            public bool IsOpenLeft => new [] { '╗', '╦', '╝', '╬', '╩', '═', '╣' }.Contains(Char);
            public bool IsOpenRight => new [] { '╔', '╠', '╦', '╚', '╬', '╩', '═' }.Contains(Char);
            public bool IsOpenTop => new [] { '║', '╠', '╚', '╝', '╬', '╩', '╣' }.Contains(Char);
            public bool IsOpenBottom => new [] { '║', '╔', '╗', '╠', '╦', '╬', '╣' }.Contains(Char);
        }

        public int Solve1(string input)
        {
            Node[,] nodes = ConvertToNodeArray(input);
            var current = nodes[0, 0];
            var goal = nodes[nodes.GetLength(0) - 1, nodes.GetLength(1) - 1];
            var visitedNodes = new HashSet<Node> { current };

            return GetShortestDistanceTo(current, goal, visitedNodes);
        }

        public int Solve2(string input)
        {
            Node[,] nodes = ConvertToNodeArray(input);
            return GetShortestTwilightDistanceTo(0, 0, 0, nodes);
        }

        private int GetShortestTwilightDistanceTo(int currentX, int currentY, int counter, Node[,] grid)
        {
            if (currentX == grid.GetLength(0) - 1 && currentY == grid.GetLength(1))
            {
                return 0;
            }

            if (counter > 100)
            {
                return int.MaxValue; // Escape hatch for now, maxrecursion (sort of)
            }

            int shortestPath = int.MaxValue - 1; // Same trick as puzzle 1

            foreach (var target in grid[currentX, currentY].ConntectedNodes)
            {
                var clone = Clone(grid);
                // shift!! and as part of the shift: reconnect nodes properly
                // possibly shift Santa here as well?
                var dist = GetShortestTwilightDistanceTo(target.X, target.Y, counter + 1, clone);
                shortestPath = Math.Min(shortestPath, dist);
            }

            return shortestPath;
        }

        private int GetShortestDistanceTo(Node current, Node goal, ISet<Node> visitedNodes)
        {
            if (current == goal)
            {
                return 0;
            }

            // -1 is dirty hack to prevent integer overflow
            // put differently, this effectively makes int.MaxValue 
            // the "length" of a dead-end path
            int shortestPath = int.MaxValue - 1;

            foreach (var target in current.ConntectedNodes.Where(n => !visitedNodes.Contains(n)))
            {
                var visitedNew = new HashSet<Node>(visitedNodes) {target};
                var dist = GetShortestDistanceTo(target, goal, visitedNew) + 1;
                shortestPath = Math.Min(shortestPath, dist);
            }

            return shortestPath;
        }

        private static Node[,] Clone(Node[,] grid)
        {
            var nodes = new Node[grid.GetLength(0), grid.GetLength(1)];

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    nodes[x, y] = new Node { X = x, Y = y, Char = grid[x, y].Char };

                    if (x > 0
                        && nodes[x, y].IsOpenLeft
                        && nodes[x - 1, y].IsOpenRight)
                    {
                        nodes[x, y].ConnectTo(nodes[x - 1, y]);
                    }

                    if (y > 0
                        && nodes[x, y].IsOpenTop
                        && nodes[x, y - 1].IsOpenBottom)
                    {
                        nodes[x, y].ConnectTo(nodes[x, y - 1]);
                    }
                }
            }

            return nodes;
        }

        private static Node[,] ConvertToNodeArray(string input)
        {
            var grid = input.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.None)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(row => row.ToArray())
                .ToArray();

            var height = grid.Length;
            var width = grid.First().Length;

            var nodes = new Node[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    nodes[x, y] = new Node { X = x, Y = y, Char = grid[y][x] };

                    if (x > 0
                        && nodes[x, y].IsOpenLeft
                        && nodes[x - 1, y].IsOpenRight)
                    {
                        nodes[x, y].ConnectTo(nodes[x - 1, y]);
                    }

                    if (y > 0
                        && nodes[x, y].IsOpenTop
                        && nodes[x, y - 1].IsOpenBottom)
                    {
                        nodes[x, y].ConnectTo(nodes[x, y - 1]);
                    }
                }
            }

            return nodes;
        }
    }
}
