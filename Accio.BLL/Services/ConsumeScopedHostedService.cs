using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Accio.BLL.Services
{
    public class ConsumeScopedHostedService : IHostedService
    {
        private readonly IServiceProvider _service;
        public ConsumeScopedHostedService(IServiceProvider service)
        {
            _service = service;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await DoWork();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task DoWork() {
            using (var scope = _service.CreateScope()) {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBackgroundEmailSender>();
                await scopedProcessingService.DoWork();
            }
        }
    }
}