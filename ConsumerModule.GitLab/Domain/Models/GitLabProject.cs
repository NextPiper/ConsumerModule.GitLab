using System.Collections.Generic;

namespace ConsumerModule.GitLab.Domain.Models
{
    public class GitLabProject
    {
        public int Project_id { get; set; }
        public string Project_name { get; set; }
        public string RepositoryName { get; set; }
        public double AverageProjectScore { get; set; }
        
        public IEnumerable<GitLabCommit> Commits { get; set; }
    }
}