using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cataloging.Database.Migrations
{
    /// <inheritdoc />
    public partial class Birtdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthday",
                schema: "Catalog",
                table: "Author",
                newName: "Birthdate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthdate",
                schema: "Catalog",
                table: "Author",
                newName: "Birthday");
        }
    }
}
