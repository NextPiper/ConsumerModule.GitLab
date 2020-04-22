using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Data;
using ConsumerModule.GitLab.Data.Models;
using ConsumerModule.GitLab.Domain.Models;
using GitLabProject = ConsumerModule.GitLab.Domain.Models.GitLabProject;

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
        private readonly IGitLabProjectRepository _gitLabProjectRepository;

        public GitLabHandler(IGitLabDataRepository repository, IGitLabProjectRepository gitLabProjectRepository)
        {
            _repository = repository;
            _gitLabProjectRepository = gitLabProjectRepository;
        }
        
        public async Task<IEnumerable<GitLabProjectOverview>> GetProjects(int page, int pageSize)
        {
            var gitLabCommits = await _repository.GetPaged(page, pageSize);

            var projectIds = new HashSet<int>();
            
            foreach (var project in gitLabCommits)
            {
                projectIds.Add(project.Project_id);
            }

            var projectsLatestBaseScore = new Dictionary<int, double>();
            var projectsLatestAccumulatedScore = new Dictionary<int, double>();

            foreach (var projectId in projectIds)
            {
                var project = await _gitLabProjectRepository.GetProjectByProjectId(projectId);
                
                GitLabProjectDirectory latestProjectDirectory = null;
                // Get latest up to date projectDirectory
                foreach (var directory in project.ProjectHistory)
                {
                    if (latestProjectDirectory == null)
                    {
                        latestProjectDirectory = directory;
                        continue;
                    }

                    if (latestProjectDirectory.CreatedAt.CompareTo(directory.CreatedAt) < 0)
                    {
                        latestProjectDirectory = directory;
                    } 
                }
                
                var baseScoreSum = 0.0;
                var accumulatedScoreSum = 0.0;

                var baseScoreAverage = 0.0;
                var accumulatedScoreAverage = 0.0;

                if (latestProjectDirectory != null)
                {
                    foreach (var fileDataScore in latestProjectDirectory.FileDataScores)
                    {
                        baseScoreSum += fileDataScore.BaseScore;
                        accumulatedScoreSum += fileDataScore.AccumulatedCodeScore;
                    }

                    if (latestProjectDirectory.FileDataScores.Any())
                    {
                        baseScoreAverage = baseScoreSum / latestProjectDirectory.FileDataScores.Count(); 
                        accumulatedScoreAverage = accumulatedScoreSum / latestProjectDirectory.FileDataScores.Count();
                    }
                }

                projectsLatestBaseScore[projectId] = baseScoreAverage;
                projectsLatestAccumulatedScore[projectId] = accumulatedScoreAverage;
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
                        AverageProjectScore = projectsLatestBaseScore[project.Project_id],
                        AccumulatedAverageProjectScore = projectsLatestAccumulatedScore[project.Project_id]
                    });   
                }
            }
            return list;
        }

        public async Task<GitLabProject> GetProject(int projectId)
        {
            var commits = await _repository.GetProject(projectId);

            var baseScoreSum = 0.0;
            var baseProjectAverage = 0.0;
            var accumulatedScoreSum = 0.0;
            var accumulatedProjectAverage = 0.0;
            var projectName = "";
            var repositoryName = "";

            var projectCommits = new List<GitLabCommit>();
             
            // Fetch project history and calculate most relevant averageScore
            foreach (var commit in commits)
            {
                projectCommits.Add(new GitLabCommit
                {
                    Average_Commit_Score = commit.Average_Commit_Score,
                    Checkout_sha = commit.Checkout_sha,
                    FileDataScores = commit.FileDataScores,
                    Files = commit.Files,
                    Id = commit.Id,
                    Ref = commit.Ref,
                    User_id = commit.User_id,
                    user_email = commit.User_email,
                    CreatedAt = commit.CreatedAt
                });
            }

            if (commits.Any())
            {
                projectName = commits.FirstOrDefault().Project_name;
                repositoryName = commits.FirstOrDefault().RepositoryName;
            }

            var project = await _gitLabProjectRepository.GetProjectByProjectId(projectId);
            
            GitLabProjectDirectory latestProjectDirectory = null;
            // Get latest up to date projectDirectory
            foreach (var directory in project.ProjectHistory)
            {
                if (latestProjectDirectory == null)
                {
                    latestProjectDirectory = directory;
                    continue;
                }

                if (latestProjectDirectory.CreatedAt.CompareTo(directory.CreatedAt) < 0)
                {
                    latestProjectDirectory = directory;
                } 
            }

            if (latestProjectDirectory != null)
            {
                foreach (var fileDataScore in latestProjectDirectory.FileDataScores)
                {
                    baseScoreSum += fileDataScore.BaseScore;
                    accumulatedScoreSum += fileDataScore.AccumulatedCodeScore;
                }

                if (latestProjectDirectory.FileDataScores.Any())
                {
                    baseProjectAverage = baseScoreSum / latestProjectDirectory.FileDataScores.Count(); 
                    accumulatedProjectAverage = accumulatedScoreSum / latestProjectDirectory.FileDataScores.Count();
                }
            }
            
            return new GitLabProject
            {
                AverageProjectScore = baseProjectAverage,
                AccumulatedAverageProjectScore = accumulatedProjectAverage,
                Project_id = projectId,
                Project_name = projectName,
                RepositoryName = repositoryName,
                Commits = projectCommits,
                ProjectHistory = project.ProjectHistory
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