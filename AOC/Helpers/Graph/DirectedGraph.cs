using System.Collections;

namespace rsmith985.AOC;

public class DirectedGraph<T>
{
    public Dictionary<T, DirectedNode<T>> Nodes{get;}
    public Dictionary<string, DirectedEdge<T>> Edges{get;}

    public DirectedGraph()
    {
        this.Nodes = new();
        this.Edges = new();
    }

    public DirectedNode<T> GetOrAddNode(T data)
    {
        if(this.Nodes.ContainsKey(data))
            return this.Nodes[data];
        var node = new DirectedNode<T>(data);
        this.Nodes.Add(data, node);
        return node;
    }

    public void Add(T n)
    {
        var node = new DirectedNode<T>(n);
        if(this.Nodes.ContainsKey(n)) return;
        this.Nodes.Add(n, node);
    }
    public void Add(T n1, T n2, double weight = 1.0)
    {
        this.Add((n1, n2), weight);
    }
    public void Add((T, T) edge, double weight = 1.0)
    {
        var key = GetEdgeKey(edge);
        if(this.Edges.ContainsKey(key)) return;

        var n1 = this.GetOrAddNode(edge.Item1);
        var n2 = this.GetOrAddNode(edge.Item2);
        var e = new DirectedEdge<T>(key, n1, n2, weight);
        this.Edges.Add(key, e);
    }
    public void Remove(DirectedNode<T> node)
    {
        foreach(var e in node.EdgesFrom.ToList())
            this.Remove(e);
        foreach(var e in node.EdgesTo.ToList())
            this.Remove(e);
        this.Nodes.Remove(node.Data);
    }
    public void Remove(DirectedEdge<T> edge)
    {
        if(!this.Edges.ContainsKey(edge.Key))
            throw new Exception();
        
        edge.From.EdgesTo.Remove(edge);
        edge.To.EdgesFrom.Remove(edge);
        this.Edges.Remove(edge.Key);
    }

    public static string GetEdgeKey((T, T) items)
    {
        var str1 = items.Item1.ToString();
        var str2 = items.Item2.ToString();
        return str1 + "_" + str2;
    }

    public void Simplify()
    {
        var toRemove = new List<DirectedNode<T>>();
        foreach(var node in this.Nodes.Values)
        {
            if(node.EdgesFrom.Count == 1 && node.EdgesTo.Count == 1)
            {
                toRemove.Add(node);
            }
        }

        foreach(var node in toRemove)
        {
            var from = node.EdgesFrom.First();
            var to = node.EdgesTo.First();

            this.Remove(node);
            this.Add(from.From.Data, to.To.Data, from.Weight + to.Weight);
        }
    }

    public void SwapEdgeDirection(DirectedEdge<T> edge)
    {
        var newEdge = (edge.To.Data, edge.From.Data);
        var weight = edge.Weight;
        this.Remove(edge);
        this.Add(newEdge, weight);
    }

    public bool PathExists(T start, T end)
    {
        var sNode = this.Nodes[start];
        var eNode = this.Nodes[end];
        return PathExists(sNode, eNode);
    }
    public bool PathExists(DirectedNode<T> start, DirectedNode<T> end)
    {
        var queue = new Queue<DirectedNode<T>>();
        var visited = new HashSet<T>();

        queue.Enqueue(start);
        visited.Add(start.Data);

        while(queue.Any())
        {
            var curr = queue.Dequeue();
            foreach(var conn in curr.GetConnectedTo())
            {
                if(conn == end) 
                    return true;

                if(visited.Contains(conn.Data)) continue;

                queue.Enqueue(conn);
                visited.Add(conn.Data);
            }
        }
        return false;
    }

