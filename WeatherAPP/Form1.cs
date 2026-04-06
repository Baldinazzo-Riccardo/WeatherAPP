using Microsoft.VisualBasic.Logging;
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

        const string STARTING_CITY = "Vicenza";
        DateTime currentCityTime;
        System.Windows.Forms.Timer? clockTimer;

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

            //centro il pannello contenente i dati
            this.pnl_data.Location = new Point(
                (this.ClientSize.Width - this.pnl_data.Width) / 2,
                (this.ClientSize.Height - this.pnl_data.Height) / 2
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

        private void Loading(bool isForStart)
        {
            pnl_Loading.Visible = isForStart;

            if (isForStart)
            {
                guna2ProgressIndicator1.Start();
            }
            else
            {
                guna2ProgressIndicator1.Stop();
            }


            Application.DoEvents();
        }

        //-----     TASK PER PRENDERE DATI DA API    ------

        private async Task<WeatherData> GetCurrentWeather(string cityName)
        {
            string url = $"{Api_Information.CURRENT_WEATHER_URL}" +
                $"?q={cityName}" +
                $"&appid={Api_Information.API_KEY}&units=metric";


            using HttpClient httpClient = new();
            HttpResponseMessage apiResponse = await httpClient.GetAsync(url);

            //controlla se è possibile e gestisce errori
            if (!isAPIAvailable(apiResponse))
            {
                throw new Exception("ERRORE: richiesta API fallita !");
            }

            string json = await apiResponse.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ConverterWeatherApi>(json);

#pragma warning disable CS8602

            return new WeatherData
            {
                Temperature = data.main.temp,
                CurrentIcon = data.weather[0].icon,
                DateTimeLocal = DateTimeOffset
                    .FromUnixTimeSeconds(data.dt)
                    .ToOffset(TimeSpan.FromSeconds(data.timezone))
                    .DateTime,
                Latitude = data.coord.lat,
                Longitude = data.coord.lon,
            };
#pragma warning restore CS8602
        }


        private async Task<AirQualityData> GetAirQuality(double latitude, double longitude)
        {
            string url = $"{Api_Information.AIR_QUALITY_URL}" +
                $"?lat={latitude}" +
                $"&lon={longitude}" +
                $"&appid={Api_Information.API_KEY}";


            using HttpClient httpClient = new();
            HttpResponseMessage apiResponse = await httpClient.GetAsync(url);

            //controlla se è possibile e gestisce errori
            if (!isAPIAvailable(apiResponse))
            {
                throw new Exception("ERRORE: richiesta API fallita !");
            }

            string json = await apiResponse.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ConverterAirQualityApi>(json);

#pragma warning disable CS8602
            return new AirQualityData
            {
                AQI = data.list[0].main.aqi,
                PM25 = data.list[0].components.pm2_5,
                PM10 = data.list[0].components.pm10
            };
#pragma warning restore CS8602
        }



        private async Task<WeatherData> GetForecastWeather(string cityName)
        {
            string url = $"{Api_Information.FORECAST_WEATHER_URL}" +
                $"?q={cityName}" +
                $"&appid={Api_Information.API_KEY}&units=metric";

            using HttpClient httpClient = new();
            HttpResponseMessage apiResponse = await httpClient.GetAsync(url);

            //controlla se è possibile e gestisce errori
            if (!isAPIAvailable(apiResponse))
            {
                throw new Exception("ERRORE: richiesta API fallita !");
            }

            string json = await apiResponse.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ConverterWeatherApi>(json);

#pragma warning disable CS8602

            return new WeatherData
            {
                Temperature = data.main.temp,
                CurrentIcon = data.weather[0].icon,
                DateTimeLocal = DateTimeOffset
                    .FromUnixTimeSeconds(data.dt)
                    .ToOffset(TimeSpan.FromSeconds(data.timezone))
                    .DateTime,
                Latitude = data.coord.lat,
                Longitude = data.coord.lon,
            };
#pragma warning restore CS8602
        }

        private bool isAPIAvailable(HttpResponseMessage apiResponse)
        {
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("ERRORE: città non trovata !");
                return false;
            }

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("ERRORE: API KEY non valida !");
                return false;
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                MessageBox.Show($"ERRORE API: {apiResponse.StatusCode}");
                return false;
            }

            return true;
        }

        private void SetTransparency()  //sistemo la trasparenza
        {
            this.pnl_data.BackColor = Color.FromArgb(100, 0, 0, 0);
            this.pnl_Loading.BackColor = Color.FromArgb(180, 0, 0, 0);
            this.guna2ProgressIndicator1.BackColor = Color.FromArgb(0, 0, 0, 0);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            //deve prendere dati della citta di base + ora
            SetTransparency();

            Loading(true);


            try
            {
                WeatherData weather = await GetCurrentWeather(STARTING_CITY);
                AirQualityData aqi = await GetAirQuality(weather.Latitude, weather.Longitude);

                LoadData(weatherData: weather, airQualityData: aqi);

                if (weather.CurrentIcon != null)
                {
                    SetBackground(currentWeather: weather.CurrentIcon);
                }

                StartClock(weather.DateTimeLocal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Loading(false);
            }
        }


        private void SetBackground(string currentWeather)
        {

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

            if (this.BackgroundImage == null)
            {
                MessageBox.Show("ERRORE: impossibile trovare lo sfondo !");
                return;
            }

            this.BackgroundImageLayout = ImageLayout.Stretch; //<------- occhio !!!!

            //sistemo la trasparenza
            SetTransparency();

        }

        private void StartClock(DateTime dateTimeLocal)
        {
            currentCityTime = dateTimeLocal;

            if (clockTimer != null)
            {
                clockTimer.Stop();
                clockTimer.Dispose();
            }

            clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            clockTimer.Tick += (s, e) =>
            {
                currentCityTime = currentCityTime.AddSeconds(1);
                //lbl_time.Text = currentCityTime.ToString("HH:mm");
            };
            clockTimer.Start();
        }

        private void LoadData(WeatherData weatherData, AirQualityData airQualityData)
        {
            Loading(isForStart: true);


            //gradi - ora attuale - icona clima
            //lbl_temperature.Text = $"{weatherData.Temperature:F1}°C";
            //lbl_time.Text = weatherData.DateTimeLocal.ToString("HH:mm");

            //prende foto da sito
            //pb_weather.ImageLocation = $"https://openweathermap.org/img/wn/{weatherData.CurrentIcon}@2x.png";   

            //dati inquinamento
            //lbl_AQI.Text = $"AQI: {airQualityData.AQI}";
            //lbl_PM25.Text = $"PM2.5: {airQualityData.PM25:F1}";
            //lbl_PM10.Text = $"PM10: {airQualityData.PM10:F1}";


            Loading(isForStart: false);
        }



        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.lbl_title.Focus();
        }

        private async void guna2Button1_Click_1(object sender, EventArgs e)
        {
            //ricerca citta con controllo se esiste
            string city = guna2TextBox1.Text.Trim();


            if (string.IsNullOrEmpty(city))
            {
                return;
            }

            //controllo se la città esiste facendo una richiesta API, se non esiste mostra messaggio di errore


            Loading(true);

            try
            {
                WeatherData weather = await GetCurrentWeather(city);
                AirQualityData aqi = await GetAirQuality(weather.Latitude, weather.Longitude);

                LoadData(weatherData: weather, airQualityData: aqi);

                if (weather.CurrentIcon != null)
                {
                    SetBackground(currentWeather: weather.CurrentIcon);
                }

                StartClock(weather.DateTimeLocal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Loading(false);
            }
        }
    }
}
