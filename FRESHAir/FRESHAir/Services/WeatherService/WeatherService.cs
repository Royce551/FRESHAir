using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    public class WeatherService
    {
        public static HttpClient HttpClient { get; set; }

        public static IWeatherProvider CurrentWeatherProvider { get; private set; } = default!;

        static WeatherService()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("FRESHAir/1.0.0 (https://github.com/royce551/freshair)");
        }

        public static async Task<WeatherSummary> GetWeatherSummaryAsync(string location) => await CurrentWeatherProvider.GetWeatherSummaryAsync(location);
    }
}
