namespace WpfColorMixer.Mixer
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class WidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int percent && values[1] is double width)
            {
                return percent * width * 0.01;
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}