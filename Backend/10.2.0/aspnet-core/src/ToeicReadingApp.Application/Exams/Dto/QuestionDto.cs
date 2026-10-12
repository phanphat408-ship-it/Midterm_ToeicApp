using Abp.Application.Services.Dto;

namespace ToeicReadingApp.Exams.Dto
{
    public class QuestionDto : EntityDto<int>
    {
        public int ExamId { get; set; }
        public int ExamPartId { get; set; }
        public int? QuestionGroupId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
