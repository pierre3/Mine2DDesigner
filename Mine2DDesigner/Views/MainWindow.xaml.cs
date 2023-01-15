using Mine2DDesigner.Services;
using Mine2DDesigner.ViewModels;
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

namespace Mine2DDesigner.Views
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
                paintPlaneView = new PaintPlaneView(paintPlane, skElementZX, skElementZY, skElementXY);
            }
            if (DataContext is IDialogServiceProvider serviceProvider)
            {
                serviceProvider.AddService(new SelectBlockWindowService(this));
                serviceProvider.AddService(new NewProjectWindowService(this));
                serviceProvider.AddService(new OpenFileDialogService(this));
                serviceProvider.AddService(new SaveFileDialogService(this));
                serviceProvider.AddService(new SettingsWindowService(this));
                serviceProvider.AddService(new SendBlocksWindowService(this));
            }
        }

        private void SKElement_PaintSurfaceZX(object sender, SKPaintSurfaceEventArgs e) => paintPlaneView?.PaintSurfaceZX(e);

        private void SKElement_PaintSurfaceXY(object sender, SKPaintSurfaceEventArgs e) => paintPlaneView?.PaintSurfaceXY(e);

        private void SKElement_PaintSurfaceZY(object sender, SKPaintSurfaceEventArgs e) => paintPlaneView?.PaintSurfaceZY(e);
    }
}
