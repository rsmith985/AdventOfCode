
using System.Linq.Expressions;
using PointT = (long X, long Y);
using Line = (System.Drawing.Point p1, System.Drawing.Point p2);
using LineTupleLong = ((long X, long Y) p1, (long X, long Y) p2);

namespace rsmith985.AOC;

public static class GeometryExt
{
    #region Convert Point/Size
    public static Point ToPoint(this Size s) => new Point(s.Width, s.Height);
    public static PointF ToPoint(this SizeF s) => new PointF(s.Width, s.Height);
    public static Size ToSize(this Point s) => new Size(s.X, s.Y);
    public static SizeF ToSize(this PointF s) => new SizeF(s.X, s.Y);
    #endregion

    #region Point
    public static Point Plus(this Point p, int num) => new Point(p.X+num, p.Y+num);
    public static Point Plus(this Point p, int x, int y) => new Point(p.X+x, p.Y+y);
    public static Point Plus(this Point p, Point p2) => new Point(p.X+p2.X, p.Y+p2.Y);
    public static Point Minus(this Point p, int num) => new Point(p.X-num, p.Y-num);
    public static Point Minus(this Point p, int x, int y) => new Point(p.X-x, p.Y-y);
    public static Point Minus(this Point p, Point p2) => new Point(p.X-p2.X, p.Y-p2.Y);
    public static Point Mult(this Point p, int num) => new Point(p.X*num, p.Y*num);
    public static Point Mult(this Point p, Point p2) => new Point(p.X*p2.X, p.Y*p2.Y);
    public static Point Divide(this Point p, int num) => new Point(p.X/num, p.Y/num);
    public static Point Divide(this Point p, Point p2) => new Point(p.X/p2.X, p.Y/p2.Y);
    public static Point Invert(this Point p) => p.Mult(-1);

    public static PointF Plus(this PointF p, float num) => new PointF(p.X+num, p.Y+num);
    public static PointF Plus(this PointF p, float x, float y) => new PointF(p.X+x, p.Y+y);
    public static PointF Plus(this PointF p, PointF p2) => new PointF(p.X+p2.X, p.Y+p2.Y);
    public static PointF Mult(this PointF p, float num) => new PointF(p.X*num, p.Y*num);
    public static PointF Mult(this PointF p, PointF p2) => new PointF(p.X*p2.X, p.Y*p2.Y);
    public static PointF Divide(this PointF p, float num) => new PointF(p.X/num, p.Y/num);
    public static PointF Divide(this PointF p, PointF p2) => new PointF(p.X/p.X, p.Y/p.Y);
    public static PointF Invert(this PointF p) => p.Mult(-1);

    public static PointF ToFloat(this Point p) => new PointF(p.X, p.Y);
    public static Point ToNonFloat(this PointF p) => new Point((int)Math.Round(p.X), (int)Math.Round(p.Y));

    public static (int x, int y) ToTuple(this Point p) => (p.X, p.Y);
    public static (float x, float y) ToTuple(this PointF p) => (p.X, p.Y);
    public static Point ToPoint(this (int x, int y) p) => new Point(p.x, p.y);
    public static PointF ToPoint(this (float x, float y) p) => new PointF(p.x, p.y);

    public static Point Min(this Point p1, Point p2) => new Point(p1.X < p2.X ? p1.X : p2.X, p1.Y < p2.Y ? p1.Y : p2.Y);
    public static Point Max(this Point p1, Point p2) => new Point(p1.X > p2.X ? p1.X : p2.X, p1.Y > p2.Y ? p1.Y : p2.Y);
    public static PointF Min(this PointF p1, PointF p2) => new PointF(p1.X < p2.X ? p1.X : p2.X, p1.Y < p2.Y ? p1.Y : p2.Y);
    public static PointF Max(this PointF p1, PointF p2) => new PointF(p1.X > p2.X ? p1.X : p2.X, p1.Y > p2.Y ? p1.Y : p2.Y);
    #endregion

