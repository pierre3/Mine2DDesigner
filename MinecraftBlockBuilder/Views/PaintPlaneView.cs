using MinecraftBlockBuilder.Models;
using MinecraftBlockBuilder.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System.Windows.Media;

namespace MinecraftBlockBuilder.Views
{
    public class PaintPlaneView
    {
        private readonly IPaintPlane paintPlane;
        private readonly SKElement skElementXZ;
        private readonly SKElement skElementZY;
        private readonly SKElement skElementXY;

        public PaintPlaneView(IPaintPlane paintPlane, SKElement skElementXZ, SKElement skElementZY, SKElement skElementXY)
        {
            this.paintPlane = paintPlane;
            this.skElementXY = skElementXY;
            this.skElementXZ = skElementXZ;
            this.skElementZY = skElementZY;
            this.paintPlane.UpdateSuface += () =>
            {
                skElementXZ.InvalidateVisual();
                skElementXY.InvalidateVisual();
                skElementZY.InvalidateVisual();
            };
        }

        private static SkiaGraphics GetGraphics(Visual visual, SKCanvas canvas)
        {
            var dpi = VisualTreeHelper.GetDpi(visual);
            return new SkiaGraphics(canvas, (float)dpi.DpiScaleX, (float)dpi.DpiScaleY);
        }

        private static void ClearCanvas(SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.DrawRect(e.Info.Rect, new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Ivory
            });
        }

        public void PaintSurfaceXZ(SKPaintSurfaceEventArgs e)
        {
            ClearCanvas(e);
            var g = GetGraphics(skElementXZ, e.Surface.Canvas);
            paintPlane?.PaintXZ(g);
        }

        public void PaintSurfaceXY(SKPaintSurfaceEventArgs e)
        {
            ClearCanvas(e);
            var g = GetGraphics(skElementXY, e.Surface.Canvas);
            paintPlane?.PaintXY(g);
        }

        public void PaintSurfaceZY(SKPaintSurfaceEventArgs e)
        {
            ClearCanvas(e);
            var g = GetGraphics(skElementZY, e.Surface.Canvas);
            paintPlane?.PaintZY(g);
        }
    }
}
