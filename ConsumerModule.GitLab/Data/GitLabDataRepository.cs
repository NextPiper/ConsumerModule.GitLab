using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Data.Models;
using ConsumerModule.GitLab.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ConsumerModule.GitLab.Data
{
    public interface IGitLabDataRepository : IMongoDB<GitLabData>
    {
        Task<IEnumerable<GitLabData>> GetProject(int projectId);
        Task<GitLabData> GetCommit(int projectId, Guid commitId);
    }
    
    public class GitLabDataRepository : BaseMongoDB<GitLabData>, IGitLabDataRepository
    {
        public GitLabDataRepository(IMongoClient mongoClient) : base(mongoClient)
        {
        }

        public async Task<IEnumerable<GitLabData>> GetProject(int projectId)
        {
            return await Collection().Find(t => t.Project_id == projectId).ToListAsync();
        }

        public async Task<GitLabData> GetCommit(int projectId, Guid commitId)
        {
            return await Collection()
                .Find(t => t.Id == commitId && t.Project_id == projectId)
                .SingleOrDefaultAsync();
        }
    }
}