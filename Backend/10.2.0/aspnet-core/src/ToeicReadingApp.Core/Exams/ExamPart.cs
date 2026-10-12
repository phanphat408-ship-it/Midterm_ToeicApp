using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ToeicReadingApp.Exams
{
    [Table("AppExamParts")]
    public class ExamPart : FullAuditedEntity<int>
    {
        public const int MaxTitleLength = 64;

        public int ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Exam Exam { get; set; }

        public int PartNumber { get; set; }

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; }

        public string Directions { get; set; }

        public virtual ICollection<QuestionGroup> QuestionGroups { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public ExamPart()
        {
            QuestionGroups = new HashSet<QuestionGroup>();
            Questions = new HashSet<Question>();
        }
    }
}
