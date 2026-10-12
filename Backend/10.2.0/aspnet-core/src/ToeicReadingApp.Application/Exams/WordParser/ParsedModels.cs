using System;
using System.Collections.Generic;

namespace ToeicReadingApp.Exams.WordParser
{
    public class ParsedExam
    {
        public string Title { get; set; }
        public int DurationMinutes { get; set; } = 75;
        public int TotalQuestions { get; set; }
        public string Description { get; set; }
        public List<ParsedPart> Parts { get; set; } = new List<ParsedPart>();
    }

    public class ParsedPart
    {
        public int PartNumber { get; set; }
        public string Title { get; set; }
        public string Directions { get; set; }
        public List<ParsedGroup> Groups { get; set; } = new List<ParsedGroup>();
        public List<ParsedQuestion> StandaloneQuestions { get; set; } = new List<ParsedQuestion>();
    }

    public class ParsedGroup
    {
        public string GroupTitle { get; set; }
        public string PassageText { get; set; }
        public string ImageUrl { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageExtension { get; set; } = ".png";
        public int OrderIndex { get; set; }
        public List<ParsedQuestion> Questions { get; set; } = new List<ParsedQuestion>();
    }

    public class ParsedQuestion
    {
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class WordExamValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; set; } = new List<string>();
        public ParsedExam Exam { get; set; }
    }
}
