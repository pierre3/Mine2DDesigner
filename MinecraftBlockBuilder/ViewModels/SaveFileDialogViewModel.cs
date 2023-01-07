using MinecraftBlockBuilder.Services;

namespace MinecraftBlockBuilder.ViewModels
{
    public class SaveFileDialogViewModel : IDialogViewModel
    {
        public string FileName { get; set; } = "untitled.json";
        public string InitialDirectory { get; set; } = ".\\data";
        public string Filter { get; set; } = "JSON(*.json)|*.json";
    }
}
