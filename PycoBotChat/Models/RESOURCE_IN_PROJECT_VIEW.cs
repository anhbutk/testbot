namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RESOURCE_IN_PROJECT_VIEW
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(255)]
        public string FULLNAME { get; set; }

        [StringLength(255)]
        public string PROJECTNAME { get; set; }

        [StringLength(255)]
        public string ResourceStatus { get; set; }
        [StringLength(255)]
        public string ProjectStatus { get; set; }

        public DateTime? FirstDate { get; set; }

        public DateTime? LastDate { get; set; }

        [StringLength(50)]
        public string DEPTNAME { get; set; }

        

        [StringLength(255)]
        public string Duration { get; set; }

      

        [StringLength(255)]
        public string SumEFFORT { get; set; }

        [StringLength(255)]
        public string CTYPE { get; set; }

        [StringLength(255)]
        public string TotalHours { get; set; }

        [StringLength(255)]
        public string PROJECTKEY { get; set; }

       
    }
}
