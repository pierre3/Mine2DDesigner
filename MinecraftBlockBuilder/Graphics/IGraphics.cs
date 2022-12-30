using Microsoft.VisualBasic;
using MinecraftBlockBuilder.Models;
using SkiaSharp;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace MinecraftBlockBuilder.Graphics
{
    public interface IGraphics
    {
        float ScaleX { get; }
        float ScaleY { get; }
        void ClearCanvas(Color color);
        void DrawRectangle(Rectangle rectangle, Stroke stroke);
        void FillRectangle(Rectangle rectangle, Fill fill);
        void DrawOval(Rectangle rectangle, Stroke stroke);
        void FillOval(Rectangle rectangle, Fill fill);
        void DrawLine(Point p1, Point p2, Stroke stroke);
        void DrawImage(Rectangle rect, byte[] bytes);
    }
}
