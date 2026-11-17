using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalHafizaTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmanTablosu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmanId",
                table: "Kullanicilar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departmanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departmanlar", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Departmanlar_DepartmanId",
                table: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Departmanlar");

            migrationBuilder.DropIndex(
                name: "IX_Kullanicilar_DepartmanId",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "DepartmanId",
                table: "Kullanicilar");
        }
    }
}
