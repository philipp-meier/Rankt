using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitRankingQuestionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54a7420f-ad5d-41b2-a73c-bfb77fd9072b", "AQAAAAIAAYagAAAAEEdaDnbnfqJRtGLD6s0+EcEGj/20n+TBKarQN2E3xIxDzeOc6Xa1iAFEy5fvqRwpNA==", "1800fe8f-67d3-40b5-b4a4-3810610c7490" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0e0b210-8bc1-4226-bee8-fe232e21733f", "AQAAAAIAAYagAAAAEGKmBhHcPlr49Dlp1mKNfEFozb3a9wvJSjHDvEOJy8R+KFZNyGG1nguTqQEEmUd+Ng==", "fcbe8dc0-1298-492e-9661-71f64d9b4162" });
        }
    }
}
