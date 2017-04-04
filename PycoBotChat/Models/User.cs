namespace PycoBotChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [StringLength(150)]
        public string UserID { get; set; }

        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        [StringLength(150)]
        public string Channel { get; set; }

        public DateTime JoinDate { get; set; }

        public bool IsVerified { get; set; }

        [StringLength(50)]
        public string OTP { get; set; }

        public DateTime ExpiredOTP { get; set; }
    }
}
