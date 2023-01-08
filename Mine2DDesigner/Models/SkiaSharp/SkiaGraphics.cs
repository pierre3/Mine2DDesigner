using Mine2DDesigner.Graphics;
using SkiaSharp;
using System.IO;

namespace Mine2DDesigner.Models
{
    public class SkiaGraphics : IGraphics
    {
        protected virtual SKCanvas Canvas { get; set; }

        public float ScaleX { get; }
        public float ScaleY { get; }

        public SkiaGraphics(SKCanvas canvas, float scaleX, float scaleY)
        {
            Canvas = canvas;
            ScaleX = scaleX;
            ScaleY = scaleY;
        }
        public virtual void ClearCanvas(Color color)
        {
            Canvas.Clear(color.ToSk());
        }

        public virtual void DrawRectangle(Rectangle rectangle, Stroke stroke)
        {
            using var paint = new SKPaint().SetStroke(stroke);
            Canvas.DrawRect(rectangle.ToSk(ScaleX, ScaleY), paint);
        }

        public virtual void FillRectangle(Rectangle rectangle, Fill fill)
        {
            using var paint = new SKPaint().SetFill(fill);
            Canvas.DrawRect(rectangle.ToSk(ScaleX, ScaleY), paint);
        }

        public virtual void DrawOval(Rectangle rectangle, Stroke stroke)
        {
            using var paint = new SKPaint().SetStroke(stroke);
            Canvas.DrawOval(rectangle.ToSk(ScaleX, ScaleY), paint);
        }

        public virtual void FillOval(Rectangle rectangle, Fill fill)
        {
            using var paint = new SKPaint().SetFill(fill);
            Canvas.DrawOval(rectangle.ToSk(ScaleX, ScaleY), paint);
        }

        public void DrawLine(Point p1, Point p2, Stroke stroke)
        {
            using var paint = new SKPaint().SetStroke(stroke);
            Canvas.DrawLine(p1.ToSk(ScaleX, ScaleY), p2.ToSk(ScaleX, ScaleY), paint);
        }

        public void DrawImage(Rectangle rect, byte[] bytes)
        {
            var bitmap = SKBitmap.Decode(bytes);
            var image = SKImage.FromBitmap(bitmap);
            Canvas.DrawImage(image, rect.ToSk(ScaleX, ScaleY));
        }
    }
}
