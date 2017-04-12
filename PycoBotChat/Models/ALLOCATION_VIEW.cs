namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ALLOCATION_VIEW
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RESOURCEID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PROJECTID { get; set; }

        public DateTime? UPDATED { get; set; }

        public int? ALLOCATIONID { get; set; }

        [StringLength(255)]
        public string FULLNAME { get; set; }

        [StringLength(255)]
        public string PROJECTNAME { get; set; }

        [StringLength(50)]
        public string REMARK { get; set; }

        public DateTime? WORKDATE { get; set; }

        public double? EFFORT { get; set; }

        [StringLength(255)]
        public string RSTATUS { get; set; }

        [StringLength(255)]
        public string PSTATUS { get; set; }

        [StringLength(50)]
        public string DEPTNAME { get; set; }

        public int? STATE { get; set; }

        [StringLength(255)]
        public string KEYTECH { get; set; }

        public int? MTYPE { get; set; }

        public int? CTYPE { get; set; }

        [StringLength(255)]
        public string COMPANY { get; set; }

        public int? RARCHIVE { get; set; }

        public int? PARCHIVE { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int R_PERIODID { get; set; }
    }
}
