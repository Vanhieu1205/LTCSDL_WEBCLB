using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClbTinhoc.Web.Models
{
    public class KetQua
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaKetQua { get; set; }
        public string MaSinhVien { get; set; }
        public int MaLopHoc { get; set; }
        public float DiemCuoiKy { get; set; }
        public DateTime NgayCapNhat { get; set; }

        // Navigation properties
        [ForeignKey("MaSinhVien")]
        public virtual SinhVien SinhVien { get; set; }

        [ForeignKey("MaLopHoc")]
        public virtual LopHoc LopHoc { get; set; }
    }
}