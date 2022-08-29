using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotApp.Services.TelegramSevices
{
    public interface ITelegramServices
    {
        Task HandleMessege(Update update, CancellationToken cancellationToken = default);
    }
}
