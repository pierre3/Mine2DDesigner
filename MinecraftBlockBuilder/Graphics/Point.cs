using System;

namespace MinecraftBlockBuilder.Graphics
{
    public readonly record struct Point
    {
        public float X { get; init; }
        public float Y { get; init; }

        public Point(float x, float y) => (X, Y) = (x, y);
        
    }
}
