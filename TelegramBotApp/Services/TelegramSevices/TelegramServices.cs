using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotApp.Database;
using TelegramBotApp.Model;
using TelegramBotApp.Model.DatabaseModel;
using TelegramBotApp.Services.WeatherServices;

namespace TelegramBotApp.Services.TelegramSevices
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
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Whelcome to Weather TelegramBot\U0001F600", cancellationToken: cancellationToken);

                    var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[] { new KeyboardButton("/current"), new KeyboardButton("/forecast") });

                    keyboard.ResizeKeyboard = true;

                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Please choose current wheather or forecast on 5 days you want", replyMarkup: keyboard);

                    return;
                }

                await using var dbContext = new WheatherChooseDbContext();

                if (update.Message.Text.Equals("/current") || update.Message.Text.Equals("/forecast"))
                {
                    await dbContext.chooseMessages.AddAsync(new WheatherChooseModel
                    {
                        UserId = update.Message.From.Id,
                        MessageId = update.Message.MessageId,
                        ChooseMessageText = update.Message.Text,
                        MessageTime = DateTime.Now
                    });

                    await dbContext.SaveChangesAsync();

                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Write a location for the weather", cancellationToken: cancellationToken);

                    return;
                }


                if (dbContext.chooseMessages == null)
                {
                    await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Эй, кожаный ублюдок!!! \nТебе же сказали, выбери сначала какая погода тебе нужна, текущая или на 5 дней, идиот!", cancellationToken: cancellationToken);

                    return;
                }


                var wheatherChoose = dbContext.chooseMessages.Where(x => x.UserId == update.Message.From.Id).OrderByDescending(x => x.MessageTime).FirstOrDefault().ChooseMessageText;

                if (wheatherChoose.Equals("/current"))
                {
                    var location = update.Message.Text;

                    var weatherInfo = await _weatherApiClient.GetCurrentWeatherByLocation(location, cancellationToken);

                    if (weatherInfo != null)
                    {
                        await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, weatherInfo.CreateWeatherMessage(), cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Эй, кожаный ублюдок!!! \nСмотри что ты пишешь, нет такого места, идиот!", cancellationToken: cancellationToken);
                    }

                    return;
                }


                if (wheatherChoose.Equals("/forecast"))
                {
                    var location = update.Message.Text;

                    var forecastInfo = await _weatherApiClient.GetWeatherFiveDaysByLocation(location, cancellationToken);

                    if (forecastInfo != null)
                    {
                        await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, forecastInfo.CreateForecastMessage(), cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Эй, кожаный ублюдок!!! \nСмотри что ты пишешь, нет такого места, идиот!", cancellationToken: cancellationToken);
                    }

                    return;
                }


                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"Я понятия не имею что за хрень ты написал, кожаный.", cancellationToken: cancellationToken);

                return;
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}