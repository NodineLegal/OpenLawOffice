using System.Windows;
using System.Windows.Controls;

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

        public CategoryTag()
        {
            InitializeComponent();
        }

        private void UIViewContainer_Click(object sender, RoutedEventArgs e)
        {
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
                if (OnSave != null) OnSave(this);
            }
        }
    }
}
