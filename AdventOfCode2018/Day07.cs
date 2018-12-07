using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day07
    {
        public const string puzzleInput = "I,Q;B,O;J,M;W,Y;U,X;T,Q;G,M;K,C;F,Z;D,A;N,Y;Y,Q;Q,Z;V,E;A,X;E,C;O,R;P,L;H,R;M,R;C,Z;R,L;L,S;S,X;Z,X;T,O;D,Z;P,R;M,Z;L,Z;W,N;Q,R;P,C;U,O;F,O;K,X;G,K;M,C;Y,Z;A,O;D,P;K,S;I,E;G,F;S,Z;N,V;F,D;A,Z;F,X;T,Y;W,H;D,H;W,G;J,X;T,X;U,R;O,P;L,X;I,B;M,L;C,R;R,X;F,N;V,H;K,A;W,O;U,Q;O,C;K,V;R,S;E,S;J,A;E,X;K,Y;Y,X;P,Z;W,X;Y,A;V,X;O,M;I,J;W,L;I,G;D,O;D,N;M,X;I,R;Y,M;F,M;U,M;Y,H;K,D;N,O;H,S;G,L;T,D;J,N;K,M;K,P;E,R;N,H";

        [Theory]
        [InlineData("1", "CABDFE", "C,A;C,F;A,B;A,D;B,E;D,E;F,E")]
        [InlineData("Actual", "IBJTUWGFKDNVEYAHOMPCQRLSZX", puzzleInput)]
        public void Test_Solve1(string nr, string expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve1(input));
        }

        [Theory]
        [InlineData("1", 2, 0, 15, "C,A;C,F;A,B;A,D;B,E;D,E;F,E")]
        [InlineData("Actual", 5, 60, 1118, puzzleInput)]
        public void Test_Solve2(string nr, int workerCount, int extraSecsPerStep, int expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve2(workerCount, extraSecsPerStep, input));
        }

        public string Solve1(string input)
        {
            var data = input
                .Split(";")
                .Select(x => new KeyValuePair<char, char>(x[0], x[2]))
                .OrderBy(x => x.Key)
                .ToList();

            var result = new StringBuilder();

            while (data.Any())
            {
                var next = data
                    .Where(i => !data.Select(x => x.Value).Contains(i.Key))
                    .OrderBy(x => x.Key)
                    .First();

                result.Append(next.Key);
                data.RemoveAll(x => x.Key == next.Key);

                if (!data.Any()) result.Append(next.Value);
            }            

            return result.ToString();
        }

        private class Worker
        {
            public char Node { get; set; }
            public int TimeLeft { get; set; }
            public bool IsBusy => TimeLeft != 0;

            public void WorkOnCharacter(char target, int time)
            {
                Node = target;
                TimeLeft = time;
            }

            public override string ToString() => $"{IsBusy} {TimeLeft} needed for {Node}";
        }

        public int Solve2(int workerCount, int extraSecsPerStep, string input)
        {
            var data = input
                .Split(";")
                .Select(x => new KeyValuePair<char, char>(x[0], x[2]))
                .OrderBy(x => x.Key)
                .ToList();

            int GetSecondsForStep(char item) => item - 'A' + 1 + extraSecsPerStep;
            int secs = 0;
            var workers = workerCount.NewItemsOfType<Worker>();

            while (data.Any())
            {
                foreach (var worker in workers.Where(w => w.IsBusy))
                {
                    if (--worker.TimeLeft == 0)
                    {
                        var finalItem = data.Where(n => n.Key == worker.Node).First().Value;
                        data.RemoveAll(n => n.Key == worker.Node);
                        if (!data.Any()) return secs + GetSecondsForStep(finalItem);
                    }
                }

                var candidates = data
                    .Where(i => 
                        !data.Select(x => x.Value).Contains(i.Key) // Has no requirements left
                        && !workers.Any(w => w.IsBusy && w.Node == i.Key) // No one working on it yet
                    ).Select(x => x.Key)
                    .OrderBy(x => x)
                    .Distinct()
                    .Take(workers.Count(w => !w.IsBusy));

                foreach (var item in candidates)
                {
                    workers.First(w => !w.IsBusy).WorkOnCharacter(item, GetSecondsForStep(item));
                }

                secs++;
            }

            return secs;
        }
    }
}
