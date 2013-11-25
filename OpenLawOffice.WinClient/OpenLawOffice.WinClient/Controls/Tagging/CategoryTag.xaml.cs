using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System;
using System.Windows.Data;

namespace OpenLawOffice.WinClient.Controls.Tagging
{
    /// <summary>
    /// Interaction logic for CategoryTag.xaml
    /// </summary>
    public partial class CategoryTag : UserControl
    {
        public enum DisplayModeType
        {
            Readonly,
            View,
            Edit
        }

        private DisplayModeType _displayMode = DisplayModeType.View;

        public delegate void ActionHandler(CategoryTag sender);
        public event ActionHandler OnCancel;
        public event ActionHandler OnSave;
        public event ActionHandler OnDelete;

        public DisplayModeType DisplayMode
        {
            get { return _displayMode; }
            set
            {
                _displayMode = value;
                if (_displayMode == DisplayModeType.Edit)
                {
                    UIEditContainer.Visibility = System.Windows.Visibility.Visible;
                    UIViewContainer.Visibility = System.Windows.Visibility.Collapsed;
                    UIReadonlyContainer.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (_displayMode == DisplayModeType.View)
                {
                    UIEditContainer.Visibility = System.Windows.Visibility.Collapsed;
                    UIViewContainer.Visibility = System.Windows.Visibility.Visible;
                    UIReadonlyContainer.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    UIEditContainer.Visibility = System.Windows.Visibility.Collapsed;
                    UIViewContainer.Visibility = System.Windows.Visibility.Collapsed;
                    UIReadonlyContainer.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        public string PreviousText { get; set; }
        public string Text { get { return UITagText.Text; } }

        public ObservableCollection<ViewModels.Tagging.TagCategory> TagCategories { get; set; }

        public CategoryTag()
        {
            TagCategories = new ObservableCollection<ViewModels.Tagging.TagCategory>();
            
            InitializeComponent();
        }

        private void UIViewContainer_Click(object sender, RoutedEventArgs e)
        {
            PreviousText = UITagText.Text.Trim();
            PopulateCategories();
            DisplayMode = DisplayModeType.Edit;
            UITagText.Focus();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (OnDelete != null) OnDelete(this);
        }

        private void UserControl_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                DisplayMode = DisplayModeType.View;
                if (OnCancel != null) OnCancel(this);
            }
            else if (e.Key == System.Windows.Input.Key.Return)
            {
                System.Reflection.PropertyInfo tagCatProp = DataContext.GetType().GetProperty("TagCategory");
                tagCatProp.SetValue(DataContext, UICategorySelector.SelectedItem, null);
                DisplayMode = DisplayModeType.View;
                if (OnSave != null) OnSave(this);
            }
        }

        private void PopulateCategories()
        {
            Controllers.Matters.Matter matterController = ControllerManager.Instance.GetController<Controllers.Matters.Matter>();

            if (TagCategories == null) TagCategories = new ObservableCollection<ViewModels.Tagging.TagCategory>();
            TagCategories.Clear();

            matterController.GetTagCategories(list =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    list.ForEach(cat =>
                    {
                        TagCategories.Add(cat);
                    });

                    Binding bind = new Binding();
                    bind.Source = TagCategories;
                    UICategorySelector.SetBinding(ComboBox.ItemsSourceProperty, bind);

                    //UICategorySelector.ItemsSource = TagCategories;
                    UICategorySelector.SelectedValuePath = "Id";
                    UICategorySelector.DisplayMemberPath = "Name";



                    System.Reflection.PropertyInfo tagCatProp = DataContext.GetType().GetProperty("TagCategory");
                    ViewModels.Tagging.TagCategory tagCat = (ViewModels.Tagging.TagCategory)tagCatProp.GetValue(DataContext, null);

                    UICategorySelector.SelectedValue = tagCat;
                    if (tagCat != null)
                        UICategorySelector.Text = tagCat.Name;
                }));
            });
        }
    }
}
