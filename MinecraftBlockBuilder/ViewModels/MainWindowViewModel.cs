using MinecraftBlockBuilder.Bindings;
using MinecraftBlockBuilder.Graphics;
using MinecraftBlockBuilder.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MinecraftBlockBuilder.ViewModels
{
    internal class MainWindowViewModel : IPaintPlane, INotifyPropertyChanged, IDisposable
    {
        private readonly BlockAria blockAria = new(25, 50, 25);
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action? UpdateSuface;


        public ReactivePropertySlim<PlaneType> ActivePlane { get; } = new(PlaneType.XZ);
        public ReactivePropertySlim<double> ScaleXZ { get; } = new(1.0);
        public ReactivePropertySlim<double> ScaleZY { get; } = new(1.0);
        public ReactivePropertySlim<double> ScaleXY { get; } = new(1.0);

        private void RaiseUpdateSuface()
        {
            UpdateSuface?.Invoke();
        }

        public ReactiveCommand<KeyEvent> KeyDownCommand { get; set; }


        public MainWindowViewModel()
        {
            KeyDownCommand = new ReactiveCommand<KeyEvent>();
            InitKeyDownCommands();
        }

        private void InitKeyDownCommands()
        {
            Action AfterAction(KeyEvent e) => () =>
            {
                if (e.IsPressedSpace)
                {
                    blockAria.SetBlock(1);
                }
                RaiseUpdateSuface();
            };

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Left)
                .Subscribe(e =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.XZ:
                            blockAria.DecrementX(AfterAction(e));
                            break;
                        case PlaneType.ZY:
                            blockAria.DecrementZ(AfterAction(e));
                            break;
                        case PlaneType.XY:
                            blockAria.DecrementX(AfterAction(e));
                            break;
                    }
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Right)
                .Subscribe(e =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.XZ:
                            blockAria.IncrementX(AfterAction(e));
                            break;
                        case PlaneType.ZY:
                            blockAria.IncrementZ(AfterAction(e));
                            break;
                        case PlaneType.XY:
                            blockAria.IncrementX(AfterAction(e));
                            break;
                    }
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Up)
                .Subscribe(e =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.XZ:
                            blockAria.IncrementZ(AfterAction(e));
                            break;
                        case PlaneType.ZY:
                            blockAria.IncrementY(AfterAction(e));
                            break;
                        case PlaneType.XY:
                            blockAria.IncrementY(AfterAction(e));
                            break;
                    }
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Down)
                .Subscribe(e =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.XZ:
                            blockAria.DecrementZ(AfterAction(e));
                            break;
                        case PlaneType.ZY:
                            blockAria.DecrementY(AfterAction(e));
                            break;
                        case PlaneType.XY:
                            blockAria.DecrementY(AfterAction(e));
                            break;
                    }

                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.PageDown)
                .Subscribe(e =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.XZ:
                            blockAria.DecrementY(AfterAction(e));
                            break;
                        case PlaneType.ZY:
                            blockAria.DecrementX(AfterAction(e));
                            break;
                        case PlaneType.XY:
                            blockAria.DecrementZ(AfterAction(e));
                            break;
                    }
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.PageUp)
                .Subscribe(e =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.XZ:
                            blockAria.IncrementY(AfterAction(e));
                            break;
                        case PlaneType.ZY:
                            blockAria.IncrementX(AfterAction(e));
                            break;
                        case PlaneType.XY:
                            blockAria.IncrementZ(AfterAction(e));
                            break;
                    }
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Space)
                .Subscribe(_ =>
                {
                    blockAria.SetBlock(1);
                    RaiseUpdateSuface();
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Tab)
                .Subscribe(_ =>
                {
                    ActivePlane.Value = (ActivePlane.Value == PlaneType.XY)
                        ? PlaneType.XZ
                        : ActivePlane.Value + 1;

                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.ZoomIn)
                .Subscribe(_ =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.XZ:
                            ScaleXZ.Value = ScaleXZ.Value switch
                            {
                                0.5 => 0.75,
                                0.75 => 1.0,
                                1.0 => 1.5,
                                1.5 => 1.75,
                                1.75 => 2.0,
                                _ => ScaleXZ.Value
                            };
                            break;
                        case PlaneType.ZY:
                            ScaleZY.Value = ScaleZY.Value switch
                            {
                                0.5 => 0.75,
                                0.75 => 1.0,
                                1.0 => 1.5,
                                1.5 => 1.75,
                                1.75 => 2.0,
                                _ => ScaleZY.Value
                            };
                            break;
                        case PlaneType.XY:
                            ScaleXY.Value = ScaleXY.Value switch
                            {
                                0.5 => 0.75,
                                0.75 => 1.0,
                                1.0 => 1.5,
                                1.5 => 1.75,
                                1.75 => 2.0,
                                _ => ScaleXY.Value
                            };
                            break;
                    }
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.ZoomOut)
                .Subscribe(_ =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.XZ:
                            ScaleXZ.Value = ScaleXZ.Value switch
                            {
                                2.0 => 1.75,
                                1.75 => 1.5,
                                1.5 => 1.0,
                                1.0 => 0.75,
                                0.75 => 0.5,
                                _ => ScaleXZ.Value
                            };
                            break;
                        case PlaneType.ZY:
                            ScaleZY.Value = ScaleZY.Value switch
                            {
                                2.0 => 1.75,
                                1.75 => 1.5,
                                1.5 => 1.0,
                                1.0 => 0.75,
                                0.75 => 0.5,
                                _ => ScaleZY.Value
                            };
                            break;
                        case PlaneType.XY:
                            ScaleXY.Value = ScaleXY.Value switch
                            {
                                2.0 => 1.75,
                                1.75 => 1.5,
                                1.5 => 1.0,
                                1.0 => 0.75,
                                0.75 => 0.5,
                                _ => ScaleXY.Value
                            };
                            break;
                    }
                })
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        void IPaintPlane.PaintXZ(IGraphics g)
        {
            blockAria.PaintXZ(g);
        }

        void IPaintPlane.PaintXY(IGraphics g)
        {
            blockAria.PaintXY(g);
        }

        void IPaintPlane.PaintZY(IGraphics g)
        {
            blockAria.PaintZY(g);
        }


    }



}
