namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ALLOCATION")]
    public partial class ALLOCATION
    {
        public int ALLOCATIONID { get; set; }

        public int RESOURCEID { get; set; }

        public int PROJECTID { get; set; }

        [StringLength(50)]
        public string REMARK { get; set; }

        public DateTime? UPDATED { get; set; }

        public virtual PROJECT PROJECT { get; set; }

        public virtual RESOURCE RESOURCE { get; set; }
    }
}
