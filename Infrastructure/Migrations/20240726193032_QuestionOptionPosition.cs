using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionOptionPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "QuestionOptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e3a762a-a5f4-471f-91d9-953024bdde16", "AQAAAAIAAYagAAAAENKPz0VtyB5quYMC4j4AHkmyarxz9oU2U45ldqj319l5Q4X3YJ+HPt+ma/fyg2OHGQ==", "803666ec-9afa-4716-a2b7-8c1961b6d8dc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "QuestionOptions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee27544b-c91d-4b2f-bdd5-310b9ca91ba0", "AQAAAAIAAYagAAAAELMioCcZz055Tewxdx3gKrOvguAJ7ompqNYbhDi+DimyBhNDo8IzBffvP871CrpDsw==", "cce18b28-4b0c-44b2-97ba-a2fc0da15f75" });
        }
    }
}
