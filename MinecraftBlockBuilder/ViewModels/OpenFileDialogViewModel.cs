using MinecraftBlockBuilder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBlockBuilder.ViewModels
{
    public class OpenFileDialogViewModel:IDialogViewModel
    {
        public string FileName { get; set; } = "untitled.json";
        public string InitialDirectory { get; set; } = ".\\data";
        public string Filter { get; set; } = "JSON(*.json)|*.json";
    }
}
