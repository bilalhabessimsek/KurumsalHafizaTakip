using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KurumsalHafizaTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToKullanici : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OnayTarihi",
                table: "Gorevler",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnayTarihi",
                table: "Gorevler");
        }
    }
}
