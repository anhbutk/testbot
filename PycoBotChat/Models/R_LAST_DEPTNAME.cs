namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class R_LAST_DEPTNAME
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RESOURCEID { get; set; }

        [StringLength(50)]
        public string DEPTNAME { get; set; }

        [StringLength(255)]
        public string TITLE { get; set; }

        [StringLength(255)]
        public string COMPANY { get; set; }

        [StringLength(255)]
        public string STATUS { get; set; }
    }
}
