using System.ComponentModel.DataAnnotations;

namespace ToeicReadingApp.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}