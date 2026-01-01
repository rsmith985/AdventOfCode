using System;

namespace rsmith985.AOC.Y2019;

public class Day01 : Day
{
    //protected override bool _useDefaultTestFile => true;
    //protected override string _testString => "1969";

    public override object Part1()
    {
        return this.GetLines()
            .Select(l => long.Parse(l))
            .Sum(m => (long)Math.Floor(m/3.0) - 2);
    }

    public override object Part2()
    {
        long tot = 0;
        foreach(var line in this.GetLines())
        {
            var subTot = 0;
            var num = int.Parse(line);

            while(num > 0)
            {
                num = (int)Math.Floor(num/3.0) - 2;  
                if(num > 0)
                    subTot += num;  
            }
            tot += subTot;
        }
        return tot;
    }    
}
