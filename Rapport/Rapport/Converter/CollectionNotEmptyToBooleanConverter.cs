﻿using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace Rapport.Converter
{
    public class CollectionNotEmptyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList collection)
            {
                return collection.Count != 0;

            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
