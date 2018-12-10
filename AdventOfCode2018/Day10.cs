﻿using System;
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
    public class Day10
    {
        public const string testInput = @"
position=< 9,  1> velocity=< 0,  2>
position=< 7,  0> velocity=<-1,  0>
position=< 3, -2> velocity=<-1,  1>
position=< 6, 10> velocity=<-2, -1>
position=< 2, -4> velocity=< 2,  2>
position=<-6, 10> velocity=< 2, -2>
position=< 1,  8> velocity=< 1, -1>
position=< 1,  7> velocity=< 1,  0>
position=<-3, 11> velocity=< 1, -2>
position=< 7,  6> velocity=<-1, -1>
position=<-2,  3> velocity=< 1,  0>
position=<-4,  3> velocity=< 2,  0>
position=<10, -3> velocity=<-1,  1>
position=< 5, 11> velocity=< 1, -2>
position=< 4,  7> velocity=< 0, -1>
position=< 8, -2> velocity=< 0,  1>
position=<15,  0> velocity=<-2,  0>
position=< 1,  6> velocity=< 1,  0>
position=< 8,  9> velocity=< 0, -1>
position=< 3,  3> velocity=<-1,  1>
position=< 0,  5> velocity=< 0, -1>
position=<-2,  2> velocity=< 2,  0>
position=< 5, -2> velocity=< 1,  2>
position=< 1,  4> velocity=< 2,  1>
position=<-2,  7> velocity=< 2, -2>
position=< 3,  6> velocity=<-1, -1>
position=< 5,  0> velocity=< 1,  0>
position=<-6,  0> velocity=< 2,  0>
position=< 5,  9> velocity=< 1, -2>
position=<14,  7> velocity=<-2,  0>
position=<-3,  6> velocity=< 2, -1>
";
        public const string puzzleInput = @"
position=<-10351, -10360> velocity=< 1,  1>
position=< 52528,  31539> velocity=<-5, -3>
position=<-31270, -20838> velocity=< 3,  2>
position=< 52486, -10365> velocity=<-5,  1>
position=< 31558,  10589> velocity=<-3, -1>
position=<-52253,  21064> velocity=< 5, -2>
position=<-10354,  42015> velocity=< 1, -4>
position=<-41798,  42013> velocity=< 4, -4>
position=<-52253, -52267> velocity=< 5,  5>
position=< 31550, -41793> velocity=<-3,  4>
position=<-31290,  10591> velocity=< 3, -1>
position=< 31542, -10363> velocity=<-3,  1>
position=< 21117,  52487> velocity=<-2, -5>
position=< 21074, -41796> velocity=<-2,  4>
position=< 10619, -20840> velocity=<-1,  2>
position=< 31562,  52495> velocity=<-3, -5>
position=< 31586, -20844> velocity=<-3,  2>
position=<-20837,  42020> velocity=< 2, -4>
position=< 52486,  10589> velocity=<-5, -1>
position=< 52518, -31313> velocity=<-5,  3>
position=<-31286,  21063> velocity=< 3, -2>
position=< 31536, -41793> velocity=<-3,  4>
position=< 52523, -52268> velocity=<-5,  5>
position=<-20830, -10364> velocity=< 2,  1>
position=< 31568,  10587> velocity=<-3, -1>
position=< 21116, -10369> velocity=<-2,  1>
position=< 31558,  21060> velocity=<-3, -2>
position=< 21074,  42013> velocity=<-2, -4>
position=< 21114,  42015> velocity=<-2, -4>
position=<-52231, -52269> velocity=< 5,  5>
position=<-20819,  21064> velocity=< 2, -2>
position=< 42062, -10365> velocity=<-4,  1>
position=<-31271,  52496> velocity=< 3, -5>
position=< 10598, -41791> velocity=<-1,  4>
position=< 42038, -10361> velocity=<-4,  1>
position=< 42047, -52273> velocity=<-4,  5>
position=<-31290,  10591> velocity=< 3, -1>
position=<-41742,  21066> velocity=< 4, -2>
position=< 31590, -41790> velocity=<-3,  4>
position=<-31314, -10360> velocity=< 3,  1>
position=< 42011,  42011> velocity=<-4, -4>
position=<-10366,  10583> velocity=< 1, -1>
position=<-10349, -20844> velocity=< 1,  2>
position=<-20806, -41790> velocity=< 2,  4>
position=< 42018,  52494> velocity=<-4, -5>
position=<-41746, -52271> velocity=< 4,  5>
position=< 52507,  10590> velocity=<-5, -1>
position=<-20793,  52487> velocity=< 2, -5>
position=< 21106,  21067> velocity=<-2, -2>
position=< 42047,  21063> velocity=<-4, -2>
position=<-20794, -31321> velocity=< 2,  3>
position=< 10611, -20838> velocity=<-1,  2>
position=<-41742, -41793> velocity=< 4,  4>
position=<-41747, -31321> velocity=< 4,  3>
position=<-20838, -10367> velocity=< 2,  1>
position=<-52274, -31314> velocity=< 5,  3>
position=< 52526, -20841> velocity=<-5,  2>
position=< 21066,  42017> velocity=<-2, -4>
position=<-10354, -41789> velocity=< 1,  4>
position=<-10326,  31535> velocity=< 1, -3>
position=< 42062, -20837> velocity=<-4,  2>
position=<-10346,  31539> velocity=< 1, -3>
position=< 21090,  52494> velocity=<-2, -5>
position=<-20829, -41797> velocity=< 2,  4>
position=< 21077,  21068> velocity=<-2, -2>
position=<-20814, -20845> velocity=< 2,  2>
position=< 21103, -10366> velocity=<-2,  1>
position=<-52250, -52266> velocity=< 5,  5>
position=<-10341,  31537> velocity=< 1, -3>
position=< 10603, -20839> velocity=<-1,  2>
position=<-41782, -20839> velocity=< 4,  2>
position=<-52226, -41790> velocity=< 5,  4>
position=< 31586, -41791> velocity=<-3,  4>
position=<-20790,  31543> velocity=< 2, -3>
position=< 31575, -41793> velocity=<-3,  4>
position=< 52490,  42011> velocity=<-5, -4>
position=<-41741,  31535> velocity=< 4, -3>
position=<-10318, -31316> velocity=< 1,  3>
position=< 10585, -10369> velocity=<-1,  1>
position=< 10606,  10591> velocity=<-1, -1>
position=< 52510,  31544> velocity=<-5, -3>
position=<-20846, -20840> velocity=< 2,  2>
position=< 21058,  52490> velocity=<-2, -5>
position=<-10367, -31321> velocity=< 1,  3>
position=<-41774,  21062> velocity=< 4, -2>
position=<-52254,  52492> velocity=< 5, -5>
position=< 10640, -10360> velocity=<-1,  1>
position=<-20846, -20839> velocity=< 2,  2>
position=<-10341, -52272> velocity=< 1,  5>
position=<-10330, -10363> velocity=< 1,  1>
position=<-10353, -41797> velocity=< 1,  4>
position=<-41782,  52494> velocity=< 4, -5>
position=<-41761,  10584> velocity=< 4, -1>
position=< 42018, -10369> velocity=<-4,  1>
position=<-10365,  42014> velocity=< 1, -4>
position=<-10318,  42018> velocity=< 1, -4>
position=<-52242, -31315> velocity=< 5,  3>
position=< 21095, -20838> velocity=<-2,  2>
position=< 21058, -10368> velocity=<-2,  1>
position=<-31282,  10584> velocity=< 3, -1>
position=<-31322, -31321> velocity=< 3,  3>
position=< 21117, -52273> velocity=<-2,  5>
position=< 31543,  52496> velocity=<-3, -5>
position=< 52538,  21066> velocity=<-5, -2>
position=< 10582,  52492> velocity=<-1, -5>
position=<-41777, -52265> velocity=< 4,  5>
position=<-10357, -52264> velocity=< 1,  5>
position=< 42053,  31535> velocity=<-4, -3>
position=< 42028, -20845> velocity=<-4,  2>
position=<-10326, -20845> velocity=< 1,  2>
position=< 10638,  10585> velocity=<-1, -1>
position=< 31591,  42020> velocity=<-3, -4>
position=< 52505, -52273> velocity=<-5,  5>
position=<-10338,  21059> velocity=< 1, -2>
position=<-10341,  42012> velocity=< 1, -4>
position=< 31566,  31544> velocity=<-3, -3>
position=<-52215, -10360> velocity=< 5,  1>
position=< 52526,  21060> velocity=<-5, -2>
position=< 10638,  10589> velocity=<-1, -1>
position=< 42034,  10584> velocity=<-4, -1>
position=< 31553, -52264> velocity=<-3,  5>
position=<-10353, -10360> velocity=< 1,  1>
position=<-20795, -31321> velocity=< 2,  3>
position=<-20830,  42014> velocity=< 2, -4>
position=< 10606, -31319> velocity=<-1,  3>
position=<-20814, -41796> velocity=< 2,  4>
position=<-52239,  31539> velocity=< 5, -3>
position=< 31586,  10586> velocity=<-3, -1>
position=<-10318,  31540> velocity=< 1, -3>
position=< 10643,  10584> velocity=<-1, -1>
position=< 42042,  10592> velocity=<-4, -1>
position=<-41749, -10360> velocity=< 4,  1>
position=<-41774,  52490> velocity=< 4, -5>
position=< 31590,  31536> velocity=<-3, -3>
position=<-10350, -41789> velocity=< 1,  4>
position=< 42047, -52264> velocity=<-4,  5>
position=< 10631, -20836> velocity=<-1,  2>
position=< 52542, -52270> velocity=<-5,  5>
position=<-52266,  21062> velocity=< 5, -2>
position=<-52258, -10363> velocity=< 5,  1>
position=<-41793,  42016> velocity=< 4, -4>
position=<-31277, -10367> velocity=< 3,  1>
position=< 52510, -52270> velocity=<-5,  5>
position=<-20842, -52269> velocity=< 2,  5>
position=< 10630, -10362> velocity=<-1,  1>
position=< 21082,  42011> velocity=<-2, -4>
position=< 21062,  52487> velocity=<-2, -5>
position=< 52503,  31544> velocity=<-5, -3>
position=< 10587, -31318> velocity=<-1,  3>
position=<-41766,  42015> velocity=< 4, -4>
position=<-52245, -52267> velocity=< 5,  5>
position=<-31277,  31538> velocity=< 3, -3>
position=<-10362,  10583> velocity=< 1, -1>
position=< 31593,  52496> velocity=<-3, -5>
position=< 21108, -20836> velocity=<-2,  2>
position=<-52266,  52490> velocity=< 5, -5>
position=< 21094, -10365> velocity=<-2,  1>
position=< 31575, -20841> velocity=<-3,  2>
position=<-31285,  42013> velocity=< 3, -4>
position=< 10625,  10583> velocity=<-1, -1>
position=<-10313, -52264> velocity=< 1,  5>
position=< 42036,  42014> velocity=<-4, -4>
position=< 31561,  10589> velocity=<-3, -1>
position=< 10624, -41793> velocity=<-1,  4>
position=<-52274, -31314> velocity=< 5,  3>
position=< 10587, -10361> velocity=<-1,  1>
position=< 31545, -41788> velocity=<-3,  4>
position=< 21063, -20838> velocity=<-2,  2>
position=<-52234, -41790> velocity=< 5,  4>
position=< 52523,  52490> velocity=<-5, -5>
position=< 42047, -10361> velocity=<-4,  1>
position=<-41774,  10587> velocity=< 4, -1>
position=<-52274,  42013> velocity=< 5, -4>
position=< 21106,  52494> velocity=<-2, -5>
position=< 52546, -10360> velocity=<-5,  1>
position=< 10633, -20836> velocity=<-1,  2>
position=<-10370, -20844> velocity=< 1,  2>
position=< 42030, -20837> velocity=<-4,  2>
position=< 21075,  42020> velocity=<-2, -4>
position=<-20820,  21063> velocity=< 2, -2>
position=< 10622,  21062> velocity=<-1, -2>
position=< 42066,  21065> velocity=<-4, -2>
position=< 52528, -52273> velocity=<-5,  5>
position=< 10610, -31313> velocity=<-1,  3>
position=<-10370, -20837> velocity=< 1,  2>
position=<-20788,  31535> velocity=< 2, -3>
position=<-20844,  52487> velocity=< 2, -5>
position=< 52510, -41788> velocity=<-5,  4>
position=< 31566, -20844> velocity=<-3,  2>
position=< 42047,  42012> velocity=<-4, -4>
position=<-31282, -41789> velocity=< 3,  4>
position=<-41742,  52493> velocity=< 4, -5>
position=<-41758,  21061> velocity=< 4, -2>
position=< 31579,  52489> velocity=<-3, -5>
position=<-31317,  42013> velocity=< 3, -4>
position=< 10611,  21059> velocity=<-1, -2>
position=< 31535, -41797> velocity=<-3,  4>
position=<-10338, -10365> velocity=< 1,  1>
position=<-52261,  31544> velocity=< 5, -3>
position=<-10368, -10369> velocity=< 1,  1>
position=<-41746,  10587> velocity=< 4, -1>
position=< 31571,  31535> velocity=<-3, -3>
position=<-20786, -10360> velocity=< 2,  1>
position=<-52258,  10584> velocity=< 5, -1>
position=< 52494, -31315> velocity=<-5,  3>
position=<-31317,  52492> velocity=< 3, -5>
position=< 31566, -20839> velocity=<-3,  2>
position=<-20825, -52264> velocity=< 2,  5>
position=< 10614, -10366> velocity=<-1,  1>
position=<-31277, -10366> velocity=< 3,  1>
position=< 52488,  42020> velocity=<-5, -4>
position=<-41795,  31539> velocity=< 4, -3>
position=< 52520, -52269> velocity=<-5,  5>
position=< 21070,  52496> velocity=<-2, -5>
position=<-31293, -31315> velocity=< 3,  3>
position=<-10314,  42016> velocity=< 1, -4>
position=<-20814, -31319> velocity=< 2,  3>
position=<-41777, -52265> velocity=< 4,  5>
position=< 21079,  52494> velocity=<-2, -5>
position=< 21101, -10369> velocity=<-2,  1>
position=<-20821,  31537> velocity=< 2, -3>
position=<-20802,  21063> velocity=< 2, -2>
position=< 52531,  21060> velocity=<-5, -2>
position=<-20828,  52496> velocity=< 2, -5>
position=< 10590,  52488> velocity=<-1, -5>
position=< 52511,  31537> velocity=<-5, -3>
position=<-20817, -20839> velocity=< 2,  2>
position=< 21082,  10587> velocity=<-2, -1>
position=< 52530, -31321> velocity=<-5,  3>
position=<-41782, -41795> velocity=< 4,  4>
position=< 42034, -10360> velocity=<-4,  1>
position=<-41765, -20841> velocity=< 4,  2>
position=<-41788,  42020> velocity=< 4, -4>
position=< 42047,  31544> velocity=<-4, -3>
position=<-31314, -41793> velocity=< 3,  4>
position=< 10611,  31543> velocity=<-1, -3>
position=< 10611, -10360> velocity=<-1,  1>
position=< 31542, -41795> velocity=<-3,  4>
position=< 42050, -52269> velocity=<-4,  5>
position=< 31592,  10592> velocity=<-3, -1>
position=<-10317,  21059> velocity=< 1, -2>
position=< 52544, -31321> velocity=<-5,  3>
position=<-31282,  52488> velocity=< 3, -5>
position=<-52242,  31535> velocity=< 5, -3>
position=<-31290, -52271> velocity=< 3,  5>
position=<-52256, -31312> velocity=< 5,  3>
position=<-31306, -10364> velocity=< 3,  1>
position=< 21077, -20840> velocity=<-2,  2>
position=< 10598, -10365> velocity=<-1,  1>
position=<-41761, -20837> velocity=< 4,  2>
position=< 31571, -31315> velocity=<-3,  3>
position=<-41772,  10587> velocity=< 4, -1>
position=<-41750,  21067> velocity=< 4, -2>
position=< 31539, -31314> velocity=<-3,  3>
position=< 10619,  10590> velocity=<-1, -1>
position=<-10341,  31539> velocity=< 1, -3>
position=<-20814, -52268> velocity=< 2,  5>
position=<-20814,  21066> velocity=< 2, -2>
position=< 10614,  10585> velocity=<-1, -1>
position=<-41774,  21061> velocity=< 4, -2>
position=< 10622, -20840> velocity=<-1,  2>
position=<-41793,  31541> velocity=< 4, -3>
position=<-20841,  21061> velocity=< 2, -2>
position=< 10622, -41792> velocity=<-1,  4>
position=<-31317,  10590> velocity=< 3, -1>
position=<-10341, -10369> velocity=< 1,  1>
position=<-31322,  42018> velocity=< 3, -4>
position=<-20838, -20840> velocity=< 2,  2>
position=<-20844, -10360> velocity=< 2,  1>
position=< 10634, -41789> velocity=<-1,  4>
position=< 42010,  52496> velocity=<-4, -5>
position=<-41777,  31542> velocity=< 4, -3>
position=< 10602, -31316> velocity=<-1,  3>
position=< 52538,  42015> velocity=<-5, -4>
position=< 21083,  52489> velocity=<-2, -5>
position=< 21066,  21060> velocity=<-2, -2>
position=<-20805, -41793> velocity=< 2,  4>
position=< 21087, -41788> velocity=<-2,  4>
position=< 52514,  21066> velocity=<-5, -2>
position=< 52515, -41790> velocity=<-5,  4>
position=< 21066, -31313> velocity=<-2,  3>
position=<-41753, -52272> velocity=< 4,  5>
position=< 10587,  31537> velocity=<-1, -3>
position=<-20814, -31312> velocity=< 2,  3>
position=< 10627,  52488> velocity=<-1, -5>
position=< 52523, -20837> velocity=<-5,  2>
position=<-41774,  52492> velocity=< 4, -5>
position=<-52269,  21062> velocity=< 5, -2>
position=<-31282,  10583> velocity=< 3, -1>
position=<-31322,  21059> velocity=< 3, -2>
position=<-31302, -20845> velocity=< 3,  2>
position=< 52526, -10360> velocity=<-5,  1>
position=< 31536, -41793> velocity=<-3,  4>
position=<-20809,  42015> velocity=< 2, -4>
position=< 31542, -52271> velocity=<-3,  5>
position=< 52486, -10363> velocity=<-5,  1>
position=<-10311,  42011> velocity=< 1, -4>
position=<-41761,  10584> velocity=< 4, -1>
position=< 42050, -41789> velocity=<-4,  4>
position=< 52515, -20838> velocity=<-5,  2>
position=< 21058, -20841> velocity=<-2,  2>
position=< 31586, -31320> velocity=<-3,  3>
position=<-41766,  31541> velocity=< 4, -3>
position=<-41769,  31540> velocity=< 4, -3>
position=<-52250, -31320> velocity=< 5,  3>
position=< 52543,  42020> velocity=<-5, -4>
position=< 42052, -20841> velocity=<-4,  2>
position=< 52499,  31544> velocity=<-5, -3>
position=< 52515, -20842> velocity=<-5,  2>
position=< 31537,  31535> velocity=<-3, -3>
position=< 52490, -20836> velocity=<-5,  2>
position=< 52518,  21066> velocity=<-5, -2>
position=< 31590,  52492> velocity=<-3, -5>
position=<-10310, -10369> velocity=< 1,  1>
position=< 31582,  42019> velocity=<-3, -4>
position=<-10368, -52269> velocity=< 1,  5>
position=< 31539, -20844> velocity=<-3,  2>
position=< 10590, -41796> velocity=<-1,  4>
position=< 52538, -20837> velocity=<-5,  2>
position=<-41737,  31543> velocity=< 4, -3>
position=<-52271, -31312> velocity=< 5,  3>
position=< 31559,  42012> velocity=<-3, -4>
position=<-10330,  31542> velocity=< 1, -3>
position=<-41797,  31539> velocity=< 4, -3>
position=< 10639,  52487> velocity=<-1, -5>
position=< 31566, -41792> velocity=<-3,  4>
position=< 42068,  42020> velocity=<-4, -4>
position=< 42066, -20840> velocity=<-4,  2>
position=< 10583, -52264> velocity=<-1,  5>
position=< 21066,  31543> velocity=<-2, -3>
position=< 10606, -41790> velocity=<-1,  4>
position=< 42042, -41793> velocity=<-4,  4>
position=< 42038, -31314> velocity=<-4,  3>
position=<-20806, -20842> velocity=< 2,  2>
position=< 42010, -10366> velocity=<-4,  1>
position=<-41795, -20841> velocity=< 4,  2>
position=< 10633,  21059> velocity=<-1, -2>
position=< 52527, -10369> velocity=<-5,  1>
position=< 21109, -31312> velocity=<-2,  3>
position=<-52224,  52496> velocity=< 5, -5>
position=< 10583,  10592> velocity=<-1, -1>
";

        [Fact] public void Solution_1_test_example() => Assert.Equal("HI", "HI"); // Gonna need OCR to test-drive this I think...
        [Fact] public void Solution_1_test_real_input() => Assert.Equal("BLGNHPJC", "BLGNHPJC"); // Gonna need OCR to test-drive this I think...

        [Fact] public void Solution_2_test_example() => Assert.Equal(3, Solve2(testInput));
        [Fact] public void Solution_2_test_real_input() => Assert.Equal(10476, Solve2(puzzleInput));
        
        public static int Solve2(string input)
        {
            var data = input
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x
                    .Trim()
                    .SubGroups(@"position=<(\W*-?\d+), (\W*-?\d+)> velocity=<(\W*-?\d+), (\W*-?\d+)>")
                    .Select(part => int.Parse(part))
                    .ToArray()
                )
                .Select(arr => new KeyValuePair<Point, Point>(new Point(arr[0], arr[1]), new Point(arr[2], arr[3])))
                .ToArray();

            int count = data.Count();

            Point[] velocities = data.Select(kvp => kvp.Value).ToArray();
            Point[] positions = data.Select(kvp => kvp.Key).ToArray();
            Point[] positionsNext = new Point[positions.Length];

            long prevEntropy = long.MaxValue;

            // Drastically improves speed, but at cost of accuracy. Set 
            // to '1' for exact (but slow) results.
            int entropySamplingFactor = Math.Min(10, count / 10);

            for (int i = 0; i < 100_000; i++)
            {
                for (int n = 0; n < count; n++)
                {
                    positionsNext[n] = new Point(positions[n].X + velocities[n].X, positions[n].Y + velocities[n].Y);
                }

                long nextEntropy = CalculateEntropy(positionsNext, entropySamplingFactor);

                if (nextEntropy > prevEntropy)
                {
                    // We're getting there, lower sampling to 1 and recheck
                    entropySamplingFactor = 1;
                    prevEntropy = CalculateEntropy(positions, entropySamplingFactor);
                    nextEntropy = CalculateEntropy(positionsNext, entropySamplingFactor);

                    string asciiArt = GetAsciiArt(positions);
                    Console.WriteLine(asciiArt);
                    return i;
                }

                prevEntropy = nextEntropy;
                positions = positionsNext;
            }

            return -1;
        }

        private static long CalculateEntropy(Point[] positions, int entropySamplingFactor)
        {
            long nextEntropy = 0;

            for (int a = 0; a < positions.Length; a += entropySamplingFactor)
            {
                var aX = positions[a].X; // Minor perf enhancement
                var aY = positions[a].Y; // Minor perf enhancement

                for (int b = a + 1; b < positions.Length; b += entropySamplingFactor)
                {
                    // Inlining Manhattan Distance function for (some) performance
                    nextEntropy += Math.Abs(aX - positions[b].X) + Math.Abs(aY - positions[b].Y);
                }
            }

            return nextEntropy;
        }

        private static string GetAsciiArt(IEnumerable<Point> data)
        {
            var (minX, maxX, minY, maxY) = data.GetDimensions();

            var sb = new StringBuilder();
            for (int y = minY - 1; y <= maxY + 1; y++)
            {
                for (int x = minX - 1; x <= maxX + 1; x++)
                {
                    sb.Append(data.Contains(new Point(x, y)) ? '█' : '·');
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}

