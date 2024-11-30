using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editProfilepicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c2b35b3-8e8b-48b0-8496-2c969c3251bb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9a86826-2c91-4303-9025-90b297998a1d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb03b60f-f899-49f0-9496-f1f11b21bca5");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0dc384bc-2fa8-4bd1-9949-3ab4022d53d9", null, "User", "User" },
                    { "17bb7ad6-e7f9-413c-a258-2d41da58bb0a", null, "Admin", "Admin" },
                    { "88da184c-f0cd-44ff-84c1-e2e3879a2d1e", null, "Doctor", "Doctor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0dc384bc-2fa8-4bd1-9949-3ab4022d53d9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17bb7ad6-e7f9-413c-a258-2d41da58bb0a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88da184c-f0cd-44ff-84c1-e2e3879a2d1e");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9c2b35b3-8e8b-48b0-8496-2c969c3251bb", null, "User", "User" },
                    { "b9a86826-2c91-4303-9025-90b297998a1d", null, "Admin", "Admin" },
                    { "eb03b60f-f899-49f0-9496-f1f11b21bca5", null, "Doctor", "Doctor" }
                });
        }
    }
}
