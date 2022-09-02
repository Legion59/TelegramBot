using System;
using System.Linq;
using TelegramBotApp.Database;
using TelegramBotApp.Model;

namespace TelegramBotApp.Services.TelegramSevices
{
    public static class CreateCurrentWeatherResponse
    {
        static string CityName { get; set; }
        static string CountryName { get; set; }
        static string WeatherDscription { get; set; }
        static int TempCurrent { get; set; }
        static int TempFeels { get; set; }
        static double Pressure { get; set; }
        static int Humidity { get; set; }
        static int WindSpeed { get; set; }
        static string WindDirection { get; set; }
        static DateTime SunriseTime { get; set; }
        static DateTime SunsetTime { get; set; }

        static void ConvertWeatherInfo(WeatherResponseModel weatherInfo, ICoolRepository coolRepository)
        {
            CityName = weatherInfo.Name;

            //Convert Country ISO Code to Full Name
            CountryName = coolRepository.GetCountryNameByCode(weatherInfo.Sys.Country);

            WeatherDscription = weatherInfo.Weather[0].Description.ToUpper();

            //Convert temperature in integer Celsius
            TempCurrent = (int)Math.Round(weatherInfo.Main.Temp);
            TempFeels = (int)Math.Round(weatherInfo.Main.FeelsLike);

            //Convert pressure from hPa in mmHg
            Pressure = Math.Round(weatherInfo.Main.Pressure / 1.333);

            Humidity = weatherInfo.Main.Humidity;
            WindSpeed = (int)Math.Round(weatherInfo.Wind.Speed);

            //Convert wind direction from Degrees to Compass Directions
            string[] windDirections = { "North", "North-East", "East", "South-East", "South", "South-West", "West", "North-West", "North" };
            WindDirection = windDirections[(int)Math.Round(weatherInfo.Wind.Deg / 45.0)];

            //Convert sunrise and sunset time from unix to normal time
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            SunriseTime = dateTime.AddSeconds(weatherInfo.Sys.Sunrise).ToLocalTime();
            SunsetTime = dateTime.AddSeconds(weatherInfo.Sys.Sunset).ToLocalTime();
        }

        public static string CreateWeatherMessage(this WeatherResponseModel weatherInfo, ICoolRepository coolRepository)
        {
            ConvertWeatherInfo(weatherInfo, coolRepository);

            string result = $"Wheather on today in {CityName}, {CountryName}:\n" +
                        $"{WeatherDscription}\n" +
                        $" -> Temperature: {TempCurrent}°C,  Feels like: {TempFeels}°C\n" +
                        $" -> Pressure: {Pressure} mmHg\n" +
                        $" -> Humidity: {Humidity}%\n" +
                        $" -> Wind speed: {WindSpeed} m/s, {WindDirection}\n" +
                        $"Sunrise in {SunriseTime.TimeOfDay}\n" +
                        $"Sunset in {SunsetTime.TimeOfDay}";

            return result;
        }
    }
}
