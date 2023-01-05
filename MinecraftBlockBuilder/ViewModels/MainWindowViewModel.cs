using MinecraftBlockBuilder.Bindings;
using MinecraftBlockBuilder.Graphics;
using MinecraftBlockBuilder.Models;
using MinecraftBlockBuilder.Services;
using MinecraftBlockBuilder.Views;
using MinecraftConnection;
using MinecraftConnection.RCON;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MinecraftBlockBuilder.ViewModels
{
    public class MainWindowViewModel : Services.IServiceProvider, IPaintPlane, INotifyPropertyChanged, IDisposable
    {
        private readonly BlockAria blockAria = new(25, 25, 25);
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action? UpdateSuface;


        public ReactivePropertySlim<PlaneType> ActivePlane { get; } = new(PlaneType.XZ);
        public ReactivePropertySlim<double> ScaleXZ { get; } = new(1.0);
        public ReactivePropertySlim<double> ScaleZY { get; } = new(1.0);
        public ReactivePropertySlim<double> ScaleXY { get; } = new(1.0);

        public ReactivePropertySlim<string> RconServer { get; } = new("localhost");
        public ReactivePropertySlim<ushort> RconPort { get; } = new(25575);
        public ReactivePropertySlim<string> RconPassword { get; } = new("minecraft");
        public ReactivePropertySlim<string> PlayerId { get; } = new("bacon_king112");
        public ReactivePropertySlim<bool> LevelingOfGround { get; } = new(true);

        public AsyncReactiveCommand SendBlocksCommand { get; }

        public ReactivePropertySlim<Block> SelectedBlock { get; }

        public ObservableCollection<Block> toolBoxItems { get; } = new()
        {
            Block.Definitions[1],
            Block.Definitions[2],
            Block.Definitions[3],
            Block.Definitions[4],
            Block.Definitions[5],
            Block.Definitions[6],
            Block.Definitions[7],
            Block.Definitions[8],
            Block.Definitions[9],
            Block.Definitions[10]
        };

        public ReadOnlyReactiveCollection<Block> ToolBoxItems { get; }
        private void RaiseUpdateSuface()
        {
            UpdateSuface?.Invoke();
        }

        public ReactiveCommand<KeyEvent> KeyDownCommand { get; set; }
        public ReactiveCommand SelectBlockCommand { get; set; }

        public ServiceCollection Services { get; } = new();

        public MainWindowViewModel()
        {
            ToolBoxItems = toolBoxItems
                .ToReadOnlyReactiveCollection()
                .AddTo(disposables);
            SelectedBlock = new(toolBoxItems[0]);
            SelectedBlock
                .Pairwise()
                .Subscribe(p =>
                {
                    if (p.NewItem is null)
                    {
                        SelectedBlock.Value = p.OldItem;
                    }
                });

            KeyDownCommand = new ReactiveCommand<KeyEvent>();
            InitKeyDownCommands();
            SelectBlockCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    var dialog = Services.Get<SelectBlockWindowService>();
                    var vm = new SelectBlockWindowViewModel(SelectedBlock.Value);
                    var blockIndex = toolBoxItems.IndexOf(SelectedBlock.Value);
                    if (blockIndex >= 0 && dialog?.ShowDialog(vm) == true)
                    {
                        toolBoxItems[blockIndex] = vm.SelectedBlock.Value;
                        SelectedBlock.Value = vm.SelectedBlock.Value;
                    }
                });

            SendBlocksCommand = new AsyncReactiveCommand().WithSubscribe(
                async () =>
                {
                    await Task.Run(() =>
                    {
                        var minecraft = new MinecraftCommands(RconServer.Value, RconPort.Value, RconPassword.Value);
                        var x0 = (int)minecraft.GetPlayerData(PlayerId.Value).Postision.X;
                        var y0 = (int)minecraft.GetPlayerData(PlayerId.Value).Postision.Y;
                        var z0 = (int)minecraft.GetPlayerData(PlayerId.Value).Postision.Z;

                        if (LevelingOfGround.Value)
                        {
                            minecraft.SendCommand($"fill {x0} {y0} {z0} {x0 + blockAria.Width - 1} {y0 + blockAria.Height - 1} {z0 + blockAria.Depth - 1} air");
                        }

                        for(int y = 0; y < blockAria.Height; y++)
                        {
                            for(int z = 0; z<blockAria.Depth; z++)
                            {
                                for(int x = 0; x < blockAria.Width; x++)
                                {
                                    var blockIndex = blockAria.GetBlock(x, y, z);
                                    if (blockIndex == 0)
                                    {
                                        continue;
                                    }
                                    var blockName = Block.Definitions[blockIndex].Name;
                                    
                                    minecraft.SetBlock(x0 + x, y0 + y, z0 + z, blockName);
                                }
                            }
                        }

                    });
                });
        }

        private void InitKeyDownCommands()
        {
            Action AfterAction(KeyEvent e) => () =>
            {
                if (e.IsPressedSpace)
                {
                    blockAria.SetBlock(SelectedBlock.Value.Index);
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
                    blockAria.SetBlock(SelectedBlock.Value.Index);
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

        public void AddService(IService service)
        {
            Services.Add(service);
        }
    }



}
