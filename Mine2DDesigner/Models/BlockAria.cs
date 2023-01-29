using Mine2DDesigner.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mine2DDesigner.Models
{
    public class BlockAria
    {
        public static readonly int BlockSize = 16;
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }

        public PaintAria PaintAria { get; } = new PaintAria();
        private bool isPaintPreview = false;
        private ushort currentX = 0;
        private ushort currentY = 0;
        private ushort currentZ = 0;

        public IReadOnlyCollection<ushort[]> Aria { get => new ReadOnlyCollection<ushort[]>(aria); }
        private IList<ushort[]> aria;

        public BlockAria(int width, int height, int depth, IEnumerable<ushort[]>? aria = null)
        {
            Width = width;
            Height = height;
            Depth = depth;
            if (aria is null)
            {
                this.aria = Enumerable.Range(0, height).Select(_ => Enumerable.Repeat<ushort>(0, width * depth).ToArray()).ToList();
            }
            else
            {
                this.aria = aria.ToList();
            }
        }

        public void StartPaintMode(PaintMode paintMode, FillMode fillMode)
        {
            if (PaintAria.PaintMode == PaintMode.None)
            {
                PaintAria.Start = new Point3i(currentX, currentY, currentZ);
                PaintAria.End = new Point3i(currentX, currentY, currentZ);
                isPaintPreview = false;
            }
            PaintAria.FillMode = fillMode;
            PaintAria.PaintMode = paintMode;
        }

        public void SetPreview()
        {
            isPaintPreview = !isPaintPreview;
        }

        public void SetPaintEnd()
        {
            if (PaintAria.PaintMode != PaintMode.None && !isPaintPreview)
            {
                PaintAria.End = new Point3i(currentX, currentY, currentZ);
            }
        }

        public void ApplyPaintArea(ushort value)
        {
            foreach(var aria in PaintAria.GetPaintArea())
            {
                SetBlock(aria.X, aria.Y, aria.Z, value);
            }
            PaintAria.PaintMode= PaintMode.None;
            isPaintPreview= false;
        }

        public void SetBlock(ushort value)
        {
            SetBlock(currentX, currentY, currentZ, 
                GetBlock(currentX, currentY, currentX) == value 
                    ? (ushort)0 
                    : value);
        }

        public void SetBlock(int x, int y, int z, ushort value)
        {
            aria[y][x * Depth + z] = value;
        }

        public ushort GetBlock(int x, int y, int z) => aria[y][x * Depth + z];

        public void PaintZX(IGraphics g)
        {
            DrawGridLine(g, Depth, Width);
            DrawImageZX(g, currentY);
            DrawCurrentLine(g, Depth, Width, currentZ, currentX);
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

        public void IncrementX(Action? afterAction = null)
        {
            if (currentX < Width - 1)
            {
                currentX++;
                SetPaintEnd();
                afterAction?.Invoke();
            }
        }

        public void DecrementX(Action? afterAction = null)
        {
            if (currentX > 0)
            {
                currentX--;
                SetPaintEnd();
                afterAction?.Invoke();

            }
        }

        public void IncrementY(Action? afterAction = null)
        {
            if (currentY < Height - 1)
            {
                currentY++;
                SetPaintEnd();
                afterAction?.Invoke();
            }
        }

        public void DecrementY(Action? afterAction = null)
        {
            if (currentY > 0)
            {
                currentY--;
                SetPaintEnd();
                afterAction?.Invoke();
            }
        }

        public void IncrementZ(Action? afterAction = null)
        {
            if (currentZ < Depth - 1)
            {
                currentZ++;
                SetPaintEnd();
                afterAction?.Invoke();
            }
        }

        public void DecrementZ(Action? afterAction = null)
        {
            if (currentZ > 0)
            {
                currentZ--;
                SetPaintEnd();
                afterAction?.Invoke();
            }
        }

        private void DrawImageZX(IGraphics g, int y)
        {
            var paintAria = PaintAria.GetPaintArea();
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    var block = GetBlock(x, y, z);
                    var rect = new Rectangle(
                        z * BlockSize,
                        (Width - 1 - x) * BlockSize,
                        BlockSize,
                        BlockSize);
                    if (block != 0)
                    {
                        g.DrawImage(
                            rect,
                            Block.Definitions[block].Textures.GetTextureBytes(TextureType.Top));
                    }
                    if (paintAria.Contains(new Point3i(x, y, z)))
                    {
                        var alpha = (byte)255;// PaintAria.Start == new Point3i(x, y, z) ? (byte)128 : (byte)64;
                        var col = PaintAria.FillMode == FillMode.Fill
                            ? new Color(0, 255, 255, alpha)
                            : new Color(255, 0, 255, alpha);
                        g.FillRectangle(rect, new Fill(col));
                    }
                }
            }
        }

        private void DrawImageXY(IGraphics g, int z)
        {
            var paintAria = PaintAria.GetPaintArea();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var block = GetBlock(x, y, z);
                    var rect = new Rectangle(
                        x * BlockSize,
                        (Height - 1 - y) * BlockSize,
                        BlockSize,
                        BlockSize);
                    if (block != 0)
                    {
                        g.DrawImage(
                            rect,
                            Block.Definitions[block].Textures.GetTextureBytes(TextureType.Side));
                    }
                    if (paintAria.Contains(new Point3i(x, y, z)))
                    {
                        var alpha = (byte)255;// PaintAria.Start == new Point3i(x, y, z) ? (byte)128 : (byte)64;
                        var col = PaintAria.FillMode == FillMode.Fill
                            ? new Color(0, 255, 255, alpha)
                            : new Color(255, 0, 255, alpha);
                        g.FillRectangle(rect, new Fill(col));
                    }
                }
            }
        }

        private void DrawImageZY(IGraphics g, int x)
        {
            var paintAria = PaintAria.GetPaintArea();
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Width; z++)
                {
                    var block = GetBlock(x, y, z);
                    var rect = new Rectangle(
                        z * BlockSize,
                        (Height - 1 - y) * BlockSize,
                        BlockSize,
                        BlockSize);
                    if (block != 0)
                    {
                        g.DrawImage(
                            rect,
                            Block.Definitions[block].Textures.GetTextureBytes(TextureType.Side));
                    }
                    if (paintAria.Contains(new Point3i(x, y, z)))
                    {
                        var alpha = (byte)255;// PaintAria.Start == new Point3i(x, y, z) ? (byte)128 : (byte)64;
                        var col = PaintAria.FillMode == FillMode.Fill
                            ? new Color(0, 255, 255, alpha)
                            : new Color(255, 0, 255, alpha);
                        g.FillRectangle(rect, new Fill(col));
                    }
                }
            }
        }

        private static void DrawGridLine(IGraphics g, int w, int h)
        {
            for (int x = 0; x < w; x++)
            {
                g.DrawLine(
                    new Point(x * BlockSize, 0),
                    new Point(x * BlockSize, h * BlockSize),
                    new Stroke(Color.DarkGray, 1));
            }
            for (int y = 0; y < h; y++)
            {
                g.DrawLine(
                    new Point(0, y * BlockSize),
                    new Point(w * BlockSize, y * BlockSize),
                    new Stroke(Color.DarkGray, 1));
            }
        }

        private static void DrawCurrentLine(IGraphics g, int w, int h, int cX, int cY)
        {
            g.DrawLine(
                    new Point(cX * BlockSize, 0),
                    new Point(cX * BlockSize, h * BlockSize),
                    new Stroke(Color.Blue, 2));
            g.DrawLine(
                new Point(cX * BlockSize + BlockSize, 0),
                new Point(cX * BlockSize + BlockSize, h * BlockSize),
                new Stroke(Color.Blue, 2));

            g.DrawLine(
                    new Point(0, (h - 1 - cY) * BlockSize),
                    new Point(w * BlockSize, (h - 1 - cY) * BlockSize),
                    new Stroke(Color.Blue, 2));
            g.DrawLine(
                new Point(0, (h - 1 - cY) * BlockSize + BlockSize),
                new Point(w * BlockSize, (h - 1 - cY) * BlockSize + BlockSize),
                new Stroke(Color.Blue, 2));
        }
    }
}
