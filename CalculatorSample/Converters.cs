using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CalculatorSample
{
    class StringToCalcModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Enum.TryParse(value.ToString(), out MainWindowVM.CalcModeEnum value_enum))
            {
                return DependencyProperty.UnsetValue;
            }

            switch (value_enum)
            {
                case MainWindowVM.CalcModeEnum.Add:
                    return "+";
                case MainWindowVM.CalcModeEnum.Sub:
                    return "-";
                case MainWindowVM.CalcModeEnum.Times:
                    return "*";
                case MainWindowVM.CalcModeEnum.Div:
                    return "/";
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string value_string = value.ToString();

            if (!Enum.TryParse(value_string, out MainWindowVM.CalcModeEnum value_enum))
            {
                return DependencyProperty.UnsetValue;
            }

            return value_enum;
        }
    }
}
