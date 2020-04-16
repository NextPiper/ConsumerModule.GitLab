using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ConsumerModule.GitLab.RabbitListener
{
    public class RabbitService : IHostedService, IDisposable
    {
        /// <summary>
        /// Receive rabbitMQ messages
        /// </summary>
        public RabbitService()
        {
            
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

/*


class Program
    {
        private const string GITLAB_COMMIT_EXCHANGE = "gitlab-commitexchange";
        private const string GITLAB_COMMIT_QUEUE = "ConsoleConsumer-987654321";
        static async Task Main(string[] args)
        {
            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "admin",
                    Password = "admin",
                    Port = 5672
                };

                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(GITLAB_COMMIT_EXCHANGE, ExchangeType.Fanout);

                        var queueName = channel.QueueDeclare(GITLAB_COMMIT_QUEUE,true, autoDelete:false,exclusive:false);
                        channel.QueueBind(
                            queue: queueName,
                            exchange: GITLAB_COMMIT_EXCHANGE,
                            routingKey: "");
                        Console.WriteLine("GitLab.Consumer waiting for events");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            Console.WriteLine("Received event at -" + DateTime.Now.ToString());
                            
                            var body = ea.Body;
                            var json = Encoding.UTF8.GetString(body);
                            var result = JsonConvert.DeserializeObject<GitLabCommitV1>(json);

                            var analyser = new Analyze();
                            
                            analyser.Analyse(result);
                            
                        };
                        
                        
                        channel.BasicConsume(
                            queue: queueName,
                            autoAck: true,
                            consumer: consumer);

                        while (true)
                        {
                            await Task.Delay(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.WriteLine("Consumer shutting down");
            Console.ReadLine();
        }
    }*/