using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    internal static class Api_Information
    {
        //informazioni per richieste di api
        public const string API_KEY = "7185f49e692f9145fc688a687eaf2d2f";//chiave per poter utlizzare i servizi openweathermap

        public const string CURRENT_WEATHER_URL = "https://api.openweathermap.org/data/2.5/weather";//meteo attuale

        public const string AIR_QUALITY_URL = "https://api.openweathermap.org/data/2.5/air_pollution";//qualita dell'aria

        public const string FORECAST_WEATHER_URL = "https://api.openweathermap.org/data/2.5/forecast";//previsioni
    }
}
