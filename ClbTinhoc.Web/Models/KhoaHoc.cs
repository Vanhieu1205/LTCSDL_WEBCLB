using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClbTinhoc.Web.Models
{
    [Table("khoahoc")]
    public class KhoaHoc
    {
        public KhoaHoc()
        {
            LopHoc = new HashSet<LopHoc>();
            DiemThi = new HashSet<DiemThi>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaKhoaHoc { get; set; }

        [Required(ErrorMessage = "Tên khóa học là bắt buộc")]
        [StringLength(100)]
        [Display(Name = "Tên khóa học")]
        public string TenKhoaHoc { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày kết thúc")]
        public DateTime NgayKetThuc { get; set; }

        [Required(ErrorMessage = "Hình ảnh là bắt buộc")]
        [Display(Name = "Hình ảnh")]
        public string image { get; set; }

        // Navigation properties
        [NotMapped]
        public virtual ICollection<LopHoc> LopHoc { get; set; }

        [NotMapped]
        public virtual ICollection<DiemThi> DiemThi { get; set; }
        public virtual ICollection<KhoaHoc_SinhVien> KhoaHoc_SinhVien { get; set; } // Thêm mối quan hệ với KhoaHoc_SinhVien
    }
}
