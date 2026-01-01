using System;

namespace rsmith985.AOC.Y2025;

public class Day05 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var ranges, var items) = parseInput();

        var fresh = 0;
        foreach(var item in items)
        {
            foreach(var range in ranges)
            {
                if(item >= range.Start && item <= range.End)
                {
                    fresh++;
                    goto Next;
                }
            }
            Next: ;
        }
        return fresh;
    }

    public override object Part2()
    {
        (var ranges, var items) = parseInput();

        ranges = ranges.OrderBy(r => r.Start).ToList();

        var final = new List<Range>();
        var curr = ranges[0];
        for(int i = 1; i < ranges.Count; i++)
        {
            var next = ranges[i];

            if(next.Start <= curr.End)
            {
                //Console.WriteLine($"Merge: {curr.Start}-{curr.End} + {next.Start}-{next.End} = {curr.Start}-{Math.Max(curr.End, next.End)}");
                curr.End = Math.Max(curr.End, next.End);
            }
            else
            {
                //Console.WriteLine($"Add: {curr.Start}-{curr.End}");
                final.Add(curr);
                curr = next;
            }
        }
        final.Add(curr);
        
        long tot = 0;
        foreach(var range in final)
        {
            tot += (range.End - range.Start) + 1;
            //Console.WriteLine($"{range.Start}-{range.End}-{tot}");
        }

        return tot;
    }


    private (List<Range> ranges, List<long> items) parseInput()
    {
        var ranges = new List<Range>();
        var items = new List<long>();

        bool b = false;
        foreach(var line in this.GetLines())
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                b = true;
                continue;
            }

            if(b)
                items.Add(long.Parse(line));
            else
                ranges.Add(new Range(line.Split2(long.Parse, "-")));
        }
        return (ranges, items);
    }

    class Range((long start, long end) item)
    {
        public long Start{get;set;} = item.start;
        public long End{get;set;}  = item.end;
    }
}