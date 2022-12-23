using System;
namespace MinecraftBlockBuilder.Graphics
{
    public readonly record struct Size
    {
        public float Width { get; init; }
        public float Height { get; init; }

        public Size(float width, float height) => (Width, Height) = (width, height);
    }
}
