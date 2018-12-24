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
    public class Day24
    {
        private readonly ITestOutputHelper output;

        public Day24(ITestOutputHelper output)
        {
            this.output = output;
        }

        private const string testInput = @"
Immune System:
17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2
989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3

Infection:
801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4
";

        private const string puzzleInput = @"
Immune System:
1432 units each with 7061 hit points (immune to cold; weak to bludgeoning) with an attack that does 41 slashing damage at initiative 17
3387 units each with 9488 hit points (weak to bludgeoning) with an attack that does 27 slashing damage at initiative 20
254 units each with 3249 hit points (immune to fire) with an attack that does 89 cold damage at initiative 1
1950 units each with 8201 hit points with an attack that does 39 fire damage at initiative 15
8137 units each with 3973 hit points (weak to slashing; immune to radiation) with an attack that does 4 radiation damage at initiative 6
4519 units each with 7585 hit points (weak to fire) with an attack that does 15 radiation damage at initiative 8
763 units each with 7834 hit points (immune to radiation, slashing, cold; weak to fire) with an attack that does 91 radiation damage at initiative 18
935 units each with 10231 hit points (immune to slashing, cold) with an attack that does 103 bludgeoning damage at initiative 12
4557 units each with 7860 hit points (immune to slashing) with an attack that does 15 slashing damage at initiative 11
510 units each with 7363 hit points (weak to fire, radiation) with an attack that does 143 fire damage at initiative 5

Infection:
290 units each with 29776 hit points (weak to cold, radiation) with an attack that does 204 bludgeoning damage at initiative 16
7268 units each with 14114 hit points (immune to radiation; weak to bludgeoning) with an attack that does 3 bludgeoning damage at initiative 19
801 units each with 5393 hit points with an attack that does 13 slashing damage at initiative 13
700 units each with 12182 hit points with an attack that does 29 cold damage at initiative 4
531 units each with 16607 hit points (immune to slashing) with an attack that does 53 bludgeoning damage at initiative 10
23 units each with 24482 hit points (weak to cold, fire) with an attack that does 2095 bludgeoning damage at initiative 7
8025 units each with 43789 hit points (weak to cold; immune to radiation) with an attack that does 8 radiation damage at initiative 9
1405 units each with 53896 hit points with an attack that does 70 slashing damage at initiative 14
566 units each with 7820 hit points (immune to cold) with an attack that does 26 cold damage at initiative 2
1641 units each with 7807 hit points (weak to fire; immune to slashing, bludgeoning) with an attack that does 7 radiation damage at initiative 3";

        [Fact] public void Solution_1_test_example_1() => Assert.Equal(5216, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(10723, Solve1(puzzleInput));

        [Fact] public void Solution_2_test_example_1() => Assert.Equal(51, Solve2(testInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(-1, Solve2(puzzleInput));

        public int Solve1(string input)
        {
            var (score, infectionLoss) = SimulateCombatAndGetScore(input, maxRoundsLogged: 10);
            return score;
        }

        public int Solve2(string input)
        {
            const int maxRoundsLogged = 0;

            for (int boost = 0; boost < 10000; boost++)
            {
                var (score, infectionLoss) = SimulateCombatAndGetScore(input, maxRoundsLogged, boost);

                if (infectionLoss == true) return score;
            }

            throw new NoSolutionFoundException();
        }

        private (int score, bool infectionLoss) SimulateCombatAndGetScore(string input, int maxRoundsLogged, int immuneSystemBoost = 0)
        {
            var combat = new Combat { Squads = ParseSquadsFromInput(input, immuneSystemBoost) };
            var round = 0;

            while (!combat.IsOver())
            {
                var deadlocked = true;

                if (round++ < maxRoundsLogged)
                {
                    output.WriteLine("");
                    output.WriteLine($"---------------------------- round {round} -----------------------------");
                    foreach (var squad in combat.Squads) output.WriteLine(squad.ToString());
                    output.WriteLine("");
                }

                // Target selection
                var targets = new Dictionary<Squad, Squad>();
                var potentialTargets = combat.GetLivingSquads().ToHashSet();
                var squadsInOrder = combat.GetLivingSquads()
                    .OrderByDescending(s => s.EffectivePower)
                    .ThenByDescending(s => s.Initiative)
                    .ToArray();

                foreach (var squad in squadsInOrder)
                {
                    var enemy = potentialTargets
                        .Where(s => squad.IsEnemeyFor(s))
                        .Where(s => squad.PotentialDamageTo(s) > 0)
                        .OrderByDescending(s => squad.PotentialDamageTo(s))
                        .ThenByDescending(s => s.EffectivePower)
                        .ThenByDescending(s => s.Initiative)
                        .FirstOrDefault();

                    targets[squad] = enemy;

                    if (enemy == null) continue;

                    potentialTargets.Remove(enemy);

                    if (round < maxRoundsLogged)
                    {
                        output.WriteLine($"{squad.Type} {squad.Id} would deal squad {enemy.Id} {squad.PotentialDamageTo(enemy)} damage");
                    }
                }

                if (round < maxRoundsLogged) output.WriteLine("");

                // Attacking
                squadsInOrder = combat.GetLivingSquads()
                    .OrderByDescending(s => s.Initiative)
                    .ToArray();

                foreach (var squad in squadsInOrder)
                {
                    if (squad.Units <= 0) continue;
                    if (targets[squad] == null) continue;

                    var result = squad.Fight(targets[squad]);
                    deadlocked = false;

                    if (round < maxRoundsLogged)
                    {
                        output.WriteLine($"{squad.Type} {squad.Id} attacks {targets[squad].Id} killing {result.UnitsKilled} units ({result.Damage} damage) {(result.IsElimination ? "ELIMINATION!" : "")}");
                    }
                }

                if (deadlocked) throw new NoSolutionFoundException("Deadlocked!");
            }

            return (combat.CalculateScore(), !combat.GetLivingSquads().Any(s => s.IsInfection));
        }

        private static ISet<Squad> ParseSquadsFromInput(string input, int immuneSystemBoost)
        {
            var lines = input.SplitByNewline(shouldTrim: true);

            var squads = new HashSet<Squad>();
            var isImmuneSystem = true;
            var id = 1;

            foreach (var line in lines)
            {
                if (line.StartsWith("Immune")) { isImmuneSystem = true; continue; }
                if (line.StartsWith("Infection")) { isImmuneSystem = false; id = 1; continue; }

                Squad squad = ParseSquadFromLine(line, isImmuneSystem, immuneSystemBoost);

                squad.Id = id++;
                squads.Add(squad);
            }

            return squads;
        }

        private static Squad ParseSquadFromLine(string line, bool isImmuneSystem, int immuneSystemBoost = 0)
        {
            const string pattern = @"(\d+) \D+ (\d+) hit points (.*)with .+ does (\d+) (\w+) damage at initiative (\d+)";

            var groups = line.SubGroups(pattern);

            var squad = new Squad
            {
                IsImmuneSystem = isImmuneSystem,
                Units = int.Parse(groups[0]),
                HitPoints = int.Parse(groups[1]),
                // Immunities = ...,
                AttackPower = int.Parse(groups[3]) + (isImmuneSystem ? immuneSystemBoost : 0),
                AttackType = groups[4],
                Initiative = int.Parse(groups[5]),
            };

            foreach (var vuln in groups[2].Replace("(", "").Replace(")", "").Split("; "))
            {
                if (vuln.StartsWith("immune to "))
                {
                    squad.Immunities = vuln.Replace("immune to ", "").Split(", ").Select(v => v.Trim()).ToHashSet();
                }

                if (vuln.StartsWith("weak to "))
                {
                    squad.Weaknesses = vuln.Replace("weak to ", "").Split(", ").Select(v => v.Trim()).ToHashSet();
                }
            }

            return squad;
        }

        public class Combat
        {
            public ISet<Squad> Squads { get; set; }

            public ISet<Squad> GetLivingSquads() => Squads.Where(s => s.Units > 0).ToHashSet();
            public bool IsOver() => GetLivingSquads().Select(s => s.IsImmuneSystem).Distinct().Count() == 1;
            public int CalculateScore() => GetLivingSquads().Sum(s => s.Units);
        }

        [Fact]
        public void Squad_fight_kills_only_whole_units()
        {
            var attacker = new Squad { IsImmuneSystem = false, Units = 1, AttackType = "fire", AttackPower = 75 };
            var defender = new Squad { IsImmuneSystem = true, Units = 10, HitPoints = 10 };
            Assert.Equal(75, attacker.PotentialDamageTo(defender));
            attacker.Fight(defender);
            Assert.Equal(3, defender.Units);
        }

        [Fact]
        public void Squad_fight_regression_test_1()
        {
            var defender = ParseSquadFromLine(@"17 units each with 5390 hit points (weak to radiation, bludgeoning) with n attack that does 4507 fire damage at initiative 2", true);
            var attacker = ParseSquadFromLine(@"801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1", false);

            var result = attacker.Fight(defender);

            Assert.Equal(17, result.UnitsKilled);
        }

        public class Squad
        {
            public int Id { get; set; }
            public bool IsImmuneSystem { get; set; }
            public bool IsInfection => !IsImmuneSystem;
            public string Type => IsImmuneSystem ? "Immune System" : "Infection";
            public int Units { get; set; }
            public int HitPoints { get; set; }
            public ISet<string> Immunities { get; set; } = new HashSet<string>();
            public ISet<string> Weaknesses { get; set; } = new HashSet<string>();
            public int AttackPower { get; set; }
            public string AttackType { get; set; }
            public int Initiative { get; set; }

            public int EffectivePower => Units * AttackPower;

            public bool IsEnemeyFor(Squad other) => IsImmuneSystem != other.IsImmuneSystem;

            public FightResult Fight(Squad other)
            {
                if (!IsEnemeyFor(other)) throw new InvalidOperationException("No friendly fire allowed!");
                var damage = PotentialDamageTo(other);
                var result = new FightResult
                {
                    Damage = damage,
                    UnitsKilled = Math.Min(other.Units, damage / other.HitPoints),
                };
                result.IsElimination = result.UnitsKilled == other.Units;
                other.Units -= result.UnitsKilled;
                return result;
            }

            public int PotentialDamageTo(Squad other)
            {
                if (other.Immunities.Contains(AttackType)) return 0;
                if (other.Weaknesses.Contains(AttackType)) return 2 * EffectivePower;
                return EffectivePower;
            }

            public override string ToString()
            {
                return $"Squad {Id} ({Type}) with {Units} units of {HitPoints} HP and {AttackPower} AP (effectively {EffectivePower}). Initiative: {Initiative}.";
            }
        }

        public class FightResult
        {
            public int Damage { get; set; }
            public int UnitsKilled { get; set; }
            public bool IsElimination { get; set; }
        }
    }
}
