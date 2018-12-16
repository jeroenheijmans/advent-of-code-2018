using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day16
    {
        private readonly ITestOutputHelper output;

        public Day16(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string testInput = @"
Before: [3, 2, 1, 1]
9 2 1 2
After:  [3, 2, 2, 1]";

        public const string puzzleInput1 = @"
Before: [1, 1, 0, 3]
3 0 2 0
After:  [0, 1, 0, 3]

Before: [0, 1, 2, 3]
12 1 2 3
After:  [0, 1, 2, 0]

Before: [1, 1, 2, 0]
12 1 2 2
After:  [1, 1, 0, 0]

Before: [2, 1, 1, 1]
1 1 3 0
After:  [1, 1, 1, 1]

Before: [0, 3, 1, 2]
15 0 0 2
After:  [0, 3, 1, 2]

Before: [1, 1, 1, 3]
5 2 1 2
After:  [1, 1, 2, 3]

Before: [0, 1, 0, 1]
1 1 3 3
After:  [0, 1, 0, 1]

Before: [2, 1, 2, 0]
8 0 1 0
After:  [1, 1, 2, 0]

Before: [3, 1, 2, 1]
4 3 2 1
After:  [3, 1, 2, 1]

Before: [2, 2, 1, 3]
15 3 3 3
After:  [2, 2, 1, 1]

Before: [2, 1, 2, 0]
15 2 0 2
After:  [2, 1, 1, 0]

Before: [1, 1, 1, 1]
0 1 0 1
After:  [1, 1, 1, 1]

Before: [1, 1, 1, 2]
0 1 0 3
After:  [1, 1, 1, 1]

Before: [2, 1, 0, 2]
8 0 1 3
After:  [2, 1, 0, 1]

Before: [2, 3, 2, 1]
4 3 2 1
After:  [2, 1, 2, 1]

Before: [0, 1, 1, 0]
10 0 0 2
After:  [0, 1, 0, 0]

Before: [2, 0, 2, 1]
7 0 1 0
After:  [1, 0, 2, 1]

Before: [0, 2, 2, 1]
4 3 2 2
After:  [0, 2, 1, 1]

Before: [2, 1, 1, 0]
5 2 1 2
After:  [2, 1, 2, 0]

Before: [3, 1, 2, 1]
4 3 2 0
After:  [1, 1, 2, 1]

Before: [1, 1, 0, 2]
13 3 3 0
After:  [0, 1, 0, 2]

Before: [0, 1, 1, 0]
10 0 0 1
After:  [0, 0, 1, 0]

Before: [0, 1, 1, 3]
5 2 1 0
After:  [2, 1, 1, 3]

Before: [1, 1, 2, 3]
0 1 0 0
After:  [1, 1, 2, 3]

Before: [2, 3, 3, 1]
13 3 3 2
After:  [2, 3, 0, 1]

Before: [0, 1, 2, 2]
12 1 2 0
After:  [0, 1, 2, 2]

Before: [0, 1, 3, 3]
15 3 3 3
After:  [0, 1, 3, 1]

Before: [1, 2, 2, 2]
2 0 2 2
After:  [1, 2, 0, 2]

Before: [2, 1, 1, 2]
5 2 1 2
After:  [2, 1, 2, 2]

Before: [0, 1, 2, 0]
12 1 2 3
After:  [0, 1, 2, 0]

Before: [1, 1, 1, 1]
5 2 1 1
After:  [1, 2, 1, 1]

Before: [1, 1, 2, 1]
13 3 3 2
After:  [1, 1, 0, 1]

Before: [2, 1, 3, 1]
1 1 3 3
After:  [2, 1, 3, 1]

Before: [2, 1, 2, 2]
12 1 2 2
After:  [2, 1, 0, 2]

Before: [1, 0, 2, 0]
2 0 2 1
After:  [1, 0, 2, 0]

Before: [3, 2, 1, 3]
14 2 1 1
After:  [3, 2, 1, 3]

Before: [2, 2, 0, 1]
11 0 3 3
After:  [2, 2, 0, 1]

Before: [2, 2, 0, 1]
11 0 3 1
After:  [2, 1, 0, 1]

Before: [0, 2, 2, 3]
10 0 0 0
After:  [0, 2, 2, 3]

Before: [1, 2, 3, 1]
13 3 3 3
After:  [1, 2, 3, 0]

Before: [2, 0, 2, 1]
11 0 3 3
After:  [2, 0, 2, 1]

Before: [1, 2, 0, 0]
3 0 2 0
After:  [0, 2, 0, 0]

Before: [2, 3, 1, 2]
13 3 3 2
After:  [2, 3, 0, 2]

Before: [3, 1, 3, 2]
9 1 2 2
After:  [3, 1, 0, 2]

Before: [3, 1, 0, 1]
13 3 3 1
After:  [3, 0, 0, 1]

Before: [1, 1, 0, 1]
3 0 2 0
After:  [0, 1, 0, 1]

Before: [1, 1, 3, 2]
9 1 2 3
After:  [1, 1, 3, 0]

Before: [1, 2, 1, 3]
6 1 3 1
After:  [1, 0, 1, 3]

Before: [3, 3, 2, 3]
6 2 3 2
After:  [3, 3, 0, 3]

Before: [1, 3, 2, 3]
2 0 2 3
After:  [1, 3, 2, 0]

Before: [0, 1, 1, 0]
5 2 1 0
After:  [2, 1, 1, 0]

Before: [1, 0, 1, 3]
6 2 3 3
After:  [1, 0, 1, 0]

Before: [1, 1, 2, 1]
7 3 1 0
After:  [0, 1, 2, 1]

Before: [1, 0, 0, 1]
3 0 2 1
After:  [1, 0, 0, 1]

Before: [0, 1, 2, 1]
12 1 2 2
After:  [0, 1, 0, 1]

Before: [1, 3, 0, 0]
3 0 2 1
After:  [1, 0, 0, 0]

Before: [1, 1, 2, 0]
12 1 2 1
After:  [1, 0, 2, 0]

Before: [2, 1, 2, 1]
12 1 2 1
After:  [2, 0, 2, 1]

Before: [3, 3, 2, 1]
13 3 3 1
After:  [3, 0, 2, 1]

Before: [2, 3, 2, 1]
13 3 3 0
After:  [0, 3, 2, 1]

Before: [2, 0, 1, 1]
11 0 3 2
After:  [2, 0, 1, 1]

Before: [1, 1, 2, 3]
0 1 0 2
After:  [1, 1, 1, 3]

Before: [2, 1, 3, 2]
9 1 2 0
After:  [0, 1, 3, 2]

Before: [2, 3, 2, 1]
13 3 3 2
After:  [2, 3, 0, 1]

Before: [0, 1, 1, 1]
1 1 3 1
After:  [0, 1, 1, 1]

Before: [3, 1, 2, 1]
4 3 2 2
After:  [3, 1, 1, 1]

Before: [3, 2, 1, 2]
14 2 1 0
After:  [2, 2, 1, 2]

Before: [2, 2, 1, 1]
14 2 1 2
After:  [2, 2, 2, 1]

Before: [3, 1, 1, 3]
5 2 1 1
After:  [3, 2, 1, 3]

Before: [2, 1, 2, 0]
12 1 2 2
After:  [2, 1, 0, 0]

Before: [0, 3, 1, 0]
10 0 0 1
After:  [0, 0, 1, 0]

Before: [0, 3, 1, 0]
10 0 0 0
After:  [0, 3, 1, 0]

Before: [0, 3, 3, 0]
10 0 0 3
After:  [0, 3, 3, 0]

Before: [1, 3, 2, 0]
2 0 2 1
After:  [1, 0, 2, 0]

Before: [0, 2, 1, 0]
10 0 0 2
After:  [0, 2, 0, 0]

Before: [2, 1, 2, 1]
15 2 0 3
After:  [2, 1, 2, 1]

Before: [0, 1, 2, 1]
1 1 3 3
After:  [0, 1, 2, 1]

Before: [0, 0, 0, 2]
15 0 0 1
After:  [0, 1, 0, 2]

Before: [0, 1, 1, 1]
5 2 1 0
After:  [2, 1, 1, 1]

Before: [2, 1, 0, 1]
7 3 1 0
After:  [0, 1, 0, 1]

Before: [2, 1, 1, 2]
8 0 1 3
After:  [2, 1, 1, 1]

Before: [0, 2, 3, 2]
10 0 0 2
After:  [0, 2, 0, 2]

Before: [0, 1, 1, 1]
5 2 1 1
After:  [0, 2, 1, 1]

Before: [3, 1, 1, 0]
5 2 1 0
After:  [2, 1, 1, 0]

Before: [3, 2, 2, 0]
8 0 2 3
After:  [3, 2, 2, 1]

Before: [3, 2, 2, 2]
7 3 2 1
After:  [3, 0, 2, 2]

Before: [1, 0, 0, 1]
3 0 2 0
After:  [0, 0, 0, 1]

Before: [2, 1, 3, 2]
13 3 3 0
After:  [0, 1, 3, 2]

Before: [1, 1, 0, 0]
0 1 0 0
After:  [1, 1, 0, 0]

Before: [1, 0, 0, 3]
3 0 2 1
After:  [1, 0, 0, 3]

Before: [1, 2, 0, 1]
3 0 2 2
After:  [1, 2, 0, 1]

Before: [0, 1, 0, 2]
10 0 0 1
After:  [0, 0, 0, 2]

Before: [1, 1, 2, 0]
2 0 2 3
After:  [1, 1, 2, 0]

Before: [0, 1, 2, 1]
12 1 2 1
After:  [0, 0, 2, 1]

Before: [1, 1, 2, 0]
15 2 2 3
After:  [1, 1, 2, 1]

Before: [2, 2, 2, 0]
15 2 0 1
After:  [2, 1, 2, 0]

Before: [0, 1, 3, 1]
13 3 3 0
After:  [0, 1, 3, 1]

Before: [0, 2, 0, 3]
6 1 3 3
After:  [0, 2, 0, 0]

Before: [3, 1, 1, 2]
5 2 1 2
After:  [3, 1, 2, 2]

Before: [1, 1, 0, 3]
15 3 3 0
After:  [1, 1, 0, 3]

Before: [1, 1, 3, 1]
7 3 1 2
After:  [1, 1, 0, 1]

Before: [3, 1, 1, 1]
13 2 3 3
After:  [3, 1, 1, 0]

Before: [2, 0, 2, 1]
4 3 2 0
After:  [1, 0, 2, 1]

Before: [0, 2, 2, 1]
4 3 2 1
After:  [0, 1, 2, 1]

Before: [3, 1, 2, 2]
12 1 2 2
After:  [3, 1, 0, 2]

Before: [1, 0, 2, 1]
4 3 2 3
After:  [1, 0, 2, 1]

Before: [0, 1, 3, 1]
9 1 2 3
After:  [0, 1, 3, 0]

Before: [2, 2, 3, 1]
7 2 0 2
After:  [2, 2, 1, 1]

Before: [2, 2, 1, 1]
11 0 3 3
After:  [2, 2, 1, 1]

Before: [3, 1, 3, 0]
15 2 1 1
After:  [3, 0, 3, 0]

Before: [3, 1, 1, 1]
5 2 1 0
After:  [2, 1, 1, 1]

Before: [0, 2, 1, 2]
10 0 0 3
After:  [0, 2, 1, 0]

Before: [3, 2, 2, 3]
6 2 3 1
After:  [3, 0, 2, 3]

Before: [2, 1, 1, 1]
5 2 1 3
After:  [2, 1, 1, 2]

Before: [1, 1, 2, 1]
2 0 2 1
After:  [1, 0, 2, 1]

Before: [1, 0, 2, 2]
7 3 2 1
After:  [1, 0, 2, 2]

Before: [2, 0, 3, 1]
11 0 3 0
After:  [1, 0, 3, 1]

Before: [3, 1, 3, 0]
9 1 2 0
After:  [0, 1, 3, 0]

Before: [2, 1, 1, 1]
11 0 3 0
After:  [1, 1, 1, 1]

Before: [1, 1, 0, 3]
3 0 2 2
After:  [1, 1, 0, 3]

Before: [0, 2, 1, 0]
14 2 1 3
After:  [0, 2, 1, 2]

Before: [1, 1, 2, 2]
12 1 2 2
After:  [1, 1, 0, 2]

Before: [1, 1, 1, 2]
5 2 1 2
After:  [1, 1, 2, 2]

Before: [3, 2, 0, 0]
7 0 2 3
After:  [3, 2, 0, 1]

Before: [2, 1, 1, 3]
7 2 1 1
After:  [2, 0, 1, 3]

Before: [2, 1, 0, 3]
8 0 1 0
After:  [1, 1, 0, 3]

Before: [3, 2, 2, 1]
4 3 2 0
After:  [1, 2, 2, 1]

Before: [1, 1, 1, 0]
5 2 1 3
After:  [1, 1, 1, 2]

Before: [2, 0, 3, 1]
7 0 1 3
After:  [2, 0, 3, 1]

Before: [0, 2, 2, 1]
4 3 2 0
After:  [1, 2, 2, 1]

Before: [1, 2, 1, 0]
14 2 1 2
After:  [1, 2, 2, 0]

Before: [1, 1, 2, 1]
1 1 3 3
After:  [1, 1, 2, 1]

Before: [1, 1, 1, 0]
0 1 0 0
After:  [1, 1, 1, 0]

Before: [1, 3, 2, 3]
6 2 3 2
After:  [1, 3, 0, 3]

Before: [2, 1, 1, 1]
11 0 3 1
After:  [2, 1, 1, 1]

Before: [2, 3, 3, 1]
11 0 3 1
After:  [2, 1, 3, 1]

Before: [3, 0, 1, 3]
15 3 2 0
After:  [0, 0, 1, 3]

Before: [2, 1, 2, 1]
4 3 2 1
After:  [2, 1, 2, 1]

Before: [1, 1, 0, 3]
3 0 2 3
After:  [1, 1, 0, 0]

Before: [1, 3, 2, 2]
2 0 2 3
After:  [1, 3, 2, 0]

Before: [1, 2, 3, 3]
6 1 3 2
After:  [1, 2, 0, 3]

Before: [0, 0, 1, 1]
10 0 0 0
After:  [0, 0, 1, 1]

Before: [2, 1, 2, 1]
11 0 3 1
After:  [2, 1, 2, 1]

Before: [1, 0, 2, 0]
2 0 2 2
After:  [1, 0, 0, 0]

Before: [0, 1, 1, 2]
5 2 1 3
After:  [0, 1, 1, 2]

Before: [1, 1, 2, 2]
0 1 0 0
After:  [1, 1, 2, 2]

Before: [0, 1, 0, 1]
1 1 3 2
After:  [0, 1, 1, 1]

Before: [1, 1, 3, 1]
0 1 0 2
After:  [1, 1, 1, 1]

Before: [3, 1, 1, 1]
1 1 3 1
After:  [3, 1, 1, 1]

Before: [1, 3, 2, 3]
2 0 2 0
After:  [0, 3, 2, 3]

Before: [2, 2, 1, 3]
6 2 3 0
After:  [0, 2, 1, 3]

Before: [0, 1, 1, 2]
5 2 1 0
After:  [2, 1, 1, 2]

Before: [2, 1, 3, 1]
13 3 3 0
After:  [0, 1, 3, 1]

Before: [2, 1, 2, 3]
12 1 2 3
After:  [2, 1, 2, 0]

Before: [3, 2, 2, 1]
4 3 2 1
After:  [3, 1, 2, 1]

Before: [1, 2, 1, 3]
6 2 3 1
After:  [1, 0, 1, 3]

Before: [1, 3, 1, 3]
6 2 3 2
After:  [1, 3, 0, 3]

Before: [1, 1, 2, 1]
0 1 0 1
After:  [1, 1, 2, 1]

Before: [2, 3, 2, 3]
6 2 3 2
After:  [2, 3, 0, 3]

Before: [1, 1, 3, 3]
15 3 3 3
After:  [1, 1, 3, 1]

Before: [0, 0, 2, 3]
6 2 3 3
After:  [0, 0, 2, 0]

Before: [1, 1, 3, 1]
0 1 0 0
After:  [1, 1, 3, 1]

Before: [3, 2, 1, 3]
15 3 3 0
After:  [1, 2, 1, 3]

Before: [1, 0, 2, 1]
2 0 2 0
After:  [0, 0, 2, 1]

Before: [3, 1, 0, 3]
7 0 2 3
After:  [3, 1, 0, 1]

Before: [1, 1, 3, 1]
1 1 3 1
After:  [1, 1, 3, 1]

Before: [2, 3, 0, 1]
11 0 3 2
After:  [2, 3, 1, 1]

Before: [2, 3, 3, 1]
7 2 0 2
After:  [2, 3, 1, 1]

Before: [1, 3, 2, 1]
13 3 3 3
After:  [1, 3, 2, 0]

Before: [0, 3, 2, 2]
7 3 2 3
After:  [0, 3, 2, 0]

Before: [2, 1, 3, 2]
13 3 3 3
After:  [2, 1, 3, 0]

Before: [2, 0, 1, 1]
7 0 1 1
After:  [2, 1, 1, 1]

Before: [3, 1, 2, 3]
8 0 2 1
After:  [3, 1, 2, 3]

Before: [2, 1, 1, 3]
6 2 3 2
After:  [2, 1, 0, 3]

Before: [2, 1, 1, 0]
5 2 1 3
After:  [2, 1, 1, 2]

Before: [0, 0, 0, 0]
10 0 0 3
After:  [0, 0, 0, 0]

Before: [2, 1, 2, 1]
1 1 3 3
After:  [2, 1, 2, 1]

Before: [3, 1, 0, 2]
7 0 2 0
After:  [1, 1, 0, 2]

Before: [1, 2, 2, 1]
13 3 3 2
After:  [1, 2, 0, 1]

Before: [3, 1, 1, 1]
5 2 1 1
After:  [3, 2, 1, 1]

Before: [1, 3, 0, 2]
3 0 2 1
After:  [1, 0, 0, 2]

Before: [0, 1, 0, 1]
1 1 3 0
After:  [1, 1, 0, 1]

Before: [3, 1, 2, 1]
12 1 2 0
After:  [0, 1, 2, 1]

Before: [1, 3, 2, 1]
2 0 2 2
After:  [1, 3, 0, 1]

Before: [2, 3, 1, 1]
11 0 3 0
After:  [1, 3, 1, 1]

Before: [0, 1, 1, 0]
5 2 1 2
After:  [0, 1, 2, 0]

Before: [0, 1, 3, 0]
9 1 2 2
After:  [0, 1, 0, 0]

Before: [2, 1, 1, 1]
5 2 1 0
After:  [2, 1, 1, 1]

Before: [1, 1, 1, 1]
0 1 0 0
After:  [1, 1, 1, 1]

Before: [1, 0, 0, 1]
3 0 2 2
After:  [1, 0, 0, 1]

Before: [0, 1, 3, 2]
9 1 2 0
After:  [0, 1, 3, 2]

Before: [1, 3, 0, 1]
3 0 2 2
After:  [1, 3, 0, 1]

Before: [2, 0, 2, 1]
4 3 2 1
After:  [2, 1, 2, 1]

Before: [0, 2, 1, 3]
6 2 3 1
After:  [0, 0, 1, 3]

Before: [1, 2, 0, 2]
3 0 2 0
After:  [0, 2, 0, 2]

Before: [0, 1, 2, 2]
12 1 2 3
After:  [0, 1, 2, 0]

Before: [1, 1, 1, 2]
0 1 0 2
After:  [1, 1, 1, 2]

Before: [1, 1, 1, 0]
0 1 0 3
After:  [1, 1, 1, 1]

Before: [3, 1, 2, 3]
6 1 3 3
After:  [3, 1, 2, 0]

Before: [2, 2, 1, 1]
11 0 3 2
After:  [2, 2, 1, 1]

Before: [2, 3, 3, 1]
11 0 3 2
After:  [2, 3, 1, 1]

Before: [0, 2, 3, 2]
15 0 0 1
After:  [0, 1, 3, 2]

Before: [0, 3, 1, 3]
6 2 3 3
After:  [0, 3, 1, 0]

Before: [3, 2, 3, 1]
15 2 3 2
After:  [3, 2, 0, 1]

Before: [0, 1, 1, 1]
7 2 1 2
After:  [0, 1, 0, 1]

Before: [3, 1, 2, 1]
1 1 3 0
After:  [1, 1, 2, 1]

Before: [0, 0, 0, 3]
10 0 0 0
After:  [0, 0, 0, 3]

Before: [1, 1, 3, 1]
9 1 2 0
After:  [0, 1, 3, 1]

Before: [0, 3, 1, 3]
10 0 0 1
After:  [0, 0, 1, 3]

Before: [1, 2, 1, 1]
14 2 1 2
After:  [1, 2, 2, 1]

Before: [3, 1, 0, 1]
1 1 3 3
After:  [3, 1, 0, 1]

Before: [0, 1, 1, 1]
1 1 3 2
After:  [0, 1, 1, 1]

Before: [1, 1, 2, 0]
0 1 0 2
After:  [1, 1, 1, 0]

Before: [0, 3, 2, 2]
7 3 2 0
After:  [0, 3, 2, 2]

Before: [0, 3, 0, 3]
10 0 0 3
After:  [0, 3, 0, 0]

Before: [1, 1, 2, 1]
12 1 2 3
After:  [1, 1, 2, 0]

Before: [0, 0, 2, 1]
4 3 2 2
After:  [0, 0, 1, 1]

Before: [1, 1, 2, 0]
12 1 2 0
After:  [0, 1, 2, 0]

Before: [0, 1, 2, 1]
12 1 2 3
After:  [0, 1, 2, 0]

Before: [0, 1, 1, 3]
6 1 3 0
After:  [0, 1, 1, 3]

Before: [2, 3, 2, 1]
11 0 3 0
After:  [1, 3, 2, 1]

Before: [1, 1, 1, 1]
5 2 1 3
After:  [1, 1, 1, 2]

Before: [1, 0, 2, 0]
2 0 2 3
After:  [1, 0, 2, 0]

Before: [1, 1, 2, 3]
2 0 2 2
After:  [1, 1, 0, 3]

Before: [2, 0, 0, 1]
11 0 3 0
After:  [1, 0, 0, 1]

Before: [3, 0, 3, 3]
15 3 2 2
After:  [3, 0, 1, 3]

Before: [1, 2, 2, 2]
2 0 2 3
After:  [1, 2, 2, 0]

Before: [1, 1, 2, 1]
12 1 2 2
After:  [1, 1, 0, 1]

Before: [1, 1, 2, 0]
0 1 0 1
After:  [1, 1, 2, 0]

Before: [1, 0, 2, 2]
13 3 3 1
After:  [1, 0, 2, 2]

Before: [2, 1, 2, 1]
12 1 2 3
After:  [2, 1, 2, 0]

Before: [0, 3, 2, 2]
10 0 0 3
After:  [0, 3, 2, 0]

Before: [1, 1, 1, 2]
5 2 1 1
After:  [1, 2, 1, 2]

Before: [3, 3, 0, 1]
13 3 3 0
After:  [0, 3, 0, 1]

Before: [1, 1, 0, 2]
3 0 2 3
After:  [1, 1, 0, 0]

Before: [2, 1, 2, 3]
15 2 2 0
After:  [1, 1, 2, 3]

Before: [2, 1, 1, 1]
8 0 1 2
After:  [2, 1, 1, 1]

Before: [0, 1, 1, 2]
10 0 0 0
After:  [0, 1, 1, 2]

Before: [1, 1, 2, 1]
0 1 0 2
After:  [1, 1, 1, 1]

Before: [1, 2, 2, 1]
15 2 2 2
After:  [1, 2, 1, 1]

Before: [0, 3, 2, 1]
4 3 2 0
After:  [1, 3, 2, 1]

Before: [0, 1, 3, 3]
9 1 2 0
After:  [0, 1, 3, 3]

Before: [0, 1, 1, 0]
7 2 1 3
After:  [0, 1, 1, 0]

Before: [1, 2, 2, 1]
2 0 2 3
After:  [1, 2, 2, 0]

Before: [2, 2, 3, 1]
11 0 3 1
After:  [2, 1, 3, 1]

Before: [3, 2, 1, 1]
14 2 1 1
After:  [3, 2, 1, 1]

Before: [3, 1, 3, 1]
9 1 2 1
After:  [3, 0, 3, 1]

Before: [2, 1, 0, 1]
1 1 3 3
After:  [2, 1, 0, 1]

Before: [1, 1, 3, 1]
0 1 0 3
After:  [1, 1, 3, 1]

Before: [2, 2, 2, 1]
4 3 2 0
After:  [1, 2, 2, 1]

Before: [1, 3, 2, 2]
2 0 2 0
After:  [0, 3, 2, 2]

Before: [2, 1, 3, 3]
9 1 2 0
After:  [0, 1, 3, 3]

Before: [3, 0, 2, 0]
8 0 2 0
After:  [1, 0, 2, 0]

Before: [1, 1, 1, 3]
0 1 0 1
After:  [1, 1, 1, 3]

Before: [2, 1, 2, 1]
11 0 3 0
After:  [1, 1, 2, 1]

Before: [1, 1, 2, 1]
2 0 2 0
After:  [0, 1, 2, 1]

Before: [1, 1, 0, 0]
3 0 2 0
After:  [0, 1, 0, 0]

Before: [0, 3, 1, 1]
15 0 0 0
After:  [1, 3, 1, 1]

Before: [1, 3, 2, 3]
6 2 3 0
After:  [0, 3, 2, 3]

Before: [0, 0, 1, 2]
13 3 3 1
After:  [0, 0, 1, 2]

Before: [1, 1, 2, 1]
4 3 2 3
After:  [1, 1, 2, 1]

Before: [1, 2, 1, 3]
14 2 1 0
After:  [2, 2, 1, 3]

Before: [0, 3, 1, 1]
10 0 0 3
After:  [0, 3, 1, 0]

Before: [2, 3, 1, 1]
13 2 3 1
After:  [2, 0, 1, 1]

Before: [3, 1, 2, 1]
4 3 2 3
After:  [3, 1, 2, 1]

Before: [2, 2, 1, 1]
11 0 3 1
After:  [2, 1, 1, 1]

Before: [0, 2, 2, 2]
10 0 0 2
After:  [0, 2, 0, 2]

Before: [0, 0, 2, 1]
4 3 2 0
After:  [1, 0, 2, 1]

Before: [3, 1, 1, 3]
5 2 1 2
After:  [3, 1, 2, 3]

Before: [2, 2, 0, 3]
6 1 3 1
After:  [2, 0, 0, 3]

Before: [3, 0, 2, 1]
4 3 2 2
After:  [3, 0, 1, 1]

Before: [3, 0, 2, 1]
8 0 2 3
After:  [3, 0, 2, 1]

Before: [3, 1, 0, 0]
7 0 2 3
After:  [3, 1, 0, 1]

Before: [2, 1, 3, 2]
9 1 2 2
After:  [2, 1, 0, 2]

Before: [0, 2, 2, 0]
10 0 0 0
After:  [0, 2, 2, 0]

Before: [1, 2, 2, 1]
4 3 2 2
After:  [1, 2, 1, 1]

Before: [2, 1, 1, 0]
8 0 1 2
After:  [2, 1, 1, 0]

Before: [1, 0, 2, 3]
6 2 3 2
After:  [1, 0, 0, 3]

Before: [1, 1, 2, 3]
6 1 3 2
After:  [1, 1, 0, 3]

Before: [2, 3, 2, 1]
4 3 2 0
After:  [1, 3, 2, 1]

Before: [1, 2, 1, 0]
14 2 1 3
After:  [1, 2, 1, 2]

Before: [1, 1, 0, 3]
0 1 0 1
After:  [1, 1, 0, 3]

Before: [2, 2, 1, 3]
15 3 3 0
After:  [1, 2, 1, 3]

Before: [0, 2, 1, 3]
10 0 0 1
After:  [0, 0, 1, 3]

Before: [1, 1, 3, 2]
0 1 0 2
After:  [1, 1, 1, 2]

Before: [2, 0, 3, 1]
11 0 3 3
After:  [2, 0, 3, 1]

Before: [2, 1, 2, 3]
12 1 2 1
After:  [2, 0, 2, 3]

Before: [1, 1, 0, 0]
3 0 2 2
After:  [1, 1, 0, 0]

Before: [3, 1, 1, 1]
13 3 3 0
After:  [0, 1, 1, 1]

Before: [0, 0, 2, 3]
10 0 0 3
After:  [0, 0, 2, 0]

Before: [3, 1, 3, 1]
9 1 2 0
After:  [0, 1, 3, 1]

Before: [1, 1, 2, 0]
0 1 0 0
After:  [1, 1, 2, 0]

Before: [0, 1, 2, 3]
6 2 3 1
After:  [0, 0, 2, 3]

Before: [2, 1, 3, 3]
9 1 2 1
After:  [2, 0, 3, 3]

Before: [1, 2, 1, 3]
14 2 1 1
After:  [1, 2, 1, 3]

Before: [0, 1, 2, 2]
10 0 0 3
After:  [0, 1, 2, 0]

Before: [2, 1, 2, 0]
12 1 2 1
After:  [2, 0, 2, 0]

Before: [1, 1, 0, 1]
1 1 3 1
After:  [1, 1, 0, 1]

Before: [1, 3, 2, 3]
15 3 2 3
After:  [1, 3, 2, 0]

Before: [1, 2, 2, 2]
7 3 2 2
After:  [1, 2, 0, 2]

Before: [3, 3, 2, 0]
8 0 2 3
After:  [3, 3, 2, 1]

Before: [0, 3, 1, 1]
10 0 0 0
After:  [0, 3, 1, 1]

Before: [0, 1, 1, 2]
13 3 3 0
After:  [0, 1, 1, 2]

Before: [1, 1, 1, 0]
5 2 1 1
After:  [1, 2, 1, 0]

Before: [1, 2, 0, 1]
3 0 2 1
After:  [1, 0, 0, 1]

Before: [3, 1, 3, 1]
9 1 2 3
After:  [3, 1, 3, 0]

Before: [1, 2, 2, 3]
2 0 2 0
After:  [0, 2, 2, 3]

Before: [0, 3, 2, 1]
4 3 2 2
After:  [0, 3, 1, 1]

Before: [1, 2, 2, 1]
15 2 1 0
After:  [1, 2, 2, 1]

Before: [2, 0, 3, 0]
7 2 0 1
After:  [2, 1, 3, 0]

Before: [1, 3, 2, 1]
4 3 2 1
After:  [1, 1, 2, 1]

Before: [1, 3, 0, 1]
3 0 2 0
After:  [0, 3, 0, 1]

Before: [3, 1, 1, 1]
13 2 3 1
After:  [3, 0, 1, 1]

Before: [2, 2, 3, 1]
11 0 3 3
After:  [2, 2, 3, 1]

Before: [3, 3, 2, 1]
15 2 2 3
After:  [3, 3, 2, 1]

Before: [3, 0, 3, 3]
15 3 2 3
After:  [3, 0, 3, 1]

Before: [1, 1, 0, 1]
3 0 2 1
After:  [1, 0, 0, 1]

Before: [1, 1, 0, 2]
0 1 0 3
After:  [1, 1, 0, 1]

Before: [0, 0, 2, 1]
10 0 0 1
After:  [0, 0, 2, 1]

Before: [1, 1, 3, 0]
0 1 0 1
After:  [1, 1, 3, 0]

Before: [1, 0, 0, 3]
3 0 2 0
After:  [0, 0, 0, 3]

Before: [0, 2, 1, 3]
10 0 0 0
After:  [0, 2, 1, 3]

Before: [3, 1, 2, 0]
12 1 2 3
After:  [3, 1, 2, 0]

Before: [2, 1, 3, 0]
8 0 1 0
After:  [1, 1, 3, 0]

Before: [1, 0, 2, 1]
4 3 2 1
After:  [1, 1, 2, 1]

Before: [2, 1, 2, 3]
6 1 3 0
After:  [0, 1, 2, 3]

Before: [1, 1, 0, 0]
0 1 0 3
After:  [1, 1, 0, 1]

Before: [3, 1, 1, 3]
7 2 1 3
After:  [3, 1, 1, 0]

Before: [0, 2, 1, 1]
14 2 1 2
After:  [0, 2, 2, 1]

Before: [2, 1, 0, 1]
11 0 3 3
After:  [2, 1, 0, 1]

Before: [1, 1, 2, 3]
0 1 0 1
After:  [1, 1, 2, 3]

Before: [2, 1, 3, 0]
9 1 2 0
After:  [0, 1, 3, 0]

Before: [0, 2, 1, 3]
6 1 3 0
After:  [0, 2, 1, 3]

Before: [1, 1, 3, 2]
0 1 0 0
After:  [1, 1, 3, 2]

Before: [0, 2, 1, 3]
14 2 1 0
After:  [2, 2, 1, 3]

Before: [0, 0, 1, 1]
13 3 3 1
After:  [0, 0, 1, 1]

Before: [2, 1, 1, 0]
5 2 1 0
After:  [2, 1, 1, 0]

Before: [3, 1, 1, 1]
13 3 3 3
After:  [3, 1, 1, 0]

Before: [1, 1, 2, 1]
1 1 3 1
After:  [1, 1, 2, 1]

Before: [0, 1, 2, 1]
1 1 3 2
After:  [0, 1, 1, 1]

Before: [0, 1, 1, 2]
5 2 1 1
After:  [0, 2, 1, 2]

Before: [2, 1, 1, 2]
8 0 1 1
After:  [2, 1, 1, 2]

Before: [2, 1, 1, 2]
8 0 1 0
After:  [1, 1, 1, 2]

Before: [2, 1, 1, 1]
5 2 1 1
After:  [2, 2, 1, 1]

Before: [3, 2, 1, 0]
14 2 1 2
After:  [3, 2, 2, 0]

Before: [2, 3, 0, 1]
11 0 3 0
After:  [1, 3, 0, 1]

Before: [0, 1, 1, 0]
5 2 1 1
After:  [0, 2, 1, 0]

Before: [3, 3, 0, 3]
7 0 2 1
After:  [3, 1, 0, 3]

Before: [1, 1, 2, 3]
6 2 3 1
After:  [1, 0, 2, 3]

Before: [1, 1, 2, 0]
2 0 2 0
After:  [0, 1, 2, 0]

Before: [3, 0, 2, 3]
8 0 2 0
After:  [1, 0, 2, 3]

Before: [0, 1, 1, 1]
1 1 3 3
After:  [0, 1, 1, 1]

Before: [2, 1, 2, 2]
12 1 2 1
After:  [2, 0, 2, 2]

Before: [3, 3, 2, 1]
4 3 2 3
After:  [3, 3, 2, 1]

Before: [1, 2, 2, 3]
2 0 2 3
After:  [1, 2, 2, 0]

Before: [1, 1, 0, 1]
0 1 0 2
After:  [1, 1, 1, 1]

Before: [0, 2, 2, 1]
4 3 2 3
After:  [0, 2, 2, 1]

Before: [0, 1, 1, 1]
7 3 1 0
After:  [0, 1, 1, 1]

Before: [2, 0, 0, 1]
11 0 3 3
After:  [2, 0, 0, 1]

Before: [1, 1, 2, 2]
0 1 0 1
After:  [1, 1, 2, 2]

Before: [1, 2, 0, 3]
3 0 2 1
After:  [1, 0, 0, 3]

Before: [1, 1, 3, 3]
9 1 2 2
After:  [1, 1, 0, 3]

Before: [3, 1, 3, 0]
9 1 2 3
After:  [3, 1, 3, 0]

Before: [1, 1, 1, 2]
0 1 0 1
After:  [1, 1, 1, 2]

Before: [0, 1, 2, 1]
4 3 2 2
After:  [0, 1, 1, 1]

Before: [1, 1, 1, 0]
5 2 1 2
After:  [1, 1, 2, 0]

Before: [1, 1, 3, 3]
6 1 3 3
After:  [1, 1, 3, 0]

Before: [0, 1, 0, 1]
7 3 1 0
After:  [0, 1, 0, 1]

Before: [3, 1, 1, 1]
1 1 3 0
After:  [1, 1, 1, 1]

Before: [2, 1, 2, 1]
4 3 2 0
After:  [1, 1, 2, 1]

Before: [2, 3, 1, 1]
13 3 3 1
After:  [2, 0, 1, 1]

Before: [2, 0, 3, 1]
11 0 3 2
After:  [2, 0, 1, 1]

Before: [0, 1, 3, 0]
9 1 2 0
After:  [0, 1, 3, 0]

Before: [1, 2, 2, 3]
2 0 2 1
After:  [1, 0, 2, 3]

Before: [1, 3, 0, 0]
3 0 2 0
After:  [0, 3, 0, 0]

Before: [0, 2, 1, 1]
14 2 1 1
After:  [0, 2, 1, 1]

Before: [1, 2, 2, 2]
2 0 2 1
After:  [1, 0, 2, 2]

Before: [0, 3, 2, 0]
10 0 0 0
After:  [0, 3, 2, 0]

Before: [1, 1, 0, 1]
0 1 0 0
After:  [1, 1, 0, 1]

Before: [3, 1, 2, 2]
7 3 2 1
After:  [3, 0, 2, 2]

Before: [1, 1, 1, 1]
5 2 1 2
After:  [1, 1, 2, 1]

Before: [1, 0, 0, 2]
3 0 2 3
After:  [1, 0, 0, 0]

Before: [1, 1, 3, 0]
0 1 0 3
After:  [1, 1, 3, 1]

Before: [0, 3, 2, 0]
15 0 0 1
After:  [0, 1, 2, 0]

Before: [2, 2, 2, 3]
15 2 2 0
After:  [1, 2, 2, 3]

Before: [1, 1, 1, 1]
0 1 0 3
After:  [1, 1, 1, 1]

Before: [0, 1, 3, 1]
15 2 3 3
After:  [0, 1, 3, 0]

Before: [0, 0, 0, 2]
10 0 0 1
After:  [0, 0, 0, 2]

Before: [1, 3, 0, 3]
3 0 2 3
After:  [1, 3, 0, 0]

Before: [3, 2, 2, 2]
8 0 2 1
After:  [3, 1, 2, 2]

Before: [2, 1, 2, 3]
6 1 3 2
After:  [2, 1, 0, 3]

Before: [3, 1, 1, 1]
5 2 1 3
After:  [3, 1, 1, 2]

Before: [0, 0, 3, 1]
10 0 0 3
After:  [0, 0, 3, 0]

Before: [3, 1, 3, 1]
9 1 2 2
After:  [3, 1, 0, 1]

Before: [1, 2, 2, 1]
13 3 3 0
After:  [0, 2, 2, 1]

Before: [1, 0, 0, 2]
13 3 3 0
After:  [0, 0, 0, 2]

Before: [0, 2, 1, 0]
14 2 1 1
After:  [0, 2, 1, 0]

Before: [3, 1, 1, 2]
5 2 1 0
After:  [2, 1, 1, 2]

Before: [2, 1, 0, 3]
8 0 1 2
After:  [2, 1, 1, 3]

Before: [1, 1, 0, 3]
0 1 0 0
After:  [1, 1, 0, 3]

Before: [2, 2, 2, 1]
4 3 2 1
After:  [2, 1, 2, 1]

Before: [1, 3, 0, 3]
3 0 2 2
After:  [1, 3, 0, 3]

Before: [2, 0, 2, 0]
7 0 1 0
After:  [1, 0, 2, 0]

Before: [3, 1, 0, 1]
1 1 3 0
After:  [1, 1, 0, 1]

Before: [1, 1, 0, 0]
3 0 2 3
After:  [1, 1, 0, 0]

Before: [2, 1, 0, 1]
11 0 3 2
After:  [2, 1, 1, 1]

Before: [3, 2, 2, 3]
6 2 3 3
After:  [3, 2, 2, 0]

Before: [2, 0, 0, 3]
7 0 1 2
After:  [2, 0, 1, 3]

Before: [0, 0, 2, 1]
4 3 2 3
After:  [0, 0, 2, 1]

Before: [0, 3, 0, 2]
10 0 0 3
After:  [0, 3, 0, 0]

Before: [2, 0, 2, 2]
7 3 2 3
After:  [2, 0, 2, 0]

Before: [1, 1, 0, 3]
0 1 0 2
After:  [1, 1, 1, 3]

Before: [2, 0, 2, 1]
11 0 3 1
After:  [2, 1, 2, 1]

Before: [1, 2, 3, 3]
15 3 2 0
After:  [1, 2, 3, 3]

Before: [2, 1, 3, 1]
7 3 1 1
After:  [2, 0, 3, 1]

Before: [1, 1, 0, 3]
6 1 3 0
After:  [0, 1, 0, 3]

Before: [1, 0, 0, 0]
3 0 2 2
After:  [1, 0, 0, 0]

Before: [2, 1, 3, 1]
11 0 3 2
After:  [2, 1, 1, 1]

Before: [2, 0, 1, 1]
11 0 3 1
After:  [2, 1, 1, 1]

Before: [1, 1, 1, 3]
0 1 0 3
After:  [1, 1, 1, 1]

Before: [1, 2, 2, 0]
2 0 2 0
After:  [0, 2, 2, 0]

Before: [1, 2, 0, 3]
3 0 2 3
After:  [1, 2, 0, 0]

Before: [1, 3, 2, 1]
4 3 2 3
After:  [1, 3, 2, 1]

Before: [0, 2, 1, 2]
14 2 1 3
After:  [0, 2, 1, 2]

Before: [3, 0, 2, 3]
8 0 2 1
After:  [3, 1, 2, 3]

Before: [0, 1, 1, 3]
10 0 0 3
After:  [0, 1, 1, 0]

Before: [2, 1, 2, 1]
4 3 2 3
After:  [2, 1, 2, 1]

Before: [1, 1, 2, 3]
6 1 3 0
After:  [0, 1, 2, 3]

Before: [2, 1, 1, 2]
5 2 1 0
After:  [2, 1, 1, 2]

Before: [2, 1, 1, 0]
5 2 1 1
After:  [2, 2, 1, 0]

Before: [0, 1, 1, 1]
5 2 1 2
After:  [0, 1, 2, 1]

Before: [2, 3, 1, 1]
11 0 3 1
After:  [2, 1, 1, 1]

Before: [1, 1, 3, 0]
0 1 0 0
After:  [1, 1, 3, 0]

Before: [1, 3, 2, 3]
2 0 2 1
After:  [1, 0, 2, 3]

Before: [0, 1, 1, 1]
5 2 1 3
After:  [0, 1, 1, 2]

Before: [0, 1, 3, 3]
6 1 3 2
After:  [0, 1, 0, 3]

Before: [2, 0, 2, 3]
6 2 3 0
After:  [0, 0, 2, 3]

Before: [2, 2, 3, 1]
7 2 0 3
After:  [2, 2, 3, 1]

Before: [1, 3, 0, 3]
3 0 2 1
After:  [1, 0, 0, 3]

Before: [1, 2, 0, 2]
3 0 2 1
After:  [1, 0, 0, 2]

Before: [2, 2, 1, 1]
14 2 1 0
After:  [2, 2, 1, 1]

Before: [2, 1, 3, 3]
9 1 2 3
After:  [2, 1, 3, 0]

Before: [1, 1, 2, 2]
0 1 0 3
After:  [1, 1, 2, 1]

Before: [0, 1, 1, 3]
15 3 3 3
After:  [0, 1, 1, 1]

Before: [1, 3, 2, 1]
4 3 2 0
After:  [1, 3, 2, 1]

Before: [2, 1, 2, 3]
8 0 1 0
After:  [1, 1, 2, 3]

Before: [1, 0, 2, 3]
2 0 2 3
After:  [1, 0, 2, 0]

Before: [0, 0, 2, 3]
15 3 3 2
After:  [0, 0, 1, 3]

Before: [0, 0, 2, 2]
15 2 2 0
After:  [1, 0, 2, 2]

Before: [3, 3, 2, 2]
8 0 2 1
After:  [3, 1, 2, 2]

Before: [1, 1, 3, 1]
13 3 3 1
After:  [1, 0, 3, 1]

Before: [3, 2, 2, 1]
4 3 2 3
After:  [3, 2, 2, 1]

Before: [1, 1, 3, 1]
1 1 3 0
After:  [1, 1, 3, 1]

Before: [0, 3, 2, 1]
4 3 2 3
After:  [0, 3, 2, 1]

Before: [3, 1, 2, 3]
12 1 2 1
After:  [3, 0, 2, 3]

Before: [1, 2, 1, 2]
14 2 1 1
After:  [1, 2, 1, 2]

Before: [1, 3, 0, 2]
3 0 2 2
After:  [1, 3, 0, 2]

Before: [1, 1, 3, 3]
0 1 0 3
After:  [1, 1, 3, 1]

Before: [3, 3, 2, 1]
4 3 2 1
After:  [3, 1, 2, 1]

Before: [0, 1, 1, 2]
10 0 0 1
After:  [0, 0, 1, 2]

Before: [1, 2, 1, 0]
14 2 1 1
After:  [1, 2, 1, 0]

Before: [2, 1, 0, 1]
1 1 3 2
After:  [2, 1, 1, 1]

Before: [2, 1, 0, 2]
13 3 3 2
After:  [2, 1, 0, 2]

Before: [1, 2, 0, 0]
3 0 2 1
After:  [1, 0, 0, 0]

Before: [3, 2, 1, 1]
14 2 1 3
After:  [3, 2, 1, 2]

Before: [3, 0, 1, 1]
13 2 3 0
After:  [0, 0, 1, 1]

Before: [2, 2, 2, 1]
11 0 3 2
After:  [2, 2, 1, 1]

Before: [2, 1, 1, 1]
1 1 3 2
After:  [2, 1, 1, 1]

Before: [0, 2, 0, 0]
10 0 0 1
After:  [0, 0, 0, 0]

Before: [1, 1, 1, 3]
0 1 0 2
After:  [1, 1, 1, 3]

Before: [3, 2, 2, 3]
8 0 2 2
After:  [3, 2, 1, 3]

Before: [1, 3, 0, 0]
3 0 2 2
After:  [1, 3, 0, 0]

Before: [2, 1, 1, 3]
15 3 3 3
After:  [2, 1, 1, 1]

Before: [2, 1, 0, 1]
11 0 3 1
After:  [2, 1, 0, 1]

Before: [3, 3, 2, 1]
13 3 3 3
After:  [3, 3, 2, 0]

Before: [3, 1, 1, 2]
5 2 1 3
After:  [3, 1, 1, 2]

Before: [1, 1, 3, 3]
6 1 3 0
After:  [0, 1, 3, 3]

Before: [0, 1, 1, 1]
1 1 3 0
After:  [1, 1, 1, 1]

Before: [1, 1, 0, 0]
0 1 0 1
After:  [1, 1, 0, 0]

Before: [1, 1, 2, 3]
2 0 2 0
After:  [0, 1, 2, 3]

Before: [1, 3, 0, 0]
3 0 2 3
After:  [1, 3, 0, 0]

Before: [0, 1, 2, 3]
15 0 0 2
After:  [0, 1, 1, 3]

Before: [0, 0, 2, 2]
10 0 0 3
After:  [0, 0, 2, 0]

Before: [1, 1, 3, 3]
0 1 0 0
After:  [1, 1, 3, 3]

Before: [0, 2, 2, 0]
10 0 0 1
After:  [0, 0, 2, 0]

Before: [0, 3, 3, 0]
10 0 0 1
After:  [0, 0, 3, 0]

Before: [0, 1, 1, 3]
5 2 1 2
After:  [0, 1, 2, 3]

Before: [3, 3, 2, 2]
8 0 2 2
After:  [3, 3, 1, 2]

Before: [2, 3, 3, 1]
11 0 3 3
After:  [2, 3, 3, 1]

Before: [2, 1, 3, 1]
7 3 1 0
After:  [0, 1, 3, 1]

Before: [3, 1, 1, 1]
5 2 1 2
After:  [3, 1, 2, 1]

Before: [3, 1, 3, 1]
1 1 3 3
After:  [3, 1, 3, 1]

Before: [0, 1, 1, 3]
5 2 1 3
After:  [0, 1, 1, 2]

Before: [2, 2, 3, 3]
6 1 3 1
After:  [2, 0, 3, 3]

Before: [3, 2, 1, 3]
15 3 0 1
After:  [3, 1, 1, 3]

Before: [1, 1, 1, 3]
0 1 0 0
After:  [1, 1, 1, 3]

Before: [2, 1, 0, 3]
6 1 3 0
After:  [0, 1, 0, 3]

Before: [1, 2, 2, 2]
15 2 1 2
After:  [1, 2, 1, 2]

Before: [2, 3, 2, 1]
11 0 3 3
After:  [2, 3, 2, 1]

Before: [2, 3, 2, 1]
11 0 3 1
After:  [2, 1, 2, 1]

Before: [1, 1, 2, 2]
2 0 2 0
After:  [0, 1, 2, 2]

Before: [1, 1, 1, 2]
5 2 1 3
After:  [1, 1, 1, 2]

Before: [2, 1, 3, 1]
11 0 3 3
After:  [2, 1, 3, 1]

Before: [2, 2, 1, 2]
14 2 1 1
After:  [2, 2, 1, 2]

Before: [0, 0, 2, 3]
15 3 3 3
After:  [0, 0, 2, 1]

Before: [2, 0, 3, 1]
7 0 1 2
After:  [2, 0, 1, 1]

Before: [3, 1, 3, 2]
9 1 2 0
After:  [0, 1, 3, 2]

Before: [0, 3, 3, 1]
13 3 3 1
After:  [0, 0, 3, 1]

Before: [1, 1, 1, 3]
6 1 3 2
After:  [1, 1, 0, 3]

Before: [3, 2, 2, 0]
15 2 1 1
After:  [3, 1, 2, 0]

Before: [0, 2, 1, 2]
14 2 1 1
After:  [0, 2, 1, 2]

Before: [3, 3, 2, 3]
15 3 3 3
After:  [3, 3, 2, 1]

Before: [2, 1, 1, 3]
5 2 1 2
After:  [2, 1, 2, 3]

Before: [2, 3, 2, 1]
11 0 3 2
After:  [2, 3, 1, 1]

Before: [3, 3, 2, 2]
7 3 2 3
After:  [3, 3, 2, 0]

Before: [1, 1, 3, 3]
0 1 0 2
After:  [1, 1, 1, 3]

Before: [0, 1, 2, 1]
4 3 2 0
After:  [1, 1, 2, 1]

Before: [2, 1, 3, 0]
8 0 1 3
After:  [2, 1, 3, 1]

Before: [2, 1, 1, 3]
6 2 3 1
After:  [2, 0, 1, 3]

Before: [1, 2, 2, 1]
4 3 2 3
After:  [1, 2, 2, 1]

Before: [0, 2, 0, 3]
15 3 1 3
After:  [0, 2, 0, 0]

Before: [0, 3, 2, 1]
4 3 2 1
After:  [0, 1, 2, 1]

Before: [3, 1, 2, 2]
7 3 2 0
After:  [0, 1, 2, 2]

Before: [3, 1, 3, 2]
9 1 2 1
After:  [3, 0, 3, 2]

Before: [1, 1, 1, 1]
0 1 0 2
After:  [1, 1, 1, 1]

Before: [0, 2, 1, 1]
14 2 1 3
After:  [0, 2, 1, 2]

Before: [1, 1, 3, 2]
9 1 2 1
After:  [1, 0, 3, 2]

Before: [2, 0, 2, 1]
11 0 3 2
After:  [2, 0, 1, 1]

Before: [2, 1, 1, 3]
8 0 1 1
After:  [2, 1, 1, 3]

Before: [0, 3, 2, 2]
10 0 0 2
After:  [0, 3, 0, 2]

Before: [1, 2, 0, 0]
3 0 2 2
After:  [1, 2, 0, 0]

Before: [3, 0, 2, 1]
4 3 2 1
After:  [3, 1, 2, 1]

Before: [2, 1, 1, 1]
11 0 3 2
After:  [2, 1, 1, 1]

Before: [2, 1, 1, 2]
5 2 1 1
After:  [2, 2, 1, 2]

Before: [1, 1, 0, 1]
1 1 3 0
After:  [1, 1, 0, 1]

Before: [0, 3, 3, 1]
13 3 3 0
After:  [0, 3, 3, 1]

Before: [0, 3, 2, 2]
10 0 0 0
After:  [0, 3, 2, 2]

Before: [3, 1, 2, 1]
1 1 3 3
After:  [3, 1, 2, 1]

Before: [2, 0, 3, 2]
7 0 1 1
After:  [2, 1, 3, 2]

Before: [0, 1, 3, 0]
9 1 2 3
After:  [0, 1, 3, 0]

Before: [1, 1, 2, 3]
12 1 2 3
After:  [1, 1, 2, 0]

Before: [1, 1, 2, 3]
0 1 0 3
After:  [1, 1, 2, 1]

Before: [1, 3, 0, 1]
3 0 2 3
After:  [1, 3, 0, 0]

Before: [1, 1, 2, 2]
12 1 2 1
After:  [1, 0, 2, 2]

Before: [3, 2, 1, 3]
14 2 1 2
After:  [3, 2, 2, 3]

Before: [2, 2, 1, 0]
14 2 1 2
After:  [2, 2, 2, 0]

Before: [2, 1, 3, 1]
1 1 3 0
After:  [1, 1, 3, 1]

Before: [1, 1, 1, 1]
5 2 1 0
After:  [2, 1, 1, 1]

Before: [3, 1, 1, 3]
5 2 1 0
After:  [2, 1, 1, 3]

Before: [1, 1, 0, 1]
0 1 0 3
After:  [1, 1, 0, 1]

Before: [0, 3, 1, 3]
10 0 0 2
After:  [0, 3, 0, 3]

Before: [1, 0, 0, 1]
3 0 2 3
After:  [1, 0, 0, 0]

Before: [0, 2, 1, 3]
14 2 1 3
After:  [0, 2, 1, 2]

Before: [1, 1, 3, 2]
15 2 1 2
After:  [1, 1, 0, 2]

Before: [3, 1, 3, 3]
9 1 2 0
After:  [0, 1, 3, 3]

Before: [2, 0, 2, 1]
4 3 2 2
After:  [2, 0, 1, 1]

Before: [2, 0, 2, 2]
7 3 2 1
After:  [2, 0, 2, 2]

Before: [2, 3, 2, 3]
15 3 2 0
After:  [0, 3, 2, 3]

Before: [2, 1, 1, 0]
7 2 1 0
After:  [0, 1, 1, 0]

Before: [1, 0, 0, 2]
3 0 2 2
After:  [1, 0, 0, 2]

Before: [1, 2, 2, 1]
4 3 2 0
After:  [1, 2, 2, 1]

Before: [0, 2, 1, 1]
10 0 0 3
After:  [0, 2, 1, 0]

Before: [3, 3, 2, 1]
8 0 2 3
After:  [3, 3, 2, 1]

Before: [3, 3, 2, 1]
8 0 2 0
After:  [1, 3, 2, 1]

Before: [2, 1, 1, 1]
8 0 1 1
After:  [2, 1, 1, 1]

Before: [1, 1, 2, 2]
2 0 2 2
After:  [1, 1, 0, 2]

Before: [1, 3, 2, 2]
2 0 2 2
After:  [1, 3, 0, 2]

Before: [2, 1, 1, 3]
5 2 1 1
After:  [2, 2, 1, 3]

Before: [2, 1, 3, 2]
8 0 1 1
After:  [2, 1, 3, 2]

Before: [0, 1, 3, 3]
15 2 1 1
After:  [0, 0, 3, 3]

Before: [1, 1, 2, 1]
0 1 0 3
After:  [1, 1, 2, 1]

Before: [3, 2, 0, 3]
6 1 3 1
After:  [3, 0, 0, 3]

Before: [2, 1, 2, 2]
8 0 1 3
After:  [2, 1, 2, 1]

Before: [0, 3, 0, 0]
10 0 0 0
After:  [0, 3, 0, 0]

Before: [3, 1, 1, 0]
5 2 1 3
After:  [3, 1, 1, 2]

Before: [1, 1, 0, 2]
3 0 2 2
After:  [1, 1, 0, 2]

Before: [0, 1, 2, 3]
6 1 3 1
After:  [0, 0, 2, 3]

Before: [0, 3, 1, 1]
13 3 3 1
After:  [0, 0, 1, 1]

Before: [0, 1, 2, 1]
7 3 1 1
After:  [0, 0, 2, 1]

Before: [1, 0, 0, 0]
3 0 2 0
After:  [0, 0, 0, 0]

Before: [3, 1, 2, 1]
1 1 3 2
After:  [3, 1, 1, 1]

Before: [1, 3, 2, 1]
2 0 2 0
After:  [0, 3, 2, 1]

Before: [0, 1, 2, 3]
12 1 2 1
After:  [0, 0, 2, 3]

Before: [1, 1, 0, 2]
13 3 3 1
After:  [1, 0, 0, 2]

Before: [0, 1, 2, 3]
10 0 0 1
After:  [0, 0, 2, 3]

Before: [1, 3, 2, 0]
2 0 2 3
After:  [1, 3, 2, 0]

Before: [1, 1, 2, 1]
1 1 3 0
After:  [1, 1, 2, 1]

Before: [1, 1, 2, 0]
12 1 2 3
After:  [1, 1, 2, 0]

Before: [2, 3, 1, 1]
11 0 3 3
After:  [2, 3, 1, 1]

Before: [3, 3, 0, 2]
7 0 2 3
After:  [3, 3, 0, 1]

Before: [0, 3, 0, 1]
10 0 0 1
After:  [0, 0, 0, 1]

Before: [3, 3, 1, 2]
13 3 3 1
After:  [3, 0, 1, 2]

Before: [1, 1, 3, 2]
0 1 0 3
After:  [1, 1, 3, 1]

Before: [3, 3, 2, 2]
8 0 2 0
After:  [1, 3, 2, 2]

Before: [3, 2, 1, 0]
14 2 1 0
After:  [2, 2, 1, 0]

Before: [1, 1, 3, 2]
13 3 3 2
After:  [1, 1, 0, 2]

Before: [2, 1, 2, 2]
7 3 2 1
After:  [2, 0, 2, 2]

Before: [1, 3, 2, 1]
2 0 2 1
After:  [1, 0, 2, 1]

Before: [1, 1, 3, 1]
0 1 0 1
After:  [1, 1, 3, 1]

Before: [2, 0, 3, 1]
11 0 3 1
After:  [2, 1, 3, 1]

Before: [0, 2, 1, 0]
14 2 1 0
After:  [2, 2, 1, 0]

Before: [1, 1, 3, 1]
9 1 2 1
After:  [1, 0, 3, 1]

Before: [3, 1, 3, 3]
9 1 2 3
After:  [3, 1, 3, 0]

Before: [2, 0, 2, 1]
4 3 2 3
After:  [2, 0, 2, 1]

Before: [1, 1, 2, 2]
12 1 2 0
After:  [0, 1, 2, 2]

Before: [2, 0, 3, 1]
7 2 0 0
After:  [1, 0, 3, 1]

Before: [1, 3, 2, 2]
7 3 2 2
After:  [1, 3, 0, 2]

Before: [1, 1, 1, 0]
0 1 0 1
After:  [1, 1, 1, 0]

Before: [2, 2, 1, 3]
14 2 1 1
After:  [2, 2, 1, 3]

Before: [1, 3, 3, 1]
13 3 3 3
After:  [1, 3, 3, 0]

Before: [3, 2, 2, 3]
6 1 3 1
After:  [3, 0, 2, 3]

Before: [1, 1, 0, 0]
3 0 2 1
After:  [1, 0, 0, 0]

Before: [1, 2, 1, 3]
14 2 1 3
After:  [1, 2, 1, 2]

Before: [3, 2, 2, 2]
7 3 2 2
After:  [3, 2, 0, 2]

Before: [1, 2, 0, 2]
3 0 2 3
After:  [1, 2, 0, 0]

Before: [0, 1, 2, 1]
1 1 3 0
After:  [1, 1, 2, 1]

Before: [1, 1, 0, 1]
3 0 2 3
After:  [1, 1, 0, 0]

Before: [0, 2, 3, 0]
10 0 0 3
After:  [0, 2, 3, 0]

Before: [2, 1, 2, 3]
12 1 2 0
After:  [0, 1, 2, 3]

Before: [2, 1, 2, 2]
12 1 2 0
After:  [0, 1, 2, 2]

Before: [0, 1, 3, 2]
10 0 0 3
After:  [0, 1, 3, 0]

Before: [3, 0, 2, 1]
4 3 2 3
After:  [3, 0, 2, 1]

Before: [1, 2, 2, 3]
15 2 1 3
After:  [1, 2, 2, 1]

Before: [0, 0, 1, 2]
10 0 0 1
After:  [0, 0, 1, 2]

Before: [1, 2, 1, 2]
14 2 1 0
After:  [2, 2, 1, 2]

Before: [2, 1, 3, 3]
9 1 2 2
After:  [2, 1, 0, 3]

Before: [2, 2, 2, 2]
15 2 0 0
After:  [1, 2, 2, 2]

Before: [1, 1, 3, 2]
9 1 2 2
After:  [1, 1, 0, 2]

Before: [1, 2, 0, 2]
13 3 3 3
After:  [1, 2, 0, 0]

Before: [0, 2, 1, 0]
14 2 1 2
After:  [0, 2, 2, 0]

Before: [2, 2, 1, 1]
13 3 3 2
After:  [2, 2, 0, 1]

Before: [2, 1, 1, 2]
7 2 1 3
After:  [2, 1, 1, 0]

Before: [2, 0, 3, 2]
13 3 3 1
After:  [2, 0, 3, 2]

Before: [0, 2, 1, 1]
14 2 1 0
After:  [2, 2, 1, 1]

Before: [1, 2, 2, 1]
2 0 2 2
After:  [1, 2, 0, 1]

Before: [0, 1, 1, 3]
10 0 0 0
After:  [0, 1, 1, 3]

Before: [0, 3, 2, 2]
7 3 2 1
After:  [0, 0, 2, 2]

Before: [0, 1, 1, 2]
5 2 1 2
After:  [0, 1, 2, 2]

Before: [1, 1, 2, 0]
2 0 2 1
After:  [1, 0, 2, 0]

Before: [0, 1, 3, 1]
13 3 3 2
After:  [0, 1, 0, 1]

Before: [0, 2, 1, 3]
14 2 1 2
After:  [0, 2, 2, 3]

Before: [0, 1, 2, 3]
12 1 2 2
After:  [0, 1, 0, 3]

Before: [2, 1, 2, 0]
8 0 1 2
After:  [2, 1, 1, 0]

Before: [0, 1, 0, 1]
1 1 3 1
After:  [0, 1, 0, 1]

Before: [2, 2, 2, 1]
4 3 2 3
After:  [2, 2, 2, 1]

Before: [0, 0, 1, 0]
10 0 0 3
After:  [0, 0, 1, 0]

Before: [2, 1, 3, 0]
8 0 1 2
After:  [2, 1, 1, 0]

Before: [0, 1, 3, 1]
9 1 2 0
After:  [0, 1, 3, 1]

Before: [1, 0, 2, 1]
4 3 2 2
After:  [1, 0, 1, 1]

Before: [1, 1, 3, 1]
1 1 3 3
After:  [1, 1, 3, 1]

Before: [3, 1, 2, 2]
15 2 2 2
After:  [3, 1, 1, 2]

Before: [2, 3, 3, 2]
7 2 0 2
After:  [2, 3, 1, 2]

Before: [1, 1, 3, 1]
15 2 1 2
After:  [1, 1, 0, 1]

Before: [2, 2, 1, 2]
14 2 1 0
After:  [2, 2, 1, 2]

Before: [2, 2, 1, 0]
14 2 1 1
After:  [2, 2, 1, 0]

Before: [0, 2, 3, 1]
13 3 3 3
After:  [0, 2, 3, 0]

Before: [2, 1, 0, 2]
8 0 1 1
After:  [2, 1, 0, 2]

Before: [1, 3, 2, 3]
2 0 2 2
After:  [1, 3, 0, 3]

Before: [0, 0, 2, 0]
10 0 0 0
After:  [0, 0, 2, 0]

Before: [1, 1, 1, 1]
7 3 1 3
After:  [1, 1, 1, 0]

Before: [2, 1, 1, 1]
1 1 3 3
After:  [2, 1, 1, 1]

Before: [3, 2, 1, 2]
14 2 1 3
After:  [3, 2, 1, 2]

Before: [2, 2, 0, 1]
11 0 3 2
After:  [2, 2, 1, 1]

Before: [0, 1, 3, 1]
1 1 3 1
After:  [0, 1, 3, 1]

Before: [0, 2, 0, 2]
10 0 0 2
After:  [0, 2, 0, 2]

Before: [2, 2, 1, 3]
6 1 3 2
After:  [2, 2, 0, 3]

Before: [1, 3, 0, 2]
3 0 2 0
After:  [0, 3, 0, 2]

Before: [3, 1, 1, 0]
7 2 1 0
After:  [0, 1, 1, 0]

Before: [1, 1, 0, 1]
0 1 0 1
After:  [1, 1, 0, 1]

Before: [3, 1, 3, 0]
9 1 2 1
After:  [3, 0, 3, 0]

Before: [1, 2, 0, 1]
3 0 2 3
After:  [1, 2, 0, 0]

Before: [3, 0, 2, 1]
13 3 3 0
After:  [0, 0, 2, 1]

Before: [2, 1, 2, 2]
13 3 3 2
After:  [2, 1, 0, 2]

Before: [1, 1, 3, 1]
9 1 2 3
After:  [1, 1, 3, 0]

Before: [1, 1, 3, 1]
1 1 3 2
After:  [1, 1, 1, 1]

Before: [2, 1, 2, 1]
8 0 1 0
After:  [1, 1, 2, 1]

Before: [3, 1, 3, 3]
6 1 3 3
After:  [3, 1, 3, 0]

Before: [0, 3, 1, 2]
10 0 0 3
After:  [0, 3, 1, 0]

Before: [0, 1, 2, 0]
12 1 2 0
After:  [0, 1, 2, 0]

Before: [2, 0, 3, 1]
13 3 3 0
After:  [0, 0, 3, 1]

Before: [0, 1, 1, 3]
6 1 3 1
After:  [0, 0, 1, 3]

Before: [0, 1, 2, 2]
12 1 2 1
After:  [0, 0, 2, 2]

Before: [2, 0, 2, 2]
7 0 1 2
After:  [2, 0, 1, 2]

Before: [1, 0, 2, 2]
2 0 2 1
After:  [1, 0, 2, 2]

Before: [3, 0, 2, 1]
4 3 2 0
After:  [1, 0, 2, 1]

Before: [1, 1, 1, 0]
0 1 0 2
After:  [1, 1, 1, 0]

Before: [3, 3, 2, 1]
4 3 2 2
After:  [3, 3, 1, 1]

Before: [1, 1, 2, 2]
12 1 2 3
After:  [1, 1, 2, 0]

Before: [3, 2, 3, 3]
15 3 1 2
After:  [3, 2, 0, 3]

Before: [0, 1, 3, 2]
9 1 2 1
After:  [0, 0, 3, 2]

Before: [2, 1, 0, 1]
1 1 3 1
After:  [2, 1, 0, 1]

Before: [0, 1, 3, 1]
9 1 2 1
After:  [0, 0, 3, 1]

Before: [1, 2, 1, 2]
14 2 1 2
After:  [1, 2, 2, 2]

Before: [3, 1, 0, 1]
1 1 3 1
After:  [3, 1, 0, 1]

Before: [2, 1, 1, 3]
5 2 1 3
After:  [2, 1, 1, 2]

Before: [3, 2, 2, 1]
4 3 2 2
After:  [3, 2, 1, 1]

Before: [2, 1, 2, 1]
4 3 2 2
After:  [2, 1, 1, 1]

Before: [0, 1, 1, 2]
13 3 3 3
After:  [0, 1, 1, 0]

Before: [1, 2, 2, 0]
2 0 2 3
After:  [1, 2, 2, 0]

Before: [0, 2, 1, 3]
6 2 3 2
After:  [0, 2, 0, 3]

Before: [0, 1, 2, 1]
4 3 2 1
After:  [0, 1, 2, 1]

Before: [2, 2, 1, 1]
14 2 1 1
After:  [2, 2, 1, 1]

Before: [2, 1, 2, 3]
12 1 2 2
After:  [2, 1, 0, 3]

Before: [3, 1, 2, 1]
12 1 2 2
After:  [3, 1, 0, 1]

Before: [2, 1, 2, 1]
1 1 3 1
After:  [2, 1, 2, 1]

Before: [1, 2, 2, 0]
2 0 2 1
After:  [1, 0, 2, 0]

Before: [2, 1, 2, 2]
8 0 1 1
After:  [2, 1, 2, 2]

Before: [2, 1, 1, 3]
5 2 1 0
After:  [2, 1, 1, 3]

Before: [3, 1, 3, 3]
9 1 2 2
After:  [3, 1, 0, 3]

Before: [2, 3, 2, 1]
4 3 2 2
After:  [2, 3, 1, 1]

Before: [3, 3, 1, 1]
13 3 3 1
After:  [3, 0, 1, 1]

Before: [0, 1, 1, 2]
10 0 0 3
After:  [0, 1, 1, 0]

Before: [2, 0, 1, 1]
11 0 3 3
After:  [2, 0, 1, 1]

Before: [3, 1, 3, 1]
1 1 3 1
After:  [3, 1, 3, 1]

Before: [2, 1, 3, 1]
9 1 2 2
After:  [2, 1, 0, 1]

Before: [0, 1, 2, 1]
10 0 0 3
After:  [0, 1, 2, 0]

Before: [1, 0, 2, 2]
2 0 2 0
After:  [0, 0, 2, 2]

Before: [0, 1, 3, 3]
9 1 2 2
After:  [0, 1, 0, 3]

Before: [1, 1, 0, 3]
0 1 0 3
After:  [1, 1, 0, 1]

Before: [3, 3, 2, 0]
8 0 2 0
After:  [1, 3, 2, 0]

Before: [1, 1, 2, 3]
12 1 2 1
After:  [1, 0, 2, 3]

Before: [2, 1, 2, 1]
12 1 2 0
After:  [0, 1, 2, 1]

Before: [1, 0, 2, 1]
4 3 2 0
After:  [1, 0, 2, 1]

Before: [1, 2, 0, 2]
3 0 2 2
After:  [1, 2, 0, 2]

Before: [2, 3, 2, 1]
4 3 2 3
After:  [2, 3, 2, 1]

Before: [0, 1, 2, 1]
1 1 3 1
After:  [0, 1, 2, 1]

Before: [2, 1, 2, 1]
11 0 3 3
After:  [2, 1, 2, 1]

Before: [0, 0, 2, 1]
4 3 2 1
After:  [0, 1, 2, 1]

Before: [2, 1, 2, 2]
15 2 0 0
After:  [1, 1, 2, 2]

Before: [2, 1, 3, 1]
9 1 2 3
After:  [2, 1, 3, 0]

Before: [1, 1, 3, 0]
9 1 2 1
After:  [1, 0, 3, 0]

Before: [0, 1, 1, 1]
13 3 3 3
After:  [0, 1, 1, 0]

Before: [2, 3, 1, 3]
6 2 3 2
After:  [2, 3, 0, 3]

Before: [2, 1, 1, 1]
1 1 3 1
After:  [2, 1, 1, 1]

Before: [0, 3, 1, 3]
10 0 0 3
After:  [0, 3, 1, 0]

Before: [2, 1, 3, 2]
9 1 2 1
After:  [2, 0, 3, 2]

Before: [2, 2, 2, 1]
13 3 3 0
After:  [0, 2, 2, 1]

Before: [3, 3, 2, 3]
8 0 2 2
After:  [3, 3, 1, 3]

Before: [1, 1, 0, 2]
0 1 0 1
After:  [1, 1, 0, 2]

Before: [1, 2, 2, 3]
2 0 2 2
After:  [1, 2, 0, 3]

Before: [1, 1, 1, 3]
5 2 1 3
After:  [1, 1, 1, 2]

Before: [2, 1, 1, 1]
8 0 1 3
After:  [2, 1, 1, 1]

Before: [0, 2, 1, 3]
14 2 1 1
After:  [0, 2, 1, 3]

Before: [1, 1, 0, 3]
3 0 2 1
After:  [1, 0, 0, 3]

Before: [0, 1, 1, 0]
5 2 1 3
After:  [0, 1, 1, 2]

Before: [3, 0, 0, 1]
7 0 2 0
After:  [1, 0, 0, 1]

Before: [2, 1, 3, 0]
9 1 2 1
After:  [2, 0, 3, 0]

Before: [2, 1, 1, 3]
6 1 3 2
After:  [2, 1, 0, 3]

Before: [1, 1, 0, 0]
0 1 0 2
After:  [1, 1, 1, 0]

Before: [2, 1, 0, 1]
1 1 3 0
After:  [1, 1, 0, 1]

Before: [3, 1, 1, 1]
1 1 3 2
After:  [3, 1, 1, 1]

Before: [0, 3, 1, 1]
13 2 3 3
After:  [0, 3, 1, 0]

Before: [2, 2, 1, 0]
14 2 1 3
After:  [2, 2, 1, 2]

Before: [1, 1, 3, 0]
9 1 2 3
After:  [1, 1, 3, 0]

Before: [2, 2, 0, 1]
11 0 3 0
After:  [1, 2, 0, 1]

Before: [1, 1, 2, 1]
4 3 2 1
After:  [1, 1, 2, 1]

Before: [2, 1, 2, 1]
11 0 3 2
After:  [2, 1, 1, 1]

Before: [2, 0, 3, 3]
7 2 0 2
After:  [2, 0, 1, 3]

Before: [3, 1, 2, 1]
1 1 3 1
After:  [3, 1, 2, 1]

Before: [1, 1, 2, 1]
1 1 3 2
After:  [1, 1, 1, 1]

Before: [2, 1, 3, 2]
7 2 0 3
After:  [2, 1, 3, 1]

Before: [1, 1, 3, 0]
0 1 0 2
After:  [1, 1, 1, 0]

Before: [0, 2, 3, 3]
15 0 0 1
After:  [0, 1, 3, 3]

Before: [3, 1, 1, 1]
1 1 3 3
After:  [3, 1, 1, 1]

Before: [0, 0, 1, 3]
6 2 3 3
After:  [0, 0, 1, 0]

Before: [2, 1, 0, 1]
7 3 1 1
After:  [2, 0, 0, 1]

Before: [1, 1, 3, 1]
15 2 3 3
After:  [1, 1, 3, 0]

Before: [1, 1, 3, 2]
0 1 0 1
After:  [1, 1, 3, 2]

Before: [0, 1, 3, 3]
6 1 3 1
After:  [0, 0, 3, 3]

Before: [0, 1, 2, 3]
6 2 3 3
After:  [0, 1, 2, 0]

Before: [0, 2, 3, 3]
10 0 0 0
After:  [0, 2, 3, 3]

Before: [2, 1, 0, 0]
8 0 1 2
After:  [2, 1, 1, 0]

Before: [2, 1, 3, 0]
15 2 1 1
After:  [2, 0, 3, 0]

Before: [0, 2, 1, 3]
15 3 1 0
After:  [0, 2, 1, 3]

Before: [0, 1, 3, 1]
1 1 3 0
After:  [1, 1, 3, 1]

Before: [2, 0, 2, 1]
13 3 3 1
After:  [2, 0, 2, 1]

Before: [2, 2, 1, 3]
6 1 3 3
After:  [2, 2, 1, 0]

Before: [2, 0, 2, 2]
7 3 2 0
After:  [0, 0, 2, 2]

Before: [3, 1, 1, 0]
5 2 1 2
After:  [3, 1, 2, 0]

Before: [2, 1, 3, 1]
8 0 1 3
After:  [2, 1, 3, 1]

Before: [1, 2, 2, 1]
4 3 2 1
After:  [1, 1, 2, 1]

Before: [0, 1, 2, 3]
12 1 2 0
After:  [0, 1, 2, 3]

Before: [1, 1, 2, 1]
0 1 0 0
After:  [1, 1, 2, 1]

Before: [1, 1, 1, 3]
5 2 1 1
After:  [1, 2, 1, 3]";
        
        private const string puzzleInput2 = @"
2 2 3 3
2 0 3 2
2 2 1 0
15 0 3 3
10 3 1 3
5 1 3 1
2 2 3 3
10 1 0 0
14 0 0 0
7 2 3 2
10 2 3 2
5 1 2 1
4 1 1 0
2 3 2 1
10 1 0 2
14 2 0 2
12 1 3 3
10 3 3 3
5 3 0 0
4 0 0 3
10 0 0 0
14 0 2 0
2 1 1 1
2 3 0 2
13 0 2 1
10 1 1 1
5 1 3 3
2 0 3 2
2 1 1 1
3 1 0 2
10 2 1 2
10 2 2 2
5 3 2 3
4 3 0 1
2 1 3 3
10 3 0 2
14 2 3 2
3 3 0 3
10 3 1 3
10 3 3 3
5 3 1 1
2 0 1 3
2 2 0 2
10 0 0 0
14 0 3 0
0 2 3 3
10 3 1 3
5 1 3 1
4 1 2 3
2 1 1 0
2 0 0 1
4 0 2 2
10 2 3 2
5 2 3 3
4 3 2 1
10 2 0 3
14 3 3 3
2 3 1 2
10 2 0 0
14 0 2 0
12 3 0 0
10 0 3 0
5 0 1 1
4 1 0 2
2 3 2 0
2 2 0 1
2 1 0 3
12 0 1 1
10 1 3 1
5 2 1 2
2 2 3 1
2 3 0 3
2 1 3 0
12 3 1 3
10 3 2 3
5 2 3 2
4 2 2 3
2 3 1 1
2 1 0 2
9 1 2 2
10 2 1 2
5 2 3 3
4 3 2 1
10 1 0 2
14 2 2 2
2 3 1 3
4 0 2 3
10 3 3 3
5 3 1 1
2 0 2 0
2 0 1 3
6 3 2 0
10 0 2 0
5 0 1 1
2 3 2 0
10 1 0 3
14 3 2 3
2 1 3 2
2 2 0 3
10 3 3 3
10 3 2 3
5 3 1 1
2 1 0 3
2 1 2 0
2 3 0 2
10 2 2 2
5 1 2 1
4 1 1 0
2 3 3 1
2 3 0 3
10 2 0 2
14 2 3 2
9 1 2 1
10 1 3 1
5 1 0 0
4 0 3 3
10 1 0 1
14 1 2 1
10 0 0 0
14 0 0 0
1 1 2 0
10 0 1 0
5 0 3 3
4 3 0 1
2 0 3 2
10 0 0 3
14 3 1 3
10 3 0 0
14 0 2 0
11 0 3 2
10 2 2 2
5 1 2 1
2 2 3 3
2 3 1 0
2 2 3 2
12 0 3 2
10 2 1 2
5 1 2 1
2 1 1 0
10 1 0 2
14 2 0 2
7 2 3 0
10 0 1 0
5 0 1 1
4 1 1 3
2 1 1 1
2 2 0 0
2 3 1 2
13 0 2 2
10 2 2 2
10 2 1 2
5 2 3 3
2 1 1 0
2 2 0 2
2 2 1 1
4 0 2 2
10 2 3 2
10 2 3 2
5 3 2 3
4 3 3 1
2 1 3 3
2 1 2 2
2 2 2 0
11 0 3 0
10 0 2 0
5 1 0 1
4 1 2 2
2 2 2 3
2 1 2 1
2 2 2 0
15 0 3 3
10 3 1 3
10 3 3 3
5 3 2 2
4 2 2 1
2 0 0 0
2 0 3 3
2 2 2 2
6 3 2 2
10 2 1 2
5 1 2 1
4 1 1 0
2 2 2 2
2 2 1 1
10 3 0 3
14 3 2 3
0 1 3 2
10 2 2 2
5 2 0 0
10 0 0 2
14 2 0 2
2 1 0 3
2 0 1 1
14 3 1 1
10 1 2 1
10 1 2 1
5 1 0 0
4 0 3 1
10 1 0 0
14 0 0 0
2 0 1 3
2 2 2 2
0 2 3 0
10 0 3 0
10 0 2 0
5 1 0 1
2 2 2 0
2 1 1 3
11 0 3 3
10 3 2 3
5 3 1 1
4 1 2 2
10 1 0 3
14 3 2 3
10 1 0 1
14 1 3 1
15 0 3 3
10 3 3 3
5 3 2 2
4 2 0 1
10 0 0 3
14 3 1 3
2 3 3 2
13 0 2 0
10 0 3 0
5 0 1 1
2 2 3 3
2 1 0 0
10 0 2 3
10 3 2 3
5 1 3 1
2 1 1 2
2 1 3 3
2 2 0 0
11 0 3 2
10 2 2 2
5 1 2 1
4 1 3 0
2 3 1 1
10 2 0 2
14 2 0 2
9 1 2 3
10 3 1 3
5 0 3 0
4 0 1 1
2 0 0 3
2 1 3 2
2 2 0 0
0 0 3 3
10 3 1 3
5 3 1 1
4 1 3 3
10 1 0 1
14 1 2 1
10 1 0 0
14 0 3 0
2 3 0 2
1 1 0 0
10 0 3 0
5 0 3 3
10 2 0 2
14 2 1 2
2 3 0 1
2 3 0 0
9 1 2 1
10 1 1 1
10 1 1 1
5 3 1 3
4 3 1 1
2 1 2 0
10 1 0 2
14 2 2 2
2 2 2 3
3 0 3 3
10 3 1 3
10 3 2 3
5 1 3 1
4 1 2 2
2 3 2 1
2 2 3 0
10 2 0 3
14 3 1 3
3 3 0 0
10 0 2 0
10 0 3 0
5 0 2 2
4 2 0 1
10 0 0 3
14 3 0 3
2 3 2 2
2 0 0 0
7 3 2 0
10 0 2 0
5 1 0 1
4 1 3 2
2 3 3 1
2 2 0 0
2 3 3 3
8 0 1 0
10 0 1 0
10 0 3 0
5 0 2 2
4 2 2 3
2 2 3 0
2 2 1 2
8 0 1 0
10 0 1 0
10 0 1 0
5 3 0 3
4 3 1 1
2 3 1 0
2 1 2 2
2 0 3 3
9 0 2 3
10 3 3 3
5 3 1 1
2 2 0 2
10 2 0 0
14 0 1 0
2 1 3 3
5 0 3 0
10 0 3 0
5 1 0 1
4 1 2 3
2 1 2 0
10 3 0 2
14 2 0 2
2 0 3 1
10 0 2 1
10 1 1 1
5 1 3 3
4 3 1 1
2 2 0 2
10 2 0 3
14 3 0 3
6 3 2 0
10 0 1 0
10 0 2 0
5 0 1 1
4 1 1 3
2 3 0 1
2 0 2 2
2 3 2 0
13 2 0 0
10 0 1 0
5 3 0 3
4 3 0 2
2 1 0 0
10 3 0 3
14 3 2 3
2 1 2 1
3 1 3 1
10 1 1 1
5 2 1 2
4 2 3 1
2 0 1 3
2 2 1 2
2 0 1 0
6 3 2 3
10 3 1 3
5 3 1 1
4 1 2 2
2 3 2 3
2 2 2 1
2 2 2 0
12 3 1 0
10 0 1 0
5 0 2 2
2 1 0 0
2 2 2 3
3 0 3 3
10 3 1 3
5 2 3 2
4 2 3 1
2 1 3 2
2 3 3 0
2 2 3 3
12 0 3 3
10 3 1 3
10 3 3 3
5 1 3 1
2 0 3 2
2 2 2 0
2 1 0 3
11 0 3 3
10 3 3 3
5 3 1 1
2 3 3 2
2 0 3 3
1 0 2 2
10 2 1 2
5 2 1 1
4 1 0 2
2 3 0 1
10 2 0 3
14 3 2 3
15 0 3 1
10 1 3 1
10 1 1 1
5 1 2 2
4 2 1 1
2 3 3 2
2 0 1 3
2 0 1 0
7 3 2 2
10 2 1 2
5 1 2 1
4 1 0 0
2 0 2 1
2 2 3 2
6 3 2 2
10 2 1 2
5 2 0 0
4 0 0 1
2 0 0 2
2 3 1 0
13 2 0 0
10 0 2 0
5 1 0 1
4 1 3 0
2 2 3 2
2 3 2 1
6 3 2 3
10 3 1 3
10 3 2 3
5 0 3 0
2 1 3 3
2 0 1 2
14 3 1 2
10 2 3 2
5 2 0 0
2 2 0 3
2 0 1 2
7 2 3 1
10 1 1 1
10 1 3 1
5 1 0 0
4 0 3 1
2 0 1 3
10 3 0 0
14 0 3 0
2 2 2 2
6 3 2 3
10 3 2 3
5 3 1 1
2 1 2 0
2 0 3 3
2 3 2 0
10 0 2 0
5 1 0 1
2 3 1 0
2 3 1 3
8 2 0 2
10 2 3 2
5 1 2 1
4 1 2 3
2 0 2 2
2 2 1 1
13 2 0 2
10 2 1 2
10 2 2 2
5 2 3 3
4 3 2 1
2 1 3 0
2 0 3 3
2 2 2 2
0 2 3 0
10 0 2 0
10 0 1 0
5 1 0 1
2 1 1 0
2 1 3 3
5 3 0 2
10 2 1 2
5 2 1 1
4 1 0 3
10 1 0 2
14 2 1 2
2 2 1 0
2 1 3 1
3 1 0 0
10 0 3 0
5 0 3 3
2 2 2 2
10 3 0 1
14 1 2 1
2 1 3 0
4 0 2 1
10 1 3 1
10 1 2 1
5 3 1 3
4 3 0 1
2 3 0 3
2 2 2 0
2 3 2 2
12 3 0 3
10 3 2 3
5 3 1 1
4 1 1 3
2 3 0 1
9 1 2 0
10 0 3 0
5 0 3 3
4 3 2 1
2 0 2 2
10 2 0 3
14 3 2 3
10 1 0 0
14 0 3 0
7 2 3 0
10 0 1 0
10 0 1 0
5 0 1 1
2 2 1 0
2 2 2 2
15 0 3 0
10 0 2 0
5 0 1 1
4 1 0 0
2 0 2 1
2 0 2 3
2 3 3 2
7 3 2 3
10 3 2 3
5 0 3 0
4 0 2 2
2 2 0 0
2 1 3 3
2 2 1 1
11 0 3 3
10 3 2 3
5 2 3 2
2 1 3 3
2 0 1 1
11 0 3 1
10 1 1 1
5 1 2 2
4 2 3 0
2 2 2 2
2 3 1 1
8 2 1 3
10 3 2 3
10 3 1 3
5 0 3 0
10 0 0 3
14 3 1 3
2 0 1 2
10 1 0 1
14 1 0 1
14 3 1 2
10 2 1 2
10 2 1 2
5 0 2 0
4 0 1 1
2 1 2 2
2 2 0 3
10 3 0 0
14 0 1 0
3 0 3 2
10 2 3 2
5 2 1 1
2 0 2 2
2 1 2 3
10 1 0 0
14 0 0 0
10 3 2 3
10 3 3 3
5 1 3 1
4 1 3 0
2 1 0 2
10 1 0 1
14 1 3 1
2 1 0 3
5 3 3 3
10 3 1 3
10 3 2 3
5 0 3 0
2 0 3 2
2 2 0 3
2 2 3 1
7 2 3 1
10 1 2 1
10 1 2 1
5 0 1 0
4 0 1 2
2 1 1 1
2 3 2 3
10 0 0 0
14 0 2 0
3 1 0 1
10 1 2 1
10 1 2 1
5 2 1 2
4 2 3 0
2 0 1 1
2 3 3 2
2 1 0 3
10 3 2 1
10 1 2 1
5 0 1 0
4 0 3 1
2 1 3 2
2 2 0 0
2 2 0 3
15 0 3 2
10 2 2 2
5 1 2 1
2 1 0 0
2 2 2 2
2 1 0 3
4 0 2 0
10 0 3 0
5 0 1 1
2 1 2 0
2 0 0 3
4 0 2 0
10 0 1 0
10 0 2 0
5 0 1 1
4 1 1 0
2 0 0 1
2 2 1 3
2 0 2 2
7 2 3 2
10 2 1 2
5 0 2 0
2 2 0 1
2 0 3 2
7 2 3 2
10 2 3 2
5 2 0 0
4 0 1 3
2 1 0 0
2 2 1 2
2 1 1 1
5 1 0 0
10 0 1 0
5 0 3 3
4 3 2 0
2 3 2 1
2 1 0 2
10 0 0 3
14 3 3 3
9 1 2 2
10 2 1 2
10 2 2 2
5 0 2 0
4 0 1 2
2 2 3 1
2 0 2 3
2 1 2 0
5 0 0 3
10 3 2 3
5 2 3 2
4 2 3 3
10 1 0 1
14 1 0 1
2 2 2 2
4 0 2 1
10 1 3 1
10 1 3 1
5 3 1 3
4 3 2 0
2 2 0 1
2 3 1 3
10 0 0 2
14 2 0 2
9 3 2 1
10 1 1 1
10 1 2 1
5 0 1 0
4 0 2 2
2 2 0 3
2 3 1 1
2 2 2 0
15 0 3 0
10 0 1 0
10 0 2 0
5 0 2 2
2 2 1 0
2 0 0 1
2 1 0 3
14 3 1 1
10 1 1 1
5 2 1 2
2 1 3 1
11 0 3 0
10 0 1 0
5 0 2 2
4 2 1 0
2 0 3 1
2 0 1 3
2 2 1 2
0 2 3 3
10 3 2 3
5 3 0 0
4 0 2 1
10 3 0 3
14 3 1 3
2 2 3 0
2 1 1 2
3 3 0 0
10 0 1 0
10 0 2 0
5 1 0 1
2 1 3 0
2 2 0 2
4 0 2 2
10 2 1 2
5 1 2 1
10 2 0 3
14 3 0 3
2 2 2 2
6 3 2 2
10 2 1 2
5 2 1 1
2 2 1 2
2 3 2 0
8 2 0 0
10 0 2 0
5 1 0 1
4 1 2 3
2 0 0 1
2 3 2 0
2 3 3 2
9 0 2 1
10 1 2 1
10 1 1 1
5 3 1 3
4 3 2 0
10 1 0 1
14 1 1 1
2 2 2 3
10 1 2 3
10 3 3 3
10 3 3 3
5 0 3 0
4 0 1 2
2 2 2 0
10 3 0 1
14 1 2 1
2 1 2 3
11 0 3 1
10 1 2 1
5 1 2 2
4 2 2 1
2 3 1 2
11 0 3 0
10 0 3 0
5 0 1 1
4 1 1 3
10 3 0 1
14 1 1 1
2 2 0 0
1 0 2 1
10 1 3 1
5 1 3 3
4 3 0 1
2 1 3 0
2 3 0 3
2 2 2 2
4 0 2 2
10 2 3 2
5 1 2 1
10 0 0 2
14 2 3 2
2 2 0 0
13 0 2 0
10 0 1 0
10 0 3 0
5 1 0 1
4 1 0 3
2 1 2 0
10 1 0 1
14 1 3 1
9 1 2 0
10 0 1 0
5 3 0 3
4 3 0 1
2 2 3 2
2 2 3 0
2 0 2 3
6 3 2 0
10 0 2 0
5 1 0 1
4 1 3 0
2 3 3 2
2 1 1 1
10 2 0 3
14 3 3 3
10 1 2 1
10 1 3 1
10 1 2 1
5 0 1 0
4 0 2 3
2 0 1 2
10 1 0 1
14 1 1 1
2 2 3 0
3 1 0 0
10 0 3 0
5 0 3 3
4 3 3 1
2 3 0 0
2 0 1 3
2 3 2 2
9 0 2 3
10 3 3 3
5 1 3 1
4 1 0 0
2 2 0 3
10 3 0 2
14 2 2 2
2 1 0 1
3 1 3 2
10 2 1 2
5 0 2 0
2 0 0 2
10 3 0 1
14 1 2 1
2 3 3 3
12 3 1 2
10 2 1 2
5 0 2 0
4 0 0 1
2 1 2 3
2 3 1 2
2 1 1 0
5 0 3 3
10 3 3 3
5 1 3 1
4 1 0 0
2 3 1 1
2 0 2 3
7 3 2 2
10 2 3 2
10 2 1 2
5 2 0 0
4 0 2 1
2 1 2 3
2 3 3 0
2 2 3 2
8 2 0 3
10 3 1 3
5 3 1 1
10 3 0 2
14 2 0 2
2 1 0 3
13 2 0 2
10 2 1 2
5 1 2 1
4 1 0 0
10 0 0 2
14 2 1 2
2 2 2 1
2 3 0 3
9 3 2 1
10 1 2 1
5 1 0 0
4 0 1 1
10 3 0 2
14 2 3 2
2 3 2 0
2 2 3 2
10 2 3 2
10 2 1 2
5 2 1 1
4 1 0 0";

        [Fact] public void Solution_1_test_example() => Assert.Equal(1, Solve1(testInput));
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(0, Solve1(puzzleInput1));

        public int Solve1(string input)
        {
            var data = input.SplitByNewline(shouldTrim: true);

            var output = 0;

            for (int i = 0; i < data.Length; i++)
            {
                int[] before = data[i].Replace("Before: [", "").Replace("]", "").Split(", ").Select(int.Parse).ToArray();
                int[] instructions = data[++i].Split().Select(int.Parse).ToArray();
                int[] after = data[++i].Replace("After:  [", "").Replace("]", "").Split(", ").Select(int.Parse).ToArray();

                var applicable = 0;

                for (int n = 0; n < 16; n++)
                {
                    instructions[0] = n;
                    int[] clone = new[] { before[0], before[1], before[2], before[3] };
                    Doop(instructions, clone);
                    if (clone[0] == after[0]
                        && clone[1] == after[1]
                        && clone[2] == after[2]
                        && clone[3] == after[3])
                    {
                        applicable++;
                    }
                }

                if (applicable >= 3) output++;
            }

            // Not: 274
            return output;
        }

        private const int a = 0, b = 1, c = 2;

        private void Doop(int[] instructions, int[] registers)
        {
            switch (instructions[0])
            {
                case 0: // adr
                    registers[c] = instructions[a] + instructions[b];
                    break;
                case 1: // addi
                    registers[c] = instructions[a] + registers[b];
                    break;
                case 2: // mulr
                    registers[c] = instructions[a] * instructions[b];
                    break;
                case 3: // muli
                    registers[c] = instructions[a] * registers[b];
                    break;
                case 4: // banr
                    registers[c] = instructions[a] & instructions[b];
                    break;
                case 5: // bani
                    registers[c] = instructions[a] & registers[b];
                    break;
                case 6: // borr
                    registers[c] = instructions[a] & instructions[b];
                    break;
                case 7: // bori
                    registers[c] = instructions[a] & registers[b];
                    break;
                case 8: // setr
                    registers[c] = instructions[a];
                    break;
                case 9: // seti
                    registers[c] = registers[a];
                    break;
                case 10: // gtir
                    registers[c] = (registers[a] > instructions[b]) ? 1 : 0;
                    break;
                case 11: // gtri
                    registers[c] = (instructions[a] > registers[b]) ? 1 : 0;
                    break;
                case 12: // gtrr
                    registers[c] = (instructions[a] > instructions[b]) ? 1 : 0;
                    break;
                case 13: // eqir
                    registers[c] = (registers[a] == instructions[b]) ? 1 : 0;
                    break;
                case 14: // eqri
                    registers[c] = (instructions[a] == registers[b]) ? 1 : 0;
                    break;
                case 15:
                    registers[c] = (instructions[a] == instructions[b]) ? 1 : 0;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
