using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ToeicReadingApp.Exams
{
    [Table("AppQuestionGroups")]
    public class QuestionGroup : FullAuditedEntity<int>
    {
        public const int MaxGroupTitleLength = 512;
        public const int MaxImageUrlLength = 1000;

        public int ExamPartId { get; set; }

        [ForeignKey(nameof(ExamPartId))]
        public virtual ExamPart ExamPart { get; set; }

        [MaxLength(MaxGroupTitleLength)]
        public string GroupTitle { get; set; }

        public string PassageText { get; set; }

        [MaxLength(MaxImageUrlLength)]
        public string ImageUrl { get; set; }

        public int OrderIndex { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public QuestionGroup()
        {
            Questions = new HashSet<Question>();
        }
    }
}
