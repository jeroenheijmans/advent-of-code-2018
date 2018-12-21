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

        six:
            //  6:
            reg[4] = reg[5] | 65536;
            //  7:
            reg[5] = 15466939;
            //  8:
        eight:
            reg[3] = reg[4] & 255;
            //  9:  
            reg[5] = reg[5] + reg[3];
            // 10:
            reg[5] = reg[5] & 16777215;
            // 11:
            reg[5] = reg[5] * 65899;
            // 12:
            reg[5] = reg[5] & 16777215;
            // 13:
            reg[3] = (256 > reg[4]) ? 1 : 0;
            // 14:  ip = 15 + reg[3]
            if (reg[3] == 0)
            {
                // 15:  ip = 17
            }
            else
            {
                // 16:  ip = 28
                goto twentyeight;
            }
            // 17:
            reg[3] = 0;
            // 18:
        eightteen:
            reg[1] = reg[3] + 1;
            // 19:
            reg[1] = reg[1] * 256;
            // 20:
            reg[1] = (reg[1] > reg[4]) ? 1 : 0;
            // 21:  ip = 22 + reg[1]
            if (reg[1] == 0)
            {
                // 22:  ip = 24
            }
            else
            {
                // 23:  ip = 26
                goto twentysix;
            }
            // 24:
            reg[3]++;
            // 25:  ip = 18
            goto eightteen;
            // 26:
        twentysix:
            reg[4] = reg[3];
            // 27:  ip = 8
            goto eight;
            // 28:
        twentyeight:
            reg[3] = (reg[5] == reg[0]) ? 1 : 0;
            // 29:  ip = 30 + reg[3]
            if (reg[3] == 0)
            {
                // 30:  ip = 6
                goto six;
            }

            return initForRegister0;
        }
    }
}
