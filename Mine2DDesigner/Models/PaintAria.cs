using Mine2DDesigner.Graphics;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Mine2DDesigner.Models
{
    public class PaintAria
    {
        public Point3i Start { get; set; }
        public Point3i End { get; set; }
        public PaintMode PaintMode { get; set; } = PaintMode.None;
        public FillMode FillMode { get; set; } = FillMode.Fill;

        public IList<Point3i> GetPaintArea()
        {
            switch (PaintMode)
            {
                case PaintMode.Cube:
                    return (FillMode == FillMode.Fill)
                        ? GetFillCube().ToList()
                        : GetSurfaceCube().ToList();
                case PaintMode.Ball:
                    return GetBallAria().ToList();
                default:
                    return new List<Point3i>();
            }
        }
        public IEnumerable<Point3i> GetBallAria()
        {
            var p0 = new Point3i(
                Math.Min(Start.X, End.X),
                Math.Min(Start.Y, End.Y),
                Math.Min(Start.Z, End.Z));
            var p1 = new Point3i(
                Math.Max(Start.X, End.X),
                Math.Max(Start.Y, End.Y),
                Math.Max(Start.Z, End.Z));

            var rx = (p1.X - p0.X) / 2.0;
            var ry = (p1.Y - p0.Y) / 2.0;
            var rz = (p1.Z - p0.Z) / 2.0;
            var (cx, cy, cz) = (p0.X + rx, p0.Y + ry, p0.Z + rz);
  
            for (var y = p0.Y; y <= p1.Y; y++)
            {
                for (var x = p0.X; x <= p1.X; x++)
                {
                    for (var z = p0.Z; z <= p1.Z; z++)
                    {
                        var tmpX = (p1.X - p0.X) == 0 ? 0.0 : (x - cx) * (x - cx) / (rx * rx);
                        var tmpY = (p1.Y - p0.Y) == 0 ? 0.0 : (y - cy) * (y - cy) / (ry * ry);
                        var tmpZ = (p1.Z - p0.Z) == 0 ? 0.0 : (z - cz) * (z - cz) / (rz * rz);
                        var a = tmpX + tmpY + tmpZ;
                        if (FillMode == FillMode.Fill && a <= 1
                            || FillMode == FillMode.Surface && a > 1 - 0.25 && a <= 1)
                        {
                            yield return new Point3i(x, y, z);
                        }
                    }
                }
            }
        }

        public IEnumerable<Point3i> GetFillCube()
        {
            var p0 = new Point3i(
                Math.Min(Start.X, End.X),
                Math.Min(Start.Y, End.Y),
                Math.Min(Start.Z, End.Z));
            var p1 = new Point3i(
                Math.Max(Start.X, End.X),
                Math.Max(Start.Y, End.Y),
                Math.Max(Start.Z, End.Z));
            for (var y = p0.Y; y <= p1.Y; y++)
            {
                for (var x = p0.X; x <= p1.X; x++)
                {
                    for (var z = p0.Z; z <= p1.Z; z++)
                    {
                        yield return new Point3i(x, y, z);
                    }
                }
            }
        }
        public IEnumerable<Point3i> GetSurfaceCube()
        {
            var p0 = new Point3i(
                Math.Min(Start.X, End.X),
                Math.Min(Start.Y, End.Y),
                Math.Min(Start.Z, End.Z));
            var p1 = new Point3i(
                Math.Max(Start.X, End.X),
                Math.Max(Start.Y, End.Y),
                Math.Max(Start.Z, End.Z));
            //Y-0 Plane
            for (var x = p0.X; x <= p1.X; x++)
            {
                for (var z = p0.Z; z <= p1.Z; z++)
                {
                    yield return new Point3i(x, p0.Y, z);
                }
            }
            //Y-1 Plane
            for (var x = p0.X; x <= p1.X; x++)
            {
                for (var z = p0.Z; z <= p1.Z; z++)
                {
                    yield return new Point3i(x, p1.Y, z);
                }
            }
            //X-0 Plane
            for (var y = p0.Y + 1; y < p1.Y; y++)
            {
                for (var z = p0.Z; z <= p1.Z; z++)
                {
                    yield return new Point3i(p0.X, y, z);
                }
            }
            //X-1 Plane
            for (var y = p0.Y + 1; y < p1.Y; y++)
            {
                for (var z = p0.Z; z <= p1.Z; z++)
                {
                    yield return new Point3i(p1.X, y, z);
                }
            }
            //Z-0 Plane
            for (var y = p0.Y + 1; y < p1.Y; y++)
            {
                for (var x = p0.X + 1; x < p1.X; x++)
                {
                    yield return new Point3i(x, y, p0.Z);
                }
            }
            //Z-1 Plane
            for (var y = p0.Y + 1; y < p1.Y; y++)
            {
                for (var x = p0.X + 1; x < p1.X; x++)
                {
                    yield return new Point3i(x, y, p1.Z);
                }
            }
        }
    }

    public enum PaintMode
    {
        None,
        Cube,
        Ball
    }

    public enum FillMode
    {
        Fill,
        Surface
    }
}
