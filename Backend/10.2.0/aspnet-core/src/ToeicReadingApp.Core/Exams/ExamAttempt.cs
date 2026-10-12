using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ToeicReadingApp.Exams
{
    [Table("AppExamAttempts")]
    public class ExamAttempt : FullAuditedEntity<int>
    {
        public const int MaxStudentNameLength = 256;

        public int ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Exam Exam { get; set; }

        [MaxLength(MaxStudentNameLength)]
        public string StudentName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime SubmitTime { get; set; }

        public int TotalCorrect { get; set; }

        public int TotalWrong { get; set; }

        public int TotalSkipped { get; set; }

        public int Score { get; set; }

        public virtual ICollection<ExamAttemptDetail> Details { get; set; }

        public ExamAttempt()
        {
            Details = new HashSet<ExamAttemptDetail>();
        }
    }
}
