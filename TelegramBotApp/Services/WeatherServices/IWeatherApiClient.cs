using System.Threading;
using System.Threading.Tasks;
using TelegramBotApp.Model;

namespace TelegramBotApp.Services.WeatherServices
{
    public interface IWeatherApiClient
    {
        Task<WeatherResponseModel> GetCurrentWeatherByLocation(string location, CancellationToken cancellationToken = default);
        Task<ForecastResponseModel> GetForecastByLocation(string location, CancellationToken cancellationToken = default);
    }
}