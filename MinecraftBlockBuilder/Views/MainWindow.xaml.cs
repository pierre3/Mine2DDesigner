using MinecraftBlockBuilder.Services;
using MinecraftBlockBuilder.ViewModels;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
        private readonly PaintPlaneView? paintPlaneView;
        public MainWindow()
        {
            InitializeComponent();
            if (DataContext is IPaintPlane paintPlane)
            {
                paintPlaneView = new PaintPlaneView(paintPlane, skElementXZ, skElementZY, skElementXY);
            }
            if (DataContext is Services.IServiceProvider serviceProvider)
            {
                serviceProvider.AddService(new SelectBlockWindowService());
            }
        }

        private void SKElement_PaintSurfaceXZ(object sender, SKPaintSurfaceEventArgs e) => paintPlaneView?.PaintSurfaceXZ(e);

        private void SKElement_PaintSurfaceXY(object sender, SKPaintSurfaceEventArgs e) => paintPlaneView?.PaintSurfaceXY(e);

        private void SKElement_PaintSurfaceZY(object sender, SKPaintSurfaceEventArgs e) => paintPlaneView?.PaintSurfaceZY(e);
    }
}
