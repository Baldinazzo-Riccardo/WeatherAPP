using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    public class WeatherRecord
    {
        public DateTime timestamp { get; set; }
        public double temperature { get; set; }
        public double aqi { get; set; }
        public double pm25 { get; set; }
        public double pm10 { get; set; }
    }
}
