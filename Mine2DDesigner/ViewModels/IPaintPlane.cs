using Mine2DDesigner.Graphics;
using System;

namespace Mine2DDesigner.ViewModels
{
    public interface IPaintPlane
    {
        event Action UpdateSuface;
        void PaintZX(IGraphics g);

        void PaintXY(IGraphics g);

        void PaintZY(IGraphics g);
    }
}
