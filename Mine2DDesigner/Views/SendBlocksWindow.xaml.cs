using Mine2DDesigner.Services;
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
using System.Windows.Shapes;

namespace Mine2DDesigner.Views
{
    /// <summary>
    /// SendBlocskWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SendBlocksWindow : Window
    {
        public SendBlocksWindow(IDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
