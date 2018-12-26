using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2018
{
    public class Infi
    {
        private readonly ITestOutputHelper output;

        public Infi(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string puzzleInput = @"
╠╬╗══╝╠╗╔═╠╦╬║╚╩╦╣╩╔
╣╩║╠╚╬╦╠╬╣╗╔╗╣╗╗╚╝╩║
╦╠╬╩║═╠╩╔╗╚╣║══╣╦╗║╔
╠╠╚╬╔╝╝╝═║╚╩╚╠═╣╚╝╗╩
╣╩╩═╩╦╗╠╗╗╠║╦╠═╔═╠═╗
╚╝╦║║╩╚╩═╬╗╚╔╚╔╝╣╠═║
║╠╗═╔╦═║╦╣╬╣╝╠╝╣╔╣╩╠
╔╣╦╚╚╗╣║╚╬╔═╝╣╚╚╚╦╔╗
║╝║╠╩═╗╩═║╔╝╚╚╝╬╗═╦═
╝╝╗╝╬═╚╠╬╬╚╩╩═╚╣╔═╣═
╩═╝╠╩╠╝╣╬║╝╦╔╗╗╚╝╣╗═
╦╔╚╠═╠╗╝║║╠═╩╚╠╚╠║╦╠
╠╔╠╠╚╚╬╗╠╚╔╝╝╣╦╝║║╣╬
╠╗╚╗║║╚╝╠╩╔╝║╚╔╔╦╦╩╚
╔║╔╣╬╝╚╔╩═╬╗╦╣╩╣║╣║╦
╝╔╚║╦╣╔╗╔╗╠╩╝╠║╦╝╚╚╔
╗╝╗═╔╝╩╔╔╝║═╚╠╚╠╚╬╬╚
╦║╣║╦══╩═╚╚╩══╗║║╣╔═
╔╝╦╝═╩═╚╩╠╔╣╚╩╚═╣╚╬╔
╦║╦╩╔╩╗╚═╝╝╦╝═╦╠╚═╩╣";

        [Fact] public void Solve_puzzle_1() => Assert.Equal(42, Solve1(puzzleInput));
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

            public bool IsOpenLeft => IsOpenLeft(Char);
            public bool IsOpenRight => IsOpenRight(Char);
            public bool IsOpenTop => IsOpenTop(Char);
            public bool IsOpenBottom => IsOpenBottom(Char);
        }

        public static bool IsOpenLeft(char Char) => new[] { '╗', '╦', '╝', '╬', '╩', '═', '╣' }.Contains(Char);
        public static bool IsOpenRight(char Char) => new[] { '╔', '╠', '╦', '╚', '╬', '╩', '═' }.Contains(Char);
        public static bool IsOpenTop(char Char) => new[] { '║', '╠', '╚', '╝', '╬', '╩', '╣' }.Contains(Char);
        public static bool IsOpenBottom(char Char) => new[] { '║', '╔', '╗', '╠', '╦', '╬', '╣' }.Contains(Char);

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
            int size = GetGridSize(input);

            var origin = new Point(0, 0);
            var goal = new Point(size - 1, size - 1);
            var position = origin;

            var state = Regex.Replace(input, "\r?\n", "");
            var time = 0;

            var visited = new HashSet<(string state, Point position)>();
            var edges = new HashSet<(string state, Point position, int distance)> { (state, position, 0) };

            var i = 0;

            while (edges.Any() && i++ < 10_000)
            {
                var newEdges = new HashSet<(string state, Point position, int distance)>();
                var newstate = Shift(state, size, time);

                foreach (var edge in edges)
                {
                    if (visited.Contains((edge.state, edge.position))) continue;
                    if (edge.position == goal)
                    {
                        output.WriteLine("Found winning position!");
                        OutputGrid(edge.state, edge.position, size, time);
                        // NOT 40 (early guess)
                        // NOT 34 (with output...?)
                        // NOT 33 (off by one, guessed?)
                        return edge.distance;
                    }

                    OutputGrid(edge.state, edge.position, size, time);

                    visited.Add((edge.state, edge.position));

                    for (int dir = 0; dir < 4; dir++)
                    {
                        if (CanMoveInDirection(edge.state, edge.position, size, dir))
                        {
                            var newpos = GetTarget(edge.position, dir);
                            newpos = ShiftSantaIfNeeded(newpos, time, size);
                            newEdges.Add((newstate, newpos, edge.distance + 1));
                            if (newpos == goal) { output.WriteLine("Will find winning move!"); OutputGrid(edge.state, edge.position, size, time); }
                        }
                    }
                }

                edges = newEdges;
                time = (time + 1) % size;
            }

            throw new NoSolutionFoundException();
        }

        private void OutputGrid(string state, Point position, int size, int time)
        {
            output.WriteLine($"At time {time}");
            for (int y = 0; y < size; y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < size; x++)
                {
                    var c = state[y * size + x];
                    if (position.X == x && position.Y == y) sb.Append($"O");
                    else sb.Append($"{c}");
                }
                output.WriteLine(sb.ToString());
            }
            output.WriteLine("");
        }

        private static int GetGridSize(string input)
        {
            var width = input.SplitByNewline(shouldTrim: true).First().Length;
            var height = input.SplitByNewline(shouldTrim: true).Count();

            if (width != height) throw new NotSupportedException("Only square grids are supported");

            var size = width;
            return size;
        }

        [Fact] public void Shift_sample_1() => Assert.Equal("4123abcd4567qwer", Shift("1234abcd4567qwer", 4, 0));
        [Fact] public void Shift_sample_2() => Assert.Equal("1w34a2cd4b67q5er", Shift("1234abcd4567qwer", 4, 1));

        private static string Shift(string state, int size, int time)
        {
            if (time % 2 == 0) // row
            {
                var sb = new StringBuilder();
                for (int y = 0; y < size; y++)
                {
                    var line = state.Substring(y * size, size);
                    if (y == time)
                    {
                        sb.Append(line.Last());
                        sb.Append(line.Take(line.Length - 1).ToArray());
                    }
                    else
                    {
                        sb.Append(line);
                    }
                }
                return sb.ToString();
            }
            else // column
            {
                var sb = new StringBuilder();
                char delayedX = state[(size - 1) * size + time];
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (x == time)
                        {
                            sb.Append(delayedX);
                            delayedX = state[y * size + x];
                        }
                        else
                        {
                            sb.Append(state[y * size + x]);
                        }
                    }
                }
                return sb.ToString();
            }
        }

        [Fact] public void ShiftSanta_sample_1() => Assert.Equal(new Point(0, 0), ShiftSantaIfNeeded(new Point(0, 0), 5, 20));
        [Fact] public void ShiftSanta_sample_2() => Assert.Equal(new Point(2, 0), ShiftSantaIfNeeded(new Point(1, 0), 0, 20));
        [Fact] public void ShiftSanta_sample_3() => Assert.Equal(new Point(1, 0), ShiftSantaIfNeeded(new Point(0, 0), 0, 20));
        [Fact] public void ShiftSanta_sample_4() => Assert.Equal(new Point(0, 0), ShiftSantaIfNeeded(new Point(0, 0), 1, 20));
        [Fact] public void ShiftSanta_sample_5() => Assert.Equal(new Point(1, 1), ShiftSantaIfNeeded(new Point(1, 0), 1, 20));

        private static Point ShiftSantaIfNeeded(Point current, int time, int size)
        {
            if (time % 2 == 0 && time == current.Y)
            {
                return current.X == (size - 1) ? new Point(0, current.Y) : new Point(current.X + 1, current.Y);
            }
            else if (time % 2 == 1 && time == current.X)
            {
                return current.Y == (size - 1) ? new Point(current.X, 0) : new Point(current.X, current.Y + 1);
            }
            else
            {
                return current;
            }
        }

        private static Point GetTarget(Point current, int direction)
        {
            if (direction == 0) return new Point(current.X, current.Y - 1);
            if (direction == 1) return new Point(current.X + 1, current.Y);
            if (direction == 2) return new Point(current.X, current.Y + 1);
            if (direction == 3) return new Point(current.X - 1, current.Y);
            throw new ArgumentException();
        }

        private static bool CanMoveInDirection(string state, Point position, int size, int direction)
        {
            char current = state[position.Y * size + position.X];

            switch (direction)
            {
                case 0: // UP
                    if (position.Y <= 0) return false;
                    if (!IsOpenTop(current)) return false;
                    if (!IsOpenBottom(state[(position.Y - 1) * size + position.X])) return false;
                    return true;
                case 1: // RIGHT
                    if (position.X >= size - 1) return false;
                    if (!IsOpenRight(current)) return false;
                    if (!IsOpenLeft(state[position.Y * size + position.X + 1])) return false;
                    return true;
                case 2: // DOWN
                    if (position.Y >= size - 1) return false;
                    if (!IsOpenBottom(current)) return false;
                    if (!IsOpenTop(state[(position.Y + 1) * size + position.X])) return false;
                    return true;
                case 3: // LEFT
                    if (position.X <= 0) return false;
                    if (!IsOpenLeft(current)) return false;
                    if (!IsOpenRight(state[position.Y * size + position.X - 1])) return false;
                    return true;
                default:
                    throw new ArgumentException();
            }
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

        private static void ReconnectNodes(Node[,] grid, int x, int y)
        {
            if (x > 0 && grid[x, y].IsOpenLeft && grid[x - 1, y].IsOpenRight)
            {
                grid[x, y].ConnectTo(grid[x - 1, y]);
            }

            if (x < grid.GetLength(0) - 2 && grid[x, y].IsOpenRight && grid[x + 1, y].IsOpenLeft)
            {
                grid[x, y].ConnectTo(grid[x + 1, y]);
            }

            if (y > 0 && grid[x, y].IsOpenTop && grid[x, y - 1].IsOpenBottom)
            {
                grid[x, y].ConnectTo(grid[x, y - 1]);
            }

            if (y < grid.GetLength(1) - 2 && grid[x, y].IsOpenBottom && grid[x, y + 1].IsOpenTop)
            {
                grid[x, y].ConnectTo(grid[x, y + 1]);
            }
        }

        private static Node[,] Clone(Node[,] grid)
        {
            var nodes = new Node[grid.GetLength(0), grid.GetLength(1)];

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    nodes[x, y] = new Node { X = x, Y = y, Char = grid[x, y].Char };
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
