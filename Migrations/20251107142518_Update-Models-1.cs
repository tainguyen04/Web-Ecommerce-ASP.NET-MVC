using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCHBanDienThoaiMoi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_SanPham_SanPhamId",
                table: "GioHang");

            migrationBuilder.DropIndex(
                name: "IX_GioHang_SanPhamId",
                table: "GioHang");

            migrationBuilder.DropColumn(
                name: "SanPhamId",
                table: "GioHang");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "GioHang");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "GioHang",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ChiTietGioHang",
                columns: table => new
                {
                    GioHangId = table.Column<int>(type: "int", nullable: false),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHang", x => new { x.GioHangId, x.SanPhamId });
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_GioHang_GioHangId",
                        column: x => x.GioHangId,
                        principalTable: "GioHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHang_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_SanPhamId",
                table: "ChiTietGioHang",
                column: "SanPhamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietGioHang");

            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "GioHang");

            migrationBuilder.AddColumn<int>(
                name: "SanPhamId",
                table: "GioHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoLuong",
                table: "GioHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_SanPhamId",
                table: "GioHang",
                column: "SanPhamId");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_SanPham_SanPhamId",
                table: "GioHang",
                column: "SanPhamId",
                principalTable: "SanPham",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
