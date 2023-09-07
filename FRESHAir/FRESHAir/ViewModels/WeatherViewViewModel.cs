using CommunityToolkit.Mvvm.ComponentModel;
using FRESHAir.Services.WeatherService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.ViewModels
{
    public partial class WeatherViewViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string locationName = "Lafayette";

        [ObservableProperty]
        private Temperature currentTemperature = Temperature.FromCelsius(29);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Location))]
        private WeatherSummary? currentWeatherSummary;

        public string Location => $"{currentWeatherSummary?.LocationName}, {currentWeatherSummary?.Admin1}, {currentWeatherSummary?.CountryCode}";

        private TemperatureScale scale;
        public TemperatureScale Scale
        {
            get => scale;
            set
            {
                CurrentTemperature.Scale = value;
                SetProperty(ref scale, value);
            }
        }


        public async Task Initialize()
        {
            CurrentWeatherSummary = await WeatherService.GetWeatherSummaryAsync("McMurdo Station", TemperatureScale.Fahrenheit);
        }

    }
}
