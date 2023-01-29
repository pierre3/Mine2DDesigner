using Mine2DDesigner.Bindings;
using Mine2DDesigner.Graphics;
using Mine2DDesigner.Models;
using Mine2DDesigner.Services;
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
using System.Text.Json;

namespace Mine2DDesigner.ViewModels
{
    public class MainWindowViewModel : IDialogServiceProvider, IPaintPlane, INotifyPropertyChanged, IDisposable
    {
        private AppSettings settings = new AppSettings();
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


        public ReactivePropertySlim<string> ProjectName { get; } = new("untitled");
        public ReactivePropertySlim<int> SelectedBlockIndex { get; }
        public ReactivePropertySlim<Block> SelectedBlock { get; }

        public AsyncReactiveCommand SaveCommand { get; }
        public AsyncReactiveCommand SaveAsCommand { get; }
        public AsyncReactiveCommand OpenCommand { get; }
        public ReactiveCommand NewCommand { get; }
        public ReactiveCommand SettingsCommand { get; }
        public ReactiveCommand ClearErrorMessagesCommand { get; }
        public ReactiveCommand SendBlocksCommand { get; }

        private string CurrentFileName { get; set; } = Path.GetFullPath(Path.Combine(".\\data", "untitled.json"));

        public ObservableCollection<Block> toolBoxItems { get; } = new()
        {
            Block.Definitions[0], Block.Definitions[1], Block.Definitions[2], Block.Definitions[3],
            Block.Definitions[4], Block.Definitions[5], Block.Definitions[6], Block.Definitions[7],
            Block.Definitions[8], Block.Definitions[9], Block.Definitions[10], Block.Definitions[11],
            Block.Definitions[12], Block.Definitions[13], Block.Definitions[14], Block.Definitions[15],
            Block.Definitions[16], Block.Definitions[17], Block.Definitions[18], Block.Definitions[19],
        };

        public ReadOnlyReactiveCollection<Block> ToolBoxItems { get; }

        public ObservableCollection<string> errorMessages { get; } = new();
        public ReadOnlyReactiveCollection<string> ErrorMessages { get; }
        public ReadOnlyReactivePropertySlim<bool> HasErrorMessages { get; }

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
            if (File.Exists("appsettings.json"))
            {
                var settingsJson = File.ReadAllText("appsettings.json");
                if (settingsJson is not null)
                {
                    settings = JsonSerializer.Deserialize<AppSettings>(settingsJson) ?? new AppSettings();
                }
            }

