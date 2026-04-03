using Newtonsoft.Json;

namespace WeatherAPP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetProprierties();
        }


        private void SetProprierties()
        {

        }


        private void Loading(bool isForStart)    //metodo per impostare schermata caricamento
        {
            //visibile quando start e spento quando stop
            this.guna2Panel1.Visible = isForStart;

            if (isForStart)
            {
                this.guna2ProgressIndicator1.Start();
            }
            else
            {
                this.guna2ProgressIndicator1.Stop();
            }
        }

        //-----     TASK PER PRENDERE DATI DA API    ------

        private async Task<WeatherData> GetCurrentWeather(string cityName)
        {
            string url = $"{Api_Information.CURRENT_WEATHER_URL}" +
                $"?q={cityName}" +
                $"&appid{Api_Information.API_KEY}&units=metric";


            using HttpClient httpClient = new();
            HttpResponseMessage apiResponse = await httpClient.GetAsync(url);

            //eccezione se non va a buon fine
            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new Exception("ERRORE: richiesta API non riuscita !");
            }

            string json = await apiResponse.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ConverterWeatherApi>(json);

#pragma warning disable CS8602

            return new WeatherData
            {
                Temperature = data.Main.Temperature,
                CurrentIcon = data.Weather[0].Icon,
                DateTimeLocal = DateTimeOffset
                    .FromUnixTimeSeconds(data.Timestamp)
                    .ToOffset(TimeSpan.FromSeconds(data.Timezone))
                    .DateTime,
                Latitude = data.Coord.Latitude,
                Longitude = data.Coord.Longitude,
            };
#pragma warning restore CS8602
        }


        private async Task<AirQualityData> GetAirQuality(double latitude, double longitude)
        {
            string url = $"{Api_Information.AIR_QUALITY_URL}" +
                $"?lat={latitude}" +
                $"&lon={longitude}" +
                $"&appid{Api_Information.API_KEY}";


            using HttpClient httpClient = new();
            HttpResponseMessage apiResponse = await httpClient.GetAsync(url);

            //eccezione se non va a buon fine
            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new Exception("ERRORE: richiesta API non riuscita !");
            }

            string json = await apiResponse.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ConverterAirQualityApi>(json);

#pragma warning disable CS8602
            return new AirQualityData
            {
                AQI = data.Items[0].Main.AQI,
                PM25 = data.Items[0].Components.PM25,
                PM10 = data.Items[0].Components.PM10
            };
#pragma warning restore CS8602
        }

        private async Task<WeatherData> GetForecastWeather(string cityName)
        {
            string url = $"{Api_Information.FORECAST_WEATHER_URL}" +
                $"?q={cityName}" +
                $"&appid{Api_Information.API_KEY}&units=metric";

            using HttpClient httpClient = new();
            HttpResponseMessage apiResponse = await httpClient.GetAsync(url);

            //eccezione se non va a buon fine
            if (!apiResponse.IsSuccessStatusCode)
            {
                throw new Exception("ERRORE: richiesta API non riuscita !");
            }

            string json = await apiResponse.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ConverterWeatherApi>(json);

#pragma warning disable CS8602

            return new WeatherData
            {
                Temperature = data.Main.Temperature,
                CurrentIcon = data.Weather[0].Icon,
                DateTimeLocal = DateTimeOffset
                    .FromUnixTimeSeconds(data.Timestamp)
                    .ToOffset(TimeSpan.FromSeconds(data.Timezone))
                    .DateTime,
                Latitude = data.Coord.Latitude,
                Longitude = data.Coord.Longitude,
            };
#pragma warning restore CS8602
        }

    }
}
