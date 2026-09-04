using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserData { get; set; } = null!;

        public int MasterCourseId { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourseData { get; set; } = null!;

        public string CertificateNumber { get; set; } = null!;

        public DateTime IssuedAt { get; set; }

        public string CertificateFile { get; set; } = null!;
    }
}