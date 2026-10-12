using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ToeicReadingApp.Exams.Dto
{
    public class UpdateExamDto : EntityDto<int>
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        public int DurationMinutes { get; set; }

        public int TotalQuestions { get; set; }

        public string Description { get; set; }

        public bool IsPublished { get; set; }
    }

    public class UpdateQuestionGroupDto : EntityDto<int>
    {
        [MaxLength(512)]
        public string GroupTitle { get; set; }

        public string PassageText { get; set; }

        public string ImageUrl { get; set; }

        public int OrderIndex { get; set; }
    }

    public class UpdateQuestionDto : EntityDto<int>
    {
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
        [MaxLength(10)]
        public string CorrectAnswer { get; set; }
    }
}
