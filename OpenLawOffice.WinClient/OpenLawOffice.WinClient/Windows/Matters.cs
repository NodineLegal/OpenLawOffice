//using System;
//using System.Collections.Generic;

//namespace OpenLawOffice.WinClient.Windows
//{
//    public class Matters : MasterDetailWindow
//    {
//        private System.Windows.RoutedEventHandler _ribbonSaveClickDelegate;
//        private System.Windows.RoutedEventHandler _ribbonEditClickDelegate;
//        private System.Windows.RoutedEventHandler _ribbonCancelClickDelegate;

//        public override string Title { get { return "Matters"; } }

//        private Controls.ListGridView _listGridView { get; set; }

//        public Matters()
//        {
//            OnDeselected += iwin =>
//            {
//                Globals.Instance.MainWindow.Matters_List.IsEnabled = true;
//                Globals.Instance.MainWindow.DisableMatterRelationshipControls();
//                Globals.Instance.MainWindow.Matters_Edit.IsEnabled = false;
//                Globals.Instance.MainWindow.Matters_Save.IsEnabled = false;
//                Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = false;
//            };

//            OnSelected += iwin =>
//            {
//                Globals.Instance.MainWindow.Matters_List.IsEnabled = false;
//                Globals.Instance.MainWindow.Matters_Edit.IsEnabled = true;
//                if (_listGridView.GetSelectedItem() != null)
//                {
//                    Globals.Instance.MainWindow.EnableMatterRelationshipControls();
//                    if (CurrentDetailPanelControlType == typeof(DetailViews.Matters.MatterEdit))
//                    { // If editing -> activate save and cancel
//                        Globals.Instance.MainWindow.Matters_Save.IsEnabled = true;
//                        Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = true;
//                    }
//                    else
//                    {
//                        Globals.Instance.MainWindow.Matters_Save.IsEnabled = false;
//                        Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = false;
//                    }
//                }
//            };

//            OnActivated += iwin =>
//            {
//                Globals.Instance.MainWindow.Matters_List.IsEnabled = false;
//                Globals.Instance.MainWindow.Matters_Edit.IsEnabled = true;
//            };

//            // Handle edit click
//            Globals.Instance.MainWindow.Matters_Edit.Click += _ribbonEditClickDelegate = (sender, e) =>
//            {
//                if (CurrentDetailPanelControlType != null)
//                {
//                    if (Globals.Instance.MainWindow.Matters_Edit.IsChecked.Value)
//                    {
//                        ShowDetailPanel(typeof(DetailViews.Matters.MatterEdit));
//                        Globals.Instance.MainWindow.Matters_Save.IsEnabled = true;
//                        Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = true;
//                    }
//                    else
//                    {
//                        ShowDetailPanel(typeof(DetailViews.Matters.Matter));
//                        Globals.Instance.MainWindow.Matters_Save.IsEnabled = false;
//                        Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = false;
//                    }
//                }
//            };

//            Globals.Instance.MainWindow.Matters_Save.Click += _ribbonSaveClickDelegate = (sender, e) =>
//            {
//                object obj1 = _listGridView.GetSelectedItem();
//                UiModels.Matters.Matter matter = (UiModels.Matters.Matter)DetailPanelDataContext;

//                try
//                {
//                    ModelManager.UpdateAsync<Models.Matters.Matter>(matter, (returnedObject, restResponse) =>
//                    {
//                        // This needs to be much better
//                        if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
//                            System.Windows.MessageBox.Show("Error updating.");
//                        App.Current.Dispatcher.BeginInvoke(new Action(delegate()
//                        {
//                            DisableDetailPanel();
//                        }), System.Windows.Threading.DispatcherPriority.Normal);
//                        UpdateList();
//                    });
//                }
//                catch (Exception ex)
//                {
//                    string a = "";
//                }
//            };

//            Globals.Instance.MainWindow.Matters_Cancel.Click += _ribbonCancelClickDelegate = (sender, e) =>
//            {
//                // This could be more efficient but would take much more code
//                UpdateList();
//            };
//        }

//        public override void Load()
//        {
//            // Load base
//            base.Load();

//            // Drop the right panel - nothing to display yet
//            DisableDetailPanel();

//            // Tell base what goes in the left panel (master panel)
//            SetMasterPanel(_listGridView = new Controls.ListGridView());

//            // Go ahead and setup the UI for the left (master) panel
//            MakeMasterView();

//            UpdateList();
//        }

//        private void MakeMasterView()
//        {
//            _listGridView.OnSelectionChanged += ListGridView_OnSelectionChanged;

