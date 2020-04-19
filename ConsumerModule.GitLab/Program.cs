using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Configurations;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ConsumerModule.GitLab
{
    public class Program
    {
        public static MongoDBConfig mongoConfig;
        public static RabbitMQConfig rabbitConfig;
        
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            
            // Try and fetch mongoDB and rabbitMQ configs
            var httpClient = new HttpClient();
            
            var rabbitMQConfigUrl = $"http://nextpipe-service.default.svc.cluster.local:5555/core/config/rabbitmq?loadBalancer=false";
            var mongoDBConfigUrl = $"http://nextpipe-service.default.svc.cluster.local:5555/core/config/mongoDB";
            //var rabbitMQConfigUrl = $"http://localhost:5555/core/config/rabbitmq?loadBalancer=false";
            //var mongoDBConfigUrl = $"http://localhost:5555/core/config/mongoDB";
            
            // Fetch rabbitMQConfig
            try
            {
                var result = await httpClient.GetAsync(rabbitMQConfigUrl);

                if (!result.IsSuccessStatusCode)
                {
                    throw new SystemException($"Couldn't resolve rabbitMQ configuration from controlplane");
                }

                var content = await result.Content.ReadAsStringAsync();
                rabbitConfig = JsonConvert.DeserializeObject<RabbitMQConfig>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

            try
            {
                var result = await httpClient.GetAsync(mongoDBConfigUrl);

                if (!result.IsSuccessStatusCode)
                {
                    throw new SystemException($"Couldn't resolve mongoDB configuration from controlPlane");
                }

                var content = await result.Content.ReadAsStringAsync();
                mongoConfig = JsonConvert.DeserializeObject<MongoDBConfig>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLamar()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}