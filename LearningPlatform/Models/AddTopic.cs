using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Models
{
    public class AddTopic
    {
        [Key]
        public int Id { get; set; }
        public  string TopicName { get; set; }
        public string VideoUrl { get; set; }
        public string Status { get; set; }
        public string Thumbnail { get; set; }

    }
}
