
 

namespace WeatherWebServices.Data
{
    public class BackgroundDataFetchServices : BackgroundService
    {

      
        private readonly ILogger<BackgroundDataFetchServices> _logger;

        private readonly WeatherForecastService _weatherService;
        private readonly WeatherForecastRepository _repository;

        public BackgroundDataFetchServices(ILogger<BackgroundDataFetchServices> logger, 
            WeatherForecastService weatherService, WeatherForecastRepository repository)
        {

            _logger = logger;
            _weatherService = weatherService;
            _repository = repository;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Stopped");

            return Task.CompletedTask;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string curDate = DateTime.Today.ToString("yyyy-MM-dd");
            try
            {
                await _weatherService.SyncAndTriggerAlert(curDate);
            }
            catch
            { }


            await Task.Delay(60000, stoppingToken);
        }
    }
}
