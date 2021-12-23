using SQLite.CodeFirst;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SQLite.EF6;


namespace BarcodePrinter
{
    public class MyDatabase : DbContext
    {
        public DbSet<Lock> Locks { get { return Set<Lock>(); } }
        public DbSet<Record> Records { get { return Set<Record>(); } }
        public DbSet<Config> Configs { get { return Set<Config>(); } }

        private static string connectionString = string.Empty;

        public MyDatabase() : base(connectionString)
        {

        }
        public MyDatabase(string connStr)
            : base(connStr)
        {
            connectionString = connStr;
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var init = new SqliteCreateDatabaseIfNotExists<MyDatabase>(modelBuilder);
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
            return conn;
        }
    }

    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureHubEntity(modelBuilder);
        }
        private static void ConfigureHubEntity(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lock>();
            modelBuilder.Entity<Record>();
            modelBuilder.Entity<Config>();
        }
    }
}
