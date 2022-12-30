using MinecraftBlockBuilder.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftBlockBuilder.Models
{
    internal class BlockAria
    {
        private static readonly int blockSize = 16;
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        private ushort currentX = 0;
        private ushort currentY = 0;
        private ushort currentZ = 0;


        private IList<ushort[]> aria;
        public BlockAria(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
            aria = Enumerable.Range(0, height).Select(_ => Enumerable.Repeat<ushort>(0, width * depth).ToArray()).ToList();
        }

        public void SetBlock(ushort value)
        {
            SetBlock(currentX, currentY, currentZ, value);
        }

        public void SetBlock(int x, int y, int z, ushort value)
        {
            aria[y][z * Width + x] = GetBlock(x, y, z) == value ? (ushort)0 : value;
        }

        public ushort GetBlock(int x, int y, int z) => aria[y][z * Width + x];

        public void PaintXZ(IGraphics g)
        {
            DrawGridLine(g, Width, Depth);
            DrawImageXZ(g, currentY);
            DrawCurrentLine(g, Width, Depth, currentX, currentZ);
        }

        public void PaintXY(IGraphics g)
        {
            DrawGridLine(g, Width, Height);
            DrawImageXY(g, currentZ);
            DrawCurrentLine(g, Width, Height, currentX, currentY);
        }

        public void PaintZY(IGraphics g)
        {
            DrawGridLine(g, Depth, Height);
            DrawImageZY(g, currentX);
            DrawCurrentLine(g, Depth, Height, currentZ, currentY);
        }

        public void IncrementX(Action? afterAction=null)
        {
            if (currentX < Width - 1)
            {
                currentX++;
                afterAction?.Invoke();
            }
        }

        public void DecrementX(Action? afterAction = null)
        {
            if (currentX > 0)
            {
                currentX--;
                afterAction?.Invoke();

            }
        }

        public void IncrementY(Action? afterAction = null)
        {
            if (currentY < Height - 1)
            {
                currentY++;
                afterAction?.Invoke();
            }
        }

        public void DecrementY(Action? afterAction = null)
        {
            if (currentY > 0)
            {
                currentY--;
                afterAction?.Invoke();
            }
        }

        public void IncrementZ(Action? afterAction = null)
        {
            if (currentZ < Depth - 1)
            {
                currentZ++;
                afterAction?.Invoke();
            }
        }

        public void DecrementZ(Action? afterAction = null)
        {
            if (currentZ > 0)
            {
                currentZ--;
                afterAction?.Invoke();
            }
        }
 
        private void DrawImageXZ(IGraphics g, int y)
        {
            for (int z = 0; z < Depth; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var block = GetBlock(x, y, z);
                    if (block != 0)
                    {
                        g.DrawImage(
                            new Rectangle(
                                x * blockSize,
                                (Depth - 1 - z) * blockSize,
                                blockSize,

                                blockSize),
                            Block.Definitions[block].Textures.GetTextureBytes(TextureType.Top));
                    }
                }
            }
        }

        private void DrawImageXY(IGraphics g, int z)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var block = GetBlock(x, y, z);
                    if (block != 0)
                    {
                        g.DrawImage(
                            new Rectangle(
                                x * blockSize,
                                (Height - 1 - y) * blockSize,
                                blockSize,
                                blockSize),
                            Block.Definitions[block].Textures.GetTextureBytes(TextureType.Top));
                    }
                }
            }
        }

        private void DrawImageZY(IGraphics g, int x)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Width; z++)
                {
                    var block = GetBlock(x, y, z);
                    if (block != 0)
                    {
                        g.DrawImage(
                            new Rectangle(
                                z * blockSize,
                                (Height - 1 - y) * blockSize,
                                blockSize,
                                blockSize),
                            Block.Definitions[block].Textures.GetTextureBytes(TextureType.Top));
                    }
                }
            }
        }

        private static void DrawGridLine(IGraphics g, int w, int h)
        {
            for (int x = 0; x < w; x++)
            {
                g.DrawLine(
                    new Point(x * blockSize, 0),
                    new Point(x * blockSize, h * blockSize),
                    new Stroke(Color.DarkGray, 1));
            }
            for (int y = 0; y < h; y++)
            {
                g.DrawLine(
                    new Point(0, y * blockSize),
                    new Point(w * blockSize, y * blockSize),
                    new Stroke(Color.DarkGray, 1));
            }
        }

        private static void DrawCurrentLine(IGraphics g, int w, int h, int cX, int cY)
        {
            g.DrawLine(
                    new Point(cX * blockSize, 0),
                    new Point(cX * blockSize, h * blockSize),
                    new Stroke(Color.Blue, 2));
            g.DrawLine(
                new Point(cX * blockSize + blockSize, 0),
                new Point(cX * blockSize + blockSize, h * blockSize),
                new Stroke(Color.Blue, 2));

            g.DrawLine(
                    new Point(0, (h - 1 - cY) * blockSize),
                    new Point(w * blockSize, (h - 1 - cY) * blockSize),
                    new Stroke(Color.Blue, 2));
            g.DrawLine(
                new Point(0, (h - 1 - cY) * blockSize + blockSize),
                new Point(w * blockSize, (h - 1 - cY) * blockSize + blockSize),
                new Stroke(Color.Blue, 2));
        }
    }
}
