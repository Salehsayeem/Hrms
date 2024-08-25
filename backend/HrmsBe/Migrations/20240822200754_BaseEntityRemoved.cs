using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrmsBe.Migrations
{
    /// <inheritdoc />
    public partial class BaseEntityRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RoomDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RoomDetails");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RoomDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "RoomDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RoomDetails");

            migrationBuilder.DropColumn(
                name: "ServerActionDateTime",
                table: "RoomDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RoomDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "timezone('UTC', now() + interval '6 hours')");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "RoomDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RoomDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "RoomDetails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "RoomDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ServerActionDateTime",
                table: "RoomDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "timezone('UTC', now() + interval '6 hours')");
        }
    }
}
