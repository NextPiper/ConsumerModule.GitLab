using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsumerModule.GitLab.Data.Models
{
    public class GitLabData : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; } = DateTime.Now;
        
        public int Project_id { get; set; }
        public string Project_name { get; set; }
        public int User_id { get; set; }
        public string RepositoryName { get; set; }
        public string @Ref { get; set; }
        public string Checkout_sha { get; set; }
        public double Average_Commit_Score { get; set; }
        public IEnumerable<string> Files { get; set; } = new List<string>();
        public IEnumerable<GitLabFileDataScore> FileDataScores { get; set; } = new List<GitLabFileDataScore>();
        
        
    }
}