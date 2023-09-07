using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    public class WeatherSummary
    {
        public string LocationName { get; init; }

        public string CountryCode { get; init; }

        public string Admin1 { get; init; }

        public WeatherObservation CurrentWeather { get; init; }

        public WeatherObservation[] ForecastedWeather { get; init; }
    }
}
