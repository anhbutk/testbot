namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PROJECT_VIEW
    {
        [Key]
        public int Expr1 { get; set; }

        [StringLength(255)]
        public string PROJECTNAME { get; set; }

        [StringLength(255)]
        public string CLIENT { get; set; }

        [StringLength(255)]
        public string STATUS { get; set; }

        [StringLength(255)]
        public string TYPE { get; set; }

        [StringLength(255)]
        public string MODEL { get; set; }

        [StringLength(255)]
        public string PROJECTKEY { get; set; }

        [StringLength(100)]
        public string ACCOUNTMANAGER { get; set; }

        [StringLength(255)]
        public string DELIVERY { get; set; }

        [Column(TypeName = "ntext")]
        public string REMARK { get; set; }

        [StringLength(500)]
        public string KEYTECH { get; set; }

        public int? ARCHIVE { get; set; }
    }
}
