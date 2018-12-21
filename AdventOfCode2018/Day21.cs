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
    public partial class Day21
    {
        private readonly ITestOutputHelper output;

        public Day21(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string puzzleInput = @"
#ip 2
seti 123 0 5
bani 5 456 5
eqri 5 72 5
addr 5 2 2
seti 0 0 2
seti 0 4 5
bori 5 65536 4
seti 15466939 9 5
bani 4 255 3
addr 5 3 5
bani 5 16777215 5
muli 5 65899 5
bani 5 16777215 5
gtir 256 4 3
addr 3 2 2
addi 2 1 2
seti 27 8 2
seti 0 7 3
addi 3 1 1
muli 1 256 1
gtrr 1 4 1
addr 1 2 2
addi 2 1 2
seti 25 2 2
addi 3 1 3
seti 17 7 2
setr 3 7 4
seti 7 3 2
eqrr 5 0 3
addr 3 2 2
seti 5 9 2
";

        [Fact] public void Solution_1_test_real_input() => Assert.Equal(-1, Solve1(puzzleInput));

        public int Solve1(string input)
        {
            var initForRegister0 = 0;

            var reg = new[] { initForRegister0, 0, 0, 0, 0, 123 };

        one:
            reg[5] = reg[5] & 456;
            if (reg[5] == 72) reg[5] = 0;
            else goto one;

        six:
            reg[4] = reg[5] | 65536;
            reg[5] = 15466939;

        eight:
            reg[3] = reg[4] & 255;
            reg[5] = reg[5] + reg[3];
            reg[5] = reg[5] & 16777215;
            reg[5] = reg[5] * 65899;
            reg[5] = reg[5] & 16777215;

            if (256 > reg[4]) goto twentyeight;
            else goto seventeen;

        seventeen:
            reg[3] = 0;

        eighteen:
            reg[1] = (reg[3] + 1) * 256;

            if (reg[1] > reg[4]) goto twentysix;
            else goto twentyfour;

        twentyfour:
            reg[3]++;
            goto eighteen;

        twentysix:
            reg[4] = reg[3];
            goto eight;

        twentyeight:
            if (reg[5] == reg[0])
            {
                throw new Exception("Program halts, but we have no clue anymore how many instructions it was");
            }
            else
            {
                goto six;
            }

            throw new NoSolutionFoundException();
        } 

        public int Solve1BruteForce(string input)
        {
            var data = input.SplitByNewline(shouldTrim: true);

            var ipRegister = int.Parse(data.First().Replace("#ip ", ""));

            var program = data.Skip(1)
                .Select(line => line.Split())
                .Select(line =>
                {
                    var inst = new int[4];
                    inst[0] = Day16.OpCodesByName[line[0]];
                    inst[1] = int.Parse(line[1]);
                    inst[2] = int.Parse(line[2]);
                    inst[3] = int.Parse(line[3]);
                    return inst;
                })
                .ToArray();

            for (int i = 0; i < 10_000; i++)
            {
                var result = SolveInternal(program, ipRegister, initForRegister0: i);
                if (result >= 0) return i;
            }

            throw new NoSolutionFoundException();
        }

        private int SolveInternal(int[][] program, int ipRegister, int initForRegister0, int maxExecutions = 1_000)
        {
            var ip = 0;
            var registers = new[] { initForRegister0, 0, 0, 0, 0, 0 };
            var counter = 0;

            while (ip < program.Length)
            {
                registers[ipRegister] = ip;
                Day16.Doop(program[ip], registers);
                ip = registers[ipRegister];

                ip++;

                if (counter++ > maxExecutions) return -1;
            }

            output.WriteLine($"REGISTERS after {counter} executions: {string.Join(";", registers)}");

            return registers[0];
        }
    }
}
