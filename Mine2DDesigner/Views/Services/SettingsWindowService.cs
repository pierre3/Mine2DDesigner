using Microsoft.Win32;
using Mine2DDesigner.Services;
using Mine2DDesigner.ViewModels;
using System;
using System.Windows;

namespace Mine2DDesigner.Views
{
    public class SettingsWindowService : IDialogService
    {
        public Type VmType { get; } = typeof(SettingsWindowViewModel);
        private readonly Window owner;
        public SettingsWindowService(Window owner)
        {
            this.owner = owner;
        }

        public bool? ShowDialog(IDialogViewModel vm)
        {
            var window = new SettingsWindow(vm);
            window.Owner = owner;
            return window.ShowDialog();
        }
    }
}
