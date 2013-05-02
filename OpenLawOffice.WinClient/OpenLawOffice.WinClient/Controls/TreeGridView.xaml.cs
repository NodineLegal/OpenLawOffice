using System;
using System.Windows.Controls;
using System.Windows;
using DW.WPFToolkit;
using DW.SharpTools;
using System.Reflection;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.Controls
{
    /// <summary>
    /// Interaction logic for TreeGridView.xaml
    /// </summary>
    public partial class TreeGridView : UserControl, IMaster
    {
        public Action<Controls.IMaster, object> OnSelectionChanged { get; set; }
        public Action<ViewModels.IViewModel> ParentChanged { get; set; }
        public event RoutedEventHandler OnNodeExpanded;

        private Point _startPoint;
        public object SelectedItem { get { return UITree.SelectedItem; } }

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
