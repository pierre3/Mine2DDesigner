using Microsoft.Win32;
using Mine2DDesigner.Services;
using Mine2DDesigner.ViewModels;
using System;
using System.Windows;

namespace Mine2DDesigner.Views
{
    public class OpenFileDialogService : IDialogService
    {
        public Type VmType { get; } = typeof(OpenFileDialogViewModel);
        private readonly Window owner;
        public OpenFileDialogService(Window owner)
        {

            this.owner = owner;
        }

        public bool? ShowDialog(IDialogViewModel vm)
        {
            var dialogViewModel = (OpenFileDialogViewModel)vm;
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = dialogViewModel.InitialDirectory,
                CheckFileExists = true,
                CheckPathExists = true,
                ReadOnlyChecked = true,
                Filter = dialogViewModel.Filter
            };
            var ret = dialog.ShowDialog(owner);
            dialogViewModel.FileName = dialog.FileName;
            return ret;
        }
    }
}
