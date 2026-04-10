using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    internal class AirQualityData
    {
        //rappresenta la qualita dell'aria in un certo momento, utile per i dati attuali presi dall'API
        public int AQI { get; set; }
        public double PM25 { get; set; }
        public double PM10 { get; set; }
    }
}
