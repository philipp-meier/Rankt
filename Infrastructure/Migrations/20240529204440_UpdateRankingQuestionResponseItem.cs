using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRankingQuestionResponseItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RankingQuestionResponseItems_RankingQuestionResponses_RankingQuestionOptionId",
                table: "RankingQuestionResponseItems");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "221d6ce1-cb21-4d7a-84fe-3c9fabda3944", "AQAAAAIAAYagAAAAEABZxYL8Surh/6KBIkTzpPifCmR+m2EVOkP/yZVQ4+zzODBlm19lQkk6ztAqjAq6eQ==", "06578885-2e89-4603-8a06-b476435ec33f" });

            migrationBuilder.AddForeignKey(
                name: "FK_RankingQuestionResponseItems_RankingQuestionOptions_RankingQuestionOptionId",
                table: "RankingQuestionResponseItems",
                column: "RankingQuestionOptionId",
                principalTable: "RankingQuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RankingQuestionResponseItems_RankingQuestionOptions_RankingQuestionOptionId",
                table: "RankingQuestionResponseItems");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54a7420f-ad5d-41b2-a73c-bfb77fd9072b", "AQAAAAIAAYagAAAAEEdaDnbnfqJRtGLD6s0+EcEGj/20n+TBKarQN2E3xIxDzeOc6Xa1iAFEy5fvqRwpNA==", "1800fe8f-67d3-40b5-b4a4-3810610c7490" });

            migrationBuilder.AddForeignKey(
                name: "FK_RankingQuestionResponseItems_RankingQuestionResponses_RankingQuestionOptionId",
                table: "RankingQuestionResponseItems",
                column: "RankingQuestionOptionId",
                principalTable: "RankingQuestionResponses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
