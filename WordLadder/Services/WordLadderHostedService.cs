using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WordLadder.Services.Abstract;

namespace WordLadder.Services
{
    public class WordLadderHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private IPayloadManager _payloadManager;
        private IWordLadderProcessor _wordLadderProcessor;
        private IPublisherHub _publisherHub;

        public WordLadderHostedService(
            ILogger<WordLadderHostedService> logger,
            IHostApplicationLifetime appLifetime,
            IPayloadManager payloadManager,
            IWordLadderProcessor wordLadderProcessor,
            IPublisherHub publisherHub
            )
        {
            _logger = logger;
            _payloadManager = payloadManager;
            _wordLadderProcessor = wordLadderProcessor;
            _publisherHub = publisherHub;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);

        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("1. StartAsync has been called.");
            await ExecuteAsync(cancellationToken);

            //return await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("4. StopAsync has been called.");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("2. OnStarted has been called.");
        }

        private void OnStopping()
        {
            _logger.LogInformation("3. OnStopping has been called.");
        }

        private void OnStopped()
        {
            _logger.LogInformation("5. OnStopped has been called.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string _errors = "";
                if (_payloadManager.IsValid(out _errors))
                {
                    var payload = _payloadManager.LoadJob();
                    var result =  await _wordLadderProcessor.ProcessAsync(payload);
                    _publisherHub.PublishResultToAll(result);
                    break;
                }
                else
                {
                    _logger.LogWarning("Invalid Arguments.{0}", _errors);
                }

            }
            //return Task.CompletedTask;
        }
    }
}
