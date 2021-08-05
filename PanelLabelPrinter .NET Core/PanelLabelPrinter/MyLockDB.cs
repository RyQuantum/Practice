//using SQLite.CodeFirst;
//using System.Data.Entity;

using Microsoft.EntityFrameworkCore;
using System;

namespace PanelLabelPrinter
{
    public class MyLockDB : DbContext
    {
        public DbSet<Lock> Locks { get { return Set<Lock>(); } }
        public DbSet<Record> Records { get { return Set<Record>(); } }

        public MyLockDB()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Rently\\Label Printer (panel)\\sqlite.db");
        }
    }
}
