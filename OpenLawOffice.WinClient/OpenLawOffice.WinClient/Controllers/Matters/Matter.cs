using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenLawOffice.WinClient.Controllers.Matters
{
    [Handle(typeof(Common.Models.Matters.Matter))]
    public class Matter
        : MasterDetailControllerCore<Controls.ListGridView, Views.Matters.MatterDetail, 
            Views.Matters.MatterEdit, Views.Matters.MatterCreate>
    {
        public override Type RequestType { get { return typeof(Common.Rest.Requests.Matters.Matter); } }
        public override Type ResponseType { get { return typeof(Common.Rest.Responses.Matters.Matter); } }
        public override Type ViewModelType { get { return typeof(ViewModels.Matters.Matter); } }
        public override Type ModelType { get { return typeof(Common.Models.Matters.Matter); } }

        public Matter()
            : base("Matters",
            Globals.Instance.MainWindow.MattersTab,
            Globals.Instance.MainWindow.Matters_Edit,
            Globals.Instance.MainWindow.Matters_Create,
            Globals.Instance.MainWindow.Matters_Disable,
            Globals.Instance.MainWindow.Matters_Save,
            Globals.Instance.MainWindow.Matters_Cancel)
        {
            _consumer = new Consumers.Matters.Matter();

            MasterDetailWindow.MasterView.AddColumn(new System.Windows.Controls.GridViewColumn()
            {
                Header = "Title",
                DisplayMemberBinding = new System.Windows.Data.Binding("Title")
                {
                    Mode = System.Windows.Data.BindingMode.TwoWay
                },
                Width = 200
            }).AddColumn(new System.Windows.Controls.GridViewColumn()
            {
                Header = "Synopsis",
                DisplayMemberBinding = new System.Windows.Data.Binding("Synopsis")
                {
                    Mode = System.Windows.Data.BindingMode.TwoWay
                },
                Width = 300
            });

            Views.Matters.ResponsibleUserRelation respUserRelation = new Views.Matters.ResponsibleUserRelation();
            respUserRelation.OnClose += iwin =>
            {
                MasterDetailWindow.HideRelationView();
            };
            respUserRelation.OnEdit += iwin =>
            {
                ViewModels.Matters.ResponsibleUser itemToView = (ViewModels.Matters.ResponsibleUser)respUserRelation.GetSelectedItem();
                ControllerManager.Instance.LoadUI<Common.Models.Matters.ResponsibleUser>(itemToView, () =>
                {
                    ControllerManager.Instance.SetDisplayMode<Common.Models.Matters.ResponsibleUser>(Controls.DisplayModeType.Edit);
                });                
            };
            respUserRelation.OnView += iwin =>
            {
                ViewModels.Matters.ResponsibleUser itemToView = (ViewModels.Matters.ResponsibleUser)respUserRelation.GetSelectedItem();
                ControllerManager.Instance.LoadUI<Common.Models.Matters.ResponsibleUser>(itemToView);
                ControllerManager.Instance.SetDisplayMode<Common.Models.Matters.ResponsibleUser>(Controls.DisplayModeType.View);
            };
            respUserRelation.OnAdd += iwin =>
            {
                ViewModels.Matters.Matter matter = iwin.ViewModel;
                ViewModels.Matters.ResponsibleUser itemToView = ViewModels.Creator.Create<ViewModels.Matters.ResponsibleUser>(
                    new Common.Models.Matters.ResponsibleUser()
                    {
                        Matter = matter.Model
                    });
                ControllerManager.Instance.LoadUI<Common.Models.Matters.ResponsibleUser>(itemToView, () =>
                {
                    ControllerManager.Instance.GoIntoCreateMode<Common.Models.Matters.ResponsibleUser>(itemToView);
                });
            };
            MasterDetailWindow.AddRelationView<Common.Models.Matters.Matter>(respUserRelation);

            Views.Security.SecuredResourceAclRelation resourcePerms = new Views.Security.SecuredResourceAclRelation();
            resourcePerms.OnClose += iwin =>
            {
                MasterDetailWindow.HideRelationView();
            };
            resourcePerms.OnEdit += iwin =>
            {
                ViewModels.Security.SecuredResourceAcl itemToView = 
                    (ViewModels.Security.SecuredResourceAcl)respUserRelation.GetSelectedItem();
                ControllerManager.Instance.LoadUI<Common.Models.Security.SecuredResourceAcl>(itemToView, () =>
                {
                    ControllerManager.Instance.SetDisplayMode<Common.Models.Security.SecuredResourceAcl>(Controls.DisplayModeType.Edit);
                });
            };
            resourcePerms.OnView += iwin =>
            {
                ViewModels.Security.SecuredResourceAcl itemToView = (ViewModels.Security.SecuredResourceAcl)respUserRelation.GetSelectedItem();
                ControllerManager.Instance.LoadUI<Common.Models.Security.SecuredResourceAcl>(itemToView);
                ControllerManager.Instance.SetDisplayMode<Common.Models.Security.SecuredResourceAcl>(Controls.DisplayModeType.View);
            };
            resourcePerms.OnAdd += iwin =>
            {
                ViewModels.Matters.Matter matter = iwin.ViewModel;
                ViewModels.Security.SecuredResourceAcl itemToView = ViewModels.Creator.Create<ViewModels.Security.SecuredResourceAcl>(
                    new Common.Models.Security.SecuredResourceAcl()
                    {
                        SecuredResource = new Common.Models.Security.SecuredResource()
                        {
                            Id = matter.Id
                        }
                    });
                ControllerManager.Instance.LoadUI<Common.Models.Security.SecuredResourceAcl>(itemToView, () =>
                {
                    ControllerManager.Instance.GoIntoCreateMode<Common.Models.Security.SecuredResourceAcl>(itemToView);
                });
            };
            MasterDetailWindow.AddRelationView<Common.Models.Security.SecuredResourceAcl>(resourcePerms);
        }

        private ViewModels.Matters.Matter BuildFilter()
        {
            ViewModels.Matters.Matter filter = ViewModels.Creator.Create<ViewModels.Matters.Matter>(new Common.Models.Matters.Matter());

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                string nameFilter = MainWindow.Matters_List_Title.Text.Trim();
                if (!string.IsNullOrEmpty(nameFilter))
                    filter.Title = nameFilter;
            }));

            return filter;
        }

        public override void LoadUI(ViewModels.IViewModel selected, Action callback = null)
        {
            ObservableCollection<ViewModels.IViewModel> viewModelCollection = null;

            // ribbon controls
            MainWindow.Matters_List.Command = new Commands.DelegateCommand(x =>
            {
                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                {
                    MasterDetailWindow.MasterDataContext = results;
                });
            });

            MainWindow.Matters_ResponsibleUsers.Command = new Commands.DelegateCommand(x =>
            {
                MasterDetailWindow.ShowRelationView<Common.Models.Matters.Matter>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.Matters_ResponsibleUsers.Command);

            MainWindow.Matters_Acls.Command = new Commands.DelegateCommand(x =>
            {
                MasterDetailWindow.ShowRelationView<Common.Models.Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.Matters_Acls.Command);

            MainWindow.Matters_SecuredResourceAcls.Command = new Commands.DelegateCommand(x =>
            {
                MasterDetailWindow.ShowRelationView<Common.Models.Security.SecuredResourceAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.Matters_SecuredResourceAcls.Command);

            MainWindow.Matters_Contacts.Command = new Commands.DelegateCommand(x =>
            {
                //MasterDetailWindow.ShowRelationView<Common.Models. .Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.Matters_Contacts.Command);

            MainWindow.Matters_Tasks.Command = new Commands.DelegateCommand(x =>
            {
                //MasterDetailWindow.ShowRelationView<Common.Models. .Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.Matters_Tasks.Command);

            MainWindow.Matters_Documents.Command = new Commands.DelegateCommand(x =>
            {
                //MasterDetailWindow.ShowRelationView<Common.Models. .Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.Matters_Documents.Command);

            MainWindow.Matters_Notes.Command = new Commands.DelegateCommand(x =>
            {
                //MasterDetailWindow.ShowRelationView<Common.Models. .Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.Matters_Notes.Command);
            
            MainWindow.Matters_Create.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModels.Matters.Matter viewModel = ViewModels.Creator.Create<ViewModels.Matters.Matter>(new Common.Models.Matters.Matter());
                    MasterDetailWindow.GoIntoCreateMode(viewModel);
                }));
            }, x => MasterDetailWindow.CreateEnabled);

            MainWindow.Matters_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.UpdateDetailViewDataContext(MasterDetailWindow.MasterView.SelectedItem);
                }));
            }, x => MasterDetailWindow.EditEnabled);

            MainWindow.Matters_Disable.Command = new Commands.DelegateCommand(x =>
            {
                System.Windows.MessageBoxResult mbResult = System.Windows.MessageBox.Show(MainWindow,
                    "Disabling the selected item will make it unusable and prevent it from showing up in searches.  Are you sure you wish to disable the selected item?",
                    "Confirm",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question,
                    System.Windows.MessageBoxResult.No);
                if (mbResult == System.Windows.MessageBoxResult.Yes)
                {
                    DisableItem((ViewModels.Matters.Matter)MasterDetailWindow.MasterView.SelectedItem, null);
                }
            }, x => MasterDetailWindow.DisableEnabled);

            MainWindow.Matters_Save.Command = new Commands.DelegateCommand(x =>
            {
                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Edit)
                {
                    UpdateItem((ViewModels.Matters.Matter)MasterDetailWindow.DetailView.DataContext, null);
                }
                else if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                {
                    CreateItem((ViewModels.Matters.Matter)MasterDetailWindow.CreateView.DataContext, null);
                }
                else
                    throw new Exception("Invalid UI state.");
            }, x => 
                { 
                    System.Diagnostics.Debug.WriteLine(MasterDetailWindow.CreateEnabled); 
                    return MasterDetailWindow.SaveEnabled; 
                });

            MainWindow.Matters_Cancel.Command = new Commands.DelegateCommand(x =>
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

                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

                LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
                {
                    MasterDetailWindow.MasterDataContext = results;
                    if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                        MasterDetailWindow.SetDisplayMode(Controls.DisplayModeType.View);
                });
            }, x => MasterDetailWindow.CancelEnabled);

            // load window
            MasterDetailWindow.Load();
            if (!MasterDetailWindow.IsSelected)
                MasterDetailWindow.SelectWindow();

            viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

            LoadItems(BuildFilter(), viewModelCollection, (results, error) =>
            {
                MasterDetailWindow.MasterDataContext = results;
                if (selected != null) SelectItem(selected);
                if (callback != null) callback();
            });

            MasterDetailWindow.EditView.TagBox.OnCancel += (tagbox, tagcat) =>
            {
                ((ViewModels.Matters.MatterTag)tagcat.DataContext).Tag = tagcat.PreviousText;
            };

            MasterDetailWindow.EditView.TagBox.OnDelete += (tagbox, tagcat) =>
            {
                DisableTag((ViewModels.Matters.MatterTag)tagcat.DataContext, () =>
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ViewModels.Matters.Matter matter = (ViewModels.Matters.Matter)MasterDetailWindow.DetailDataContext;
                        RefreshTags(matter);
                    }));
                });
            };

            MasterDetailWindow.EditView.TagBox.OnSave += (tagbox, tagcat) =>
            {
                SaveTag((ViewModels.Matters.MatterTag)tagcat.DataContext, () =>
                {
                    App.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ViewModels.Matters.Matter matter = (ViewModels.Matters.Matter)MasterDetailWindow.DetailDataContext;
                        RefreshTags(matter);
                    }));
                });
            };

            MasterDetailWindow.EditView.TagBox.OnAdd += (tagbox, tagcat, tag) =>
            {
                ViewModels.Matters.Matter matter = (ViewModels.Matters.Matter)MasterDetailWindow.DetailDataContext;

                ViewModels.Matters.MatterTag viewModel = 
                    ViewModels.Creator.Create<ViewModels.Matters.MatterTag>(
                    new Common.Models.Matters.MatterTag());

                if (tagcat != null && tagcat.Id > 0)
                    viewModel.TagCategory = tagcat;

                viewModel.Matter = matter;
                viewModel.Tag = tag;

                CreateTag(viewModel, () =>
                {
                    RefreshTags(matter);
                });
            };
        }

        public override Task LoadItems(ViewModels.IViewModel filter, ICollection<ViewModels.IViewModel> collection, Action<ICollection<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            return base.LoadItems(filter, collection, (results, error) =>
            {
                if (onComplete != null)
                    onComplete(results, error);
            });
        }

        public override Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                MasterDetailWindow.DetailView.IsBusy = true;
                MasterDetailWindow.EditView.IsBusy = true;
            }));

            return Task.Factory.StartNew(new Action(() =>
            {
                Task taskTags, taskCore;
                ViewModels.Matters.Matter castViewModel = (ViewModels.Matters.Matter)viewModel;

                Consumers.ConsumerResult<Common.Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter>
                    detailResult = _consumer.GetSingle<Common.Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter>(
                    new Common.Rest.Requests.Matters.Matter()
                    {
                        AuthToken = Globals.Instance.AuthToken,
                        Id = castViewModel.Id
                    });

                if (!detailResult.ResponseContainer.WasSuccessful)
                {
                    ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                        new ErrorHandling.ActionableError()
                        {
                            Level = ErrorHandling.LevelType.Error,
                            Title = "Error",
                            SimpleMessage = "Fetching data failed.  Would you like to retry?",
                            Message = "Error: " + detailResult.ResponseContainer.Error.Message,
                            Exception = detailResult.ResponseContainer.Error.Exception,
                            Source = this.GetType().FullName + "ListItems(Common.Rest.Requests.RequestBase, Action<List<ViewModels.IViewModel>>)",
                            Recover = (error, data, onFail) =>
                            {
                            },
                            Fail = (error, data) =>
                            {
                                App.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    if (onComplete != null)
                                        onComplete(null, error);
                                }));
                            }
                        });
                    return;
                }

                // Replace
                Common.Models.Matters.Matter replacementModel = Mapper.Map<Common.Models.Matters.Matter>(detailResult.ResponseContainer.Data);
                castViewModel.Bind(replacementModel);

                // Load Tags
                taskTags = LoadTags(castViewModel, result =>
                {
                    castViewModel = result;
                });

                taskCore = PopulateCoreDetails<Common.Models.Matters.Matter>(castViewModel, () =>
                {
                    MasterDetailWindow.DetailView.IsBusy = false;
                    MasterDetailWindow.EditView.IsBusy = false;
                });

                Task.Factory.ContinueWhenAll(new Task[] { taskTags, taskCore }, data =>
                {
                    if (onComplete != null) onComplete(castViewModel, null);
                });
            }));
        }

        private Task LoadTagCategory(ViewModels.Tagging.TagCategory viewModel, Action<ViewModels.Tagging.TagCategory> onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
                Consumers.Tagging.TagCategory tagCatConsumer = new Consumers.Tagging.TagCategory();

                tagCatConsumer.GetSingle<Common.Rest.Requests.Tagging.TagCategory, Common.Rest.Responses.Tagging.TagCategory>(
                new Common.Rest.Requests.Tagging.TagCategory()
                {
                    AuthToken = Globals.Instance.AuthToken,
                    Id = viewModel.Id
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
                                SimpleMessage = "Fetching tag category data failed.",
                                Message = "Error: " + result.ResponseContainer.Error.Message,
                                Exception = result.ResponseContainer.Error.Exception,
                                Source = this.GetType().FullName + "LoadTagCategory(ViewModels.Tagging.TagCategory)",
                                Fail = (error, data) =>
                                {
                                    onComplete(null);
                                }
                            });
                    }
                    else
                    {
                        Common.Models.Tagging.TagCategory tagCatModel = Mapper.Map<Common.Models.Tagging.TagCategory>(result.ResponseContainer.Data);
                        viewModel = ViewModels.Creator.Create<ViewModels.Tagging.TagCategory>(tagCatModel);
                        onComplete(viewModel);
                    }
                });
            }));
        }

        public void GetTagCategories(Action<List<ViewModels.Tagging.TagCategory>> onComplete)
        {
            Consumers.Tagging.TagCategory tagCatConsumer = new Consumers.Tagging.TagCategory();

            tagCatConsumer.GetList<Common.Rest.Requests.Tagging.TagCategory, Common.Rest.Responses.Tagging.TagCategory>(
            new Common.Rest.Requests.Tagging.TagCategory()
            {
                AuthToken = Globals.Instance.AuthToken
            },
            result =>
            {
                if (!result.ListResponseContainer.WasSuccessful)
                {
                    ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                        new ErrorHandling.ActionableError()
                        {
                            Level = ErrorHandling.LevelType.Error,
                            Title = "Error",
                            SimpleMessage = "Fetching tag categories failed.",
                            Message = "Error: " + result.ListResponseContainer.Error.Message,
                            Exception = result.ListResponseContainer.Error.Exception,
                            Source = this.GetType().FullName + "LoadTagCategory(ViewModels.Tagging.TagCategory)",
                            Recover = (error, data, onFail) =>
                            {
                                GetTagCategories(onComplete);
                            },
                            Fail = (error, data) =>
                            {
                            }
                        });
                }
                else
                {
                    List<ViewModels.Tagging.TagCategory> list = new List<ViewModels.Tagging.TagCategory>();
                    result.ListResponseContainer.Data.ForEach(cat =>
                    {
                        list.Add(
                            ViewModels.Creator.Create<ViewModels.Tagging.TagCategory>(
                                Mapper.Map<Common.Models.Tagging.TagCategory>(cat)));

                    });
                    onComplete(list);
                }
            });
        }

        private Task LoadTags(ViewModels.Matters.Matter viewModel, Action<ViewModels.Matters.Matter> onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
                Consumers.Matters.MatterTag tagConsumer = new Consumers.Matters.MatterTag();
                Common.Rest.Requests.Matters.MatterTag tagRequest = new Common.Rest.Requests.Matters.MatterTag()
                {
                    AuthToken = Globals.Instance.AuthToken,
                    MatterId = viewModel.Id
                };

                tagConsumer.GetList<Common.Rest.Requests.Matters.MatterTag, Common.Rest.Responses.Matters.MatterTag>
                    (tagRequest,
                    result =>
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
                                    Source = this.GetType().FullName + "LoadTags(ViewModels.Matters.Matter, Action<ViewModels.Matters.Matter>)",
                                    Recover = (error, data, onFail) =>
                                    {
                                        LoadTags(viewModel, onComplete);
                                    }
                                });
                        }
                        else
                        {
                            List<Task> catFetchTasks = new List<Task>();

                            List<ViewModels.Matters.MatterTag> tagVMs = new List<ViewModels.Matters.MatterTag>();
                        
                            //ViewModels.Matters.MatterTag
                            result.ListResponseContainer.Data
                                .ForEach(tag =>
                                {
                                    ViewModels.Matters.MatterTag tagVM = 
                                        ViewModels.Creator.Create<ViewModels.Matters.MatterTag>(Mapper.Map<Common.Models.Matters.MatterTag>(tag));
                                    tagVMs.Add(tagVM);
                                    if (tagVM.TagCategory != null)
                                    {
                                        catFetchTasks.Add(LoadTagCategory(tagVM.TagCategory,
                                            loadCatResult =>
                                            {
                                                tagVM.TagCategory = loadCatResult;
                                            }));
                                    }
                                });

                            if (result.ListResponseContainer.Data.Count > 0)
                            {
                                Task.Factory.ContinueWhenAll(catFetchTasks.ToArray(), data =>
                                {
                                    App.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        viewModel.Tags.Clear();

                                        tagVMs.ForEach(tag =>
                                        {
                                            viewModel.Tags.Add(tag);
                                        });

                                        if (onComplete != null) onComplete(viewModel);
                                    }));
                                });
                            }
                            else
                            {
                                App.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    viewModel.Tags.Clear();

                                    if (onComplete != null) onComplete(viewModel);
                                }));
                            }
                        }
                    });
            }));
        }

        private Task SaveTag(ViewModels.Matters.MatterTag viewModel, Action onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
                Consumers.Matters.MatterTag tagConsumer = new Consumers.Matters.MatterTag();
                viewModel.UpdateModel();
                Common.Rest.Requests.Matters.MatterTag request = Mapper.Map<Common.Rest.Requests.Matters.MatterTag>(viewModel.Model);
                tagConsumer.Update<Common.Rest.Requests.Matters.MatterTag, Common.Rest.Responses.Matters.MatterTag>(
                    request, result =>
                    {
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
                                    Source = this.GetType().FullName + "SaveTag(ViewModels.Matters.MatterTag)",
                                    Recover = (error, data, onFail) =>
                                    {
                                        SaveTag(viewModel, onComplete);
                                    },
                                    Fail = (error, data) =>
                                    {
                                        if (onComplete != null) onComplete();
                                    }
                                });
                        }
                        else
                            if (onComplete != null) onComplete();
                    });
            }));
        }

        private Task CreateTag(ViewModels.Matters.MatterTag viewModel, Action onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
                Consumers.Matters.MatterTag tagConsumer = new Consumers.Matters.MatterTag();
                viewModel.UpdateModel();
                Common.Rest.Requests.Matters.MatterTag request = Mapper.Map<Common.Rest.Requests.Matters.MatterTag>(viewModel.Model);
                tagConsumer.Create<Common.Rest.Requests.Matters.MatterTag, Common.Rest.Responses.Matters.MatterTag>(
                    request, result =>
                    {
                        if (!result.ResponseContainer.WasSuccessful)
                        {
                            ErrorHandling.ErrorManager.CreateAndThrow<ErrorHandling.ActionableError>(
                                new ErrorHandling.ActionableError()
                                {
                                    Level = ErrorHandling.LevelType.Error,
                                    Title = "Error",
                                    SimpleMessage = "Create failed.  Would you like to retry?",
                                    Message = "Error: " + result.ResponseContainer.Error.Message,
                                    Exception = result.ResponseContainer.Error.Exception,
                                    Source = this.GetType().FullName + "SaveTag(ViewModels.Matters.MatterTag)",
                                    Recover = (error, data, onFail) =>
                                    {
                                        SaveTag(viewModel, onComplete);
                                    },
                                    Fail = (error, data) =>
                                    {
                                        if (onComplete != null) onComplete();
                                    }
                                });
                        }
                        if (onComplete != null) onComplete();
                    });
            }));
        }

        private Task DisableTag(ViewModels.Matters.MatterTag viewModel, Action onComplete)
        {
            return Task.Factory.StartNew(new Action(() =>
            {
                Consumers.Matters.MatterTag tagConsumer = new Consumers.Matters.MatterTag();
                viewModel.UpdateModel();
                Common.Rest.Requests.Matters.MatterTag request = Mapper.Map<Common.Rest.Requests.Matters.MatterTag>(viewModel.Model);
                tagConsumer.Disable<Common.Rest.Requests.Matters.MatterTag, Common.Rest.Responses.Matters.MatterTag>(
                    request, result =>
                    {
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
                                    Source = this.GetType().FullName + "DisableTag(ViewModels.Matters.MatterTag)",
                                    Recover = (error, data, onFail) =>
                                    {
                                        DisableTag(viewModel, onComplete);
                                    },
                                    Fail = (error, data) =>
                                    {
                                        if (onComplete != null) onComplete();
                                    }
                                });
                        }
                        else
                            if (onComplete != null) onComplete();
                    });
            }));
        }

        private void RefreshTags(ViewModels.Matters.Matter viewModel)
        {
            LoadTags(viewModel, result =>
            {
                MasterDetailWindow.DetailDataContext = result;
            });
        }

        public override Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return UpdateItem<Common.Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter>
                ((Common.Rest.Requests.Matters.Matter)request, onComplete);
        }

        public override Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return CreateItem<Common.Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter>
                ((Common.Rest.Requests.Matters.Matter)request, onComplete);
        }

        public override Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel, ErrorHandling.ActionableError> onComplete)
        {
            return DisableItem<Common.Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter>
                ((Common.Rest.Requests.Matters.Matter)request, onComplete);
        }

        public override Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>, ErrorHandling.ActionableError> onComplete)
        {
            return ListItems<Common.Rest.Requests.Matters.Matter, Common.Rest.Responses.Matters.Matter>
                ((Common.Rest.Requests.Matters.Matter)request, onComplete);
        }
    }
}
