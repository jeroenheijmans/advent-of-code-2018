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
            throw new NoSolutionFoundException();
        }

        private int HandrolledProgram(int initForRegister0)
        {
            var reg = new long[] { initForRegister0, 0, 0, 0, 0, 0 };

            do
            { 
                reg[4] = reg[5] | 0b1_0000_0000_0000_0000;
                reg[5] = 15466939;

                while (true)
                {
                    reg[5] = reg[5] + (reg[4] & 0b1111_1111);
                    reg[5] = reg[5] & 0b1111_1111_1111_1111_1111_1111; 
                    reg[5] = reg[5] * 65899;
                    reg[5] = reg[5] & 0b1111_1111_1111_1111_1111_1111;

                    if (reg[4] < 256) break;

                    reg[4] = reg[4] / 256;
                }

            } while (reg[5] != reg[0]);

            return initForRegister0;
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
