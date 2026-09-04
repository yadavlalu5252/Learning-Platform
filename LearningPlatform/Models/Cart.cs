using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class Cart
    {
         [Key]
        public int CartId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserData { get; set; } = null!;

        public int? MasterCourseId { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse? MasterCourseData { get; set; }

        public int? SubCourseId { get; set; }

        [ForeignKey("SubCourseId")]
        public SubCourse? SubCourseData { get; set; }

        public int? SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public Subscription? SubscriptionData { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
