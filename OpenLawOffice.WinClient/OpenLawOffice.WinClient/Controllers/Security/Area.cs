using System;
using System.Windows.Input;
using AutoMapper;
using System.Collections.Generic;
using DW.SharpTools;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenLawOffice.WinClient.Controllers.Security
{
    [Handle(typeof(Common.Models.Security.Area))]
    public class Area
        : MasterDetailControllerCore<Controls.TreeGridView, Views.Security.AreaDetail, 
            Views.Security.AreaEdit, Views.Security.AreaCreate>
    {
        public override Type RequestType { get { return typeof(Common.Rest.Requests.Security.Area); } }
        public override Type ResponseType { get { return typeof(Common.Rest.Responses.Security.Area); } }
        public override Type ViewModelType { get { return typeof(ViewModels.Security.Area); } }
        public override Type ModelType { get { return typeof(Common.Models.Security.Area); } }

        protected Consumers.Security.Area _typedConsumer
        {
            get { return (Consumers.Security.Area)_consumer; }
            set { _consumer = value; }
        }

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


            Views.Security.AreaAclRelation areaAclRelation = new Views.Security.AreaAclRelation();
            areaAclRelation.OnClose += iwin =>
            {
                MasterDetailWindow.HideRelationView();
            };
            areaAclRelation.OnEdit += iwin =>
            {
                System.Windows.MessageBox.Show("Feature not yet supported.");
            };
            areaAclRelation.OnView += iwin =>
            {
                System.Windows.MessageBox.Show("Feature not yet supported.");
            };
            MasterDetailWindow.AddRelationView<Common.Models.Security.AreaAcl>(areaAclRelation);
        }

        private ViewModels.Security.Area BuildFilter()
        {
            ViewModels.Security.Area filter = ViewModels.Creator.Create<ViewModels.Security.Area>(new Common.Models.Security.Area());

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                string nameFilter = MainWindow.SecurityAreas_List_Name.Text.Trim();
                if (!string.IsNullOrEmpty(nameFilter))
                    filter.Name = nameFilter;
            }));

            return filter;
        }

        public override void LoadUI()
        {
            ObservableCollection<ViewModels.IViewModel> viewModelCollection = null;

            // ribbon controls
            MainWindow.SecurityAreas_List.Command = new Commands.DelegateCommand(x =>
            {
                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();
                
                LoadItems(BuildFilter(), viewModelCollection, results =>
                {
                    MasterDetailWindow.MasterDataContext = results;
                });
            });

            MainWindow.SecurityAreas_Acls.Command = new Commands.DelegateCommand(x =>
            {
                MasterDetailWindow.ShowRelationView<Common.Models.Security.AreaAcl>();
            }, x => MasterDetailWindow.RelationshipsEnabled);
            MasterDetailWindow.RelationshipCommands.Add((Commands.DelegateCommand)MainWindow.SecurityAreas_Acls.Command);

            MainWindow.SecurityAreas_Create.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ViewModels.Security.Area viewModel = ViewModels.Creator.Create<ViewModels.Security.Area>(new Common.Models.Security.Area());
                    MasterDetailWindow.GoIntoCreateMode(viewModel);
                }));
            }, x => MasterDetailWindow.CreateEnabled);
            
            MainWindow.SecurityAreas_Edit.Command = new Commands.DelegateCommand(x =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    MasterDetailWindow.UpdateDetailViewDataContext(MasterDetailWindow.MasterView.SelectedItem);
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
                    DisableItem((ViewModels.Security.Area)MasterDetailWindow.MasterView.SelectedItem, null);
                }
            }, x => MasterDetailWindow.DisableEnabled);

            MainWindow.SecurityAreas_Save.Command = new Commands.DelegateCommand(x =>
            {
                if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Edit)
                {
                    UpdateItem((ViewModels.Security.Area)MasterDetailWindow.DetailView.DataContext, null);
                }
                else if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                {
                    CreateItem((ViewModels.Security.Area)MasterDetailWindow.CreateView.DataContext, null);
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

                viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();
                
                LoadItems(BuildFilter(), viewModelCollection, results =>
                {
                    MasterDetailWindow.MasterDataContext = results;
                    if (MasterDetailWindow.DisplayMode == Controls.DisplayModeType.Create)
                        MasterDetailWindow.SetDisplayMode(Controls.DisplayModeType.View);
                });
            }, x => MasterDetailWindow.CancelEnabled);

            MasterDetailWindow.MasterView.ParentChanged += viewModel =>
            {
                UpdateItem((ViewModels.Security.Area)viewModel, null);
            };

            MasterDetailWindow.MasterView.OnNodeExpanded += (sender, e) =>
            {
                DW.WPFToolkit.TreeListViewItem treeItem = (DW.WPFToolkit.TreeListViewItem)e.OriginalSource;
                ViewModels.Security.Area viewModel = (ViewModels.Security.Area)treeItem.DataContext;
                ICollection<ViewModels.IViewModel> collection = viewModel.Children.Cast<ViewModels.IViewModel>().ToList();
                ViewModels.Security.Area filter = ViewModels.Creator.Create<ViewModels.Security.Area>(
                    new Common.Models.Security.Area()
                    {
                        Parent = new Common.Models.Security.Area()
                        {
                            Id = viewModel.Id
                        }
                    });
                LoadItems(filter, collection, results =>
                {
                    viewModel.Children.Clear();
                    foreach (ViewModels.Security.Area viewModelToAdd in results)
                    {
                        viewModel.AddChild(viewModelToAdd);
                    }
                });
            };

            _consumer = new Consumers.Security.Area();

            // load window
            MasterDetailWindow.Load();
            if (!MasterDetailWindow.IsSelected)
                MasterDetailWindow.SelectWindow();

            viewModelCollection = new ObservableCollection<ViewModels.IViewModel>();

            LoadItems(BuildFilter(), viewModelCollection, results =>
            {
                MasterDetailWindow.MasterDataContext = results;
            });
        }

        public override Task LoadItems(ViewModels.IViewModel filter, ICollection<ViewModels.IViewModel> collection, Action<ICollection<ViewModels.IViewModel>> onComplete)
        {
            return base.LoadItems(filter, collection, results =>
            {
                // We know that filter will either have the Id of the parent or be null meaning it is a base object
                // Thus, we can use that to apply the parent
                if (filter != null &&
                    ((ViewModels.Security.Area)filter).Parent != null &&
                    ((ViewModels.Security.Area)filter).Parent.Id.HasValue && 
                    ((ViewModels.Security.Area)filter).Parent.Id.Value > 0)
                {
                    ObservableCollection<ViewModels.Security.Area> castCollection =
                        CastObservableCollection<ViewModels.Security.Area>(MasterDetailWindow.MasterDataContext);
                    
                    ViewModels.Security.Area parentViewModel =
                        Find(castCollection, ((ViewModels.Security.Area)filter).Parent);

                    foreach (ViewModels.Security.Area result in results)
                    {
                        result.Parent = parentViewModel;
                    }
                }

                if (onComplete != null)
                    onComplete(results);
            });
        }

        public override Task LoadDetails(ViewModels.IViewModel viewModel, Action<ViewModels.IViewModel> onComplete)
        {
            ViewModels.Security.Area castViewModel = (ViewModels.Security.Area)viewModel;

            MasterDetailWindow.DetailView.IsBusy = true;
            MasterDetailWindow.EditView.IsBusy = true;

            return PopulateCoreDetails<Common.Models.Security.Area>(castViewModel, () =>
            {
                MasterDetailWindow.DetailView.IsBusy = false;
                MasterDetailWindow.EditView.IsBusy = false;
            });
        }

        public override Task UpdateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete)
        {
            return UpdateItem<Common.Rest.Requests.Security.Area, Common.Rest.Responses.Security.Area>
                ((Common.Rest.Requests.Security.Area)request, result =>
            {
                ViewModels.Security.Area castedResult = (ViewModels.Security.Area)result;

                if (castedResult.Parent != null)
                {
                    // Updates remove hierarchical information except for parent id, so we need to track it down

                    ObservableCollection<ViewModels.Security.Area> castCollection =
                        CastObservableCollection<ViewModels.Security.Area>(MasterDetailWindow.MasterDataContext);
                    
                    ViewModels.Security.Area dummyParent = ViewModels.Creator.Create<ViewModels.Security.Area>(new Common.Models.Security.Area());
                    dummyParent.Id = castedResult.Parent.Id;

                    ViewModels.Security.Area parentViewModel =
                        Find(castCollection, dummyParent);

                    castedResult.Parent = parentViewModel;
                }

                if (onComplete != null) onComplete(castedResult);
            });
        }

        public override Task CreateItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete)
        {
            return CreateItem<Common.Rest.Requests.Security.Area, Common.Rest.Responses.Security.Area>
                ((Common.Rest.Requests.Security.Area)request, result =>
            { 
                ViewModels.Security.Area castedResult = (ViewModels.Security.Area)result;
                
                // setup dummy child
                castedResult.AddChild(ViewModels.Creator.CreateDummy<ViewModels.Security.Area>(new Common.Models.Security.Area()));

                // Lookup parent
                if (castedResult.Parent == null)
                {
                    // Not found - push to datacontext
                    ICollection<ViewModels.IViewModel> collection = (ICollection<ViewModels.IViewModel>)MasterDetailWindow.MasterDataContext;
                    collection.Add(castedResult);
                }
                else
                {
                    // Found - push to parent's children
                    ObservableCollection<ViewModels.Security.Area> castCollection =
                        CastObservableCollection<ViewModels.Security.Area>(MasterDetailWindow.MasterDataContext);

                    ViewModels.Security.Area parent = Find(castCollection, castedResult.Parent);
                    parent.AddChild(castedResult);
                }

                if (onComplete != null) onComplete(castedResult);
            });
        }
        
        public override Task DisableItem(Common.Rest.Requests.RequestBase request, Action<ViewModels.IViewModel> onComplete)
        {
            return base.DisableItem<Common.Rest.Requests.Security.Area, Common.Rest.Responses.Security.Area>
                ((Common.Rest.Requests.Security.Area)request, result =>
            { 
                ViewModels.Security.Area castedResult = (ViewModels.Security.Area)result;

                if (castedResult.Parent != null)
                {
                    // Updates remove hierarchical information except for parent id, so we need to track it down
                    ObservableCollection<ViewModels.Security.Area> castCollection =
                        CastObservableCollection<ViewModels.Security.Area>(MasterDetailWindow.MasterDataContext);
                    
                    ViewModels.Security.Area dummyParent = ViewModels.Creator.Create<ViewModels.Security.Area>(new Common.Models.Security.Area());
                    dummyParent.Id = castedResult.Parent.Id;

                    ViewModels.Security.Area parentViewModel =
                        Find(castCollection, dummyParent);

                    castedResult.Parent = parentViewModel;

                    castedResult.Parent.RemoveChild(castedResult);
                }
                else
                {
                    ((ObservableCollection<ViewModels.IViewModel>)MasterDetailWindow.MasterDataContext).Remove(castedResult);
                }

                if (onComplete != null) onComplete(castedResult);
            });
        }

        public override Task ListItems(Common.Rest.Requests.RequestBase request, Action<List<ViewModels.IViewModel>> onComplete)
        {
            return ListItems<Common.Rest.Requests.Security.Area, Common.Rest.Responses.Security.Area>
                ((Common.Rest.Requests.Security.Area)request, viewModels =>
            {
                foreach (ViewModels.Security.Area viewModel in viewModels)
                {
                    viewModel.AddChild(ViewModels.Creator.CreateDummy<ViewModels.Security.Area>(new Common.Models.Security.Area()));
                }
                if (onComplete != null)
                    onComplete(viewModels);
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

        private ViewModels.Security.Area Find(ObservableCollection<ViewModels.Security.Area> list, ViewModels.Security.Area target)
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
    }
}
