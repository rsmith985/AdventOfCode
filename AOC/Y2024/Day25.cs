using System;
using Emgu.CV.Structure;

namespace rsmith985.AOC.Y2024;

public class Day25 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var curr = new List<string>();
        var keys = new List<int[]>();
        var locks = new List<int[]>();
        int idx = 0;
        foreach(var line in this.GetLines())
        {
            if(idx == 7)
            {
                addItem(curr);
                curr = new List<string>();
                idx = 0;
            }
            else
            {
                curr.Add(line);
                idx++;
            }
        }
        addItem(curr);

        var count = 0;
        foreach(var l in locks)
        {
            foreach(var k in keys)
            {
                var valid = true;
                for(int i = 0; i < 5; i++)
                {
                    if(l[i] + k[i] <= 5) continue;
                    valid = false;
                    break;
                }
                if(valid) 
                    count++;
            }
        }

        return count;

        void addItem(List<string> set)
        {
            (var isLock, int[] nums) = getCounts(curr);
            if(isLock)
                locks.Add(nums);
            else
                keys.Add(nums);
        }
    }

    private (bool isLock, int[] nums) getCounts(List<string> set)
    {
        bool isLock = set[0][0] == '#';
        var nums = new int[5];
        for(int x = 0; x < 5; x++)
        {
            var count = 0;
            for(int y = 1; y <= 5; y++)
            {
                if(set[y][x]== '#')
                    count++;
            }
            nums[x] = count;
        }
        return (isLock, nums);
    }

    public override object Part2()
    {
        throw new NotImplementedException();
    }
}
