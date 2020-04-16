using ConsumerModule.GitLab.XMLConverter;
using Lamar;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ConsumerModule.GitLab.Registry
{
    public class WebRegistry : ServiceRegistry
    {
        public WebRegistry()
        {
            For<IExcelManager>().Use<ExcelManager>();
            For<IMongoClient>().Use(ctx => new MongoClient(Program.mongoConfig.MongoClusterConnectionString)).Singleton();
            
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<WebRegistry>();
                scanner.SingleImplementationsOfInterface();
            });
        }
    }
}