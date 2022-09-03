using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotApp.Model.DatabaseModel;
using TelegramBotApp.Services.TelegramSevices;
using Update = Telegram.Bot.Types.Update;

namespace TelegramBotApp.Database
{
    public class CoolRepository : ICoolRepository
    {
        private readonly WeatherDbContext _weatherDbContext;
        private readonly ILogger<CoolRepository> _logger;



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

            _weatherDbContext.SaveChanges();
        }

        public string GetCountryNameByCode(string countryCode) => _weatherDbContext.CountryNames.Where(x => x.Code == countryCode).FirstOrDefault().Name;

        public string GetLastUserCommand(long userId) => _weatherDbContext.CommandMessages.Where(x => x.UserId == userId).
                                                                            OrderByDescending(x => x.MessageTime).FirstOrDefault().ChooseMessageText;

        public async Task SaveCountryName()
        {
            var result = JsonConvert.DeserializeObject<CountryNameFromCodeModel[]>
                (File.ReadAllText(@"C:\Users\Admin\Documents\TelegramBot\TelegramBotApp\TelegramBotApp\Data\Country codes (ISO 3166-1 alpha-2).json"));

            foreach (var countryName in result)
            {
                await _weatherDbContext.CountryNames.AddAsync(new CountryNameFromCodeModel
                {
                    Code = countryName.Code,
                    Name = countryName.Name
                });

                await _weatherDbContext.SaveChangesAsync();
            }
        }
    }
}
