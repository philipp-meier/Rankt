using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameRankingQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b562bf95-ac55-488a-85d2-45685c915c09", "AQAAAAIAAYagAAAAEOpzhgg+/5b4PI8LA08iK24AchTFYciXG1+ObItsxumpKPM52n8wBYd+x0cLXN65lA==", "dcd233f2-9cfd-4a9a-be64-910d12a3e313" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d66ea1cf-c9ed-454c-9888-dbbdc71bdc01", "AQAAAAIAAYagAAAAEMOBRv/yTqvfeIUq0xN5tVJ7FIOmb2u5BNQmRR4vhfnaX5M2tbknCjc//A23yuS4og==", "5f11962e-c860-448f-932e-06ec75cabef3" });
        }
    }
}
