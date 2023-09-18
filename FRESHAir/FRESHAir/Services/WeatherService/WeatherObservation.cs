using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    public class WeatherObservation
    {
        public DateTime Time { get; init; }

        public Temperature CurrentTemperature { get; init; }

        public decimal RelativeHumidity { get; init; }

        public string Icon { get; init; }

        public Temperature DewPoint { get; init; }

        public decimal CloudArea { get; init; }

        public Speed WindSpeed { get; init; }

        public Speed WindGustSpeed { get; init; }

        public decimal WindFromDirection { get; init; }

        public decimal UVIndex { get; init; }
    }
}
