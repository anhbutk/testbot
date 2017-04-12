namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RESOURCE_EMPLOYMMENT_DURATION
    {
        [StringLength(255)]
        public string FULLNAME { get; set; }

        [Key]
        [Column(Order = 0)]
        public DateTime START_EMPLOYMENT { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime END_EMPLOYMENT { get; set; }

        public int? DURATION { get; set; }

        public DateTime? MAXSTARTDATE { get; set; }

        public DateTime? MINENDDATE { get; set; }
    }
}
