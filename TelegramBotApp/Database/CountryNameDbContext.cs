using Microsoft.EntityFrameworkCore;
using TelegramBotApp.Model.DatabaseModel;

namespace TelegramBotApp.Database
{
    public class CountryNameDbContext: DbContext
    {
        private readonly BotConfiguration _botConfiguration;
        public CountryNameDbContext(BotConfiguration botConfiguration) :base()
        {
            _botConfiguration = botConfiguration;
        }

        public DbSet<CountryNameFromCodeModel> countryNames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_botConfiguration.ConnectionString);
        }
    }
}
