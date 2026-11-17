using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalHafizaTakip.Migrations
{
    /// <inheritdoc />
    public partial class KullaniciDurumEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Durum",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Kullanicilar");
        }
    }
}
