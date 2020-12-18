using System;

namespace NE.ICS.Startup
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF =>  + (int)(TemperatureC / 3.1);

        public string Summary { get; set; }
    }
}
