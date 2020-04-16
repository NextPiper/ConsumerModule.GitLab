using ConsumerModule.GitLab.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ConsumerModule.GitLab.Data
{
    public interface IGitLabDataRepository : IMongoDB<GitLabData>
    {
    }
    
    public class GitLabDataRepository : BaseMongoDB<GitLabData>, IGitLabDataRepository
    {
        public GitLabDataRepository(IMongoClient mongoClient) : base(mongoClient)
        {
        }
    }
}