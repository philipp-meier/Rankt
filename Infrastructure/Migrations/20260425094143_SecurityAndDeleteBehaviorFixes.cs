using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecurityAndDeleteBehaviorFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionStatus_StatusId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionType_TypeId",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                column: "PasswordHash",
                value: null);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionStatus_StatusId",
                table: "Questions",
                column: "StatusId",
                principalTable: "QuestionStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionType_TypeId",
                table: "Questions",
                column: "TypeId",
                principalTable: "QuestionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionStatus_StatusId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionType_TypeId",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENKPz0VtyB5quYMC4j4AHkmyarxz9oU2U45ldqj319l5Q4X3YJ+HPt+ma/fyg2OHGQ==");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionStatus_StatusId",
                table: "Questions",
                column: "StatusId",
                principalTable: "QuestionStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionType_TypeId",
                table: "Questions",
                column: "TypeId",
                principalTable: "QuestionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
