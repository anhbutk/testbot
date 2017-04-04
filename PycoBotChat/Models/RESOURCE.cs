namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RESOURCE")]
    public partial class RESOURCE
    {
        public int RESOURCEID { get; set; }

        [StringLength(255)]
        public string FULLNAME { get; set; }

        [StringLength(255)]
        public string REMARK { get; set; }

        [StringLength(255)]
        public string KEYTECH { get; set; }

        [StringLength(255)]
        public string USERNAME { get; set; }

        [StringLength(255)]
        public string EMAIL { get; set; }

        [StringLength(255)]
        public string SKYPE { get; set; }

        [StringLength(255)]
        public string PHONE { get; set; }

        [StringLength(255)]
        public string EMPLOYEEID { get; set; }

        public int? ARCHIVE { get; set; }
    }
}
