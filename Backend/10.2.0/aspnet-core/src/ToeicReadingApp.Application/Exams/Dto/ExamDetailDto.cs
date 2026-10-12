using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace ToeicReadingApp.Exams.Dto
{
    public class ExamDetailDto : EntityDto<int>
    {
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public int TotalQuestions { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreationTime { get; set; }
        public List<ExamPartDto> Parts { get; set; } = new List<ExamPartDto>();
    }
}
