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
                return new List<WeatherRecord>();

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<WeatherRecord>>(json) ?? new List<WeatherRecord>();
        }

        public static void AppendRecord(WeatherRecord record)
        {
            if (!string.Equals(record.city, "Vicenza", StringComparison.OrdinalIgnoreCase))
                return;

            string path = GetPath();
            var list = Load();

            list.Add(record);

            //solo gli ultimi 15 giorni
            DateTime cutoff = DateTime.Now.AddDays(-15);//oggi meno 15gg
            list = list.Where(r => r.timestamp >= cutoff).ToList();//prendo solo quelli degli ultimi 15 giorni, data>cutoff

            string output = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(path, output);
        }
    }
}

