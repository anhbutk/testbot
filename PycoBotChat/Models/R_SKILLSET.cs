namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class R_SKILLSET
    {
        [Key]
        public int SKILLSETID { get; set; }

        public int? RESOURCEID { get; set; }

        [StringLength(255)]
        public string TECHTYPE { get; set; }

        [StringLength(255)]
        public string CATEGORY { get; set; }

        [StringLength(255)]
        public string SKILL { get; set; }

        public string TECHNOLOGY { get; set; }

        public int? YEAR_OF_EXP { get; set; }

        public int? PROFICIENCY { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UPDATED { get; set; }
    }
}
