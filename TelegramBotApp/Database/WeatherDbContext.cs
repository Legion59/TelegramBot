using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WheatherChooseModel>()
                .HasData(new WheatherChooseModel
                {
                    Id = 1,
                    UserId = 42,
                    MessageId = 42,
                    ChooseMessageText = "Best Text",
                    MessageTime = System.DateTime.Now
                });
        }
    }
}
