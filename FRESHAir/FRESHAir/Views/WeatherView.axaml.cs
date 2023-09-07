using Avalonia.Controls;
using FRESHAir.ViewModels;

namespace FRESHAir.Views
{
    public partial class WeatherView : UserControl
    {
        public WeatherView()
        {
            DataContext = new WeatherViewViewModel();
            (DataContext as WeatherViewViewModel).Initialize();
            InitializeComponent();
        }
    }
}
