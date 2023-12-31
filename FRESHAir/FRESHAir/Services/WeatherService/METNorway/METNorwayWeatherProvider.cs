﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService.METNorwayWeatherProvider
{
    public class METNorwayWeatherProvider : IWeatherProvider
    {
        public HttpClient HttpClient { get; init; } = default!;

        public async Task<WeatherSummary> GetWeatherSummaryAsync(string location, TemperatureScale temperatureScale, SpeedScale speedScale)
        {
            var response = await HttpClient.PostAsync("https://api-thetroposphere.vicr123.com/api/locations/search", new StringContent($"{{\"query\":\"{location}\"}}", Encoding.UTF8, "application/json"));

            var responseAsString = await response.Content.ReadAsStringAsync();
            var possibleLocations = JsonSerializer.Deserialize<List<TheTroposphereLocation>>(responseAsString) ?? throw new Exception();

            var locationToUse = possibleLocations[0];

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(locationToUse.TimezoneString);

            var forecast = await HttpClient.GetFromJsonAsync<METNorwayWeatherResponse>($"https://api.met.no/weatherapi/locationforecast/2.0/complete?lat={locationToUse.Latitude}&lon={locationToUse.Longitude}") ?? throw new Exception();

            

            var weatherNow = forecast.Properties.Timeseries[0];
            var weatherForecast = forecast.Properties.Timeseries.Take(new Range(new(1), new(forecast.Properties.Timeseries.Length)));

            List<WeatherObservation> forecastAsGeneralWeatherObservation = new();

            foreach (var forecastTime in weatherForecast)
            {
                forecastAsGeneralWeatherObservation.Add(new WeatherObservation()
                {
                    CurrentTemperature = Temperature.FromCelsius(forecastTime.Data.CurrentData.Observation.AirTemperature).WithScale(temperatureScale),
                    RelativeHumidity = forecastTime.Data.CurrentData.Observation.RelativeHumidity,
                    Time = TimeZoneInfo.ConvertTimeFromUtc(forecastTime.Time, timeZone),
                    Icon = $"avares://FRESHAir/Assets/WeatherIcons/{forecastTime.Data.NextHour?.Summary.SymbolCode}.png",
                    DewPoint = Temperature.FromCelsius(forecastTime.Data.CurrentData.Observation.DewPoint).WithScale(temperatureScale),
                    CloudArea = forecastTime.Data.CurrentData.Observation.CloudArea,
                    WindSpeed = Speed.FromMetersPerSecond(forecastTime.Data.CurrentData.Observation.WindSpeed).WithScale(speedScale),
                    WindGustSpeed = Speed.FromMetersPerSecond(forecastTime.Data.CurrentData.Observation.WindGustSpeed).WithScale(speedScale),
                    WindFromDirection = forecastTime.Data.CurrentData.Observation.WindFromDirection,
                    UVIndex = forecastTime.Data.CurrentData.Observation.UVIndex
                });
            }

            return new WeatherSummary()
            {
                CurrentWeather = new WeatherObservation()
                {
                    CurrentTemperature = Temperature.FromCelsius(weatherNow.Data.CurrentData.Observation.AirTemperature).WithScale(temperatureScale),
                    RelativeHumidity = weatherNow.Data.CurrentData.Observation.RelativeHumidity,
                    Time = TimeZoneInfo.ConvertTimeFromUtc(weatherNow.Time, timeZone),
                    Icon = $"avares://FRESHAir/Assets/WeatherIcons/{weatherNow.Data.NextHour.Summary.SymbolCode}.png",
                    DewPoint = Temperature.FromCelsius(weatherNow.Data.CurrentData.Observation.DewPoint).WithScale(temperatureScale),
                    CloudArea = weatherNow.Data.CurrentData.Observation.CloudArea,
                    WindSpeed = Speed.FromMetersPerSecond(weatherNow.Data.CurrentData.Observation.WindSpeed).WithScale(speedScale),
                    WindGustSpeed = Speed.FromMetersPerSecond(weatherNow.Data.CurrentData.Observation.WindGustSpeed).WithScale(speedScale),
                    WindFromDirection = weatherNow.Data.CurrentData.Observation.WindFromDirection,
                    UVIndex = weatherNow.Data.CurrentData.Observation.UVIndex
                },
                ForecastedWeather = forecastAsGeneralWeatherObservation.ToArray(),
                LocationName = locationToUse.Name,
                Admin1 = locationToUse.Admin1,
                CountryCode = locationToUse.CountryCode
            };
        }
    }

    public class TheTroposphereLocation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("lat")]
        public decimal Latitude { get; set; }

        [JsonPropertyName("lng")]
        public decimal Longitude { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; } = default!;

        [JsonPropertyName("admin1")]
        public string Admin1 { get; set; } = default!;

        [JsonPropertyName("timezone")]
        public string TimezoneString { get; set; } = default!;
    }

    public class METNorwayWeatherResponse
    {
        [JsonPropertyName("properties")]
        public METNorwayWeatherResponseProperties Properties { get; set; }
    }

    public class METNorwayWeatherResponseProperties
    {
        [JsonPropertyName("meta")]
        public METNorwayWeatherResponseMetadata Metadata { get; set; }

        [JsonPropertyName("timeseries")]
        public METNorwayWeatherTimeseries[] Timeseries { get; set; }
    }

    public class METNorwayWeatherResponseMetadata
    {
        [JsonPropertyName("updated_at")]
        public DateTime LastUpdated { get; set; }

        // TODO: handle units
    }

    public class METNorwayWeatherTimeseries
    {
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("data")]
        public METNorwayWeatherTimeseriesData Data { get; set; }
    }

    public class METNorwayWeatherTimeseriesData
    {
        [JsonPropertyName("instant")]
        public METNorwayWeatherTimeseriesInstantData CurrentData { get; set; }

        [JsonPropertyName("next_12_hours")]
        public METNorwayWeatherTimeseriesFutureData Next12Hours { get; set; }

        [JsonPropertyName("next_1_hours")]
        public METNorwayWeatherTimeseriesFutureData NextHour { get; set; }
    }

    public class METNorwayWeatherTimeseriesInstantData
    {
        [JsonPropertyName("details")]
        public METNorwayWeatherObservation Observation { get; set; }
    }

    public class METNorwayWeatherTimeseriesFutureData
    {
        [JsonPropertyName("summary")]
        public METNorwayWeatherTimeseriesFutureSummaryData Summary { get; set; }
    }

    public class METNorwayWeatherTimeseriesFutureSummaryData
    {
        [JsonPropertyName("symbol_code")]
        public string SymbolCode { get; set; }
    }

    public class METNorwayWeatherObservation
    {
        [JsonPropertyName("air_temperature")]
        public decimal AirTemperature { get; set; }

        [JsonPropertyName("air_pressure_at_sea_level")]
        public decimal AirPressureAtSeaLevel { get; set; }

        [JsonPropertyName("relative_humidity")]
        public decimal RelativeHumidity { get; set; }

        [JsonPropertyName("dew_point_temperature")]
        public decimal DewPoint { get; set; }

        [JsonPropertyName("cloud_area_fraction")]
        public decimal CloudArea { get; set; }

        [JsonPropertyName("wind_from_direction")]
        public decimal WindFromDirection { get; set; }

        [JsonPropertyName("wind_speed")]
        public decimal WindSpeed { get; set; }

        [JsonPropertyName("wind_speed_of_gust")]
        public decimal WindGustSpeed { get; set; }

        [JsonPropertyName("ultraviolet_index_clear_sky")]
        public decimal UVIndex { get; set; }
    }
}
