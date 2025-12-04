using System;
using System.Collections.Concurrent;
using System.Globalization;

namespace rsmith985.AOC.Y2024;

public class Day22 : Day
{

    //protected override bool _useDefaultTestFile => true;

    int _numIterations = 2000;
    public override object Part1()
    {
        var nums = this.GetLines().Select(i => int.Parse(i));

        return nums.Sum(n => (long)valueAfterN(n, _numIterations));
    }

    public override object Part2()
    {
        var nums = this.GetLines().Select(i => int.Parse(i));

        var seqTotals = new ConcurrentDictionary<int, int>();
        Parallel.ForEach(nums, n => 
        {
            var seen = new HashSet<int>();
            var prices = new List<int>();
            prices.Add(n % 10);
            for(int i = 0; i < _numIterations; i++)
            {
                n = performIteration(n);
                prices.Add(n % 10);

                if(i >= 4)
                {
                    var d1 = prices[i-3] - prices[i-4];
                    var d2 = prices[i-2] - prices[i-3];
                    var d3 = prices[i-1] - prices[i-2];
                    var d4 = prices[i-0] - prices[i-1];
                    var d = d4 + d3 * 100 + d2 * 10000 + d1 * 1000000;

                    if(seen.Contains(d)) continue;
                    seen.Add(d);
                    seqTotals.AddOrUpdate(d, prices[i], (key, prev) => prev + prices[i]);
                }
            }
        });

        return seqTotals.Values.Max();
    }

    private int valueAfterN(int num, int iterations)
    {
        for(int i = 0; i < iterations; i++)
        {
            num = performIteration(num);
        }
        return num;
    }

    private int performIteration(int num)
    {
        var n2 = num << 6;
        num ^= n2;
        num &= 16777215;
        n2 = num >> 5;
        num ^= n2;
        num &= 16777215;
        n2 = num << 11;
        num ^= n2;
        num &= 16777215;
        return num;
    }
}
