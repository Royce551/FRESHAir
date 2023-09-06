using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    public class WeatherObservation
    {
        public Temperature CurrentTemperature { get; init; }

        public decimal RelativeHumidity { get; init; }
    }
}
