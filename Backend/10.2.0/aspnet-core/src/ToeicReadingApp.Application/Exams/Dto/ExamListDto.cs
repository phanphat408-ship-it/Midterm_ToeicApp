using System;
using Abp.Application.Services.Dto;

namespace ToeicReadingApp.Exams.Dto
{
    public class ExamListDto : EntityDto<int>
    {
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public int TotalQuestions { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreationTime { get; set; }
        public int PartCount { get; set; }
    }
}
