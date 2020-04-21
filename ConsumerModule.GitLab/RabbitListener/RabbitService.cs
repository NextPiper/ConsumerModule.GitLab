using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Data;
using ConsumerModule.GitLab.Data.Models;
using ConsumerModule.GitLab.Domain;
using ConsumerModule.GitLab.RabbitListener.RabbitModels;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerModule.GitLab.RabbitListener
{
    public class RabbitService : IHostedService, IDisposable
    {
        private readonly IGitLabDataRepository _gitLabDataRepository;
        private readonly IGitLabProjectHandler _gitLabProjectHandler;

        private const string PROCESS_MODULE_EXCHANGE = "gitlab-process-exchange";
        private const string CONSUMER_MODULE_QUEUE = "consumer-module.gitlab";
        
        /// <summary>
        /// Receive rabbitMQ messages
        /// </summary>
        public RabbitService(IGitLabDataRepository gitLabDataRepository, IGitLabProjectHandler gitLabProjectHandler)
        {
            _gitLabDataRepository = gitLabDataRepository;
            _gitLabProjectHandler = gitLabProjectHandler;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            StartRabbitListener();
        }

        private async Task StartRabbitListener()
        {
            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = Program.rabbitConfig.Hostname,
                    UserName = Program.rabbitConfig.Username,
                    Password = Program.rabbitConfig.Password,
                    Port = Program.rabbitConfig.Port
                };

                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(PROCESS_MODULE_EXCHANGE, ExchangeType.Fanout);

                        var queueName = channel.QueueDeclare(CONSUMER_MODULE_QUEUE, durable: true, autoDelete: false,
                            exclusive: false);
                        channel.QueueBind(
                            queue: queueName,
                            exchange: PROCESS_MODULE_EXCHANGE,
                            routingKey: string.Empty);
                        Console.WriteLine("GitLab.Consumer waiting for events");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += async (model, ea) =>
                        {
                            Console.WriteLine($"Received event at - {DateTime.Now.ToString()}");

                            var body = ea.Body.ToArray();
                            var json = Encoding.UTF8.GetString(body);
                            var result = JsonConvert.DeserializeObject<GitLabCommitV1Processed>(json);

                            Console.WriteLine($"Succesfully serialized rabbit message from rabbitQue: ${CONSUMER_MODULE_QUEUE}");
                            
                            // Store event in mongoDB
                            
                            // Map analysed commited filees to GitLabFileDataScore
                            var fileNames = new List<string>();
                            var fileDetails = new List<GitLabFileDataScore>();
                            foreach (var scoredFile in result.CommitedFiles)
                            {
                                fileNames.Add(scoredFile.file_name);
                                fileDetails.Add(new GitLabFileDataScore
                                {
                                    AccumulatedCodeScore = scoredFile.AccumulatedCodeScore,
                                    BaseScore = scoredFile.BaseScore,
                                    DetailedScoreDict = scoredFile.DetailedScoreDict,
                                    FileName = scoredFile.file_name,
                                    Ref = scoredFile.@ref
                                });
                            }

                            await _gitLabProjectHandler.UpdateProject(result.project_id, result.project.name,
                                result.checkout_sha, fileNames, fileDetails);

                            await _gitLabDataRepository.Insert(new GitLabData
                            {
                                Id = Guid.NewGuid(),
                                Project_id = result.project_id,
                                User_id = result.user_id,
                                RepositoryName = result.repository.name,
                                Ref = result.@ref,
                                Average_Commit_Score = result.AverageCommitScore,
                                Checkout_sha = result.checkout_sha,
                                Files = fileNames,
                                FileDataScores = fileDetails,
                                Project_name = result.project.name
                            });
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
                Console.WriteLine(ex);
            }
            
            Console.WriteLine("GitLab.Consumer rabbit listener shutting down");
        }
        

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }

        public void Dispose()
        {
        }
    }
}