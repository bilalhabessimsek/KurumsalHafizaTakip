using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalHafizaTakip.Migrations
{
    /// <inheritdoc />
    public partial class BildirimlerTablosuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Departmanlar_DepartmanId",
                table: "Kullanicilar");

            migrationBuilder.DropIndex(
                name: "IX_Kullanicilar_DepartmanId",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "DepartmanId",
                table: "Kullanicilar");

            migrationBuilder.CreateTable(
                name: "Bildirimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlgiliKayitId = table.Column<int>(type: "int", nullable: true),
                    OkunduMu = table.Column<bool>(type: "bit", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bildirimler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bildirimler");

            migrationBuilder.AddColumn<int>(
                name: "DepartmanId",
                table: "Kullanicilar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_DepartmanId",
                table: "Kullanicilar",
                column: "DepartmanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanicilar_Departmanlar_DepartmanId",
                table: "Kullanicilar",
                column: "DepartmanId",
                principalTable: "Departmanlar",
                principalColumn: "Id");
        }
    }
}
