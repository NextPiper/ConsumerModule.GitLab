using System.Collections.Generic;

namespace ConsumerModule.GitLab.Data.Models
{
    public class GitLabFileDataScore
    {
        public string FileName { get; set; }
        public string Ref { get; set; }
        public double BaseScore { get; set; }
        public double AccumulatedCodeScore { get; set; }
        public Dictionary<string, double> DetailedScoreDict { get; set; }
    }
}