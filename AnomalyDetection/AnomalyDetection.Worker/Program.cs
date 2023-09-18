using AnomalyDetection.Worker;
using Confluent.Kafka;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
        {
            // Configure Kafka integration and services here
            services.Configure<ConsumerConfig>(hostContext.Configuration.GetSection("KafkaConfig"));

            // Register your worker service
            services.AddHostedService<AnomalyDetectionWorker>();
        })
    .Build();

host.Run();