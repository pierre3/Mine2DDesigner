using Microsoft.Win32;
using MinecraftBlockBuilder.Services;
using MinecraftBlockBuilder.ViewModels;
using System;
using System.Windows;

namespace MinecraftBlockBuilder.Views
{
    public class NewProjectWindowService : IDialogService
    {
        public Type VmType { get; } = typeof(NewProjectWindowViewModel);
        private readonly Window owner;
        public NewProjectWindowService(Window owner)
        {

            this.owner = owner;
        }

        public bool? ShowDialog(IDialogViewModel vm)
        {
            var window = new NewProjectWindow(vm);
            window.Owner = owner;
            return window.ShowDialog();
        }
    }
}
