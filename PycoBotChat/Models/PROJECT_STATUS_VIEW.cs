namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PROJECT_STATUS_VIEW
    {
        [StringLength(255)]
        public string PROJECTNAME { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime UpdatedDate { get; set; }

        public string Remark { get; set; }
    }
}
