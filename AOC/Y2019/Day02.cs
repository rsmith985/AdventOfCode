using System;

namespace rsmith985.AOC.Y2019;

public class Day02 : Day
{
    //protected override string _testString => "1,9,10,3,2,3,11,0,99,30,40,50";

    public override object Part1()
    {
        var nums = this.GetLines()[0].Split(',').Select(i => long.Parse(i)).ToArray();

        nums[1] = 12;
        nums[2] = 2;
        for(int i = 0; i < nums.Length; i+=4)
        {
            var op = nums[i];
            if(op == 99)
                break;
            else if(op == 1)
                nums[nums[i+3]] = nums[nums[i+1]] + nums[nums[i+2]];
            else if(op == 2)
                nums[nums[i+3]] = nums[nums[i+1]] * nums[nums[i+2]];
            else
                throw new Exception("invalid op");   
        }

       // nums.PrintToConsole();
        return nums[0];
    }

    public override object Part2()
    {
        var baseMemory = this.GetLines()[0].Split(',').Select(i => long.Parse(i)).ToArray();

        for(int n = 0; n <= 99; n++)
        {
            for(int v = 0; v <= 99; v++)
            {
                var nums = baseMemory.ToArray();
                nums[1] = n;
                nums[2] = v;
                for(int i = 0; i < nums.Length; i+=4)
                {
                    var op = nums[i];
                    if(op == 99)
                        break;
                    else if(op == 1)
                        nums[nums[i+3]] = nums[nums[i+1]] + nums[nums[i+2]];
                    else if(op == 2)
                        nums[nums[i+3]] = nums[nums[i+1]] * nums[nums[i+2]];
                    else
                        goto Next; 
                }
                if(nums[0] == 19690720)
                    return 100 * n + v;
                Next:;
            }
        }
        return -1;
    }    
}