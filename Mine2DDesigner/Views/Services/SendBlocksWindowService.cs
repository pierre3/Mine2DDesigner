using Microsoft.Win32;
using Mine2DDesigner.Services;
using Mine2DDesigner.ViewModels;
using System;
using System.Windows;

namespace Mine2DDesigner.Views
{
    public class SendBlocksWindowService : IDialogService
    {
        public Type VmType { get; } = typeof(SendBlocksWindowViewModel);
        private readonly Window owner;
        public SendBlocksWindowService(Window owner)
        {
            this.owner = owner;
        }

        public bool? ShowDialog(IDialogViewModel vm)
        {
            var window = new SendBlocksWindow(vm);
            window.Owner = owner;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Topmost= true;
            owner.IsEnabled = false;
            window.Show();
            window.Closed += (_, _) => { owner.IsEnabled = true; };
                
            return true;
        }
    }
}
