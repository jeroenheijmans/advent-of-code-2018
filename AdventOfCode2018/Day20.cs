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
    public class Day20
    {
        private readonly ITestOutputHelper output;

        public Day20(ITestOutputHelper output)
        {
            this.output = output;
        }

        public const string puzzleInput = @"^WSWWNNNNENWWSWWNENNENWNWWWNEENNNWWWNEENWNWSWSWWNENENENNWNWWWSESS(EN(E|N)|WWSWWSSESWWNWSSESWWWNNWWSESSSSWSSSSWWSSSWWSSSESEESEENWNWWNEEESESSSSEEENNNWSSWNNNNWNWWW(WWSNEE|)NNESEEEES(W|EESS(WNWSNESE|)SEESEENWNNEEEENNWNWWWWWS(WWNENNNESENESEES(EENWNNEES(W|E(NNN(ESSNNW|)WSWNNWWWWSSS(WWWWSWNNWW(NEENEN(WWWSEWNEEE|)ESSWSEENE(NWNNN(WSNE|)EEN(EEESES(ENESNWSW|)SWNWNWSS(WNWSNESE|)E|NW(S|N(ENEWSW|)W))|S)|SSE(SWSS(WN(NNNN|WW)|ENE(ENWESW|)S)|N))|ESENNN(WS|ES))|SWSEESWW(SSENEEE(NNWSNESS|)SWSEESSSWWWWWNWSWSSEEEN(ESSSWWN(E|WWSWWNNWSSSEESESEESSEEENNNN(NEESEENWNNESEESWSESSSSESENEESENEEESSWW(NEWS|)SWNWW(N|WSWNWNWNNNWWSSSE(NN|SESWSSWNNNWNW(WWWWWWSEESESEESSWSSSSSEENWNEEENNWW(NNE(NNW(NWSWNW|S)|SEEENEEEEN(NESSEESSENESEESSESSSWSWNNENWWWSWSWSESSWNWNWSSESSSSSSWNNWNNE(NWNNNNENNWNENNN(EESWSSSS(ENNE(N(EEESWW|W)|S)|S)|WWWSSE(NESNWS|)SWSSSSE(NN(N|E)|SSSSSWSESSE(SWWNNWWSWNWSWWSWWNNNENNWWWSE(SSWNWNWSSWWSSSSENNEN(W|E(N|EESSESESENN(W|ENEESENE(SSSESENENEEEENNEENNWSWW(NENWNNEN(ENEN(WNEN(W|N)|EEENESENENWNNNEENNNWSSWW(SSSS|WNENNWWNNW(WWS(SENESSEE(WWNNWSNESSEE|)|WW)|NEESSENNESSS(EEEESSEESWWSEESWWW(SSSSW(WSWSESESESWSSESWSWNNNWSSSSSSESWWNNNWWNW(SSSSESE(NNWNEWSESS|)SWSWWWSESSEEENEES(W|EEEEENESENNENWNENEENWNWNWSS(E|SWNWNENWNNNW(NN(W|ESENNWWNNWNN(WSNE|)EENESENNEEENNWWNNW(SSS(WSWNW(NEEWWS|)SS|EE)|NENNNESEENWNENWWNNNNNWWNWNEESEEENNNESESSESEENESSSSENNENWNNENNENENENESSWSESWW(N|WSS(W|SENEN(W|EESSW(SSWWW(NEENSWWS|)WSSSEENN(WSNE|)ESSENNEESESWSSSESSENNNEESESENNESE(NNNWNENNNWSWS(SWNWWSESWS(W(WSNE|)NNNWNNEEES(WW|ENNE(NNWSWNWNEENNWSWNWNWWNW(NWWWNEEENNNESSSSEES(ESEENWNWNWWNNEES(W|ESEES(EE(SSSWW(NENWESWS|)SES(SSE(SWEN|)NNN|W)|NNNNNNNNNNNWNWNWSWWSSWWSSWSESS(WWWNE(E|NNNNE(NNWWWWSSWNNNNEENNWSWNWNENEEES(WW|SESWSES(WWW|ENENEENESS(SWNWSWSSEN(SWNNENSWSSEN|)|EENENE(NWWSW(NNNWNWSWNWNENWNWNWNWWNWNNENWNNNNWWNEENESSEESWSESSW(NWNNSSES|)SEESWW(SEESEENNNNNWS(SSS|WNNENNESSENEEESSSSEE(NWNNE(S|NWNNWNEE(SS|NWWNWSSWWNENNNNNWSWSESWWSWNWWSWS(WNNWWNWWNWSSWWSSSENEN(ENESEESWWS(W(SSWWSWWWNNNE(NWNENWNWNWSWNNWWWWNNNEEENENNESENENWWNNWNWSWWNNE(S|NENWNWSSWSWNNWSWNNEEN(WWWWWSWNWSSEESWWWWWSWWSSEESWSESESWSESEENESENNEESWSSEEN(NEE(NWNW(NENWNEESSSEEEE(NN(ESNW|)WWS(E|WNNNWWNWSWSSWNNN(WSWNWWWS(SENESEESSWWN(WWSESS(ENE(EEENWESWWW|)S|S)|E)|WW)|ENWNENW(ESWSESNWNENW|)))|SWWWSEE(WWNEEEWWWSEE|))|S)|SSSW(NN|WSSSWSWSWSESSSSSWNWNENNWWS(SWWNWSSSSSWNNNNWNWWNWWNEEEEENNNENESSSW(N|SS(EE(N(W|EENWWNNESEENWNWNEESE(ENNW(WNWWS(WNWSWWNENWNWSSWWNENNNNE(SESWENWN|)NWWWNWWWWWWWSWWWWWWSSWWWNWWWWSESWSEENE(NWES|)ESESSESWSEESSWNWWSSSWWSWNWNEENE(S|NNWSWNNEEE(S|NNW(N|SWWN(WSSSWSS(WSWWNENWNNWNWWNWWNEENESES(W|ENNWNWWWNWNWSSWWNENNWWNEENESENEENNENEN(WWWS(E|WWWWSS(WS(E|WNNENWN(WWSESSSWSSWNWSWSESENEEE(NNWSNESS|)SE(NEWS|)SSSWWWSSEEN(EENNESE(NE(N|E)|SWSWWSWSSWSWNNN(ESNW|)WSWNNENWWSWNWWSESWWNNWWWNEENNWSWNWSSSWWWSWSESWWSSWWW(SSESSW(N|SEENEENESESWSSSSWNWSWSWSSW(SEENEESWSWSESWWSW(NNNESNWSSS|)SSSENNEESESESEENENEEEENESSWWWSW(WSWSWWSSWWWSSWSW(NNNE(NENNWSW(NNEEE(NWNSES|)SE(SSWNSENN|)(N|EE)|S)|S)|SESENNENNEEEESENESEEEENESSWSEEENWNEESEESWSW(N|SWNWSWNWWSESEEESEESENNN(WSWENE|)NESSSSSWSESSSWSESSWNWWSWNWSSEESEEEEEEENESSESWWWN(E|WWWSWWN(E|WWSESESWWNWSSWSWWNENENNNN(E(SS|E)|NWWNWSWSWWSSSWSEEESSE(NNNWWNEN(W|ENEESWS(S(ENSW|)S|W))|SESSWNWSWNWWNNWSSSWSSWWNENWWWSESWSSW(NNNNNEEEEENNWNNESE(EEESWSEE(WWNENWESWSEE|)|NNNNNNWNENWWSWWWW(SESEEE(NWWEES|)ESWWSES(ENSW|)WSSSS(WWWNNESENNNNWW(SSENSWNN|)N(EE|N)|E)|NNNNEESWSES(ENENESEEENWNW(S|WWNEEENNNNEESSSESWS(WNNNNSSSSE|)SSES(WS(WW(NEWS|)S(EE|SSS)|E)|ENESEE(SWEN|)NNNWWS(ESNW|)WWNENEENENWWNEEE(NWWWWSW(SESEWNWN|)NNNNE(SSEWNN|)NWN(EESNWW|)WSWWN(E|WNWSSESWWSEES(EENWNEESS(NNWWSEWNEESS|)|SSWSESWWNWNWSSES(SS|WWNW(NENNEEE(ENWWN(NNENNSSWSS|)WSWNW(SSS|N)|S)|S))))|EESSSWSEE(ENWNNSSESW|)SWWW(SEEEWWWN|)NN(NNESNWSS|)W)))|W)))|SSSEEEENWNNEES(ESSENEEESWWSESSWWN(WWSWWNWN(EE(E(NN|EE)|S)|WSW(SESE(N|SWSESSSSENEESWSEEEESEENWNENWNEENWNNESENEESWSWSSSSSW(NNN|SESWSWWWSWSSSSSS(WWWNWNWSWW(SEEEWWWN|)NNNENWNNENWNENEN(WNW(NENWN(NNN|E)|SS)|ESSSSSESSWW(S(WSNE|)EEES(W|SENNNNW(NNNNN(WSSSNNNE|)ESSE(SWSNEN|)N(ESENES|N)|S))|N(N(W|NNN)|E)))|EENWNENENNN(EENESSWWSSSSENNNEESWSESWS(WWWNNSSEEE|)EENESEENEENESS(WW|EEEEENNESENNWNNESESSSS(WW|ENESENENNENESENNNNNESSEESWSESWW(NN|SWWSEES(WWW(W|NN)|EENEN(WWSNEE|)ENEENWWW(S|NNESEENESENN(ESSSEENNN(WSSNNE|)NNE(ESSE(NN|ESEEEESWSESSEENE(NEEENWWWWW(NENWW(W|NEEESES(ENNNNWSS(NNESSSNNNWSS|)|W))|S(E|S))|SSESWWWWN(WWS(WWNENEN(ESNW|)WWWS(WNW(SWNWWWNWSSESENESEES(EENWESWW|)WWWWWNWW(NEWS|)S(W(W|N)|E)|NEEN(EES(ENSW|)W|NWW(SEWN|)NN))|E)|E)|EE)))|N)|NWWWW(S(WNWSWNNWNNNE(NNWNWSS(E|SWWNNWSWSWSEE(N|EESWSE(ENNSSW|)SWSWSEE(N|SWWSWSWS(WNNNE(NE(NWWSW(S|WNNE(NEES(W|ENN(ESSNNW|)WWWNNWWSSWNWSWSWNNNWSSSSEEESWSEE(ENNW(S|N(EENNSSWW|)W)|SSWS(E|WWSW(NWWWWNENEES(ENE(SENEWSWN|)NNWWNNNNWWNWNENESE(SWEN|)EEENWNEENWWWWWS(ESENSWNW|)WNNENWWWWNNWWNENESESSEEEESSEENWNEESENNENESSSW(S(WW|EEESSWNWSW(N|SS(SW(WSNE|)NN|EEN(W|EES(SS|W|ENNWNENEENESSS(WNWSSNNESE|)E(SS|NEENWNNW(NWWNENWWSWWSWSS(WNNNNWNWNEEES(W|EE(SWWSNEEN|)NNNEESENENWNENWWSSWNNNWSSSWWSS(E(S|N)|WNNWWWNNESEEEENNWNEEEEEEEESW(WWW|SSESESWSW(WSEEEESSSEEEENEE(SESEEN(W|ESSEENNW(S|NEE(NW(WWSNEE|)NNE(NWES|)S|SSSESSSWWSS(ENSW|)WNNNEENWWWWWN(WSWS(EESEE(NWES|)SWWSS(WS|ENE)|WWNNE(N(ESNW|)(WWSWNWWSESWSW(SSEE(SWEN|)N(W|EN(W|E(NWES|)S))|NNNNWW(SESSNNWN|)NEENWWW(WWN(E|W(N|S))|SS))|N)|S))|E))))|NNNNNN(E|WWN(WWN(NWSWNW(NWNNNESES(ENE(S(SWW|ENES)|NWNW(WWWSSSSWWNWSWWWWNWWWWNEEN(WWWSWNWSSEESESWS(WSWNNENWWWWWWSWN(NEEEEENWWNEENE(WSWWSEWNEENE|)|WSSWNWSSESSWNWSWNWWSWNW(NEWS|)WSS(SEEN(WNSE|)ESEEN(WNSE|)EEESSS(EENESEENWNWWNEEESEENWNNE(NNWSWNW(NEEEWWWS|)WWSSW(S(SSS|EENE(SEN|NW))|N)|SESSENNN(W|E(N|SSES(EENWESWW|)SW(N|WWWWSESWSWNW(W|SSEEEEENW(W|NEEN(ES(SWSNEN|)E|WW)))))))|SWWSSWN(NNEN(NESSNNWS|)WWWWW|WSSES(WWSSSWNNNWW(SESWSESSE(N|SWWNNWWWNNEE(SWEN|)NWWNEE)|N)|EEEN(ESSSSSSENNNESE(NNWWEESS|)SWSSSESSESE(NNWESS|)SWWWSS(WNNNE(E|NNWNWWSW(SES(W|E(N(E|N)|SWSSE(SSEE(NWES|)S(EE|S|WW)|N)))|NNNE(SEEWWN|)N(N|W)))|ENEEEE)|W(W|NN)))))|WWWWSSSWN(SENNNEWSSSWN|)))|EENNEESWSSE(E|N))|NESSEESENE(NNWNNN(WNN(ESNW|)WSWNNWSSW(SES(W|ENESESSS(WNNWWEESSE|)E)|WWNNWSSWNW(NENNESENNWWNNW(NENNNNNESESSSW(NN|SESS(WNSE|)E(SSSSE(SWEN|)N|NNNNEE(SSWNSENN|)NEENNNNNE(NWNNWWNNNNNWSSSWWSEESWWWNNNENNNWSSWNNNNEEEESW(S(EEENESSSEESWSS(WWNN(ESNW|)NN|EEE(NNNNNESENENNNWWWSWS(EEENWESWWW|)SS(SSSWENNN|)WNNW(SS|NENNNNESS(S|ENNESSEENNW(S|NENNNNWSWNWSWNWSWNNWSWSSE(EEESS(WNWSWNWWSESWSESWS(WWNNWNNEE(S(SS|W)|NNNE(S|NWNNWNNWNWNNW(SWSSWSSEESWSWWWSEESSWWSWSESSENNESENN(WW|ESE(SSW(N|WS(EEENSWWW|)SWN(WSWSWNWWNENWWSWWWSEEESEESWW(SESWSEENEEEEE(SWWWSW(SSEESSSW(SEENEENWWNNNN(WSWENE|)EE(SSW(SEESSSEESS(EES(WSWNSENE|)ENNNNWNN(ESNW|)WWWSSEE(NW|SESW)|WNWWW(NEWS|)WSESSSS(NNNNWNSESSSS|))|N)|N)|WWNW(NNNE(NWWWSSE(SSWWWS(WNNENE(SEWN|)NW(NEWS|)WWS(WSWS(EEN|WN)|E)|E)|N)|SSESEN)|S))|N)|NWNW(NEESNWWS|)WS(E|W(N|W)))|WNWSWNWNNWWWWNENNWW(S(SSSSW(NN|S(SESE(ENWN(NNEESS(WNSE|)E(S(E(E|N)|W)|NN)|W)|S)|WW))|E)|WN(WSWENE|)EEENWNNEENW(NENNNNNNENNN(WSSWSSWWN(ENNNES|WSSS(WW(SEWN|)N(E|W)|EEN(ESS(W|S)|W)))|ESE(SWSEESSSSESEEEENESE(S(E|WWWSSE(S(SWNWSWSWWNWNNNNN(WNENNWSW(NNEEWWSS|)SSSSSSE(NNN|SWSSSENNESESWS(EENEEN(E(SESSE|NW)|WW)|WWWNNNNNW(ESSSSSNNNNNW|)))|ESSSENNESE(SWSW(S|W)|N))|EE)|N))|NNWWNNN(ESSEEWWNNW|)WWSWW(SES(ES(ENN(W|N)|W)|W)|NEN(EENWESWW|)WW))|N))|W)))|N))|NNNWN(WSSEWNNE|)EN(ESSSENNNNWWNN(ESNW|)W(N(E|N)|W)|W)))|NENEEESWWSEEEENN(ESSSE(SWWNWSSW(SEENEEESENEE(SWSEE(S(EESSENENNW(NEENN(EESESESENNWNEENENEEEENESESESS(ENNNNESSSENE(SSWWEENN|)NEN(EEESNWWW|)WW(S|N(E|WWWS(WNNWSWNWWNEEENEENWNEN(WN(WWWWSESSS(ENNE(NWES|)S|WNNWWSESWSWSSS(SWWS(E|W(NNNWNNNESSENE(SSWSEWNENN|)NWNW(NNEES(SE(NNEEWWSS|)S|W)|WW(SS|W))|S))|ENNE(SSEENW|N)))|E)|EESSW(N|S(SSWNWESENN|)E))|S)))|WNWNWSSWSESEN(ESSWSWNWNWSWNNWW(NEEN(W|E(ENWWEESW|)SS)|SSWSWSWWWNENN(W(WS(SSSSSSSEESEENWNWWNNEES(W|EEEENWWWN(EEN(NE(NN|ESWSESSSSWNWSSSENEENEEESSENNEEESWSSSESENNESSENEEEENNEESWSSEESENESSWSWSWNN(E|NWWNWWWSWWSSSWWWSWSSWSEENNESSSEENWNENN(W(S|W)|ENEESSW(SW(N|SESWSWWSSENEEEESSWWSEESESSENESEENNNNNENNNNENNWSWSSWWWSESSWW(SEEE(NNENWESWSS|)SWS(WNW|ES)|N(E|NWW(W|NEENWWNENESEENE(SSWWEENN|)EN(NESENENNW(SWEN|)NNEES(SSENNNNWWWNENEES(EESSSEEESSENNNNWNENNWNNESEESWSESEESEEEESESWSS(WNWSWNNN(ESENSWNW|)WSSSWWSW(SESSWSESSENNNEESEN(ESSSSWWW(SSESSWWSSWSSEEENENN(NEENEES(SW(N|WSEESESWS(WWSSS(WS(WS(E|SWWSS(ENEWSW|)W(NW(S|NWNNESE(NEE(SWEN|)NNWSWNNN(EEEE(N(W|N(ESENSWNW|)NN)|SW(WW|S(E|S)))|WWWWSSWNWNWSSSE(SWSEE(EENWN(WSNE|)EEENWN(WSNE|)E|SSW(SEWN|)NWWNWNNE(S|NNNNWSSSWSWWNWSSSSESWSWNNNNWNWNWWWSSSSSESEESWSEESSWWWN(W(SSEEEEEEENWNEEEE(SSWNWSSWSES(WWWNENW(ESWSEEWWNENW|)|ESSENNE(E(NNW(WWSEEWWNEE|)NNE(NWES|)S|E)|SSS))|NWWNW(NEE(S|NE(EE|NWW(S|NN(ES|WS))))|WS(E|W(SS|NWNNNNWWW(NNESE|SESEN)))))|NN(ESNW|)WWSWNWNNWSWNWNENWW(SSSSSES(W|EEE(EEENSWWW|)NWNW(WNSE|)S)|WNENENWNNNNNW(SSSSSWWNW(NEE(N|S)|SSES(WWSSWWNENW(ESWSEEWWNENW|)|E(NEWS|)S))|NENENWNENNWWNN(ESE(N|ESSSEEE(NEEWWS|)SSEESESWWNWWSESSWNWNNNN(ESNW|)WSSWSSSE(NN|ESSEEEE(SSWSSWNNWN(EE|WSW(NNNWSNESSS|)S(ESSE(NN|EEE(NN|SS(WNWSNESE|)E))|W))|NESEENEENWN(EESEENESE(SWS(WNWSW(N|W(SESNWN|)W)|E|S)|NNWNENWNENWNW(NNNEENNNN(WSSSNNNE|)NESENENWN(EEESWSSWSSWNWSSESSW(WS(WNSE|)SENEENNNESEEE(SSE(NN|SWWWW(NNEESW|WWSWSSSEESS(WNWSNESE|)SENEENESS(WWSNEE|)ENNNNE(S|NWN(EE(SE|NWNE)|WSWWN(WWSWSEE(N|SSENNESEN(SWNWSSNNESEN|))|E)))))|NWNEN(WNENWWSSS(WNSE|)S|E))|N)|NNN)|SWSESSSW(S|NN)))|WSW(WWW(NEEE|SWW)|S)))))|WSSSSW(WNNE(S|NWNWNNWSW(SESESSSW(ENNNWNSESSSW|)|NNENNNNNNWWWW(SSENEESSSSWNN(W|N)|NWNN(EE(NNNWESSS|)S(W|ES(W|EEN(EESWSESWSSSSENENN(WSNE|)NNNEN(ESSE(ESWSS(SWSW(S(E|SSW(WNN(E(N|S)|W)|S))|NNENWN(E|N))|ENESENNW(ESSWNWESENNW|))|NNNN)|WNNWSWW(EENESSNNWSWW|))|W)))|WSWWWNN(ESENSWNW|)NNNNN(WWSSE(N|SWSSE(SWSESSS(W(NN|SW(WWSESW|N))|E)|N))|N)))))|SSE(EN(NNEWSS|)W|SSWNWSS(NNESENSWNWSS|)))))))|EE)))|N))|S))|S))|E)|E)|E))|ENNW(WWWW(SEWN|)N(E|W)|NEN(W|E)))|WWS(SWEN|)E)|NEN(WN|ES))|NWNWW(SEWN|)NEEE(E|S))|NNENWNEN(WN|ESSS))|ENENNNE(NWW(WN(E|NNNNNWNENWW(NN|WWSEESSWSWW(SSENESS(WW|ES(ENNNN(WSSNNE|)N|W))|NEN(WNWSWS(ESWENW|)WWNEN(WNNWWWNEEENWWWNWWNENNE(SES(W|EE(NW|SWW))|NWWSWS(WNW(NNESEWNWSS|)SWSWSSENENEESSSWSWWN(ENENSWSW|)WSSEEESSWWN(WSWWWWWW(SSESWWWN(E|WW(WWWWWWNEEENNNN(WSWSS(ENSW|)WNNWSW(NNEWSS|)SSSENN|E(NWES|)SEE(NWES|)SE(SWS(E|WWNENW)|N))|SSS(WSNE|)EN(N|ESSENESENN(EEENWWNEN(WW|ESEEEN(EEEENWNENE(SSEENN(WSNE|)ESSSWWSW(SEESWW(WWWWWN(ENEES(ENSW|)W|WSSSE(NEWS|)SWW(NNNN|W))|SEESE(SWSEEWWNEN|)N(NNNWESSS|)E)|N)|NW(NNES|WS))|WW))|WW))))|NNESE(N|E))|E)|E))|E)|E))))|S)|ESWSEE(SWS(SEE(NWES|)SW(S(EENEN(ESNW|)W|S)|W)|W(W|N))|N)))|W)|W)|WWW(WWNNWWN(WSNE|)EE(NWES|)ESE(SWEN|)N|S))))))|N))))|W)|WWW))|E)|N(E|N))|ES(E|S)))|N))|N)|S|WW)|WWWWN(WNWSWSWN(SENENEWSWSWN|)|E))|N)|N)|NWN(E|W))|NN)|W(NWWEES|)S))))|EE(SWWSNEEN|)ENWNENW(ESWSESNWNENW|))|EENN(WS|ES))|N))))|SS))|SSS)|WW)|S)))|SSSS)|SS(WWWNSEEE|)EE))|EESWSESSES(E(NNNEEENWNENWN(W(NEWS|)SSSWW(NEWS|)SS|EEEE(SS|NNN(E(S|EEEE(S(ENESNWSW|)W|N(WW|N)))|W)))|S)|W))|S))|S))|W)|SS(W|EESW(W|SESS(W(N|W)|EENWNE(NWES|)EESSSSWN(WWSESWWNN(SSEENWESWWNN|)|NN)))))|EE)|E)))|NN))))|E(EN(NES(EE|S)|W)|S))|SS)))))))|N)|W)|SEENES))))|S))|S)|S)|E))))|SS)|E(EE|S))|NEN(E(SE|NW)|W))))))))|WSSW(S|N)))))|N))|E)|W))))))))|N)|NNNENWNEE(N(ENESSW|WW)|S)))|NENWNNNNENENNESSSE(SWW(WSSE(ENWESW|)SS|N)|EENE(NNWNNEENWNWWS(E|SSSS(E|SWNNNWNNWSWW(NNNENESS(WSNE|)ENNNEEN(ESEESEESESWW(WN(WNWSWW(NEWS|)SSS|E)|SEESSENENESSSSSWNN(W(SSSEEENNE(NWNNNNE(S|NWWNWW(NN(EEEEEESWWS(WNWWEESE|)EES(EN|WSE)|WWW(SEESNWWN|)WW)|SES(WSNE|)E))|EEES(WWWSEE|EN(ES|NW)))|WWNNWSSWNW(ESENNEWSSWNW|))|N))|WWWWS(W(S|N)|E))|SSS(ENNEWSSW|)S)))|S))))|W)|EEEEE))|ENEEESWW(EENWWWEEESWW|)))|EEEEEESSWNWSWNWWSESSWNW(WSESWSSS(WNNNWS(WW|S)|EESENENWWN(WSNE|)NEENNEESWSS(WW|S(SSWSSW(SESW(W|SSE(NENNNN|S))|N)|EEENWWNENNEENENEN(EESEESWWSSENEESEES(SWNWSWWWWWWNEE(EEENSWWW|)NWW(NENE(N|S)|WS(WNSE|)SSEESEN(SWNWWNSEESEN|))|ENESEENEENESS(WW|EENE(SSWENN|)NEENWWNN(ESENESEE(NW|SWW)|WWSS(WNWWS(E|WS(E|W(S|NWNEN(WWWWSESE(N|S(E|WWNWNNWW(EESSESNWNNWW|)))|E(S|EEE)))))|S(ENNSSW|)S))))|WWSW(N|S)))))|N)))|E(N|E))|E))))|E)|S)|S))|S)|W(WW|S)))|E)))|W)|E(SS|EEESEESWSS(ENESESW(SES(EEEEEESESESWWNWWWWW(NEEEEWWWWS|)SSSWWW(NEENSWWS|)WSWWS(WNSE|)EEENEESENESEES(EENESENENEEENEESWSSSS(ENNNE(ENNEEENWWWW(NNESEEEENNNWWWS(SENESNWSWN|)WNWNENEEEES(E(SSSSSSW(SWWS(WNSE|)ESWSEENNE(SSSSW(NWWSE|SE)|N)|N)|NNNWWS(WWNNWWSWSWWSSSWSEENNNE(ENENSWSW|)SSS(SSSWWNN(ESNW|)WSWSW(NNENNNWWNWNEESENE(SS|NWWNEEESENNWN(WWWWWWSESS(WWSWSS(ENE(N|SESESEN(SWNWNWESESEN|))|WNWW(SEWN|)NEENENE(NWN(WWWSWW(SESEEN(ENWESW|)W|N(E|WW))|E)|E))|ENNEEE)|EEEES(WWSNEE|)ENEEESWWS(EEENNSSWWW|)S))|S(EENSWW|)WWN(WWNWWSESW(WNNSSE|)SEE(E(NWES|)EE|S)|E))|E)|E))|WWW)|SS)|S)|WWW(S|WWN(ENENEESWS(E|W)|WW)))|WWWW(SESEENW(ESWWNWESEENW|)|W(N|W)))|W)|W)|W))))|SE(EEN|SW))|N)|EE)|W)|ESSEE(SS|ENN(WW(SEWN|)N|E(NEWS|)SSESENESE(WNWSWNSENESE|))))))|SWWSEESSWNWSWNWSSW(SSENEEESSSS(WNNWNE|ENNNNNWWW(EEESSSNNNWWW|))|WN(W|ENNENWNNNESSE(NNNWESSS|)SS))))|W)|S)|SSS(WNSE|)S))))|S))|SEEENESESE(S(WWNWESEE|)S|NNNW(WWNNN(NNEESWSEE(SSWWNE|N)|WWSSSSENNN(SSSWNNSSENNN|))|S))))|W))|W)|SSSSENE(NWES|)ESSW(SS(W(SSWNSENN|)NN|EE(NWES|)E)|N))|S))|EE(S|E(NWES|)E))|E)|SWSWWWW(N(N|E)|SESSSEENENE(N(WWWS(S|E)|N)|SSWSESWWNWWSESWWNNWWSESWSWSESWSEEESSSSSSWWSESWSWNNNWWNNEES(W|ENNWWWNWNWWSWNWSWSWSWNWSSWNWWSESSSW(SSSSSWS(WN(WSNE|)NENNWS|EENNNEESWSEES(WW|EEEEENNNESSENESENESES(EEENWWNEEENEESSW(WSEEENNNNWWWNENNWW(NNNENEEE(SSWNWSWSEEESSSWNN(SSENNNSSSWNN|)|NWNNWWSW(S(EENSWW|)S|NNWWNNESENESEEE(NWN(WSNE|)E|S(SS|WWW))))|WSE(E|SWWSWWS(E(EEENWESWWW|)S|WNWNNE(S|NWWNWWNWSWWN(NE(EEEN(ESSWENNW|)(WWW|N)|S)|WSW(N|SEESSSSSES(WWNWNENNWSWWN(WSNE|)NE(NWNEWSES|)(EE|S)|ENNWNNNNESENESESWS(WNWS|ESE))))))))|N)|WWWWW)))|NNW(NNNENWNENNWSWSSSWSES(S|WWWNNE(NNE(NWNNWW(SEWN|)N(W|EEENEN(ESESWS(EESESSS(SWNNNSSSEN|)ENENENNNWWNNEES(W|EEESWSSS(EENENENNWNW(SS(SWSNEN|)E|NWWS(E|WNNNNENNEENWWNEENN(WSWNNNWWWWNEENWWWWSWS(EE(SWSSESEESENNE(NNWWWS(SENEWSWN|)W|SSSWS(WSSSE(NN|SWWNWSSW(WNENNENE(S|NN(E|WSWSW(WWNEENN(WWSEWNEE|)E(N|S|EE)|SS)))|SSE(N|SEESWS)))|E))|N)|WNNNE(N(W|E(NN|SE(EEE|N)))|S))|ESENEE(NN|SSSSE(SSESWWWSEE(SSSWW(SESWSWS(W|EE(S(W|SENESE(SSS|N))|N))|NN(ESNW|)W(S|NW(S|NWSWWNENNESENES(S|EN(ESNW|)NWNEN(WWSSNNEE|)N))))|EEENNW(NWES|)S)|N)))))|W(SWSWENEN|)NNN))|W(SWNSEN|)N)|W))|S)|S))|S))))))|N))))))|SWSWS(WNSE|)SSEE(NWNENSWSES|)SWWWSEEESEE(NWNSES|)SWWWWN(WS(SEEEWWWN|)WNNW(NEWS|)SSW(WSWNSENE|)N|E))))|NEEE(SWEN|)NWNENEN(ESNW|)WWSWNNNESE(E|NNWWWSSSS(SSENSWNN|)WNW(NENSWS|)(S|W)))|NNNWNE)|NNNN)|SS))))|W)|SSWNWSWWSWN(SENEENSWWSWN|))|N))))|E)|N)))|S)|WWW(N|W)))|S(WNSE|)E)|NENNN(W(N|SS)|EEE))))|WSSSWNNWWN(N|EE|W)))|WW)|W)))|WWWW)|SE(SSWNSENN|)EEN(ESEWNW|)WW)))$";

        [Fact] public void Solution_1_test_example_1() => Assert.Equal(3, Solve1("^WNE$"));
        [Fact] public void Solution_1_test_example_2() => Assert.Equal(10, Solve1("^ENWWW(NEEE|SSE(EE|N))$"));
        [Fact] public void Solution_1_test_example_3() => Assert.Equal(18, Solve1("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$"));
        [Fact] public void Solution_1_test_example_4() => Assert.Equal(23, Solve1("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$"));
        [Fact] public void Solution_1_test_example_5() => Assert.Equal(31, Solve1("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$"));

        [Fact] public void Solution_1_test_real_input() => Assert.Equal(3983, Solve1(puzzleInput));

        public int Solve1(string input)
        {
            var startingNode = new Node { Point = new Point(0, 0) };
            var nodes = new HashSet<Node> { startingNode };
            var data = input.Substring(1, input.Length - 2);

            TraverseInput(data, startingNode, nodes);

            OutputGrid(nodes);

            var visited = new HashSet<Node>();
            var distances = new Dictionary<Node, int> { { startingNode, 0 } };
            var edges = new HashSet<Node> { startingNode };
            var dist = 0;

            while (edges.Any())
            {
                var newEdges = new HashSet<Node>();
                foreach (var edge in edges)
                {
                    if (visited.Contains(edge)) continue;
                    visited.Add(edge);
                    distances[edge] = dist;
                    if (edge.North != null) newEdges.Add(edge.North);
                    if (edge.East != null) newEdges.Add(edge.East);
                    if (edge.South != null) newEdges.Add(edge.South);
                    if (edge.West != null) newEdges.Add(edge.West);
                }

                dist++;
                edges = newEdges;
            }

            return distances.Values.Max();
        }

        private readonly ISet<char> directions = new HashSet<char> { 'N', 'E', 'S', 'W' };

        private void TraverseInput(string input, Node position, ISet<Node> nodes)
        {
            if (input == "") return;
            if (input[0] == ')') return;

            Node find(Node from, char dir)
            {
                if (dir == 'N') return nodes.SingleOrDefault(n => n.Point == from.Point.Up());
                if (dir == 'E') return nodes.SingleOrDefault(n => n.Point == from.Point.Right());
                if (dir == 'S') return nodes.SingleOrDefault(n => n.Point == from.Point.Down());
                if (dir == 'W') return nodes.SingleOrDefault(n => n.Point == from.Point.Left());
                throw new NotSupportedException();
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (directions.Contains(input[i]))
                {
                    var node = find(position, input[i]) ?? position.CreateNewNode(input[i]);
                    position.Link(node, input[i]);
                    position = node;
                    nodes.Add(position);
                }
                else if (input[i] == '(')
                {
                    var parenStack = 1;
                    var sb = new StringBuilder();

                    while (parenStack > 0)
                    {
                        i++;
                        if (input[i] == '(') parenStack++;
                        else if (input[i] == ')') parenStack--;

                        if (input[i] == '|' && parenStack == 1) sb.Append(Environment.NewLine);
                        else sb.Append(input[i]);
                    }

                    foreach (var slice in sb.ToString().SplitByNewline())
                    {
                        TraverseInput(slice, position, nodes);
                    }
                }
            }
        }
        
        public class Node
        {
            public Point Point { get; set; }
            public Node North { get; set; }
            public Node East { get; set; }
            public Node South { get; set; }
            public Node West { get; set; }

            public Node CreateNewNode(char dir)
            {
                if (dir == 'N') return CreateNewNodeNorth();
                if (dir == 'E') return CreateNewNodeEast();
                if (dir == 'S') return CreateNewNodeSouth();
                if (dir == 'W') return CreateNewNodeWest();
                throw new NotSupportedException();
            }

            public Node Link(Node other, char dir)
            {
                if (dir == 'N') { other.South = this; North = other; }
                if (dir == 'E') { other.West = this; East = other; }
                if (dir == 'S') { other.North = this; South = other; }
                if (dir == 'W') { other.East = this; West = other; }
                return other;
            }

            public Node CreateNewNodeNorth() => North = new Node { Point = Point.Up(), South = this };
            public Node CreateNewNodeEast() => East = new Node { Point = Point.Right(), West = this };
            public Node CreateNewNodeSouth() => South = new Node { Point = Point.Down(), North = this };
            public Node CreateNewNodeWest() => West = new Node { Point = Point.Left(), East = this };

            private string Connections =>
                (North == null ? "" : "N") +
                (East == null ? "" : "E") +
                (South == null ? "" : "S") +
                (West == null ? "" : "W");

            public override string ToString() => $"({Point.X},{Point.Y}) connected to {Connections}";
        }

        private void OutputGrid(HashSet<Node> nodes)
        {
            var gridItems = new Dictionary<Point, char>();

            foreach (var node in nodes)
            {
                var scaled = new Point(node.Point.X * 2, node.Point.Y * 2);
                gridItems[scaled] = ' ';
                if (node.North != null) gridItems[new Point(scaled.X, scaled.Y - 1)] = '-';
                if (node.East != null) gridItems[new Point(scaled.X + 1, scaled.Y)] = '|';
                if (node.South != null) gridItems[new Point(scaled.X, scaled.Y + 1)] = '-';
                if (node.West != null) gridItems[new Point(scaled.X - 1, scaled.Y)] = '|';
            }

            var minx = gridItems.Keys.Min(p => p.X);
            var maxx = gridItems.Keys.Max(p => p.X);
            var miny = gridItems.Keys.Min(p => p.Y);
            var maxy = gridItems.Keys.Max(p => p.Y);

            for (int y = miny - 1; y < maxy + 2; y++)
            {
                var sb = new StringBuilder();
                for (int x = minx - 1; x < maxx + 2; x++)
                {
                    if (x == 0 && y == 0) sb.Append('X');
                    else sb.Append(gridItems.GetOrDefault(new Point(x, y), '█'));
                }
                output.WriteLine(sb.ToString());
            }
        }
    }
}
