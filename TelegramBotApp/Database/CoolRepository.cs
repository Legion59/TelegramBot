using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotApp.Model.DatabaseModel;
using Update = Telegram.Bot.Types.Update;

namespace TelegramBotApp.Database
{
    public class CoolRepository : ICoolRepository
    {
        private readonly WeatherDbContext _weatherDbContext;

        public CoolRepository(WeatherDbContext weatherDbContext)
        {
            _weatherDbContext = weatherDbContext;
        }

        public async Task AddWeatherCommand(Update update)
        {
            await _weatherDbContext.CommandMessages.AddAsync(new WheatherChooseModel
            {
                UserId = update.Message.From.Id,
                MessageId = update.Message.MessageId,
                ChooseMessageText = update.Message.Text,
                MessageTime = DateTime.Now
            });

            await _weatherDbContext.SaveChangesAsync();
        }

        public string GetCountryNameByCode(string countryCode) => _weatherDbContext.CountryNames.FirstOrDefaultAsync(x => x.Code == countryCode).Result.Name;

        public string GetLastUserCommand(long userId) => _weatherDbContext.CommandMessages.OrderByDescending(x => x.MessageTime)
                                                                                            .FirstOrDefaultAsync(x => x.UserId == userId).Result.ChooseMessageText;

        public async Task SaveCountryName()
        {
            var result = JsonConvert.DeserializeObject<CountryNameFromCodeModel[]>
                (File.ReadAllText(@"C:\Users\Admin\Documents\TelegramBot\TelegramBotApp\TelegramBotApp\Data\Country codes (ISO 3166-1 alpha-2).json"));

            await _weatherDbContext.CountryNames.AddRangeAsync(result);

            await _weatherDbContext.SaveChangesAsync();
        }
    }
}
