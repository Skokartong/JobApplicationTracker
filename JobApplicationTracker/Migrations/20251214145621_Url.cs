using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobApplicationTracker.Migrations
{
    /// <inheritdoc />
    public partial class Url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "JobListings");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationDetailsId",
                table: "JobListings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_ApplicationDetailsId",
                table: "JobListings",
                column: "ApplicationDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Details_ApplicationDetailsId",
                table: "JobListings",
                column: "ApplicationDetailsId",
                principalTable: "Details",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Details_ApplicationDetailsId",
                table: "JobListings");

            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_ApplicationDetailsId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "ApplicationDetailsId",
                table: "JobListings");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "JobListings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
