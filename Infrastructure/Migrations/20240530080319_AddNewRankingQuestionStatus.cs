using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewQuestionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "53746146-6c35-4731-a3aa-8ef0e77153cb", "AQAAAAIAAYagAAAAEOxn/UHtZIuuNXPDHO99iN9N7pQ3cFwvmXc6EvFxk+j5n+i2RMSLrALLgUzhs1TPnA==", "23dd244a-07a5-4cd7-ad88-5894d2cbae1f" });

            migrationBuilder.InsertData(
                table: "QuestionStatus",
                columns: new[] { "Id", "Identifier", "Name" },
                values: new object[,]
                {
                    { 1, "NEW", "new" },
                    { 2, "PUBLISHED", "published" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuestionStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "QuestionStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "92318cff-f862-493a-9a34-170f26bba41a", "AQAAAAIAAYagAAAAEBT0vE3AAOVD28XRr4NiP1HpI/GNVLs2LOMcucCq1ET08mVkvQlv975PgKw7UXonTg==", "5d8e2fad-3846-417a-83e5-74f2dba64d88" });
        }
    }
}
