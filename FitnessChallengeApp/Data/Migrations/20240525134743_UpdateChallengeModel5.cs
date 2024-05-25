using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessChallengeApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChallengeModel5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "UserRatings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "UserRatings");
        }
    }
}
