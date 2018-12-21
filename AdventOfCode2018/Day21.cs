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
    public class Day21
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
            /*  #ip 2

                seti   123                   5                         0:  reg[5] = 123
                bani   5          456        5                         1:  reg[5] = reg[5] & 456
                eqri   5          72         5                         2:  reg[5] = (reg[5] == 72) ? 1 : 0
                addr   5          2          2    JUMP_TO_04_OR_05     3:  ip = 3 + reg[5]
                seti   0                     2    JUMP_TO_01           4:  ip = 0
                seti   0                     5                         5:  reg[5] = 0
                bori   5          65536      4                         6:  reg[4] = reg[5] | 65536
                seti   15466939              5                         7:  reg[5] = 15466939
                bani   4          255        3                         8:  reg[3] = reg[4] & 255
                addr   5          3          5                         9:  reg[5] = reg[5] + reg[3]
                bani   5          16777215   5                        10:  reg[5] = reg[5] & 16777215
                muli   5          65899      5                        11:  reg[5] = reg[5] * 65899
                bani   5          16777215   5                        12:  reg[5] = reg[5] & 16777215
                gtir   256        4          3                        13:  reg[3] = (256 > reg[4]) ? 1 : 0
                addr   3          2          2    JUMP_TO_15_OR_16    14:  ip = 14 + reg[3]
                addi   2          1          2    JUMP_TO_17          15:  ip = 16
                seti   27                    2    JUMP_TO_28          16:  ip = 27
                seti   0                     3                        17:  reg[3] = 0
                addi   3          1          1                        18:  reg[1] = reg[3] + 1
                muli   1          256        1                        19:  reg[1] = reg[1] * 256
                gtrr   1          4          1                        20:  reg[1] = (reg[1] > reg[4]) ? 1 : 0
                addr   1          2          2    JUMP_TO_22_OR_23    21:  ip = 21 + reg[1]
                addi   2          1          2    JUMP_TO_24          22:  ip = 23
                seti   25                    2    JUMP_TO_26          23:  ip = 25
                addi   3          1          3                        24:  reg[3]++
                seti   17                    2    JUMP_TO_18          25:  ip = 17
                setr   3                     4                        26:  reg[4] = reg[3]
                seti   7                     2    JUMP_TO_08          27:  ip = 7
                eqrr   5          0          3                        28:  REG[3] = (REG[5] == REG[0]) ? 1 : 0
                addr   3          2          2    JUMP_TO_30_OR_EXIT  29:  ip = 29 + reg[3]
                seti   5                     2    JUMP_TO_06          30:  ip = 5
            */


            // To answer:
            // > What is the lowest non-negative integer value for register 0 that causes the 
            // > program to halt after executing the fewest instructions?
            return -1;
        }
    }
}
