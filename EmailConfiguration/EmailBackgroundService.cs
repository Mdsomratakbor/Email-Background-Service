namespace EmailConfiguration
{


    public class EmailBackgroundService : BackgroundService
    {
        public bool IsEnabled { get; set; }

        private readonly TimeSpan _period = TimeSpan.FromSeconds(600);
        private readonly ILogger<EmailBackgroundService> _logger;
        private readonly IServiceScopeFactory _factory;
        private int _executionCount = 0;
     

        public EmailBackgroundService(
            ILogger<EmailBackgroundService> logger,
            IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    if (IsEnabled)
                    {
                        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                          SampleService sampleService = asyncScope.ServiceProvider.GetRequiredService<SampleService>();
                         await sampleService.DoSomethingAsync();
                        _executionCount++;
                        _logger.LogInformation(
                            $"Executed PeriodicHostedService - Count: {_executionCount}");
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Skipped PeriodicHostedService");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(
                        $"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
                }
            }
        }
    }
}
