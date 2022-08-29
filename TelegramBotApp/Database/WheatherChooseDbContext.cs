using Microsoft.EntityFrameworkCore;
using TelegramBotApp.Model.DatabaseModel;

namespace TelegramBotApp.Database
{
    public class WheatherChooseDbContext : DbContext
    {
        private readonly BotConfiguration _botConfiguration;
        public WheatherChooseDbContext(BotConfiguration botConfiguration) : base()
        {
            _botConfiguration = botConfiguration;
        }

        public DbSet<WheatherChooseModel> chooseMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_botConfiguration.ConnectionString);
        }
    }
}
