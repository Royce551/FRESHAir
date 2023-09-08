using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    internal class DummyWeatherProvider : IWeatherProvider
    {
        public HttpClient HttpClient { get; set; } = default!;

        public async Task<WeatherSummary> GetWeatherSummaryAsync(string location, TemperatureScale temperatureScale)
        {
            return new WeatherSummary()
            {
                LocationName = "Shishiro",
                Admin1 = "Kemonomimia",
                CountryCode = "KR",
                CurrentWeather = new WeatherObservation()
                {
                    CurrentTemperature = Temperature.FromFahrenheit(87),
                    RelativeHumidity = 78.2M,
                    Time = new DateTime(2023, 9, 7, 1, 0, 0),
                    Icon = "avares://FRESHAir/Assets/WeatherIcons/clearsky_day.png"
                },
                ForecastedWeather = new WeatherObservation[]
                {
                    new WeatherObservation()
                    {
                        CurrentTemperature = Temperature.FromFahrenheit(82),
                        RelativeHumidity = 78.2M,
                        Time = new DateTime(2023, 9, 7, 2, 0, 0),
                        Icon = "avares://FRESHAir/Assets/WeatherIcons/clearsky_day.png"
                    },
                    new WeatherObservation()
                    {
                        CurrentTemperature = Temperature.FromFahrenheit(76),
                        RelativeHumidity = 78.2M,
                        Time = new DateTime(2023, 9, 7, 3, 0, 0),
                        Icon = "avares://FRESHAir/Assets/WeatherIcons/clearsky_day.png"
                    },
                    new WeatherObservation()
                    {
                        CurrentTemperature = Temperature.FromFahrenheit(70),
                        RelativeHumidity = 78.2M,
                        Time = new DateTime(2023, 9, 8, 2, 0, 0),
                        Icon = "avares://Assets/WeatherIcons/clearsky_day.png"
                    },
                    new WeatherObservation()
                    {
                        CurrentTemperature = Temperature.FromFahrenheit(60),
                        RelativeHumidity = 78.2M,
                        Time = new DateTime(2023, 9, 9, 2, 0, 0),
                        Icon = "avares://FRESHAir/Assets/WeatherIcons/clearsky_day.png"
                    },
                }
            };
        }
    }
}
