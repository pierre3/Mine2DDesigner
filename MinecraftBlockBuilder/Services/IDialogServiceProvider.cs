using MinecraftBlockBuilder.ViewModels;

namespace MinecraftBlockBuilder.Services
{
    public interface IDialogServiceProvider
    {
        DialogServiceCollection Services { get; }
        void AddService(IDialogService service);
    }
}
