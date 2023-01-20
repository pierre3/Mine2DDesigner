using Mine2DDesigner.ViewModels;

namespace Mine2DDesigner.Services
{
    public interface IDialogServiceProvider
    {
        DialogServiceCollection Services { get; }
        IDialogServiceProvider AddService(IDialogService service);
    }
}
