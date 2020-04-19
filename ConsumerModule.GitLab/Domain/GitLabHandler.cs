using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Data;
using ConsumerModule.GitLab.Domain.Models;

namespace ConsumerModule.GitLab.Domain
{
    public interface IGitLabHandler
    {
        Task<IEnumerable<GitLabProjectOverview>> GetProjects(int page, int pageSize);
        Task<GitLabProject> GetProject(int projectId);
        Task<GitLabCommit> GetProjectCommit(int projectId, Guid commitId);
    }
    
    public class GitLabHandler : IGitLabHandler
    {
        private readonly IGitLabDataRepository _repository;

        public GitLabHandler(IGitLabDataRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IEnumerable<GitLabProjectOverview>> GetProjects(int page, int pageSize)
        {
            var gitLabCommits = await _repository.GetPaged(page, pageSize);

            var sumDic = new Dictionary<int, double>();
            var countDic = new Dictionary<int, int>();

            // Sum the average score of different projects
            foreach (var project in gitLabCommits)
            {
                if (sumDic.Keys.Any(t => t == project.Project_id))
                {
                    // Put
                    sumDic[project.Project_id] += project.Average_Commit_Score;
                    countDic[project.Project_id] += 1;
                }
                else
                {
                    // Insert new
                    sumDic[project.Project_id] = project.Average_Commit_Score;
                    countDic[project.Project_id] = 1;
                }
            }

            var list = new List<GitLabProjectOverview>();
            // Sum the average score of different projects
            foreach (var project in gitLabCommits)
            {
                if (list.All(t => t.ProjectId != project.Project_id))
                {
                    list.Add(new GitLabProjectOverview
                    {
                        Name = project.Project_name,
                        ProjectId = project.Project_id,
                        AverageProjectScore = sumDic[project.Project_id] / countDic[project.Project_id]
                    });   
                }
            }
            return list;
        }

        public async Task<GitLabProject> GetProject(int projectId)
        {
            var commits = await _repository.GetProject(projectId);

            var scoreSum = 0.0;
            var projectAverage = 0.0;
            var projectName = "";
            var repositoryName = "";
            

            var projectCommits = new List<GitLabCommit>();  
               
            foreach (var commit in commits)
            {
                scoreSum += commit.Average_Commit_Score;
                projectCommits.Add(new GitLabCommit
                {
                    Average_Commit_Score = commit.Average_Commit_Score,
                    Checkout_sha = commit.Checkout_sha,
                    FileDataScores = commit.FileDataScores,
                    Files = commit.Files,
                    Id = commit.Id,
                    Ref = commit.Ref,
                    User_id = commit.User_id,
                    user_email = commit.User_email
                });
            }

            if (commits.Any())
            {
                projectAverage = scoreSum / commits.Count();
                projectName = commits.FirstOrDefault().Project_name;
                repositoryName = commits.FirstOrDefault().RepositoryName;
            }
            
            return new GitLabProject
            {
                AverageProjectScore = projectAverage,
                Project_id = projectId,
                Project_name = projectName,
                RepositoryName = repositoryName,
                Commits = projectCommits
            };
        }

        public async Task<GitLabCommit> GetProjectCommit(int projectId, Guid commitId)
        {
            var commit = await _repository.GetCommit(projectId, commitId);
            
            return new GitLabCommit
            {
                Average_Commit_Score = commit.Average_Commit_Score,
                Checkout_sha = commit.Checkout_sha,
                FileDataScores = commit.FileDataScores,
                Files = commit.Files,
                User_id = commit.User_id,
                Id = commit.Id,
                Ref = commit.Ref,
                user_email = commit.User_email
            };
        }
    }
}