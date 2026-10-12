using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ToeicReadingApp.Exams
{
    [Table("AppExams")]
    public class Exam : FullAuditedEntity<int>
    {
        public const int MaxTitleLength = 256;

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; }

        public int DurationMinutes { get; set; } = 75;

        public int TotalQuestions { get; set; } = 100;

        public string Description { get; set; }

        public bool IsPublished { get; set; } = true;

        public virtual ICollection<ExamPart> Parts { get; set; }

        public Exam()
        {
            Parts = new HashSet<ExamPart>();
            DurationMinutes = 75;
            TotalQuestions = 100;
            IsPublished = true;
        }
    }
}
