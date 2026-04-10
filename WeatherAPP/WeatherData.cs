using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    public class WeatherData
    {
        //classe che contiene i dati meteo attuali
        public string CityName { get; set; }
        public double Temperature { get; set; }
        public string? CurrentIcon { get; set; }
        public DateTime DateTimeLocal { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