            ErrorMessages = errorMessages
                .ToReadOnlyReactiveCollection()
                .AddTo(disposables);
            HasErrorMessages = ErrorMessages
                .CollectionChangedAsObservable()
                .Select(_ => errorMessages.Count > 0).ToReadOnlyReactivePropertySlim<bool>();
            ClearErrorMessagesCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    errorMessages.Clear();
                })
                .AddTo(disposables);

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
                    try
                    {
                        if (SelectedBlockIndex.Value >= 0)
                        {
                            var saveIndex = SelectedBlockIndex.Value;
                            var (isSelected, selectedBlock) = Services.SelectBlock(toolBoxItems[SelectedBlockIndex.Value]);
                            if (isSelected)
                            {
                                toolBoxItems[SelectedBlockIndex.Value] = selectedBlock;
                                SelectedBlockIndex.Value = saveIndex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(ex.Message);
                    }
                }).AddTo(disposables);

            KeyDownCommand = new ReactiveCommand<KeyEvent>();
            SaveCommand = new AsyncReactiveCommand();
            SaveAsCommand = new AsyncReactiveCommand();
            OpenCommand = new AsyncReactiveCommand();
            NewCommand = new ReactiveCommand();
            SettingsCommand = new ReactiveCommand();
            SendBlocksCommand = new ReactiveCommand();
            InitKeyDownCommands();
            InitMenuCommands();
        }


        private void InitMenuCommands()
        {
            SaveCommand.Subscribe(async () =>
            {
                try
                {
                    await DialogInvoker.Save(CurrentFileName, blockAria, toolBoxItems);
                }
                catch (Exception ex)
                {
                    errorMessages.Add(ex.Message);
                }
            }).AddTo(disposables);

            SaveAsCommand.Subscribe(async () =>
            {
                try
                {
                    CurrentFileName = await Services.SaveAs(CurrentFileName, blockAria, toolBoxItems);
                    ProjectName.Value = Path.GetFileNameWithoutExtension(CurrentFileName);
                }
                catch (Exception ex)
                {
                    errorMessages.Add(ex.Message);
                }
            }).AddTo(disposables);

            OpenCommand.Subscribe(async () =>
            {
                try
                {
                    var (isOpened, fileName, data) = await Services.OpenFile();
                    if (isOpened)
                    {
                        SetToolBoxItems(data);
                        SetBlockAria(data.Width, data.Height, data.Depth, data.Aria);
                        RaiseUpdateSuface();
                        CurrentFileName = fileName;
                        ProjectName.Value = Path.GetFileNameWithoutExtension(fileName);
                    }
                }
                catch (Exception ex)
                {
                    errorMessages.Add(ex.Message);
                }
            }).AddTo(disposables);

            NewCommand.Subscribe(() =>
            {
                try
                {
                    var (isCreated, name, width, height, depth)
                        = Services.NewProject(blockAria.Width, blockAria.Height, blockAria.Depth);
                    if (isCreated)
                    {
                        ProjectName.Value = name;
                        CurrentFileName = Path.Combine(Path.GetDirectoryName(CurrentFileName) ?? "", name + ".json");
                        SetBlockAria(width, height, depth);
                        RaiseUpdateSuface();
                    }
                }
                catch (Exception ex)
                {
                    errorMessages.Add(ex.Message);
                }
            }).AddTo(disposables);

            SettingsCommand.Subscribe(() =>
            {
                try
                {
                    Services.OpenSettings(settings);
                }
                catch (Exception ex)
                {
                    errorMessages.Add(ex.Message);
                }

            }).AddTo(disposables);

            SendBlocksCommand.Subscribe(() =>
            {
                Services.SendBlocks(settings, blockAria, errorMessages);
            })
            .AddTo(disposables);
        }

        private void SetToolBoxItems(SaveData data)
        {
            if (data.ToolBoxItems is not null)
            {
                toolBoxItems.Clear();
                foreach (var item in data.ToolBoxItems)
                {
                    toolBoxItems.Add(item);
                }
            }
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
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Left)
                .Subscribe(e =>
                {
                    MoveLeft(e);
                })
                .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Right)
                .Subscribe(e =>
                {
                    MoveRight(e);
                })
                .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Up)
                .Subscribe(e =>
                {
                    MoveUp(e);
                })
                .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Down)
                .Subscribe(e =>
                {
                    Movedown(e);
                })
                .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.PageDown)
                .Subscribe(e =>
                {
                    PageDown(e);
                })
                .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.PageUp)
                .Subscribe(e =>
                {
                    PageUp(e);
                })
                .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Space)
                .Subscribe(_ =>
                {
                    if (SelectedBlockIndex.Value >= 0)
                    {
                        if (blockAria.PaintAria.PaintMode != PaintMode.None)
                        {
                            blockAria.ApplyPaintArea(toolBoxItems[SelectedBlockIndex.Value].Index);
                        }
                        else
                        {
                            blockAria.SetBlock(toolBoxItems[SelectedBlockIndex.Value].Index);
                        }
                    }
                    RaiseUpdateSuface();
                })
                .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.Tab)
                .Subscribe(_ =>
                {
                    ActivePlane.Value = (ActivePlane.Value == PlaneType.ZY)
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
               .Where(e => e.KeyType == KeyType.F1)
               .Subscribe(e =>
               {
                   blockAria.StartPaintMode(PaintMode.Cube, FillMode.Fill);
                   RaiseUpdateSuface();

               })
               .AddTo(disposables);
            KeyDownCommand
               .Where(e => e.KeyType == KeyType.F2)
               .Subscribe(e =>
               {
                   blockAria.StartPaintMode(PaintMode.Cube, FillMode.Surface);
                   RaiseUpdateSuface();
               })
               .AddTo(disposables);
            KeyDownCommand
               .Where(e => e.KeyType == KeyType.F3)
               .Subscribe(e =>
               {
                   blockAria.StartPaintMode(PaintMode.Ball, FillMode.Fill);
                   RaiseUpdateSuface();
               })
               .AddTo(disposables);
            KeyDownCommand
               .Where(e => e.KeyType == KeyType.F4)
               .Subscribe(e =>
               {
                   blockAria.StartPaintMode(PaintMode.Ball, FillMode.Surface);
                   RaiseUpdateSuface();
               })
               .AddTo(disposables);
            KeyDownCommand
               .Where(e => e.KeyType == KeyType.Escape)
               .Subscribe(e =>
               {
                   blockAria.StartPaintMode(PaintMode.None, FillMode.Fill);
                   RaiseUpdateSuface();
               })
               .AddTo(disposables);

            KeyDownCommand
                .Where(e => e.KeyType == KeyType.ZoomIn)
                .Subscribe(_ =>
                {
                    static double ZoomIn(double scale) => scale switch
                    {
                        0.5 => 0.75,
                        0.75 => 1.0,
                        1.0 => 1.5,
                        1.5 => 1.75,
                        1.75 => 2.0,
                        _ => scale
                    };
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.ZX:
                            ScaleZX.Value = ZoomIn(ScaleZX.Value);
                            break;
                        case PlaneType.ZY:
                            ScaleZY.Value = ZoomIn(ScaleZY.Value);
                            break;
                        case PlaneType.XY:
                            ScaleXY.Value = ZoomIn(ScaleXY.Value);
                            break;
                    }
                })
                .AddTo(disposables);
            KeyDownCommand
                .Where(e => e.KeyType == KeyType.ZoomOut)
                .Subscribe(_ =>
                {
                    static double ZoomOut(double scale) => scale switch
                    {
                        2.0 => 1.75,
                        1.75 => 1.5,
                        1.5 => 1.0,
                        1.0 => 0.75,
                        0.75 => 0.5,
                        _ => scale
                    };
                    switch (ActivePlane.Value)
                    {
                        case PlaneType.ZX:
                            ScaleZX.Value = ZoomOut(ScaleZX.Value);
                            break;
                        case PlaneType.ZY:
                            ScaleZY.Value = ZoomOut(ScaleZY.Value);
                            break;
                        case PlaneType.XY:
                            ScaleXY.Value = ZoomOut(ScaleXY.Value);
                            break;
                    }
                })
                .AddTo(disposables);
        }

        private Action AfterAction(KeyEvent e) => () =>
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

        private void MoveLeft(KeyEvent e)
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
        }

        private void MoveRight(KeyEvent e)
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
        }

        private void MoveUp(KeyEvent e)
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
        }

        private void Movedown(KeyEvent e)
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
        }

        private void PageDown(KeyEvent e)
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
        }

        private void PageUp(KeyEvent e)
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

        public IDialogServiceProvider AddService(IDialogService service)
        {
            Services.Add(service);
            return this;
        }
    }

}
