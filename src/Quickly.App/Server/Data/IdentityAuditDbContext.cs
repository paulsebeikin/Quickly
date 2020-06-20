using Quickly.App.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Threading;

namespace Quickly.App.Server.Data
{
    public class IdentityAuditDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        private readonly AuditAdapter _adapter = new AuditAdapter();

        public IdentityAuditDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<UserAudit> Audits { get; set; }

        public override Task<int> SaveChangesAsync(
            CancellationToken token = default)
        {
            _adapter.Snap(this);
            return base.SaveChangesAsync(token);
        }
    }
}
