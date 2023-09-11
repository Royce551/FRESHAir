using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FRESHAir.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ViewModelBase> Views { get; set; } = new()
        {
            new WeatherViewModel
            {
                SearchCity = "Lafayette"
            },
            new SettingsViewModel()
        };

        [ObservableProperty]
        private ViewModelBase? selectedView;

        [ObservableProperty]
        private string? locationSearchText = null;

        public void AddSearchedLocation()
        {
            Views.Insert(Views.Count - 1,new WeatherViewModel
            {
                SearchCity = LocationSearchText
            });
        }
    }
}