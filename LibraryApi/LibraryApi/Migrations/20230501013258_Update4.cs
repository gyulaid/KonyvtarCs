using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    public partial class Update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lendings_Books_BookId",
                table: "Lendings");

            migrationBuilder.DropForeignKey(
                name: "FK_Lendings_Members_MemberId",
                table: "Lendings");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "Lendings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Lendings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lendings_Books_BookId",
                table: "Lendings",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lendings_Members_MemberId",
                table: "Lendings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lendings_Books_BookId",
                table: "Lendings");

            migrationBuilder.DropForeignKey(
                name: "FK_Lendings_Members_MemberId",
                table: "Lendings");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "Lendings",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Lendings",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Lendings_Books_BookId",
                table: "Lendings",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lendings_Members_MemberId",
                table: "Lendings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}
