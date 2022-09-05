using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using TelegramBotApp.Database;
using TelegramBotApp.Model;

namespace TelegramBotApp.Services.TelegramSevices
{
    public static class CreateForecastResponse
    {
        static string CityName { get; set; }
        static string CountryName { get; set; }

        static List<ForecastReport> forecastReports = new List<ForecastReport>();

        public static string CreateForecastMessage(this ForecastResponseModel forecastInfo, ICoolRepository coolRepository)
        {
            ConvertForecastInfo(forecastInfo, coolRepository);
            string result = null;

            result += $"Forecast in {CityName}, {CountryName}\n\n";

            foreach (var item in forecastReports)
            {
                result += $"The weather report on {item.DateReport.ToString("d")}\n" +
                        $"{item.WeatherDscription}\n" +
                        $" —> Temperature min/max: {item.TempMin}°C / {item.TempMax}°C\n" +
                        $" —> Pressure: {item.Pressure} mmHg\n" +
                        $" —> Humidity: {item.Humidity}%\n" +
                        $" —> Wind speed: {item.WindSpeed} m/s, {item.WindDirection}\n" +
                        $"Sunrise: {item.SunriseTime.TimeOfDay}\n" +
                        $"Sunset: {item.SunsetTime.TimeOfDay}\n\n\n";
            }

            return result;
        }


        static void ConvertForecastInfo(ForecastResponseModel forecastInfo, ICoolRepository coolRepository)
        {
            string[] windDirections = { "North", "North-East", "East", "South-East", "South", "South-West", "West", "North-West", "North" };

            CityName = forecastInfo.City.Name;

            //Convert Country ISO Code to Full Name
            CountryName = coolRepository.GetCountryNameByCode(forecastInfo.City.Country);


            foreach (var weather in forecastInfo.List)
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                forecastReports.Add(new ForecastReport
                {
                    WeatherDscription = weather.Weather[0].Description.ToUpper(),

                    //Convert temperature in integer Celsius
                    TempMin = (int)Math.Round(weather.Temp.Min),
                    TempMax = (int)Math.Round(weather.Temp.Max),

                    //Convert pressure from hPa in mmHg
                    Pressure = Math.Round(weather.Pressure / 1.333),

                    Humidity = weather.Humidity,
                    WindSpeed = (int)Math.Round(weather.Speed),

                    //Convert wind direction from Degrees to Compass Directions
                    WindDirection = windDirections[(int)Math.Round(weather.Deg / 45.0)],

                    //Convert sunrise and sunset time from unix to normal time
                    DateReport = dateTime.AddSeconds(weather.Dt).ToLocalTime(),
                    SunriseTime = dateTime.AddSeconds(weather.Sunrise).ToLocalTime(),
                    SunsetTime = dateTime.AddSeconds(weather.Sunset).ToLocalTime()
                });
            }
        }

        class ForecastReport
        {
            public string WeatherDscription { get; set; }
            public int TempMin { get; set; }
            public int TempMax { get; set; }
            public double Pressure { get; set; }
            public int Humidity { get; set; }
            public int WindSpeed { get; set; }
            public string WindDirection { get; set; }
            public DateTime DateReport { get; set; }
            public DateTime SunriseTime { get; set; }
            public DateTime SunsetTime { get; set; }
        }
    }
}
