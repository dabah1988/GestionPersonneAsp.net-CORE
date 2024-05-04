using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MesEntites.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    countryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    countryName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.countryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateofBirth = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceivesNewsLetter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Persons_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "countryId");
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "countryId", "countryName" },
                values: new object[,]
                {
                    { new Guid("0f916af0-87c1-44f2-a413-7c6d0e0ed344"), "Suisse" },
                    { new Guid("25f18418-f2a6-4c36-8dfd-37c02fe86791"), "Canada" },
                    { new Guid("c70d400b-4ec4-4557-af98-0aa7b87d0230"), "Espagne" },
                    { new Guid("da0a5d64-df49-4cd7-87cf-8b4939e07a33"), "France" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Adress", "CountryId", "DateofBirth", "Email", "Gender", "PersonName", "ReceivesNewsLetter" },
                values: new object[,]
                {
                    { new Guid("21e761fa-4c90-4760-84ab-552e8b541471"), "PO Box 24501", new Guid("0f916af0-87c1-44f2-a413-7c6d0e0ed344"), null, "dcharter5@devhub.com", "Agender", "Dewey", false },
                    { new Guid("2a2d3ec6-2018-47ba-b6b6-68cc27ba877a"), "Room 1550", new Guid("0f916af0-87c1-44f2-a413-7c6d0e0ed344"), null, "gkarlik2@cloudflare.com", "Female", "Gilligan", false },
                    { new Guid("4fa348f3-75c7-4176-b97c-3b1853f8f0fa"), "Apt 52", new Guid("c70d400b-4ec4-4557-af98-0aa7b87d0230"), null, "ladolfsen3@taobao.com", "Male", "Lars", false },
                    { new Guid("5452c7d6-d58e-4760-8a9b-cc6ee6efe7e5"), "PO Box 60084", new Guid("da0a5d64-df49-4cd7-87cf-8b4939e07a33"), null, "sgaddes1@arstechnica.com", "Female", "Shalna", false },
                    { new Guid("74a935fa-f2d4-40d7-8324-71406ffa82f7"), "5th Floor", new Guid("0f916af0-87c1-44f2-a413-7c6d0e0ed344"), null, "jromeuf0@biglobe.ne.jp", "Male", "Jefferson", false },
                    { new Guid("a383c1df-6957-4cc6-adec-94886bf23967"), "Apt 1480", new Guid("25f18418-f2a6-4c36-8dfd-37c02fe86791"), null, "tpryell4@china.com.cn", "Female", "Tracy", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryId",
                table: "Persons",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
