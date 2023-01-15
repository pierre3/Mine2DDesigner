using Mine2DDesigner.Models;
using Mine2DDesigner.Services;
using Reactive.Bindings;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mine2DDesigner.ViewModels
{
    public static class DialogInvoker
    {
        public static async Task<(bool isOpened, string fileName, SaveData data)> OpenFile(this DialogServiceCollection services)
        {
            var dialog = services.Get<OpenFileDialogViewModel>();
            var vm = new OpenFileDialogViewModel()
            {
                InitialDirectory = Path.GetFullPath(".\\data")
            };
            if (dialog?.ShowDialog(vm) == true)
            {
                using var stream = new FileStream(vm.FileName, FileMode.Open, FileAccess.Read);
                var data = await JsonSerializer.DeserializeAsync<SaveData>(stream);
                return data is null
                    ? (false, string.Empty, new SaveData())
                    : (true, vm.FileName, data);
            }
            return (false, string.Empty, new SaveData());
        }

        public static async Task<string> SaveAs(this DialogServiceCollection services,
            string initialFileName, BlockAria blockAria, IList<Block> toolBoxItems)
        {
            var dialog = services.Get<SaveFileDialogViewModel>();
            var vm = new SaveFileDialogViewModel()
            {
                FileName = Path.GetFileName(initialFileName),
                InitialDirectory = Path.GetDirectoryName(initialFileName) ?? string.Empty
            };
            if (dialog?.ShowDialog(vm) == true)
            {
                var fileName = vm.FileName;
                Path.GetFileNameWithoutExtension(vm.FileName);
                await Save(fileName, blockAria, toolBoxItems);
                return fileName;
            }
            return initialFileName;
        }

        public static async Task Save(string fileName, BlockAria blockAria, IList<Block> toolBoxItems)
        {
            using var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, new SaveData
            {
                ToolBoxItems = toolBoxItems,
                Width = blockAria.Width,
                Height = blockAria.Height,
                Depth = blockAria.Depth,
                Aria = (IList<ushort[]>)blockAria.Aria
            });
        }

        public static (bool isCreated, string projectName, int width, int height, int depth) NewProject(
            this DialogServiceCollection Services, int initialWidth, int initialHeight, int initialDepth)
        {
            var dialog = Services.Get<NewProjectWindowViewModel>();
            var vm = new NewProjectWindowViewModel(initialWidth, initialHeight, initialDepth);
            if (dialog?.ShowDialog(vm) == true)
            {
                return (true, vm.Name.Value, vm.Width.Value, vm.Height.Value, vm.Depth.Value);
            }
            return (false, "", 0, 0, 0);
        }

        public static void OpenSettings(this DialogServiceCollection services, AppSettings settings)
        {
            var dialog = services.Get<SettingsWindowViewModel>();
            var vm = new SettingsWindowViewModel(settings.Rcon.Server, settings.Rcon.Port, settings.Rcon.Password);
            if (dialog?.ShowDialog(vm) == true)
            {
                settings.Rcon.Server = vm.Server.Value;
                settings.Rcon.Port = vm.Port.Value;
                settings.Rcon.Password = vm.Password.Value;
                File.WriteAllText("appsettings.json", JsonSerializer.Serialize(settings));
            }
        }

        public static (bool isSelected, Block selectedBlock) SelectBlock(this DialogServiceCollection services, Block currentBlock)
        {
            var dialog = services.Get<SelectBlockWindowViewModel>();
            var vm = new SelectBlockWindowViewModel(currentBlock);
            if (dialog?.ShowDialog(vm) == true)
            {
                return (true, vm.SelectedBlock.Value);
                
            }
            return (false, currentBlock);
        }

        public static void SendBlocks(this DialogServiceCollection services,AppSettings settings,BlockAria blockAria,IList<string> errorMessages)
        {
            var dialog = services.Get<SendBlocksWindowViewModel>();
            var vm = new SendBlocksWindowViewModel(settings, blockAria, errorMessages);
            dialog?.ShowDialog(vm);
        }
    }

}
