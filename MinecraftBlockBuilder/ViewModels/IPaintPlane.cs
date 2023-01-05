using MinecraftBlockBuilder.Graphics;
using System;

namespace MinecraftBlockBuilder.ViewModels
{
    public interface IPaintPlane
    {
        event Action UpdateSuface;
        void PaintXZ(IGraphics g);

        void PaintXY(IGraphics g);

        void PaintZY(IGraphics g);
    }
}
