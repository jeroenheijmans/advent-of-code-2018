﻿using System;
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

        private const int ExampleBasicSolution = 27730;
        private const string ExampleBasicInput = @"
#######   
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#   
#######
";

        private const int ExtraExample1Solution = 36334;
        private const string ExtraExample1Input = @"
#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######
";

        private const int ExtraExample2Solution = 39514;
        private const string ExtraExample2Input = @"
#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######
";

        private const int ExtraExample3Solution = 27755;
        private const string ExtraExample3Input = @"
#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######
";

        private const int ExtraExample4Solution = 28944;
        private const string ExtraExample4Input = @"
#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######
";

        private const int ExtraExample5Solution = 18740;
        private const string ExtraExample5Input = @"
#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########
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

        private const string puzzleInput = @"
################################
##############.#################
##########G##....###############
#########.....G.################
#########...........############
#########...........############
##########.....G...#############
###########.........############
########.#.#..#..G....##########
#######..........G......########
##..GG..................###.####
##G..........................###
####G.G.....G.#####...E.#.G..###
#....##......#######........####
#.GG.#####.G#########.......####
###..####...#########..E...#####
#...####....#########........###
#.G.###.....#########....E....##
#..####...G.#########E.....E..##
#..###G......#######E.........##
#..##.........#####..........###
#......................#..E....#
##...G........G.......#...E...##
##............#..........#..####
###.....#...#.##..#......#######
#####.###...#######...#..#######
#########...E######....#########
###########...######.###########
############..#####..###########
#############.E..##.############
################.#..############
################################
";

        [Fact] public void Solution_1_test_my_example_A() => Assert.Equal(My_Example_A_Solution, Solve1(My_Example_A_Input));
        [Fact] public void Solution_1_test_my_example_B() => Assert.Equal(My_Example_B_Solution, Solve1(My_Example_B_Input));
        [Fact] public void Solution_1_test_my_example_C() => Assert.Equal(My_Example_C_Solution, Solve1(My_Example_C_Input));

        [Fact] public void Solution_1_basic_example() => Assert.Equal(ExampleBasicSolution, Solve1(ExampleBasicInput));

        [Fact] public void Solution_1_extra_example_1() => Assert.Equal(ExtraExample1Solution, Solve1(ExtraExample1Input));
        [Fact] public void Solution_1_extra_example_2() => Assert.Equal(ExtraExample2Solution, Solve1(ExtraExample2Input)); 
        [Fact] public void Solution_1_extra_example_3() => Assert.Equal(ExtraExample3Solution, Solve1(ExtraExample3Input)); 
        [Fact] public void Solution_1_extra_example_4() => Assert.Equal(ExtraExample4Solution, Solve1(ExtraExample4Input)); 
        [Fact] public void Solution_1_extra_example_5() => Assert.Equal(ExtraExample5Solution, Solve1(ExtraExample5Input)); 

        // Not: 251877
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(0, Solve1(puzzleInput));

        private const int StartingHitPoints = 200;
        private const int DefaultAttackPower = 3;

        public int Solve1(string input)
        {
            var battle = CreateBattleFromInput(input);
            output.WriteLine("Battle is starting...");
            OutputGrid(battle);

            for (int numberOfCompletedRounds = 0; numberOfCompletedRounds < 1_000; numberOfCompletedRounds++)
            {
                var creaturesInActingOrder = battle.Creatures
                    .OrderBy(c => c.Position.Point.Y)
                    .ThenBy(c => c.Position.Point.X)
                    .ToArray();

                var someCreatureActed = false; // To detect bugs

                foreach (var creature in creaturesInActingOrder)
                {
                    if (battle.IsOver())
                    {
                        var score = battle.GetScoreFor(numberOfCompletedRounds);

                        output.WriteLine("");
                        output.WriteLine("Battle is over!");
                        OutputGrid(battle, includeCreatures: true);
                        output.WriteLine("");
                        output.WriteLine($"Outcome: {numberOfCompletedRounds} full rounds x {battle.GetHitPointsLeft()} HP = {score}");

                        return score;
                    }

                    var moveOptions = creature.Position.EnumerateAdjacentPositionsInReadingOrder();
                    
                    if (!creature.CanAttack())
                    {
                        try
                        {
                            var optimalMove = GetOptimalMoveFor(creature);
                            creature.MoveTo(optimalMove);
                            someCreatureActed = true;
                        }
                        catch (NoMoveFoundException)
                        {
                            // output.WriteLine($"No optimal move available for [{creature}].");
                        }
                    }

                    var target = creature.FirstOrDefaultTarget();

                    if (target != null)
                    {
                        someCreatureActed = true;
                        var result = battle.Fight(creature, target);
                        if (result == FightResult.Death)
                        {
                            output.WriteLine($"Killing blow for {target}!");
                            OutputGrid(battle);
                        }
                    }
                }

                if (!someCreatureActed)
                {
                    output.WriteLine("No single creature could act anymore with this grid state:");
                    OutputGrid(battle);
                    throw new NotSupportedException("Deadlocks are not supported");
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
            Assert.Equal(battle.Grid[1, 2], result);
        }

        public class NoMoveFoundException : Exception
        { }

        private static Position GetOptimalMoveFor(Creature creature)
        {
            var consideredPositions = new HashSet<Position> { creature.Position };

            var positionsToTargetLocation = creature.Position
                .EnumerateAdjacentPositionsInReadingOrder()
                .Where(p => !p.HasCreature)
                .ToDictionary(p => p, p => p.EnumerateAdjacentPositionsInReadingOrder());

            var depth = 100;
            for (int i = 0; i < depth; i++)
            {
                var newPositionsToTargetLocation = new Dictionary<Position, List<Position>>();

                foreach (var kvp in positionsToTargetLocation)
                {
                    newPositionsToTargetLocation[kvp.Key] = new List<Position>();

                    foreach (var target in kvp.Value)
                    {
                        if (consideredPositions.Contains(target))
                        {
                            continue;
                        }

                        consideredPositions.Add(target);

                        if (target.HasEnemyFor(creature))
                        {
                            return kvp.Key;
                        }

                        if (target.HasCreature) // Must be a friend... (who's blocking us!)
                        {
                            continue;
                        }

                        newPositionsToTargetLocation[kvp.Key].AddRange(target.EnumerateAdjacentPositionsInReadingOrder());
                    }
                }

                positionsToTargetLocation = newPositionsToTargetLocation;
            }

            throw new NoMoveFoundException();
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

        [Fact]
        public void Creature_targets_lowest_hp_enemy_first()
        {
            var battle = CreateBattleFromInput("EGE");
            var attacker = battle.Creatures.Single(c => c.IsGoblin);
            var weakTarget = battle.Creatures.Single(c => c.Position.Point.X == 2);
            weakTarget.HitPoints -= 1;

            var result = attacker.FirstOrDefaultTarget();

            Assert.Equal(weakTarget, result);
        }

        [Fact]
        public void Creature_targets_reading_order_enemy_first()
        {
            var battle = CreateBattleFromInput("EGE");
            var attacker = battle.Creatures.Single(c => c.IsGoblin);
            var leftTarget = battle.Creatures.Single(c => c.Position.Point.X == 0);

            var result = attacker.FirstOrDefaultTarget();

            Assert.Equal(leftTarget, result);
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
                this.Position.Creature = null;
                this.Position = other;
                other.Creature = this;
            }

            public bool CanAttack() =>
                true == Position.Up?.Creature?.IsEnemyFor(this)
                || true == Position.Left?.Creature?.IsEnemyFor(this)
                || true == Position.Right?.Creature?.IsEnemyFor(this)
                || true == Position.Down?.Creature?.IsEnemyFor(this);

            public Creature FirstOrDefaultTarget()
            {
                return Position
                    .EnumerateAdjacentCreatures()
                    .Where(c => c.IsEnemyFor(this))
                    .OrderBy(c => c.HitPoints)
                    .ThenBy(c => c.Position.Point.Y)
                    .ThenBy(c => c.Position.Point.X)
                    .FirstOrDefault();
            }

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

            public List<Position> EnumerateAdjacentPositionsInReadingOrder() =>
                new [] { Up, Left, Right, Down }.Where(p => p != null).ToList();

            public List<Creature> EnumerateAdjacentCreatures() =>
                new[] { Up, Left, Right, Down }.Select(p => p?.Creature).Where(c => c != null).ToList();

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

            public FightResult Fight(Creature attacker, Creature target)
            {
                if (!attacker.IsEnemyFor(target))
                {
                    throw new InvalidOperationException("Friendly fire forbidden!");
                }

                target.HitPoints -= attacker.AttackPower;

                if (target.HitPoints <= 0)
                {
                    Creatures.Remove(target);
                    target.Position.Creature = null;
                    return FightResult.Death;
                }

                return FightResult.JustPain;
            }

            public bool IsOver() => Creatures.All(c => c.IsGoblin) || Creatures.All(c => c.IsElf);

            public int GetHitPointsLeft() => Creatures.Sum(x => x.HitPoints);

            public int GetScoreFor(int rounds)
            {
                return rounds * GetHitPointsLeft();
            }
        }

        public enum FightResult { JustPain, Death }
        
        private void OutputGrid(Battle battle, bool includeCreatures = false)
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

            if (includeCreatures)
            {
                var creatures = battle.Creatures
                    .OrderBy(c => c.Position.Point.Y)
                    .ThenBy(c => c.Position.Point.X);

                foreach (var creature in creatures)
                {
                    output.WriteLine(creature.ToString());
                }
            }
        }
    }
}
