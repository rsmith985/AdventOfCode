using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace rsmith985.AOC.Y2024;

public class Day21 : Day
{
    //protected override bool _useDefaultTestFile => true;

    private Dictionary<char, Point> _numDict = new Dictionary<char, Point>
    {
        {'7', new Point(0,0)},{'8', new Point(1,0)},{'9', new Point(2,0)},
        {'4', new Point(0,1)},{'5', new Point(1,1)},{'6', new Point(2,1)},
        {'1', new Point(0,2)},{'2', new Point(1,2)},{'3', new Point(2,2)},
                              {'0', new Point(1,3)},{'A', new Point(2,3)}
    };
    private Dictionary<char, Point> _dirDict = new Dictionary<char, Point>
    {
                              {'^', new Point(1,0)},{'A', new Point(2,0)},
        {'<', new Point(0,1)},{'v', new Point(1,1)},{'>', new Point(2,1)}
    };

    private Keypad _numpad;
    private Keypad _dirpad;


    private Dictionary<(string, int), long> _cache;
    


    public override object Part1()
    {
        _numpad = new Keypad(_numDict, new Point(0,3));
        _dirpad = new Keypad(_dirDict, new Point(0,0));

        _cache = new Dictionary<(string, int), long>();

        var tot = 0L;
        foreach(var line in this.GetLines())
        {
            var count = getCount(line, _numpad, 2);
            var num = long.Parse(line[..3]);
            //Console.WriteLine(count + " " + num);
            tot += count*num;
        }
        
        return tot;
    }

    public override object Part2()
    {
        _numpad = new Keypad(_numDict, new Point(0,3));
        _dirpad = new Keypad(_dirDict, new Point(0,0));

        _cache = new Dictionary<(string, int), long>();
        
        var tot = 0L;
        foreach(var line in this.GetLines())
        {
            var count = getCount(line, _numpad, 25);
            var num = long.Parse(line[..3]);
            //Console.WriteLine(count + " " + num);
            tot += count*num;
        }
        
        return tot;
    }


    private long getCount(string line, Keypad keypad, int level)
    {
        line = 'A' + line;
        //Console.WriteLine(line);

        var next = new List<string>();
        foreach(var pair in line.GetAdjacentPairs())
        {
            var seq = keypad.GetSequence(pair) + "A";
            next.Add(seq);
        }

        if(level == 0)
            return next.Sum(i => i.Length);
        
        var sum = 0L;
        foreach(var i in next)
        {
            var key = (i, level);
            if(_cache.ContainsKey(key))
                sum += _cache[key];
            else
            {
                var count = getCount(i, _dirpad, level - 1);
                _cache.Add(key, count);
                sum += count;
            }
        }
        return sum;
    }

    private List<string> parseInput()
    {
        var rv = new List<string>();
        foreach(var line in this.GetLines())
        {
            rv.Add(line[..3]);
        }
        return rv;
    }

    class Keypad
    {
        private Dictionary<char, Point> _dict;
        private Point _hole;

        private HashSet<Point> _validLocs;


        // In order based on shortest distance for robots to do a sequence from a
        private static readonly Direction[] _dirs = [Direction.W, Direction.S, Direction.N, Direction.E];
        private static readonly Dictionary<Direction, char> _dirChars = new Dictionary<Direction, char>()
        {
            {Direction.N, '^'}, {Direction.S, 'v'}, {Direction.E, '>'}, {Direction.W, '<'} 
        };

        public Keypad(Dictionary<char, Point> locs, Point hole)
        {
            _dict = locs;
            _hole = hole;
            _validLocs = new HashSet<Point>(_dict.Values);
        }
        public string GetSequence((char from, char to) key)
        {
            var rv = new StringBuilder();

            var p1 = _dict[key.from];
            var p2 = _dict[key.to];

            var delta = p2.Minus(p1);

            if(delta == Point.Empty) return "";

            var curr = p1;
            var dirIdx = 0;
            while(true)
            {
                var dir = _dirs[dirIdx++%4];
                var move = dir.ToPoint();
                var amt = move.X == 0 ? delta.Y / move.Y : delta.X / move.X;

                if(amt <= 0)
                    continue;

                var next = curr.GetNeighbor(dir, amt);
                if(next == _hole || !_validLocs.Contains(next))
                    continue;

                for(int i = 0; i < amt; i++)
                    rv.Append(_dirChars[dir]);

                if(next == p2)
                    break;

                curr = next;
            }
            return rv.ToString();
        }

    }
}
