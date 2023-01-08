namespace MinecraftBlockDesigner.Graphics
{
    public class Stroke
    {
        public Color Color { get; set; } = Color.Black;
        public float Width { get; set; } = 1f;

        public Stroke(Color color , float width)
        {
            Color = color;
            Width = width;
        }
    }
}
