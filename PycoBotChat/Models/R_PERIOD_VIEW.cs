namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class R_PERIOD_VIEW
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int R_PERIODID { get; set; }

        [StringLength(255)]
        public string FULLNAME { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime START_EMPLOYMENT { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime END_EMPLOYMENT { get; set; }

        [Key]
        [Column(Order = 3)]
        public double CAPACITY { get; set; }

        [StringLength(255)]
        public string TITLE { get; set; }

        [StringLength(50)]
        public string DEPTNAME { get; set; }

        [StringLength(255)]
        public string STATUS { get; set; }

        [StringLength(255)]
        public string COMPANY { get; set; }

        [StringLength(255)]
        public string KEYTECH { get; set; }

        public int? ARCHIVE { get; set; }
    }
}
