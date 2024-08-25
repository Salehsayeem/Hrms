using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrmsBe.Migrations
{
    /// <inheritdoc />
    public partial class TableAdded4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    BillType = table.Column<string>(type: "text", nullable: false),
                    BillOptions = table.Column<string>(type: "text", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    NoOfUnits = table.Column<int>(type: "integer", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('UTC', now() + interval '6 hours')"),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ServerActionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('UTC', now() + interval '6 hours')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    HouseId = table.Column<long>(type: "bigint", nullable: false),
                    RoomCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    BasePrice = table.Column<int>(type: "integer", nullable: false),
                    BillGenerationDate = table.Column<short>(type: "smallint", nullable: false),
                    IsRented = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('UTC', now() + interval '6 hours')"),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ServerActionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('UTC', now() + interval '6 hours')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomDetails");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
