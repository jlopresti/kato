﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Kato.vNext.Models;

namespace Kato.vNext.Converters
{
    public class StatusToBuildingColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BuildStatus status = (BuildStatus)value;
            Brush color = App.Current.Resources["DisabledBuildColorBrush"] as Brush;
            switch (status)
            {
                case BuildStatus.Success:
                    color = App.Current.Resources["SuccessBuildColorBrush"] as Brush;
                    break;
                case BuildStatus.SuccessAndBuilding:
                    color = App.Current.Resources["SuccessAndBuildingBuildColorBrush"] as Brush;
                    break;
                case BuildStatus.Failed:
                    color = App.Current.Resources["FailedBuildColorBrush"] as Brush;
                    break;
                case BuildStatus.FailedAndBuilding:
                    color = App.Current.Resources["FailedAndBuildingBuildColorBrush"] as Brush;
                    break;
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
