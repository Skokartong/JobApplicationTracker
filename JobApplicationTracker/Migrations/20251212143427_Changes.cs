using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobApplicationTracker.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Employers_EmployerId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "CompanyDescription",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "JobLevel",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "JobListings");

            migrationBuilder.AlterColumn<int>(
                name: "EmployerId",
                table: "JobListings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "JobListings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "JobListings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DescriptionId",
                table: "JobListings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobTypeId",
                table: "JobListings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "JobApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(type: "text", nullable: false),
                    StreetAddress = table.Column<string>(type: "text", nullable: false),
                    Postcode = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Description",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Description", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_AddressId",
                table: "JobListings",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_CategoryId",
                table: "JobListings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_DescriptionId",
                table: "JobListings",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_JobTypeId",
                table: "JobListings",
                column: "JobTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Addresses_AddressId",
                table: "JobListings",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Categories_CategoryId",
                table: "JobListings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Description_DescriptionId",
                table: "JobListings",
                column: "DescriptionId",
                principalTable: "Description",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Employers_EmployerId",
                table: "JobListings",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Types_JobTypeId",
                table: "JobListings",
                column: "JobTypeId",
                principalTable: "Types",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Addresses_AddressId",
                table: "JobListings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Categories_CategoryId",
                table: "JobListings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Description_DescriptionId",
                table: "JobListings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Employers_EmployerId",
                table: "JobListings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Types_JobTypeId",
                table: "JobListings");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Description");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_AddressId",
                table: "JobListings");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_CategoryId",
                table: "JobListings");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_DescriptionId",
                table: "JobListings");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_JobTypeId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "DescriptionId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "JobTypeId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "City",
                table: "JobApplications");

            migrationBuilder.AlterColumn<int>(
                name: "EmployerId",
                table: "JobListings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyDescription",
                table: "JobListings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "JobListings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "JobListings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobLevel",
                table: "JobListings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "JobListings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "JobListings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Employers_EmployerId",
                table: "JobListings",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
