using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;
using System.DirectoryServices.ActiveDirectory;
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
        System.Windows.Forms.Timer? clockTimer;//timer per aggiornare l'orario ogni secondo

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
            Bitmap bitmapCursor = Properties.Resources.cursor;
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



        }

        private void Loading(bool isForStart)
        {
            pnl_Loading.Visible = isForStart;

            if (isForStart)
            {
                guna2ProgressIndicator1.Start();//se true mostra la rotellina che carica
            }
            else
            {
                guna2ProgressIndicator1.Stop();//se false ferma la rotellina
            }


            Application.DoEvents();//aggiorna l'interfccia durante il caricamento, altrimenti si blocca tutto e non si vede nulla
        }

        //-----     TASK PER PRENDERE DATI DA API    ------

        private async Task<WeatherData> GetCurrentWeather(string cityName)
        {
            string url = $"{Api_Information.CURRENT_WEATHER_URL}" +
                $"?q={cityName}" +
                $"&appid={Api_Information.API_KEY}&units=metric";//costruisce il link per la richiesta API, con i parametri necessari (città, chiave API)


            using HttpClient httpClient = new();//crea un'istanza di HttpClient per fare la richiesta API
            HttpResponseMessage apiResponse = await httpClient.GetAsync(url);//fa la richiesta API e aspetta la risposta

            //controlla se è possibile e gestisce errori
            if (!isAPIAvailable(apiResponse))
            {
                throw new Exception("ERRORE: richiesta API fallita !");
            }

            string json = await apiResponse.Content.ReadAsStringAsync();//legge il contenuto della risposta API come stringa JSON

            var data = JsonConvert.DeserializeObject<ConverterWeatherApi>(json);//deserializza la stringa JSON in un oggetto ConverterWeatherApi

#pragma warning disable CS8602

            return new WeatherData//crea un nuovo oggetto WeatherData con i dati deserializzati
            {
                Temperature = data.main.temp,
                CurrentIcon = data.weather[0].icon,
                CityName = data.name,
                //converte il timestamp in un oggetto DateTimeOffset,
                //poi lo converte in un oggetto DateTime locale usando l'offset del fuso orario restituito dall'API
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
            //stessa logica di GetCurrentWeather ma con parametri diversi,per l'aria
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



        private async Task<ForecastData> GetForecastWeather(string cityName)
        {
            //classe per le previsioni di 4gg
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

            var data = JsonConvert.DeserializeObject<ConverterForecastApi>(json);

#pragma warning disable CS8602

            // ogni 8 elementi = 1 giorno (24h / 3h)
            var d1 = data.list[8];
            var d2 = data.list[16];
            var d3 = data.list[24];
            var d4 = data.list[32];

            return new ForecastData
            {
                Day1_Date = DateTimeOffset.FromUnixTimeSeconds(d1.dt).DateTime,
                Day1_Temp = d1.main.temp,
                Day1_Icon = d1.weather[0].icon,

                Day2_Date = DateTimeOffset.FromUnixTimeSeconds(d2.dt).DateTime,
                Day2_Temp = d2.main.temp,
                Day2_Icon = d2.weather[0].icon,

                Day3_Date = DateTimeOffset.FromUnixTimeSeconds(d3.dt).DateTime,
                Day3_Temp = d3.main.temp,
                Day3_Icon = d3.weather[0].icon,

                Day4_Date = DateTimeOffset.FromUnixTimeSeconds(d4.dt).DateTime,
                Day4_Temp = d4.main.temp,
                Day4_Icon = d4.weather[0].icon,
            };
#pragma warning restore CS8602
        }

        private bool isAPIAvailable(HttpResponseMessage apiResponse)
        {
            //controlla se la risposta API è valida e gestisce errori specifici come città non trovata o chiave API non valida
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

            Loading(true);//mostra il loader durante il caricamento dei dati iniziali


            try
            {
                WeatherData weather = await GetCurrentWeather(STARTING_CITY);//prende i dati meteo della città di partenza ecc.
                AirQualityData aqi = await GetAirQuality(weather.Latitude, weather.Longitude);
                ForecastData forecast = await GetForecastWeather(STARTING_CITY);


                LoadData(weatherData: weather, airQualityData: aqi, forecastData: forecast);//carica i dati nella UI


                if (weather.CurrentIcon != null)//imposta lo sfondo in base al meteo attuale
                {
                    SetBackground(currentWeather: weather.CurrentIcon);
                }

                StartClock(weather.DateTimeLocal);//avvia l'orologio con l'ora locale della città
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Loading(false);//nasconde il loader dopo aver caricato i dati
            }
        }


        private void SetBackground(string currentWeather)
        {
            switch (currentWeather)
            {
                //g: giorno sereno
                case "01d":
                    this.BackgroundImage = Properties.Resources.sunny_background;
                    break;
                //n: notte serena
                case "01n":
                    this.BackgroundImage = Properties.Resources.notte_serena;
                    break;

                //g: nuvoloso/parzialmente nuvoloso giorno
                case "02d":
                case "03d":
                case "04d":
                    this.BackgroundImage = Properties.Resources.Gemini_Generated_Image_9x0dav9x0dav9x0d__1_;
                    break;
                //n: nuvoloso/parzialmente nuvoloso notte
                case "02n":
                case "03n":
                case "04n":
                    this.BackgroundImage = Properties.Resources.notte_nuvolosa;
                    break;

                //g: pioggia giorno
                case "09d":
                case "10d":
                    this.BackgroundImage = Properties.Resources.rainy_background1;
                    break;
                //n: pioggia notte
                case "09n":
                case "10n":
                    this.BackgroundImage = Properties.Resources.pioggia_notte;
                    break;

                //g: neve giorno
                case "13d":
                    this.BackgroundImage = Properties.Resources.snowy_background;
                    break;
                //n: neve notte
                case "13n":
                    this.BackgroundImage = Properties.Resources.snowy_background;
                    break;

                //g: temporale giorno
                case "11d":
                    this.BackgroundImage = Properties.Resources.temptesta;
                    break;
                //n: temporale notte
                case "11n":
                    this.BackgroundImage = Properties.Resources.temporale_notte;
                    break;
                default:
                    this.BackgroundImage = Properties.Resources.sunny_background;
                    break;
            }

            this.BackgroundImageLayout = ImageLayout.Stretch;
            SetTransparency();
        }

        private void StartClock(DateTime dateTimeLocal)
        {
            currentCityTime = dateTimeLocal;//imposta l'ora locale della città come ora iniziale

            if (clockTimer != null)//se il timer esiste già, lo ferma e lo elimina per evitare timer multipli
            {
                clockTimer.Stop();
                clockTimer.Dispose();
            }

            clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };//crea un nuovo timer che scatta ogni secondo 
            clockTimer.Tick += (s, e) =>//ogni volta che il timer scatta, aggiorna l'ora aggiungendo un secondo all'ora attuale della città 
            {
                currentCityTime = currentCityTime.AddSeconds(1);
                lbl_time.Text = currentCityTime.ToString("HH:mm");
            };
            clockTimer.Start();
        }

        private void LoadData(WeatherData weatherData, AirQualityData airQualityData, ForecastData forecastData)
        {
            //carica i dati nella UI, impostando le etichette e le immagini in base ai dati ricevuti dalle API
            Loading(isForStart: true);
            this.pb_weather.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pb_weather.BackColor = Color.Transparent;

            this.lbl_temperature.Text = $"{weatherData.Temperature:F1}°C";
            this.lbl_time.Text = "Ora: " + weatherData.DateTimeLocal.ToString("HH:mm");

            this.pb_weather.ImageLocation = $"https://openweathermap.org/img/wn/{weatherData.CurrentIcon}@2x.png";
            this.lbl_city.Text = $"Previsioni Città: {weatherData.CityName}";

            this.lbl_AQI.Text = $"AQI: {airQualityData.AQI}";
            this.lbl_pm25.Text = $"PM 2.5: {airQualityData.PM25:F1}";
            this.lbl_PM10.Text = $"PM 10: {airQualityData.PM10:F1}";

            this.pb_day1.ImageLocation = $"https://openweathermap.org/img/wn/{forecastData.Day1_Icon}@2x.png";
            this.pb_day2.ImageLocation = $"https://openweathermap.org/img/wn/{forecastData.Day2_Icon}@2x.png";
            this.pb_day3.ImageLocation = $"https://openweathermap.org/img/wn/{forecastData.Day3_Icon}@2x.png";
            this.pb_day4.ImageLocation = $"https://openweathermap.org/img/wn/{forecastData.Day4_Icon}@2x.png";

            this.pb_day1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pb_day2.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pb_day3.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pb_day4.SizeMode = PictureBoxSizeMode.StretchImage;

            this.pb_day1.BackColor = Color.Transparent;
            this.pb_day2.BackColor = Color.Transparent;
            this.pb_day3.BackColor = Color.Transparent;
            this.pb_day4.BackColor = Color.Transparent;
            this.lbl_day1.Text = forecastData.Day1_Date.ToString("dddd, dd MMMM");
            this.lbl_temperatura_day1.Text = $"{forecastData.Day1_Temp:F1}°C";

            this.lbl_day2.Text = forecastData.Day2_Date.ToString("dddd, dd MMMM");
            this.lbl_temperatura_day2.Text = $"{forecastData.Day2_Temp:F1}°C";

            this.lbl_day3.Text = forecastData.Day3_Date.ToString("dddd, dd MMMM");
            this.lbl_temperatura_day3.Text = $"{forecastData.Day3_Temp:F1}°C";

            this.lbl_day4.Text = forecastData.Day4_Date.ToString("dddd, dd MMMM");
            this.lbl_temperatura_day4.Text = $"{forecastData.Day4_Temp:F1}°C";

            //salvataggio del record, con i dati attuali, in un file JSON per poterli visualizzare nei grafici
            WeatherRecord record = new()
            {
                city = weatherData.CityName,
                timestamp = weatherData.DateTimeLocal,
                temperature = weatherData.Temperature,
                aqi = airQualityData.AQI,
                pm25 = airQualityData.PM25,
                pm10 = airQualityData.PM10
            };
            JsonManager.AppendToday(record);
            JsonManager.AppendForecast(forecastData, airQualityData.AQI, airQualityData.PM25, airQualityData.PM10);
            //aggiunge il record al file JSON

            Loading(isForStart: false);
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.lbl_city.Focus();//imposta il focus sulla casella di testo della città all'avvio del form
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
                WeatherData weather = await GetCurrentWeather(city);//prende i dati meteo della città inserita ecc.
                AirQualityData aqi = await GetAirQuality(weather.Latitude, weather.Longitude);
                ForecastData forecast = await GetForecastWeather(city);

                LoadData(weatherData: weather, airQualityData: aqi, forecastData: forecast);//carica i dati nella UI

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


        private void guna2Button2_Click(object sender, EventArgs e)//per aprire form
        {
            //apre form con grafici e dati
            using (var graphsForm = new GraphsForm())
            {
                this.Hide();
                graphsForm.ShowDialog();
                this.Show();
            }
        }
    }
}
