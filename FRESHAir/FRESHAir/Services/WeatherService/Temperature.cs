using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    public partial class Temperature : ObservableObject
    {
        public decimal Value => Scale switch
        {
            TemperatureScale.Celsius => ValueInCelsius,
            TemperatureScale.Fahrenheit => (ValueInCelsius * 9 / 5) + 32,
            TemperatureScale.Kelvin => (ValueInCelsius + (decimal)273.15),
            _ => throw new ArgumentOutOfRangeException()
        };

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Value))]
        private TemperatureScale scale;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Value))]
        private decimal valueInCelsius;

        public static Temperature FromCelsius(decimal value) => new()
        {
            ValueInCelsius = value
        };

        public static Temperature FromFahrenheit(decimal value) => new()
        {
            ValueInCelsius = Math.Round((value - 32) * 5 / 9)
        };

        public Temperature WithScale(TemperatureScale scale)
        {
            Scale = scale;
            return this;
        }
    }

    public enum TemperatureScale
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }
}
