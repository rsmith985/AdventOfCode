using System;

namespace rsmith985.AOC.Y2025;

public class Day02 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        long tot = 0;
        foreach(var item in this.GetLines()[0].Split(','))
        {
            (var num1, var num2) = item.Split2(long.Parse, "-");
            for(var i = num1; i <= num2; i++)
            {
                if(isDuplicate(i))
                {
                    tot += i;
                }
            }
        }
        return tot;
    }

    public override object Part2()
    {
        long tot = 0;
        foreach(var item in this.GetLines()[0].Split(','))
        {
            (var num1, var num2) = item.Split2(long.Parse, "-");
            for(var i = num1; i <= num2; i++)
            {
                if(isRepeated(i))
                {
                    tot += i;
                }
            }
        }
        return tot;
    }

    private bool isDuplicate(long num)
    {
        var str = num.ToString();
        if(str.Length % 2 == 1) return false;

        var half = str.Length/2;
        return str[..half] == str[half..];
    }

    private bool isRepeated(long num)
    {
        var str = num.ToString();
        for(int i = 2; i <= str.Length; i++)
        {
            var size = str.Length/i;
            if(size * i != str.Length) continue;

            var valid = true;
            var val = str[..size];
            for(int j = 1; j < i && valid; j++)
            {
                if(str[(size*j)..(size*(j+1))] != val)
                    valid = false;
            }

            if(valid)
            {
                //Console.WriteLine(str);
                return true;
            }
        }
        return false;
    }
}
