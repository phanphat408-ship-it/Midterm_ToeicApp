using System;
using System.Collections.Generic;

namespace ToeicReadingApp.Exams.Dto
{
    public class QuestionAnswerInputDto
    {
        public int QuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public string SelectedOption { get; set; } // "A", "B", "C", "D" or null
    }

    public class SubmitExamInputDto
    {
        public int ExamId { get; set; }
        public string StudentName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime SubmitTime { get; set; }
        public List<QuestionAnswerInputDto> Answers { get; set; } = new List<QuestionAnswerInputDto>();
    }

    public class QuestionResultDto
    {
        public int QuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public string SelectedOption { get; set; }
        public string CorrectOption { get; set; }
        public bool IsCorrect { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
    }

    public class ExamSubmitResultDto
    {
        public int AttemptId { get; set; }
        public int ExamId { get; set; }
        public string StudentName { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalCorrect { get; set; }
        public int TotalWrong { get; set; }
        public int TotalSkipped { get; set; }
        public int Score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime SubmitTime { get; set; }
        public List<QuestionResultDto> Details { get; set; } = new List<QuestionResultDto>();
    }

    public class ImportExamResultDto
    {
        public bool Success { get; set; }
        public int? ExamId { get; set; }
        public string ExamTitle { get; set; }
        public int TotalQuestions { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
