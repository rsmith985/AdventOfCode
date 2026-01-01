using System;
using Emgu.CV.XImgproc;

namespace rsmith985.AOC.Helpers.Graph;

#region Prim
public class MSTPrim<K>
{
    private Dictionary<K, double> _distTo;
    private Dictionary<K, Edge<K>> _edgeTo;
    private HashSet<K> _marked;
    private PriorityQueue<K, double> _queue;
    private Graph<K> _graph;

    public MSTPrim(Graph<K> graph)
    {
        _graph = graph;
    }

    public IEnumerable<Edge<K>> Compute()
    {
        _distTo = new Dictionary<K, double>();
        _edgeTo = new Dictionary<K, Edge<K>>();
        _marked = new HashSet<K>();
        _queue = new PriorityQueue<K, double>();

        foreach(var key in _graph.Nodes.Keys)
            _distTo.Add(key, double.PositiveInfinity);

        foreach(var key in _graph.Nodes.Keys)
        {
            if(!_marked.Contains(key))
                process(key);
        }

        var mst = new Queue<Edge<K>>();
        foreach(var key in _edgeTo.Keys)
        {
            var e = _edgeTo[key];
            if(e != null)
            mst.Enqueue(e);
        }

        return mst;
    }

    private void process(K key)
    {
        _distTo[key] = 0.0;

        _queue.Insert(key, _distTo[key]);
        while(!_queue.IsEmpty)
        {
            scan(_queue.PopMin());
        }
    }

    private void scan(K key)
    {
        _marked.Add(key);

        foreach(var e in _graph.Nodes[key].Edges)
        {
            var w = e.GetOpposite(key);
            if(_marked.Contains(w)) continue;
            if(e.Weight < _distTo[w])
            {
                _distTo[w] = e.Weight;
                _edgeTo[w] = e;

                if(_queue.Contains(w))
                    _queue.ChangePriority(w, _distTo[w]);
                else
                    _queue.Insert(w, _distTo[w]);
            }
        }
    }
}
#endregion

#region Kruskal
public class MSTKruskal<K>
{
    private Graph<K> _graph;

    private Node<K>[] _nodes;

    private int _numNodes;

    public MSTKruskal(Graph<K> graph)
    {
        _graph = graph;

        _numNodes = graph.Nodes.Count;
        _nodes = new Node<K>[_numNodes];
        var idx = 0;
        foreach(var node in graph.Nodes.Values)
        {
            node.ArrayIndex = idx;
            _nodes[idx++] = node;
        }
    }
    
    public List<Edge<K>> Compute(int stopAt = -1)
    {
        var rv = new List<Edge<K>>();

        var subsets = new Subset[_numNodes];
        for (int v = 0; v < _numNodes; ++v)
        {
            subsets[v] = new Subset(v, 0);
            subsets[v].Parent = v;
            subsets[v].Rank = 0;
        }

        var edges = _graph.Edges.Values.ToList();
        edges.Sort();

        foreach(var edge in edges)
        {
            int x = find(subsets, edge.Node1.ArrayIndex);
            int y = find(subsets, edge.Node2.ArrayIndex);

            if (x != y)
            {
                rv.Add(edge);
                union(subsets, x, y);
            }

            if(rv.Count == (_numNodes - 1) || (stopAt > 0 && rv.Count == stopAt))
                break;
        }

        return rv;
    }

    private int find(Subset[] subsets, int i)
    {
        if (subsets[i].Parent != i)
            subsets[i].Parent = find(subsets, subsets[i].Parent);

        return subsets[i].Parent;
    }

    private void union(Subset[] subsets, int x, int y)
    {
        int xroot = find(subsets, x);
        int yroot = find(subsets, y);

        if (subsets[xroot].Rank < subsets[yroot].Rank)
            subsets[xroot].Parent = yroot;
        else if (subsets[xroot].Rank > subsets[yroot].Rank)
            subsets[yroot].Parent = xroot;
        else
        {
            subsets[yroot].Parent = xroot;
            subsets[xroot].Rank++;
        }
    }

    class Subset(int parent, int rank)
    {
        public int Parent{get;set;} = parent;
        public int Rank{get;set;} = rank;
    }
}
#endregion