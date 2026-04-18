
 

namespace WeatherWebServices.Data
{
    public class BackgroundDataFetchServices : BackgroundService
    {

      
        private readonly ILogger<BackgroundDataFetchServices> _logger;

        private readonly WeatherForecastService _weatherService;
      

        public BackgroundDataFetchServices(ILogger<BackgroundDataFetchServices> logger, 
            WeatherForecastService weatherService)
        {

            _logger = logger;
            _weatherService = weatherService;
           
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Background Service is Started.");
            _logger.LogInformation("Service Started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Background Service is Stopped.");
            _logger.LogInformation("Service Stopped");

            return Task.CompletedTask;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            System.Console.WriteLine("Background Service is running.");    




            string curDate = DateTime.Today.ToString("yyyy-MM-dd");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _weatherService.SyncAndTriggerAlert("");
                }
                catch (Exception ex)
                {
                    // Log the error so the loop can continue to the next interval
                    _logger.LogError(ex, "Error occurred during execution. Retrying in next interval.");
                }

                // The loop will stay here until the timer finishes
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

                //await Task.Delay(6000, stoppingToken);
            }
        }
    }
    
}
