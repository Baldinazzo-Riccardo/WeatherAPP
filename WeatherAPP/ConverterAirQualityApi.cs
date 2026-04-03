using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    internal class ConverterAirQualityApi
    {
        //coord
        public Coordinates? Coord { get; set; }

        //list
        public List<AirQualityItem>? Items { get; set; }
    }

    public class AirQualityItem
    {
        //main
        public AirQualityMain? Main { get; set; }

        //components
        public AirQualityComponents? Components { get; set; }

        //dt
        public long Timestamp { get; set; }
    }

    public class AirQualityMain
    {
        //aqi
        public int AQI { get; set; }
    }

    public class AirQualityComponents
    {
        //pm2_5
        public double PM25 { get; set; }

        //pm10
        public double PM10 { get; set; }
    }
}
