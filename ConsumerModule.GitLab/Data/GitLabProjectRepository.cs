using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Data.Models;
using MongoDB.Driver;

namespace ConsumerModule.GitLab.Data
{
    public interface IGitLabProjectRepository : IMongoDB<GitLabProject>
    {
        Task<GitLabProject> GetProjectByProjectId(int projectId);
        Task UpdateProjectDirectory(Guid id, List<GitLabProjectDirectory> projectHistory);
    }
    
    public class GitLabProjectRepository : BaseMongoDB<GitLabProject>, IGitLabProjectRepository
    {
        public GitLabProjectRepository(IMongoClient mongoClient) : base(mongoClient)
        {
        }

        public async Task<GitLabProject> GetProjectByProjectId(int projectId)
        {
            return await Collection().Find(t => t.Project_id == projectId).SingleOrDefaultAsync();
        }

        public async Task UpdateProjectDirectory(Guid id, List<GitLabProjectDirectory> projectHistory)
        {
            await Collection()
                .FindOneAndUpdateAsync(t => t.Id == id, Update.Set(t => t.ProjectHistory, projectHistory));
        }
    }
}