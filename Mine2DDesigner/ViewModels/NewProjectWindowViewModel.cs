using Mine2DDesigner.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;

namespace Mine2DDesigner.ViewModels
{
    public class NewProjectWindowViewModel : INotifyPropertyChanged, IDisposable, IDialogViewModel
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        public ReactivePropertySlim<string> Name { get; }
        public ReactivePropertySlim<int> Width { get; }
        public ReactivePropertySlim<int> Height { get; }
        public ReactivePropertySlim<int> Depth { get; }

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public NewProjectWindowViewModel(int defaultWidth, int defaultHeight, int defaultDepth)
        {
            Name = new ReactivePropertySlim<string>("untitled").AddTo(disposables);
            Width = new ReactivePropertySlim<int>(defaultWidth).AddTo(disposables);
            Height = new ReactivePropertySlim<int>(defaultHeight).AddTo(disposables);
            Depth = new ReactivePropertySlim<int>(defaultDepth).AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
