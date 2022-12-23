using SkiaSharp;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftBlockBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SKElement_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.DrawRect(e.Info.Rect, new SKPaint() { Style = SKPaintStyle.Fill, Color = SKColors.Ivory });
            var skElement = (sender as SKElement)!;
            var dpi = VisualTreeHelper.GetDpi(skElement);

            var currentX = 10;
            var currentY = 10;

            for (int y = 0; y < skElement.Height / 16; y++)
            {
                var color = y == currentY ? SKColors.LightBlue : SKColors.LightGray;
                var width = y == currentY ? 4 : 1;

                e.Surface.Canvas.DrawLine(
                    new SKPoint(0, (float)(y * 16 * dpi.DpiScaleY)),
                    new SKPoint((float)(e.Info.Width * dpi.DpiScaleX), (float)(y * 16 * dpi.DpiScaleY)),
                    new SKPaint() { Color = color, StrokeWidth = width });
                e.Surface.Canvas.DrawLine(
                    new SKPoint(0, (float)((y * 16 + 15) * dpi.DpiScaleY)),
                    new SKPoint((float)(e.Info.Width * dpi.DpiScaleX), (float)((y * 16 + 15) * dpi.DpiScaleY)),
                    new SKPaint() { Color = color, StrokeWidth = width });
            }
            for (int x = 0; x < skElement.Width / 16; x++)
            {
                var color = x == currentX ? SKColors.LightBlue : SKColors.LightGray;
                var width = x == currentX ? 4 : 1;
                e.Surface.Canvas.DrawLine(
                    new SKPoint((float)(x * 16 * dpi.DpiScaleX), 0),
                    new SKPoint((float)(x * 16 * dpi.DpiScaleX), (float)(e.Info.Height * dpi.DpiScaleY)),
                    new SKPaint() { Color = color, StrokeWidth = width });
                e.Surface.Canvas.DrawLine(
                    new SKPoint((float)((x * 16 + 15) * dpi.DpiScaleX), 0),
                    new SKPoint((float)((x * 16 + 15) * dpi.DpiScaleX), (float)(e.Info.Height * dpi.DpiScaleY)),
                    new SKPaint() { Color = color, StrokeWidth = width });
            }
        }
    }
}
