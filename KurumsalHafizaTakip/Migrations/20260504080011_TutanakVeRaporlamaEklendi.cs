using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalHafizaTakip.Migrations
{
    /// <inheritdoc />
    public partial class TutanakVeRaporlamaEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToplantiTutanaklari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Konu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Katilimcilar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlinanKararlar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosyaYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EkleyenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToplantiTutanaklari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToplantiTutanaklari_Kullanicilar_EkleyenId",
                        column: x => x.EkleyenId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToplantiTutanaklari_EkleyenId",
                table: "ToplantiTutanaklari",
                column: "EkleyenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToplantiTutanaklari");
        }
    }
}
