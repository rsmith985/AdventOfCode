using System;

namespace rsmith985.AOC.Y2025;

public class Day11 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var graph = getInput();

        var start = graph.Nodes["you"];
        var end = graph.Nodes["out"];

        return graph.CountPaths_NoCycles(start, end);
    }

    public override object Part2()
    {
        var graph = getInput();

        var levels = getLevels(graph, "svr", "out");
        
        var lvlDac = getLevel(levels, "dac");
        var lvlFft = getLevel(levels, "fft");
        //Console.WriteLine("Level: " + lvlDac + " " + lvlFft);
        if(lvlDac < lvlFft)
        {
            var srv_dac = getNumBetween(graph, levels, 0, "svr", "dac");
            var dac_fft = getNumBetween(graph, levels, lvlDac + 1, "dac", "fft");
            var fft_out = getNumBetween(graph, levels, lvlFft + 1, "fft", "out");
            return srv_dac * dac_fft * fft_out;
        }
        else
        {
            var srv_fft = getNumBetween(graph, levels, 0, "svr", "fft");
            var fft_dac = getNumBetween(graph, levels, lvlFft + 1, "fft", "dac");
            var fft_out = getNumBetween(graph, levels,  lvlDac + 1, "dac", "out");
            return srv_fft * fft_dac * fft_out;
        }
    }


    private long getNumBetween(DirectedGraph<string> graph, List<HashSet<string>> levels, int startLvl, string start, string end)
    {
        //var levels = getLevels(graph, start, end);

        var dict = new Dictionary<string, long>();
        dict.Add(start, 1);

        for(int lvlNum = startLvl; lvlNum < levels.Count; lvlNum++)
        {
            var lvl = levels[lvlNum];
            //Console.WriteLine($"** Level {lvlNum++} **");
            foreach(var key in lvl)
            {
                var node = graph.Nodes[key];
                var from = node.GetConnectedFrom();
                var count = from.Sum(f => dict.ContainsKey(f.Data) ? dict[f.Data] : 0);
                if(key == end)
                    return count;
                dict.Add(key, count);
                //Console.WriteLine(key + " " + count);
            }
        }
        return 0;//dict[end];
    }

    private int getLevel(List<HashSet<string>> levels, string node)
    {
        for(int i = 0; i < levels.Count; i++)
        {
            if(levels[i].Contains(node))
                return i;
        }
        return -1;
    }
    private List<HashSet<string>> getLevels(DirectedGraph<string> graph, string start, string end)
    {
        var rv = new List<HashSet<string>>();
        var curr = new HashSet<string>(){start};
        var all = new HashSet<string>(){start};
        while(true)
        {
            var next = new HashSet<string>();
            foreach(var n in curr)
            {
                var node = graph.Nodes[n];
                foreach(var to in node.GetConnectedTo())
                {
                    if(to.GetConnectedFrom().All(n => all.Contains(n.Data)))
                        next.Add(to.Data);
                }
            }
            //next.PrintToConsole();
            next.Perform(n => all.Add(n));
            curr = next;
            rv.Add(curr);
            if(next.Contains(end))
                break;
        }

        return rv;
    }

    private DirectedGraph<string> getInput()
    {
        var graph = new DirectedGraph<string>();
        foreach(var line in this.GetLines())
        {
            var node = line[..3];
            
            foreach(var to in line[5..].Split(' ', StringSplitOptions.RemoveEmptyEntries))
                graph.Add(node, to);
        }
        return graph;
    }
}