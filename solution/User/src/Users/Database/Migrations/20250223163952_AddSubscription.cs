using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_Id",
                schema: "Addresses",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                schema: "Users",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                schema: "Addresses",
                table: "User");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Users",
                newName: "Users",
                newSchema: "User");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Addresses",
                newName: "Addresses",
                newSchema: "User");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                schema: "User",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "User",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "User",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                schema: "User",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionType = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                schema: "User",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                schema: "User",
                table: "Subscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                schema: "User",
                table: "Addresses",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                schema: "User",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "Subscriptions",
                schema: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "User",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                schema: "User",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserId",
                schema: "User",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "User",
                table: "Users");

            migrationBuilder.EnsureSchema(
                name: "Addresses");

            migrationBuilder.EnsureSchema(
                name: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "User",
                newName: "User",
                newSchema: "Users");

            migrationBuilder.RenameTable(
                name: "Addresses",
                schema: "User",
                newName: "User",
                newSchema: "Addresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                schema: "Users",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                schema: "Addresses",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_Id",
                schema: "Addresses",
                table: "User",
                column: "Id",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
