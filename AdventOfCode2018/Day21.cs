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

        [Fact] public void Solution_1_test_real_input() => Assert.Equal(15615244, Solve1(puzzleInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(12963935, Solve2(puzzleInput));

        public long Solve1(string input)
        {
            return GetAnswer().First();
        }

        public long Solve2(string input)
        {
            return GetAnswer().Last();
        }

        [Theory]
        [InlineData(1092)]
        [InlineData(5285447)]
        [InlineData(15615244)]
        public void HandrolledProgram_still_working(int register0)
        {
            Assert.True(HandrolledProgram(register0, 100_000) > 0);
        }

        private ICollection<long> GetAnswer()
        {
            var chain = new List<long>();
            
            long reg4 = 0;
            long reg5 = 0;

            while (true)
            {
                reg4 = reg5 | 65536;
                reg5 = 15466939;

                reg5 = ((reg5 + (reg4 & 255)) * 65899) & 16777215;
                reg4 = reg4 >> 8;
                reg5 = ((reg5 + (reg4 & 255)) * 65899) & 16777215;
                reg4 = reg4 >> 8;
                reg5 = ((reg5 + (reg4 & 255)) * 65899) & 16777215;

                if (chain.Contains(reg5)) return chain;
                chain.Add(reg5);
            }
        }

        private int HandrolledProgram(long initForRegister0, int maxLoops = 1)
        {
            var counter = 0;
            long reg4 = 0;
            long reg5 = 0;

            while (true)
            {
                reg4 = reg5 | 65536;
                reg5 = 15466939;

                reg5 = ((reg5 + (reg4 & 255)) * 65899) & 16777215;
                reg4 = reg4 >> 8;
                reg5 = ((reg5 + (reg4 & 255)) * 65899) & 16777215;
                reg4 = reg4 >> 8;
                reg5 = ((reg5 + (reg4 & 255)) * 65899) & 16777215;

                if (counter++ > maxLoops) return -1;
                if (reg5 == initForRegister0) return counter;
            }

            throw new NoSolutionFoundException();
        }

        private int FindNumberOfInstructionsNeeded(int ipRegister, int[][] program, int maxLoops = 1000)
        {
            var counter = 0;

            long ip = 0;
            var registers = new long[] { 0, 0, 0, 0, 0, 0 };

            while (ip < program.Length)
            {
                registers[ipRegister] = ip;
                ElfCodeMachine.Doop(program[ip], registers);
                ip = registers[ipRegister];
                ip++;

                if (counter++ > maxLoops) throw new NoSolutionFoundException();
            }

            return counter;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(255)]
        [InlineData(256)]
        [InlineData(257)]
        [InlineData(1234)]
        [InlineData(1092)]
        [InlineData(5285447)]
        [InlineData(15615244)]
        [InlineData(16777215)]
        public void Sanity_check_on_division_by_256(int number)
        {
            Assert.Equal(number / 256, number >> 8);
        }

        [Fact] public void Sanity_check_on_integer_as_binary_16777215() => Assert.Equal(16777215, 0b1111_1111_1111_1111_1111_1111);
        [Fact] public void Sanity_check_on_integer_as_binary_65899() => Assert.Equal(65899, 0b1_0000_0001_0110_1011);
        [Fact] public void Sanity_check_on_integer_as_binary_65536() => Assert.Equal(65536, 0b1_0000_0000_0000_0000);
        [Fact] public void Sanity_check_on_integer_as_binary_255() => Assert.Equal(255, 0b1111_1111);

        [Theory]
        [InlineData(0, 0)]
        [InlineData(255, 0)]
        [InlineData(256, 1)]
        [InlineData(257, 1)]
        [InlineData(511, 1)]
        [InlineData(512, 2)]
        [InlineData(1234, 4)]
        public void Sanity_check_on_loop_logic(int reg4, int expected)
        {
            var reg = new long[] { 0, 0, 0, 0, reg4 };

            // New algorithm:
            reg[4] = reg[4] / 256;

            // For reference, the 'original' logic from the disassembly:
            //
            //reg[3] = 0;
            //while (true)
            //{
            //    if (((reg[3] + 1) * 256) > reg[4]) break;
            //    reg[3]++;
            //}
            //reg[4] = reg[3];

            Assert.Equal(expected, reg[4]);
        }
    }
}
