namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectStatusHistory")]
    public partial class ProjectStatusHistory
    {
        [Key]
        public int StatusHistoryID { get; set; }

        public int ProjectID { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public string Remark { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
