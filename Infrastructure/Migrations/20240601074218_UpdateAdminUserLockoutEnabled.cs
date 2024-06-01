using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminUserLockoutEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "LockoutEnabled", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d66ea1cf-c9ed-454c-9888-dbbdc71bdc01", true, "AQAAAAIAAYagAAAAEMOBRv/yTqvfeIUq0xN5tVJ7FIOmb2u5BNQmRR4vhfnaX5M2tbknCjc//A23yuS4og==", "5f11962e-c860-448f-932e-06ec75cabef3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "LockoutEnabled", "PasswordHash", "SecurityStamp" },
                values: new object[] { "53746146-6c35-4731-a3aa-8ef0e77153cb", false, "AQAAAAIAAYagAAAAEOxn/UHtZIuuNXPDHO99iN9N7pQ3cFwvmXc6EvFxk+j5n+i2RMSLrALLgUzhs1TPnA==", "23dd244a-07a5-4cd7-ad88-5894d2cbae1f" });
        }
    }
}
