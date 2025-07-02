using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace Bookshelf.Converters
{
    public class TruncateConverter : IValueConverter
    {
        public int MaxLength { get; set; } = 150;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string text)
                return string.Empty;

            var tempt = text.Length > MaxLength
                ? $"{text.Substring(0, MaxLength)}…"
                : text;

            return text.Length > MaxLength
                ? $"{text.Substring(0, MaxLength)}…"
                : text;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
