using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    public class WeatherSummary
    {
        public WeatherObservation CurrentWeather { get; init; }

        public WeatherObservation[] ForecastedWeather { get; init; }
    }
}
