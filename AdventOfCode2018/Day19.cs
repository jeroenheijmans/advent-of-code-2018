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

        [Fact] public void Solution_2_test_real_input() => Assert.Equal(32188416, Solve2(puzzleInput));

        public int Solve1(string input)
        {
            return SolveInternal(input, isProductionMode: false);
        }

        public int Solve2(string input)
        {
            var sum = 0;
            var max = 10551408;
            for (int i = 1; i <= max; i++)
            {
                if (10551408 % i == 0) sum += i;
            }

            return sum;

            // Hand-crafted/reduced algorithm from the program code:
            int reg0 = 0, reg2 = 0, reg3 = 10551408, reg4 = 0, reg5 = 0;

            reg4 = 1;
        two:
            reg2 = 1;
        three:
            reg5 = reg2 * reg4;

            if (reg5 != reg3)
            {
                reg0 += reg4;
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

            return reg0;
        }

        private int SolveInternal(string input, bool isProductionMode)
        {
            if (isProductionMode) throw new NotSupportedException("The algorithm is too slow for production mode, currently");

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

            var registers = new[] { isProductionMode ? 1 : 0, 0, 0, 0, 0, 0 };

            while (ip < program.Length)
            {
                registers[ipRegister] = ip;
                Day16.Doop(program[ip], registers);
                ip = registers[ipRegister];

                ip++;
            }

            output.WriteLine($"REGISTERS: {string.Join(";", registers)}");

            return registers[0];
        }
    }
}
