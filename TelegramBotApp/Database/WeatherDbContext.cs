using Microsoft.EntityFrameworkCore;
using TelegramBotApp.Model.DatabaseModel;

namespace TelegramBotApp.Database
{
    public class WeatherDbContext: DbContext
    {
        public WeatherDbContext() : base()
        {
        }

        public WeatherDbContext(DbContextOptions<WeatherDbContext> dbContextOptions) :base(dbContextOptions)
        {
        }

        
        public DbSet<WheatherChooseModel> CommandMessages { get; set; }
        public DbSet<CountryNameFromCodeModel> CountryNames { get; set; }
    }
}
