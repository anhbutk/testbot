namespace PycoBotChat.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
 
    public partial class BotDataEntities : DbContext
    {
        public BotDataEntities()
            : base("name=BotDataEntities")
        {
        }

        public virtual DbSet<R_PERIOD> R_PERIOD { get; set; }
        public virtual DbSet<RESOURCE> RESOURCEs { get; set; }
        public virtual DbSet<R_PERIOD_VIEW> R_PERIOD_VIEW { get; set; }
        public virtual DbSet<RESOURCE_VIEW> RESOURCE_VIEW { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
