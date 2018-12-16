using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

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

        [Fact]
        public void Solution_1_movement_example()
        {
            // Mainly meant to check the test output for movements, the answer was
            // grabbed from my own algorithm, not provided by AoC.
            Assert.Equal(27828, Solve1(@"
                #########
                #G..G..G#
                #.......#
                #.......#
                #G..E..G#
                #.......#
                #.......#
                #G..G..G#
                #########
            "));
        }

        // Not: 251877
        // Not: 244860 ("too low")
        // Not: 247277 ("too low", of course) after disabling the "IsDead => continue" option
        // Not: 247192 (after sneakily doing round++ before calculating score)
        // Not: 244860 (just tried it again...)
        // Not: 245544 (with search depth 20 and no "consideredPoints" set at all)
        // Not: 275520 (with the sort-attack-order on Y position disabled)
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(0, Solve1(puzzleInput));

        private const int StartingHitPoints = 200;
        private const int DefaultAttackPower = 3;

        public int Solve1(string input)
        {
            var battle = new Battle(input);
            output.WriteLine("Battle is starting...");
            OutputGrid(battle);

            for (int numberOfCompletedRounds = 0; numberOfCompletedRounds < 1_000_000; numberOfCompletedRounds++)
            {
                var creaturesInActingOrder = battle.GetCreaturesInActingOrder();

                var someCreatureActed = false; // To detect bugs

                foreach (var creature in creaturesInActingOrder)
                {
                    if (creature.IsDead) continue;

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

                    if (!creature.CanAttack())
                    {
                        try
                        {
                            var optimalMove = GetOptimalMoveFor(creature, battle);
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
                            output.WriteLine("");
                            output.WriteLine($"Killing blow at {numberOfCompletedRounds} for {target}!");
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

            throw new Exception("No outcome found.");
        }

        [Fact]
        public void GetOptimalMoveFor_aoc_scenario_1()
        {
            var battle = new Battle(@"
                #######
                #E..G.#
                #...#.#
                #.G.#G#
                #######
            ");
            var attacker = battle.Creatures.Single(c => c.IsElf);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[2, 1], result);
        }

        [Fact]
        public void GetOptimalMoveFor_aoc_scenario_2()
        {
            var battle = new Battle(@"
                #######
                #.E...#
                #.....#
                #...G.#
                #######
            ");
            var attacker = battle.Creatures.Single(c => c.IsElf);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[3, 1], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_1()
        {
            var battle = new Battle("E.G");
            var attacker = battle.Creatures.Single(c => c.IsElf);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[1, 0], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_2()
        {
            var battle = new Battle(@"
                ######
                #E#GE#
                #....#
                ######
            ");
            var attacker = battle.Creatures.Single(c => c.Position.Point.X == 1);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[1, 2], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_3a()
        {
            var battle = new Battle(@"
                #########
                #.......#
                #...#.G.#
                #...#...#
                #.E.#...#
                #.......#
                #########
            ");
            var attacker = battle.Creatures.Single(c => c.Position.Point.Y == 2);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[6, 1], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_3b()
        {
            var battle = new Battle(@"
                #########
                #.......#
                #.E.#...#
                #...#...#
                #...#.G.#
                #.......#
                #########
            ");
            var attacker = battle.Creatures.Single(c => c.Position.Point.Y == 2);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[2, 1], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_3c()
        {
            var battle = new Battle(@"
                #########
                #.......#
                #...#.G.#
                #...#...#
                #.E.#...#
                #.......#
                #########
            ");
            var attacker = battle.Creatures.Single(c => c.IsElf);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[2, 3], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_4()
        {
            var battle = new Battle(@"
                #########
                #.......#
                #...#.G.#
                #..E#...#
                #.E.#...#
                #.......#
                #########
            ");
            var attacker = battle.Creatures.Single(c => c.Position.Point.Y == 3);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[3, 2], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_5()
        {
            var battle = new Battle(@"
                #########
                #.......#
                #...#.G.#
                #..E#...#
                #..E#...#
                #...#...#
                #########
            ");
            var attacker = battle.Creatures.Single(c => c.Position.Point.Y == 4);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[2, 4], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_6()
        {
            var battle = new Battle(@"
                #####
                #E.G#
                #.G.#
                #####
            ");
            var attacker = battle.Creatures.Single(c => c.IsElf);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[2, 1], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_7()
        {
            var battle = new Battle(@"
                ###########
                #......G..#
                #.........#
                #....##G..#
                #.E.##....#
                #...##....#
                #...##....#
                ###########
            ");
            var attacker = battle.Creatures.Single(c => c.Position.Point.Y == 4);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[2, 3], result);
        }

        [Fact]
        public void GetOptimalMoveFor_scenario_8()
        {
            var battle = new Battle(@"
                #########
                #G..E..G#
                #########
            ");
            var attacker = battle.Creatures.Single(c => c.Position.Point.X == 4);
            var result = GetOptimalMoveFor(attacker, battle);
            Assert.Equal(battle.Grid[5, 1], result);
        }

        public class NoMoveFoundException : Exception
        { }

        private static Position GetOptimalMoveFor(Creature creature, Battle battle)
        {
            var possibleMoves = new Dictionary<Position, int>();
            if (creature.Position.Up?.HasCreature == false) possibleMoves.Add(creature.Position.Up, 0);
            if (creature.Position.Left?.HasCreature == false) possibleMoves.Add(creature.Position.Left, 1);
            if (creature.Position.Right?.HasCreature == false) possibleMoves.Add(creature.Position.Right, 2);
            if (creature.Position.Down?.HasCreature == false) possibleMoves.Add(creature.Position.Down, 3);

            var targetsByDirection = new Dictionary<int, ISet<Position>>
            {
                { 0, new HashSet<Position>() },
                { 1, new HashSet<Position>() },
                { 2, new HashSet<Position>() },
                { 3, new HashSet<Position>() },
            };

            foreach (var pos in battle.Creatures.Where(c => c.IsEnemyFor(creature)).Select(c => c.Position))
            {
                if (pos.Up?.HasCreature == false) targetsByDirection[0].Add(pos.Up);
                if (pos.Left?.HasCreature == false) targetsByDirection[1].Add(pos.Left);
                if (pos.Right?.HasCreature == false) targetsByDirection[2].Add(pos.Right);
                if (pos.Down?.HasCreature == false) targetsByDirection[3].Add(pos.Down);
            }

            var visited = new HashSet<Position>();
            var exhausted = false;

            while (!exhausted)
            {
                exhausted = true;

                for (int i = 0; i < 4; i++)
                {
                    var newTargets = new HashSet<Position>();

                    Position bestMove = null;
                    int bestROrder = int.MaxValue;

                    foreach (var pos in targetsByDirection[i])
                    {
                        if (visited.Contains(pos)) continue;

                        visited.Add(pos);
                        exhausted = false;

                        if (possibleMoves.ContainsKey(pos) && possibleMoves[pos] < bestROrder)
                        {
                            bestMove = pos;
                            bestROrder = possibleMoves[pos];
                        }

                        if (pos.Up?.HasCreature == false) newTargets.Add(pos.Up);
                        if (pos.Left?.HasCreature == false) newTargets.Add(pos.Left);
                        if (pos.Right?.HasCreature == false) newTargets.Add(pos.Right);
                        if (pos.Down?.HasCreature == false) newTargets.Add(pos.Down);
                    }

                    targetsByDirection[i] = newTargets;

                    if (bestMove != null) return bestMove;
                }
            }

            throw new NoMoveFoundException();
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
            var battle = new Battle("EGE");
            var attacker = battle.Creatures.Single(c => c.IsGoblin);
            var weakTarget = battle.Creatures.Single(c => c.Position.Point.X == 2);
            weakTarget.HitPoints -= 1;

            var result = attacker.FirstOrDefaultTarget();

            Assert.Equal(weakTarget, result);
        }

        [Theory]
        [InlineData("... \n EGE \n ...", 0, 1)]
        [InlineData("... \n EGE \n .E.", 0, 1)]
        [InlineData(".E. \n EGE \n .E.", 1, 0)]
        [InlineData("... \n .GE \n .E.", 2, 1)]
        [InlineData("... \n .G. \n .E.", 1, 2)]
        public void Creature_targets_reading_order_enemy_first(string input, int x, int y)
        {
            var battle = new Battle(input);
            var attacker = battle.Creatures.Single(c => c.IsGoblin);
            var expectedTarget = battle.Creatures.Single(c => c.Position.Point.X == x && c.Position.Point.Y == y);

            var result = attacker.FirstOrDefaultTarget();

            Assert.Equal(expectedTarget, result);
        }

        public class Creature
        {
            public bool IsGoblin { get; set; }
            public int HitPoints { get; set; } = StartingHitPoints;
            public int AttackPower => DefaultAttackPower;
            public Position Position { get; set; }

            public bool IsDead => HitPoints <= 0;

            public bool IsElf => !IsGoblin;
            public bool IsEnemyFor(Creature other) => other.IsGoblin != this.IsGoblin;

            public void MoveTo(Position other)
            {
                if (other.Creature != null) throw new InvalidOperationException("Cannot move into occupied position");
                if (other == this.Position) throw new InvalidOperationException("Can only move to a *new* position");
                if (other != Position.Up && other != Position.Left && other != Position.Right && other != Position.Down)
                    throw new InvalidOperationException("Can only move to a connected position");

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

            // Original:
            // public char Rune => IsGoblin ? 'G' : 'E';
            // For debugging:
            private static int goblinId = 'Z';
            private static char GetNextId() => (char)(goblinId = goblinId > 89 ? 65 : goblinId + 1);
            private char myGoblinId = GetNextId();
            public char Rune => IsGoblin ? myGoblinId : '@';

            public override string ToString() => $"{Rune} ({HitPoints} HP) at ({Position?.Point.X}, {Position?.Point.Y})";

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

            public char Rune => Creature?.Rune ?? '\u2003';

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

            public List<Position> EnumerateAdjacentPositionsInReadingOrder()
            {
                var list = new List<Position>();
                if (Up != null) list.Add(Up);
                if (Left != null) list.Add(Left);
                if (Right != null) list.Add(Right);
                if (Down != null) list.Add(Down);
                return list;
            }

            public List<Creature> EnumerateAdjacentCreatures()
            {
                var list = new List<Creature>();
                if (Up?.Creature != null) list.Add(Up.Creature);
                if (Left?.Creature != null) list.Add(Left.Creature);
                if (Right?.Creature != null) list.Add(Right.Creature);
                if (Down?.Creature != null) list.Add(Down.Creature);
                return list;
            }

            public override string ToString() => $"({Point.X}, {Point.Y}) with {Creature?.Rune ?? '.'}";
        }

        [Fact]
        public void ActingOrder_aoc_scenario()
        {
            var battle = new Battle(@"
                #######
                #.G.E.#
                #E.G.E#
                #.G.E.#
                #######
            ");
            var result = battle.GetCreaturesInActingOrder().Select(p => p.Position.Point).ToArray();
            Assert.Equal(
                new[] { new Point(2, 1), new Point(4, 1), new Point(1, 2), new Point(3, 2), new Point(5, 2), new Point(2, 3), new Point(4, 3) },
                result
            );
        }

        [Theory]
        [InlineData("E")]
        [InlineData("G")]
        [InlineData("EE")]
        [InlineData("GG")]
        [InlineData("EEE")]
        public void IsOver_true_when_only_one_kind_left(string input) => Assert.True(new Battle(input).IsOver());

        [Theory]
        [InlineData("EG")]
        [InlineData("GE")]
        [InlineData("EEG")]
        [InlineData("GGE")]
        public void IsOver_false_when_only_many_kind_left(string input) => Assert.False(new Battle(input).IsOver());

        [Theory]
        [InlineData("EG")]
        [InlineData("GE")]
        public void IsOver_true_when_only_dead_others(string input)
        {
            var battle = new Battle(input);
            battle.Creatures.First().HitPoints = -1;
            Assert.True(battle.IsOver());
        }

        public class Battle
        {
            public Battle(string input)
            {
                var data = input.SplitByNewline(shouldTrim: true);
                var width = data.First().Length;
                var height = data.Length;

                Grid = new Position[width, height];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (data[y][x] == '#') continue;

                        Creature creature = null;
                        if (data[y][x] == 'G') creature = Creature.CreateGoblin();
                        if (data[y][x] == 'E') creature = Creature.CreateElf();
                        if (creature != null) Creatures.Add(creature);

                        Grid[x, y] = new Position(x, y, creature);

                        if (y > 0 && Grid[x, y - 1] != null)
                        {
                            Grid[x, y].ConnectToPositionAbove(Grid[x, y - 1]);
                        }

                        if (x > 0 && Grid[x - 1, y] != null)
                        {
                            Grid[x, y].ConnectToPositionToTheLeft(Grid[x - 1, y]);
                        }
                    }
                }
            }

            public Position[,] Grid { get; }
            public ISet<Creature> Creatures { get; } = new HashSet<Creature>();

            public Creature[] GetCreaturesInActingOrder() => Creatures
                    .OrderBy(c => c.Position.Point.Y)
                    .ThenBy(c => c.Position.Point.X)
                    .ToArray();

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
                    target.Position = null;
                    return FightResult.Death;
                }

                return FightResult.JustPain;
            }

            public bool IsOver() => Creatures.All(c => c.IsGoblin || c.IsDead) || Creatures.All(c => c.IsElf || c.IsDead);
            public int GetHitPointsLeft() => Creatures.Where(c => !c.IsDead).Sum(x => x.HitPoints);
            public int GetScoreFor(int completedRounds) => completedRounds * GetHitPointsLeft();
        }

        public enum FightResult { JustPain, Death }
        
        private void OutputGrid(Battle battle, bool includeCreatures = false)
        {
            for (int y = 0; y < battle.Grid.GetLength(1); y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < battle.Grid.GetLength(0); x++)
                {
                    if (battle.Grid[x, y] == null) sb.Append('█');
                    else sb.Append(battle.Grid[x, y].Rune);
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
