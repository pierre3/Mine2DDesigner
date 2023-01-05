using MinecraftBlockBuilder.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBlockBuilder.ViewModels
{
    public class SelectBlockWindowViewModel
    {
        public IReadOnlyList<Block> Blocks { get => Block.Definitions; }

        public ReactivePropertySlim<Block> SelectedBlock { get; }

        public SelectBlockWindowViewModel(Block currentBlock)
        {
            SelectedBlock = new ReactivePropertySlim<Block>(currentBlock);
        }
    }
}