//            _listGridView.AddColumn(new System.Windows.Controls.GridViewColumn()
//            {
//                Header = "Title",
//                DisplayMemberBinding = new System.Windows.Data.Binding("Title") { Mode = System.Windows.Data.BindingMode.OneWay }
//            });
//        }

//        private void ListGridView_OnSelectionChanged(Controls.ListGridView sender, object selectedItem)
//        {
//            SetDetailPanelDataContext(selectedItem);

//            if (selectedItem == null)
//            {
//                Globals.Instance.MainWindow.DisableMatterRelationshipControls();
//                Globals.Instance.MainWindow.Matters_Save.IsEnabled = false;
//                Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = false;
//            }
//            else
//            {
//                Models.Matters.Matter matter = (Models.Matters.Matter)selectedItem;

//                Globals.Instance.MainWindow.EnableMatterRelationshipControls();
//                if (Globals.Instance.MainWindow.Matters_Edit.IsChecked.Value)
//                {
//                    ShowDetailPanel(typeof(DetailViews.Matters.MatterEdit));
//                    Globals.Instance.MainWindow.Matters_Save.IsEnabled = true;
//                    Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = true;
//                }
//                else
//                {
//                    // load created by
//                    Models.User createdBy = new Models.User()
//                    {
//                        Id = matter.CreatedBy.Id.Value
//                    };
//                    ModelManager.FetchAsync<Models.User>(createdBy, (returnedObject, restResponse) =>
//                    {
//                        matter.CreatedBy = returnedObject;
//                    });

//                    // load modified by
//                    Models.User modifiedBy = new Models.User()
//                    {
//                        Id = matter.ModifiedBy.Id.Value
//                    };
//                    ModelManager.FetchAsync<Models.User>(modifiedBy, (returnedObject, restResponse) =>
//                    {
//                        matter.ModifiedBy = returnedObject;
//                    });

//                    // load disabled by, if !null
//                    if (matter.DisabledBy != null)
//                    {
//                        Models.User disabledBy = new Models.User()
//                        {
//                            Id = matter.DisabledBy.Id.Value
//                        };
//                        ModelManager.FetchAsync<Models.User>(disabledBy, (returnedObject, restResponse) =>
//                        {
//                            matter.DisabledBy = returnedObject;
//                        });
//                    }

//                    ShowDetailPanel(typeof(DetailViews.Matters.Matter));
//                    Globals.Instance.MainWindow.Matters_Save.IsEnabled = false;
//                    Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = false;
//                }
//            }
//        }

//        public void UpdateList()
//        {
//            Models.Matters.Matter matter = new Models.Matters.Matter();

//            if (Globals.Instance.MainWindow.Matters_List_Title.HasUserData)
//                matter.Title = Globals.Instance.MainWindow.Matters_List_Title.Text.Trim();
//            if (Globals.Instance.MainWindow.Matters_List_Tags.HasUserData)
//            {
//                string[] tags = Globals.Instance.MainWindow.Matters_List_Tags.Text.Trim().Split(' ');
//                if (matter.MatterTags == null) matter.MatterTags = new List<Models.Matters.MatterTag>();
//                matter.MatterTags.Clear();
//                foreach (string tag in tags)
//                {
//                    matter.MatterTags.Add(new Models.Matters.MatterTag()
//                    {
//                        Tag = tag
//                    });
//                }
//            }

//            ModelManager.FetchListAsync<Models.Matters.Matter>(matter,
//                (modelList, restResponse) =>
//                {
//                    App.Current.Dispatcher.BeginInvoke(new Action(delegate()
//                    {
//                        _listGridView.DataContext = DataContext = modelList;
//                    }), System.Windows.Threading.DispatcherPriority.Normal);
//                });
//        }

//        public override void Close()
//        {
//            base.Close();
//            Globals.Instance.MainWindow.DisableMatterRelationshipControls();
//            Globals.Instance.MainWindow.Matters_Save.IsEnabled = false;
//            Globals.Instance.MainWindow.Matters_Cancel.IsEnabled = false;
//        }

//        public override void Refresh()
//        {
//            UpdateList();
//        }

//        public override void Dispose()
//        {
//            Globals.Instance.MainWindow.Matters_Cancel.Click -= _ribbonCancelClickDelegate;
//            Globals.Instance.MainWindow.Matters_Edit.Click -= _ribbonEditClickDelegate;
//            Globals.Instance.MainWindow.Matters_Save.Click -= _ribbonSaveClickDelegate;
//        }
//    }
//}
