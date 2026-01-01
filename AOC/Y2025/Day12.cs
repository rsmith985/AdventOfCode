using System;

namespace rsmith985.AOC.Y2025;

public class Day12 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var input = this.GetLines();

        var count = 0;
        for(int i = 30; i < input.Length; i++)
        {
            var line = input[i];
            var w = int.Parse(line[0..2]);
            var h = int.Parse(line[3..5]);
            var n0 = int.Parse(line[7..9]);
            var n1 = int.Parse(line[10..12]);
            var n2 = int.Parse(line[13..15]);
            var n3 = int.Parse(line[16..18]);
            var n4 = int.Parse(line[19..21]);
            var n5 = int.Parse(line[22..24]);

            var s1 = w*h;
            var s2 = n0*5+n1*7+n2*6+n3*7+n4*7+n5*7;
            var s3 = (n0+n1+n2+n3+n4+n5)*9;
            if(s3 <= s1)
                count++;
            //Console.WriteLine($"{w}x{h}: {(s1 > s2)} {(s1 > s3)} | {s1},{s2},{s3}");
        }
        return count;
    }

    public override object Part2()
    {
        return "Yay!";
    }
}