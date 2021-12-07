using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_Web_App.Migrations
{
    public partial class usersettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isEmailConfirm = table.Column<bool>(type: "bit", nullable: false),
                    isRegisterOpen = table.Column<bool>(type: "bit", nullable: false),
                    MinimumPassLength = table.Column<int>(type: "int", nullable: false),
                    MaxPassLength = table.Column<int>(type: "int", nullable: false),
                    isDigit = table.Column<bool>(type: "bit", nullable: false),
                    isUpper = table.Column<bool>(type: "bit", nullable: false),
                    SendWelcomeMessage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
