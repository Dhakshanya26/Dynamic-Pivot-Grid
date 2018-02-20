using System.Data.Entity;

namespace Sample_Code.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<RagStatus> RagStatuss { get; set; }
        public DatabaseContext()
            : base("Data Source=dvonlinesql.database.windows.net;initial catalog=eshopping;user id=dvadmin;password=Pa55word$;")
        {
            Database.SetInitializer<DatabaseContext>(null);
            Configuration.LazyLoadingEnabled = true;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RagStatus>().ToTable("tbl_exp_RagViewModels");
            modelBuilder.Entity<RagStatus>().HasKey(k => k.Id);

        }
    }

    public class RagStatus
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string DocName { get; set; }

        public int? DocId { get; set; }

        public int StatusId { get; set; }
    }
}