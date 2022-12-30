using System;
using System.IO;

namespace MinecraftBlockBuilder.Models
{
    internal record class Textures
    {
        public string Top { get; init; }
        public string Side { get; init; }

        public Textures(string name)
        {
            var fileName = Path.Combine("assets", "block", name + ".png");
            var topFileName = Path.Combine("assets", "block", name + "_top.png");
            var sideFileName = Path.Combine("assets", "block", name + "_side.png");

            if (File.Exists(topFileName) && File.Exists(sideFileName))
            {
                Top = topFileName;
                Side = sideFileName;
            }
            else if (File.Exists(fileName))
            {
                Top = fileName;
                Side = fileName;
            }
            else
            {
                throw new FileNotFoundException($"{name} texture is not found.");
            }

        }
        public Textures()
        {
            Top = string.Empty;
            Side = string.Empty;
        }

        private byte[]? TopTextureBytes { get; set; }
        private byte[]? SideTextureBytes { get; set; }

        public byte[] GetTextureBytes(TextureType textureType)
        {
            if (Top == string.Empty || Side == string.Empty)
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
