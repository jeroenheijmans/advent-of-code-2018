using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

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
        [InlineData("Actual", "", puzzleInput)]
        public void Test_Solve2(string nr, string expected, string input)
        {
            Assert.Equal(nr, nr); // Suppresses warning
            Assert.Equal(expected, Solve2(input));
        }

        public long Solve1(string input)
        {
            var ids = input.Split(",");

            var idCharArrays = ids.Select(i => i.ToArray());

            var grps = idCharArrays.Select(i => i.GroupBy(c => c).Select(g => g.Count()));

            var twos = grps.Where(g => g.Any(i => i == 2)).Count();
            var threes = grps.Where(g => g.Any(i => i == 3)).Count();
            var result = twos * threes;

            return result;
        }

        public string Solve2(string input)
        {
            var ids = input.Split(",");

            for (int i = 0; i < ids.Length; i++)
            {
                for (int j = i + 1; j < ids.Length; j++)
                {
                    if (GetLevenshteinDistance(ids[i], ids[j]) == 1)
                    {
                        // Plus some manual work:
                        return ids[j] + " vs " + ids[j];
                    }
                }
            }

            return "notfound";
        }

        // Variation of https://stackoverflow.com/a/9453762/419956
        private static int GetLevenshteinDistance(string a, string b)
        {
            if (String.IsNullOrEmpty(a)) throw new ArgumentException(nameof(a));
            if (String.IsNullOrEmpty(b)) throw new ArgumentException(nameof(b));

            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
            {
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            }

            return distances[lengthA, lengthB];
        }
    }
}
