using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Emgu.CV.Shape;

namespace rsmith985.AOC.Y2024;

public class Day24 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        (var nets, var funcs) = parseInput();

        while(true)
        {
            var undefined = nets.Where(n => n.Value == null).ToList();
            if(!undefined.Any()) break;
            foreach(var kv in undefined)
            {
                var net = kv.Key;
                var func = funcs[net];
                if(nets[func.in1].HasValue && nets[func.in2].HasValue)
                {
                    var i1 = nets[func.in1].Value;
                    var i2 = nets[func.in2].Value;
                    var result = 
                        func.gate == "AND" ? i1 && i2 :
                        func.gate == "OR" ? i1 || i2 :
                        (i1 && !i2) || (!i1 && i2);
                    //Console.WriteLine($"{net} = {func.gate}({func.in1}, {func.in2}) - {result}");
                    nets[net] = result;
                }
            }
            //Console.WriteLine("-");
        }

        var oValue = new string(nets.Where(n => n.Key.StartsWith("z")).OrderBy(n => n.Key).Reverse().Select(n => n.Value.Value ? '1' : '0').ToArray());
        //Console.WriteLine(oValue);
        return Convert.ToInt64(oValue, 2);
    }

    public override object Part2()
    {
        var invalid = new string[]{"wpq", "grf", "z18", "fvw", "z22", "mdb", "z36", "nwq"};
        var sorted = invalid.OrderBy(i => i);
        return sorted.Print();

        /*
        (var nets, var funcs) = parseInput();

        var dict = new Dictionary<string, HashSet<string>>();

        for(int i = 0; i <= 44; i++)
        {
            dict.Add(i.ToString().PadLeft(2, '0'), new HashSet<string>());
        }


        foreach(var func in funcs.ToList())
        {
            var num = func.Value.in1[1..];  // input xor and input AND
            if(dict.ContainsKey(num))
            {
                dict[num].Add(func.Key);
                if(func.Value.gate == "AND")
                {
                    foreach(var f2 in funcs.ToList())
                    {
                        if(f2.Value.in1 == func.Key || f2.Value.in2 == func.Key)
                        {
                            dict[num].Add(f2.Key); // Cout OR
                            if(f2.Value.in1 == func.Key)    // Other And
                                dict[num].Add(f2.Value.in2);
                            else if(f2.Value.in2 == func.Key)
                                dict[num].Add(f2.Value.in1);
                        }
                    }
                }
            }
            var num2 = func.Key[1..];       // output xor
            if(dict.ContainsKey(num2))
                dict[num2].Add(func.Key);
        }

        foreach(var key in dict.Keys)
        {
            Console.WriteLine(key);
            var group = new List<(string net, (string in1, string in2, string gate) func)>();
            foreach(var k in dict[key])
            {
                group.Add((k, funcs[k]));
            }
            foreach(var kv in group.OrderBy(i => i.func.gate == "XOR").ThenBy(i => i.func.gate == "AND"))
                writeFunc(kv.net, kv.func);
            Console.WriteLine();
        }

        var all = new HashSet<string>();
        foreach(var set in dict.Values)
        {
            foreach(var i in set)
                all.Add(i);
        }

        Console.WriteLine("missing");
        foreach(var net in funcs.Keys)
        {
            if(!all.Contains(net))
                writeFunc(net, funcs[net]);
        }
        return 0;
        */
    }

    private void writeFunc(string net, (string in1, string in2, string gate) func)
    {
        Console.WriteLine($"{net} = {func.gate}({func.in1}, {func.in2})");
    }

    private (Dictionary<string, bool?> nets, Dictionary<string, (string in1, string in2, string gate)> funcs) parseInput()
    {
        var lines = this.GetLines();

        var nets = new Dictionary<string, bool?>();
        var funcs = new Dictionary<string, (string, string, string)>();
        foreach(var line in lines)
        {
            if(line.Contains(":"))
            {
                var val = line[5..6];
                nets.Add(line[..3], val == "1");
            }
            else if(line.Contains("->"))
            {
                var in1 = line[..3];
                var func = line[4..7].Trim();
                var in2 = line[7..11].Trim();
                var out1 = line[^3..];

                if(!nets.ContainsKey(in1)) nets.Add(in1, null);
                if(!nets.ContainsKey(in2)) nets.Add(in2, null);
                if(!nets.ContainsKey(out1)) nets.Add(out1, null);

                funcs.Add(out1, (in1, in2, func));
            }
        }
        return (nets, funcs);
    }

    class LogicGate
    {
        public string NameIn1;
        public string NameIn2;
        public string NameOut;

        public bool? In1;
        public bool? In2;
        public bool? Out;

        public string Func;

    }
}
