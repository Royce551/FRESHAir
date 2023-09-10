using Avalonia.Controls;
using FRESHAir.ViewModels;

namespace FRESHAir.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }
    }
}