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
}
