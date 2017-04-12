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

        public virtual DbSet<C003_ALLOCATION_WEEK_VIEW> C003_ALLOCATION_WEEK_VIEW { get; set; }
        public virtual DbSet<C003_COMPARE_TMS_RMD> C003_COMPARE_TMS_RMD { get; set; }
        public virtual DbSet<C003_TIMESHEET_WEEK_VIEW> C003_TIMESHEET_WEEK_VIEW { get; set; }
        public virtual DbSet<ALLOCATION_EFFORT_VIEW> ALLOCATION_EFFORT_VIEW { get; set; }
        public virtual DbSet<ALLOCATION_VIEW> ALLOCATION_VIEW { get; set; }
        public virtual DbSet<PROJECT_STATUS_VIEW> PROJECT_STATUS_VIEW { get; set; }
        public virtual DbSet<PROJECT_VIEW> PROJECT_VIEW { get; set; }
        public virtual DbSet<R_LAST_DEPTNAME> R_LAST_DEPTNAME { get; set; }
        public virtual DbSet<R_LAST_ENDDATE> R_LAST_ENDDATE { get; set; }
      
        public virtual DbSet<RESOURCE_EMPLOYMMENT_DURATION> RESOURCE_EMPLOYMMENT_DURATION { get; set; }

        public virtual DbSet<SKILLSET_VIEW> SKILLSET_VIEW { get; set; }
        public virtual DbSet<TIMESHEET_PROJECT_INFO_VIEW> TIMESHEET_PROJECT_INFO_VIEW { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TIMESHEET_PROJECT_INFO_VIEW>()
              .Property(e => e.Commercial_Type)
              .IsUnicode(false);
        }
    }
}
