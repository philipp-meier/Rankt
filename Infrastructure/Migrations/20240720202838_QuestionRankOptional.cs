using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionRankOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4ddffd30-e74b-40d9-9ce5-f4d98aced37b", "AQAAAAIAAYagAAAAEDtW/8CN0XDuF2cm46ZpPnRdoO2QQbg2q9pnHEIbvIikqlHe6VPWBQM2e4oX9DEavw==", "6feacc6b-6dd7-415e-8d8f-492fa63c9af6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f201a712-697e-4681-a502-efe95f079c3d", "AQAAAAIAAYagAAAAEMMfXuA7maTpGaW/7pBdgv4L98qXmzTjudpLLiEsEeJWdaN3TaBX9nythle1q+U8OA==", "7b3a2431-842f-4961-a92e-035554038fca" });
        }
    }
}
