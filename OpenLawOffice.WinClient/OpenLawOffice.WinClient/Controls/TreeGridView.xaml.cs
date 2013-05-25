using System;
using System.Windows.Controls;
using System.Windows;
using DW.WPFToolkit;
using DW.SharpTools;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for TreeGridView.xaml
    /// </summary>
    public partial class TreeGridView : UserControl, IMaster
    {
        public Action<Controls.IMaster, object> OnSelectionChanged { get; set; }
        public Action<ViewModels.IViewModel> ParentChanged { get; set; }
        public Func<ViewModels.IViewModel, ViewModels.IViewModel> GetItemDetails { get; set; }
        public Func<ViewModels.IViewModel, List<ViewModels.IViewModel>> GetItemChildren { get; set; }
        public event RoutedEventHandler OnNodeExpanded;

        private Point _startPoint;
        public object SelectedItem 
        { 
            get { return UITree.SelectedItem; }
            set { SelectItem(value); }
        }

        public TreeGridView()
        {
            InitializeComponent();
            UITree.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(TreeViewItemExpanded));
        }

        public TreeGridView SetExpanderColumnTemplate(string header, string textBindingPath, double width = Double.NaN)
        {
            string xaml = string.Format(@"<DataTemplate 
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                    xmlns:toolkit=""clr-namespace:DW.WPFToolkit;assembly=DW.WPFToolkit"">
                    <DockPanel>
                        <toolkit:TreeListViewExpander DockPanel.Dock=""Left"" />
                        <TextBlock Text=""{{Binding {0}}}"" Margin=""5,0,0,0"" />
                    </DockPanel>
                </DataTemplate>", textBindingPath);

            ExpanderColumn.Header = header;
            ExpanderColumn.CellTemplate = (DataTemplate)System.Windows.Markup.XamlReader.Parse(xaml);
            ExpanderColumn.Width = width;

            return this;
        }

        public TreeGridView AddColumn(GridViewColumn column)
        {
            UIGridView.Columns.Add(column);
            return this;
        }

        public TreeGridView AddResource(Type dataType, object resource)
        {
            UITree.Resources.Add(new DataTemplateKey(dataType), resource);
            return this;
        }

        public object GetSelectedItem()
        {
            return UITree.SelectedItem;
        }

        public void ClearSelected()
        {
            foreach (TreeViewItem item in UITree.SelectedTreeViewItems)
            {
                item.IsSelected = false;
            }
        }

        private void TreeViewItemExpanded(object sender, RoutedEventArgs e)
        {
            if (OnNodeExpanded != null) OnNodeExpanded(sender, e);
        }

        public void SelectItem(object obj)
        {
            // Visually selecting an item on a just in time loaded tree will be a bit more complicated
            // than the ListGridView.

            /* To visually select an item, we need to start with the destination and back track through parents, 
             * expanding them as necessary.  This becomes increasingly networkin intensive as we progress
             * through the levels, thus, fewer levels is better for selection.
             * 
             * Implementation:
             * 1) Cast ViewModel from object passed through the obj argument
             * 2) Download the ViewModel
             * 3) Get ViewModel's Parent - only will have an id
             * 4) Download ViewModel's Parent - gets all info
             * 5) Download a list of all children of ViewModel's Parent
             * 6) Append children of ViewModel's Parent to the same
             * Assign ViewModel := ViewModel's Parent and cycle steps 3-6
             * 7) Download all root elements
             * 8) Set the UI's DataContext to the list (which will obviously need to be an ObservableCollection, not actually a list)
             * 9) Visually select the argument ViewModel
             *  a) If the "Select" event is not fired when the programmatic ui selection is made, it will 
             *      need to be programmatically fired.
             */

            // Test to make sure obj implements IViewModel
            if (!typeof(ViewModels.IViewModel).IsAssignableFrom(obj.GetType()))
                throw new ArgumentException("Argument must implement ViewModels.IViewModel.");

            // 1) Cast ViewModel from object passed through the obj argument
            ViewModels.IViewModel viewModel = (ViewModels.IViewModel)obj;

            while (true)
            {
                // 2) Download the ViewModel
                viewModel = GetItemDetails(viewModel);

                // 3) Get ViewModel's Parent - only will have an id
                PropertyInfo parentProperty = viewModel.GetType().GetProperty("Parent");
                ViewModels.IViewModel parentViewModel = (ViewModels.IViewModel)parentProperty.GetValue(viewModel, null);

                // 4) Download ViewModel's Parent - gets all info
                parentViewModel = GetItemDetails(parentViewModel);

                // 5) Download a list of all children of ViewModel's Parent
                List<ViewModels.IViewModel> parentsChildren = GetItemChildren(parentViewModel);

                // 6) Append children of ViewModel's Parent to the same
                ObservableCollection<ViewModels.IViewModel> observableParentsChildren =
                    new ObservableCollection<ViewModels.IViewModel>(parentsChildren);
                PropertyInfo parentChildrenProperty = parentViewModel.GetType().GetProperty("Children");
                parentChildrenProperty.SetValue(parentViewModel, observableParentsChildren, null);

                if (parentViewModel == null)
                    break;

                // Assign ViewModel := ViewModel's Parent and cycle steps 3-6
                viewModel = parentViewModel;
            }

            // 7) Download all root elements
            ObservableCollection<ViewModels.IViewModel> rootCollection = 
                new ObservableCollection<ViewModels.IViewModel>(GetItemChildren(null));

            for (int i=0; i<rootCollection.Count; i++)
            {
                if (rootCollection[i] == viewModel)
                    rootCollection[i] = viewModel;
            }

            // 8) Set the UI's DataContext to the list (which will obviously need to be an ObservableCollection, not actually a list)
            UITree.DataContext = rootCollection;

            //TreeViewItem tvi = new TreeViewItem();

            //tvi.IsSelected
            
        }

        private void UITree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (OnSelectionChanged != null) OnSelectionChanged(this, SelectedItem);
        }

        private void UITree_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Store the mouse position
            _startPoint = e.GetPosition(null);
        }

        private void UITree_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged TreeListViewItem
                TreeListViewItem treeListViewItem = FindAncestor<TreeListViewItem>((DependencyObject)e.OriginalSource);

                if (treeListViewItem == null) return;

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("oloFormat", treeListViewItem);
                DragDrop.DoDragDrop(treeListViewItem, dragData, DragDropEffects.Move);
            } 
        }

        private void UITree_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("oloFormat") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void UITree_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("oloFormat"))
            {
                ViewModels.IViewModel sourceViewModelParent;
                ViewModels.IViewModel destViewModel;

                TreeListViewItem sourceTlvi = (TreeListViewItem)e.Data.GetData("oloFormat");
                TreeListViewItem sourceParentTlvi = FindVisualParent<TreeListViewItem>(sourceTlvi);
                ViewModels.IViewModel sourceViewModel = (ViewModels.IViewModel)sourceTlvi.DataContext;
                TreeListViewItem destTlvi = GetTreeListViewItemAtLocation(e.GetPosition((UIElement)sender));
                
                // Make sure it actually went somewhere different
                if (sourceTlvi == destTlvi && sourceTlvi != null)
                    return;

                // Reflect - someone might have an idea on how to better accomplish this, without reflection
                PropertyInfo sourceViewModelParentProperty = sourceTlvi.DataContext.GetType().GetProperty("Parent");
                sourceViewModelParent = (ViewModels.IViewModel)sourceViewModelParentProperty.GetValue(sourceTlvi.DataContext, null);

                // Consider - Parent gets moved into its child.  We have two options
                // 1) Child takes Parent's Parent
                // 2) We refuse it
                // In 1, we have to assume this is what the user wants - Personally, I dislike this approach
                // In 2, we need to explain to the user why

                // Checking
                if (destTlvi != null)
                {
                    // Has a parent, so get that parent
                    destViewModel = (ViewModels.IViewModel)destTlvi.DataContext;

                    // Get the parent of destination
                    // No need to re-reflect, same type, just use it
                    //destViewModelParent = (ViewModels.IViewModel)sourceViewModelParentProperty.GetValue(destViewModel, null);
                    
                    // Check to make sure the destination is not a child of source (or any other decendant)
                    if (IsAncestor(destViewModel, sourceViewModel))
                    {
                        MessageBox.Show("Cannot make item a decendant of itself.", "Cannot Complete Action", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    // There is no source parent and no node at destination -> root to root, so do nothing.
                    if (sourceViewModelParent == null)
                        return;
                }



                // Source work
                if (sourceViewModelParent != null)
                {
                    // Has a parent, so we actually need to replace

                    // Removes the sourceViewModel from its parent's collection of children
                    // this method will automatically null the sourceViewModel's parent
                    MethodInfo removeChild = sourceViewModelParent.GetType().GetMethod("RemoveChild");
                    removeChild.Invoke(sourceViewModelParent, new object[] { sourceViewModel });
                }
                else
                {
                    // Root element - remove from root
                    System.Collections.IList list = (System.Collections.IList)DataContext;
                    list.Remove(sourceViewModel);
                }

                // Destination work
                if (destTlvi == null)
                {
                    // Root element

                    // Add to root
                    System.Collections.IList list = (System.Collections.IList)DataContext;
                    list.Add(sourceViewModel);
                }
                else
                {
                    // Has a parent, so get that parent
                    destViewModel = (ViewModels.IViewModel)destTlvi.DataContext;
                    
                    // Add the child to the parent
                    // this method will automatically set the sourceViewModel's parent
                    MethodInfo addChild = destViewModel.GetType().GetMethod("AddChild");
                    addChild.Invoke(destViewModel, new object[] { sourceViewModel });
                }

                if (sourceParentTlvi != null)
                    sourceParentTlvi.DataContext = sourceViewModelParent;

                // Notify subscribers
                if (ParentChanged != null) ParentChanged(sourceViewModel);
            }
        }

        private bool IsAncestor(ViewModels.IViewModel tree, ViewModels.IViewModel find)
        {
            PropertyInfo idProperty = tree.GetType().GetProperty("Id");
            object treeId = idProperty.GetValue(tree, null);
            object findId = idProperty.GetValue(find, null);

            if (Convert.ToInt64(treeId) == Convert.ToInt64(findId)) 
                return true;

            PropertyInfo startParentProperty = tree.GetType().GetProperty("Parent");
            ViewModels.IViewModel parent = (ViewModels.IViewModel)startParentProperty.GetValue(tree, null);
            if (parent == null) return false;
            return IsAncestor(parent, find);
        }

        private TreeListViewItem GetTreeListViewItemAtLocation(Point location)
        {
            System.Windows.Media.HitTestResult hitTestResult = System.Windows.Media.VisualTreeHelper.HitTest(UITree, location);

            if (hitTestResult.VisualHit is TreeListViewItem)
                return (TreeListViewItem)hitTestResult.VisualHit;
            else
                return FindAncestor<TreeListViewItem>(hitTestResult.VisualHit);
        }

        private static T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = System.Windows.Media.VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
        
        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the queried item.</param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null reference is being returned.</returns>
        public static T FindVisualParent<T>(DependencyObject child)
          where T : DependencyObject
        {
            // get parent item
            DependencyObject parentObject = System.Windows.Media.VisualTreeHelper.GetParent(child);

            // we’ve reached the end of the tree
            if (parentObject == null) return null;

            // check if the parent matches the type we’re looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                // use recursion to proceed with next level
                return FindVisualParent<T>(parentObject);
            }
        }
    }
}
