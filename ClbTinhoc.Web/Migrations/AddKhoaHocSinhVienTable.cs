using Microsoft.EntityFrameworkCore.Migrations;

namespace ClbTinhoc.Web.Migrations
{
    public partial class AddKhoaHocSinhVienTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "khoahoc_sinhvien",
                columns: table => new
                {
                    MaSinhVien = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khoahoc_sinhvien", x => new { x.MaSinhVien, x.MaKhoaHoc });
                    table.ForeignKey(
                        name: "FK_khoahoc_sinhvien_sinhvien_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "sinhvien",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_khoahoc_sinhvien_khoahoc_MaKhoaHoc",
                        column: x => x.MaKhoaHoc,
                        principalTable: "khoahoc",
                        principalColumn: "MaKhoaHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_khoahoc_sinhvien_MaKhoaHoc",
                table: "khoahoc_sinhvien",
                column: "MaKhoaHoc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "khoahoc_sinhvien");
        }
    }
}