using System;
using System.Collections.Generic;

namespace ConsumerModule.GitLab.Data.Models
{
    public class GitLabProject : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; } = DateTime.Now;
        
        public int Project_id { get; set; }
        public string Project_name { get; set; }
        public List<GitLabProjectDirectory> ProjectHistory { get; set; }
    }
}