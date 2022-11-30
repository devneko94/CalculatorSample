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
            throw new NotImplementedException();
        }
    }

    class DecimalToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !decimal.TryParse(value.ToString(), out decimal in_num))
            {
                return "";
            }

            string result = "";
            decimal int_part = Math.Floor(in_num);
            decimal dec_part = in_num - int_part;

            if (in_num.ToString().Contains("."))
            {
                result = int_part.ToString("N0") + dec_part.ToString("0.".PadRight(29, '#')).Substring(1);
            }
            else
            {
                result = int_part.ToString("N0");
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
