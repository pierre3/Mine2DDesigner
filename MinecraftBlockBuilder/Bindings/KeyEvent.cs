using System.Diagnostics.Eventing.Reader;

namespace MinecraftBlockBuilder.Bindings
{
    public class KeyEvent
    {
        public KeyType KeyType { get; set; }
        public bool IsPressedSpace { get; set; }
    }
}
