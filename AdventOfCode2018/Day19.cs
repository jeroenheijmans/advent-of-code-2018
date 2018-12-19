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

        public const int addr = 0;
        public const int addi = 1;
        public const int mulr = 2;
        public const int muli = 3;
        public const int banr = 4;
        public const int bani = 5;
        public const int borr = 6;
        public const int bori = 7;
        public const int setr = 8;
        public const int seti = 9;
        public const int gtir = 10;
        public const int gtri = 11;
        public const int gtrr = 12;
        public const int eqir = 13;
        public const int eqri = 14;
        public const int eqrr = 15;

        private readonly ISet<int> JumpInstructions = new HashSet<int> { seti, setr, addi, addr };

        private readonly IDictionary<string, int> InstructionNames = new Dictionary<string, int>
        {
            { "addr", addr },
            { "addi", addi },
            { "mulr", mulr },
            { "muli", muli },
            { "banr", banr },
            { "bani", bani },
            { "borr", borr },
            { "bori", bori },
            { "setr", setr },
            { "seti", seti },
            { "gtir", gtir },
            { "gtri", gtri },
            { "gtrr", gtrr },
            { "eqir", eqir },
            { "eqri", eqri },
            { "eqrr", eqrr },
        };


        [Fact] public void Solution_1_test_example() => Assert.Equal(6, Solve1(testInput));

        // NOT 1008 ("That's not the right answer; your answer is too low. Curiously, it's the right answer for someone else; you're either logged in to the wrong account, unlucky, or cheating. ")
        // Whuuuutt!? Off by one?
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(-1, Solve1(puzzleInput));

        public int Solve1(string input)
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

            var registers = new[] { 0, 0, 0, 0, 0, 0 };
            var i = 0;

            while (ip < program.Length)
            {
                if (i++ < 10) OutputRegisters(registers);
                if (i == 10) output.WriteLine("...");

                if (JumpInstructions.Contains(program[ip][0]))
                {
                    registers[ipRegister] = ip;
                }

                Doop(program[ip], registers);

                if (JumpInstructions.Contains(program[ip][0]))
                {
                    ip = registers[ipRegister];
                }

                ip++;
            }

            OutputRegisters(registers);

            return registers[0];
        }

        private void Doop(int[] instructions, int[] registers)
        {
            int a = instructions[1], b = instructions[2], c = instructions[3];

            switch (instructions[0])
            {
                case addr: // stores into register C the result of adding register A and register B
                    registers[c] = registers[a] + registers[b];
                    break;
                case addi: // stores into register C the result of adding register A and value B
                    registers[c] = registers[a] + b;
                    break;

                case mulr: // stores into register C the result of multiplying register A and register B
                    registers[c] = registers[a] * registers[b];
                    break;
                case muli: // stores into register C the result of multiplying register A and value B
                    registers[c] = registers[a] * b;
                    break;

                case banr: // stores into register C the result of the bitwise AND of register A and register B
                    registers[c] = registers[a] & registers[b];
                    break;
                case bani: // stores into register C the result of the bitwise AND of register A and value B
                    registers[c] = registers[a] & b;
                    break;

                case borr: // stores into register C the result of the bitwise OR of register A and register B
                    registers[c] = registers[a] | registers[b];
                    break;
                case bori: // stores into register C the result of the bitwise OR of register A and value B
                    registers[c] = registers[a] | b;
                    break;

                case setr: // copies the contents of register A into register C. (Input B is ignored.)
                    registers[c] = registers[a];
                    break;
                case seti: // stores value A into register C. (Input B is ignored.)
                    registers[c] = a;
                    break;

                case gtir: // sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0
                    registers[c] = (a > registers[b]) ? 1 : 0;
                    break;
                case gtri: // sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0
                    registers[c] = (registers[a] > b) ? 1 : 0;
                    break;
                case gtrr: // sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0
                    registers[c] = (registers[a] > registers[b]) ? 1 : 0;
                    break;

                case eqir: // sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0
                    registers[c] = (a == registers[b]) ? 1 : 0;
                    break;
                case eqri: // sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0
                    registers[c] = (registers[a] == b) ? 1 : 0;
                    break;
                case eqrr: // sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0
                    registers[c] = (registers[a] == registers[b]) ? 1 : 0;
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private void OutputRegisters(int[] registers)
        {
            output.WriteLine($"REGISTERS: [{string.Join(", ", registers)}]");
        }
    }
}
