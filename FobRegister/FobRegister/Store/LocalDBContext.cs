using SQLite.CodeFirst;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SQLite.EF6;


namespace FobRegister
{
    public class LocalDBContext : DbContext
    {
        public DbSet<Fob> Fobs { get { return Set<Fob>(); } }

        private static string connectionString = string.Empty;

        public LocalDBContext() : base(@"data source=sqlite.db")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var init = new SqliteCreateDatabaseIfNotExists<LocalDBContext>(modelBuilder);
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
            modelBuilder.Entity<Fob>();
        }
    }
}
