using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FRESHAir.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public ObservableCollection<WeatherViewModel> WeatherViews { get; set; } = new()
        {
            new WeatherViewModel
            {
                SearchCity = "Lafayette"
            }
        };

        [ObservableProperty]
        private WeatherViewModel? selectedWeatherView;

        [ObservableProperty]
        private string? locationSearchText = null;

        public void AddSearchedLocation()
        {
            WeatherViews.Add(new WeatherViewModel
            {
                SearchCity = LocationSearchText
            });
        }
    }
}