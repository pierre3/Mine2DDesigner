using System.Diagnostics.Eventing.Reader;

namespace MinecraftBlockBuilder.Bindings
{
    class KeyEvent
    {
        public KeyType KeyType { get; set; }
        public bool IsPressedSpace { get; set; }
    }
}
