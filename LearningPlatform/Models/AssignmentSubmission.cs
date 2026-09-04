using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class AssignmentSubmission
    {
        [Key]
        public int SubmissionId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserData { get; set; } = null!;

        public int MaterialId { get; set; }

        [ForeignKey("MaterialId")]
        public AddMaterial MaterialData { get; set; } = null!;

        public string SolutionFile { get; set; } = null!;

        public DateTime SubmittedAt { get; set; }

        public string Status { get; set; } = null!;
    }
}