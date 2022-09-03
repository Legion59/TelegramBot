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
        public void HandleMessege_ShoudReturnSameCommandFromTheBaseAsInText()
        {
            //Arrange
            Update command = new Update();
            command.Message.Text = "/current";


            //Act
            _ = _telegramServicesTest.HandleMessege(command);

            //Assert
            Assert.Equal
        }
    }
} 