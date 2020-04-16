namespace ConsumerModule.GitLab.Configurations
{
    public class MongoDBConfig
    {
        public string MongoClusterConnectionString { get; set; }
        public string DefaultDatabaseName { get; set; } = "consumer-gitlab";
    }
}