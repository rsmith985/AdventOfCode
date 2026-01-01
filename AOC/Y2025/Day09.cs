using System;
using PointLong = (long X, long Y);
using LineTupleLong = ((long X, long Y) p1, (long X, long Y) p2);

namespace rsmith985.AOC.Y2025;

public class Day09 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var points = new List<PointLong>();

        foreach(var line in this.GetLines())
            points.Add(line.Split2(long.Parse, ","));

        return points.GetAllPairs().Max(p => area(p.i1, p.i2));
    }

    public override object Part2()
    {
        var poly = new List<PointLong>();

        foreach(var line in this.GetLines())
            poly.Add(line.Split2(long.Parse, ","));

        var lines = poly.GetAdjacentPairs(true).ToList();
/*
        foreach(var pair in poly.GetAllPairs())
        {
            if(valid(pair.i1, pair.i2, lines))
            {
                Console.WriteLine($"valid - {pair.i1.X}, {pair.i1.Y} | {pair.i2.X}, {pair.i2.Y} > {area(pair.i1, pair.i2)}");


            }
            else
            {
                Console.WriteLine($"not valid - {pair.i1.X}, {pair.i1.Y} | {pair.i2.X}, {pair.i2.Y} > {area(pair.i1, pair.i2)}");
            }
        }
        return 0;
        */

        return poly.GetAllPairs()
            .Where(p => valid(p.i1, p.i2, lines))
            .Max(p => area(p.i1, p.i2));
    }

    private bool valid(PointLong p1, PointLong p2, List<LineTupleLong> lines)
    {
        var l = Math.Min(p1.X, p2.X) + 1;
        var r = Math.Max(p1.X, p2.X) - 1;
        var b = Math.Min(p1.Y, p2.Y) + 1;
        var t = Math.Max(p1.Y, p2.Y) - 1;

        if(t < b || r < l) 
            return false; // technically valid but ignore it
        //Console.WriteLine(l + " " + r + " | " + b +" " + t);

        var l1 = ((l, b), (l, t));
        if(lines.Any(l => l.LinesIntersect(l1)))
            return false;
    
        var l2 = ((l, t), (r, t));
        if(lines.Any(l => l.LinesIntersect(l2)))
            return false;

        var l3 = ((r,t), (r, b));
        if(lines.Any(l => l.LinesIntersect(l3)))
            return false;

        var l4 = ((r, b), (l, b));
        if(lines.Any(l => l.LinesIntersect(l4)))
            return false;
            /*
        var l1 = (p1, (p2.X, p1.Y));
        if(lines.Any(l => l.LinesIntersect(l1)))
            return false;
    
        var l2 = ((p2.X, p1.Y), p2);
        if(lines.Any(l => l.LinesIntersect(l2)))
            return false;

        var l3 = (p2, (p1.X, p2.Y));
        if(lines.Any(l => l.LinesIntersect(l3)))
            return false;

        var l4 = ((p1.X, p2.Y), p1);
        if(lines.Any(l => l.LinesIntersect(l4)))
            return false;
            */
        return true;
    }

    private long area(PointLong p1, PointLong p2)
    {
        return (Math.Abs(p1.X - p2.X) + 1) * (Math.Abs(p1.Y - p2.Y) + 1);
    }
}