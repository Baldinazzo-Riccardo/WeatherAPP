using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    public static class JsonManager
    {
        public static string GetPath()
        {
            return Path.Combine(Application.StartupPath, "Data", "weather_history.json");
        }


        public static List<WeatherRecord> Load()
        {
            string path = GetPath();

            if (!File.Exists(path))
                return new List<WeatherRecord>();//ritorna lista vuota

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<WeatherRecord>>(json) ?? new List<WeatherRecord>();
        }
        public static void AppendToday(WeatherRecord record)
        {
            var list = Load();//carica i dati esistenti

            list.RemoveAll(r => r.timestamp.Date == record.timestamp.Date);//rimuove eventuali record con la stessa data

            list.Add(record);//aggiunge il nuovo record

            Save(list);
        }
        public static void AppendForecast(ForecastData f, double aqi, double pm25, double pm10)
        {
            var list = Load();//carica i dati esistenti

            var forecasts = new[]//array di tuple con le date e le temperature dei 4 giorni
            {
                (f.Day1_Date, f.Day1_Temp),
                (f.Day2_Date, f.Day2_Temp),
                (f.Day3_Date, f.Day3_Temp),
                (f.Day4_Date, f.Day4_Temp),
            };
            //elimina eventuali duplicati e aggiunge i nuovi record come previsione
            foreach (var (date, temp) in forecasts)
            {
                list.RemoveAll(r => r.timestamp.Date == date.Date);

                list.Add(new WeatherRecord
                {
                    city = "Provincia di Vicenza",
                    timestamp = date,
                    temperature = temp,
                    aqi = aqi,//sara 3 perche api fornisce valori solo per giorno stesso
                    pm25 = pm25,
                    pm10 = pm10
                });
            }

            Save(list);
        }

        private static void Save(List<WeatherRecord> list)
        {
            //ordina i record per data e li salva in formato JSON con indentazione
            string output = JsonConvert.SerializeObject(
                list.OrderBy(r => r.timestamp),
                Formatting.Indented
            );

            File.WriteAllText(GetPath(), output);
        }
    }
}

