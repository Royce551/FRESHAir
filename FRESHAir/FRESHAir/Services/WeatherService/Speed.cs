using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRESHAir.Services.WeatherService
{
    public partial class Speed : ObservableObject
    {
        public decimal Value => Scale switch
        {
            SpeedScale.KilometersPerHour => ValueInKilometersPerHour,
            SpeedScale.MilesPerHour => (ValueInKilometersPerHour / 1.609344M),
            _ => throw new ArgumentOutOfRangeException()
        };

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Value))]
        private SpeedScale scale;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Value))]
        private decimal valueInKilometersPerHour;

        public static Speed FromKilometersPerHour(decimal value) => new()
        {
            ValueInKilometersPerHour = value
        };

        public static Speed FromMilesPerHour(decimal value) => new()
        {
            ValueInKilometersPerHour = value * 1.609344M
        };

        public static Speed FromMetersPerSecond(decimal value) => new()
        {
            ValueInKilometersPerHour = value * 3.6M
        };

        public Speed WithScale(SpeedScale scale)
        {
            Scale = scale;
            return this;
        }
    }

    public enum SpeedScale
    {
        MilesPerHour,
        KilometersPerHour
    }
}
