using System;

namespace rsmith985.AOC.Y2025;

public class Day01 : Day
{
    //protected override bool _useDefaultTestFile => true;
    //protected override string _testString => "R49" + Environment.NewLine + "R102";

    public override object Part1()
    {
        var nums = this.GetLines()
            .Select(l => l.Replace('L', '-').Replace('R', '+'))
            .Select(i => int.Parse(i));

        var val = 50;
        var count = 0;
        foreach(var num in nums)
        {
            val = (val + num).Mod(100);
            if(val == 0) count++;
        }
        return count;
    }

    public override object Part2()
    {
        var nums = this.GetLines()
            .Select(l => l.Replace('L', '-').Replace('R', '+'))
            .Select(i => int.Parse(i));

        var val = 50;
        var count = 0;
        foreach(var num in nums)
        {
            var was0 = val == 0;
            val += num;
            if(val >= 100)
                count += (int)(val/100);
            else if(val <= 0)
                count += (int)(val/-100) + (was0 ? 0 : 1);
            val = val.Mod(100);

            //Console.WriteLine(val + " " + num + " " +  count);
        }
        return count;
    }   
        
}
