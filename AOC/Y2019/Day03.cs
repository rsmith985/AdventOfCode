using System;
using PointT = (long X, long Y);

namespace rsmith985.AOC.Y2019;

public class Day03 : Day
{
    protected override string _testString => 
@"R8,U5,L5,D3
U7,R6,D4,L4";

    public override object Part1()
    {
        var input = this.GetLines();
        var lines1 = getSegments(input[0]);
        var lines2 = getSegments(input[1]);

        var minDist = int.MaxValue;
        for(int i = 0; i < lines1.Count; i++)
        {
            for(int j = 0; j < lines2.Count; j++)
            {
                var l1 = lines1[i];
                var l2 = lines2[j];
                var p = l1.LinesIntersectPoint(l2);
                Console.WriteLine(l1 + "  |  " + l2 + "  |  " + p);
                if(p != null)
                {
                    var pp = p.Value;
                    if(pp == Point.Empty) continue;
                    //Console.WriteLine(pp);

                    var dist = Math.Abs(pp.X) + Math.Abs(pp.Y);
                    if(dist < minDist)
                        minDist = dist;
                }
            }
        }

        return minDist;
    }

    private List<(Point, Point)> getSegments(string path)
    {
        var curr = Point.Empty;
        var segments = new List<(Point, Point)>();
        foreach(var delta in path.Split(','))
        {
            var dir = 
                delta[0] == 'R' ? Direction.E :
                delta[0] == 'L' ? Direction.W :
                delta[0] == 'U' ? Direction.S :
                delta[0] == 'D' ? Direction.N :
                throw new Exception();
            var amt = int.Parse(delta[1..]);
            var next = curr.GetNeighbor(dir, amt);
            segments.Add((curr, next));
            curr = next;
        }
        return segments;
    }

    public override object Part2()
    {
        return -1;
    }  
}
