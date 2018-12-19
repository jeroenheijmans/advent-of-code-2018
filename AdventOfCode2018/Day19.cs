using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day19
    {
        private readonly ITestOutputHelper output;

        public Day19(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string testInput = @"
#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5
";

        public const string puzzleInput = @"
#ip 1
addi 1 16 1
seti 1 4 4
seti 1 1 2
mulr 4 2 5
eqrr 5 3 5
addr 5 1 1
addi 1 1 1
addr 4 0 0
addi 2 1 2
gtrr 2 3 5
addr 1 5 1
seti 2 4 1
addi 4 1 4
gtrr 4 3 5
addr 5 1 1
seti 1 1 1
mulr 1 1 1
addi 3 2 3
mulr 3 3 3
mulr 1 3 3
muli 3 11 3
addi 5 7 5
mulr 5 1 5
addi 5 18 5
addr 3 5 3
addr 1 0 1
seti 0 7 1
setr 1 3 5
mulr 5 1 5
addr 1 5 5
mulr 1 5 5
muli 5 14 5
mulr 5 1 5
addr 3 5 3
seti 0 7 0
seti 0 6 1
";
        private readonly ISet<int> JumpInstructions = new HashSet<int> { Day16.seti, Day16.setr, Day16.addi, Day16.addr };

        private readonly IDictionary<string, int> InstructionNames = new Dictionary<string, int>
        {
            { "addr", Day16.addr },
            { "addi", Day16.addi },
            { "mulr", Day16.mulr },
            { "muli", Day16.muli },
            { "banr", Day16.banr },
            { "bani", Day16.bani },
            { "borr", Day16.borr },
            { "bori", Day16.bori },
            { "setr", Day16.setr },
            { "seti", Day16.seti },
            { "gtir", Day16.gtir },
            { "gtri", Day16.gtri },
            { "gtrr", Day16.gtrr },
            { "eqir", Day16.eqir },
            { "eqri", Day16.eqri },
            { "eqrr", Day16.eqrr },
        };

        [Fact] public void Solution_1_test_example() => Assert.Equal(6, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(3224, Solve1(puzzleInput));

        // Not 3224 (too low - just guessed the same as the first puzzle)
        // Not 5 (guessed based on register 1 - what was I thinking!?)
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(-1, Solve2(puzzleInput));

        public int Solve1(string input)
        {
            return SolveInternal(input, isProductionMode: false);
        }

        public int Solve2(string input)
        {
            int reg0 = 1, reg2 = 0, reg3 = 0, reg4 = 0, reg5 = 0;

            goto seventeen; // Line 1

        one:
            reg4 = 1;

        two:
            reg2 = 1;

        three:
            reg5 = reg2 * reg4;

            if (reg5 != reg3)
            {
                reg0 = reg4 + reg0;
            }

            reg2++;

            if (reg2 != reg3)
            {
                goto three;
            }

            reg4++;

            if (reg4 <= reg3)
            {
                goto two;
            }

            goto exit; // Line 16

        seventeen:
            reg3 += reg2;
            reg3 *= reg3;
            reg3 *= 19;
            reg3 *= 11;
            reg5 += 7;
            reg5 *= 22;
            reg5 += 18;
            reg3 += reg5;
            
            if (reg0 == 0)
            {
                goto one;
            }
            else
            {
                reg5 = 27;
                reg5 *= 28;
                reg5 += 29;
                reg5 *= 30;
                reg5 *= 13;
                reg5 *= 32;
                reg3 += reg5;
                reg0 = 0;
                goto one;
            }

        exit:
            return reg0;
        }

        private int SolveInternal(string input, bool isProductionMode)
        {
            var data = input.SplitByNewline(shouldTrim: true);

            var ipRegister = int.Parse(data.First().Replace("#ip ", ""));
            var ip = 0;

            var program = data.Skip(1)
                .Select(line => line.Split())
                .Select(line =>
                {
                    var inst = new int[4];
                    inst[0] = InstructionNames[line[0]];
                    inst[1] = int.Parse(line[1]);
                    inst[2] = int.Parse(line[2]);
                    inst[3] = int.Parse(line[3]);
                    return inst;
                })
                .ToArray();

            var nrOfExecutionPerLine = new int[program.Length];

            var registers = new[] { isProductionMode ? 1 : 0, 0, 0, 0, 0, 0 };
            var i = 0;

            while (ip < program.Length)
            {
                if (i++ % 500_000 == 0) OutputRegisters(registers, i);
                if (i > 100_000_000) break; // throw new Exception("No answer found");

                nrOfExecutionPerLine[ip]++;

                registers[ipRegister] = ip;
                Day16.Doop(program[ip], registers);
                ip = registers[ipRegister];

                ip++;
            }

            output.WriteLine(string.Join(";", nrOfExecutionPerLine));

            OutputRegisters(registers);

            return registers[0];
        }

        private void OutputRegisters(int[] registers, int? i = null)
        {
            output.WriteLine($"{i};-----;{string.Join(";", registers)}");
        }
    }
}
