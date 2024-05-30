using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rankt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitRankingQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RankingQuestionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingQuestionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RankingQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExternalIdentifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxSelectableItems = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedById = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankingQuestions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RankingQuestions_AspNetUsers_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RankingQuestions_RankingQuestionStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RankingQuestionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankingQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RankingQuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExternalIdentifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedById = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankingQuestionOptions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RankingQuestionOptions_AspNetUsers_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RankingQuestionOptions_RankingQuestions_RankingQuestionId",
                        column: x => x.RankingQuestionId,
                        principalTable: "RankingQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankingQuestionResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RankingQuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExternalIdentifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingQuestionResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankingQuestionResponses_RankingQuestions_RankingQuestionId",
                        column: x => x.RankingQuestionId,
                        principalTable: "RankingQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankingQuestionResponseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RankingQuestionResponseId = table.Column<int>(type: "INTEGER", nullable: false),
                    RankingQuestionOptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingQuestionResponseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankingQuestionResponseItems_RankingQuestionResponses_RankingQuestionOptionId",
                        column: x => x.RankingQuestionOptionId,
                        principalTable: "RankingQuestionResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RankingQuestionResponseItems_RankingQuestionResponses_RankingQuestionResponseId",
                        column: x => x.RankingQuestionResponseId,
                        principalTable: "RankingQuestionResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0e0b210-8bc1-4226-bee8-fe232e21733f", "AQAAAAIAAYagAAAAEGKmBhHcPlr49Dlp1mKNfEFozb3a9wvJSjHDvEOJy8R+KFZNyGG1nguTqQEEmUd+Ng==", "fcbe8dc0-1298-492e-9661-71f64d9b4162" });

            migrationBuilder.InsertData(
                table: "RankingQuestionStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "in progress" },
                    { 2, "completed" },
                    { 3, "archived" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestionOptions_CreatedById",
                table: "RankingQuestionOptions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestionOptions_ExternalIdentifier",
                table: "RankingQuestionOptions",
                column: "ExternalIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestionOptions_LastModifiedById",
                table: "RankingQuestionOptions",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestionOptions_RankingQuestionId",
                table: "RankingQuestionOptions",
                column: "RankingQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestionResponseItems_RankingQuestionOptionId",
                table: "RankingQuestionResponseItems",
                column: "RankingQuestionOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestionResponseItems_RankingQuestionResponseId",
                table: "RankingQuestionResponseItems",
                column: "RankingQuestionResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestionResponses_RankingQuestionId",
                table: "RankingQuestionResponses",
                column: "RankingQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestions_CreatedById",
                table: "RankingQuestions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestions_ExternalIdentifier",
                table: "RankingQuestions",
                column: "ExternalIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestions_LastModifiedById",
                table: "RankingQuestions",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestions_StatusId",
                table: "RankingQuestions",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RankingQuestionOptions");

            migrationBuilder.DropTable(
                name: "RankingQuestionResponseItems");

            migrationBuilder.DropTable(
                name: "RankingQuestionResponses");

            migrationBuilder.DropTable(
                name: "RankingQuestions");

            migrationBuilder.DropTable(
                name: "RankingQuestionStatus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "118d6207-3d51-4ad0-b059-ffab450e4458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32909fe5-dced-4124-ac1b-c8c9ddc98e60", "AQAAAAIAAYagAAAAEEwRMx5xbufEa/tGEf9pTpf49j1xJet0qUumWWQobMof5F/ZqXAevCLOYrC0GmYqDQ==", "48806fe6-71e2-4c37-ac83-9acf73646a47" });
        }
    }
}
