using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using FRESHAir.Services.WeatherService;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            CurrentWeatherSummary = await WeatherService.GetWeatherSummaryAsync("Opelousas", TemperatureScale.Fahrenheit);
        }

    }

    public class TemperatureToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null) return null;
            if (value is Temperature temperature)
            {
                var decimalSymbol = temperature.Scale switch
                {
                    TemperatureScale.Celsius => "°C",
                    TemperatureScale.Fahrenheit => "°F",
                    TemperatureScale.Kelvin => "°K",
                    _ => throw new Exception()
                };

                return $"{temperature.Value:F1}{decimalSymbol}";
            }
            else throw new ArgumentException("Converted a non-temperature with the TemperatureToStringConverter");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
