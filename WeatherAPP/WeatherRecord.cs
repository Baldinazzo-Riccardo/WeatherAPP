using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    public class WeatherRecord
    {
        //rappresenta una registrazione in un preciso momento
        public DateTime timestamp { get; set; }//data di registrazione del dato
        public double temperature { get; set; }
        public double aqi { get; set; }//indice di qualità dell'aria
        public double pm25 { get; set; }//particelle inquinanti di diametro inferiore a 2.5 micrometri
        public double pm10 { get; set; }//particelle inquinanti di diametro inferiore a 10 micrometri
    }
}
