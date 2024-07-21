using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionRankOptional2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rank",
                table: "QuestionResponseItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee27544b-c91d-4b2f-bdd5-310b9ca91ba0", "AQAAAAIAAYagAAAAELMioCcZz055Tewxdx3gKrOvguAJ7ompqNYbhDi+DimyBhNDo8IzBffvP871CrpDsw==", "cce18b28-4b0c-44b2-97ba-a2fc0da15f75" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rank",
                table: "QuestionResponseItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4ddffd30-e74b-40d9-9ce5-f4d98aced37b", "AQAAAAIAAYagAAAAEDtW/8CN0XDuF2cm46ZpPnRdoO2QQbg2q9pnHEIbvIikqlHe6VPWBQM2e4oX9DEavw==", "6feacc6b-6dd7-415e-8d8f-492fa63c9af6" });
        }
    }
}
