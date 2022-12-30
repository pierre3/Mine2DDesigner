using MinecraftBlockBuilder.Graphics;
using System;

namespace MinecraftBlockBuilder.ViewModels
{
    interface IPaintPlane
    {
        event Action UpdateSuface;
        void PaintXZ(IGraphics g);

        void PaintXY(IGraphics g);

        void PaintZY(IGraphics g);
    }

    enum PlaneType
    {
        XZ,
        ZY,
        XY
    }
}
