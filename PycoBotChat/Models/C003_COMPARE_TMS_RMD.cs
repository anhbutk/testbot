namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("003_COMPARE_TMS_RMD")]
    public partial class C003_COMPARE_TMS_RMD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RESOURCEID { get; set; }

        [StringLength(255)]
        public string FULLNAME { get; set; }

        public double? Hours_RMD { get; set; }

        public double? Hours_TMS { get; set; }

        [StringLength(255)]
        public string PROJECTNAME { get; set; }

        [StringLength(255)]
        public string Project_TMS { get; set; }

        [StringLength(50)]
        public string DEPTNAME { get; set; }

        public int? Week { get; set; }

        public int? Year { get; set; }

        [StringLength(255)]
        public string PROJECTKEY { get; set; }

        [StringLength(255)]
        public string STATUS { get; set; }
    }
}