    /// <summary>
    /// Same as GetPaths, but doesn't track actual path if just need count so runs faster
    /// </summary>
    public int CountPaths_NoCycles(DirectedNode<T> start, DirectedNode<T> end)
    {
        var queue = new Queue<DirectedNode<T>>();
        var visited = new HashSet<T>();

        queue.Enqueue(start);
        visited.Add(start.Data);

        var paths = 0;
        while(queue.Any())
        {
            var curr = queue.Dequeue();
            foreach(var edge in curr.EdgesTo)
            {
                if(edge.To == end)
                {
                    paths++;
                    continue;
                }

                queue.Enqueue(edge.To);
            }
        }
        return paths;
    }
    /// <summary>
    /// Returns all paths between 2 points.  This doesn't check for cycles and so wouldn't return if there is one
    /// </summary>
    public List<List<DirectedEdge<T>>> GetAllPaths_NoCycles(DirectedNode<T> start, DirectedNode<T> end)
    {
        var queue = new Queue<(DirectedNode<T> node, List<DirectedEdge<T>> path)>();
        var visited = new HashSet<T>();

        queue.Enqueue((start, new List<DirectedEdge<T>>()));
        visited.Add(start.Data);

        var paths = new List<List<DirectedEdge<T>>>();
        while(queue.Any())
        {
            var curr = queue.Dequeue();
            var node = curr.node;
            var path = curr.path.ToList();
            foreach(var edge in node.EdgesTo)
            {
                path.Add(edge);
                if(edge.To == end)
                {
                    paths.Add(path);
                    continue;
                }

                queue.Enqueue((edge.To, path));
            }
        }
        return paths;
    }


    public List<HashSet<T>> GetAllPaths_ContainsCycles(DirectedNode<T> start, DirectedNode<T> end)
    {
        var queue = new Queue<(DirectedNode<T> node, HashSet<T> path)>();

        queue.Enqueue((start, new HashSet<T>()));

        var paths = new List<HashSet<T>>();
        while(queue.Any())
        {
            var curr = queue.Dequeue();
            var node = curr.node;
            var path = new HashSet<T>(curr.path);
            foreach(var edge in node.EdgesTo)
            {
                if(path.Contains(edge.To.Data)) 
                    continue;

                path.Add(edge.To.Data);
                if(edge.To == end)
                {
                    paths.Add(path);
                    path.PrintToConsole();
                    continue;
                }
                queue.Enqueue((edge.To, path));
            }
        }
        return paths;
    }

    public DirectedGraph<T> Copy(bool? edgesUnique = null)
    {
        var copy = new DirectedGraph<T>();
        foreach(var e in this.Edges.Values)
            copy.Add(e.From.Data, e.To.Data, e.Weight);
        return copy;
    }
    public DirectedGraph<T> GetSubGraph(HashSet<T> itemsInSubGraph)
    {
        var rv = new DirectedGraph<T>();
        foreach(var item in itemsInSubGraph)
            rv.Add(item);

        foreach(var item in itemsInSubGraph)
        {
            var node = this.Nodes[item];
            foreach(var conn in node.GetConnectedTo())
            {
                if(itemsInSubGraph.Contains(conn.Data))
                    rv.Add((item, conn.Data));
            }
        }
        return rv;
    }
}

public class DirectedNode<T>
{
    public T Data{ get; }

    public List<DirectedEdge<T>> EdgesTo{get;}
    public List<DirectedEdge<T>> EdgesFrom{get;}

    public DirectedNode(T data)
    {
        this.Data = data;
        this.EdgesTo = new List<DirectedEdge<T>>();
        this.EdgesFrom = new List<DirectedEdge<T>>();
    }

    public IEnumerable<DirectedNode<T>> GetConnectedTo()
    {
        foreach(var edge in this.EdgesTo)
            yield return edge.GetOpposite(this);
    }
    public IEnumerable<DirectedNode<T>> GetConnectedFrom()
    {
        foreach(var edge in this.EdgesFrom)
            yield return edge.GetOpposite(this);
    }
}


public class DirectedEdge<T>
{
    public string Key{get;}
    public DirectedNode<T> From{get;}
    public DirectedNode<T> To{get;}
    public double Weight {get;}

    public DirectedEdge(string key, DirectedNode<T> from, DirectedNode<T> to, double weight = 1.0)
    {
        this.Key = key;
        this.From = from;
        this.To = to;
        this.From.EdgesTo.Add(this);
        this.To.EdgesFrom.Add(this);
        this.Weight = weight;
    }

    public DirectedNode<T> GetOpposite(DirectedNode<T> node)
    {
        if(node == this.From) return this.To;
        if(node == this.To) return this.From;
        throw new Exception();
    }
    public T GetOpposite(T node)
    {
        if(node.Equals(this.From.Data)) return this.To.Data;
        if(node.Equals(this.To.Data)) return this.From.Data;
        throw new Exception();
    }
}
