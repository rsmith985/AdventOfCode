using System;

namespace rsmith985.AOC.Y2025;

public class Day04 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var grid = this.GetGrid_Char();
        var size = grid.GetSize();
        grid = grid.PadBorder('.');

        return countGrid(grid, size, 4);
    }

    public override object Part2()
    {
        var grid = this.GetGrid_Char();
        var size = grid.GetSize();
        grid = grid.PadBorder('.');

        //grid.PrintToConsole();

        var tot = 0;
        while(true)
        {
            var numChanged = 0;
            var next = grid.Copy_();
            foreach(var p in size.GetPointsInGrid())
            {
                var pt = p.Plus(1);
                if(grid.Get(pt) == '@')
                {
                    var count = countNeighbors8(grid, pt);
                    if(count < 4)
                    {
                        next.Set(pt, '.');
                        numChanged++;
                    }
                }
            }
            tot += numChanged;
            
            if(numChanged == 0)
                break;
            grid = next;
        }

        //grid.PrintToConsole();

        return tot;
    }

    private int countGrid(char[,] grid, Size size, int numNeighbors)
    {
        var tot = 0;
        foreach(var p in size.GetPointsInGrid())
        {
            var pt = p.Plus(1);
            if(grid.Get(pt) == '@')
            {
                var count = countNeighbors8(grid, pt);
                if(count < numNeighbors)
                    tot++;
            }
        }
        return tot;
    }

    private int countNeighbors8(char[,] grid, Point p)
    {
        var count = 0;
        foreach(var dir in Utils.Directions8)
        {
            var n = p.GetNeighbor(dir);
            var c = grid.Get(n);
            if(c == '@')
                count++;
        }
        return count;
    }
}