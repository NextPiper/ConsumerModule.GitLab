using System;
using System.Collections.Generic;
using ConsumerModule.GitLab.Data.Models;

namespace ConsumerModule.GitLab.Domain.Models
{
    public class GitLabCommit
    {
        public Guid Id { get; set; }
        public int User_id { get; set; }
        public string user_email { get; set; }
        public string @Ref { get; set; }
        public string Checkout_sha { get; set; }
        public double Average_Commit_Score { get; set; }
        public IEnumerable<string> Files { get; set; } = new List<string>();
        public IEnumerable<GitLabFileDataScore> FileDataScores { get; set; } = new List<GitLabFileDataScore>();
    }
}