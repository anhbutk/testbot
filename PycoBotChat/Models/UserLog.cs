namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserLog")]
    public partial class UserLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string UserID { get; set; }

        [Required]
        [StringLength(150)]
        public string UserName { get; set; }

        [Required]
        [StringLength(150)]
        public string Channel { get; set; }

        public DateTime created { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public int? CountOfTurnsToWin { get; set; }

        [StringLength(150)]
        public string WinnerUserName { get; set; }
    }
}
