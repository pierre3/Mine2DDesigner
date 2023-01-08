using Mine2DDesigner.ViewModels;

namespace Mine2DDesigner.Services
{
    public interface IDialogServiceProvider
    {
        DialogServiceCollection Services { get; }
        void AddService(IDialogService service);
    }
}
