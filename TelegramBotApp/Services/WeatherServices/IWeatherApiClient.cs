using System.Threading;
using System.Threading.Tasks;
using TelegramBotApp.Model;

namespace TelegramBotApp.Services.WeatherServices
{
    public interface IWeatherApiClient
    {
        Task<bool> CheckResponse(string location, CancellationToken cancellationToken = default);
        Task<WeatherResponseModel> GetCurrentWeatherByLocation(string location, CancellationToken cancellationToken = default);
        Task<ForecastResponseModel> GetWeatherFiveDaysByLocation(string location, CancellationToken cancellationToken = default);
    }
}