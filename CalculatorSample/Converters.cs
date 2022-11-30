using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            if (value == null)
            {
                return "";
            }

            if (!decimal.TryParse(value.ToString(), out decimal in_num) || value.ToString().EndsWith("."))
            {
                return value;
            }

            string result = "";
            decimal int_part = Math.Floor(in_num);
            decimal dec_part = in_num - int_part;
            string dec_part_str = dec_part.ToString();
            string dec_part_without_dot = dec_part_str.Length > 1 ? dec_part_str.Substring(2) : dec_part_str;

            if (!in_num.ToString().Contains("."))
            {
                result = int_part.ToString("N0");
            }
            else if (Regex.IsMatch(dec_part_without_dot, @".*0+$"))
            {
                result = int_part.ToString("N0") + "." + dec_part_without_dot;
            }
            else if (dec_part_without_dot.Any(x => x != '0'))
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
