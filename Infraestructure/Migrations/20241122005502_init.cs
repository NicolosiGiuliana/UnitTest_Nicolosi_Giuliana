using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldType",
                columns: table => new
                {
                    FieldTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.FieldTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    FieldID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Size = table.Column<string>(type: "varchar(50)", nullable: false),
                    FieldTypeID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.FieldID);
                    table.ForeignKey(
                        name: "FK_Field_FieldType_FieldTypeID",
                        column: x => x.FieldTypeID,
                        principalTable: "FieldType",
                        principalColumn: "FieldTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Availability",
                columns: table => new
                {
                    AvailabilityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpenHour = table.Column<TimeSpan>(type: "time", nullable: false),
                    CloseHour = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availability", x => x.AvailabilityID);
                    table.ForeignKey(
                        name: "FK_Availability_Field_FieldID",
                        column: x => x.FieldID,
                        principalTable: "Field",
                        principalColumn: "FieldID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FieldType",
                columns: new[] { "FieldTypeID", "Description" },
                values: new object[,]
                {
                    { 1, "pasto" },
                    { 2, "sintetico" },
                    { 3, "Cemento" }
                });

            migrationBuilder.InsertData(
                table: "Field",
                columns: new[] { "FieldID", "FieldTypeID", "IsActive", "Name", "Size" },
                values: new object[,]
                {
                    { new Guid("a98ed5b8-d793-4421-9add-cb28723c5f9e"), 1, false, "Campo 1", "5" },
                    { new Guid("b3bc9c00-c6ef-4b22-8654-3c54622a1f1b"), 2, false, "Campo 2", "7" },
                    { new Guid("b6424eed-203a-4988-8ce5-f62691426c3e"), 1, false, "Campo 3", "11" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Availability_FieldID",
                table: "Availability",
                column: "FieldID");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FieldTypeID",
                table: "Field",
                column: "FieldTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Availability");

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "FieldType");
        }
    }
}
