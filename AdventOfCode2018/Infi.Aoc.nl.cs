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

        [DebuggerDisplay("{Char} ({X},{Y})")]
        public class Node
        {
            public int X { get; set; } // For debugging only
            public int Y { get; set; } // For debugging only
            public char Char { get; set; } // For debugging only
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
            var grid = input.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.None)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(row => row.ToArray())
                .ToArray();

            Node[,] nodes = ConvertToNodeArray(grid);
            var height = grid.Length;
            var width = grid.First().Length;
            var current = nodes[0, 0];
            var goal = nodes[width - 1, height - 1];
            var visitedNodes = new HashSet<Node> { current };

            return GetShortestDistanceTo(current, goal, visitedNodes);
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

        private static Node[,] ConvertToNodeArray(char[][] grid)
        {
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
