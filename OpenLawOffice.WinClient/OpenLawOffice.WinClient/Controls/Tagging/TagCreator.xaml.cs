using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace OpenLawOffice.WinClient.Controls.Tagging
{
    /// <summary>
    /// Interaction logic for TagCreator.xaml
    /// </summary>
    public partial class TagCreator : UserControl
    {
        public delegate void SaveActionHandler(TagCreator tagcreator, ViewModels.Tagging.TagCategory category, string tag);
        public delegate void CancelActionHandler(TagCreator tagcreator);

        public event SaveActionHandler OnSave;
        public event CancelActionHandler OnCancel;

        public TagCreator()
        {
            InitializeComponent();
            Visibility = System.Windows.Visibility.Hidden;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CloseOverlay();
            if (OnCancel != null) OnCancel(this);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (OnSave != null)
                OnSave(this, (ViewModels.Tagging.TagCategory)CategorySelector.SelectedItem, TagText.Text.Trim());
            CloseOverlay();
        }

        private void CloseOverlay()
        {
            TagText.Text = "";
            Visibility = System.Windows.Visibility.Hidden;
        }

        public void Load()
        {
            Visibility = System.Windows.Visibility.Visible;
            PopulateCategories();
        }

        private void PopulateCategories()
        {
            Controllers.Matters.Matter matterController = ControllerManager.Instance.GetController<Controllers.Matters.Matter>();

            ObservableCollection<ViewModels.Tagging.TagCategory> tagCategories = new ObservableCollection<ViewModels.Tagging.TagCategory>();
            tagCategories.Clear();

            matterController.GetTagCategories(list =>
            {
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    tagCategories.Add(ViewModels.Creator.Create<ViewModels.Tagging.TagCategory>(
                        new Common.Models.Tagging.TagCategory()
                        {
                            Id = 0,
                            Name = "[None]"
                        }));

                    list.ForEach(cat =>
                    {
                        tagCategories.Add(cat);
                    });

                    Binding bind = new Binding();
                    bind.Source = tagCategories;
                    CategorySelector.SetBinding(ComboBox.ItemsSourceProperty, bind);

                    //UICategorySelector.ItemsSource = TagCategories;
                    CategorySelector.SelectedValuePath = "Id";
                    CategorySelector.DisplayMemberPath = "Name";

                    CategorySelector.SelectedValue = 0;
                    CategorySelector.Text = "[None]";
                }));
            });
        }
    }
}