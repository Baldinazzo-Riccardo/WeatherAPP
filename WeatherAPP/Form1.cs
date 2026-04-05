using Newtonsoft.Json;
using WeatherAPP.Properties;
using static Guna.UI2.WinForms.Suite.Descriptions;

namespace WeatherAPP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetProprierties();
        }

        const string STARTING_CITY = "Rome";

        private void SetProprierties()
        {
            this.pnl_Loading.Size = this.Size;

            SetTransparency();

            this.pnl_Loading.Visible = false;   //nascondo il pannello del loader

            //centro il pannello del loader
            this.pnl_Loading.Location = new Point(
                (this.ClientSize.Width - this.pnl_Loading.Width) / 2,
                (this.ClientSize.Height - this.pnl_Loading.Height) / 2
            );


            //impostare il cursore con controllo eccezzione
            Bitmap bitmapCursor = new(Path.Combine(Application.StartupPath, "img", "cursor.png"));

            if (bitmapCursor != null)
            {
                IntPtr cur = bitmapCursor.GetHicon();
                this.Cursor = new Cursor(cur);
            }


            //centro il loader nel panello
            this.guna2ProgressIndicator1.Location = new Point(
                (this.pnl_Loading.Width - this.guna2ProgressIndicator1.Width) / 2,
                (this.pnl_Loading.Height - this.guna2ProgressIndicator1.Height) / 2
            );

            //this.guna2ProgressIndicator1.Start();

            lbl_title.Focus();

        }


        private void Loading(bool isForStart)    //metodo per impostare schermata caricamento
        {

            //visibile quando start e spento quando stop
            this.pnl_Loading.Visible = isForStart;

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

        private void MainForm_Load(object sender, EventArgs e)
        {
            //deve prendere dati della citta di base + ora
            SetTransparency();

            //avvio schermata caricamento
            //Loading(isForStart: true);

            //uso task per non bloccare l'interfaccia utente

            //blocco caricamento
            //Loading(isForStart: false);
        }

        private void SetBackground(string currentWeather)
        {
            Loading(isForStart: true);

            switch (currentWeather.ToLower())
            {
                case "clear":
                    this.BackgroundImage = null;
                    break;
                case "sunny":
                    this.BackgroundImage = null;
                    break;
                case "cloudy":
                    this.BackgroundImage = null;
                    break;
                case "rainy":
                    this.BackgroundImage = null;
                    break;
                case "snowy":
                    this.BackgroundImage = null;
                    break;
                case "storm":
                    this.BackgroundImage = null;
                    break;
                default:
                    this.BackgroundImage = null;
                    break;
            }

            this.BackgroundImageLayout = ImageLayout.Stretch; //<------- occhio !!!!

            //sistemo la trasparenza
            SetTransparency();

            Loading(isForStart: false);
        }

        private void SetTransparency()  //sistemo la trasparenza
        {
            this.panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            this.pnl_Loading.BackColor = Color.FromArgb(180, 0, 0, 0);
            this.guna2ProgressIndicator1.BackColor = Color.FromArgb(0, 0, 0, 0);
        }

        private void LoadData(WeatherData weatherData, AirQualityData airQualityData)
        {
            Loading(isForStart: true);

            //carico i dati nella schermata

            Loading(isForStart: false);
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //ricerca citta con controllo se esiste

        }
    }
}
