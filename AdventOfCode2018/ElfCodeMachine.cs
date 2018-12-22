using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    public class ElfCodeMachine
    {
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

        public static readonly IReadOnlyDictionary<string, int> OpCodesByName = new Dictionary<string, int>
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

        public static (int ipRegister, int[][] program) ParseInputToProgram(string input)
        {
            var data = input.SplitByNewline(shouldTrim: true);

            var ipRegister = int.Parse(data.First().Replace("#ip ", ""));

            var program = data.Skip(1)
                .Select(line => line.Split())
                .Select(line =>
                {
                    var inst = new int[4];
                    inst[0] = OpCodesByName[line[0]];
                    inst[1] = int.Parse(line[1]);
                    inst[2] = int.Parse(line[2]);
                    inst[3] = int.Parse(line[3]);
                    return inst;
                })
                .ToArray();

            return (ipRegister, program);
        }

        public static void Doop(int[] instructions, long[] registers)
        {
            long a = instructions[1], b = instructions[2], c = instructions[3];

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
    }
}
