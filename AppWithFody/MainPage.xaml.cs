using Windows.UI.Xaml.Controls;

namespace AppWithFody
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();
            DataContext = new Person();
        }
    }
}
