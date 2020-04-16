using System;

namespace ConsumerModule.GitLab.Data
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime CreatedAt { get; set; }
        DateTime EditedAt { get; set; }
    }
}