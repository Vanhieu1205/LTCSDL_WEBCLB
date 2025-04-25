using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClbTinhoc.Web.Models
{
    public class SupportLopHoc
    {
        
        [Column(Order = 0)]
        public string MaSupport { get; set; }

        
        [Column(Order = 1)]
        public int MaLopHoc { get; set; }

        // Navigation properties
        [ForeignKey("MaSupport")]
        public virtual Support Support { get; set; }

        [ForeignKey("MaLopHoc")]
        public virtual LopHoc LopHoc { get; set; }
    }
}