using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClbTinhoc.Web.Models
{
    public class StudentLopHoc
    {
        
        [Column(Order = 0)]
        public string MaSinhVien { get; set; }

        
        [Column(Order = 1)]
        public int MaKhoaHoc { get; set; }

        // Navigation properties
        [ForeignKey("MaSinhVien")]
        public virtual SinhVien SinhVien { get; set; }

        [ForeignKey("MaKhoaHoc")]
        public virtual LopHoc KhoaHoc { get; set; }
    }
}