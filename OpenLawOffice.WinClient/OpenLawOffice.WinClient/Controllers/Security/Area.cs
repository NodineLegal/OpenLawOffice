using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenLawOffice.WinClient.Controllers.Security
{
    [Handle(typeof(Common.Models.Security.Area))]
    public class Area 
        : MasterDetailController<Controls.TreeGridView, Views.Security.AreaDetail, 
            Views.Security.AreaEdit, Views.Security.AreaCreate>
    {
        private Consumers.Security.Area _consumer;
        private Common.Rest.Requests.Security.Area _lastRequest;
        private RestSharp.IRestResponse _lastRestSharpResponse;

        public Area()
            : base("Security Areas", 
            Globals.Instance.MainWindow.SecurityAreaTab,
            Globals.Instance.MainWindow.SecurityAreas_Edit,
            Globals.Instance.MainWindow.SecurityAreas_Create,
            Globals.Instance.MainWindow.SecurityAreas_Disable,
            Globals.Instance.MainWindow.SecurityAreas_Save,
            Globals.Instance.MainWindow.SecurityAreas_Cancel)
        {
            _consumer = new Consumers.Security.Area();
            _lastRequest = null;

            MasterDetailWindow.MasterView
                .AddResource(typeof(ViewModels.Security.Area), new System.Windows.HierarchicalDataTemplate()
                {
                    DataType = typeof(ViewModels.Security.Area),
                    ItemsSource = new System.Windows.Data.Binding("Children")
                })
                .SetExpanderColumnTemplate("Name", "Name", 225)
                .AddColumn(new System.Windows.Controls.GridViewColumn()
                {
                    Header = "Description",
                    DisplayMemberBinding = new System.Windows.Data.Binding("Description")
                    {
                        Mode = System.Windows.Data.BindingMode.TwoWay
                    },
                    Width = 200
                });
        }

        public override void LoadUI()
        {
            // ribbon controls
            MainWindow.SecurityAreas_List.Command = new Commands.DelegateCommand(x =>
            {
                string nameFilter = null;

                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    nameFilter = MainWindow.SecurityAreas_List_Name.Text.Trim();
                }));

                MasterDetailWindow.IsBusy = true;

                GetData<Common.Models.Security.Area>(data =>
                {
                    List<Common.Models.Security.Area> sysModelList =
                        (List<Common.Models.Security.Area>)data;

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        UpdateUI(null, sysModelList, Controls.DisplayModeType.View);
                        MasterDetailWindow.IsBusy = false;
                    }), null);
                }, new { Name = nameFilter });
            });

            MainWindow.SecurityAreas_Acls.Command = new Commands.DelegateCommand(x =>
            {
                ControllerManager.Instance.LoadUI<Common.Models.Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.SecurityAreas_Acls.Command);

            MainWindow.SecurityAreas_Create.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModels.Security.Area areaVM = (ViewModels.Security.Area)new ViewModels.Security.Area().AttachModel(new Common.Models.Security.Area());
                    MasterDetailWindow.GoIntoCreateMode(areaVM);
                }));
            }, x => MasterDetailWindow.CreateEnabled);
            
            MainWindow.SecurityAreas_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.UpdateDetailAndEditDataContext(MasterDetailWindow.MasterView.SelectedItem);
                }));
            }, x => MasterDetailWindow.EditEnabled);

            MainWindow.SecurityAreas_Disable.Command = new Commands.DelegateCommand(x =>
            {
                System.Windows.MessageBoxResult mbResult = System.Windows.MessageBox.Show(MainWindow,
                    "Disabling the selected item will make it unusable and prevent it from showing up in searches.  Are you sure you wish to disable the selected item?",
                    "Confirm",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question,
                    System.Windows.MessageBoxResult.No);
                if (mbResult == System.Windows.MessageBoxResult.Yes)
                {
                    DisableArea((ViewModels.Security.Area)MasterDetailWindow.MasterView.SelectedItem);
                }
            }, x => MasterDetailWindow.DisableEnabled);

            MainWindow.SecurityAreas_Save.Command = new Commands.DelegateCommand(x =>
            {
                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Edit)
                {
                    UpdateArea((ViewModels.Security.Area)MasterDetailWindow.DetailView.DataContext);
                }
                else if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                {
                    CreateArea((ViewModels.Security.Area)MasterDetailWindow.CreateView.DataContext);
                }
                else
                    throw new Exception("Invalid UI state.");
            }, x => MasterDetailWindow.SaveEnabled);

            MainWindow.SecurityAreas_Cancel.Command = new Commands.DelegateCommand(x =>
            {
                // Will need to reload the model from the server (easiest way)
                // This needs to be improved - if we want to keep loading from the server instead of 
                // doing a deep copy, then it needs to only load a single, not force the whole
                // tree to reload.
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.MasterView.ClearSelected();
                    MasterDetailWindow.Clear();
                }));

                MasterDetailWindow.IsBusy = true;

                GetData<Common.Models.Security.Area>(data =>
                {
                    List<Common.Models.Security.Area> sysModelList = (List<Common.Models.Security.Area>)data;
                    if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                        UpdateUI(null, sysModelList, Controls.DisplayModeType.View);
                    else
                        UpdateUI(null, sysModelList, null);

                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        MasterDetailWindow.IsBusy = false;
                    }));
                }, null);
            }, x => MasterDetailWindow.CancelEnabled);

            MasterDetailWindow.MasterView.ParentChanged += viewModel =>
            {
                UpdateAreaFromParentChanged((ViewModels.Security.Area)viewModel);
            };
            
            MasterDetailWindow.MasterView.OnNodeExpanded += (sender, e) =>
            {
                DW.WPFToolkit.TreeListViewItem treeItem = (DW.WPFToolkit.TreeListViewItem)e.OriginalSource;
                ViewModels.IViewModel vm = (ViewModels.IViewModel)treeItem.DataContext;

                MasterDetailWindow.IsBusy = true;

                GetData(data =>
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        UpdateUI(vm, data);
                        MasterDetailWindow.IsBusy = false;
                    }));
                }, vm);
            };

            // load window
            MasterDetailWindow.Load();
            if (!MasterDetailWindow.IsSelected)
                MasterDetailWindow.SelectWindow();

            MasterDetailWindow.IsBusy = true;

            // Ignores selection
            GetData<Common.Models.Security.Area>(data =>
            {
                List<Common.Models.Security.Area> sysModelList = (List<Common.Models.Security.Area>)data;
                UpdateUI(null, sysModelList, Controls.DisplayModeType.View);

                App.Current.Dispatcher.Invoke(new Action(() => 
                {
                    MasterDetailWindow.IsBusy = false;
                }), null);
            }, null);
        }

        private void UpdateAreaFromParentChanged(ViewModels.Security.Area viewModel)
        {
            MasterDetailWindow.IsBusy = true;

            Task.Factory.StartNew(() =>
            {
                Common.Models.Security.Area sysModel = viewModel.Model;
                Common.Rest.Requests.Security.Area requestModel = Mapper.Map<Common.Rest.Requests.Security.Area>(sysModel);
                requestModel.AuthToken = Globals.Instance.AuthToken;
                _consumer.Update(requestModel, result =>
                {
                    if (!result.ResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Failed to save changes to the security area.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = "OpenLawOffice.WinClient.Controllers.Security. Area.UpdateAreaFromParentChanged()",
                                Recover = (error, data, onFail) =>
                                {
                                    UpdateAreaFromParentChanged(viewModel);
                                }
                            });
                    }
                    else
                    {
                        sysModel = Mapper.Map<Common.Models.Security.Area>(result.Response);
                        viewModel.Synchronize(() =>
                        {
                            viewModel = (ViewModels.Security.Area)new ViewModels.Security.Area().AttachModel(sysModel);
                            MasterDetailWindow.IsBusy = false;
                        });
                    }
                });
            });
        }

        private void UpdateArea(ViewModels.Security.Area viewModel)
        {
            MasterDetailWindow.IsBusy = true;

            Task.Factory.StartNew(() =>
            {
                Common.Models.Security.Area sysModel = viewModel.Model;
                Common.Rest.Requests.Security.Area requestModel = Mapper.Map<Common.Rest.Requests.Security.Area>(sysModel);
                requestModel.AuthToken = Globals.Instance.AuthToken;
                _consumer.Update(requestModel, result =>
                {
                    if (!result.ResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Failed to save changes to the security area.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = "OpenLawOffice.WinClient.Controllers.Security.Area.UpdateArea()",
                                Recover = (error, data, onFail) =>
                                {
                                    UpdateArea(viewModel);
                                }
                            });
                    }
                    else
                    {
                        sysModel = Mapper.Map<Common.Models.Security.Area>(result.Response);
                        viewModel.Synchronize(() =>
                        {
                            viewModel = (ViewModels.Security.Area)new ViewModels.Security.Area().AttachModel(sysModel);
                            MasterDetailWindow.MasterView.ClearSelected();
                            if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                                MasterDetailWindow.SetDisplayMode(Controls.DisplayModeType.View);
                            MasterDetailWindow.IsBusy = false;
                        });
                    }
                });
            });
        }

        private void CreateArea(ViewModels.Security.Area viewModel)
        {
            MasterDetailWindow.IsBusy = true;

            Task.Factory.StartNew(() =>
            {
                Common.Models.Security.Area sysModel = viewModel.Model;
                Common.Rest.Requests.Security.Area requestModel = Mapper.Map<Common.Rest.Requests.Security.Area>(sysModel);
                requestModel.AuthToken = Globals.Instance.AuthToken;
                _consumer.Create(requestModel, result =>
                {
                    if (!result.ResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Failed to create the security area.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = "OpenLawOffice.WinClient.Controllers.Security.Area.CreateArea()",
                                Recover = (error, data, onFail) =>
                                {
                                    CreateArea(viewModel);
                                }
                            });
                    }
                    else
                    {
                        sysModel = Mapper.Map<Common.Models.Security.Area>(result.Response);
                        viewModel.Synchronize(() =>
                        {
                            viewModel = (ViewModels.Security.Area)new ViewModels.Security.Area().AttachModel(sysModel);
                            MasterDetailWindow.MasterView.ClearSelected();

                            if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                                MasterDetailWindow.SetDisplayMode(Controls.DisplayModeType.View);

                            // setup dummy child
                            viewModel.AddChild(
                                new ViewModels.Security.Area()
                                {
                                    IsDummy = true
                                });

                            // Lookup parent
                            if (viewModel.Parent == null)
                            {
                                ((EnhancedObservableCollection<ViewModels.Security.Area>)MasterDetailWindow.MasterDataContext).Add(viewModel);
                            }
                            else
                            {
                                ViewModels.Security.Area parent = Find((EnhancedObservableCollection<ViewModels.Security.Area>)MasterDetailWindow.MasterDataContext, viewModel.Parent);
                                parent.AddChild(viewModel);
                            }

                            MasterDetailWindow.IsBusy = false;
                        });
                    }
                });
            });
        }

        private void DisableArea(ViewModels.Security.Area viewModel)
        {
            MasterDetailWindow.IsBusy = true;

            Task.Factory.StartNew(() =>
            {
                Common.Models.Security.Area sysModel = viewModel.Model;
                Common.Rest.Requests.Security.Area requestModel = Mapper.Map<Common.Rest.Requests.Security.Area>(sysModel);
                requestModel.AuthToken = Globals.Instance.AuthToken;
                _consumer.Disable(requestModel, result =>
                {
                    if (!result.ResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Failed to disable the security area.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = "OpenLawOffice.WinClient.Controllers.Security.Area.DisableArea()",
                                Recover = (error, data, onFail) =>
                                {
                                    DisableArea(viewModel);
                                }
                            });
                    }
                    else
                    {
                        viewModel.Synchronize(() =>
                        {
                            if (viewModel.Parent != null)
                                viewModel.Parent.RemoveChild(viewModel);
                            else
                                ((List<ViewModels.Security.Area>)MasterDetailWindow.MasterDataContext).Remove(viewModel);

                            MasterDetailWindow.IsBusy = false;
                        });
                    }
                });
            });
        }

        private ViewModels.Security.Area Find(List<ViewModels.Security.Area> list, ViewModels.Security.Area target)
        {
            ViewModels.Security.Area found = null;

            if (!target.Id.HasValue || target.Id.Value < 1)
                throw new ArgumentException("Target must have an Id with a positive value.");

            foreach (ViewModels.Security.Area area in list)
            {
                if (area.Id == target.Id)
                    return area;

                found = Find(area.Children, target);
                if (found != null) break;
            }

            return found;
        }

        private ViewModels.Security.Area Find(EnhancedObservableCollection<ViewModels.Security.Area> list, ViewModels.Security.Area target)
        {
            ViewModels.Security.Area found = null;

            if (!target.Id.HasValue || target.Id.Value < 1)
                throw new ArgumentException("Target must have an Id with a positive value.");

            foreach (ViewModels.Security.Area area in list)
            {
                if (area.Id == target.Id)
                    return area;

                found = Find(area.Children, target);
                if (found != null) break;
            }

            return found;
        }

        public override void GetDetailData(Action onComplete, ViewModels.IViewModel viewModel)
        {
            if (typeof(ViewModels.Security.Area).IsAssignableFrom(viewModel.GetType()))
                GetDetailData(onComplete, (ViewModels.Security.Area)viewModel);
            else
                throw new ArgumentException("Argument viewModel of incorrect type.");
        }

        public void GetDetailData(Action onComplete, ViewModels.Security.Area viewModel)
        {
            List<Task> tasks = new List<Task>();

            MasterDetailWindow.DetailView.IsBusy = true;
            MasterDetailWindow.EditView.IsBusy = true;

            tasks.Add(Task.Factory.StartNew(() =>
            {
                PopulateCreatedByData(() =>
                {
                }, viewModel);
            }));

            tasks.Add(Task.Factory.StartNew(() =>
            {
                PopulateModifiedByData(() =>
                {
                }, viewModel);
            }));

            if (viewModel.DisabledBy == null || viewModel.DisabledBy.Id.HasValue)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    PopulateDisabledByData(() =>
                    {
                    }, viewModel);
                }));
            }

            Task.Factory.ContinueWhenAll(tasks.ToArray(), data =>
            {
                if (onComplete != null) onComplete();
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.DetailView.IsBusy = false;
                    MasterDetailWindow.EditView.IsBusy = false;
                }));
            });
        }

        private void PopulateCreatedByData(Action onComplete, ViewModels.Security.Area viewModel)
        {
            new Consumers.Security.User().GetSingle(
                new Common.Rest.Requests.Security.User()
                {
                    AuthToken = Globals.Instance.AuthToken,
                    Id = viewModel.CreatedBy.Id
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
                                Source = "OpenLawOffice.WinClient.Controllers.Security.Area.PopulateCreatedByData()",
                                Recover = (error, data, onFail) =>
                                {
                                    PopulateCreatedByData(onComplete, viewModel);
                                },
                                Fail = (error, data) =>
                                {
                                    onComplete();
                                }
                            });
                    }
                    else
                    {
                        Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(result.Response);
                        viewModel.CreatedBy = (ViewModels.Security.User)new ViewModels.Security.User().AttachModel(sysUser);
                        onComplete();
                    }
                });
        }

        private void PopulateModifiedByData(Action onComplete, ViewModels.Security.Area viewModel)
        {
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            userConsumer.GetSingle(
                new Common.Rest.Requests.Security.User()
                {
                    AuthToken = Globals.Instance.AuthToken,
                    Id = viewModel.ModifiedBy.Id
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
                                SimpleMessage = "Failed to retrieve the details about the user that modified the security area.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = "OpenLawOffice.WinClient.Controllers.Security.Area.PopulateModifiedByData()",
                                Recover = (error, data, onFail) =>
                                {
                                    PopulateModifiedByData(onComplete, viewModel);
                                },
                                Fail = (error, data) =>
                                {
                                    onComplete();
                                }
                            });
                    }
                    else
                    {
                        Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(result.Response);
                        viewModel.ModifiedBy = (ViewModels.Security.User)new ViewModels.Security.User().AttachModel(sysUser);
                        onComplete();
                    }
                });
        }

        private void PopulateDisabledByData(Action onComplete, ViewModels.Security.Area viewModel)
        {
            Consumers.Security.User userConsumer = new Consumers.Security.User();

            userConsumer.GetSingle(
                new Common.Rest.Requests.Security.User()
                {
                    AuthToken = Globals.Instance.AuthToken,
                    Id = viewModel.DisabledBy.Id
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
                                SimpleMessage = "Failed to retrieve the details about the user that disabled the security area.  Would you like to retry?",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = "OpenLawOffice.WinClient.Controllers.Security.Area.PopulateDisabledByData()",
                                Recover = (error, data, onFail) =>
                                {
                                    PopulateDisabledByData(onComplete, viewModel);
                                },
                                Fail = (error, data) =>
                                {
                                    onComplete();
                                }
                            });
                    }
                    else
                    {
                        Common.Models.Security.User sysUser = Mapper.Map<Common.Models.Security.User>(result.Response);
                        viewModel.DisabledBy = (ViewModels.Security.User)new ViewModels.Security.User().AttachModel(sysUser);
                        onComplete();
                    }
                });
        }

        public override void GetData<TModel>(Action<object> onComplete, object obj)
        {
            Common.Rest.Requests.Security.Area request;
            Type requestType = typeof(Common.Rest.Requests.Security.Area);
            Dictionary<string, PropertyInfo> requestPropertyDict = new Dictionary<string,PropertyInfo>();
            
            request = new Common.Rest.Requests.Security.Area()
            {
                AuthToken = Globals.Instance.AuthToken
            };

            if (obj != null)
            {
                foreach (PropertyInfo prop in requestType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
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

            GetData(onComplete, request);
        }

        public override void GetData(Action<object> onComplete, ViewModels.IViewModel filter = null)
        {
            ViewModels.Security.Area areaVM = null;
            Common.Rest.Requests.Security.Area request;
            
            request = new Common.Rest.Requests.Security.Area()
            {
                AuthToken = Globals.Instance.AuthToken
            };
            
            if (filter != null && filter.GetType() == typeof(ViewModels.Security.Area))
            {
                areaVM = (ViewModels.Security.Area)filter;
                if (areaVM != null && areaVM.Id.HasValue)
                    request.ParentId = areaVM.Id.Value;
            }

            GetData(onComplete, request);
        }

        private void GetData(Action<object> onComplete, Common.Rest.Requests.Security.Area request)
        {
            Task.Factory.StartNew(() =>
            {
                _consumer.GetList(request,
                result =>
                {
                    // Put the last request updating here, while it could go outside the callback,
                    // I am putting it in here to be certain we never have a race condition
                    _lastRequest = result.Request;
                    _lastRestSharpResponse = result.RestSharpResponse;

                    List<Common.Models.Security.Area> sysModelList = new List<Common.Models.Security.Area>();

                    if (!result.ListResponseContainer.WasSuccessful)
                    {
                        ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                            new ErrorHandling.ActionableError()
                            {
                                Level = ErrorHandling.LevelType.Error,
                                Title = "Error",
                                SimpleMessage = "Failed to retrieve the list of security areas.  Would you like to retry?",
                                Message = "Error: " + result.ListResponseContainer.Error.Message,
                                Exception = result.ListResponseContainer.Error.Exception,
                                Source = "OpenLawOffice.WinClient.Controllers.Security.Area.GetData()",
                                Recover = (error, data, onFail) =>
                                {
                                    GetData(onComplete, request);
                                },
                                Fail = (error, data) =>
                                {
                                    if (onComplete != null) onComplete(sysModelList);
                                }
                            });
                    }
                    else
                    {
                        foreach (Common.Rest.Responses.Security.Area area in result.Response)
                        {
                            sysModelList.Add(Mapper.Map<Common.Models.Security.Area>(area));
                        }

                        if (onComplete != null) onComplete(sysModelList);
                    }
                });
            });
        }

        public override void UpdateUI(ViewModels.IViewModel viewModel, object data, Controls.DisplayModeType? displayMode = null)
        {
            if (!typeof(ViewModels.Security.Area).IsAssignableFrom(viewModel.GetType()))
                throw new ArgumentException("Invalid ViewModel type.");
            if (!typeof(List<Common.Models.Security.Area>).IsAssignableFrom(data.GetType()))
                throw new ArgumentException("Invalid data type.");

            UpdateUI((ViewModels.Security.Area)viewModel,
                (List<Common.Models.Security.Area>)data, displayMode);
        }

        public void UpdateUI(ViewModels.Security.Area viewModel, 
            List<Common.Models.Security.Area> sysModelList, Controls.DisplayModeType? displayMode = null)
        {
            bool pushToDataContext = false;
            EnhancedObservableCollection<ViewModels.Security.Area> viewModels;
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (viewModel != null)
                {
                    viewModels = viewModel.Children;
                    viewModels.Clear();
                }
                else
                {
                    viewModels = new EnhancedObservableCollection<ViewModels.Security.Area>();
                    pushToDataContext = true;
                }
            
                foreach (Common.Models.Security.Area sysModel in sysModelList)
                {
                    ViewModels.Security.Area childVM = new ViewModels.Security.Area();
                    childVM.AttachModel(sysModel);

                    if (viewModel != null)
                    {
                        sysModel.Parent = viewModel.Model;

                        // Set child's parent
                        childVM.Parent = viewModel;

                        // Add parent's child
                        viewModels.Add(childVM);
                    }
                    else
                    {
                        viewModels.Add(childVM);
                    }

                    childVM.AddChild(new ViewModels.Security.Area()
                    {
                        IsDummy = true
                    });
                }

                if (pushToDataContext)
                {
                    MasterDetailWindow.Clear();
                    MasterDetailWindow.UpdateMasterDataContext(viewModels);
                    if (displayMode.HasValue)
                        MasterDetailWindow.SetDisplayMode(displayMode.Value);
                    MasterDetailWindow.UpdateCommandStates();
                }
            }));
        }
    }
}
