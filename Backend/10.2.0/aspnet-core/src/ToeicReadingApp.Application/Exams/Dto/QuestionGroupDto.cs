using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace ToeicReadingApp.Exams.Dto
{
    public class QuestionGroupDto : EntityDto<int>
    {
        public int ExamPartId { get; set; }
        public string GroupTitle { get; set; }
        public string PassageText { get; set; }
        public string ImageUrl { get; set; }
        public int OrderIndex { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
