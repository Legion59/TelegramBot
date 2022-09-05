using Xunit;
using Moq;
using TelegramBotApp.Services.TelegramSevices;
using TelegramBotApp.Services.WeatherServices;
using TelegramBotApp.Database;
using Telegram.Bot;
using Update = Telegram.Bot.Types.Update;

namespace TelegramBotTests
{
    public class TelegramServicesTests
    {
        private readonly TelegramServices _telegramServicesTest;
        private readonly Mock<ITelegramBotClient> _telegramBotClientMock = new Mock<ITelegramBotClient>();
        private readonly Mock<IWeatherApiClient> _weatherApiClientMock = new Mock<IWeatherApiClient>();
        private readonly Mock<ICoolRepository> _coolRepoMock = new Mock<ICoolRepository>();

        public TelegramServicesTests()
        {
            _telegramServicesTest = new TelegramServices(_telegramBotClientMock.Object, _weatherApiClientMock.Object, _coolRepoMock.Object);
        }

        [Fact]
        public async Task HandleMessege_ShoudAddCommadInDb()
        {
            //Arrange
            Update commandCurrent = new Update
            {
                Message = new Telegram.Bot.Types.Message
                {
                    Text = "/current"
                }
            };
            Update commandForecast = new Update
            {
                Message = new Telegram.Bot.Types.Message
                {
                    Text = "/forecast"
                }
            };

            //Act
            await _telegramServicesTest.HandleMessege(commandCurrent);
            await _telegramServicesTest.HandleMessege(commandForecast);

            //Assert
            _coolRepoMock.Verify(x => x.AddWeatherCommand(commandCurrent), Times.Once());
            _coolRepoMock.Verify(x => x.AddWeatherCommand(commandForecast), Times.Once());
        }
    }
} 