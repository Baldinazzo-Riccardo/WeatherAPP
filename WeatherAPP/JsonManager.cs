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

