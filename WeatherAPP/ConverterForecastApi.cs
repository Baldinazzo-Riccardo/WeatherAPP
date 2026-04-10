using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    internal class ConverterForecastApi
    {
        //list
        public List<ForecastItem>? list { get; set; }

        //city
        public ForecastCity? city { get; set; }
    }

    public class ForecastItem
    {
        //dt
        public long dt { get; set; }

        //main
        public ForecastMain? main { get; set; }

        //weather
        public List<ForecastWeather>? weather { get; set; }
    }

    public class ForecastMain
    {
        //temp
        public double temp { get; set; }
    }

    public class ForecastWeather
    {
        //icon
        public string? icon { get; set; }//es "01d"
    }

    public class ForecastCity
    {
        //name
        public string? name { get; set; }

        //timezone
        public int timezone { get; set; }//orario locale della citta in secondi rispetto a UTC
    }

}

