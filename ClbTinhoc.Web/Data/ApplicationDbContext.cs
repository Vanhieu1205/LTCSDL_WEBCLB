using ClbTinhoc.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ClbTinhoc.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SinhVien> SinhVien { get; set; }
        public DbSet<Support> Support { get; set; }
        public DbSet<KhoaHoc> KhoaHoc { get; set; }
        public DbSet<LopHoc> LopHoc { get; set; }
        public DbSet<KetQua> KetQua { get; set; }
        public DbSet<StudentLopHoc> StudentLopHoc { get; set; }
        public DbSet<SupportLopHoc> SupportLopHoc { get; set; }
        public DbSet<DiemThi> DiemThi { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }

        public DbSet<KhoaHoc_SinhVien> KhoaHoc_SinhVien { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình composite key cho KhoaHoc_SinhVien
            modelBuilder.Entity<KhoaHoc_SinhVien>()
                .HasKey(ks => new { ks.MaSinhVien, ks.MaKhoaHoc });

            // Cấu hình mối quan hệ
            modelBuilder.Entity<KhoaHoc_SinhVien>()
                .HasOne(ks => ks.SinhVien)
                .WithMany(s => s.KhoaHoc_SinhVien)
                .HasForeignKey(ks => ks.MaSinhVien)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KhoaHoc_SinhVien>()
                .HasOne(ks => ks.KhoaHoc)
                .WithMany(k => k.KhoaHoc_SinhVien)
                .HasForeignKey(ks => ks.MaKhoaHoc)
                .OnDelete(DeleteBehavior.Cascade);
            // Cấu hình khóa chính cho các bảng
            modelBuilder.Entity<SinhVien>().HasKey(s => s.MaSinhVien);
            modelBuilder.Entity<Support>().HasKey(s => s.MaSupport);
            modelBuilder.Entity<KhoaHoc>()
                        .HasMany(k => k.LopHoc)
                        .WithOne(l => l.KhoaHoc)
                        .HasForeignKey(l => l.MaKhoaHoc);
            modelBuilder.Entity<LopHoc>().HasKey(l => l.MaLopHoc);
            modelBuilder.Entity<KetQua>().HasKey(k => k.MaKetQua);
            modelBuilder.Entity<DiemThi>().HasKey(d => d.MaDiem);
            modelBuilder.Entity<UserLogin>().HasKey(u => u.Id);

            // Cấu hình khóa chính cho các bảng liên kết
            modelBuilder.Entity<StudentLopHoc>()
                .HasKey(sl => new { sl.MaSinhVien, sl.MaKhoaHoc });

            modelBuilder.Entity<SupportLopHoc>()
                .HasKey(sl => new { sl.MaSupport, sl.MaLopHoc });

            // Cấu hình tên bảng
            modelBuilder.Entity<SinhVien>().ToTable("sinhvien");
            modelBuilder.Entity<Support>().ToTable("support");
            modelBuilder.Entity<KhoaHoc>().ToTable("khoahoc");
            modelBuilder.Entity<LopHoc>().ToTable("lophoc");
            modelBuilder.Entity<KetQua>().ToTable("ketqua");
            modelBuilder.Entity<StudentLopHoc>().ToTable("student_lophoc");
            modelBuilder.Entity<SupportLopHoc>().ToTable("support_lophoc");
            modelBuilder.Entity<DiemThi>().ToTable("diemthi");
            modelBuilder.Entity<UserLogin>().ToTable("user_login");

            
        }
    }
}