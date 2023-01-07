using MinecraftBlockBuilder.Models;
using MinecraftBlockBuilder.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBlockBuilder.ViewModels
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
