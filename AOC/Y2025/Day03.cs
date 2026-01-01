using System;

namespace rsmith985.AOC.Y2025;

public class Day03 : Day
{
    //protected override bool _useDefaultTestFile => true;
    //protected override string _testString => "811111111111119";

    public override object Part1()
    {
        var tot = 0;
        foreach(var line in this.GetLines())
        {
            var max = maxJolt(line);
            //Console.WriteLine(max);
            tot += max;
        }
        return tot;
    }

    public override object Part2()
    {
        long tot = 0;
        foreach(var line in this.GetLines())
        {
            var max = maxJolt(line, 12);
            //Console.WriteLine(line + " >> " + max);
            tot += max;
        }
        return tot;
    }

    private int maxJolt(string str)
    {
        var max1 = str.Max();
        var idx = str.IndexOf(max1);

        if(idx == str.Length - 1)
        {
            var max2 = str[..^1].Max();
            return int.Parse(max2 + "" + max1);
        }
        else
        {
            var max2 = str[(idx+1)..].Max();
            return int.Parse(max1 + "" + max2);
        }
    }

    private long maxJolt(string str, int len)
    {
        var nums = str.Select(c => c.ToNum()).ToList();

        var stack = new Stack<int>();
        stack.Push(nums[0]);

        for(int i = 1; i < nums.Count; i++)
        {
            var n = nums[i];
            //Console.WriteLine(stack.Print());
            if(stack.Count + (nums.Count - i) == len)
            {
                stack.Push(n);
                continue;
            }

            if(stack.Count > 0 && n > stack.Peek())
                stack.Pop();
            while(stack.Count > 0 && n > stack.Peek() && (stack.Count + (nums.Count - i) > len))
                stack.Pop();
            
            if(stack.Count < len)
                stack.Push(n);
        }

        var rv = "";
        foreach(var i in stack.Reverse())
            rv += i.ToString();
        return long.Parse(rv);
    }
}
