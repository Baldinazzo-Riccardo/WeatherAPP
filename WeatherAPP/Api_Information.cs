using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    internal static class Api_Information
    {
        //informazioni per richieste di api
        public const string API_KEY = "LA_TUA_API_KEY";

        public const string CURRENT_WEATHER_URL = "https://api.openweathermap.org/data/2.5/weather";

        public const string AIR_QUALITY_URL = "https://api.openweathermap.org/data/2.5/air_pollution";

        public const string FORECAST_WEATHER_URL = "https://api.openweathermap.org/data/2.5/forecast";
    }
}
