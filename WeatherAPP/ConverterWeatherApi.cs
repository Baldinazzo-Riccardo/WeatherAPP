using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    internal class ConverterWeatherApi
    {
        //coord
        public Coordinates? coord { get; set; }

        //weather
        public List<WeatherInfo>? weather { get; set; }

        //main
        public WeatherMain? main { get; set; }

        //dt
        public long dt { get; set; }

        //timezone
        public int timezone { get; set; }
        //name per label citta
        public string name { get; set; }
    }

    public class Coordinates
    {
        //lon
        public double lon { get; set; }

        //lat
        public double lat { get; set; }
    }

    public class WeatherInfo
    {
        //icon
        public string? icon { get; set; }
    }

    public class WeatherMain
    {
        //temp
        public double temp { get; set; }
    }
}
