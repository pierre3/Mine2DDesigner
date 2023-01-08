using System.Diagnostics.Eventing.Reader;

namespace MinecraftBlockDesigner.Bindings
{
    public class KeyEvent
    {
        public KeyType KeyType { get; set; }
        public bool IsPressedSpace { get; set; }
        public int NumKey { get; set; } = -1;
    }
}
