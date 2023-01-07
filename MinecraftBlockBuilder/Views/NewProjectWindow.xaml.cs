using MinecraftBlockBuilder.Services;
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

namespace MinecraftBlockBuilder.Views
{
    /// <summary>
    /// NewProjectWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        public NewProjectWindow(IDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
