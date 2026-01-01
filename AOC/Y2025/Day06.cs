using System;

namespace rsmith985.AOC.Y2025;

public class Day06 : Day
{

    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var lines = this.GetLines();
        var num1 = getNumbers(lines[0]);
        var num2 = getNumbers(lines[1]);
        var num3 = getNumbers(lines[2]);
        var num4 = _useDefaultTestFile ? null : getNumbers(lines[3]);
        var symbols = _useDefaultTestFile ? getSymbols(lines[3]) : getSymbols(lines[4]);

        long tot = 0;
        for(int i = 0; i < num1.Count; i++)
        {
            if(_useDefaultTestFile)
            {
                var val = symbols[i] == "+" ?
                    num1[i] + num2[i] + num3[i] :
                    num1[i] * num2[i] * num3[i];
                tot += val;
            }
            else
            {
                var val = symbols[i] == "+" ?
                    num1[i] + num2[i] + num3[i] + num4[i] :
                    num1[i] * num2[i] * num3[i] * num4[i];
                tot += val;
            }
        }
        return tot;
    }

    public override object Part2()
    {
        var lines = this.GetGrid_Char().RemoveRow().Transpose().ToStringArray();
        
        var symbols = getSymbols(this.GetLines()[_useDefaultTestFile ? 3 : 4]);

        var grps = new List<List<long>>();
        var curr = new List<long>();
        foreach(var line in lines)
        {
            if(String.IsNullOrWhiteSpace(line))
            {
                grps.Add(curr);
                curr = new List<long>();
            }
            else
            {
                curr.Add(long.Parse(line));
            }
        }
        grps.Add(curr);

        long tot = 0;
        for(int i = 0; i < symbols.Count; i++)
        {
            var grp = grps[i];
            var val = symbols[i] == "+" ?
                grp.Aggregate((a, b) => a+b) :
                grp.Aggregate((a, b) => a*b);
            tot += val;
        }
        return tot;
    }



    private List<long> getNumbers(string line)
    {
        var vals = line.Split(' ', StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
        return vals.Select(i => long.Parse(i)).ToList();
    }
    private List<string> getSymbols(string line)
    {
        return line.Split(' ', StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries).ToList();
    }
}