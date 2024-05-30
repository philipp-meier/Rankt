using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRankingQuestionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RankingQuestionStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RankingQuestionStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "RankingQuestionStatus",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "92318cff-f862-493a-9a34-170f26bba41a", "AQAAAAIAAYagAAAAEBT0vE3AAOVD28XRr4NiP1HpI/GNVLs2LOMcucCq1ET08mVkvQlv975PgKw7UXonTg==", "5d8e2fad-3846-417a-83e5-74f2dba64d88" });

            migrationBuilder.UpdateData(
                table: "RankingQuestionStatus",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "INPROGRESS", "in progress" });

            migrationBuilder.InsertData(
                table: "RankingQuestionStatus",
                columns: new[] { "Id", "Identifier", "Name" },
                values: new object[,]
                {
                    { 4, "COMPLETED", "completed" },
                    { 5, "ARCHIVED", "archived" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RankingQuestionStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RankingQuestionStatus",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "RankingQuestionStatus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "221d6ce1-cb21-4d7a-84fe-3c9fabda3944", "AQAAAAIAAYagAAAAEABZxYL8Surh/6KBIkTzpPifCmR+m2EVOkP/yZVQ4+zzODBlm19lQkk6ztAqjAq6eQ==", "06578885-2e89-4603-8a06-b476435ec33f" });

            migrationBuilder.UpdateData(
                table: "RankingQuestionStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "archived");

            migrationBuilder.InsertData(
                table: "RankingQuestionStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "in progress" },
                    { 2, "completed" }
                });
        }
    }
}
