using MinecraftBlockDesigner.Services;

namespace MinecraftBlockDesigner.ViewModels
{
    public class OpenFileDialogViewModel:IDialogViewModel
    {
        public string FileName { get; set; } = "untitled.json";
        public string InitialDirectory { get; set; } = ".\\data";
        public string Filter { get; set; } = "JSON(*.json)|*.json";
    }
}
