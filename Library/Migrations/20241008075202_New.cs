using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    ID_Book = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Book = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenreID = table.Column<int>(type: "int", nullable: false),
                    Year_Public = table.Column<DateOnly>(type: "date", nullable: false),
                    Count_Copy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.ID_Book);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    ID_Genre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Genre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.ID_Genre);
                });

            migrationBuilder.CreateTable(
                name: "Reader",
                columns: table => new
                {
                    ID_Reader = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Reader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FName_Reader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    number_phone = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reader", x => x.ID_Reader);
                });

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
                name: "Book");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Reader");

            migrationBuilder.DropTable(
                name: "Rents");
        }
    }
}
