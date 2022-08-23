using Microsoft.EntityFrameworkCore;
using TelegramBotApp.Model;

namespace TelegramBotApp.Database
{
    public class LocationMessageDbContext : DbContext
    {
        public DbSet<LocationMessageModel> locationMessages { get; set; }

        public LocationMessageDbContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-FNOV9O5\SQLEXPRESS;Database=MessagesLocationDB;Trusted_Connection=True;");
        }
    }
}
