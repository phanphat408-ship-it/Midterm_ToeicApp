using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ToeicReadingApp.Exams
{
    [Table("AppQuestions")]
    public class Question : FullAuditedEntity<int>
    {
        public const int MaxAnswerLength = 10;

        public int ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Exam Exam { get; set; }

        public int ExamPartId { get; set; }

        [ForeignKey(nameof(ExamPartId))]
        public virtual ExamPart ExamPart { get; set; }

        public int? QuestionGroupId { get; set; }

        [ForeignKey(nameof(QuestionGroupId))]
        public virtual QuestionGroup QuestionGroup { get; set; }

        public int QuestionNumber { get; set; }

        public string QuestionText { get; set; }

        [Required]
        public string OptionA { get; set; }

        [Required]
        public string OptionB { get; set; }

        [Required]
        public string OptionC { get; set; }

        [Required]
        public string OptionD { get; set; }

        [Required]
        [MaxLength(MaxAnswerLength)]
        public string CorrectAnswer { get; set; }
    }
}
