using SQLite.CodeFirst;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace PanelLabelPrinter
{
    public class MyLockDB : DbContext
    {
        public DbSet<Lock> Locks { get { return Set<Lock>(); } }
        public DbSet<Record> Records { get { return Set<Record>(); } }

        private static string connectionString = string.Empty;
        public MyLockDB() : base(connectionString)
        {

        }
        public MyLockDB(string connStr)
            : base(connStr)
        {
            connectionString = connStr;
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var init = new SqliteCreateDatabaseIfNotExists<MyLockDB>(modelBuilder);
            Database.SetInitializer(init);
        }

    }

    public class Configuration : DbConfiguration, IDbConnectionFactory
    {
        public Configuration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);

            var providerServices = (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices));

            SetProviderServices("System.Data.SQLite", providerServices);
            SetProviderServices("System.Data.SQLite.EF6", providerServices);

            SetDefaultConnectionFactory(this);
        }

        public DbConnection CreateConnection(string connectionString)
        {
            var conn = new SQLiteConnection(connectionString);
            //conn.ChangePassword("password1");
            return conn;
        }
    }

    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureEntity(modelBuilder);
        }
        private static void ConfigureEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lock>();
            modelBuilder.Entity<Record>();
        }
    }
}
