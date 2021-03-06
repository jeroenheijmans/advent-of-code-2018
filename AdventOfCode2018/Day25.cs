﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day25
    {
        private readonly ITestOutputHelper output;

        public Day25(ITestOutputHelper output)
        {
            this.output = output;
        }

        private const string testInput = @"
 0,0,0,0
 3,0,0,0
 0,3,0,0
 0,0,3,0
 0,0,0,3
 0,0,0,6
 9,0,0,0
12,0,0,0
";

        private const string puzzleInput = @"
-1,-7,-2,-6
6,-6,6,0
0,8,-5,-3
-1,-5,3,2
4,3,0,-7
0,5,0,3
-1,4,0,-5
5,-7,5,-4
6,4,1,1
4,-3,-1,4
4,2,3,-3
4,5,8,6
-7,8,-4,-1
3,-3,0,-5
-1,4,8,2
-7,6,-7,-3
-3,-6,1,-8
7,0,0,-2
-3,2,-3,2
8,-8,-8,-2
-5,8,-3,1
-6,-7,5,5
1,-3,7,-3
-4,7,0,0
7,-3,3,-4
2,3,-6,6
0,-6,6,4
-4,-2,8,-5
0,4,-4,6
8,-7,-5,2
-5,-5,-8,6
0,2,7,-7
8,1,-7,0
-5,0,5,4
-7,-8,-6,4
7,-7,3,-4
0,-2,-2,-7
-1,0,1,-5
-6,-6,0,2
-4,6,8,6
1,8,2,5
5,-1,1,3
6,6,-1,-1
-1,6,-5,4
8,-3,-4,3
8,-3,-6,5
-3,6,-8,-3
-7,0,5,-7
-3,-2,1,-5
-3,-7,0,3
0,5,-5,-3
8,-1,-4,0
4,0,7,-2
-7,8,-5,0
1,-2,-6,2
5,8,-4,-2
5,0,-5,-1
0,3,8,2
7,-1,-2,4
-3,-8,-5,0
2,0,-7,5
8,5,-1,5
-2,-2,5,6
4,-4,0,-1
8,-3,0,2
7,2,6,5
8,-1,-5,-7
-5,-7,2,-1
1,0,2,-2
-4,5,1,0
5,4,-6,2
2,2,-2,8
4,2,-6,-4
-5,3,-2,1
4,4,6,-3
0,0,-7,-2
4,3,0,1
2,0,-5,2
7,2,-2,8
-2,2,4,3
0,8,-7,-8
2,0,2,-1
4,-4,2,0
4,3,8,3
2,8,-7,1
-3,-7,7,2
-6,5,-5,0
1,-7,4,-4
-8,-7,3,-2
8,-3,-3,3
3,4,3,-7
-2,-3,1,7
8,-5,-5,0
2,-4,-8,-4
-4,3,0,4
3,-4,0,7
-1,-6,-3,1
4,0,1,7
-7,7,5,-5
-7,1,3,6
3,-7,-4,6
0,2,2,1
-8,0,4,-5
8,2,5,7
-6,-6,-2,0
-8,1,6,-6
3,6,0,-7
8,-6,8,0
-5,8,8,0
7,-8,1,-2
-3,8,-6,0
-3,0,-7,-6
-8,-6,-4,-2
-2,6,-5,-5
-2,1,3,-2
-7,-7,7,-3
0,-6,5,-7
5,2,-1,-2
-4,3,-7,-5
0,-3,-7,-3
-8,-5,1,-3
-7,-7,6,0
-4,2,2,-6
-1,6,6,4
8,-1,-1,6
-8,-8,-6,-1
7,2,-3,-3
-5,1,-1,5
-6,-6,3,4
-6,0,-5,-1
-6,6,2,-5
-3,-6,-4,0
-2,6,3,0
6,-2,5,5
-3,5,5,0
-2,2,0,7
-4,7,-3,4
-5,4,-3,5
8,1,-4,-5
2,-7,7,5
0,-6,2,-2
5,0,6,8
2,-4,8,7
-3,-2,7,5
7,0,-7,6
5,-3,-8,-5
-8,3,2,4
-8,8,-2,5
1,3,-6,-8
-7,3,0,3
-8,-2,-2,-8
-4,-8,-1,2
8,-4,-4,-8
0,-7,-7,2
-1,-6,0,-2
8,-5,7,-1
-7,6,-2,5
-4,1,8,4
-5,3,2,6
0,4,-5,0
1,7,-3,0
8,3,7,1
-3,-1,0,1
-5,-5,0,4
1,0,5,3
1,-2,-7,0
0,-7,-6,8
-3,3,4,6
-2,-7,2,-4
-6,-8,-5,2
4,-4,-8,7
1,0,4,5
0,3,7,7
2,-7,0,6
2,-3,2,-4
6,-8,-5,-3
1,-4,0,-5
3,4,5,7
-5,2,5,2
-2,-4,2,0
2,-4,-6,6
-1,4,-7,-4
-2,-2,2,-3
0,4,7,-8
0,0,-2,3
3,6,2,8
-5,5,0,4
0,0,-3,-7
3,3,-5,-6
6,-7,3,-3
0,0,-5,6
5,4,7,4
3,5,-6,0
0,-7,4,-2
-7,0,-1,6
-1,2,-5,-6
3,0,-8,-3
8,4,-2,-4
6,0,6,-3
-8,1,2,-3
8,-2,5,0
3,5,-5,-2
-1,1,-7,6
-2,6,-7,4
7,1,-5,-4
-6,0,0,2
4,6,-4,1
-7,1,6,-4
5,-5,-1,1
-1,0,0,-6
-6,-6,5,3
-2,-5,6,-1
0,-1,4,-3
-8,-4,-5,8
7,4,-8,-7
-4,0,5,3
-1,5,5,-7
1,-7,7,-7
-2,-7,-3,-4
-2,-8,-7,1
6,5,-8,-2
6,4,3,-8
5,6,-7,-5
0,3,4,-4
0,4,5,-5
4,-3,2,1
-5,-2,0,1
-8,1,6,2
8,2,-6,1
-1,-6,2,-3
3,-3,0,-2
4,0,-8,-4
3,-4,8,-7
1,-8,-1,6
8,-1,3,-7
-4,4,-5,0
-8,2,-3,-3
5,-6,-3,-4
-4,-2,-1,-7
8,8,8,-2
0,8,6,-4
-4,3,-4,4
2,0,0,0
2,-8,-4,-6
7,-6,8,-7
0,-1,-5,-1
6,-1,7,0
-3,0,1,0
4,6,6,6
-5,6,4,3
0,-3,0,-8
2,-7,6,-4
5,5,2,8
-3,6,-1,-1
7,-3,4,2
-4,-5,-4,0
-5,3,-1,8
4,-1,0,0
-8,-6,-3,-5
6,4,1,8
2,-7,0,-5
4,-8,4,8
-6,-7,5,4
7,4,2,-2
-4,-2,-7,2
3,-1,-5,2
-5,-2,3,5
3,0,0,1
-4,-3,-3,-6
-6,5,1,0
-7,-2,8,0
-1,2,-2,-3
-3,-1,5,2
3,-1,-2,5
1,6,8,-7
-8,2,4,4
-1,3,5,8
-5,8,6,-3
1,6,-6,8
-5,-4,3,4
-5,0,7,-3
5,7,4,-5
1,6,-3,-6
6,0,5,-5
-5,-4,1,2
-3,-5,2,0
5,4,-8,-2
3,0,-5,-3
2,2,1,8
-7,-1,-4,7
7,1,1,-6
3,-1,-3,-4
0,-2,-4,7
2,5,6,-4
8,4,-5,-5
2,2,-3,-2
7,-5,7,0
-6,0,-2,-3
7,0,6,4
-4,5,7,6
-7,-5,2,-2
-7,7,0,0
-1,1,-4,2
5,-3,-4,-1
5,-2,-2,8
3,6,-3,-4
-6,-5,3,-5
-7,-7,1,6
-8,8,0,4
-8,0,8,3
0,4,8,6
3,1,-8,5
-6,1,7,0
-5,4,7,2
7,-1,8,4
3,-8,6,3
-1,5,-2,-8
0,2,8,7
-5,8,-3,-8
-4,4,5,5
-5,6,3,-4
2,8,2,-2
-2,7,-2,-2
0,-7,1,8
3,3,6,4
3,-6,5,6
-2,6,-2,-8
2,4,2,0
1,4,-5,7
8,-5,7,4
1,3,2,-3
-2,-3,0,2
8,-5,8,1
8,1,4,5
1,-8,-7,1
-7,0,0,-8
5,7,3,1
7,-3,-7,7
-7,4,-3,5
-4,0,3,0
-7,1,-3,5
-2,-2,8,2
2,2,-7,6
-8,3,0,0
-2,3,8,0
7,2,-7,5
3,-8,-2,0
6,3,-1,0
4,-2,-4,4
-5,-7,3,-2
-6,0,-4,2
4,3,0,-8
2,8,1,-8
0,-6,5,-4
-4,-7,-6,3
-6,6,8,7
3,-7,0,0
6,3,8,-8
5,0,-1,0
7,3,-2,7
-3,-3,-3,6
-8,-4,0,3
8,-7,0,-4
-7,-3,-6,0
6,-4,1,3
-5,-8,-1,-5
3,-4,3,-1
-4,-5,1,5
-2,6,-4,0
-5,3,3,0
8,6,-7,0
-4,1,-1,3
7,8,0,0
5,-4,-1,-4
8,2,3,-6
-6,4,-2,0
4,1,5,0
6,-6,3,7
-7,-3,-2,8
5,3,5,0
8,-4,1,-6
5,6,-8,8
-5,-4,-4,1
1,0,-5,2
7,-3,4,0
2,7,1,2
-1,3,3,-2
8,-3,3,-4
3,-1,-6,-6
4,6,0,1
1,6,-4,-4
5,8,-8,-6
-2,5,6,6
0,-6,-6,-7
-5,2,4,8
6,0,5,-4
-4,-8,8,0
-4,-2,-4,8
2,1,6,-4
-1,3,4,-4
2,-1,-1,0
0,-5,5,-2
4,-1,-2,8
-5,-6,6,7
4,1,6,1
7,8,1,3
2,-7,0,-7
-7,-2,-8,7
-4,5,-3,-1
-8,5,-8,1
3,2,3,-3
2,1,6,3
-8,-2,-1,0
8,0,2,-8
4,-7,-8,-7
7,0,-6,3
2,3,0,8
3,6,-4,0
0,2,-3,-4
3,-6,6,8
6,6,-4,-2
2,-1,-7,0
6,7,8,2
-4,0,0,-1
2,-8,-6,8
8,2,-2,1
-5,4,0,8
0,3,-4,3
4,4,-6,7
7,-5,-4,8
5,6,8,6
-6,8,2,-7
-1,4,-7,7
0,-7,-5,-2
-1,7,-4,-6
-8,-8,1,1
0,-6,-7,1
-7,6,5,6
6,-7,7,0
-6,0,0,-2
-2,3,-6,3
-4,3,8,7
2,-6,-8,-4
3,1,-2,-3
7,-4,-2,6
-5,-2,8,7
-6,-3,-6,3
-2,7,6,-6
-8,7,7,6
5,5,0,-2
-2,2,0,-2
5,2,-6,4
-5,-8,-4,-1
-2,7,0,-4
-3,-7,0,-8
5,0,0,3
-8,-2,-7,3
6,-5,-8,-5
0,-7,-1,2
5,-1,2,-1
-8,-3,0,5
4,-5,6,-6
6,1,-8,0
-5,5,-7,4
6,-8,-6,-8
-7,0,8,6
4,2,7,-8
0,1,-5,-4
-7,2,-5,-2
8,-2,4,-8
-5,-7,4,0
-4,4,-5,2
-3,-3,-3,-8
-6,-8,-8,4
1,0,2,-3
-6,3,-2,0
4,-8,2,5
0,-8,-8,-3
8,-6,-3,0
8,6,0,6
8,7,-6,6
5,-3,0,-5
3,3,2,-1
-1,1,7,-6
-3,-7,8,5
-7,0,-5,-4
-8,-1,-7,7
8,6,0,4
-5,3,-7,2
-1,0,-2,0
0,-1,-5,6
7,-7,0,0
-8,-5,-7,-5
8,3,-5,-3
8,-6,8,7
5,4,3,-6
-6,-7,-2,-4
0,-6,1,-4
3,1,4,0
-7,0,0,7
-6,8,-6,1
7,2,0,2
-7,-1,0,4
-2,-3,-8,7
-6,7,0,-3
-3,5,-3,-2
1,-3,-6,2
4,7,3,-1
3,5,8,8
-7,2,6,6
5,-7,0,-6
1,0,7,-2
-6,-4,-2,-2
-5,8,-8,-4
0,-8,-8,-7
2,7,-2,0
5,-4,-1,0
-7,5,-6,3
-4,1,-6,5
-2,-7,-2,-4
0,-6,-3,-6
2,3,3,8
-3,-5,4,7
5,5,-5,-4
8,6,-2,0
8,2,8,0
-3,-7,3,-8
-7,-8,0,6
1,-2,-7,-5
8,-8,-2,-8
5,-3,7,-6
6,-8,3,7
-7,2,-4,-8
3,-8,2,-8
-7,-4,5,4
-4,0,-8,-4
-7,-6,0,2
-8,2,6,8
0,-4,1,-1
-5,5,5,4
-2,-7,-1,0
4,2,3,-6
2,-8,-7,-1
2,-4,3,-8
0,3,-6,3
-4,-6,-6,-7
-2,0,-6,-5
-3,1,-8,8
-7,-3,3,-4
-1,-4,3,0
-4,8,-5,-1
-3,2,-8,-8
3,8,-8,-4
-3,1,6,-7
1,8,-6,8
-5,-3,8,0
8,-2,-3,3
8,-3,6,-3
-7,0,-3,-8
8,4,4,1
7,-7,3,8
-2,7,2,-8
-3,-1,6,7
-1,0,1,-8
6,5,-7,1
7,-4,-6,-6
4,-3,-1,0
-4,1,8,2
-5,-7,-1,1
0,3,7,4
-3,1,-2,0
2,-2,-3,1
-4,1,-4,2
7,4,-8,0
-2,2,0,-5
3,-5,-2,0
-3,6,4,-1
3,8,7,-1
3,-7,0,-7
5,-6,-7,-3
-6,-2,-7,-7
0,8,1,0
6,0,-6,5
7,-3,0,-7
7,-2,2,0
-2,-4,-4,0
-4,-8,-4,5
7,5,1,7
8,-5,-1,5
-3,5,-8,-7
0,-5,8,-2
-6,-5,0,-4
-6,2,5,4
0,2,8,-2
-3,-4,8,0
-3,2,-5,3
2,5,-4,-3
0,-3,-6,-4
8,-6,5,4
-3,0,-3,0
0,4,1,8
8,-1,-6,-4
4,-7,3,6
0,2,-3,3
5,-3,-1,-3
7,2,7,-1
-5,6,3,-3
-7,5,-6,0
-7,-1,2,-3
2,-2,8,6
-8,-6,1,5
3,-1,-6,8
8,3,-3,5
3,4,-6,-8
6,2,7,0
0,0,0,0
0,-2,0,2
2,-4,0,1
-1,8,0,-8
8,-2,8,8
-8,2,-1,-6
3,0,-5,-5
7,-3,0,-6
6,2,-4,7
0,3,4,8
-1,1,0,-3
4,-6,3,-8
-2,3,6,-1
6,7,-5,2
4,-4,-8,1
0,-1,3,-4
7,-4,5,-8
7,5,-3,7
2,1,0,3
4,-8,8,7
2,-7,7,-7
8,3,7,-2
-3,5,8,-2
-5,3,1,-7
-6,1,6,-5
-7,0,-7,-1
0,3,5,3
5,-6,-7,0
0,0,8,0
-2,2,-4,0
-8,2,-8,-3
-8,-6,-8,5
7,-3,4,-4
-3,-7,1,-4
7,-8,4,4
0,8,-8,-6
-5,-6,4,7
-1,1,-2,7
5,3,5,-6
-2,4,4,4
-3,1,5,6
-5,1,3,-6
-2,-6,4,-8
-7,1,-1,8
-8,2,1,4
-8,8,8,5
7,3,-8,1
5,-2,-4,8
8,-1,-1,-3
3,1,-3,-5
-2,8,0,5
-3,0,5,0
4,5,1,-8
1,0,-1,-5
-2,3,4,3
4,0,5,-4
-3,1,-3,1
2,-8,0,5
-2,-7,5,-1
3,-6,-8,4
-5,1,-8,7
2,0,-8,4
-2,-1,-5,-1
4,7,-6,-3
0,1,4,-2
6,5,2,2
0,-8,3,0
-4,-3,-5,6
0,-5,4,5
-2,4,5,-3
2,-5,-7,-6
5,3,7,2
-2,-6,-4,3
-8,2,5,6
-4,-6,-1,-1
-4,5,0,-8
5,2,5,-1
1,-5,-8,-5
-3,5,-4,-2
-5,-4,1,-2
1,0,8,1
-6,-7,-7,-2
7,8,-6,-1
8,0,1,-4
-6,-6,-2,-5
-8,0,2,-7
2,1,1,6
-1,-1,-6,2
5,4,0,-7
-8,1,-2,0
6,-4,-3,-7
8,-2,3,0
-5,6,8,0
2,-8,-5,4
6,-1,-4,4
0,0,8,-5
3,6,0,7
7,-3,8,-2
-8,0,-8,6
6,7,6,0
-3,1,2,3
8,-6,6,7
0,-3,5,-1
6,0,-3,-8
3,2,0,1
1,8,2,-8
5,-5,7,4
2,8,8,3
0,5,6,0
6,-4,0,-1
-4,5,4,1
6,-6,2,0
4,-4,-6,-6
0,7,7,2
4,-1,-3,-2
6,-3,-7,-6
-3,4,1,-1
0,4,-5,7
1,5,-2,0
8,4,-8,2
-4,3,0,5
8,0,7,7
-1,-4,-8,6
5,-3,3,-4
-1,-5,6,0
0,-8,-2,-4
-7,4,1,0
-4,-4,-7,-8
6,-6,8,7
-8,-5,4,0
7,-3,6,7
1,-2,0,5
5,1,-5,4
4,8,5,-5
1,8,0,-8
-3,-4,-8,0
-6,-3,5,3
-2,-3,-3,6
-6,-1,3,5
-8,-4,2,1
7,1,-1,3
2,-6,-3,-7
-1,2,0,2
-3,-7,0,7
6,-1,-7,1
8,1,-1,1
4,0,8,-4
-8,-4,3,2
1,-4,1,2
6,-4,-4,0
6,0,8,-8
3,-8,-8,-6
4,8,-4,5
5,0,7,0
8,0,-3,0
4,-4,-7,4
-7,-4,5,-2
0,-6,5,8
5,0,7,6
-1,-2,0,0
-7,3,-3,5
-2,-4,-6,4
-7,7,6,6
-1,1,-3,-6
3,-5,7,-5
-7,-4,4,7
-8,-5,-7,0
3,6,-7,2
3,1,4,6
-2,0,-8,0
-8,-8,7,-5
1,7,7,8
-1,-2,-5,-1
-7,7,8,-8
5,-3,-7,-2
6,7,8,4
8,2,4,-3
-1,8,-2,-2
-3,-7,-4,1
-6,4,-6,2
8,-4,0,-3
4,-8,0,3
0,-3,-8,-2
6,-6,7,6
-1,-2,3,8
0,3,0,-3
1,8,-2,-3
-5,-1,-1,3
0,1,-8,-7
3,5,-1,6
5,0,-7,6
-8,-3,5,-5
2,7,-8,8
4,2,3,3
2,4,-7,-5
-7,-7,7,-8
-1,4,3,-6
1,4,-5,-3
-2,8,0,-3
-5,2,-2,0
5,-1,2,-4
8,-5,1,6
-4,5,5,0
0,-5,-7,-1
-6,0,-4,-6
-7,7,-8,-8
-3,0,3,5
-2,-6,2,2
-3,3,-6,-8
-2,-3,-3,-8
-4,-7,4,0
3,-6,0,-1
0,-3,0,7
-7,-7,-4,5
-2,1,0,1
-3,3,-7,-8
0,4,-4,-8
-3,3,4,0
1,3,-3,4
-7,2,-6,0
8,1,-8,-3
-8,-2,-8,-2
-1,-6,5,-8
-5,7,1,0
5,0,1,6
0,-6,1,-7
-5,-3,-4,-7
4,-3,-1,-1
-8,2,-5,6
5,2,0,6
-6,2,1,-4
-7,4,-3,-8
2,0,1,-6
0,-2,-4,5
4,-1,8,-8
7,0,2,-1
-5,8,-4,4
4,7,-5,-8
8,7,2,-5
-7,-8,-6,-3
-1,-1,-1,-4
-3,-5,5,0
-5,-1,5,-3
8,2,-8,-6
5,4,4,2
1,2,5,-2
1,7,6,1
0,3,-6,-8
-8,0,-3,1
7,-2,6,7
4,-8,8,8
-4,-2,-2,-3
8,8,1,-6
-4,3,-6,0
-2,5,7,0
-5,7,1,1
-6,7,-7,-5
0,0,-5,-7
5,-8,5,5
-2,6,5,-4
-7,-3,2,5
-2,7,4,-7
3,1,4,5
-7,7,7,-2
-3,0,0,-2
-1,6,-2,-2
1,-7,-6,4
5,-6,5,1
-3,1,-1,-6
-8,-5,0,-3
-4,3,-1,2
-7,2,-7,0
-2,5,-2,0
5,7,5,-4
8,2,-7,-4
-8,-1,0,1
0,-5,-3,4
0,7,6,8
1,0,5,-7
-3,8,0,0
3,5,6,-3
7,6,-1,2
-6,-3,-2,4
8,1,-4,-4
8,-5,-1,0
8,6,-7,-3
-4,7,-3,8
8,-5,-8,5
1,3,5,0
-8,-7,5,5
-4,-1,2,-8
-7,-7,-3,6
4,0,-6,-6
-3,-8,6,-6
-3,-3,6,2
-6,0,-4,-3
-7,-1,-1,-6
1,-8,-2,-6
-1,4,5,-7
-8,-6,-4,-3
-4,8,-3,0
5,2,-7,3
7,-2,7,5
-8,6,6,8
-1,-7,2,-2
-7,8,6,3
-7,4,-6,5
-2,-6,7,0
-1,5,5,2
-7,4,2,7
1,-8,-8,-2
6,3,0,-5
-5,2,8,0
-8,3,1,-7
-1,0,8,-3
-3,-4,2,2
-6,-3,-8,-4
1,-8,-7,7
-5,3,-2,6
-1,-8,0,4
2,7,5,-4
2,7,-8,-3
-4,6,-6,-8
0,4,-4,4
-6,-4,2,7
0,-2,3,7
-7,4,2,-3
8,4,1,0
-1,7,4,5
-5,-4,7,-6
-1,-6,6,-7
-5,-1,0,0
6,-1,2,1
-2,8,-1,-2
5,0,0,-7
-7,-8,5,-6
2,7,-5,-7
4,-8,-5,7
3,4,-8,8
-5,4,2,-1
4,-7,-3,-6
-1,4,2,-7
6,-7,8,1
6,0,-2,-5
-8,-3,-1,-7
-1,6,2,4
8,-6,1,-1
2,8,-5,0
-7,-7,1,0
3,-3,1,-2
-1,-3,-6,0
7,-8,-5,-7
-5,7,1,6
1,8,5,0
-7,3,0,6
-6,-7,0,-1
-5,-1,-4,6
0,-3,-2,-1
0,6,-2,0
0,-2,-2,4
8,0,8,-4
-8,-7,4,8
-4,3,-6,-6
7,-2,0,3
-8,4,5,-2
0,5,3,4
5,4,1,6
-3,6,-3,-4
-5,-7,6,6
-3,-2,7,8
-2,-5,5,-2
0,-2,-7,-4
-8,2,5,3
0,-6,6,3
0,-1,-7,-2
0,-4,-6,0
2,7,0,5
-3,4,4,2
0,-4,7,-5
-2,-6,5,-4
-7,7,-5,0
-1,-2,0,-7
-1,-1,-4,1
-1,8,0,3
0,7,3,6
-3,-4,0,-3
-2,7,-3,5
2,5,5,1
5,1,-8,8
-6,-5,7,-7
0,0,8,-2
-1,7,-5,-4
-4,4,2,-5
8,8,3,-7
-1,-2,5,0
-5,1,4,5
-5,1,2,-8
0,-1,5,4
-7,2,-8,-1
0,7,-3,5
-2,3,-1,4
0,5,-6,1
-2,-6,5,-1
-7,-3,-3,7
-3,6,-3,6
7,5,1,-5
8,2,8,2
-6,7,0,6
-8,2,3,5
3,3,-3,3
-1,-1,-2,4
2,2,-8,0
-4,-2,0,-6
1,-1,1,4
8,-7,-6,7
1,3,-7,0
7,-7,6,6
-5,-5,6,-2
-2,-4,-3,-6
0,7,-5,5
7,8,5,-1
0,2,8,6
0,-2,-6,3
5,-7,8,-8
8,3,2,3
4,8,-5,4
-8,4,3,-3
7,-2,0,-5
2,-3,0,-2
4,-8,-3,8
-2,7,6,2
8,-6,8,2
2,-6,-7,-4
-3,1,0,-8
2,0,-7,6
8,-2,3,-4
-7,3,4,-4
0,5,4,-8
2,3,6,-1
2,7,2,-5
-7,0,5,-5
3,-7,6,1
4,-8,6,6
-1,0,2,2
-6,6,7,-2
-2,-3,1,-1
-4,1,7,6
-8,-1,8,-1
2,-8,7,5
3,-1,6,4
-1,0,-1,0
-4,-3,4,8
-5,0,-7,4
6,2,0,-7
-2,-1,-1,-7
0,-6,6,-5
5,5,5,4
0,-4,3,6
1,-6,6,8
6,-2,-2,-1
-5,8,-1,8
0,7,0,-2
6,2,4,3
-1,-1,2,7
2,-8,-7,5
-3,-1,-7,1
-2,-1,-8,0
3,-8,-5,2
-1,3,1,4
-8,-4,4,-4
1,8,2,6
-8,-5,-5,-4
-1,-3,-8,-4
-1,7,8,8
-1,4,-6,4
3,5,-3,3
-2,3,0,8
4,7,-3,6
0,-1,-7,-5
-7,-6,-8,-5
0,4,-6,-2
-3,-7,-4,-3
1,2,0,0
-8,-3,8,-2
-5,-8,-4,7
2,4,2,-1
4,7,0,-4
8,4,-3,3
-3,-7,3,7
5,2,-2,-5
-2,-5,4,1
-2,1,-6,6
2,-6,-3,-4
-3,0,-8,-7
-4,-7,8,6
0,4,5,3
-5,1,0,2
-5,-5,7,-1
2,-6,-5,1
4,6,4,2
2,7,-8,-7
4,3,-2,-7
2,8,6,8
4,0,6,-2
-5,-8,-6,-2
-1,2,7,-6
-4,0,7,2
0,5,-7,-7
-7,4,5,-8
-4,0,-2,-1
-2,0,-7,8
8,-3,3,6
-8,-1,-4,-1
8,5,3,1
4,-7,2,0
4,0,1,2
-7,2,-7,5
0,-8,4,-2
-2,0,6,0
-3,-7,0,-1
-1,3,5,-6
3,-1,8,-2
3,4,-7,4
6,7,0,7
-7,2,0,8
2,-7,-1,0
6,8,-1,-1
5,-6,0,2
6,4,-4,-5
-8,3,7,2
0,-3,6,-2
7,-6,-8,-3
-7,-7,0,6
2,5,2,-4
3,8,-2,5
0,1,-6,5
5,-6,-3,-6
1,5,-1,-2
-7,-4,-6,-1
1,6,6,3
6,-1,1,-5
2,-3,-7,-7
-3,0,-3,-5
6,-5,6,-6
7,1,-3,0
-6,-1,8,0
3,-1,5,-4
-2,3,0,-5
8,-3,-4,1
-2,-4,4,-6
4,-5,8,4
7,-8,-1,-3
-4,-6,-2,5
-6,5,1,2
-6,-8,5,2
5,1,-6,-4
7,-3,-8,-1
8,8,-5,4
-5,5,-1,-6
1,-1,0,7
5,1,8,1
0,8,5,7
8,-3,5,6
5,4,-6,-4
6,-1,5,4
6,5,8,0
-5,8,7,2
7,0,-3,2
0,-8,8,-7
-5,3,1,-6
8,-4,-8,3
1,-6,1,-6
8,7,0,-4
-1,-2,-6,-8
0,-7,-3,7
1,8,-2,-8
-4,0,-7,6
5,0,0,5
-6,-4,6,-7
2,-6,8,3
4,5,-7,6
8,-5,-6,-4
8,-1,-3,4
-2,0,6,4
3,0,-4,3
-1,7,-3,6
2,1,6,5
-3,-7,5,0
-5,1,-8,-2
8,-4,4,-8
5,-6,6,0
1,3,1,-1
-2,5,7,7
-1,-2,2,2
-2,5,-7,-7
7,-5,-4,7
-1,-5,-2,4
-6,-3,-7,5
0,2,-5,-7
-4,-4,-7,8
1,6,0,-3
6,-2,-8,-8
-1,-3,2,5
-6,4,0,1
3,-6,1,-8
4,3,-4,6
-8,-1,-7,-1
0,1,3,5
4,6,0,-6
3,6,5,0
7,-4,-3,4
-7,-7,3,1
-8,6,1,-5
7,-3,1,-6
-3,-5,-2,-2
8,0,1,-7
-3,2,2,-5
1,5,-7,7
0,-5,8,-4
-6,-2,-2,-8
-2,-3,-1,1
-7,8,-4,-4
7,-1,6,1
-4,-3,-5,4
0,-3,-2,6
4,-7,7,7
6,0,-8,-3
1,6,-5,4
-1,8,-2,8
3,0,1,-5
0,-4,-4,1
1,-6,6,6
-8,-6,-1,-2
8,-7,-4,-2
-7,8,-6,-6
5,-6,6,7
5,6,3,-5
6,5,3,7
-2,2,1,-4
-1,5,-4,-3
0,-1,8,0
7,4,-7,-6
-5,3,0,-7
6,-1,-7,8
-3,7,-3,-6
-2,-2,6,0
7,-8,7,7
-7,2,-1,-3
-1,-6,-5,0
7,7,-2,-6
0,3,0,3
0,2,-7,-1
7,1,2,-4
0,-1,0,1
4,0,-8,-7
6,-7,0,6
5,6,-4,-7
8,-5,6,4
2,-5,-1,2
6,-1,0,0
-6,3,2,4
0,-2,-1,-5
2,2,0,-4
6,-2,-1,-4
-4,-3,-2,-7
0,0,-5,4
-8,-1,5,4
2,-2,3,-2
0,-3,8,-2
-6,-4,1,5
3,6,2,-4
4,5,-2,0
0,-2,5,5
6,6,0,8
8,7,8,-4
2,0,-4,8
0,-2,-4,-7
-8,-6,6,8
8,8,-4,5
-4,5,-7,0
-5,0,3,-1
-7,0,-4,-1
4,7,-6,7
6,4,-1,4
8,6,-3,-3
-1,4,4,3
8,6,-2,-4
-3,-5,-8,1
3,4,1,7
-4,-8,-7,-7
1,1,-1,-6
-3,-8,-1,8
-4,8,-5,8
0,-6,-7,-6
5,7,-2,0
7,-3,-1,2
-1,-2,-3,-2
2,4,6,3
6,0,-2,7
-8,-8,-8,6
2,0,-6,8
3,0,-8,-6
4,3,1,-5
2,-3,3,-6
-5,-1,1,2
-6,0,6,3
-5,2,5,0
5,-6,8,5
-6,1,3,7
-4,7,-7,-5
-5,6,-4,-6
2,-3,6,-6
2,4,4,-4
8,5,-5,-8
1,0,-5,0
2,3,-1,2
-3,0,6,-5
4,7,0,2
6,0,0,-3
-6,-3,5,6
8,3,8,-4
2,0,8,-3
-8,-2,-2,1
-3,-2,-4,0
-3,5,-2,4
0,0,8,2
5,2,-5,6
-1,8,8,4
4,0,-2,-5
8,0,-3,8
7,8,4,-6
4,-2,-7,6
8,2,3,5
3,7,-4,2
-1,4,6,-5
1,4,-7,-7
-6,-1,4,-3
5,0,-4,7
8,-7,3,1
5,2,6,-3
-2,-7,-8,-7
-4,1,6,5
1,-4,-6,8
3,-3,1,-1
2,-5,3,1
-2,-3,4,0
-8,2,-2,-5
-3,-2,-2,-6
-7,-1,8,-1
0,2,0,8
6,-7,-1,-3
-5,-7,-1,-2
-4,5,-3,1
-4,5,-7,-1
-6,3,-1,-3
6,0,-7,-5
3,-6,-5,2
5,7,8,-2
8,-5,0,5
-7,7,-8,-5
1,-4,6,-4
6,-8,3,0
-5,-4,1,-6
1,-3,0,-7
-4,7,4,-1
4,2,2,-7
0,-3,-6,0
-4,2,0,-3
5,8,5,1
0,0,5,4
4,6,2,8
-8,5,-6,8
1,-6,-2,-1
-6,8,0,-5
-7,0,1,-2
-5,8,6,8
-7,4,0,1
3,1,-1,-3
7,5,3,8
3,-4,7,-1
-7,-3,2,-3
0,8,-7,0
8,6,7,8
-6,1,6,3
1,0,-4,-1
-3,8,1,-6
-2,-2,0,-6
-1,0,2,8
1,-8,3,1
-7,-7,-5,-2
-1,-2,0,-3
-3,6,-5,4
5,3,7,-3
8,3,5,7
7,6,-6,5
-4,-1,-5,-1
-7,-7,-4,-6
-7,8,-7,0
-2,1,-4,6
-1,-4,-6,-1
-4,8,5,4
-1,-2,-1,-3
4,3,-2,-2
2,-4,-2,-4
2,-3,-4,3
-8,-1,-5,6
-7,-6,0,-4
-5,-3,7,8
4,-7,-3,6
-4,0,7,0
-2,0,4,0
7,-3,-3,4
-5,8,1,-7
-4,-2,-5,5
3,6,-6,-6
5,8,-8,-1
-5,5,1,1
-3,8,-6,-6
8,6,6,4
-7,3,1,-7
0,-5,8,0
-5,2,1,-2
-7,-5,0,7
8,-3,0,5
7,0,5,0
-4,-7,-8,-2
6,0,-8,-6
-4,1,1,-5
6,-1,-6,4
-4,4,-3,-3
4,2,-1,5
1,-8,3,2
8,3,0,-7
-8,-8,7,6
-8,5,7,7
0,4,-4,-6
7,2,5,-4
-3,3,5,8
-4,-5,-3,0
-5,-2,-1,-6
-7,-7,-1,8
3,-7,0,6
3,7,-7,3
-8,-2,5,7
-3,-7,2,-3
2,-2,-1,-2
7,-3,7,6
-6,3,-2,2
2,1,-3,-7
-2,-1,-6,1
-8,7,-6,0
0,6,-8,5
-8,-5,6,6
6,2,-7,0
-4,-4,-5,7
5,-5,6,0
-1,5,-8,-7
2,6,0,-7
-8,-4,1,2
-7,-1,-7,0
-5,-3,2,-3
-5,1,-6,-4
3,6,-4,2
-8,7,-7,-2
-6,1,0,-3
6,4,5,7
7,-8,0,-6
1,-6,-2,0
4,4,6,-2
-6,6,-6,-5
8,2,-1,6
2,-2,-5,4
3,7,-4,6
-1,-2,-6,2
-7,8,-4,-5";

        [Fact] public void Solution_1_test_example_1() => Assert.Equal(2, Solve1(testInput));

        [Fact] public void Solution_1_test_example_2() => Assert.Equal(4, Solve1(@"
-1,2,2,0
0,0,2,-2
0,0,0,-2
-1,2,0,0
-2,-2,-2,2
3,0,2,-1
-1,3,2,2
-1,0,-1,0
0,2,1,-2
3,0,0,0"));

        [Fact] public void Solution_1_test_real_input() => Assert.Equal(318, Solve1(puzzleInput));

        public class Group
        {
            public HashSet<Item> Items { get; set; } = new HashSet<Item>();
        }

        public class Item
        {
            public int[] Coords { get; set; }
            public HashSet<Item> Neighbors { get; } = new HashSet<Item>();
        }

        public int Solve1(string input)
        {
            var data = input.SplitByNewline(shouldTrim: true)
                .Select(s => s.Split(",").Select(int.Parse).ToArray())
                .Select(d => new Group { Items = new HashSet<Item> { new Item { Coords = d } } })
                .ToHashSet();

            var neighbors = new Dictionary<Item, HashSet<Item>>();

            foreach (var grp in data)
            {
                var item = grp.Items.Single();
                neighbors[item] = new HashSet<Item>();
                foreach (var other in data)
                {
                    if (grp == other) continue;

                    var dist = GetDist(item, other.Items.Single());
                    if (dist <= 3)
                    {
                        neighbors[item].Add(other.Items.Single());
                    }                    
                }
            }

            var acted = true;
            while (acted)
            {
                acted = false;
                var newGroups = new HashSet<Group>();
                var joined = new HashSet<Group>();

                foreach (var group in data)
                {
                    if (joined.Contains(group)) continue;

                    newGroups.Add(group);

                    foreach (var othergroup in data)
                    {
                        if (othergroup == group) continue;
                        if (joined.Contains(othergroup)) continue;
                        if (newGroups.Contains(othergroup)) continue;

                        if (group.Items.Any(i => neighbors[i].Any(n => othergroup.Items.Contains(n))))
                        {
                            joined.Add(othergroup);
                            foreach (var x in othergroup.Items)
                            {
                                group.Items.Add(x);
                            }
                            acted = true;
                        }
                    }

                }

                data = newGroups;
            }

            return data.Count();
        }

        private static int GetDist(Item one, Item two)
        {
            return Math.Abs(one.Coords[0] - two.Coords[0])
                + Math.Abs(one.Coords[1] - two.Coords[1])
                + Math.Abs(one.Coords[2] - two.Coords[2])
                + Math.Abs(one.Coords[3] - two.Coords[3]);
        }
    }
}
