using System.Windows.Data;
using OpenLawOffice.Common.Models;

namespace OpenLawOffice.WinClient.Controls
{
    public class PermissionValueConverter : IValueConverter
    {
        private PermissionType _permission;

        public PermissionValueConverter()
        {
        }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PermissionType mask = (PermissionType)parameter;
            if (value == null) 
                return false;
            if (!typeof(PermissionType).IsAssignableFrom(value.GetType()))
                return false;
            _permission = (PermissionType)value;
            return ((mask & _permission) != 0);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            _permission ^= (PermissionType)parameter;
            return _permission;
        }
    }
}
