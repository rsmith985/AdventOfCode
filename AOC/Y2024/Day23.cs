using System;

namespace rsmith985.AOC.Y2024;

public class Day23 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var graph = parseInput();

        var all = new List<List<Node<string>>>();
        var alreadyChecked = new HashSet<string>();
        foreach(var node in graph.Nodes.Values)
        {
            if(node.Data.StartsWith("t"))
            {
                alreadyChecked.Add(node.Data);
                all.AddRange(findConnectedSubgraphs3(node, alreadyChecked));
            }
        }
        return all.Count;
    }

    public override object Part2()
    {
        var graph = parseInput();

        var degreeDict = new Dictionary<int, int>();
        foreach(var node in graph.Nodes.Values)
        {
            if(!degreeDict.TryAdd(node.Degree, 1))
                degreeDict[node.Degree]++;
        }

        foreach(var kv in degreeDict.OrderBy(i => i.Key))
        {
            Console.WriteLine(kv.Key + " " + kv.Value);
        }

        for(int i = degreeDict.Keys.Max(); i >= 3; i--)
        {
            var cliques = findClique(graph, i);
            if(cliques.Count >= 1)
            {
                if(cliques.Count != 1) throw new Exception();

                var first = cliques.First();
                return first.Select(i => i.Data).OrderBy(i => i).PrintableString();
            }

        }

        return 0;
    }

    private List<List<Node<string>>> findClique(Graph<string> graph, int size)
    {
        var nodes = graph.Nodes.Values.ToArray();

        var rv = new List<List<Node<string>>>();
        findCliquesRecursive(0, size, new List<Node<string>>(), rv, nodes);
        return rv;
    }    
    private void findCliquesRecursive(int startVertex, int cliqueSize, List<Node<string>> currentClique, List<List<Node<string>>> cliques, Node<string>[] nodes)
    {
        if (currentClique.Count == cliqueSize)
        {
            cliques.Add(currentClique.ToList()); // Add a copy
            return;
        }

        if (startVertex >= nodes.Length)
        {
            return; // No more vertices to explore
        }


        for (int v = startVertex; v < nodes.Length; v++)
        {
            var node1 = nodes[v];
            bool canAdd = true;
            foreach (var node2 in currentClique)
            {
                if (!node1.IsConnectedTo(node2)) // Check for edge
                {
                    canAdd = false;
                    break;
                }
            }

            if (canAdd)
            {
                currentClique.Add(node1);
                findCliquesRecursive(v + 1, cliqueSize, currentClique, cliques, nodes);
                currentClique.RemoveAt(currentClique.Count - 1); // Backtrack
            }
        }
    }

    private List<List<Node<string>>> findConnectedSubgraphs3(Node<string> node, HashSet<string> dontCheck)
    {
        var possible = new HashSet<Node<string>>(node.GetConnected().Where(i => i.Degree >= 2 && !dontCheck.Contains(i.Data)));

        var alreadyChecked = new HashSet<string>();
        var rv = new List<List<Node<string>>>();
        foreach(var n1 in possible)
        {
            alreadyChecked.Add(n1.Data);
            foreach(var n2 in n1.GetConnected())
            {
                if(possible.Contains(n2) && !dontCheck.Contains(n2.Data) && !alreadyChecked.Contains(n2.Data))
                {
                    rv.Add(new List<Node<string>>(){node, n1, n2});
                }
            }
        }
        return rv;
    }

    private Graph<string> parseInput()
    {
        var graph = new Graph<string>();

        foreach(var line in this.GetLines())
        {
            var key1 = line[..2];
            var key2 = line[3..];

            graph.Add(key1, key2);
        }
        return graph;
    }
}
