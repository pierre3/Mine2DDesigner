using MinecraftBlockDesigner.ViewModels;

namespace MinecraftBlockDesigner.Services
{
    public interface IDialogServiceProvider
    {
        DialogServiceCollection Services { get; }
        void AddService(IDialogService service);
    }
}
