using System;
using Microsoft.Z3;

namespace rsmith985.AOC.Y2025;

public class Day10 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var machines = this.GetLines().Select(l => new Machine(l)).ToList();

        var tot = 0;
        foreach(var machine in machines)
        {
            var queue = new Queue<(int state, int presses)>();
            queue.Enqueue((0, 0));

            var presses = -1;
            while(presses < 0)
            {
                var item = queue.Dequeue();
                foreach(var num in machine.ButtonsP1)
                {
                    var state = item.state^num;
                    var p = item.presses + 1;
                    if(state == machine.TargetLight)
                    {
                        presses = p;
                        break;
                    }
                    queue.Enqueue((state, p));
                }
            }
            tot += presses;
        }
        return tot;
    }

    public override object Part2()
    {
        var ctx = new Context();
        var machines = this.GetLines().Select(l => new Machine(l)).ToList();

        long tot = 0;
        var zero = ctx.MkInt(0);
        foreach(var machine in machines)
        {
            var op = ctx.MkOptimize();

            // Creates variables for each button
            var buttonVars = machine.ButtonsP2.Select((b, i) => ctx.MkIntConst($"b{i}")).ToList();

            // Asserts every button can't be pressed negative number of times
            buttonVars.Perform(b => op.Add(ctx.MkGe(b, zero)));

            // Make an equality for each target jolt.
            // eg.  If Jolt[0] target is 10, and button[2] and button[3] increment this jolt[0]
            //      Then we are adding equation button2 + button3 = jolt0
            for(int j = 0; j < machine.Jolts.Count; j++)
            {
                // Get all the buttons that increment this jolt
                var vars = new List<IntExpr>();
                for(int b =0; b < machine.ButtonsP2.Count; b++)
                {
                    if(machine.ButtonsP2[b].Contains(j))
                        vars.Add(buttonVars[b]);
                }

                // make the equality
                var addition = ctx.MkAdd(vars);
                var target = ctx.MkInt(machine.Jolts[j]);
                op.Add(ctx.MkEq(addition, target));
            }

            // Goal is to minimize the total button presses, which is the addition of all of the buttons.
            var goal = ctx.MkAdd(buttonVars);
            var answer = op.MkMinimize(goal);

            // Some safety checks
            var result = op.Check();
            if(result != Status.SATISFIABLE)
                throw new Exception("Well this sucks");
            
            if(answer.Value is not Microsoft.Z3.IntNum intNum)
                throw new Exception("Well this sucks");

            tot += intNum.Int;
        }
        return tot;
    }
    public object Part2_WillNeverFinish()
    {
        // I tried, but yeah after doing the math, using approach from part 1 is impossible

        var machines = this.GetLines().Select(l => new Machine(l)).ToList();

        var tot = 0;
        foreach(var machine in machines)
        {
            //Console.WriteLine($"Starting Machine {machine.Jolts.PrintableString()}");
            //Console.WriteLine(machine.Jolts.PrintableString());

            var queue = new Queue<(int[] jolts, int presses)>();
            queue.Enqueue((new int[machine.Jolts.Count], 0));

            var minPresses = machine.Jolts.Sum() / machine.ButtonsP2[0].Count;

            var presses = -1;
            while(presses < 0)
            {
                var item = queue.Dequeue();

                //Console.WriteLine(item.jolts.PrintableString());
                foreach(var buttons in machine.ButtonsP2)
                {
                    var p = item.presses + 1;
                    var jolts = item.jolts.ToArray();
                    for(int i = 0; i < buttons.Count; i++)
                        jolts[buttons[i]]++;
                    
                    if(p < minPresses)
                    {
                        queue.Enqueue((jolts, p));
                        continue;
                    }

                    for(int i = 0; i < jolts.Length; i++)
                    {
                        if(jolts[i] > machine.Jolts[i])
                            goto Next;
                    }
                    
                    if(jolts.SequenceEqual(machine.Jolts))
                    {
                        presses = p;
                        break;
                    }

                    queue.Enqueue((jolts, p));
                    Next: ;
                }
            }
            //Console.WriteLine($"Done {presses}");
            tot += presses;
        }
        return tot;
    }


    class Machine
    {
        public int TargetLight{get;set;}

        public List<int> ButtonsP1 {get;set;}
        public List<List<int>> ButtonsP2{get;set;}
        public List<int> Jolts{get;set;}
        public Machine(string line)
        {
            var idx0 = line.IndexOf(']');
            var idx1 = line.IndexOf('{');
            var lightStr = new string(line[1..idx0].Reverse<char>().ToArray());
            var buttonStr = line[(idx0+2)..(idx1-1)];
            var joltStr = line[(idx1+1)..^1];

            this.TargetLight = Convert.ToInt32(lightStr.Replace('.', '0').Replace('#', '1'), 2);
            this.Jolts = joltStr.Split(',').Select(i => int.Parse(i)).ToList();
            
            this.ButtonsP1 = new List<int>();
            this.ButtonsP2 = new List<List<int>>();

            var buttons = buttonStr.Split(' ');
            foreach(var button in buttons)
            {
                // Makes a number where each bit it high if that button is on
                var num = button[1..^1].Split(',').Select(i => (int)Math.Pow(2, int.Parse(i))).Aggregate((a, b) => a^b);
                this.ButtonsP1.Add(num);
                this.ButtonsP2.Add(button[1..^1].Split(',').Select(i => int.Parse(i)).ToList());
            }
            // Thought this might help with naive approach, it wouldn't
            //this.ButtonsP2 = this.ButtonsP2.OrderBy(l => l.Count).Reverse().ToList();
        }
    }
}