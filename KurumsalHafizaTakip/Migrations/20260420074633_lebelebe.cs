using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalHafizaTakip.Migrations
{
    /// <inheritdoc />
    public partial class lebelebe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeler_Kullanicilar_ProjeBaskaniId",
                table: "Projeler");

            migrationBuilder.RenameColumn(
                name: "ProjeBaskaniId",
                table: "Projeler",
                newName: "YoneticiId");

            migrationBuilder.RenameIndex(
                name: "IX_Projeler_ProjeBaskaniId",
                table: "Projeler",
                newName: "IX_Projeler_YoneticiId");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Projeler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projeler_Kullanicilar_YoneticiId",
                table: "Projeler",
                column: "YoneticiId",
                principalTable: "Kullanicilar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeler_Kullanicilar_YoneticiId",
                table: "Projeler");

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Projeler");

            migrationBuilder.RenameColumn(
                name: "YoneticiId",
                table: "Projeler",
                newName: "ProjeBaskaniId");

            migrationBuilder.RenameIndex(
                name: "IX_Projeler_YoneticiId",
                table: "Projeler",
                newName: "IX_Projeler_ProjeBaskaniId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projeler_Kullanicilar_ProjeBaskaniId",
                table: "Projeler",
                column: "ProjeBaskaniId",
                principalTable: "Kullanicilar",
                principalColumn: "Id");
        }
    }
}
