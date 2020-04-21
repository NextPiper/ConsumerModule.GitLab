using System;
using System.Collections.Generic;

namespace ConsumerModule.GitLab.Data.Models
{
    public class GitLabProjectDirectory
    {
        public string Commit_Sha { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public IEnumerable<string> Files { get; set; } = new List<string>();
        public IEnumerable<GitLabFileDataScore> FileDataScores { get; set; } = new List<GitLabFileDataScore>();   
    }
}