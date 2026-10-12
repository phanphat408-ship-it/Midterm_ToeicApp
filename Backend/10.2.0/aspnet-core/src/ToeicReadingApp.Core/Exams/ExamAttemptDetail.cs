using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ToeicReadingApp.Exams
{
    [Table("AppExamAttemptDetails")]
    public class ExamAttemptDetail : Entity<int>
    {
        public const int MaxOptionLength = 10;

        public int ExamAttemptId { get; set; }

        [ForeignKey(nameof(ExamAttemptId))]
        public virtual ExamAttempt ExamAttempt { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }

        public int QuestionNumber { get; set; }

        [MaxLength(MaxOptionLength)]
        public string SelectedOption { get; set; }

        [Required]
        [MaxLength(MaxOptionLength)]
        public string CorrectOption { get; set; }

        public bool IsCorrect { get; set; }
    }
}
