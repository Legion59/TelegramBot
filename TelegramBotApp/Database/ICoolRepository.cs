using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotApp.Database
{
    public interface ICoolRepository
    {
        Task AddWeatherCommand(Update update);
        string GetCountryNameByCode(string countryCode);
        string GetLastUserCommand(long userId);
        Task SaveCountryName();
    }
}