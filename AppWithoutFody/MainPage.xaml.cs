using Windows.UI.Xaml.Controls;

namespace AppWithoutFody
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
