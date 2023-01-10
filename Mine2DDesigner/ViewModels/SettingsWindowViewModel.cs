using Mine2DDesigner.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace Mine2DDesigner.ViewModels
{
    public class SettingsWindowViewModel : INotifyPropertyChanged, IDisposable, IDialogViewModel
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        public ReactivePropertySlim<string> Server { get; }
        public ReactivePropertySlim<int> Port { get; }
        public ReactivePropertySlim<string> Password { get; }

        private readonly CompositeDisposable disposables = new();

        public SettingsWindowViewModel(string server, int port, string password)
        {
            Server = new ReactivePropertySlim<string>(server).AddTo(disposables);
            Port = new ReactivePropertySlim<int>(port).AddTo(disposables);
            Password = new ReactivePropertySlim<string>(password).AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
