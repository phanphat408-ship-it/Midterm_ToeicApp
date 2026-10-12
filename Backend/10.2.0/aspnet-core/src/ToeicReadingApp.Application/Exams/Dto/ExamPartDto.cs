using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace ToeicReadingApp.Exams.Dto
{
    public class ExamPartDto : EntityDto<int>
    {
        public int ExamId { get; set; }
        public int PartNumber { get; set; }
        public string Title { get; set; }
        public string Directions { get; set; }
        public List<QuestionGroupDto> QuestionGroups { get; set; } = new List<QuestionGroupDto>();
        public List<QuestionDto> StandaloneQuestions { get; set; } = new List<QuestionDto>();
        public List<QuestionDto> AllQuestions { get; set; } = new List<QuestionDto>();
    }
}
