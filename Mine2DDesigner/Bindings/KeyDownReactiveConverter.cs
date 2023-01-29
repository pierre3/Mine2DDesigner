using Reactive.Bindings.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mine2DDesigner.Bindings
{
    public class KeyDownReactiveConverter : ReactiveConverter<KeyEventArgs, KeyEvent>
    {
        protected override IObservable<KeyEvent?> OnConvert(IObservable<KeyEventArgs?> source)
        {
            return source.Where(e => e?.IsDown == true).Select(e =>
            {
                var keyEvent = new KeyEvent()
                {
                    IsPressedSpace = e!.KeyboardDevice.IsKeyDown(Key.Space) == true,
                    KeyType = e?.Key switch
                    {
                        Key.Left => KeyType.Left,
                        Key.Right => KeyType.Right,
                        Key.Up => KeyType.Up,
                        Key.Down => KeyType.Down,
                        Key.Space => KeyType.Space,
                        Key.PageUp => KeyType.PageUp,
                        Key.PageDown => KeyType.PageDown,
                        Key.Tab => KeyType.Tab,
                        Key.Z => KeyType.ZoomIn,
                        Key.X => KeyType.ZoomOut,
                        Key.F1=> KeyType.F1,
                        Key.F2=> KeyType.F2,
                        Key.F3=> KeyType.F3,
                        Key.F4=> KeyType.F4,
                        Key.Escape=> KeyType.Escape,
                        Key.Enter=> KeyType.Enter,
                        Key _key when
                            (_key >= Key.D0 && _key <= Key.D9)
                            || (_key >= Key.NumPad0 && _key <= Key.NumPad9) => KeyType.Num,
                        _ => KeyType.None
                    }
                };
                if (keyEvent.KeyType == KeyType.Num)
                {
                    keyEvent.NumKey = e!.Key < Key.NumPad0
                        ? e.Key - Key.D0
                        : e.Key - Key.NumPad0;
                }
                if (keyEvent.KeyType != KeyType.None) { e!.Handled = true; }
                return keyEvent;
            }).Where(k => k.KeyType != KeyType.None);
        }
    }
}
