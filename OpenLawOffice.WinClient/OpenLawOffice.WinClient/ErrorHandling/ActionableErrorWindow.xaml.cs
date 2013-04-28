using System;
using System.Windows;
using System.Windows.Controls;

namespace OpenLawOffice.WinClient.ErrorHandling
{
    /// <summary>
    /// Interaction logic for ActionableErrorWindow.xaml
    /// </summary>
    public partial class ActionableErrorWindow : Window
    {
        public static readonly RoutedEvent YesEvent =
            EventManager.RegisterRoutedEvent("Yes", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(ActionableErrorWindow));

        public static readonly RoutedEvent NoEvent =
            EventManager.RegisterRoutedEvent("No", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(ActionableErrorWindow));

        public event RoutedEventHandler Yes
        {
            add { AddHandler(YesEvent, value); }
            remove { RemoveHandler(YesEvent, value); }
        }

        public event RoutedEventHandler No
        {
            add { AddHandler(NoEvent, value); }
            remove { RemoveHandler(NoEvent, value); }
        }

        public ActionableErrorWindow()
        {
            InitializeComponent();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ActionableErrorWindow.NoEvent));
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ActionableErrorWindow.YesEvent));
        }
    }
}
