using Mine2DDesigner.ViewModels;
using Reactive.Bindings.Interactivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Mine2DDesigner.Bindings
{
    [ValueConversion(typeof(PlaneType), typeof(Brush))]
    public class PlaneTypeToBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var plane = parameter as string;
            var type = (PlaneType)value;
            return plane switch
            {
                "ZX" => (type == PlaneType.ZX) ? Brushes.Orange : Brushes.Transparent,
                "XY" => (type == PlaneType.XY) ? Brushes.Orange : Brushes.Transparent,
                "ZY" => (type == PlaneType.ZY) ? Brushes.Orange : Brushes.Transparent,
                _ => Brushes.Transparent
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
