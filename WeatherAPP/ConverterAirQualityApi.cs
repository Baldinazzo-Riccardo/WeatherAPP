using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WeatherAPP
{
    internal class ConverterAirQualityApi
    {
        //classi per convertire i dati dell'API in oggetti C#
        //coord
        public Coordinates? coord { get; set; }//coordinate della citta

        //list
        public List<AirQualityItem>? list { get; set; }//lista di dati della qualita dell'aria
    }

    public class AirQualityItem
    {
        //main
        public AirQualityMain? main { get; set; }//AQI

        //components
        public AirQualityComponents? components { get; set; }//PM2.5 e PM10

        //dt
        public long dt { get; set; }//data di registrazione del dato
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
