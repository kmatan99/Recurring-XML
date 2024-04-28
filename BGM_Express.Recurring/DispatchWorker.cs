using BGM_Express.Core.Service;

namespace BGM_Express.Recurring
{
    public class DispatchWorker : BackgroundService
    {
        private readonly ILogger<DispatchWorker> _logger;
        private readonly IXMLService _xmlService;
        private int _counter = 0;

        public DispatchWorker(ILogger<DispatchWorker> logger, IXMLService xmlService)
        {
            _logger = logger;
            _xmlService = xmlService;
            _counter = _xmlService.GetProcessedOrdersAmount();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try 
                {
                    _logger.LogInformation($"Dispatch worker cycle running at {DateTime.Now}, creating order number: #{_counter}");
                    _xmlService.DispatchOrder(_counter);
                    _counter++;
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error while dispatching order number: #{ex.Message}");
                    throw;
                }

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
