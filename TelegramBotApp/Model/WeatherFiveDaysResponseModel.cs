using System;

namespace TelegramBotApp.Model
{
    public class WeatherFiveDaysResponseModel
    {
        public List[] List{ get; set; }
        public City City { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    }

    public class List
    {
        public Main Main { get; set; }
        public Weather[] Weather { get; set; }
        public Wind Wind { get; set; }
        public DateTime DtTxt { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
    }
}
