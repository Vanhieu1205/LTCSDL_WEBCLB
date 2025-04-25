using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClbTinhoc.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "khoahoc",
                columns: table => new
                {
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhoaHoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khoahoc", x => x.MaKhoaHoc);
                });

            migrationBuilder.CreateTable(
                name: "sinhvien",
                columns: table => new
                {
                    MaSinhVien = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LopSinhHoat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    NgayThamGia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sinhvien", x => x.MaSinhVien);
                });

            migrationBuilder.CreateTable(
                name: "support",
                columns: table => new
                {
                    MaSupport = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LopSinhHoat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support", x => x.MaSupport);
                });

            migrationBuilder.CreateTable(
                name: "user_login",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    pass = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_login", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lophoc",
                columns: table => new
                {
                    MaLopHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLopHoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lophoc", x => x.MaLopHoc);
                    table.ForeignKey(
                        name: "FK_lophoc_khoahoc_MaKhoaHoc",
                        column: x => x.MaKhoaHoc,
                        principalTable: "khoahoc",
                        principalColumn: "MaKhoaHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "diemthi",
                columns: table => new
                {
                    MaDiem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSinhVien = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false),
                    Diem = table.Column<float>(type: "real", nullable: false),
                    LanThi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diemthi", x => x.MaDiem);
                    table.ForeignKey(
                        name: "FK_diemthi_khoahoc_MaKhoaHoc",
                        column: x => x.MaKhoaHoc,
                        principalTable: "khoahoc",
                        principalColumn: "MaKhoaHoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_diemthi_sinhvien_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "sinhvien",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ketqua",
                columns: table => new
                {
                    MaKetQua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSinhVien = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    MaLopHoc = table.Column<int>(type: "int", nullable: false),
                    DiemCuoiKy = table.Column<float>(type: "real", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ketqua", x => x.MaKetQua);
                    table.ForeignKey(
                        name: "FK_ketqua_lophoc_MaLopHoc",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ketqua_sinhvien_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "sinhvien",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_lophoc",
                columns: table => new
                {
                    MaSinhVien = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    MaLopHoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_lophoc", x => new { x.MaSinhVien, x.MaLopHoc });
                    table.ForeignKey(
                        name: "FK_student_lophoc_lophoc_MaLopHoc",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_lophoc_sinhvien_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "sinhvien",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_lophoc",
                columns: table => new
                {
                    MaSupport = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    MaLopHoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support_lophoc", x => new { x.MaSupport, x.MaLopHoc });
                    table.ForeignKey(
                        name: "FK_support_lophoc_lophoc_MaLopHoc",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_support_lophoc_support_MaSupport",
                        column: x => x.MaSupport,
                        principalTable: "support",
                        principalColumn: "MaSupport",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_diemthi_MaKhoaHoc",
                table: "diemthi",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "IX_diemthi_MaSinhVien",
                table: "diemthi",
                column: "MaSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_ketqua_MaLopHoc",
                table: "ketqua",
                column: "MaLopHoc");

            migrationBuilder.CreateIndex(
                name: "IX_ketqua_MaSinhVien",
                table: "ketqua",
                column: "MaSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_lophoc_MaKhoaHoc",
                table: "lophoc",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "IX_student_lophoc_MaLopHoc",
                table: "student_lophoc",
                column: "MaLopHoc");

            migrationBuilder.CreateIndex(
                name: "IX_support_lophoc_MaLopHoc",
                table: "support_lophoc",
                column: "MaLopHoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "diemthi");

            migrationBuilder.DropTable(
                name: "ketqua");

            migrationBuilder.DropTable(
                name: "student_lophoc");

            migrationBuilder.DropTable(
                name: "support_lophoc");

            migrationBuilder.DropTable(
                name: "user_login");

            migrationBuilder.DropTable(
                name: "sinhvien");

            migrationBuilder.DropTable(
                name: "lophoc");

            migrationBuilder.DropTable(
                name: "support");

            migrationBuilder.DropTable(
                name: "khoahoc");
        }
    }
}
