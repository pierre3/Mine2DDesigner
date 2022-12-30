using Reactive.Bindings.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MinecraftBlockBuilder.Bindings
{
    class KeyDownReactiveConverter : ReactiveConverter<KeyEventArgs, KeyEvent>
    {
        protected override IObservable<KeyEvent?> OnConvert(IObservable<KeyEventArgs?> source)
        {
            return source.Where(e => e?.IsDown == true).Select(e =>
            {
                var keyEvent = new KeyEvent()
                {
                    IsPressedSpace = e?.KeyboardDevice?.IsKeyDown(Key.Space) == true,
                    KeyType = e?.Key switch
                    {
                        Key.Left => KeyType.Left,
                        Key.Right => KeyType.Right,
                        Key.Up => KeyType.Up,
                        Key.Down => KeyType.Down,
                        Key.Space => KeyType.Space,
                        Key.PageUp => KeyType.PageUp,
                        Key.PageDown => KeyType.PageDown,
                        Key.Tab=> KeyType.Tab,
                        Key.Z=>KeyType.ZoomIn,
                        Key.X=>KeyType.ZoomOut,
                        _ => KeyType.None
                    }
                };
                if (e is not null && keyEvent.KeyType != KeyType.None) { e.Handled = true; }
                return keyEvent;
            }).Where(k => k.KeyType != KeyType.None);
        }
    }
}
