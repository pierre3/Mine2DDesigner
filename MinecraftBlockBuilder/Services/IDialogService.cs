using MinecraftBlockBuilder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBlockBuilder.Services
{
    public interface IDialogService
    {
        Type VmType { get; }
        bool? ShowDialog(IDialogViewModel vm);
    }
}
