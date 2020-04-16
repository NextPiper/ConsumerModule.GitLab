using System;

namespace ConsumerModule.GitLab.Data.Models
{
    public class GitLabData : IEntity
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; } = DateTime.Now;
    }
}