using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClbTinhoc.Web.Models
{
    public class LopHoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLopHoc { get; set; }
        public string TenLopHoc { get; set; }
        public int MaKhoaHoc { get; set; }

        // Navigation properties
        [ForeignKey("MaKhoaHoc")]
        public virtual KhoaHoc KhoaHoc { get; set; }
        public virtual ICollection<StudentLopHoc> StudentLopHocs { get; set; }
        public virtual ICollection<SupportLopHoc> SupportLopHocs { get; set; }
        public virtual ICollection<KetQua> KetQuas { get; set; }
    }
}