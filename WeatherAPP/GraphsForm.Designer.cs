namespace WeatherAPP
{
    partial class GraphsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnl_tabs;
        private Button btn_temp;
        private Button btn_aqi;
        private Button btn_pm;
        private Button btn_corr;
        private Panel pnl_container;
        private ScottPlot.WinForms.FormsPlot plot_temp;
        private ScottPlot.WinForms.FormsPlot plot_aqi;
        private ScottPlot.WinForms.FormsPlot plot_pm;
        private ScottPlot.WinForms.FormsPlot plot_corr;
        private Button btn_close;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnl_tabs = new Panel();
            this.btn_temp = new Button();
            this.btn_aqi = new Button();
            this.btn_pm = new Button();
            this.btn_corr = new Button();
            this.pnl_container = new Panel();
            this.plot_temp = new ScottPlot.WinForms.FormsPlot();
            this.plot_aqi = new ScottPlot.WinForms.FormsPlot();
            this.plot_pm = new ScottPlot.WinForms.FormsPlot();
            this.plot_corr = new ScottPlot.WinForms.FormsPlot();
            this.btn_close = new Button();
            this.SuspendLayout();

            this.pnl_tabs.Dock = DockStyle.Top;
            this.pnl_tabs.Height = 40;
            this.pnl_tabs.BackColor = Color.FromArgb(40, 40, 40);
            this.pnl_tabs.Controls.AddRange(new Control[] { btn_corr, btn_pm, btn_aqi, btn_temp });

            Button[] tabs = { btn_temp, btn_aqi, btn_pm, btn_corr };
            string[] names = { "Temperatura", "AQI", "PM2.5/PM10", "Correlazione" };
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].Text = names[i];
                tabs[i].Dock = DockStyle.Left;
                tabs[i].Width = 150;
                tabs[i].FlatStyle = FlatStyle.Flat;
                tabs[i].ForeColor = Color.White;
                tabs[i].BackColor = Color.FromArgb(60, 60, 60);
            }

            this.pnl_container.Dock = DockStyle.Fill;
            this.pnl_container.BackColor = Color.FromArgb(30, 30, 30);

            ScottPlot.WinForms.FormsPlot[] plots = { plot_temp, plot_aqi, plot_pm, plot_corr };
            foreach (var p in plots)
            {
                p.Dock = DockStyle.Fill;
                p.Visible = false;
                this.pnl_container.Controls.Add(p);
            }

            this.btn_close.Text = "Chiudi";
            this.btn_close.Dock = DockStyle.Bottom;
            this.btn_close.Height = 40;
            this.btn_close.FlatStyle = FlatStyle.Flat;
            this.btn_close.ForeColor = Color.White;
            this.btn_close.BackColor = Color.FromArgb(50, 50, 50);

            this.Controls.Add(this.pnl_container);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.pnl_tabs);
            this.ClientSize = new Size(900, 600);
            this.Text = "Grafici";
            this.ResumeLayout(false);
        }
    }
}
