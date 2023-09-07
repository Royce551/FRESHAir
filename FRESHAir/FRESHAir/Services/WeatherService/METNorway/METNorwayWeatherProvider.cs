using System;
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

        public async Task<WeatherSummary> GetWeatherSummaryAsync(string location)
        {
            var response = await HttpClient.PostAsync("https://api-thetroposphere.vicr123.com/api/locations/search", new StringContent($"{{\"query\":\"{location}\"}}", Encoding.UTF8, "application/json"));

            var responseAsString = await response.Content.ReadAsStringAsync();
            var possibleLocations = JsonSerializer.Deserialize<List<TheTroposphereLocation>>(responseAsString);

            var locationToUse = possibleLocations[0];

            try
            {
                var forecast = await HttpClient.GetFromJsonAsync<METNorwayWeatherResponse>($"https://api.met.no/weatherapi/locationforecast/2.0/complete?lat={locationToUse.Latitude}&lon={locationToUse.Longitude}");
                Console.WriteLine(forecast);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           

            throw new NotImplementedException();
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
    }

    public class METNorwayWeatherTimeseriesInstantData
    {
        [JsonPropertyName("details")]
        public METNorwayWeatherObservation Observation { get; set; }
    }

    public class METNorwayWeatherObservation
    {
        [JsonPropertyName("air_temperature")]
        public decimal AirTemperature { get; set; }

        [JsonPropertyName("air_pressure_at_sea_level")]
        public decimal AirPressureAtSeaLevel { get; set; }

        [JsonPropertyName("relative_humidity")]
        public decimal RelativeHumidity { get; set; }

        [JsonPropertyName("wind_from_direction")]
        public decimal WindFromDirection { get; set; }

        [JsonPropertyName("wind_speed")]
        public decimal WindSpeed { get; set; }

        [JsonPropertyName("wind_speed_of_gust")]
        public decimal WindGustSpeed { get; set; }
    }
}
