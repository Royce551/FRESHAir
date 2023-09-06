using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService.METNorwayWeatherProvider
{
    public class METNorwayWeatherProvider : IWeatherProvider
    {
        public HttpClient HttpClient { get; } = default!;
    }
}
