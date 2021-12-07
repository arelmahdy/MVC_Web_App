using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_Web_App.Migrations
{
    public partial class alterAppUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailConfirm",
                table: "AppUser",
                newName: "LockOut");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "UserRoles",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValueSql: "NewID()",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppUser",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValueSql: "NewID()",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirm",
                table: "AppUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ErrorLogCount",
                table: "AppUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockTime",
                table: "AppUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppRoles",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValueSql: "NewID()",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirm",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "ErrorLogCount",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "LockTime",
                table: "AppUser");

            migrationBuilder.RenameColumn(
                name: "LockOut",
                table: "AppUser",
                newName: "EmilConfirm");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "UserRoles",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldDefaultValueSql: "NewID()");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppUser",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldDefaultValueSql: "NewID()");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppRoles",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldDefaultValueSql: "NewID()");
        }
    }
}
