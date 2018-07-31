using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HostedService.DbContext.Sample
{
    public class HostedServiceWithContext : HostedService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public HostedServiceWithContext(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = this.serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<MyDbContext>();
            }
            
            return Task.CompletedTask;
        }
    }
}