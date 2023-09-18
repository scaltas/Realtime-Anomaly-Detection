using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace AnomalyDetection.Worker
{
    public class AnomalyDetectionWorker : BackgroundService
    {
        private readonly ILogger<AnomalyDetectionWorker> _logger;
        private readonly IConsumer<string, string> _consumer;

        public AnomalyDetectionWorker(
            ILogger<AnomalyDetectionWorker> logger,
            IOptions<ConsumerConfig> consumerConfig)
        {
            _logger = logger;
            _consumer = new ConsumerBuilder<string, string>(consumerConfig.Value).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("mytopic");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    var messageKey = consumeResult.Message.Key;
                    var messageValue = consumeResult.Message.Value;

                    // Perform anomaly detection with your machine learning model
                    // Implement alerting and reporting logic here based on detected anomalies

                    _logger.LogInformation($"Received message: Key: {messageKey}, Value: {messageValue}");
                }
                catch (OperationCanceledException)
                {
                    // Handle cancellation
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing Kafka messages.");
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();
            _consumer.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}