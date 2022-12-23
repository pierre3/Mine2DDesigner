using MinecraftBlockBuilder.Graphics;
using SkiaSharp.Views.WPF;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftBlockBuilder.Models
{
    internal class BlockAria
    {
        private static readonly int blockSize = 16;
        private readonly int width;
        private readonly int height;
        private readonly int depth;

        private IList<ushort[]> aria;
        public BlockAria(int width, int height, int depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            aria = Enumerable.Repeat(Enumerable.Repeat<ushort>(0, width * depth).ToArray(), height).ToList();
        }

        public void SetBlock(int x, int y, int z, ushort value)
        {
            aria[y][z * width + x] = value;
        }

        public ushort GetBlock(int x, int y, int z) => aria[y][z * width + x];

        public void PaintZX(IGraphics g, int currentX, int currentY, int currentZ)
        {
            DrawImageZX(g, currentY);
            DrawGridLineV(g, width, depth, currentX);
            DrawGridLineH(g, width, depth, currentZ);
        }

        private void DrawImageZX(IGraphics g, int y)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    var block = GetBlock(x, y, z);
                    if (block != 0)
                    {
                        
                    }
                }
            }
        }

        private static void DrawGridLineV(IGraphics g, int w, int h, int current)
        {
            for (int x = 0; x < w; x++)
            {
                var stroke = x == current ? new Stroke(Color.LightBlue, 4) : new Stroke(Color.DarkGray, 1);

                g.DrawLine(
                    new Point(x * blockSize, 0),
                    new Point(x * blockSize, h * blockSize),
                    stroke);
                g.DrawLine(
                    new Point(x * blockSize + (blockSize - 1), 0),
                    new Point(x * blockSize + (blockSize - 1), h * blockSize),
                    stroke);
            }
        }
        private static void DrawGridLineH(IGraphics g, int w, int h, int current)
        {
            for (int y = 0; y < h; y++)
            {
                var stroke = y == current ? new Stroke(Color.LightBlue, 4) : new Stroke(Color.DarkGray, 1);

                g.DrawLine(
                    new Point(0, y * blockSize),
                    new Point(w * blockSize, y * blockSize),
                    stroke);
                g.DrawLine(
                    new Point(0, y * blockSize + (blockSize - 1)),
                    new Point(w * blockSize, y * blockSize + (blockSize - 1)),
                    stroke);
            }
        }
    }
}
