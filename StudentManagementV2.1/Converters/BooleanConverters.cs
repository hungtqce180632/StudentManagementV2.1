using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StudentManagementV2._1.Converters
{
    /// <summary>
    /// Converts a boolean value to a Visibility value (True = Visible, False = Collapsed)
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a Visibility
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a Visibility to a boolean
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
    }

    /// <summary>
    /// Converts a boolean value to a Visibility value (True = Collapsed, False = Visible)
    /// </summary>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a Visibility (inverted)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Converts a Visibility to a boolean (inverted)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Collapsed;
        }
    }

    /// <summary>
    /// Converts a boolean value to a background color (True = Highlight, False = Normal)
    /// </summary>
    public class BooleanToBackgroundConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a background brush
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? new SolidColorBrush(Color.FromRgb(33, 150, 243)) : new SolidColorBrush(Color.FromRgb(245, 245, 245));
        }

        /// <summary>
        /// Converts a background brush to a boolean
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts a boolean value to a foreground color (True = White, False = Black)
    /// </summary>
    public class BooleanToForegroundConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a foreground brush
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Brushes.White : Brushes.Black;
        }

        /// <summary>
        /// Converts a foreground brush to a boolean
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
