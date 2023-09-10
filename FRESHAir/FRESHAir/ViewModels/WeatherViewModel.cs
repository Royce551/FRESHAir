using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using FRESHAir.Services.WeatherService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.ViewModels
{
    public partial class WeatherViewModel : ViewModelBase
    {
        private string? searchCity = "New Orleans";
        public string? SearchCity
        {
            get => searchCity;
            set
            {
                SetProperty(ref searchCity, value);
                _ = Initialize();
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Location))]
        [NotifyPropertyChangedFor(nameof(ObservationsForNext12Hours))]
        private WeatherSummary? currentWeatherSummary;

        public string Location => $"{CurrentWeatherSummary?.LocationName}, {CurrentWeatherSummary?.Admin1}, {CurrentWeatherSummary?.CountryCode}";

        public WeatherObservation[]? ObservationsForNext12Hours => CurrentWeatherSummary?.ForecastedWeather.Take(12).ToArray();

        private TemperatureScale scale;
        public TemperatureScale Scale
        {
            get => scale;
            set
            {
                //CurrentTemperature.Scale = value;
                SetProperty(ref scale, value);
            }
        }


        public async Task Initialize()
        {
            CurrentWeatherSummary = await WeatherService.GetWeatherSummaryAsync(searchCity, TemperatureScale.Fahrenheit);
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

    /// <summary>
    /// <para>
    /// Converts a string path to a bitmap asset.
    /// </para>
    /// <para>
    /// The asset must be in the same assembly as the program. If it isn't,
    /// specify "avares://<assemblynamehere>/" in front of the path to the asset.
    /// </para>
    /// </summary>
    public class BitmapAssetValueConverter : IValueConverter
    {
        public static BitmapAssetValueConverter Instance = new BitmapAssetValueConverter();

        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is string rawUri && targetType.IsAssignableFrom(typeof(Bitmap)))
            {
                Uri uri;

                // Allow for assembly overrides
                if (rawUri.StartsWith("avares://"))
                {
                    uri = new Uri(rawUri);
                }
                else
                {
                    string assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                    uri = new Uri($"avares://{assemblyName}/{rawUri}");
                }

                var asset = AssetLoader.Open(uri);

                return new Bitmap(asset);
            }

            throw new NotSupportedException();
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
