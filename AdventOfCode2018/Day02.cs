using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using static AdventOfCode2018.Util;

namespace AdventOfCode2018
{
    public class Day02
    {
        public const string puzzleInput = "naosmkcwtdbfivxuphzweraljq,nvssmicltdbfiyxuphzgeraljq,nvosmkcwwdbfiyxuphzeeraljx,nvosmkcqtdbfiyxupkzgeraljw,qvosmkcwtdbhiyxuphzgeraljh,nvocqkcktdbfiyxuphzgeraljq,nvosmhcwtdbfiyxmphzgekaljq,nvosmkcwtdbfuyxwpszgeraljq,nvosmocwtcbfiyxupfzgeraljq,nvosmkcwtdbfiyxubczgeraljv,nvosmkswtdbfiyxuphzgeruejq,nlosmkcwtqbfiyxuphzgyraljq,nvosmkcwtdbficxuphzgwraljk,nvosmkkwtdbfiyxxphzgeralcq,vvosmkcetdbfiyxumhzgeraljq,evosmkcdtdbfiyxuphkgeraljq,nvosmkvwtdbkiyxuphzgeraejq,nvoszkcwtdbfimxuphzgeraljb,nvozmkcwtdbfiyxuphzgrrcljq,nvosvacwtdbfiyxuphzgeralzq,nvosmkcwgdofiyxuthzgeraljq,nvosmkcwasbfiyxuphzgeradjq,nvosmkcatobfiyxtphzgeraljq,nvosmkewtdsfiyxuphzgekaljq,tvormkcwtdbfiyxuphzieraljq,nvosgkcwtdbfiyxuuhzgeraqjq,nvosmkcwtdbqiwxuphzgeralvq,nvosmkcwtybfiydcphzgeraljq,nvosnkcwtdbfiyxuphzulraljq,nvosmkcwtdbbiyuupnzgeraljq,nvosmwcwtdbfiyxuphzneraojq,nvohlkcwtdbftyxuphzgeraljq,nvasmkcwbdbfiyiuphzgeraljq,nvosmkujtdbfiyxuphzgeraljz,nvosmgcstdbfiyxuphzgeraljd,nvoswkcwtsbziyxuphzgeraljq,nvosmmcwtdbfiyxupbzzeraljq,nvosmkcwtdbfifxulhzgeralji,nvolmkcwtdbmiyxuphzgeraljv,lvnsmkcwtdbfiyxuphzzeraljq,nvqsmkcwtdbfiyxuphageralfq,nvosmkcwtdmfiyluphzgeralzq,nvommvcwtdbfiyxupjzgeraljq,naosmkcwtdbfsyxuphzgsraljq,avosmkcwtdbfiyxuphzgebafjq,ndozmkcwtdbfiyxuhhzgeraljq,nvosmkcwtubfiyxuphooeraljq,nvosmkcwtdbliyxuphzgmraljx,nvosmkcuddbfimxuphzgeraljq,wvosmkzwrdbfiyxuphzgeraljq,nvosmkcqtdbfiyxupjzgeraijq,nvosbkcwtdbfiyduphzgeruljq,yzosmkcntdbfiyxuphzgeraljq,nvolmkcwtdbfiyxuphugeralfq,nvrsmkcwtdbjiyxuphzgejaljq,nvgsmkcwtdbfiyxuphoglraljq,nvosmkcwtdbfioxuphzgezalhq,nvosjkcwtdbfipxuphzgekaljq,nvosmkcwtabfiyxlpazgeraljq,nvosmkfwtpnfiyxuphzgeraljq,nvokmbcwtdbeiyxuphzgeraljq,nvosmkcwtdbfiyxupmzgmlaljq,nvosmkcwtdhfiykurhzgeraljq,nvosmkcwwdbfiyxumhzgiraljq,cvosmscwtdbfikxuphzgeraljq,nvosmkcwtdnzirxuphzgeraljq,nvosmscwtdbfiyxuuhbgeraljq,nvosmkcwtdbfidxpphzgeraajq,nvosmkcwtdbfiyxuqhzgurcljq,nvosmkcwtekfiyxrphzgeraljq,ntosmkcwtpqfiyxuphzgeraljq,nvosmkcdtdbfhyxsphzgrraljq,nvolmkkwtdbfiyxuphzgeralgq,nvosmrcwtdbfiyxuphzgefkljq,nvoxmkcwtdbfiysuphzeeraljq,nvjsmkswtdbfiyxuphzqeraljq,nvosmkcetdbfiyfuphdgeraljq,nvosmkkwtpbfsyxuphzgeraljq,nvosdgcwtdbfiyxupyzgeraljq,nvosmkcwudbfiyzvphzgeraljq,nvosmkcwtlbfiyxupkzgerzljq,nvosmkcwtdbfiywuphyzeraljq,nvocmkcwtdufiyxukhzgeraljq,nvosmkcwtdqfiyxuphzgevaxjq,nvosvkcwtdbgiyxuphzgeralzq,nqosmkcwtdbfiyxuphzeeraljr,nvosmkcetdbfiyxuphzgeroljo,nvosmkcwtdvfiyxuphzceraliq,nvosmkcwtnxfiyxuphzgyraljq,nvosmkfwtdefiyxupxzgeraljq,nvosmacwtdbfiyxuphzseragjq,nvpsmkcwtdbfzyxuvhzgeraljq,nvormkcwtdbfiyxuphzairaljq,rvysmkcwtdbfmyxuphzgeraljq,nvosmscwzdbfiyxuphzgerbljq,nvosmkcwtdufmyxuphzqeraljq,nvosmkcwtxbfiyxxphzgeralxq,nvosmkcwtdbsiyxupsfgeraljq,nvosmccwtdbfiqxuthzgeraljq,nvosmtcwtqbuiyxuphzgeraljq,nvosmkcwtdbfiysurbzgeraljq,nvowmkcwtdbfiyxuywzgeraljq,xvosmkcktdbfiyxuhhzgeraljq,nvosmkgwsdbfiyxmphzgeraljq,jvofmkcwtdbfiyxupyzgeraljq,nvozakcwtdbfiexuphzgeraljq,nvosmkcptdbfiyxuphzgepaljn,nvosmkcwtdbpiyxuphzgeraxjw,nvoszkcwtdbfiyjuphzeeraljq,nvosmkcwtdbfiyxuppzoeraejq,nvosmkiytdbfiyhuphzgeraljq,nvosmkcwtdvfiywupyzgeraljq,nvosmecwtdofiyxuphzgeralja,nvosmkqwtdbfixxuphzgeraojq,nvosmkwwtdbfiyxfpdzgeraljq,nvosmkgwtdbfiyzupwzgeraljq,nmosmucwtdvfiyxuphzgeraljq,nvosmdcwtdbmiyxuphzveraljq,wvosmkcwtpbfiyxuphzgetaljq,nvosmmcwtdlfbyxuphzgeraljq,nvosmkcwtabmiexuphzgeraljq,nvosqpcwtdbfiyxuphzgqraljq,nvosmecwjdbfiyxuphzgeraljk,nyohmkcwtdbfiyxuphzgzraljq,nlosmkcwtkbfiyxuphzgeraejq,nvosmkcwrdbliyxuphzgerpljq,nvusmkzwtdbfxyxuphzgeraljq,nvosmkcwtdbfiyxuhizgerazjq,nvosmkhptdbfbyxuphzgeraljq,nvosmfcwtdbgiyxupdzgeraljq,nvosmkmwtdbfiyxuphzgevalpq,nvosmkcwtdwfiyxuphzherjljq,nvosmkcwjwbfiyxuphzgeualjq,nvosmkcwxdbflymuphzgeraljq,nvosmkcwpdbriyxuphzoeraljq,nvoshkcwcdbfiyxuphzgeravjq,nvosmkcetcbfiyxgphzgeraljq,nvosmkcwtdyfiyxuphzgerwqjq,nuosmkcwedbfiyxurhzgeraljq,nvosmkcwtdbfiixuphzctraljq,nvoszkcwtdbfwyxuphzgerpljq,nvormkcwtdbfiyxuphzgeralzn,nvosmkyttdbfiywuphzgeraljq,nvosmkcwtdbhiyxupazgeralhq,nvotmkcwtdbfiyxuphzgevalbq,nvosmkcwedbfiyxuphzguraljr,nvssmkcwtdbfiyxushzgeralbq,nvosmkcwtdeziyxuphzgeralhq,nvogmkcwtdbfiyxuphzgerrxjq,ncormkcwtdbfiyxuphzgeraloq,nvosmkcwbdbfiyeuphzgerqljq,nvosxkcwtdbfsyxupfzgeraljq,nvohmkcwtdbfiyxuphzseraajq,nvoscdcwtdbfiyxuphzgeralqq,neosmkcwtdbfiyxuchzgeralgq,njosmvcwpdbfiyxuphzgeraljq,nvosmkcwtwbfiyxuphzgehamjq,nvosmkcwtdbfiyxushzgejaljv,nvosmkcwodbfiyxuphzgeryqjq,nvoymqcwtdbfiyxuphzgeralbq,nvosmkcwtdjfiyxuphzgesaljb,nvjsmdcwedbfiyxuphzgeraljq,nvosmkcwydbfiyxuihzmeraljq,nvrsmkcwtdifiyxuphzgqraljq,nposmkcwtdbfiyxiohzgeraljq,dvosmkcwtdbfiyxuphzrvraljq,pvosmkcwudbfsyxuphzgeraljq,noosmkcwtdbfiyxuphtgexaljq,nvosmkcwtdbfiaxuphyferaljq,nvhsmlcwtdbfiyxuphzgeualjq,nvosekcwtdbbiyxuphzgerabjq,nvosvkcitdbfiyxuphzgerarjq,nvotmkkwtdbfiyxuphzgeraljj,nvosmecwtdbfiyxuphzgyralwq,hvosmkcwtdbfiyxuphzysraljq,nvosmkcvtdbfiyxlphzgeraljb,nvosmkcwttbfiyxuphngtraljq,nvoslkcwtdbfiyxuphzqeraljr,nxosmkcwtdbfibxuphzgrraljq,nvokmkhwtdbfiyxuphzgwraljq,nvosmkfwtdbfiyxuphzgdraljo,nvcsmkcwtdbfibxuphzgeraljl,nvosmkcwtdcfiaxuphzeeraljq,wvosmkcwtdbyiyxjphzgeraljq,nyosmbcwtjbfiyxuphzgeraljq,nvosmkcwtdbiiyxuahzieraljq,nqosmkcwtdbfiyxuyhzgerapjq,nvosmkcwtdbfiyxuwhzzetaljq,nvosmkcwfgbfiyxuphzgerrljq,nvosmbcwtdbfipxuphzderaljq,nvosmkcwtdgfiyxupdzgerjljq,noosmkcwtdcfiyxuphlgeraljq,nvonmkcutdbfiyxuphzieraljq,nvocmkcwtdbfiyyuphageraljq,nvosmkcwtdbfoyxuphzneraqjq,nvoskkcwtdbtiyxuphzgevaljq,ocosmkswtdbfiyxuphzgeraljq,nvosmkcqtdbfiyxfvhzgeraljq,noosmkcwtdbfiyquphzberaljq,nvosmkcwttbfijxuchzgeraljq,nvogmkcwtdbfiyxupazgeralaq,nvqsmkcwtdbfikxuphzgeraliq,nvosmkuwtdbfiyxuphzjwraljq,nyosmhcwtdbfiyxuphzgereljq,nvosmncwtdbfietuphzgeraljq,gvpsmkcwtdbfiyxuyhzgeraljq,nvozmkewtlbfiyxuphzgeraljq,nvostkcltpbfiyxuphzgeraljq,nvosmkcwtdbdiyxuphzgehaljz,nvosmkcwtjbziyxuphzgexaljq,nvosmkcwtdbfiyptphzggraljq,nvosmkcwtdbliyxupjzgebaljq,nvosmkawtdbfiyxupxzgtraljq,vvosmkcwtdbfiyxfphzperaljq,nvosmkawtdbfiyxutczgeraljq,nvosmkcbtdbuiyxrphzgeraljq,nvbsmkcwtdbfiyxdphzgerasjq,nvosnkcwqdsfiyxuphzgeraljq,nvosmkcwtdbfiyxwphzgzzaljq,nvosmkcwtdbffyquphzgeralcq,nvosmkcwtzbfiyxdphzgzraljq,nvysmkcwtdbfiycvphzgeraljq,nvowmkcwtdbfiycuyhzgeraljq,nvosbkcwtdbfiyiuphzgeraqjq,nvosmecwtdbfiyxupqzmeraljq,nvosmkcdtdbfhyxsphzgeraljq,nmosmkcwtdbziyxuphzgercljq,nvosmkcwtdbfiyxupfmgersljq,nvosmkcvtdbpyyxuphzgeraljq,nvosmkcwtkbfiyaupxzgeraljq,nvosmkcwtzbiiyxuphzgerazjq,nvoxmkcwtdbfiyxuphztegaljq,nvonmkcwtdafiyxuphzgerkljq,rvommkcwtdbfiyxzphzgeraljq,nvosmkcwthbfiysuphzgeraxjq,nvosmkcwtdbfnyxuphzgerccjq,nrosmzcwtdbfiyxuphkgeraljq,nvolmkcdtdbfiyxuphtgeraljq,nvosfkcwtdbfiyeuphcgeraljq,nvowmkcwtdbfhyxuphzgerafjq,gvosmkcwtdbfiyxupbpgeraljq,nvosmkcwtdbkiyxuphegebaljq,nvommufwtdbfiyxuphzgeraljq,uvksmkcwtdbfiysuphzgeraljq,nvosmkcwevbfiyxuphtgeraljq,nvosmkcmtdbfiycuphzgeraxjq,nvcsxkcwtdbfiyxuphzgeraljn,nvosmkcwtdbtiymuphzgeraltq,nvosmfcwtdlfjyxuphzgeraljq,svosmkcitdbfiyxuphzgsraljq";

