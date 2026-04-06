using System.Collections.Generic;

namespace WeatherAPP
{
    internal class ConverterAirQualityApi
    {
        //coord
        public Coordinates? coord { get; set; }

        //list
        public List<AirQualityItem>? list { get; set; }
    }

    public class AirQualityItem
    {
        //main
        public AirQualityMain? main { get; set; }

        //components
        public AirQualityComponents? components { get; set; }

        //dt
        public long dt { get; set; }
    }

    public class AirQualityMain
    {
        //aqi
        public int aqi { get; set; }
    }

    public class AirQualityComponents
    {
        //pm2_5
        public double pm2_5 { get; set; }

        //pm10
        public double pm10 { get; set; }
    }
}
