namespace BirthdayBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Birthday birthday1;
        public Worker(Birthday birthday, ILogger<Worker> logger)
        {
            birthday1 = birthday;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                if (DateTime.UtcNow.Hour == 0)
                {
                    await birthday1.WishBirthday();
                    await Task.Delay(3600000, stoppingToken);
                }
                
            }
        }
    }
}