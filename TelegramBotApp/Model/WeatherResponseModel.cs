namespace TelegramBotApp.Model
{
    public class WeatherResponseModel
    {
        public string Name { get; set; }
        public Weather[] Weather { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
    }

    public class Main
    {
        public decimal Temp { get; set; }
        public decimal FeelsLike { get; set; }
        public decimal TempMin { get; set; }
        public decimal TempMax { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }

    public class Wind
    {
        public decimal Speed { get; set; }
        public int Deg { get; set; }
    }

    public class Sys
    {
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    }
}