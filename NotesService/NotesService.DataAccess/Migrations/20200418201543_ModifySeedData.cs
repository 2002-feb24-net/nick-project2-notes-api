using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NotesService.DataAccess.Migrations
{
    public partial class ModifySeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Angular client");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "ASP.NET Core MVC client");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Harold" },
                    { 4, "Nick" }
                });

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthorId", "DateModified" },
                values: new object[] { 4, new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AuthorId", "DateModified" },
                values: new object[] { 3, new DateTime(2020, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NoteTags",
                keyColumns: new[] { "NoteId", "TagName" },
                keyValues: new object[] { 1, "basic" });

            migrationBuilder.DeleteData(
                table: "NoteTags",
                keyColumns: new[] { "NoteId", "TagName" },
                keyValues: new object[] { 1, "services" });

            migrationBuilder.DeleteData(
                table: "NoteTags",
                keyColumns: new[] { "NoteId", "TagName" },
                keyValues: new object[] { 2, "basic" });

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "AuthorId", "DateModified", "TagName", "Text" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2020, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "C# is an OOP language" },
                    { 1, 2, new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "REST stands for representational state transfer" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Harold");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Nick");

            migrationBuilder.InsertData(
                table: "NoteTags",
                columns: new[] { "NoteId", "TagName", "Order" },
                values: new object[] { 2, "basic", 0 });

            migrationBuilder.InsertData(
                table: "NoteTags",
                columns: new[] { "NoteId", "TagName", "Order" },
                values: new object[] { 1, "basic", 0 });

            migrationBuilder.InsertData(
                table: "NoteTags",
                columns: new[] { "NoteId", "TagName", "Order" },
                values: new object[] { 1, "services", 0 });
        }
    }
}
