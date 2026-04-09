using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAPP
{
    internal class ForecastData
    {
        public DateTime Day1_Date { get; set; }
        public double Day1_Temp { get; set; }
        public string? Day1_Icon { get; set; }

        public DateTime Day2_Date { get; set; }
        public double Day2_Temp { get; set; }
        public string? Day2_Icon { get; set; }

        public DateTime Day3_Date { get; set; }
        public double Day3_Temp { get; set; }
        public string? Day3_Icon { get; set; }

        public DateTime Day4_Date { get; set; }
        public double Day4_Temp { get; set; }
        public string? Day4_Icon { get; set; }
    }
}
