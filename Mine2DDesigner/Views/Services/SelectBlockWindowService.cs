using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mine2DDesigner.Services;
using Mine2DDesigner.ViewModels;

namespace Mine2DDesigner.Views
{
    public class SelectBlockWindowService : IDialogService
    {
        public Type VmType { get; } = typeof(SelectBlockWindowViewModel);

        private readonly Window owner;
        public SelectBlockWindowService(Window owner)
        {
            this.owner = owner;
        }

        public bool? ShowDialog(IDialogViewModel vm)
        {
            var window = new SelectBlockWindow(vm);
            window.Owner = owner;
            return window.ShowDialog();
        }
    }
}
