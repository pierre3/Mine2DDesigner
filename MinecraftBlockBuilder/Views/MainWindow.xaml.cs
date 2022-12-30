using MinecraftBlockBuilder.Models;
using MinecraftBlockBuilder.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
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

namespace MinecraftBlockBuilder.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPaintPlane? PaintPlane { get => DataContext as IPaintPlane; }
        public MainWindow()
        {
            InitializeComponent();

            if (PaintPlane is not null)
            {
                PaintPlane.UpdateSuface += () =>
                {
                    skElementXZ.InvalidateVisual();
                    skElementXY.InvalidateVisual();
                    skElementZY.InvalidateVisual();
                };
            }
        }

        private void SKElement_PaintSurfaceXZ(object sender, SKPaintSurfaceEventArgs e)
        {
            ClearCanvas(e);
            var g = GetGraphics((Visual)sender, e.Surface.Canvas);
            PaintPlane?.PaintXZ(g);
        }

        private void SKElement_PaintSurfaceXY(object sender, SKPaintSurfaceEventArgs e)
        {
            ClearCanvas(e);
            var g = GetGraphics((Visual)sender, e.Surface.Canvas);
            PaintPlane?.PaintXY(g);
        }

        private void SKElement_PaintSurfaceZY(object sender, SKPaintSurfaceEventArgs e)
        {
            ClearCanvas(e);
            var g = GetGraphics((Visual)sender, e.Surface.Canvas);
            PaintPlane?.PaintZY(g);
        }

        private static SkiaGraphics GetGraphics(Visual sender, SKCanvas canvas)
        {
            var dpi = VisualTreeHelper.GetDpi(sender);
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
    }
}
