using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locationMessages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MessageTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locationMessages", x => x.MessageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "locationMessages");
        }
    }
}
