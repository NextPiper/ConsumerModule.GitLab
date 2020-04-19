using System.Collections.Generic;

namespace ConsumerModule.GitLab.RabbitListener.RabbitModels
{
    public class GitLabFile
    {
        public string file_name { get; set; }
        public string file_path { get; set; }
        public int size { get; set; }
        public string encoding { get; set; }
        public string content_sha256 { get; set; }
        public string @ref { get; set; }
        public string blob_id { get; set; }
        public string commit_id { get; set; }
        public string last_commit_id { get; set; }
        public string content { get; set; }
        public double AccumulatedCodeScore { get; set; }
        public double BaseScore { get; set; }
        public Dictionary<string, double> DetailedScoreDict { get; set; }
    }
}