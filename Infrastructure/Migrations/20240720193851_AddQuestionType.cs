using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Identifier = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f201a712-697e-4681-a502-efe95f079c3d", "AQAAAAIAAYagAAAAEMMfXuA7maTpGaW/7pBdgv4L98qXmzTjudpLLiEsEeJWdaN3TaBX9nythle1q+U8OA==", "7b3a2431-842f-4961-a92e-035554038fca" });

            migrationBuilder.InsertData(
                table: "QuestionType",
                columns: new[] { "Id", "Identifier", "Name" },
                values: new object[,]
                {
                    { 1, "RQ", "Ranking Question" },
                    { 2, "V", "Voting" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TypeId",
                table: "Questions",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionType_TypeId",
                table: "Questions",
                column: "TypeId",
                principalTable: "QuestionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionType_TypeId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionType");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TypeId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b562bf95-ac55-488a-85d2-45685c915c09", "AQAAAAIAAYagAAAAEOpzhgg+/5b4PI8LA08iK24AchTFYciXG1+ObItsxumpKPM52n8wBYd+x0cLXN65lA==", "dcd233f2-9cfd-4a9a-be64-910d12a3e313" });
        }
    }
}
