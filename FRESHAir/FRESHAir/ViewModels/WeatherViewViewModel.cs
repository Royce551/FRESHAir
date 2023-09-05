using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.ViewModels
{
    public partial class WeatherViewViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string locationName = "Lafayette";

        [ObservableProperty]
        private Temperature currentTemperature = Temperature.FromCelsius(29);

        private TemperatureScale scale;
        public TemperatureScale Scale
        {
            get => scale;
            set
            {
                CurrentTemperature.Scale = value;
                SetProperty(ref scale, value);
            }
        }

        public void ToggleTemperatureScale()
        {
            if (Scale != TemperatureScale.Kelvin)
                Scale = (TemperatureScale)(Scale + 1);
            else Scale = TemperatureScale.Celsius;
        }
    }

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
            ValueInCelsius = (value - 32) * 5 / 9
        };
    }

    public enum TemperatureScale
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }
}
