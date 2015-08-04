using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace AppWithFody
{
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            InitializeComponent();
            var viewModel = new PersonViewModel();
            DataContext = viewModel;
            var propertyChanged = (INotifyPropertyChanged) viewModel;
            propertyChanged.PropertyChanged += OnPropertyChanged;
        }
        
        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            eventLog.Text += string.Format("PropertyName:{1}{0}", Environment.NewLine, e.PropertyName);
        }
    }
}
