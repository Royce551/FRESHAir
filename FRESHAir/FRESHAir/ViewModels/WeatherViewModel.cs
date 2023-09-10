using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using FRESHAir.Services.WeatherService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BackgroundImage))]
        private string? backgroundImagePath = null;

        public Bitmap? BackgroundImage
        {
            get
            {
                if (BackgroundImagePath is null) return null;
                else return new Bitmap(BackgroundImagePath);
            }
        }

        public async Task Initialize()
        {
            if (SearchCity is null) return;

            CurrentWeatherSummary = await WeatherService.GetWeatherSummaryAsync(SearchCity, TemperatureScale.Fahrenheit);

            var backgroundDirectory = AssociateWeatherIconWithBackground(CurrentWeatherSummary.CurrentWeather.Icon) switch
            {
                BackgroundType.Day => "Assets/Backgrounds/Day",
                BackgroundType.DayPartlyCloudy => "Assets/Backgrounds/DayPartlyCloudy",
                BackgroundType.Night => "Assets/Backgrounds/Night",
                BackgroundType.NightPartlyCloudy => "Assets/Backgrounds/NightPartlyCloudy",
                BackgroundType.Cloudy => "Assets/Backgrounds/Cloudy",
                BackgroundType.Fog => "Assets/Backgrounds/Fog",
                BackgroundType.Storm => "Assets/Backgrounds/Storm",
                BackgroundType.Rain => "Assets/Backgrounds/Rain",
                BackgroundType.Snow => "Assets/Backgrounds/Snow",
                _ => throw new Exception()
            };

            var images = Directory.EnumerateFiles(backgroundDirectory).ToList();

            BackgroundImagePath = images[Random.Shared.Next(0, images.Count)];
        }

        private BackgroundType AssociateWeatherIconWithBackground(string weatherIcon)
        {
            var rawImagePath = Path.GetFileNameWithoutExtension(weatherIcon);
            BackgroundType backgroundType = BackgroundType.Day;

            switch (rawImagePath)
            {
                case "clearsky_day":
                    backgroundType = BackgroundType.Day;
                    break;
                case "clearsky_night":
                    backgroundType = BackgroundType.Night;
                    break;
                case "fair_day":
                case "fair_polartwilight":
                case "partlycloudy_day":
                case "partlycloudy_polartwilight":
                    backgroundType = BackgroundType.DayPartlyCloudy;
                    break;
                case "fair_night":
                case "partlycloudy_night":
                    backgroundType = BackgroundType.NightPartlyCloudy;
                    break;
                case "cloudy":
                    backgroundType = BackgroundType.Cloudy;
                    break;
                case "fog":
                    backgroundType = BackgroundType.Fog;
                    break;
                default:
                    if (rawImagePath.Contains("storm")) backgroundType = BackgroundType.Storm;
                    else if (rawImagePath.Contains("rain")) backgroundType = BackgroundType.Rain;
                    else if (rawImagePath.Contains("sleet") || weatherIcon.Contains("snow")) backgroundType = BackgroundType.Snow;
                    else if (rawImagePath.Contains("showers")) backgroundType = BackgroundType.Storm;
                    break;
            }

            return backgroundType;
        }
    }

    public enum BackgroundType
    {
        Day,
        DayPartlyCloudy,
        Night,
        NightPartlyCloudy,
        Cloudy,
        Fog,
        Rain,
        Snow,
        Storm
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
