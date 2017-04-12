namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SKILLSET_VIEW
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SKILLSETID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RESOURCEID { get; set; }

        [StringLength(255)]
        public string FULLNAME { get; set; }

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

        [StringLength(50)]
        public string DEPTNAME { get; set; }

        [StringLength(255)]
        public string STATUS { get; set; }
    }
}
