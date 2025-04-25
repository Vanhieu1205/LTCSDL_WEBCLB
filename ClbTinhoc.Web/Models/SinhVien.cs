using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClbTinhoc.Web.Models
{
    [Table("sinhvien")]
    public class SinhVien
    {
        public SinhVien()
        {
            // Khởi tạo các collection để tránh lỗi null
            StudentLopHocs = new HashSet<StudentLopHoc>();
            KetQuas = new HashSet<KetQua>();
            DiemThis = new HashSet<DiemThi>();
        }

        [Key]
        [Column("MaSinhVien")]
        [StringLength(20)]
        [Display(Name = "Mã sinh viên")]
        public string MaSinhVien { get; set; }

        [Required]
        [Column("HoTen")]
        [StringLength(50)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Required]
        [Column("LopSinhHoat")]
        [StringLength(50)]
        [Display(Name = "Lớp sinh hoạt")]
        public string LopSinhHoat { get; set; }

        [Required]
        [Column("Email")]
        [StringLength(50)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Column("SoDienThoai")]
        [StringLength(15)]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }

        [Required]
        [Column("NgayThamGia")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày tham gia")]
        public DateTime NgayThamGia { get; set; }

        // Navigation properties - không bắt buộc
        [NotMapped] // Thêm thuộc tính này để không ánh xạ vào database
        public virtual ICollection<StudentLopHoc> StudentLopHocs { get; set; }

        [NotMapped]
        public virtual ICollection<KetQua> KetQuas { get; set; }

        [NotMapped]
        public virtual ICollection<DiemThi> DiemThis { get; set; }
        public virtual ICollection<KhoaHoc_SinhVien> KhoaHoc_SinhVien { get; set; }
    }
}