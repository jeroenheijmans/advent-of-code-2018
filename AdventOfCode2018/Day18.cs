using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day18
    {
        private readonly ITestOutputHelper output;

        public Day18(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string testInput = @"
.#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.
";

        public const string puzzleInput = @"
.#....|###.|.||...|..|#|..#...###|...|....#....##.
#.|.|.||.#...|.#..#.#.##.##||.###|..|.#.|.#...|.#|
..|#...#|||......#..#.|..#.|..#|##....|||.....##..
..#|.|......|.##...#.|||.|.||.#.|.#...#...#.....|.
####|.##.|#...|..||#.#.##....#|..|.#|...#...#.....
.#.#.|....##...#..|.|..|.#.|...#..#|#....|#|#..#..
||...|#.....#..............#..#|..##.|#|#|.#.#....
.#|##...|.|.|....#|....#|.|.|...#|..#.......#.||#|
....#.|..#..###|#|.#.#......#.#.#..#.#.#....#.#|||
#.#.#..#...|....#####|....#...|#|...#|#.|.....||..
.#...#|....||..||..|....##.#..#..#.|#.....|.#|.##.
....|..|..#|..#||.|#.....||..#|.|............#...|
..#...|#...|#|.....|....#...##....#.|#...#..##|##.
........|.#.#.#.|#..|..#...|...|#||.##.|........|.
..#.##...#.#.##|.||.|#..#|.|.#|.|.|...#|.|..|.|.|.
##....|#.|##|...|##..#.||..|..#.|....#.##||.......
.....|...|...|||...#.|##||||.|.####.||#.##|#.#...#
..|##|.#..#.|.||..|...#...|........#.|.|...##.|.#.
.##.#|.|.|..||....#.|..#|.|..#|.||#||##.....#.|.|.
.#...||.|..............|......#|..##.#.#|.||.#..#.
.....|#.|##|.||##.#..###..#.##..|..|....|...#.#||.
|........|.|##..#||.#.|.|.##.|.....#.......||....#
|##|.#.|...#.#||#.|.||..#|#.....|.|.....##..#..||.
....||..|.|....||..|#..|.#.|.##..|...##..........|
.#.#|#.#|..|..|.||#...|......|..#|#.#..#.###......
|.|...|##.|#..##....|#|....##|..#|#|.|..#...|#|...
....||....###......|.#.|#.|#.#|...#.|.##.......|.#
...|.....|..#.|.|.|#.|###|||...|.#||#..#......#..#
...##|......#..#|#|...||||.|##|.#.......#|....##..
|.|||#.#..|...#..|#.|...|..#.|.##..#.#........#.|#
|##.......|.#|##|...|....#|.#.|.#.#||.#|.#...##.|#
##.#.#.|#......#...#|.....||.|..||#|...|#..||#....
..|#..#.....#.#......|.||#..||....#...#|##...##..|
.|..|.|.||..|##..#.###.||#.|...#...|.#.......|.|#.
..#|...##.##.......|#..##|...|.|.#|#|...|.#..#..|.
.#|#|.||.|||...##|.#..|...#..##|#|.#..#......##|.|
..|.....#|..#....|....#.........##...|.....|...#.#
..|#.|..#.###.||||.||||#.|.||#....#.........|.|...
...#.|..#..|..||....|...#....|.#|##.#...|#|.#||...
....|.|......####|.#..|........||.#|.#|.#....#..|#
...||....|||..|....|..|...##.|.|#...|..|.|..|##.#.
.|.####...#.#|...###.|.|......|......#....#..#....
....#.###.....#|..||..##||.|||....|###.###||...||.
.|.|..#..#|..#.#.##..|.#.|..#.#..##..#||...|..|.#.
.|..|.#.###.|.#.||.##.........#.#|#...##..#.|#...|
##|.|.##..||.##|...#|##..|#.|...||..|.#..#.|.#|.|.
.|#.#|..|.#|.||.#.##.##...||.....#|.....|..|#.....
#|.##.##.|##.#.#|.....|.##||.|#.#.#.#..#...||..#||
...||...|#...##..||.....|..|....|.#.|.#......#.|..
.|...|.||.||.|...|#|......|....|..|#.#.......#...|
";

        [Fact] public void Solution_1_test_example() => Assert.Equal(1147, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(360720, Solve1(puzzleInput));

        // With some help of CSV output and Excel, as there was a recurring trend.
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(197276, Solve2(puzzleInput));

        public int Solve1(string input)
        {
            return SolveInternal(input);
        }

        public int Solve2(string input)
        {
            return SolveInternal(input, 1_000_000_000);
        }

        private int SolveInternal(string input, int iterations = 10)
        {
            var data = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.ToArray())
                .ToArray();

            var width = data[0].Length;
            var height = data.Length;

            var grid1 = new char[width, height];
            var grid2 = new char[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid1[x, y] = data[y][x];
                    grid2[x, y] = data[y][x];
                }
            }

            var stable = false;
            var display = iterations / 10;
            iterations = Math.Min(2000, iterations);
            
            for (int i = 0; i < iterations && !stable; i++)
            {
                // if (i % display == 0) OutputGrid(width, height, grid1);

                if (i > 1000)
                {
                    var score = CalcScore(width, height, grid1);
                    output.WriteLine($"{i};{score}");
                }

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int treecount = 0;
                        int yardcount = 0;

                        for (int n = x - 1; n < x + 2; n++)
                        {
                            for (int m = y - 1; m < y + 2; m++)
                            {
                                if (n < 0 || n >= width) continue;
                                if (m < 0 || m >= height) continue;
                                if (n == x && y == m) continue;

                                if (grid1[n, m] == '|') treecount++;
                                if (grid1[n, m] == '#') yardcount++;
                            }
                        }

                        if (grid1[x, y] == '.')
                        {
                            if (treecount >= 3) grid2[x, y] = '|';
                        }

                        if (grid1[x, y] == '|')
                        {
                            if (yardcount >= 3) grid2[x, y] = '#';
                        }

                        if (grid1[x, y] == '#')
                        {
                            if (yardcount >= 1 && treecount >= 1) grid2[x, y] = '#';
                            else grid2[x, y] = '.';
                        }
                    }
                }

                stable = true;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (grid1[x, y] != grid2[x, y]) stable = false;
                        grid1[x, y] = grid2[x, y];
                    }
                }
            }

            return CalcScore(width, height, grid1);
        }

        private static int CalcScore(int width, int height, char[,] grid1)
        {
            var totalTrees = 0;
            var totalYards = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid1[x, y] == '|') totalTrees++;
                    if (grid1[x, y] == '#') totalYards++;
                }
            }

            return totalTrees * totalYards;
        }

        private void OutputGrid(int width, int height, char[,] grid1)
        {
            output.WriteLine("");

            for (int y = 0; y < height; y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < width; x++)
                {
                    sb.Append(grid1[x, y]);
                }
                output.WriteLine(sb.ToString());
            }
            output.WriteLine("");
        }
    }
}
