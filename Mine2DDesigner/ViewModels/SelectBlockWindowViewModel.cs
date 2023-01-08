using Mine2DDesigner.Models;
using Mine2DDesigner.Services;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mine2DDesigner.ViewModels
{
    public class SelectBlockWindowViewModel : INotifyPropertyChanged, IDisposable, IDialogViewModel
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        public IReadOnlyList<Block> Blocks { get => Block.Definitions; }

        public ReactivePropertySlim<Block> SelectedBlock { get; }

        public SelectBlockWindowViewModel(Block currentBlock)
        {
            SelectedBlock = new ReactivePropertySlim<Block>(currentBlock);

        }

        public void Dispose()
        {
            SelectedBlock.Dispose();
        }
    }
}
