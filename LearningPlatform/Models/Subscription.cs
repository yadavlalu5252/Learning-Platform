using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class Subscription
    {
        [Key]
        public int sid { get; set; }

        public string stype { get; set; } = null!;

        public int MasterCourseId { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourseData { get; set; } = null!;

        public int SubCourseId { get; set; }

        [ForeignKey("SubCourseId")]
        public SubCourse SubCourseData { get; set; } = null!;

        public int amount { get; set; }
        public int Validity { get; set; }

        public string status { get; set; } = null!;

        public string Thumbnail { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

    }
}