namespace ConsumerModule.GitLab.Domain.Models
{
    public class GitLabProjectOverview
    {
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public double AverageProjectScore { get; set; }
    }
}