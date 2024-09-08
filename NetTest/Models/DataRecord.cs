using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetTest.Models
{
    [Table("Tbl_BasicData")]
    public class DataRecord
    {
        [Key]
        public int DataId { get; set; }

        [Required]
        public string DataName { get; set; }

        public string DataDesc { get; set; }

        [BindNever]
        public string? DataImage { get; set; } // Nullable string

        public DateTime InquiryDate { get; set; }

        public DateTime? UpdateDate { get; set; } // Nullable DateTime

        [Required]
        public string InquiryUser { get; set; }

        public string? UpdateUser { get; set; } // Nullable string
    }


}