    #region Size
    public static Size Plus(this Size size, int num) => new Size(size.Width+num, size.Height+num);
    public static Size Plus(this Size size, Size s) => new Size(size.Width+s.Width, size.Height+s.Height);
    public static Size Mult(this Size size, int num) => new Size(size.Width*num, size.Height*num);
    public static Size Mult(this Size size, Size s) => new Size(size.Width*s.Width, size.Height*s.Height);
    public static Size Divide(this Size p, int num) => new Size(p.Width/num, p.Height/num);
    public static Size Divide(this Size p, Size p2) => new Size(p.Width/p.Width, p.Height/p.Height);
    public static Size Invert(this Size p) => p.Mult(-1);

    public static SizeF Plus(this SizeF size, float num) => new SizeF(size.Width+num, size.Height+num);
    public static SizeF Plus(this SizeF size, SizeF s) => new SizeF(size.Width+s.Width, size.Height+s.Height);
    public static SizeF Mult(this SizeF size, float num) => new SizeF(size.Width*num, size.Height*num);
    public static SizeF Mult(this SizeF size, SizeF s) => new SizeF(size.Width*s.Width, size.Height*s.Height);
    public static SizeF Divide(this SizeF p, float num) => new SizeF(p.Width/num, p.Height/num);
    public static SizeF Divide(this SizeF p, SizeF p2) => new SizeF(p.Width/p.Width, p.Height/p.Height);
    public static SizeF Invert(this SizeF p) => p.Mult(-1);

    public static SizeF ToFloat(this Size p) => new SizeF(p.Width, p.Height);
    public static Size ToNonFloat(this SizeF p) => new Size((int)Math.Round(p.Width), (int)Math.Round(p.Height));
    #endregion

    public static double DistanceTo(this Point p1, Point p2)
        => Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
    public static int DistanceTo_CityBlock(this Point p1, Point p2)
        => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

