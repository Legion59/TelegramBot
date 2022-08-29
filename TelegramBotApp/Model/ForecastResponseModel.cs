using System;

namespace TelegramBotApp.Model
{
    public class ForecastResponseModel
    {
        public City City { get; set; }

        public List[] List{ get; set; }
    }

    public class List
    {
        public int Dt { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
        public Temp Temp { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public Weather[] Weather { get; set; }
        public double Speed { get; set; }
        public int Deg { get; set; }
        public int Clouds { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }

    public class Temp
    {
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
