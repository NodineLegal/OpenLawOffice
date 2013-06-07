using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AutoMapper;
using System.Reflection;
using System.Linq;

namespace OpenLawOffice.WinClient.Controllers
{
    public abstract class ControllerBase
    {
        public abstract Type ModelType { get; }
        public abstract Type ViewModelType { get; }
        public abstract Type RequestType { get; }
        public abstract Type ResponseType { get; }

        protected Consumers.ConsumerBase _consumer;
        public MainWindow MainWindow = Globals.Instance.MainWindow;

        public abstract void LoadUI(ViewModels.IViewModel selected);
        public abstract void LoadUI();

        public abstract Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel> onComplete);
        
        public virtual Task LoadItems(ViewModels.IViewModel filter, ICollection<ViewModels.IViewModel> collection, Action<ICollection<ViewModels.IViewModel>> onComplete)
        {
            return ListItems(filter, results =>
            {
                collection.Clear();
                foreach (ViewModels.IViewModel viewModel in results)
                {
                    collection.Add(viewModel);
                }
                if (onComplete != null) onComplete(collection);
            });
        }

        public abstract Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete);

        public virtual Task UpdateItem(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel> onComplete)
        {
            viewModel.UpdateModel();
            Common.Models.ModelBase sysModel = viewModel.GetModel();
            Common.Rest.Requests.RequestBase request = (Common.Rest.Requests.RequestBase)Mapper.Map(sysModel, sysModel.GetType(), RequestType);
            request.AuthToken = Globals.Instance.AuthToken;
            return UpdateItem(request, onComplete);
        }
        public virtual Task UpdateItem<TRequest, TResponse>(TRequest request, Action<ViewModels.IViewModel> onComplete)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            return Task.Factory.StartNew(() =>
            {
                _consumer.Update<TRequest, TResponse>(request, result =>
                {
                    Common.Models.ModelBase sysModel = null;
                    ViewModels.IViewModel viewModel = null;
                    if (!result.ResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Save failed.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = this.GetType().FullName + "UpdateItem<TRequest, TResponse>(TRequest, Action<ViewModels.IViewModel>)",
                                Recover = (error, data, onFail) =>
                                {
                                    UpdateItem(request, onComplete);
                                }
                            });
                    }
                    else
                    {
                        sysModel = (Common.Models.ModelBase)Mapper.Map(result.Response, result.Response.GetType(), ModelType);
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            viewModel = ViewModels.Creator.Create(sysModel, ViewModelType);
                            if (onComplete != null) onComplete(viewModel);
                        }));
                    }
                });
            });
        }


        public virtual Task UpdateItem(object filter, Action<ViewModels.IViewModel> onComplete)
        {
            Common.Rest.Requests.RequestBase request = BuildRequestFromAnonymousObject(filter);
            return UpdateItem(request, onComplete);
        }

        public virtual Task CreateItem<TRequest, TResponse>(TRequest request, Action<ViewModels.IViewModel> onComplete)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            return Task.Factory.StartNew(() =>
            {
                _consumer.Create<TRequest, TResponse>(request, result =>
                {
                    Common.Models.ModelBase sysModel = null;
                    ViewModels.IViewModel viewModel = null;
                    if (!result.ResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Creation failed.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = this.GetType().FullName + "CreateItem<TRequest, TResponse>(TRequest, Action<ViewModels.IViewModel>)",
                                Recover = (error, data, onFail) =>
                                {
                                    CreateItem(request, onComplete);
                                }
                            });
                    }
                    else
                    {
                        sysModel = (Common.Models.ModelBase)Mapper.Map(result.Response, result.Response.GetType(), ModelType);
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            viewModel = ViewModels.Creator.Create(sysModel, ViewModelType);
                            if (onComplete != null) onComplete(viewModel);
                        }));
                    }
                });
            });
        }

        public abstract Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete);
        
        public virtual Task CreateItem(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel> onComplete)
        {
            viewModel.UpdateModel();
            Common.Models.ModelBase sysModel = viewModel.GetModel();
            Common.Rest.Requests.RequestBase request = (Common.Rest.Requests.RequestBase)Mapper.Map(sysModel, sysModel.GetType(), RequestType);
            request.AuthToken = Globals.Instance.AuthToken;
            return CreateItem(request, onComplete);
        }

        public virtual Task CreateItem(object filter, Action<ViewModels.IViewModel> onComplete)
        {
            Common.Rest.Requests.RequestBase request = BuildRequestFromAnonymousObject(filter);
            return CreateItem(request, onComplete);
        }

        public virtual Task DisableItem<TRequest, TResponse>(TRequest request, Action<ViewModels.IViewModel> onComplete)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            return Task.Factory.StartNew(() =>
            {
                _consumer.Disable<TRequest, TResponse>(request, result =>
                {
                    Common.Models.ModelBase sysModel = null;
                    ViewModels.IViewModel viewModel = null;
                    if (!result.ResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Disable failed.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = this.GetType().FullName + "DisableItem<TRequest, TResponse>(TRequest, Action<ViewModels.IViewModel>)",
                                Recover = (error, data, onFail) =>
                                {
                                    DisableItem(request, onComplete);
                                }
                            });
                    }
                    else
                    {
                        sysModel = (Common.Models.ModelBase)Mapper.Map(result.Response, result.Response.GetType(), ModelType);
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            viewModel = ViewModels.Creator.Create(sysModel, ViewModelType);
                            if (onComplete != null) onComplete(viewModel);
                        }));
                    }
                });
            });
        }

        public abstract Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete);

        public virtual Task DisableItem(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel> onComplete)
        {
            viewModel.UpdateModel();
            Common.Models.ModelBase sysModel = viewModel.GetModel();
            Common.Rest.Requests.RequestBase request = (Common.Rest.Requests.RequestBase)Mapper.Map(sysModel, sysModel.GetType(), RequestType);
            request.AuthToken = Globals.Instance.AuthToken;
            return DisableItem(request, onComplete);
        }

        public virtual Task DisableItem(object filter, Action<ViewModels.IViewModel> onComplete)
        {
            Common.Rest.Requests.RequestBase request = BuildRequestFromAnonymousObject(filter);
            return DisableItem(request, onComplete);
        }

        public abstract Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>> onComplete);

        public virtual Task ListItems<TRequest, TResponse>(TRequest request, Action<List<ViewModels.IViewModel>> onComplete)
            where TRequest : Common.Rest.Requests.RequestBase
            where TResponse : Common.Rest.Responses.ResponseBase
        {
            return Task.Factory.StartNew(() =>
            {
                _consumer.GetList<TRequest, TResponse>(request, result =>
                {
                    if (!result.ListResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Fetching data failed.  Would you like to retry?",
                                Message = "Error: " + result.ListResponseContainer.Error.Message,
                                Exception = result.ListResponseContainer.Error.Exception,
                                Source = this.GetType().FullName + "ListItems(Common.Rest.Requests.RequestBase, Action<List<ViewModels.IViewModel>>)",
                                Recover = (error, data, onFail) =>
                                {
                                    ListItems(request, onComplete);
                                }
                            });
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            Common.Models.ModelBase sysModel;
                            ViewModels.IViewModel viewModel;
                            List<ViewModels.IViewModel> viewModels = new List<ViewModels.IViewModel>();
                            foreach (Common.Rest.Responses.ResponseBase response in result.ListResponseContainer.Data)
                            {
                                sysModel = (Common.Models.ModelBase)Mapper.Map(response, response.GetType(), ModelType);
                                viewModel = ViewModels.Creator.Create(sysModel, ViewModelType);
                                viewModels.Add(viewModel);
                            }

                            if (onComplete != null) 
                                onComplete(viewModels);
                        }));
                    }
                });
            });
        }

        public virtual Task ListItems(ViewModels.IViewModel filter, Action<List<ViewModels.IViewModel>> onComplete)
        {
            filter.UpdateModel();
            Common.Models.ModelBase sysModel = filter.GetModel();
            Common.Rest.Requests.RequestBase request = (Common.Rest.Requests.RequestBase)Mapper.Map(sysModel, sysModel.GetType(), RequestType);
            request.AuthToken = Globals.Instance.AuthToken;
            return ListItems(request, onComplete);
        }
        
        public virtual Task ListItems(object filter, Action<List<ViewModels.IViewModel>> onComplete)
        {
            Common.Rest.Requests.RequestBase request = BuildRequestFromAnonymousObject(filter);
            return ListItems(request, onComplete);
        }

        public abstract void SelectItem(ViewModels.IViewModel viewModel);
        public abstract void SetDisplayMode(Controls.DisplayModeType mode);

        protected virtual Common.Rest.Requests.RequestBase BuildRequestFromAnonymousObject(object obj)
        {
            Dictionary<string, PropertyInfo> requestPropertyDict = new Dictionary<string, PropertyInfo>();

            ConstructorInfo ci = RequestType.GetConstructor(new Type[] { });
            if (ci == null)
                throw new TargetException("Must have a public parameterless constructor.");

            Common.Rest.Requests.RequestBase request = (Common.Rest.Requests.RequestBase)ci.Invoke(null);
            request.AuthToken = Globals.Instance.AuthToken;

            if (obj != null)
            {
                // Cycle each property in the request class adding to the dict
                foreach (PropertyInfo prop in RequestType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    requestPropertyDict.Add(prop.Name, prop);
                }

                foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (!requestPropertyDict.ContainsKey(prop.Name))
                        throw new ArgumentException("Property named '" + prop.Name + "' does not exist in the request object.");

                    // Sets the value of the argument "obj.[property name]" as the value of "request.[property name]"
                    requestPropertyDict[prop.Name].SetValue(request, prop.GetValue(obj, null), null);
                }
            }

            return request;
        }

        protected static ObservableCollection<TTo> CastObservableCollection<TTo>(object from)
            where TTo : ViewModels.IViewModel
        {
            if (!IsAssignableToGenericType(from.GetType(), typeof(ObservableCollection<>)))
                throw new ArgumentException("From must be of type ObservableCollection<>");

            ObservableCollection<ViewModels.IViewModel> a = (ObservableCollection<ViewModels.IViewModel>)from;
            IEnumerable<TTo> b = a.Cast<TTo>();
            return new ObservableCollection<TTo>(b);
        }

        protected static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}
