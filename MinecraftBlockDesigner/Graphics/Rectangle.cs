using System;

namespace MinecraftBlockDesigner.Graphics
{
    public record struct Rectangle
    {
        public Point Location { get; init; }
        public Size Size { get; init; }

        public float Left => Location.X;
        public float Top => Location.Y;
        public float Right => Location.X + Size.Width;
        public float Bottom => Location.Y + Size.Height;
        public float Width => Size.Width;
        public float Height => Size.Height;


        public Rectangle(Point location, Size size)
        {
            Location = location;
            Size = size;
        }

        public Rectangle(float left, float top, float width, float height)
            : this(new Point(left, top), new Size(width, height)) { }

    }
}