    #region  Polygon
    public static bool PolygonContains(this IList<PointF> poly, PointF p)
    {
        var rv = false;
        if(poly.Count <= 2) return false;

        var prev = poly[poly.Count - 2];
        var curr = poly[poly.Count - 1];

        for(int i = 0; i < poly.Count; i++)
        {
            var p1 = curr.X > prev.X ? prev : curr;
            var p2 = curr.X > prev.X ? curr : prev;

            if((curr.X < p.X) == (p.X <= prev.X) && (p.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (p.X - p1.X))
                rv = !rv;
            
            prev = curr;
            curr = poly[i];
        }

        return rv;
    }
    public static bool PolygonContains(this IList<Point> poly, Point p)
    {
        var rv = false;
        if(poly.Count <= 2) return false;

        var prev = poly[poly.Count - 2];
        var curr = poly[poly.Count - 1];

        for(int i = 0; i < poly.Count; i++)
        {
            var p1 = curr.X > prev.X ? prev : curr;
            var p2 = curr.X > prev.X ? curr : prev;

            if((curr.X < p.X) == (p.X <= prev.X) && (p.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (p.X - p1.X))
                rv = !rv;
            
            prev = curr;
            curr = poly[i];
        }

        return rv;
    }
    public static bool PolygonContains(this IList<(long X, long Y)> poly, (long X, long Y) p)
    {
        var rv = false;
        if(poly.Count <= 2) return false;

        var prev = poly[poly.Count - 2];
        var curr = poly[poly.Count - 1];

        for(int i = 0; i < poly.Count; i++)
        {
            var p1 = curr.X > prev.X ? prev : curr;
            var p2 = curr.X > prev.X ? curr : prev;

            if((curr.X < p.X) == (p.X <= prev.X) && (p.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (p.X - p1.X))
                rv = !rv;
            
            prev = curr;
            curr = poly[i];
        }

        return rv;
    }

    public static long PolygonArea(this IList<Point> points)
    {
        if(points.Count <= 2) return 0;

        var tot = polyAreaAdd(points[^1], points[0]);
        for(int i = 1; i < points.Count; i++)
            tot += polyAreaAdd(points[i-1], points[i]);

        return Math.Abs(tot) / 2;

        long polyAreaAdd(Point p1, Point p2) => p1.X * p2.Y - p2.X * p1.Y;
    }
    public static long PolygonArea(this IList<(long X, long Y)> points)
    {
        if(points.Count <= 2) return 0;

        var tot = polyAreaAdd(points[^1], points[0]);
        for(int i = 1; i < points.Count; i++)
            tot += polyAreaAdd(points[i-1], points[i]);

        return Math.Abs(tot) / 2;

        long polyAreaAdd((long X, long Y) p1, (long X, long Y) p2) => p1.X * p2.Y - p2.X * p1.Y;
    }

    public static double PolygonPerimeter(this IList<Point> points)
    {
        var tot = points[^1].DistanceTo(points[0]);
        for(int i = 1; i < points.Count; i++)
            tot += points[i-1].DistanceTo(points[i]);
        return tot;
    }
    #endregion

    #region Bounding Rectangle
    public static RectangleF GetBoundingRect(this IEnumerable<PointF> points)
    {
        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;

        foreach(var p in points)
        {
            if(p.X < minX) minX = p.X;
            if(p.X > maxX) maxX = p.X;
            if(p.Y < minY) minY = p.Y;
            if(p.Y > maxY) maxY = p.Y;
        }
        return new RectangleF(minX, minY, maxX-minX, maxY-minY);
    }
    public static Rectangle GetBoundingRect(this IEnumerable<Point> points)
    {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;

        foreach(var p in points)
        {
            if(p.X < minX) minX = p.X;
            if(p.X > maxX) maxX = p.X;
            if(p.Y < minY) minY = p.Y;
            if(p.Y > maxY) maxY = p.Y;
        }
        return new Rectangle(minX, minY, maxX-minX, maxY-minY);
    }
    #endregion


    #region Lines
    public static bool LinesIntersect(this LineTupleLong line1, LineTupleLong line2, bool includeCollinear = false)
    {
        // Find the four orientations needed for general and special cases
        int o1 = orientation(line1, line2.p1);
        int o2 = orientation(line1, line2.p2);
        int o3 = orientation(line2, line1.p1);
        int o4 = orientation(line2, line1.p2);

        // **General Case**
        // The segments intersect if and only if the orientations are different 
        // for each line with respect to the other line's endpoints.
        if (o1 != o2 && o3 != o4)
            return true;

        if(includeCollinear)
        {
            // **Special Cases (Collinear Points)**:
            if (o1 == 0 && onSegment(line1.p1, line2.p1, line1.p2)) return true;
            if (o2 == 0 && onSegment(line1.p1, line2.p2, line1.p2)) return true;
            if (o3 == 0 && onSegment(line2.p1, line1.p1, line2.p2)) return true;
            if (o4 == 0 && onSegment(line2.p1, line1.p2, line2.p2)) return true;
        }
        return false; // Doesn't fall into any of the above cases

        
        int orientation(LineTupleLong line, PointT r)
        {
            var val = (line.p2.Y - line.p1.Y) * (r.X - line.p2.X) - 
                        (line.p2.X - line.p1.X) * (r.Y - line.p2.Y);

            if (val == 0) return 0;  // Collinear
            return (val > 0) ? 1 : 2; // Clockwise or Counterclockwise
        }
        bool onSegment(PointT p, PointT q, PointT r)
        {
            return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
        }
    }
    /// <summary>
    /// Calculates the intersection point of two lines defined by their respective endpoints.
    /// </summary>
    /// <returns>The Point of intersection, or null if the lines are parallel.</returns>
    public static Point? LinesIntersectPoint(this Line line1, Line line2)
    {
        var l1 = CreateLine(line1);
        var l2 = CreateLine(line2);

        // Determinant (D) = A1*B2 - A2*B1
        double det = l1.A * l2.B - l2.A * l1.B;

        // Use a small epsilon for floating-point comparison
        double epsilon = 1e-9; 

        if (Math.Abs(det) < epsilon)
        {
            // The lines are parallel (or collinear/coincident)
            return null;
        }

        // Calculate X coordinate
        // X = (C1*B2 - C2*B1) / D
        var x = (int)((l1.C * l2.B - l2.C * l1.B) / det);

        // Calculate Y coordinate
        // Y = (A1*C2 - A2*C1) / D
        var y = (int)((l1.A * l2.C - l2.A * l1.C) / det);

        return new Point(x, y);

        LineCoefficients CreateLine(Line line)
        {
            var rv = new LineCoefficients();
            
            // A = Y2 - Y1
            rv.A = line.p2.Y - line.p1.Y;
            
            // B = X1 - X2
            rv.B = line.p1.X - line.p2.X;
            
            // C = A*X1 + B*Y1
            rv.C = rv.A * line.p1.X + rv.B * line.p1.Y;

            return rv;
        }
    }
    /// <summary>
    /// Calculates the intersection point of two lines defined by their respective endpoints.
    /// </summary>
    /// <returns>The Point of intersection, or null if the lines are parallel.</returns>
    public static PointT? LinesIntersectPoint(this LineTupleLong line1, LineTupleLong line2)
    {
        var l1 = CreateLine(line1);
        var l2 = CreateLine(line2);

        // Determinant (D) = A1*B2 - A2*B1
        double det = l1.A * l2.B - l2.A * l1.B;

        // Use a small epsilon for floating-point comparison
        double epsilon = 1e-9; 

        if (Math.Abs(det) < epsilon)
        {
            // The lines are parallel (or collinear/coincident)
            return null;
        }

        // Calculate X coordinate
        // X = (C1*B2 - C2*B1) / D
        var x = (long)((l1.C * l2.B - l2.C * l1.B) / det);

        // Calculate Y coordinate
        // Y = (A1*C2 - A2*C1) / D
        var y = (long)((l1.A * l2.C - l2.A * l1.C) / det);

        return (x, y);

        LineCoefficients CreateLine(LineTupleLong line)
        {
            var rv = new LineCoefficients();
            
            // A = Y2 - Y1
            rv.A = line.p2.Y - line.p1.Y;
            
            // B = X1 - X2
            rv.B = line.p1.X - line.p2.X;
            
            // C = A*X1 + B*Y1
            rv.C = rv.A * line.p1.X + rv.B * line.p1.Y;

            return rv;
        }
    }
    struct LineCoefficients
    {
        public double A, B, C;
    }/// <summary>
    /// Checks if point Q lies on the finite LINE SEGMENT PR.
    /// This requires Q to be collinear AND within the bounding box of P and R.
    /// </summary>
    public static bool IsPointOnLineSegment(LineTupleLong line, PointT r)
    {
        // 1. Check for collinearity (Q must be on the line)
        if (!IsPointOnLine(line, r))
        {
            return false;
        }
        
        // 2. Check if Q is within the bounding box of the segment PR
        // This is necessary because the first check only confirms Q is on the infinite line.
        return (line.p2.X <= Math.Max(line.p1.X, r.X) && line.p2.X >= Math.Min(line.p1.X, r.X) &&
                line.p2.Y <= Math.Max(line.p1.Y, r.Y) && line.p2.Y >= Math.Min(line.p1.Y, r.Y));
    }
    /// <summary>
    /// Checks if point Q lies on the infinite line passing through P and R.
    /// </summary>
    /// <returns>True if the points are collinear, false otherwise.</returns>
    public static bool IsPointOnLine(LineTupleLong line, PointT p)
    {
        // Calculate the cross-product component (or determinant value)
        // This value determines the orientation of the triplet (p, q, r).
        double val = (line.p2.Y - line.p1.Y) * (p.X - line.p2.X) - (line.p2.X - line.p1.X) * (p.Y - line.p2.Y);

        // We use a small epsilon for floating-point comparisons to handle
        // precision errors.
        const double Epsilon = 1e-9; 

        // If the value is close to zero, the points are collinear.
        if (Math.Abs(val) < Epsilon)
        {
            return true;
        }

        // The points are not collinear.
        return false;
    }
    #endregion
}
