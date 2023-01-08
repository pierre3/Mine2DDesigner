using Mine2DDesigner.Models;
using Mine2DDesigner.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System.Windows.Media;

namespace Mine2DDesigner.Views
{
    public class PaintPlaneView
    {
        private readonly IPaintPlane paintPlane;
        private readonly SKElement skElementZX;
        private readonly SKElement skElementZY;
        private readonly SKElement skElementXY;

        public PaintPlaneView(IPaintPlane paintPlane, SKElement skElementZX, SKElement skElementZY, SKElement skElementXY)
        {
            this.paintPlane = paintPlane;
            this.skElementXY = skElementXY;
            this.skElementZX = skElementZX;
            this.skElementZY = skElementZY;
            this.paintPlane.UpdateSuface += () =>
            {
                skElementZX.InvalidateVisual();
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

        public void PaintSurfaceZX(SKPaintSurfaceEventArgs e)
        {
            ClearCanvas(e);
            var g = GetGraphics(skElementZX, e.Surface.Canvas);
            paintPlane?.PaintZX(g);
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
