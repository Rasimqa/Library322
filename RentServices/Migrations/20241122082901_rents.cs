using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentServices.Migrations
{
    /// <inheritdoc />
    public partial class rents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    ID_Rental = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Reader = table.Column<int>(type: "int", nullable: false),
                    ID_Book = table.Column<int>(type: "int", nullable: false),
                    Rental_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Return_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Returned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.ID_Rental);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rents");
        }
    }
}
