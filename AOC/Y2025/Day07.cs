using System;

namespace rsmith985.AOC.Y2025;

public class Day07 : Day
{
    //protected override bool _useDefaultTestFile => true;

    public override object Part1()
    {
        var grid = this.GetGrid_Char();
        var size = grid.GetSize();

        for(int x = 0; x < size.Width; x++)
        {
            if(grid[x,0] == 'S')
                grid[x,0] = '|';
        }

        var splits = 0;
        for(int y = 0; y < size.Height - 1; y++)
        {
            for(int x = 0; x < size.Width; x++)
            {
                if(grid[x,y] == '|')
                {
                    if(grid[x,y+1] == '^')
                    {
                        grid[x-1,y+1] = '|';
                        grid[x+1,y+1] = '|';
                        splits++;
                    }
                    else
                    {
                        grid[x,y+1] = '|';
                    }
                }
            }
        }

        //grid.PrintToConsole();
        return splits;
    }

    public override object Part2()
    {
        var grid = this.GetGrid_Char();
        var size = grid.GetSize();
        var counts = new long[size.Width, size.Height];

        for(int x = 0; x < size.Width; x++)
        {
            if(grid[x,0] == 'S')
            {
                grid[x,0] = '|';
                counts[x,0] = 1;
            }
        }

        var splits = 0;
        for(int y = 0; y < size.Height - 1; y++)
        {
            for(int x = 0; x < size.Width; x++)
            {
                if(grid[x,y] == '|')
                {
                    var num = counts[x,y];
                    if(grid[x,y+1] == '^')
                    {
                        grid[x-1,y+1] = '|';
                        grid[x+1,y+1] = '|';
                        counts[x-1,y+1] += num;
                        counts[x+1,y+1] += num;
                        splits++;
                    }
                    else
                    {
                        grid[x,y+1] = '|';
                        counts[x,y+1] += num;
                    }
                }
            }
        }   
        
        long tot = 0;
        for(int x = 0; x < size.Width; x++)
            tot += counts[x, size.Height -1];

        //grid.PrintToConsole();
        //for(int x = 0; x < size.Width; x++)
        //    Console.WriteLine(counts[x, size.Height -1]);
        return tot;
    }
}