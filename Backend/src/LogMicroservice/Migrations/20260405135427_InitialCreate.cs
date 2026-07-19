using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeadLetterLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QueueName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MessageBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    ErrorDescription = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    EnqueuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeadLetteredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadLetterLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Recipient = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeadLetterLogs_CorrelationId",
                table: "DeadLetterLogs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLogs_OwnerId",
                table: "ServiceLogs",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeadLetterLogs");

            migrationBuilder.DropTable(
                name: "ServiceLogs");
        }
    }
}
