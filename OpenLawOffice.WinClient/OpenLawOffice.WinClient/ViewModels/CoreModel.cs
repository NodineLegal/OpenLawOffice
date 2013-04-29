using System;

namespace OpenLawOffice.WinClient.ViewModels
{
    public abstract class CoreModel<TModel> : ModelWithDatesOnly<TModel>
        where TModel : Common.Models.Core, new()
    {
        private Security.User _createdByViewModel;
        public Security.User CreatedBy
        {
            get
            {
                return _createdByViewModel;
            }
            set
            {
                if (value == null)
                {
                    _createdByViewModel = null;
                    return;
                }

                if (_createdByViewModel == null)
                {
                    _createdByViewModel = new Security.User();
                    _createdByViewModel.AttachModel(_model.CreatedBy);
                }

                _createdByViewModel.Id = value.Id;
                _createdByViewModel.Username = value.Username;
                _createdByViewModel.Password = value.Password;
                _createdByViewModel.UserAuthToken = value.UserAuthToken;
                _createdByViewModel.UserAuthTokenExpiry = value.UserAuthTokenExpiry;
                _createdByViewModel.UtcCreated = value.UtcCreated;
                _createdByViewModel.UtcModified = value.UtcModified;
                _createdByViewModel.UtcDisabled = value.UtcDisabled;
                OnPropertyChanged("CreatedBy");
            }
        }

        private Security.User _modifiedByViewModel;
        public Security.User ModifiedBy
        {
            get { return _modifiedByViewModel; }
            set
            {
                if (value == null)
                {
                    _modifiedByViewModel = null;
                    return;
                }

                if (_modifiedByViewModel == null)
                {
                    _modifiedByViewModel = new Security.User();
                    _modifiedByViewModel.AttachModel(_model.ModifiedBy);
                }

                _modifiedByViewModel.Id = value.Id;
                _modifiedByViewModel.Username = value.Username;
                _modifiedByViewModel.Password = value.Password;
                _modifiedByViewModel.UserAuthToken = value.UserAuthToken;
                _modifiedByViewModel.UserAuthTokenExpiry = value.UserAuthTokenExpiry;
                _modifiedByViewModel.UtcCreated = value.UtcCreated;
                _modifiedByViewModel.UtcModified = value.UtcModified;
                _modifiedByViewModel.UtcDisabled = value.UtcDisabled;
                OnPropertyChanged("ModifiedBy");
            }
        }

        private Security.User _disabledByViewModel;
        public Security.User DisabledBy
        {
            get { return _disabledByViewModel; }
            set
            {
                if (value == null)
                {
                    _disabledByViewModel = null;
                    return;
                }

                if (_disabledByViewModel == null)
                {
                    _disabledByViewModel = new Security.User();
                    _disabledByViewModel.AttachModel(_model.DisabledBy);
                }

                _disabledByViewModel.Id = value.Id;
                _disabledByViewModel.Username = value.Username;
                _disabledByViewModel.Password = value.Password;
                _disabledByViewModel.UserAuthToken = value.UserAuthToken;
                _disabledByViewModel.UserAuthTokenExpiry = value.UserAuthTokenExpiry;
                _disabledByViewModel.UtcCreated = value.UtcCreated;
                _disabledByViewModel.UtcModified = value.UtcModified;
                _disabledByViewModel.UtcDisabled = value.UtcDisabled;
                OnPropertyChanged("DisabledBy");
            }
        }

        public CoreModel()
        {
        }

        public override IViewModel AttachModel(Common.Models.ModelBase model)
        {
            if (!typeof(Common.Models.Core).IsAssignableFrom(model.GetType()))
                throw new ArgumentException("Argument 'model' must inherit from Common.Models.Core");

            Common.Models.Core coreModel = (Common.Models.Core)model;
            _createdByViewModel = (ViewModels.Security.User)new Security.User().AttachModel(coreModel.CreatedBy);
            _modifiedByViewModel = (ViewModels.Security.User)new Security.User().AttachModel(coreModel.ModifiedBy);
            _disabledByViewModel = (ViewModels.Security.User)new Security.User().AttachModel(coreModel.DisabledBy);
            
            return base.AttachModel(model);
        }

        public override IViewModel AttachModel(TModel model)
        {
            Common.Models.Core coreModel = (Common.Models.Core)model;
            
            _createdByViewModel = (ViewModels.Security.User)new Security.User().AttachModel(coreModel.CreatedBy);
            _modifiedByViewModel = (ViewModels.Security.User)new Security.User().AttachModel(coreModel.ModifiedBy);
            _disabledByViewModel = (ViewModels.Security.User)new Security.User().AttachModel(coreModel.DisabledBy);
            
            return base.AttachModel(model);
        }
    }
}
