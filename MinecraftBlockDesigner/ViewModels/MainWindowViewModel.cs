using MinecraftBlockDesigner.Bindings;
using MinecraftBlockDesigner.Graphics;
using MinecraftBlockDesigner.Models;
using MinecraftBlockDesigner.Services;
using MinecraftConnection;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MinecraftBlockDesigner.ViewModels
{
    public class MainWindowViewModel : IDialogServiceProvider, IPaintPlane, INotifyPropertyChanged, IDisposable
    {
        private BlockAria blockAria = new(25, 25, 25);
        private readonly CompositeDisposable disposables = new CompositeDisposable();

#pragma warning disable 0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067
        public event Action? UpdateSuface;


        public ReactivePropertySlim<PlaneType> ActivePlane { get; } = new(PlaneType.ZX);
        public ReactivePropertySlim<double> ScaleZX { get; } = new(1.0);
        public ReactivePropertySlim<double> ScaleZY { get; } = new(1.0);
        public ReactivePropertySlim<double> ScaleXY { get; } = new(1.0);

        public ReactivePropertySlim<int> WidthZX { get; } = new(BlockAria.BlockSize * 25);
        public ReactivePropertySlim<int> WidthZY { get; } = new(BlockAria.BlockSize * 25);
        public ReactivePropertySlim<int> WidthXY { get; } = new(BlockAria.BlockSize * 25);

        public ReactivePropertySlim<int> HeightZX { get; } = new(BlockAria.BlockSize * 25);
        public ReactivePropertySlim<int> HeightZY { get; } = new(BlockAria.BlockSize * 25);
        public ReactivePropertySlim<int> HeightXY { get; } = new(BlockAria.BlockSize * 25);


        public ReactivePropertySlim<string> RconServer { get; } = new("localhost");
        public ReactivePropertySlim<ushort> RconPort { get; } = new(25575);
        public ReactivePropertySlim<string> RconPassword { get; } = new("minecraft");
        public ReactivePropertySlim<string> PlayerId { get; } = new("bacon_king112");
        public ReactivePropertySlim<bool> LevelingOfGround { get; } = new(true);
        public ReactivePropertySlim<string> ProjectName { get; } = new("untitled");
        public ReactivePropertySlim<int> SelectedBlockIndex { get; }
        public ReactivePropertySlim<Block> SelectedBlock { get; }

        public AsyncReactiveCommand SendBlocksCommand { get; }
        public AsyncReactiveCommand SaveCommand { get; }
        public AsyncReactiveCommand SaveAsCommand { get; }
        public AsyncReactiveCommand OpenCommand { get; }
        public ReactiveCommand NewCommand { get; }

        private string CurrentFileName { get; set; } = Path.GetFullPath(Path.Combine(".\\data", "untitled.json"));

        public ObservableCollection<Block> toolBoxItems { get; } = new()
        {
            Block.Definitions[0],
            Block.Definitions[1],
            Block.Definitions[2],
            Block.Definitions[3],
            Block.Definitions[4],
            Block.Definitions[5],
            Block.Definitions[6],
            Block.Definitions[7],
            Block.Definitions[8],
            Block.Definitions[9],
            Block.Definitions[10],
            Block.Definitions[11],
            Block.Definitions[12],
            Block.Definitions[13],
            Block.Definitions[14],
            Block.Definitions[15],
            Block.Definitions[16],
            Block.Definitions[17],
            Block.Definitions[18],
            Block.Definitions[19],
        };

        public ReadOnlyReactiveCollection<Block> ToolBoxItems { get; }
        private void RaiseUpdateSuface()
        {
            UpdateSuface?.Invoke();
        }

        public ReactiveCommand<KeyEvent> KeyDownCommand { get; set; }
        public ReactiveCommand SelectBlockCommand { get; set; }

        public DialogServiceCollection Services { get; } = new();

        public MainWindowViewModel()
        {
            Directory.CreateDirectory(".\\data");
            ToolBoxItems = toolBoxItems
                .ToReadOnlyReactiveCollection()
                .AddTo(disposables);

            SelectedBlockIndex = new(1);
            SelectedBlockIndex
                .Pairwise()
                .Subscribe(p =>
                {
                    if (p.NewItem < 0)
                    {
                        SelectedBlockIndex.Value = p.OldItem;
                    }
                }).AddTo(disposables);

            SelectedBlock = new(toolBoxItems[1]);
            SelectedBlockIndex
                .Subscribe(i =>
                {
                    if (i >= 0)
                    {
                        SelectedBlock.Value = toolBoxItems[i];
                    }
                });
            SelectBlockCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    if (SelectedBlockIndex.Value >= 0)
                    {
                        var saveIndex = SelectedBlockIndex.Value;
                        var dialog = Services.Get<SelectBlockWindowViewModel>();
                        var vm = new SelectBlockWindowViewModel(toolBoxItems[SelectedBlockIndex.Value]);

                        if (dialog?.ShowDialog(vm) == true)
                        {
                            toolBoxItems[SelectedBlockIndex.Value] = vm.SelectedBlock.Value;
                            SelectedBlockIndex.Value = saveIndex;
                        }
                    }
                }).AddTo(disposables);

            KeyDownCommand = new ReactiveCommand<KeyEvent>();
            InitKeyDownCommands();

            SendBlocksCommand = new AsyncReactiveCommand();
            SaveCommand = new AsyncReactiveCommand();
            SaveAsCommand = new AsyncReactiveCommand();
            OpenCommand = new AsyncReactiveCommand();
            NewCommand = new ReactiveCommand();
            InitMenuCommands();
        }

        private void InitMenuCommands()
        {
            SendBlocksCommand.Subscribe(async () =>
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

                    for (int y = 0; y < blockAria.Height; y++)
                    {
                        for (int z = 0; z < blockAria.Depth; z++)
                        {
                            for (int x = 0; x < blockAria.Width; x++)
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
            }).AddTo(disposables);

            SaveCommand.Subscribe(async () =>
            {
                await Save(CurrentFileName);

            }).AddTo(disposables);

            SaveAsCommand.Subscribe(async () =>
            {
                var dialog = Services.Get<SaveFileDialogViewModel>();
                if (dialog == null) { return; }
                var vm = new SaveFileDialogViewModel()
                {
                    FileName = ProjectName.Value,
                    InitialDirectory = Path.GetDirectoryName(CurrentFileName) ?? string.Empty
                };
                dialog.ShowDialog(vm);
                CurrentFileName = vm.FileName;
                ProjectName.Value = Path.GetFileNameWithoutExtension(vm.FileName);
                await Save(CurrentFileName);

            }).AddTo(disposables);

            OpenCommand.Subscribe(async () =>
            {
                var dialog = Services.Get<OpenFileDialogViewModel>();
                if (dialog == null) { return; }
                var vm = new OpenFileDialogViewModel()
                {
                    InitialDirectory = Path.GetFullPath(".\\data")
                };
                if (dialog.ShowDialog(vm) != true) { return; }
                using var stream = new FileStream(vm.FileName, FileMode.Open, FileAccess.Read);
                var data = await System.Text.Json.JsonSerializer.DeserializeAsync<SaveData>(stream);
                if (data is null) { return; }
                if (data.ToolBoxItems is null) { return; }

                toolBoxItems.Clear();
                foreach (var item in data.ToolBoxItems)
                {
                    toolBoxItems.Add(item);
                }
                SetBlockAria(data.Width, data.Height, data.Depth, data.Aria);
                RaiseUpdateSuface();
                CurrentFileName = vm.FileName;
            }).AddTo(disposables);

            NewCommand.Subscribe(() =>
            {
                var dialog = Services.Get<NewProjectWindowViewModel>();
                var vm = new NewProjectWindowViewModel(blockAria.Width, blockAria.Height, blockAria.Depth);
                dialog?.ShowDialog(vm);
                ProjectName.Value = vm.Name.Value;
                CurrentFileName = Path.Combine(
                    Path.GetDirectoryName(CurrentFileName) ?? Path.GetFullPath(".\\data"),
                    ProjectName.Value + ".json");
                SetBlockAria(vm.Width.Value, vm.Height.Value, vm.Depth.Value);
                RaiseUpdateSuface();
            }).AddTo(disposables);
        }

        private async Task Save(string fileName)
        {
            using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await System.Text.Json.JsonSerializer.SerializeAsync(stream, new SaveData
            {
                ToolBoxItems = toolBoxItems.ToList(),
                Width = blockAria.Width,
                Height = blockAria.Height,
                Depth = blockAria.Depth,
                Aria = (IList<ushort[]>)blockAria.Aria
            });
        }

        private void SetBlockAria(int width, int height, int depth, IEnumerable<ushort[]>? aria = null)
        {
            blockAria = new BlockAria(width, height, depth, aria);
            WidthXY.Value = BlockAria.BlockSize * blockAria.Width;
            WidthZX.Value = BlockAria.BlockSize * blockAria.Depth;
            WidthZY.Value = BlockAria.BlockSize * blockAria.Depth;
            HeightXY.Value = BlockAria.BlockSize * blockAria.Height;
            HeightZX.Value = BlockAria.BlockSize * blockAria.Width;
            HeightZY.Value = BlockAria.BlockSize * blockAria.Height;
        }

        private void InitKeyDownCommands()
        {
            Action AfterAction(KeyEvent e) => () =>
            {
                if (e.IsPressedSpace)
                {
                    if (SelectedBlockIndex.Value >= 0)
                    {
                        blockAria.SetBlock(toolBoxItems[SelectedBlockIndex.Value].Index);
                    }
                }
                RaiseUpdateSuface();
            };

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Left)
                .Subscribe(e =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.ZX:
                            blockAria.DecrementZ(AfterAction(e));
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
                        case PlaneType.ZX:
                            blockAria.IncrementZ(AfterAction(e));
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
                        case PlaneType.ZX:
                            blockAria.IncrementX(AfterAction(e));
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
                        case PlaneType.ZX:
                            blockAria.DecrementX(AfterAction(e));
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
                        case PlaneType.ZX:
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
                        case PlaneType.ZX:
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
                    if (SelectedBlockIndex.Value >= 0)
                    {
                        blockAria.SetBlock(toolBoxItems[SelectedBlockIndex.Value].Index);
                    }
                    RaiseUpdateSuface();
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Tab)
                .Subscribe(_ =>
                {
                    ActivePlane.Value = (ActivePlane.Value == PlaneType.XY)
                        ? PlaneType.ZX
                        : ActivePlane.Value + 1;

                })
                .AddTo(disposables);
            KeyDownCommand
               .Where(e => e.KeyType == KeyType.Num)
               .Subscribe(e =>
               {
                   SelectedBlockIndex.Value = e.NumKey;
               })
               .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.ZoomIn)
                .Subscribe(_ =>
                {
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.ZX:
                            ScaleZX.Value = ScaleZX.Value switch
                            {
                                0.5 => 0.75,
                                0.75 => 1.0,
                                1.0 => 1.5,
                                1.5 => 1.75,
                                1.75 => 2.0,
                                _ => ScaleZX.Value
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
                        case PlaneType.ZX:
                            ScaleZX.Value = ScaleZX.Value switch
                            {
                                2.0 => 1.75,
                                1.75 => 1.5,
                                1.5 => 1.0,
                                1.0 => 0.75,
                                0.75 => 0.5,
                                _ => ScaleZX.Value
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

        void IPaintPlane.PaintZX(IGraphics g)
        {
            blockAria.PaintZX(g);
        }

        void IPaintPlane.PaintXY(IGraphics g)
        {
            blockAria.PaintXY(g);
        }

        void IPaintPlane.PaintZY(IGraphics g)
        {
            blockAria.PaintZY(g);
        }

        public void AddService(IDialogService service)
        {
            Services.Add(service);
        }
    }



}
