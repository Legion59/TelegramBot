using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotApp.Model;

namespace TelegramBotApp.Services
{
    public class TelegramServices : ITelegramServices 
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IWeatherApiClient _weatherApiClient;

        public TelegramServices(ITelegramBotClient telegramBotClient, IWeatherApiClient weatherApiClient)
        {
            _telegramBotClient = telegramBotClient;
            _weatherApiClient = weatherApiClient;
        }

        public async Task HandleMessege(Update update, CancellationToken cancellationToken = default)
        {
            try
            {
                if (update.Type != UpdateType.Message)
                {
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"I cannot handle {update}", cancellationToken: cancellationToken);

                    return;
                }

                if (update.Message.Text.Equals("/start"))
                {
                    await _telegramBotClient.SendStickerAsync(update.Message.Chat.Id, "CAACAgIAAxkBAAEFmoBi_y-b_JKwoTSHbZNz0YDDVvlmlQACHAAD9wLID3Acci1tkxh4KQQ", cancellationToken: cancellationToken);
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Whelcome to Weather TelegramBot\U0001F600 \n" +
                                                                  $"Please write the city where you would like to know the weather.", cancellationToken: cancellationToken);
                    return;
                }

                string location = update.Message.Text;

                var weatherInfo = await _weatherApiClient.GetWeatherFiveDaysByLocation(location, cancellationToken);

                /*
                if (_weatherApiClient.CheckResponse(update.Message.Text).Result && location == null)
                {
                    location = update.Message.Text;
               
                    var keyboard = new  ReplyKeyboardMarkup(new KeyboardButton[] { new KeyboardButton("Current weather"), new KeyboardButton("Forecast on few days") });
                    
                    keyboard.ResizeKeyboard = true;

                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "What weather?", replyMarkup: keyboard);

                    return;
                }

                if (update.Message.Text.Equals("Current weather"))
                {
                    var weatherInfo = await _weatherApiClient.GetCurrentWeatherByLocation(location, cancellationToken);
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, ConvertAndFormCurrentWeatherResponse(weatherInfo), cancellationToken: cancellationToken);
                }*/

                if (weatherInfo != null)
                {
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, ConvertAndFormFiveDaysWeatherResponse(weatherInfo), cancellationToken: cancellationToken);

                    return;
                }

                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"I cannot find your location by query {location}.", cancellationToken: cancellationToken);

                return;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public string ConvertAndFormCurrentWeatherResponse(WeatherResponseModel weatherInfo)
        {
            string city = weatherInfo.Name;
            string weatherDescription = weatherInfo.Weather[0].Description.ToUpper();

            //Convert temperature in integer Celsius
            int tempNow = (int)Math.Round(weatherInfo.Main.Temp);
            int tempFeels = (int)Math.Round(weatherInfo.Main.FeelsLike);
            int tempMin = (int)Math.Round(weatherInfo.Main.TempMin);
            int tempMax = (int)Math.Round(weatherInfo.Main.TempMax);

            //Convert pressure from hPa in mmHg
            double pressure = Math.Round(weatherInfo.Main.Pressure / 1.333);

            int humidity = weatherInfo.Main.Humidity;
            int windSpeed = (int)Math.Round(weatherInfo.Wind.Speed);

            //Convert wind direction from Degrees to Compass Directions
            string[] windDirections = { "North", "North-East", "East", "South-East", "South", "South-West", "West", "North-West", "North" };
            string windDirection = windDirections[(int)Math.Round(weatherInfo.Wind.Deg / 45.0)];

            //Convert sunrise and sunset time from unix to normal time
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime sunriseTime = dateTime.AddSeconds(weatherInfo.Sys.Sunrise).ToLocalTime();
            DateTime sunsetTime = dateTime.AddSeconds(weatherInfo.Sys.Sunset).ToLocalTime();

            //Convert Country ISO Code to Full Name
            var countryCodesText = System.IO.File.ReadAllText(@"C:\Users\Admin\Documents\TelegramBot\TelegramBotApp\TelegramBotApp\Data\Country codes (ISO 3166-1 alpha-2).json");
            var countryCodes = JsonConvert.DeserializeObject<CountryNameFromCodeModel[]>(countryCodesText);
            var country = countryCodes.Where(x => x.Code == weatherInfo.Sys.Country).Select(x => x.Name).FirstOrDefault();


            string result = $"Wheather on today in {city}, {country}:\n" +
                        $"{weatherDescription}\n" +
                        $" -> Temperature: {tempNow}°C,  Feels like: {tempFeels}°C\n" +
                        $" -> Pressure: {pressure} mmHg\n" +
                        $" -> Humidity: {humidity}%\n" +
                        $" -> Wind speed: {windSpeed} m/s, {windDirection}\n" +
                        $"Sunrise in {sunriseTime.TimeOfDay}\n" +
                        $"Sunset in {sunsetTime.TimeOfDay}";

            return result;
        }

        public string ConvertAndFormFiveDaysWeatherResponse(WeatherFiveDaysResponseModel weatherInfoOnFiveDays)
        {
            string result = null;
            string[] windDirections = { "North", "North-East", "East", "South-East", "South", "South-West", "West", "North-West", "North" };

            string city = weatherInfoOnFiveDays.City.Name;

            //Convert Country ISO Code to Full Name
            var countryCodesText = System.IO.File.ReadAllText(@"C:\Users\Admin\Documents\TelegramBot\TelegramBotApp\TelegramBotApp\Data\Country codes (ISO 3166-1 alpha-2).json");
            var countryCodes = JsonConvert.DeserializeObject<CountryNameFromCodeModel[]>(countryCodesText);
            var country = countryCodes.Where(x => x.Code == weatherInfoOnFiveDays.City.Country).Select(x => x.Name).FirstOrDefault();


            result += $"Forecast in {city}, {country}\n\n";


            foreach (var weather in weatherInfoOnFiveDays.List)
            {
                //Convert temperature in integer Celsius
                int tempMin = (int)Math.Round(weather.Temp.Min);
                int tempMax = (int)Math.Round(weather.Temp.Max);

                //Convert pressure from hPa in mmHg
                double pressure = Math.Round(weather.Pressure / 1.333);

                int humidity = weather.Humidity;
                int windSpeed = (int)Math.Round(weather.Speed);

                //Convert wind direction from Degrees to Compass Directions
                string windDirection = windDirections[(int)Math.Round(weather.Deg / 45.0)];

                //Convert sunrise and sunset time from unix to normal time
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                DateTime dateReport = dateTime.AddSeconds(weather.Dt).ToLocalTime();
                DateTime sunriseTime = dateTime.AddSeconds(weather.Sunrise).ToLocalTime();
                DateTime sunsetTime = dateTime.AddSeconds(weather.Sunset).ToLocalTime();

                result += $"The weather report on {dateReport.Date.ToString("d")}\n" +
                        $"{weather.Weather[0].Description.ToUpper()}\n" +
                        $" —> Temperature min/max: {tempMin}°C / {tempMax}°C\n" +
                        $" —> Pressure: {pressure} mmHg\n" +
                        $" —> Humidity: {humidity}%\n" +
                        $" —> Wind speed: {windSpeed} m/s, {windDirection}\n" +
                        $"Sunrise: {sunriseTime.TimeOfDay}\n" +
                        $"Sunset: {sunsetTime.TimeOfDay}\n\n\n";
            }

            return result;
        }
    }
}