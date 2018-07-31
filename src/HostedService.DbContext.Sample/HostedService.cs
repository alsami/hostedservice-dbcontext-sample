using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace HostedService.DbContext.Sample
{
    public abstract class HostedService : IHostedService
    {
        private Task executingTask;
        private CancellationTokenSource cancellationTokenSource;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            this.executingTask = this.ExecuteAsync(this.cancellationTokenSource.Token);

            return this.executingTask.IsCompleted ? this.executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (this.executingTask == null)
            {
                return;
            }

            this.cancellationTokenSource.Cancel();

            await Task.WhenAny(this.executingTask, Task.Delay(-1, cancellationToken));
        }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}