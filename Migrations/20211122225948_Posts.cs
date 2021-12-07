using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_Web_App.Migrations
{
    public partial class Posts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    PostContent = table.Column<string>(maxLength: 4000, nullable: false),
                    PostImage = table.Column<string>(maxLength: 1000, nullable: true),
                    Author = table.Column<string>(maxLength: 250, nullable: false),
                    PostDate = table.Column<DateTime>(nullable: false),
                    PostViews = table.Column<int>(nullable: false),
                    PostLike = table.Column<int>(nullable: false),
                    LikeUserName = table.Column<string>(nullable: true),
                    SubId = table.Column<int>(nullable: false),
                    IsPublish = table.Column<bool>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 70, nullable: true),
                    Price = table.Column<double>(nullable: true),
                    Discount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_SubCategories_SubId",
                        column: x => x.SubId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SubId",
                table: "Posts",
                column: "SubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
