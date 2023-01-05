namespace MinecraftBlockBuilder.Services
{
    public interface IServiceProvider
    {
        ServiceCollection Services { get; }
        void AddService(IService service);
    }
}
