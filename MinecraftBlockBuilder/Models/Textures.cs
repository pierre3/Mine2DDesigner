using System;
using System.IO;

namespace MinecraftBlockBuilder.Models
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
