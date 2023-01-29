using System;
using System.IO;

namespace Mine2DDesigner.Models
{
    public class Textures
    {
        public string? Top { get; init; }
        public string? Side { get; init; }

        public Textures(string name)
        {
            var fileName = Path.GetFullPath(Path.Combine("assets", "block", name + ".png"));
            var topFileName = Path.GetFullPath(Path.Combine("assets", "block", name + "_top.png"));
            var sideFileName = Path.GetFullPath(Path.Combine("assets", "block", name + "_side.png"));

            var common = File.Exists(fileName) ? fileName : null;
            Top = File.Exists(topFileName)? topFileName: common;
            Side = File.Exists(sideFileName) ? sideFileName : common;
            
            if(Top is null || Side is null)
            {
                throw new FileNotFoundException($"{name} texture is not found.");
            }

        }
        public Textures()
        {
            Top = null;
            Side = null;
        }

        private byte[]? TopTextureBytes { get; set; }
        private byte[]? SideTextureBytes { get; set; }

        public byte[] GetTextureBytes(TextureType textureType)
        {
            if (string.IsNullOrEmpty(Top) || string.IsNullOrEmpty(Side))
            {
                return Array.Empty<byte>();
            }
            return textureType switch
            {
                TextureType.Top => TopTextureBytes ??= File.ReadAllBytes(Top),
                TextureType.Side => (Top == Side)
                    ? TopTextureBytes ??= File.ReadAllBytes(Top)
                    : (SideTextureBytes ??= File.ReadAllBytes(Side)),
                _ => throw new InvalidOperationException("Invalid TextureType.")
            };
        }

    }

}
