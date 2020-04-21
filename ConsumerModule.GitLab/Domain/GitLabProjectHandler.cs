using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerModule.GitLab.Data;
using ConsumerModule.GitLab.Data.Models;

namespace ConsumerModule.GitLab.Domain
{
    public interface IGitLabProjectHandler
    {
        Task UpdateProject(int projectId, string projectName, string commitSha, IEnumerable<string> files, IEnumerable<GitLabFileDataScore> dataScores);
    }
    
    public class GitLabProjectHandler : IGitLabProjectHandler
    {
        private readonly IGitLabProjectRepository _gitLabProjectRepository;

        public GitLabProjectHandler(IGitLabProjectRepository gitLabProjectRepository)
        {
            _gitLabProjectRepository = gitLabProjectRepository;
        }

        public async Task UpdateProject(int projectId, string projectName, string commitSha, IEnumerable<string> files, IEnumerable<GitLabFileDataScore> dataScores)
        {
            // Check if project already exist witth projectId
            var project = await _gitLabProjectRepository.GetProjectByProjectId(projectId);

            if (project == null)
            {
                // Create a new project with the respective information
                await _gitLabProjectRepository.Insert(new GitLabProject
                {
                    Project_id = projectId,
                    Project_name = projectName,
                    ProjectHistory = new List<GitLabProjectDirectory>
                    {
                        new GitLabProjectDirectory
                        {
                            Commit_Sha = commitSha,
                            FileDataScores = dataScores,
                            Files = files
                        }
                    }
                });
            }
            else
            {
                // Create new GitLabProjectDirectory
                var projectDirectory = new GitLabProjectDirectory();
                projectDirectory.Commit_Sha = commitSha;

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
                
                // Update the file list with modified and addedFiles
                var copyFiles = new List<string>(latestProjectDirectory.Files);
                foreach (var file in files)
                {
                    if (!copyFiles.Contains(file))
                    {
                        copyFiles.Add(file);
                    }
                }

                var copyFileDataScore = new List<GitLabFileDataScore>(latestProjectDirectory.FileDataScores);
                foreach (var dataScore in dataScores)
                {
                    if (copyFileDataScore.Any(t => t.FileName == dataScore.FileName))
                    {
                        // Update content
                        var update = copyFileDataScore.SingleOrDefault(t => t.FileName == dataScore.FileName);
                        update.Ref = dataScore.Ref;
                        update.FileName = dataScore.FileName;
                        update.BaseScore = dataScore.BaseScore;
                        update.AccumulatedCodeScore = dataScore.AccumulatedCodeScore;
                        update.DetailedScoreDict = dataScore.DetailedScoreDict;
                    }
                    else
                    {
                        copyFileDataScore.Add(dataScore);
                    }
                }

                projectDirectory.Files = copyFiles;
                projectDirectory.FileDataScores = copyFileDataScore;

                project.ProjectHistory.Add(projectDirectory);

                await _gitLabProjectRepository.UpdateProjectDirectory(project.Id, project.ProjectHistory);
            }
        }
    }
}