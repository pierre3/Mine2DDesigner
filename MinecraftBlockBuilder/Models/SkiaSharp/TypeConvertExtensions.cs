using MinecraftBlockBuilder.Graphics;
using SkiaSharp;

namespace MinecraftBlockBuilder.Models
{
    public static class TypeConvertExtensions
    {
        public static SKPoint ToSk(this Point p, float scaleX = 1f, float scaleY = 1f)
            => new(p.X * scaleX, p.Y * scaleY);
        public static SKSize ToSk(this Size s, float scaleX = 1f, float scaleY = 1f)
            => new(s.Width * scaleX, s.Height * scaleY);
        public static SKRect ToSk(this Rectangle rect, float scaleX = 1f, float scaleY = 1f)
            => new(rect.Left * scaleX, rect.Top * scaleY, rect.Right * scaleX, rect.Bottom * scaleY);
        public static SKColor ToSk(this Color color)
            => new(color.R, color.G, color.B, color.A);
    }
}
