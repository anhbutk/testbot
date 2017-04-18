namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PROJECT")]
    public partial class PROJECT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROJECT()
        {
            ALLOCATIONs = new HashSet<ALLOCATION>();
        }

        public int PROJECTID { get; set; }

        [StringLength(255)]
        public string PROJECTNAME { get; set; }

        [StringLength(255)]
        public string CLIENT { get; set; }

        [StringLength(255)]
        public string STATUS { get; set; }

        [StringLength(255)]
        public string TYPE { get; set; }

        [StringLength(255)]
        public string MODEL { get; set; }

        [StringLength(255)]
        public string PROJECTKEY { get; set; }

        [StringLength(500)]
        public string KEYTECH { get; set; }

        [StringLength(100)]
        public string ACCOUNTMANAGER { get; set; }

        [StringLength(255)]
        public string DELIVERY { get; set; }

        [Column(TypeName = "ntext")]
        public string REMARK { get; set; }

        public int? ARCHIVE { get; set; }

        public int? STATE { get; set; }

        public int? CTYPE { get; set; }

        public int? MTYPE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALLOCATION> ALLOCATIONs { get; set; }
    }
}
