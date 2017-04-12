namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TIMESHEET_PROJECT_INFO_VIEW
    {
        [Column("Project Key")]
        [StringLength(255)]
        public string Project_Key { get; set; }

        [Key]
        [Column("Commercial Type")]
        [StringLength(18)]
        public string Commercial_Type { get; set; }

        [Column("Project Name")]
        [StringLength(255)]
        public string Project_Name { get; set; }
    }
}
