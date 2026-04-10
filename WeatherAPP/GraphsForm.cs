using Newtonsoft.Json;
using ScottPlot;
using ScottPlot.WinForms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

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

        // converte un int argb in colore scottplot perche usa formato diverso rispetto a System.Drawing.Color
        private ScottPlot.Color col(uint argb)
        {
            return ScottPlot.Color.FromARGB(argb);
        }

        private void sethandlers()
        {//cpllega i bottoni ai grafici
            btn_temp.Click += (s, e) => showplot(plot_temp, btn_temp);//quando clicchi appare quel grafico
            btn_aqi.Click += (s, e) => showplot(plot_aqi, btn_aqi);
            btn_pm.Click += (s, e) => showplot(plot_pm, btn_pm);
            btn_corr.Click += (s, e) => showplot(plot_corr, btn_corr);
            btn_close.Click += (s, e) => Close();
        }

        private void hideallplots()//nasconde tutti i grafici
        {
            plot_temp.Visible = false;
            plot_aqi.Visible = false;
            plot_pm.Visible = false;
            plot_corr.Visible = false;
        }

        private void showplot(FormsPlot target, Button tab)//mostra un singolo grafico
        {
            hideallplots();
            target.Visible = true;

            var basecolor = System.Drawing.Color.FromArgb(255, 60, 60, 60);//colore di base dei bottoni
            btn_temp.BackColor = basecolor;
            btn_aqi.BackColor = basecolor;
            btn_pm.BackColor = basecolor;
            btn_corr.BackColor = basecolor;

            tab.BackColor = System.Drawing.Color.FromArgb(255, 80, 80, 80);//colore del bottone attivo
        }

        private void loadhistoryandplot()
        {
            string path = Path.Combine(Application.StartupPath, "Data", "weather_history.json");

            if (!File.Exists(path))
            {
                MessageBox.Show("file json non trovato");
                return;
            }

            string json = File.ReadAllText(path);
            List<WeatherRecord>? history = JsonConvert.DeserializeObject<List<WeatherRecord>>(json);

            if (history == null || history.Count == 0)
            {
                MessageBox.Show("il file json è vuoto");
                return;
            }

            history = history.OrderBy(h => h.timestamp).ToList();//ordino per data crescente

            double[] xs = history.Select(h => h.timestamp.ToOADate()).ToArray();//converto le date in formato numerico per scottplot
            double[] temps = history.Select(h => h.temperature).ToArray();//estraggo i dati in array per scottplot
            double[] aqi = history.Select(h => h.aqi).ToArray();    
            double[] pm25 = history.Select(h => h.pm25).ToArray();
            double[] pm10 = history.Select(h => h.pm10).ToArray();

            // temperatura
            plot_temp.Plot.Clear();
            var t = plot_temp.Plot.Add.Scatter(xs, temps);
            t.Color = col(4280193279); // dodgerblue colore
            t.LineWidth = 2;//spessore linea
            plot_temp.Plot.Axes.DateTimeTicksBottom();
            plot_temp.Plot.Title("temperatura ultimi 15 giorni");
            plot_temp.Plot.YLabel("°c");
            plot_temp.Plot.XLabel("data");
            plot_temp.Refresh();

            // aqi
            plot_aqi.Plot.Clear();
            var a = plot_aqi.Plot.Add.Scatter(xs, aqi);
            a.Color = col(4281519410); // limegreen
            a.LineWidth = 2;
            plot_aqi.Plot.Axes.DateTimeTicksBottom();
            plot_aqi.Plot.Title("aqi ultimi 15 giorni");
            plot_aqi.Plot.YLabel("aqi");
            plot_aqi.Plot.XLabel("data");
            plot_aqi.Refresh();

            // pm2.5 e pm10
            plot_pm.Plot.Clear();
            var p25 = plot_pm.Plot.Add.Scatter(xs, pm25);
            p25.Color = col(4294944000); // orange
            p25.LineWidth = 2;
            p25.LegendText = "pm2.5";

            var p10 = plot_pm.Plot.Add.Scatter(xs, pm10);
            p10.Color = col(4294901760); // red
            p10.LineWidth = 2;
            p10.LegendText = "pm10";

            plot_pm.Plot.ShowLegend();
            plot_pm.Plot.Axes.DateTimeTicksBottom();
            plot_pm.Plot.Title("pm2.5 e pm10 ultimi 15 giorni");
            plot_pm.Plot.YLabel("µg/m³");
            plot_pm.Plot.XLabel("data");
            plot_pm.Refresh();

            // correlazione
            plot_corr.Plot.Clear();
            var c1 = plot_corr.Plot.Add.Scatter(temps, aqi);
            c1.Color = col(4281519410); // limegreen
            c1.LineWidth = 2;
            c1.LegendText = "aqi";

            var c2 = plot_corr.Plot.Add.Scatter(temps, pm25);
            c2.Color = col(4294944000); // orange
            c2.LineWidth = 2;
            c2.LegendText = "pm2.5";

            var c3 = plot_corr.Plot.Add.Scatter(temps, pm10);
            c3.Color = col(4294901760); // red
            c3.LineWidth = 2;
            c3.LegendText = "pm10";

            plot_corr.Plot.ShowLegend();
            plot_corr.Plot.Title("correlazione temperatura – inquinamento");
            plot_corr.Plot.XLabel("temperatura (°c)");
            plot_corr.Plot.YLabel("valori inquinanti");
            plot_corr.Refresh();

            showplot(plot_temp, btn_temp);
        }
    }
}