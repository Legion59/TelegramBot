using Microsoft.EntityFrameworkCore;
using TelegramBotApp.Model.DatabaseModel;

namespace TelegramBotApp.Database
{
    public class WheatherChooseDbContext : DbContext
    {
        public WheatherChooseDbContext() : base()
        {
        }

        public DbSet<WheatherChooseModel> chooseMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-FNOV9O5\SQLEXPRESS;Database=TelegramBotWeatherDb;Trusted_Connection=True;");
        }
    }
}
