using System;
using Microsoft.VisualBasic;
using rsmith985.AOC.Helpers.Graph;

namespace rsmith985.AOC.Y2025;

public class Day08 : Day
{
    //protected override bool _useDefaultTestFile => true;

    private Graph<Vector3> _inputGraph;

    public override object Part1()
    {
        parseInput();
        
        var num = _useDefaultTestFile ? 10 : 1000;

        var edges = _inputGraph.Edges.Values.ToList();
        edges.Sort();

        var graph = new Graph<Vector3>();
        for(int i = 0; i < num; i++)
            graph.Add(edges[i].Node1.Data, edges[i].Node2.Data, edges[i].Weight);

        var subgraphs = graph.GetSubGraphs();

        return subgraphs
            .Select(g => g.Nodes.Count)
            .OrderBy(s => s)
            .Reverse()
            .Take(3)
            .Aggregate(1, (a, b) => a*b);
    }

    public override object Part2()
    {
        parseInput();
        var alg = new MSTKruskal<Vector3>(_inputGraph);
        var result = alg.Compute();

        return (long)result[^1].Node1.Data.X * (long)result[^1].Node2.Data.X;

    }
    private void parseInput()
    {
        if(_inputGraph != null) return;

        var vectors = new List<Vector3>();
        foreach(var line in this.GetLines())
            vectors.Add(new Vector3(line));

        _inputGraph = new Graph<Vector3>();
        for(int i = 0; i < vectors.Count - 1; i++)
        {
            var v1 = vectors[i];
            for(int j = i + 1; j < vectors.Count; j++)
            {
                var v2 = vectors[j];
                var x = (double)(v1.X - v2.X);
                var y = (double)(v1.Y - v2.Y);
                var z = (double)(v1.Z - v2.Z);
                var dist = Math.Sqrt(x*x + y*y + z*z);

                _inputGraph.Add(v1, v2, dist);
            }
        }
    }

}