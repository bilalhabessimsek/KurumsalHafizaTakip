using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalHafizaTakip.Migrations
{
    /// <inheritdoc />
    public partial class AnlikMesajTablosunaGecis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mesajlar_Kullanicilar_GonderenId",
                table: "Mesajlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mesajlar",
                table: "Mesajlar");

            migrationBuilder.DropIndex(
                name: "IX_Mesajlar_GonderenId",
                table: "Mesajlar");

            migrationBuilder.RenameTable(
                name: "Mesajlar",
                newName: "AnlikMesaj");

            migrationBuilder.AlterColumn<string>(
                name: "Icerik",
                table: "AnlikMesaj",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AliciId",
                table: "AnlikMesaj",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DosyaYolu",
                table: "AnlikMesaj",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "AnlikMesaj",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnlikMesaj",
                table: "AnlikMesaj",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnlikMesaj",
                table: "AnlikMesaj");

            migrationBuilder.DropColumn(
                name: "AliciId",
                table: "AnlikMesaj");

            migrationBuilder.DropColumn(
                name: "DosyaYolu",
                table: "AnlikMesaj");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "AnlikMesaj");

            migrationBuilder.RenameTable(
                name: "AnlikMesaj",
                newName: "Mesajlar");

            migrationBuilder.AlterColumn<string>(
                name: "Icerik",
                table: "Mesajlar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mesajlar",
                table: "Mesajlar",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_GonderenId",
                table: "Mesajlar",
                column: "GonderenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mesajlar_Kullanicilar_GonderenId",
                table: "Mesajlar",
                column: "GonderenId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
