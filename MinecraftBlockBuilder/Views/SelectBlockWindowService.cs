using System;
using System.Text;
using System.Threading.Tasks;
using MinecraftBlockBuilder.Services;
using MinecraftBlockBuilder.ViewModels;

namespace MinecraftBlockBuilder.Views
{
    public class SelectBlockWindowService : IService
    {

        public SelectBlockWindowService()
        {
        }

        public bool? ShowDialog(SelectBlockWindowViewModel vm)
        {
            var window = new SelectBlockWindow(vm);
            return window.ShowDialog();
        }
    }
}
