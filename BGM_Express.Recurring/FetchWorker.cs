using BGM_Express.Core.Service;

namespace BGM_Express.Recurring
{
    public class FetchWorker : BackgroundService
    {
        private readonly ILogger<FetchWorker> _logger;
        private readonly IXMLService _xmlService;

        public FetchWorker(ILogger<FetchWorker> logger, IXMLService xmlService)
        {
            _logger = logger;
            _xmlService = xmlService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Delay start time of fetch worker
            await Task.Delay(5000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("FetchWorker worker running cycle at {time}", DateTime.Now);

                try 
                {
                    _xmlService.CorrectOrderMismatch();
                    var result = _xmlService.UpsertOrders();
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error while fetching orders: {ex.Message}");
                }

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