        [Theory]
        [InlineData("1", 12L, "abcdef,bababc,abbcde,abcccd,aabcdd,abcdee,ababab")]
        [InlineData("Actual", 5390L, puzzleInput)]
        public void Test_Solve1(string nr, long expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve1(input));
        }

        [Theory]
        [InlineData("1", "fgij", "abcde,fghij,klmno,pqrst,fguij,axcye,wvxyz")]
        [InlineData("Actual", "nvosmkcdtdbfhyxsphzgraljq", puzzleInput)]
        public void Test_Solve2(string nr, string expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve2(input));
        }

        public long Solve1(string input)
        {
            var ids = input.Split(",");

            var grps = ids
                .Select(i => i.ToArray())
                .Select(i => i.GroupBy(c => c)
                .Select(g => g.Count()));

            var twos = grps.Where(g => g.Any(i => i == 2)).Count();
            var threes = grps.Where(g => g.Any(i => i == 3)).Count();

            return twos * threes;
        }

        public string Solve2(string input)
        {
            var ids = input.Split(",");

            for (int i = 0; i < ids.Length; i++)
            {
                for (int j = i + 1; j < ids.Length; j++)
                {
                    if (IsAlmostEqual(ids[i], ids[j]))
                    {
                        return GetCommonId(ids[i], ids[j]);
                    }
                }
            }

            return "notfound";
        }
        
        private static bool IsAlmostEqual(string a, string b)
        {
            var diff = 0;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) diff++;
                if (diff > 1) return false;
            }

            return diff == 1 ? true : false;
        }

        private string GetCommonId(string v1, string v2)
        {
            var result = new StringBuilder();

            for (int i = 0; i < v1.Length; i++)
            {
                if (v1[i] == v2[i]) result.Append(v1[i]);
            }

            return result.ToString();
        }
    }
}
