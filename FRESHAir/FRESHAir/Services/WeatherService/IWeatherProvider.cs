using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    public interface IWeatherProvider
    {
        public HttpClient HttpClient { get; }

        public Task<WeatherSummary> GetWeatherSummaryAsync(string location, TemperatureScale temperatureScale);
    }
}
