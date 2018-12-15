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
    public class Day15
    {
        private readonly ITestOutputHelper output;

        public Day15(ITestOutputHelper output)
        {
            this.output = output;
        }

        private const int Example1Solution = 27730;
        private const string Example1Input = @"
#######   
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#   
#######
";

        private const int My_Example_A_Solution = 33 * (101 + 200);
        private const string My_Example_A_Input = @"
###
#E#
#G#
#E#
###
";

        private const int My_Example_B_Solution = 33 * (101 + 200);
        private const string My_Example_B_Input = @"
###
#G#
#E#
#G#
###
";

        private const int My_Example_C_Solution = 33 * (101 + 200);
        private const string My_Example_C_Input = @"
###
#G#
#.#
#E#
#.#
#G#
###
";

        private const string puzzleInput = @"";

        [Fact] public void Solution_1_test_my_example_A() => Assert.Equal(My_Example_A_Solution, Solve1(My_Example_A_Input));
        [Fact] public void Solution_1_test_my_example_B() => Assert.Equal(My_Example_B_Solution, Solve1(My_Example_B_Input));
        [Fact] public void Solution_1_test_my_example_C() => Assert.Equal(My_Example_C_Solution, Solve1(My_Example_C_Input));
        [Fact] public void Solution_1_test_example_1() => Assert.Equal(Example1Solution, Solve1(Example1Input));
        //[Fact] public void Solution_1_test_real_input() => Assert.Equal(0, Solve1(puzzleInput));

        private const int StartingHitPoints = 200;
        private const int DefaultAttackPower = 3;

        public int Solve1(string input)
        {
            var battle = CreateBattleFromInput(input);
            OutputGrid(battle);

            for (int numberOfCompletedRounds = 0; numberOfCompletedRounds < 1_000; numberOfCompletedRounds++)
            {
                var creaturesInActingOrder = battle.Creatures
                    .OrderBy(c => c.Position.Point.Y)
                    .ThenBy(c => c.Position.Point.X)
                    .ToArray();

                foreach (var creature in creaturesInActingOrder)
                {
                    if (battle.IsOver())
                    {
                        OutputGrid(battle);
                        return battle.GetScoreFor(numberOfCompletedRounds);
                    }

                    var moveOptions = creature.Position.EnumerateAdjacentPositionsInReadingOrder();
                    
                    if (!creature.CanAttack())
                    {
                        var optimalMove = GetOptimalMoveFor(creature);
                        creature.MoveTo(optimalMove);
                    }

                    var target = creature.Position
                        .EnumerateAdjacentPositionsInReadingOrder()
                        .Select(p => p.Creature)
                        .FirstOrDefault(c => c.IsEnemyFor(creature));

                    if (target != null)
                    {
                        battle.Fight(creature, target);
                    }
                }
            }

            return 0;
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_1()
        {
            var battle = CreateBattleFromInput("E.G");
            var attacker = battle.Creatures.Single(c => c.IsElf);
            var result = GetOptimalMoveFor(attacker);
            Assert.Equal(battle.Grid[1, 0], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_2()
        {
            var battle = CreateBattleFromInput(@"
                ######
                #E#GE#
                #....#
                ######
            ");
            var attacker = battle.Creatures.Single(c => c.Position.Point.X == 1);
            var result = GetOptimalMoveFor(attacker);
            Assert.Equal(battle.Grid[1, 0], result);
        }

        private static Position GetOptimalMoveFor(Creature creature)
        {
            var consideredPositions = new HashSet<Position> { creature.Position };

            List<List<Position>> paths = creature.Position
                .EnumerateAdjacentPositionsInReadingOrder()
                .Where(p => !p.HasCreature)
                .Select(p => new List<Position> { p })
                .ToList();

            var depth = 100;
            for (int i = 0; i < depth; i++)
            {
                var newPaths = new List<List<Position>>();

                foreach (var path in paths)
                {
                    var last = path.Last();

                    foreach (var p in last.EnumerateAdjacentPositionsInReadingOrder())
                    {
                        if (consideredPositions.Contains(p)) continue;

                        if (p.HasEnemyFor(creature))
                        {
                            return path.First();
                        }

                        if (p.HasCreature) // Must be a friend...
                        {
                            continue;
                        }

                        var newPath = new List<Position>(path);
                        newPath.Add(p);
                        newPaths.Add(newPath);

                        consideredPositions.Add(p);
                    }
                }
            }

            throw new Exception("No optimal move found. Not expected based on puzzle description.");
        }

        private static Battle CreateBattleFromInput(string input)
        {
            var data = input.SplitByNewline(shouldTrim: true);
            var width = data.First().Length;
            var height = data.Length;
            var battle = new Battle(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (data[y][x] == '#') continue;

                    Creature creature = null;
                    if (data[y][x] == 'G') creature = Creature.CreateGoblin();
                    if (data[y][x] == 'E') creature = Creature.CreateElf();
                    if (creature != null) battle.Creatures.Add(creature);

                    battle.Grid[x, y] = new Position(x, y, creature);

                    if (y > 0 && battle.Grid[x, y - 1] != null)
                    {
                        battle.Grid[x, y].ConnectToPositionAbove(battle.Grid[x, y - 1]);
                    }

                    if (x > 0 && battle.Grid[x - 1, y] != null)
                    {
                        battle.Grid[x, y].ConnectToPositionToTheLeft(battle.Grid[x - 1, y]);
                    }
                }
            }

            return battle;
        }

        [Fact]
        public void Creature_enemy_detection_works()
        {
            Assert.True(Creature.CreateElf().IsEnemyFor(Creature.CreateGoblin()));
            Assert.True(Creature.CreateGoblin().IsEnemyFor(Creature.CreateElf()));
            Assert.False(Creature.CreateElf().IsEnemyFor(Creature.CreateElf()));
            Assert.False(Creature.CreateGoblin().IsEnemyFor(Creature.CreateGoblin()));
        }

        public class Creature
        {
            public bool IsGoblin { get; set; }
            public int HitPoints { get; set; } = StartingHitPoints;
            public int AttackPower => DefaultAttackPower;
            public Position Position { get; set; }

            public bool IsElf => !IsGoblin;
            public bool IsEnemyFor(Creature other) => other.IsGoblin != this.IsGoblin;

            public void MoveTo(Position other)
            {
                if (other.Creature != null) throw new InvalidOperationException("Cannot move into occupied position");
                this.Position = other;
                other.Creature = this;
            }

            public bool CanAttack() =>
                true == Position.Up?.Creature?.IsEnemyFor(this)
                || true == Position.Left?.Creature?.IsEnemyFor(this)
                || true == Position.Right?.Creature?.IsEnemyFor(this)
                || true == Position.Down?.Creature?.IsEnemyFor(this);

            public char Rune => IsGoblin ? 'G' : 'E';

            public override string ToString() => $"{Rune} ({HitPoints} HP) at ({Position.Point.X}, {Position.Point.Y})";

            public static Creature CreateGoblin() => new Creature { IsGoblin = true };
            public static Creature CreateElf() => new Creature { IsGoblin = false };
        }

        public class Position
        {
            public Position(int x, int y, Creature creature = null)
            {
                Point = new Point(x, y);
                Creature = creature;
                if (creature != null) creature.Position = this;
            }

            public Point Point { get; }
            public Creature Creature { get; set; }
            public bool HasCreature => Creature != null;
            public bool HasEnemyFor(Creature other) => Creature?.IsEnemyFor(other) ?? false;

            public Position Up { get; set; }
            public Position Left { get; set; }
            public Position Right { get; set; }
            public Position Down { get; set; }

            public void ConnectToPositionAbove(Position other)
            {
                other.Down = this;
                this.Up = other;
            }

            public void ConnectToPositionToTheLeft(Position other)
            {
                other.Right = this;
                this.Left = other;
            }

            public Position[] EnumerateAdjacentPositionsInReadingOrder() =>
                new [] { Up, Left, Right, Down }.Where(p => p != null).ToArray();

            public override string ToString() => $"({Point.X}, {Point.Y}) with {Creature?.Rune ?? '.'}";
        }

        public class Battle
        {
            public Battle(int width, int height)
            {
                Width = width;
                Height = height;
                Grid = new Position[width, height];
            }

            public int Width { get; }
            public int Height { get; }
            public Position[,] Grid { get; }
            public ISet<Creature> Creatures { get; } = new HashSet<Creature>();

            public void Fight(Creature attacker, Creature target)
            {
                if (!attacker.IsEnemyFor(target)) throw new InvalidOperationException("Friendly fire forbidden!");
                target.HitPoints -= attacker.AttackPower;
                if (target.HitPoints <= 0) Creatures.Remove(target);
            }

            public bool IsOver() => Creatures.All(c => c.IsGoblin) || Creatures.All(c => c.IsElf);

            public int GetScoreFor(int rounds)
            {
                return rounds * Creatures.Sum(x => x.HitPoints);
            }
        }
        
        private void OutputGrid(Battle battle)
        {
            for (int y = 0; y < battle.Height; y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < battle.Width; x++)
                {
                    if (battle.Grid[x, y] == null) sb.Append('█');
                    else sb.Append(battle.Grid[x, y].Creature?.Rune ?? '.');
                }
                output.WriteLine(sb.ToString());
            }
        }
    }
}
