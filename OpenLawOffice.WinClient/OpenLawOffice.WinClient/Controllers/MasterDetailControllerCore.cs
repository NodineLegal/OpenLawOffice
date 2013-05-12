using System;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controllers
{
    public abstract class MasterDetailControllerCore<TMasterView, TDetailView, TEditView, TCreateView>
        : MasterDetailController<TMasterView, TDetailView, TEditView, TCreateView>
        where TMasterView : UserControl, Controls.IMaster, new()
        where TDetailView : UserControl, new()
        where TEditView : UserControl, new()
        where TCreateView : UserControl, new()
    {
        public MasterDetailControllerCore(string title, RibbonTab ribbonTab, RibbonToggleButton editButton,
            RibbonButton createButton, RibbonButton disableButton, RibbonButton saveButton, RibbonButton cancelButton)
            : base(title, ribbonTab, editButton, createButton, disableButton, saveButton, cancelButton)
        {
        }

        public virtual Task PopulateCoreDetails<TModel>(ViewModels.ViewModelCore<TModel> viewModel, Action onComplete)
            where TModel : Common.Models.Core, new()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(PopulateCreatingUser<TModel>(viewModel));
            tasks.Add(PopulateModifyingUser<TModel>(viewModel));

            if (viewModel.DisabledBy != null && viewModel.DisabledBy.Id.HasValue)
                tasks.Add(PopulateDisablingUser<TModel>(viewModel));

            return Task.Factory.ContinueWhenAll(tasks.ToArray(), data =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (onComplete != null) onComplete();
                }));
            });
        }

        public virtual Task PopulateCreatingUser<TModel>(ViewModels.ViewModelCore<TModel> viewModel)
            where TModel : Common.Models.Core, new()
        {
            return Task.Factory.StartNew(new Action(() =>
            {
                DownloadUser(new Action<ViewModels.Security.User>(userViewModel =>
                {
                    PushCreatingUserToUI(viewModel, userViewModel);
                }), viewModel.CreatedBy.Id.Value);
            }));
        }

        public virtual Task PopulateModifyingUser<TModel>(ViewModels.ViewModelCore<TModel> viewModel)
            where TModel : Common.Models.Core, new()
        {
            return Task.Factory.StartNew(new Action(() =>
            {
                DownloadUser(new Action<ViewModels.Security.User>(userViewModel =>
                {
                    PushModifyingUserToUI(viewModel, userViewModel);
                }), viewModel.ModifiedBy.Id.Value);
            }));
        }

        public virtual Task PopulateDisablingUser<TModel>(ViewModels.ViewModelCore<TModel> viewModel)
            where TModel : Common.Models.Core, new()
        {
            return Task.Factory.StartNew(new Action(() =>
            {
                DownloadUser(new Action<ViewModels.Security.User>(userViewModel =>
                {
                    PushDisablingUserToUI(viewModel, userViewModel);
                }), viewModel.DisabledBy.Id.Value);
            }));
        }

        public virtual void DownloadUser(Action<ViewModels.Security.User> onComplete, int userid)
        {
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            userConsumer.GetSingle<Common.Rest.Requests.Security.User, Common.Rest.Responses.Security.User>
                (new Common.Rest.Requests.Security.User()
            {
                AuthToken = Globals.Instance.AuthToken,
                Id = userid
            },
            result =>
            {
                if (!result.ResponseContainer.WasSuccessful)
                {
                    ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                        new ErrorHandling.ActionableError()
                        {
                            Level = ErrorHandling.LevelType.Error,
                            Title = "Error",
                            SimpleMessage = "Failed to retrieve the details about the user that created the security area.  Would you like to retry?",
                            Message = "Error: " + result.ResponseContainer.Error.Message,
                            Exception = result.ResponseContainer.Error.Exception,
                            Source = "OpenLawOffice.WinClient.Controllers.Security.Area.DownloadUser()",
                            Recover = (error, data, onFail) =>
                            {
                                DownloadUser(onComplete, userid);
                            },
                            Fail = (error, data) =>
                            {
                                onComplete(null);
                            }
                        });
                }
                else
                {
                    Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(result.Response);
                    onComplete(ViewModels.Creator.Create<ViewModels.Security.User>(sysUser));
                }
            });
        }

        public virtual void PushCreatingUserToUI<TModel>(ViewModels.ViewModelCore<TModel> viewModelToUpdate, ViewModels.Security.User viewModelToPush)
            where TModel : Common.Models.Core, new()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                viewModelToUpdate.CreatedBy = viewModelToPush;
            }));
        }

        public virtual void PushModifyingUserToUI<TModel>(ViewModels.ViewModelCore<TModel> viewModelToUpdate, ViewModels.Security.User viewModelToPush)
            where TModel : Common.Models.Core, new()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                viewModelToUpdate.ModifiedBy = viewModelToPush;
            }));
        }

        public virtual void PushDisablingUserToUI<TModel>(ViewModels.ViewModelCore<TModel> viewModelToUpdate, ViewModels.Security.User viewModelToPush)
            where TModel : Common.Models.Core, new()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                viewModelToUpdate.DisabledBy = viewModelToPush;
            }));
        }
    }
}