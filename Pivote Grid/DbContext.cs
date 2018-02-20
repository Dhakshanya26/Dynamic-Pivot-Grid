using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Privote_Grid
{
    public class RagDBContext1 : DbContext
    {
        public DbSet<RagStatus> RagStatuss { get; set; }
        public RagDBContext1()
            : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\Sudha Arumugam\Documents\GitHub\Dynamic-Pivot-Grid\Pivote Grid\App_Data\SampleDb.mdf';Integrated Security=True")
        {
            Database.SetInitializer<RagDBContext1>(null);
            Configuration.LazyLoadingEnabled = true;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RagStatus>().ToTable("tbl_FileModel");
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