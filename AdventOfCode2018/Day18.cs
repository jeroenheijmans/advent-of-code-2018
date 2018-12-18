using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var grid1 = input.To2DCharArray();
            var grid2 = input.To2DCharArray();

            var width = grid1.GetLength(0);
            var height = grid1.GetLength(1);

            var previousStates = new List<string>();

            for (int i = 0; i < iterations; i++)
            {
                var sb = new StringBuilder();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        sb.Append(grid1[x, y]);

                        int treecount = 0;
                        int yardcount = 0;

                        for (int n = Math.Max(0, x - 1); n < Math.Min(width, x + 2); n++)
                        {
                            for (int m = Math.Max(0, y - 1); m < Math.Min(height, y + 2); m++)
                            {
                                if (n == x && y == m) continue;
                                if (grid1[n, m] == '|') treecount++;
                                if (grid1[n, m] == '#') yardcount++;
                            }
                        }

                        if (grid1[x, y] == '.' && treecount >= 3)
                        {
                            grid2[x, y] = '|';
                        }

                        if (grid1[x, y] == '|' && yardcount >= 3)
                        {
                            grid2[x, y] = '#';
                        }

                        if (grid1[x, y] == '#')
                        {
                            if (yardcount >= 1 && treecount >= 1) grid2[x, y] = '#';
                            else grid2[x, y] = '.';
                        }
                    }
                }

                // Repeat state detection is "lucky" as it assumes a state not occurring
                // extra times in the middle of a cycle.
                var state = sb.ToString();
                var idx = previousStates.IndexOf(state);
                if (idx >= 0)
                {
                    var cycleSize = i - idx;
                    var skipCount = (iterations - i) / cycleSize;
                    output.WriteLine($"Found repeat at {i} from {idx} (cycle size {cycleSize}).");                    
                    i += skipCount * cycleSize;
                    output.WriteLine($"Skipped {skipCount} to {i}.");
                    previousStates.Clear();
                }
                previousStates.Add(state);

                Array.Copy(grid2, grid1, grid1.Length);
            }

            return grid1.Cast<char>().Count(c => c == '|') * grid1.Cast<char>().Count(c => c == '#');
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
