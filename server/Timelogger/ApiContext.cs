using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using Timelogger.Infrastructure.Entities;
using Timelogger.Infrastructure.Interfaces;

namespace Timelogger
{
	public class ApiContext : DbContext, IApiContext
	{
		public ApiContext(DbContextOptions<ApiContext> options)
			: base(options)
		{
		}

		public DbSet<Project> Project { get; set; }
        public DbSet<Timesheet> Timesheet { get; set; }

        public void Save()
        {
            SaveChanges();
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var changeSet = ChangeTracker.Entries();

            if (changeSet == null) return base.SaveChanges();
            var dbEntityEntries = changeSet.ToList();
            foreach (var entry in dbEntityEntries.Where(c => c.State == EntityState.Added))
            {
                var creationDateInfo = entry.Entity.GetType().GetProperty("CreationDate", BindingFlags.Public | BindingFlags.Instance);
                var modifiedDateInfo = entry.Entity.GetType().GetProperty("UpdateDate", BindingFlags.Public | BindingFlags.Instance);
                var isDeletedInfo = entry.Entity.GetType().GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance);

                if (null != creationDateInfo && creationDateInfo.CanWrite)
                {
                    creationDateInfo.SetValue(entry.Entity, DateTime.Now, null);
                }

                if (null != modifiedDateInfo && modifiedDateInfo.CanWrite)
                {
                    modifiedDateInfo.SetValue(entry.Entity, DateTime.Now, null);
                }

                if (null != isDeletedInfo && isDeletedInfo.CanWrite)
                {
                    isDeletedInfo.SetValue(entry.Entity, false, null);
                }
            }

            foreach (var entry in dbEntityEntries.Where(c => c.State == EntityState.Modified))
            {
                var modifiedDate = entry.Entity.GetType().GetProperty("UpdateDate", BindingFlags.Public | BindingFlags.Instance);

                if (null != modifiedDate && modifiedDate.CanWrite)
                {
                    modifiedDate.SetValue(entry.Entity, DateTime.UtcNow, null);
                }
            }

            return base.SaveChanges();
        }
    }
}
