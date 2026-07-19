using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatchMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationOperations",
                columns: table => new
                {
                    CorrelationId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationOperations", x => x.CorrelationId);
                    table.ForeignKey(
                        name: "FK_NotificationOperations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationOperations_UserId",
                table: "NotificationOperations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationOperations_UserId_CreatedAtUtc",
                table: "NotificationOperations",
                columns: new[] { "UserId", "CreatedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationOperations");
        }
    }
}
