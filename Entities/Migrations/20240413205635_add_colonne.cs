using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesEntites.Migrations
{
    /// <inheritdoc />
    public partial class add_colonne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PossedeLePermis",
                table: "Persons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("21e761fa-4c90-4760-84ab-552e8b541471"),
                column: "PossedeLePermis",
                value: false);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("2a2d3ec6-2018-47ba-b6b6-68cc27ba877a"),
                column: "PossedeLePermis",
                value: false);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("4fa348f3-75c7-4176-b97c-3b1853f8f0fa"),
                column: "PossedeLePermis",
                value: false);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("5452c7d6-d58e-4760-8a9b-cc6ee6efe7e5"),
                column: "PossedeLePermis",
                value: false);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("74a935fa-f2d4-40d7-8324-71406ffa82f7"),
                column: "PossedeLePermis",
                value: false);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("a383c1df-6957-4cc6-adec-94886bf23967"),
                column: "PossedeLePermis",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PossedeLePermis",
                table: "Persons");
        }
    }
}
