using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL
{
    public class PostreSqlDbContext<IDbContext> : DbContext where IDbContext : DbContext
    {

        private readonly IConfiguration _configuration;

        private readonly IAuthenticatedUserService _authenticatedUserService;

        public PostreSqlDbContext(
            DbContextOptions<IDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options)
        {
            _configuration = configuration;
            _authenticatedUserService = authenticatedUserService;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies()
                .UseNpgsql(_configuration.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine, LogLevel.Information); ;
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<AuditEntity>();

            foreach (var entry in entries)
            {
                var authenticationName = _authenticatedUserService.GetAuthenticatedUserId();

                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.CreatedBy == null) entry.Entity.CreatedBy = authenticationName;
                    entry.Property(x => x.UpdatedBy).IsModified = false;
                    entry.Property(x => x.UpdatedAt).IsModified = false;
                    entry.Property(x => x.DeletedBy).IsModified = false;
                    entry.Property(x => x.DeletedAt).IsModified = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity.IsDeleted)
                    {
                        if (entry.Entity.DeletedBy == Guid.Empty) entry.Entity.DeletedBy = authenticationName;
                        entry.Entity.DeletedAt = DateTime.Now;
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        entry.Property(x => x.CreatedAt).IsModified = false;
                        entry.Property(x => x.UpdatedBy).IsModified = false;
                        entry.Property(x => x.UpdatedAt).IsModified = false;
                    }
                    else
                    {
                        if (entry.Entity.UpdatedBy == Guid.Empty) entry.Entity.UpdatedBy = authenticationName;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        entry.Property(x => x.CreatedAt).IsModified = false;
                        entry.Property(x => x.DeletedBy).IsModified = false;
                        entry.Property(x => x.DeletedAt).IsModified = false;
                    }
                }
            }
        }

    }

}
