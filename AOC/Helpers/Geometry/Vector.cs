using System;
using System.Diagnostics.CodeAnalysis;

namespace rsmith985.AOC;


public struct Vector3
{
    public int X{get;set;}
    public int Y{get;set;}
    public int Z{get;set;}

    public Vector3(int x, int y, int z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    public Vector3(string str)
    {
        var parts = str.Split(',', StringSplitOptions.TrimEntries);
        this.X = int.Parse(parts[0]);
        this.Y = int.Parse(parts[1]);
        this.Z = int.Parse(parts[2]);
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is Vector3 vector && this.X == vector.X && this.Y == vector.Y && this.Z == vector.Z;
    }
    public override int GetHashCode()
    {
        return (this.X, this.Y, this.Z).GetHashCode();
    }
    public override string ToString()
    {
        return $"({this.X}, {this.Y}, {this.Z})";
    }
}