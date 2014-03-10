namespace OpenLawOffice.WebClient.ViewModels
{
    using AutoMapper;
    using OpenLawOffice.Common.Models;

    public class PermissionsViewModel
    {
        public bool Read { get; set; }
        public bool List { get; set; }
        public bool Create { get; set; }
        public bool Modify { get; set; }
        public bool Disable { get; set; }
        public bool Delete { get; set; }

        public static explicit operator PermissionType(PermissionsViewModel vm)
        {
            PermissionType perms = PermissionType.None;

            if (vm.Read)
                perms |= PermissionType.None;
            if (vm.List)
                perms |= PermissionType.List;
            if (vm.Create)
                perms |= PermissionType.Create;
            if (vm.Modify)
                perms |= PermissionType.Modify;
            if (vm.Disable)
                perms |= PermissionType.Disable;
            if (vm.Delete)
                perms |= PermissionType.Delete;

            return perms;
        }

        public static explicit operator int(PermissionsViewModel vm)
        {
            return (int)((PermissionType)vm);
        }


        public static explicit operator PermissionsViewModel(int value)
        {
            PermissionsViewModel vm = new PermissionsViewModel();
            PermissionType perms = (PermissionType)value;

            if (perms.HasFlag(PermissionType.Read))
                vm.Read = true;
            if (perms.HasFlag(PermissionType.List))
                vm.List = true;
            if (perms.HasFlag(PermissionType.Create))
                vm.Create = true;
            if (perms.HasFlag(PermissionType.Modify))
                vm.Modify = true;
            if (perms.HasFlag(PermissionType.Disable))
                vm.Disable = true;
            if (perms.HasFlag(PermissionType.Delete))
                vm.Delete = true;

            return vm;
        }
    }
}