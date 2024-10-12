using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
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
                    if (entry.Entity.CreatedBy == string.Empty)
                    {
                        entry.Entity.CreatedBy = authenticationName;
                    }

                    if (entry.Entity.UpdatedBy == string.Empty)
                    {
                        entry.Entity.UpdatedBy = authenticationName;
                    }

                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.UpdatedAt = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity.UpdatedBy == string.Empty)
                    {
                        entry.Entity.UpdatedBy = authenticationName;
                    }
                    entry.Entity.UpdatedAt = DateTime.Now;
                    // Để tránh thay đổi CreatedBy và CreatedAt khi cập nhật
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    entry.Property(x => x.CreatedAt).IsModified = false;
                }
            }
        }

    }

}
