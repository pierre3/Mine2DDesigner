namespace Mine2DDesigner.Graphics
{
    public readonly record struct Point3i
    {
        public int X { get; init; }
        public int Y { get; init; }
        public int Z { get; init; }

        public Point3i(int x, int y, int z) => (X, Y, Z) = (x, y, z);

    }
}
