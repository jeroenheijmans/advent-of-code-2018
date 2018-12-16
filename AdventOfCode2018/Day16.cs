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
        [Fact] public void Solution_1_test_real_input() => Assert.Equal(640, Solve1(puzzleInput1));

        [Fact] public void Solution_2_test_real_input() => Assert.Equal(-1, Solve2(puzzleInput1));

        public int Solve1(string input)
        {
            var output = 0;
            var data = input.SplitByNewline(shouldTrim: true);
            var entries = new List<(int[] before, int[] instructions, int[] after)>();

            for (int i = 0; i < data.Length; i++)
            {
                int[] before = data[i].Replace("Before: [", "").Replace("]", "").Split(", ").Select(int.Parse).ToArray();
                int[] instructions = data[++i].Split().Select(int.Parse).ToArray();
                int[] after = data[++i].Replace("After:  [", "").Replace("]", "").Split(", ").Select(int.Parse).ToArray();

                entries.Add((before, instructions, after));
            }

            foreach (var (before, instructions, after) in entries)
            {
                var applicable = 0;

                for (int n = 0; n < 16; n++)
                {
                    instructions[0] = n;

                    if (IsOpPossible(instructions, before, after))
                    {
                        applicable++;
                    }
                }

                if (applicable >= 3) output++;
            }

            return output;
        }

        public int Solve2(string input)
        {
            var data = input.SplitByNewline(shouldTrim: true);
            var entries = new List<(int[] before, int[] instructions, int[] after)>();

            for (int i = 0; i < data.Length; i++)
            {
                int[] before = data[i].Replace("Before: [", "").Replace("]", "").Split(", ").Select(int.Parse).ToArray();
                int[] instructions = data[++i].Split().Select(int.Parse).ToArray();
                int[] after = data[++i].Replace("After:  [", "").Replace("]", "").Split(", ").Select(int.Parse).ToArray();

                entries.Add((before, instructions, after));
            }

            var mappings = Enumerable.Range(0, 16)
                .ToDictionary(i => i, _ => Enumerable.Range(0, 16).ToHashSet());

            foreach (var (before, instructions, after) in entries)
            {
                var realOpCode = instructions[0];

                for (int n = 0; n < 16; n++)
                {
                    if (!mappings[realOpCode].Contains(n)) continue;

                    instructions[0] = n;

                    if (!IsOpPossible(instructions, before, after))
                    {
                        mappings[realOpCode].Remove(n);
                    }
                }
            }

            var acted = false;
            do
            {
                acted = false;

                // Pass 1
                foreach (var single in mappings.Where(m => m.Value.Count == 1))
                {
                    foreach (var map in mappings.Where(m => m.Value.Count > 1))
                    {
                        if (map.Value.Contains(single.Value.Single()))
                        {
                            acted = acted || map.Value.Remove(single.Value.Single());
                        }
                    }
                }

                // Pass 2
                foreach (var map in mappings.Where(m => m.Value.Count > 1).ToArray())
                {
                    foreach (var val in map.Value)
                    {
                        var combinedOthers = mappings
                            .Where(m => m.Key != map.Key)
                            .SelectMany(m => m.Value)
                            .ToHashSet();

                        if (!combinedOthers.Contains(val))
                        {
                            mappings[map.Key] = new HashSet<int> { val };
                            acted = true;
                            break;
                        }
                    }
                }

            } while (acted);
            
            OutputMappings(mappings);

            // Not: 3 (just a guess :D)
            return 3;
        }

        private void OutputMappings(Dictionary<int, HashSet<int>> mappings)
        {
            foreach (var kvp in mappings)
            {
                output.WriteLine(";" + string.Join(";", kvp.Value));
            }
            output.WriteLine("");
        }

        private bool IsOpPossible(int[] instructions, int[] registers, int[] expected)
        {
            var clone = Clone(registers);

            Doop(instructions, clone);

            return (clone[0] == expected[0]
                    && clone[1] == expected[1]
                    && clone[2] == expected[2]
                    && clone[3] == expected[3]);
        }

        private static int[] Clone(int[] before)
        {
            return new[] { before[0], before[1], before[2], before[3] };
        }

        [Theory]
        [InlineData(mulr)]
        [InlineData(addi)]
        [InlineData(seti)]
        public void Doop_aoc_example_puzzle1(int op)
        {
            var registers = new[] { 3, 2, 1, 1 };
            Doop(new[] { op, 2, 1, 2 }, registers);
            Assert.Equal(new[] { 3, 2, 2, 1 }, registers);
        }

        #region OpCodes
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
        #endregion

        private void Doop(int[] instructions, int[] registers)
        {
            int a = instructions[1], b = instructions[2], c = instructions[3];

            switch (instructions[0])
            {
                case addr: // stores into register C the result of adding register A and register B
                    registers[c] = registers[a] + registers[b];
                    break;
                case addi: // stores into register C the result of adding register A and value B
                    registers[c] = b + registers[a];
                    break;

                case mulr: // stores into register C the result of multiplying register A and register B
                    registers[c] = a * b;
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
                    registers[c] = registers[a] & registers[b];
                    break;
                case bori: // stores into register C the result of the bitwise OR of register A and value B
                    registers[c] = registers[a] & b;
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
