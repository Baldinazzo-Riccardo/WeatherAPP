using Newtonsoft.Json;
using ScottPlot;
using ScottPlot.WinForms;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WeatherAPP
{
    public partial class GraphsForm : Form
    {
        public GraphsForm()
        {
            InitializeComponent();
            sethandlers();
            hideallplots();
            loadhistoryandplot();
        }
        //converte ARGB in ScottPlot.Color perche ScottPlot usa un formato diverso da System.Drawing.Color
        private ScottPlot.Color col(uint argb) =>
            ScottPlot.Color.FromARGB(argb);

        // colore barra AQI in base al livello
        private ScottPlot.Color aqicolor(double value)
        {
            if (value <= 1) return col(0xFF00E400);
            if (value <= 2) return col(0xFFFFFF00);
            if (value <= 3) return col(0xFFFF7E00);
            if (value <= 4) return col(0xFFFF0000);
            return col(0xFF8F3F97);
        }
        //collega i pulsanti ai ripsettivi grafici
        private void sethandlers()
        {
            btn_temp.Click += (s, e) => showplot(plot_temp, btn_temp);
            btn_aqi.Click += (s, e) => showplot(plot_aqi, btn_aqi);
            btn_pm.Click += (s, e) => showplot(plot_pm, btn_pm);
            btn_corr.Click += (s, e) => showplot(plot_corr, btn_corr);
            btn_close.Click += (s, e) => Close();
        }
        //nasconde tutti i grafici
        private void hideallplots()
        {
            plot_temp.Visible = false;
            plot_aqi.Visible = false;
            plot_pm.Visible = false;
            plot_corr.Visible = false;
        }
        //mostra un grafico ed evidenzia il pulsante selezionato    
        private void showplot(FormsPlot target, Button tab)
        {
            hideallplots();
            target.Visible = true;
            //resetta colore pulsnati
            var basecolor = System.Drawing.Color.FromArgb(255, 60, 60, 60);
            btn_temp.BackColor = basecolor;
            btn_aqi.BackColor = basecolor;
            btn_pm.BackColor = basecolor;
            btn_corr.BackColor = basecolor;
            //colore evidenziato
            tab.BackColor = System.Drawing.Color.FromArgb(255, 80, 80, 80);
        }
        //estrae i dati dal json e fa grafici
        private void loadhistoryandplot()
        {
            string path = Path.Combine(Application.StartupPath, "Data", "weather_history.json");

            if (!File.Exists(path))
            {
                MessageBox.Show("File JSON non trovato.");
                return;
            }

            string json = File.ReadAllText(path);
            List<WeatherRecord>? history = JsonConvert.DeserializeObject<List<WeatherRecord>>(json);

            if (history == null || history.Count == 0)
            {
                MessageBox.Show("Il file JSON è vuoto.");
                return;
            }
            //ordina i record per data 
            history = history.OrderBy(h => h.timestamp).ToList();
            //convenrte in array per essere compatibile con scottplot
            double[] xs = history.Select(h => h.timestamp.ToOADate()).ToArray();
            //prende i valori
            double[] temps = history.Select(h => h.temperature).ToArray();
            double[] aqi = history.Select(h => (double)h.aqi).ToArray();
            double[] pm25 = history.Select(h => h.pm25).ToArray();
            double[] pm10 = history.Select(h => h.pm10).ToArray();
            //costruisce i grafici
            BuildTemperature(xs, temps);
            BuildAqi(xs, aqi);
            BuildPm(xs, pm25, pm10);
            BuildCorrelation(xs, temps, aqi);
            //di default viene mostrato il grafico della temperatura
            showplot(plot_temp, btn_temp);
        }

        // temperatura nel tempo
        private void BuildTemperature(double[] xs, double[] temps)
        {
            plot_temp.Plot.Clear();//pulisce il grafico prima di disegnare
            //scatter crea la linea con i punti collegati
            var line = plot_temp.Plot.Add.Scatter(xs, temps);//aggiunge una linea con i dati di temperatura
            line.Color = col(0xFF1E90FF);
            line.LineWidth = 2;
            line.MarkerSize = 6;
            line.LegendText = "Temperatura (°C)";

            plot_temp.Plot.Axes.DateTimeTicksBottom();//formatta l'asse x come data

            plot_temp.Plot.ShowLegend(Alignment.UpperRight);//mostra legenda
            plot_temp.Plot.Title("Temperatura nel tempo");
            plot_temp.Plot.YLabel("°C");
            plot_temp.Plot.XLabel("Data");
            plot_temp.Refresh();
        }

        // aqi con barre colorate
        private void BuildAqi(double[] xs, double[] aqi)
        {
            plot_aqi.Plot.Clear();
            //per ogni punto AQI crea una barra colorata in base al valore
            for (int i = 0; i < xs.Length; i++)
            {
                var bar = plot_aqi.Plot.Add.Bar(xs[i], aqi[i]);
                bar.Color = aqicolor(aqi[i]);
            }
            //aggiunge legenda manuale con i colori e i livelli di AQI
            plot_aqi.Plot.Add.Annotation(
                "■ 1 Buono  ■ 2 Discreto  ■ 3 Moderato  ■ 4 Scadente  ■ 5 Pessimo",
                Alignment.LowerRight);

            plot_aqi.Plot.Axes.DateTimeTicksBottom();
            plot_aqi.Plot.Title("Qualità dell'aria (AQI)");
            plot_aqi.Plot.YLabel("AQI  (1 = Buono → 5 = Pessimo)");
            plot_aqi.Plot.XLabel("Data");
            plot_aqi.Refresh();
        }

        // pm2.5 e pm10 con barre affiancate
        private void BuildPm(double[] xs, double[] pm25, double[] pm10)
        {
            plot_pm.Plot.Clear();

            double offset = 0.15;//per separare le barre
            //per ogni punto crea due barre affiancate, una per PM2.5 e una per PM10, con colori diversi
            for (int i = 0; i < xs.Length; i++)
            {
                var b25 = plot_pm.Plot.Add.Bar(xs[i] - offset, pm25[i]);
                b25.Color = col(0xFFFFA500);

                var b10 = plot_pm.Plot.Add.Bar(xs[i] + offset, pm10[i]);
                b10.Color = col(0xFFE03030);
            }

            var soglia25 = plot_pm.Plot.Add.HorizontalLine(15);//soglia per PM2.5
            soglia25.Color = col(0xAAFFA500);
            soglia25.LineWidth = 1.5f;
            soglia25.LinePattern = LinePattern.Dashed;

            var soglia10 = plot_pm.Plot.Add.HorizontalLine(45);//soglia per PM10
            soglia10.Color = col(0xAAE03030);
            soglia10.LineWidth = 1.5f;
            soglia10.LinePattern = LinePattern.Dashed;

            plot_pm.Plot.Add.Annotation(
                "■ PM2.5 (arancione)   ■ PM10 (rosso)\n--- soglia OMS: PM2.5=15 µg/m³  PM10=45 µg/m³",
                Alignment.UpperRight);

            plot_pm.Plot.Axes.DateTimeTicksBottom();
            plot_pm.Plot.Title("PM2.5 e PM10 nel tempo");
            plot_pm.Plot.YLabel("µg/m³");
            plot_pm.Plot.XLabel("Data");
            plot_pm.Refresh();
        }

        // correlazione temperatura vs aqi
        private void BuildCorrelation(double[] xs, double[] temps, double[] aqi)
        {
            plot_corr.Plot.Clear();

            var lineTemp = plot_corr.Plot.Add.Scatter(xs, temps);
            lineTemp.Color = col(0xFF1E90FF);
            lineTemp.LineWidth = 2;
            lineTemp.MarkerSize = 5;
            lineTemp.LegendText = "Temperatura (°C)  [asse sx]";
            lineTemp.Axes.YAxis = plot_corr.Plot.Axes.Left;

            var lineAqi = plot_corr.Plot.Add.Scatter(xs, aqi);
            lineAqi.Color = col(0xFFFF4444);
            lineAqi.LineWidth = 2;
            lineAqi.MarkerSize = 5;
            lineAqi.LegendText = "AQI  [asse dx]";
            lineAqi.Axes.YAxis = plot_corr.Plot.Axes.Right;

            plot_corr.Plot.Axes.Left.Label.Text = "Temperatura (°C)";
            plot_corr.Plot.Axes.Right.Label.Text = "AQI (1–5)";
            plot_corr.Plot.Axes.SetLimitsY(0, 6);

            double r = pearson(temps, aqi);
            //valuta la forza della correlazione in base al valore di r
            string strength = Math.Abs(r) >= 0.7 ? "forte" :
                              Math.Abs(r) >= 0.4 ? "moderata" : "debole";

            plot_corr.Plot.Axes.DateTimeTicksBottom();
            plot_corr.Plot.ShowLegend(Alignment.UpperRight);
            plot_corr.Plot.Title($"Temperatura vs AQI nel tempo  —  r = {r:F3} ({strength})");
            plot_corr.Plot.XLabel("Data");
            plot_corr.Refresh();
        }

        private static double[] lineartrend(double[] x, double[] y)
        {
            //
            double mx = x.Average();
            double my = y.Average();
            double num = x.Zip(y, (xi, yi) => (xi - mx) * (yi - my)).Sum();
            double den = x.Sum(xi => Math.Pow(xi - mx, 2));
            double slope = den == 0 ? 0 : num / den;
            return new[] { slope, my - slope * mx };
        }

        private static double pearson(double[] x, double[] y)
        {
            //calcola il coefficiente di correlazione di Pearson tra due serie di dati x e y
            // Il valore restituito è compreso tra -1 e 1, dove 1 indica una correlazione positiva perfetta,
            // -1 una correlazione negativa perfetta e 0 nessuna correlazione lineare
            if (x.Length < 2) return 0;
            double mx = x.Average();
            double my = y.Average();
            double num = x.Zip(y, (xi, yi) => (xi - mx) * (yi - my)).Sum();
            double dx = Math.Sqrt(x.Sum(xi => Math.Pow(xi - mx, 2)));
            double dy = Math.Sqrt(y.Sum(yi => Math.Pow(yi - my, 2)));
            return (dx == 0 || dy == 0) ? 0 : num / (dx * dy);
        }
    }
}
