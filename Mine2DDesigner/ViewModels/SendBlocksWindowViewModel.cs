using Mine2DDesigner.Models;
using Mine2DDesigner.Services;
using MinecraftConnection;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Mine2DDesigner.ViewModels
{
    public class SendBlocksWindowViewModel : INotifyPropertyChanged, IDisposable, IDialogViewModel
    {
        public ReactivePropertySlim<string> PlayerId { get; } = new("bacon_king112");
        public ReactivePropertySlim<bool> ReplaceAirBlocks { get; } = new(true);
        public ReactivePropertySlim<int> StartX { get; } = new(0);
        public ReactivePropertySlim<int> StartY { get; } = new(0);
        public ReactivePropertySlim<int> StartZ { get; } = new(0);

        public AsyncReactiveCommand GetPlayerLocationCommand { get; }
        public AsyncReactiveCommand SendBlocksCommand { get; }

        private readonly AppSettings settings;
        private readonly IList<string> errorMessages;
        private readonly BlockAria blockAria;
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler? PropertyChanged;

        public SendBlocksWindowViewModel(AppSettings settings, BlockAria blockAria, IList<string> errorMessages)
        {
            this.settings = settings;
            this.errorMessages = errorMessages;
            this.blockAria = blockAria;

            GetPlayerLocationCommand = new AsyncReactiveCommand();
            SendBlocksCommand = new AsyncReactiveCommand();
            InitRconCommands();
        }

        private void InitRconCommands()
        {
            GetPlayerLocationCommand.Subscribe(async () =>
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var minecraft = new MinecraftCommands(settings.Rcon.Server, settings.Rcon.Port, settings.Rcon.Password);
                        StartX.Value = (int)minecraft.GetPlayerData(PlayerId.Value).Postision.X;
                        StartY.Value = (int)minecraft.GetPlayerData(PlayerId.Value).Postision.Y;
                        StartZ.Value = (int)minecraft.GetPlayerData(PlayerId.Value).Postision.Z;
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(ex.Message);
                    }
                });
            }).AddTo(disposables);

            SendBlocksCommand.Subscribe(async () =>
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var minecraft = new MinecraftCommands(settings.Rcon.Server, settings.Rcon.Port, settings.Rcon.Password);

                        for (int y = 0; y < blockAria.Height; y++)
                        {
                            for (int z = 0; z < blockAria.Depth; z++)
                            {
                                for (int x = 0; x < blockAria.Width; x++)
                                {
                                    var blockIndex = blockAria.GetBlock(x, y, z);
                                    if (!ReplaceAirBlocks.Value && blockIndex == 0)
                                    {
                                        continue;
                                    }
                                    var blockName = Block.Definitions[blockIndex].Name;

                                    minecraft.SetBlock(StartX.Value + x, StartY.Value + y, StartZ.Value + z, blockName);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(ex.Message);
                    }
                });
            }).AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}