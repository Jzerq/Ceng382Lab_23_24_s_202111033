using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessChallengeApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChallengeModel6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_ChallengeId",
                table: "UserRatings",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChallenges_ChallengeId",
                table: "UserChallenges",
                column: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChallenges_Challenges_ChallengeId",
                table: "UserChallenges",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRatings_Challenges_ChallengeId",
                table: "UserRatings",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChallenges_Challenges_ChallengeId",
                table: "UserChallenges");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRatings_Challenges_ChallengeId",
                table: "UserRatings");

            migrationBuilder.DropIndex(
                name: "IX_UserRatings_ChallengeId",
                table: "UserRatings");

            migrationBuilder.DropIndex(
                name: "IX_UserChallenges_ChallengeId",
                table: "UserChallenges");
        }
    }
}
