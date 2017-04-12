namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("003_ALLOCATION_WEEK_VIEW")]
    public partial class C003_ALLOCATION_WEEK_VIEW
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RESOURCEID { get; set; }

        [StringLength(255)]
        public string FULLNAME { get; set; }

        public double? Hours_RMD { get; set; }

        [StringLength(255)]
        public string PROJECTKEY { get; set; }

        [StringLength(255)]
        public string PROJECTNAME { get; set; }

        public int? Week { get; set; }

        public int? Year { get; set; }

        [StringLength(50)]
        public string DEPTNAME { get; set; }

        [StringLength(255)]
        public string STATUS { get; set; }
    }
}
