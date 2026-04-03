using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    internal class ConverterWeatherApi
    {
        //coord
        public Coordinates? Coord { get; set; }

        //weather
        public List<WeatherInfo>? Weather { get; set; }

        //main
        public WeatherMain? Main { get; set; }

        //dt
        public long Timestamp { get; set; }

        //timezone
        public int Timezone { get; set; }
    }

    public class Coordinates
    {
        //lon
        public double Longitude { get; set; }

        //lat
        public double Latitude { get; set; }
    }

    public class WeatherInfo
    {
        //icon
        public string? Icon { get; set; }
    }

    public class WeatherMain
    {
        //temp
        public double Temperature { get; set; }
    }
}