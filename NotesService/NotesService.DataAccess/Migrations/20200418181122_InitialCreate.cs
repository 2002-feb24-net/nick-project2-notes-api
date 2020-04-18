using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NotesService.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    DateModified = table.Column<DateTime>(type: "DATETIME2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    TagName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Tags_TagName",
                        column: x => x.TagName,
                        principalTable: "Tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NoteTags",
                columns: table => new
                {
                    NoteId = table.Column<int>(nullable: false),
                    TagName = table.Column<string>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTags", x => new { x.NoteId, x.TagName });
                    table.ForeignKey(
                        name: "FK_NoteTags_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteTags_Tags_TagName",
                        column: x => x.TagName,
                        principalTable: "Tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tags",
                column: "Name",
                values: new object[]
                {
                    "services",
                    "basic",
                    "advanced"
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Harold" },
                    { 2, "Nick" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "AuthorId", "DateModified", "TagName", "Text" },
                values: new object[] { 2, 1, new DateTime(2020, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "C# is an OOP language" });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "AuthorId", "DateModified", "TagName", "Text" },
                values: new object[] { 1, 2, new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "REST stands for representational state transfer" });

            migrationBuilder.InsertData(
                table: "NoteTags",
                columns: new[] { "NoteId", "TagName", "Order" },
                values: new object[] { 2, "basic", 0 });

            migrationBuilder.InsertData(
                table: "NoteTags",
                columns: new[] { "NoteId", "TagName", "Order" },
                values: new object[] { 1, "services", 0 });

            migrationBuilder.InsertData(
                table: "NoteTags",
                columns: new[] { "NoteId", "TagName", "Order" },
                values: new object[] { 1, "basic", 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AuthorId",
                table: "Notes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TagName",
                table: "Notes",
                column: "TagName");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTags_TagName",
                table: "NoteTags",
                column: "TagName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteTags");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
