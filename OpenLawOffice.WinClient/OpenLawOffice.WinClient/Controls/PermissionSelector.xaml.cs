using System;
using System.Windows;
using System.Windows.Controls;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for PermissionSelector.xaml
    /// </summary>
    public partial class PermissionSelector : UserControl
    {
        public PermissionSelector()
        {
            InitializeComponent();
        }

        private void UICreate_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            perms |= Common.Models.PermissionType.Create;
            DataContext = perms;
        }

        private void UICreate_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            if (perms.HasFlag(Common.Models.PermissionType.Create))
                perms ^= Common.Models.PermissionType.Create;
            DataContext = perms;
        }

        private void UIRead_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            perms |= Common.Models.PermissionType.Read;
            DataContext = perms;
        }

        private void UIRead_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            if (perms.HasFlag(Common.Models.PermissionType.Read))
                perms ^= Common.Models.PermissionType.Read;
            DataContext = perms;
        }

        private void UIDisable_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            perms |= Common.Models.PermissionType.Disable;
            DataContext = perms;
        }

        private void UIDisable_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            if (perms.HasFlag(Common.Models.PermissionType.Disable))
                perms ^= Common.Models.PermissionType.Disable;
            DataContext = perms;
        }

        private void UIList_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            perms |= Common.Models.PermissionType.List;
            DataContext = perms;
        }

        private void UIList_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            if (perms.HasFlag(Common.Models.PermissionType.List))
                perms ^= Common.Models.PermissionType.List;
            DataContext = perms;
        }

        private void UIModify_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            perms |= Common.Models.PermissionType.Modify;
            DataContext = perms;
        }

        private void UIModify_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            if (perms.HasFlag(Common.Models.PermissionType.Modify))
                perms ^= Common.Models.PermissionType.Modify;
            DataContext = perms;
        }

        private void UIDelete_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            perms |= Common.Models.PermissionType.Delete;
            DataContext = perms;
        }

        private void UIDelete_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            Common.Models.PermissionType perms = (Common.Models.PermissionType)DataContext;
            if (perms.HasFlag(Common.Models.PermissionType.Delete))
                perms ^= Common.Models.PermissionType.Delete;
            DataContext = perms;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            object o = e.NewValue;
            object o2 = e.OldValue;
        }
    }
}
