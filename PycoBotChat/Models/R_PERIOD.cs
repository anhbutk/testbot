namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class R_PERIOD
    {
        public int R_PERIODID { get; set; }

        public int RESOURCEID { get; set; }

        public DateTime START_EMPLOYMENT { get; set; }

        public DateTime END_EMPLOYMENT { get; set; }

        public double CAPACITY { get; set; }

        [StringLength(255)]
        public string TITLE { get; set; }

        [StringLength(50)]
        public string DEPTNAME { get; set; }

        [StringLength(255)]
        public string STATUS { get; set; }

        [StringLength(255)]
        public string COMPANY { get; set; }
    }
}
