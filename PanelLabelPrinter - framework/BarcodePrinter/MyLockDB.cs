using SQLite.CodeFirst;
using System.Data.Entity;

namespace BarcodePrinter
{
    public class MyLockDB : DbContext
    {
        public DbSet<Lock> Locks { get { return Set<Lock>(); } }
        public DbSet<Record> Records { get { return Set<Record>(); } }

        public MyLockDB() : base("dbConn")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            ModelConfiguration.Configure(modelBuilder);
            var init = new SqliteCreateDatabaseIfNotExists<MyLockDB>(modelBuilder);
            Database.SetInitializer(init);
        }

    }

    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureLockEntity(modelBuilder);
        }
        private static void ConfigureLockEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lock>();
            modelBuilder.Entity<Record>();
        }
    }

}
