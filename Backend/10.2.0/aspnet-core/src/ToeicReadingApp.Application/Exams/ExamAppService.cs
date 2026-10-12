using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using ToeicReadingApp.Exams.Dto;
using ToeicReadingApp.Exams.WordParser;

namespace ToeicReadingApp.Exams
{
    public class ExamAppService : ToeicReadingAppAppServiceBase, IExamAppService
    {
        private readonly IRepository<Exam, int> _examRepository;
        private readonly IRepository<ExamPart, int> _examPartRepository;
        private readonly IRepository<QuestionGroup, int> _questionGroupRepository;
        private readonly IRepository<Question, int> _questionRepository;
        private readonly IRepository<ExamAttempt, int> _examAttemptRepository;
        private readonly IRepository<ExamAttemptDetail, int> _examAttemptDetailRepository;

        public ExamAppService(
            IRepository<Exam, int> examRepository,
            IRepository<ExamPart, int> examPartRepository,
            IRepository<QuestionGroup, int> questionGroupRepository,
            IRepository<Question, int> questionRepository,
            IRepository<ExamAttempt, int> examAttemptRepository,
            IRepository<ExamAttemptDetail, int> examAttemptDetailRepository)
        {
            _examRepository = examRepository;
            _examPartRepository = examPartRepository;
            _questionGroupRepository = questionGroupRepository;
            _questionRepository = questionRepository;
            _examAttemptRepository = examAttemptRepository;
            _examAttemptDetailRepository = examAttemptDetailRepository;
        }

        [AbpAllowAnonymous]
        public async Task<List<ExamListDto>> GetAllExamsAsync()
        {
            var exams = await _examRepository.GetAll()
                .Include(e => e.Parts)
                .OrderByDescending(e => e.CreationTime)
                .ToListAsync();

            return exams.Select(e => new ExamListDto
            {
                Id = e.Id,
                Title = e.Title,
                DurationMinutes = e.DurationMinutes,
                TotalQuestions = e.TotalQuestions,
                Description = e.Description,
                IsPublished = e.IsPublished,
                CreationTime = e.CreationTime,
                PartCount = e.Parts?.Count ?? 0
            }).ToList();
        }

        [AbpAllowAnonymous]
        public async Task<ExamDetailDto> GetExamDetailAsync(int id)
        {
            var exam = await _examRepository.GetAll()
                .Include(e => e.Parts)
                    .ThenInclude(p => p.QuestionGroups)
                        .ThenInclude(g => g.Questions)
                .Include(e => e.Parts)
                    .ThenInclude(p => p.Questions)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null)
            {
                throw new UserFriendlyException($"Không tìm thấy đề thi với Id = {id}");
            }

            var dto = new ExamDetailDto
            {
                Id = exam.Id,
                Title = exam.Title,
                DurationMinutes = exam.DurationMinutes,
                TotalQuestions = exam.TotalQuestions,
                Description = exam.Description,
                IsPublished = exam.IsPublished,
                CreationTime = exam.CreationTime,
                Parts = new List<ExamPartDto>()
            };

            foreach (var part in exam.Parts.OrderBy(p => p.PartNumber))
            {
                var partDto = new ExamPartDto
                {
                    Id = part.Id,
                    ExamId = part.ExamId,
                    PartNumber = part.PartNumber,
                    Title = part.Title,
                    Directions = part.Directions,
                    QuestionGroups = new List<QuestionGroupDto>(),
                    StandaloneQuestions = new List<QuestionDto>(),
                    AllQuestions = new List<QuestionDto>()
                };

                // Standalone questions (e.g. Part 5)
                var standalone = part.Questions
                    .Where(q => q.QuestionGroupId == null)
                    .OrderBy(q => q.QuestionNumber)
                    .Select(MapToQuestionDto)
                    .ToList();
                partDto.StandaloneQuestions.AddRange(standalone);
                partDto.AllQuestions.AddRange(standalone);

                // Grouped questions (Part 6, Part 7)
                foreach (var group in part.QuestionGroups.OrderBy(g => g.OrderIndex))
                {
                    var groupDto = new QuestionGroupDto
                    {
                        Id = group.Id,
                        ExamPartId = group.ExamPartId,
                        GroupTitle = group.GroupTitle,
                        PassageText = group.PassageText,
                        ImageUrl = group.ImageUrl,
                        OrderIndex = group.OrderIndex,
                        Questions = group.Questions
                            .OrderBy(q => q.QuestionNumber)
                            .Select(MapToQuestionDto)
                            .ToList()
                    };

                    partDto.QuestionGroups.Add(groupDto);
                    partDto.AllQuestions.AddRange(groupDto.Questions);
                }

                partDto.AllQuestions = partDto.AllQuestions.OrderBy(q => q.QuestionNumber).ToList();
                dto.Parts.Add(partDto);
            }

            return dto;
        }

        public async Task<UpdateExamDto> UpdateExamAsync(UpdateExamDto input)
        {
            var exam = await _examRepository.GetAsync(input.Id);
            exam.Title = input.Title;
            exam.DurationMinutes = input.DurationMinutes;
            exam.TotalQuestions = input.TotalQuestions;
            exam.Description = input.Description;
            exam.IsPublished = input.IsPublished;

            await _examRepository.UpdateAsync(exam);
            return input;
        }

        public async Task UpdateQuestionGroupAsync(UpdateQuestionGroupDto input)
        {
            var group = await _questionGroupRepository.GetAsync(input.Id);
            group.GroupTitle = input.GroupTitle;
            group.PassageText = input.PassageText;
            if (!string.IsNullOrEmpty(input.ImageUrl))
            {
                group.ImageUrl = input.ImageUrl;
            }
            group.OrderIndex = input.OrderIndex;

            await _questionGroupRepository.UpdateAsync(group);
        }

        public async Task UpdateQuestionAsync(UpdateQuestionDto input)
        {
            var q = await _questionRepository.GetAsync(input.Id);
            q.QuestionNumber = input.QuestionNumber;
            q.QuestionText = input.QuestionText;
            q.OptionA = input.OptionA;
            q.OptionB = input.OptionB;
            q.OptionC = input.OptionC;
            q.OptionD = input.OptionD;
            q.CorrectAnswer = input.CorrectAnswer.Trim().ToUpperInvariant();

            await _questionRepository.UpdateAsync(q);
        }

        public async Task DeleteExamAsync(int id)
        {
            // Delete related attempts first
            var attempts = await _examAttemptRepository.GetAllListAsync(a => a.ExamId == id);
            foreach (var attempt in attempts)
            {
                await _examAttemptRepository.DeleteAsync(attempt);
            }

            // Delete questions
            var questions = await _questionRepository.GetAllListAsync(q => q.ExamId == id);
            foreach (var q in questions)
            {
                await _questionRepository.DeleteAsync(q);
            }

            // Delete parts & groups
            var parts = await _examPartRepository.GetAllIncluding(p => p.QuestionGroups)
                .Where(p => p.ExamId == id)
                .ToListAsync();

            foreach (var part in parts)
            {
                if (part.QuestionGroups != null)
                {
                    foreach (var grp in part.QuestionGroups)
                    {
                        await _questionGroupRepository.DeleteAsync(grp);
                    }
                }
                await _examPartRepository.DeleteAsync(part);
            }

            await _examRepository.DeleteAsync(id);
        }

        [AbpAllowAnonymous]
        public async Task<ExamSubmitResultDto> SubmitExamAsync(SubmitExamInputDto input)
        {
            var exam = await _examRepository.FirstOrDefaultAsync(input.ExamId);
            if (exam == null)
            {
                throw new UserFriendlyException($"Không tìm thấy đề thi với Id = {input.ExamId}");
            }

            var questions = await _questionRepository.GetAllListAsync(q => q.ExamId == input.ExamId);
            if (questions.Count == 0)
            {
                throw new UserFriendlyException("Đề thi này chưa có câu hỏi nào.");
            }

            var answerDict = input.Answers?
                .GroupBy(a => a.QuestionId)
                .ToDictionary(g => g.Key, g => g.First())
                ?? new Dictionary<int, QuestionAnswerInputDto>();

            int totalCorrect = 0;
            int totalWrong = 0;
            int totalSkipped = 0;

            var details = new List<ExamAttemptDetail>();
            var resultDetails = new List<QuestionResultDto>();

            foreach (var q in questions.OrderBy(q => q.QuestionNumber))
            {
                string selectedOption = null;
                bool isCorrect = false;

                if (answerDict.TryGetValue(q.Id, out var userAns) && !string.IsNullOrWhiteSpace(userAns.SelectedOption))
                {
                    selectedOption = userAns.SelectedOption.Trim().ToUpperInvariant();
                    isCorrect = string.Equals(selectedOption, q.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);
                    if (isCorrect)
                    {
                        totalCorrect++;
                    }
                    else
                    {
                        totalWrong++;
                    }
                }
                else
                {
                    totalSkipped++;
                }

                var detail = new ExamAttemptDetail
                {
                    QuestionId = q.Id,
                    QuestionNumber = q.QuestionNumber,
                    SelectedOption = selectedOption,
                    CorrectOption = q.CorrectAnswer,
                    IsCorrect = isCorrect
                };
                details.Add(detail);

                resultDetails.Add(new QuestionResultDto
                {
                    QuestionId = q.Id,
                    QuestionNumber = q.QuestionNumber,
                    SelectedOption = selectedOption,
                    CorrectOption = q.CorrectAnswer,
                    IsCorrect = isCorrect,
                    QuestionText = q.QuestionText,
                    OptionA = q.OptionA,
                    OptionB = q.OptionB,
                    OptionC = q.OptionC,
                    OptionD = q.OptionD
                });
            }

            int score = CalculateToeicReadingScore(totalCorrect, questions.Count);

            var attempt = new ExamAttempt
            {
                ExamId = input.ExamId,
                StudentName = string.IsNullOrWhiteSpace(input.StudentName) ? "Thí sinh" : input.StudentName.Trim(),
                StartTime = input.StartTime == default ? DateTime.Now.AddMinutes(-exam.DurationMinutes) : input.StartTime,
                SubmitTime = input.SubmitTime == default ? DateTime.Now : input.SubmitTime,
                TotalCorrect = totalCorrect,
                TotalWrong = totalWrong,
                TotalSkipped = totalSkipped,
                Score = score,
                Details = details
            };

            await _examAttemptRepository.InsertAsync(attempt);
            await CurrentUnitOfWork.SaveChangesAsync();

            return new ExamSubmitResultDto
            {
                AttemptId = attempt.Id,
                ExamId = exam.Id,
                StudentName = attempt.StudentName,
                TotalQuestions = questions.Count,
                TotalCorrect = totalCorrect,
                TotalWrong = totalWrong,
                TotalSkipped = totalSkipped,
                Score = score,
                StartTime = attempt.StartTime,
                SubmitTime = attempt.SubmitTime,
                Details = resultDetails
            };
        }

        public async Task<ImportExamResultDto> ImportWordExamAsync(Stream docxStream, string uploadPhysicalFolder)
        {
            var parser = new WordExamParser();
            var parseResult = parser.ParseAndValidate(docxStream);

            if (!parseResult.IsValid)
            {
                return new ImportExamResultDto
                {
                    Success = false,
                    Errors = parseResult.Errors
                };
            }

            var parsedExam = parseResult.Exam;

            // Ensure upload directory exists if needed
            if (!string.IsNullOrEmpty(uploadPhysicalFolder) && !Directory.Exists(uploadPhysicalFolder))
            {
                Directory.CreateDirectory(uploadPhysicalFolder);
            }

            var exam = new Exam
            {
                Title = parsedExam.Title,
                DurationMinutes = parsedExam.DurationMinutes,
                TotalQuestions = parsedExam.TotalQuestions,
                Description = $"Được import từ file Word lúc {DateTime.Now:dd/MM/yyyy HH:mm}",
                IsPublished = true
            };

            var examId = await _examRepository.InsertAndGetIdAsync(exam);

            foreach (var p in parsedExam.Parts)
            {
                var part = new ExamPart
                {
                    ExamId = examId,
                    PartNumber = p.PartNumber,
                    Title = p.Title,
                    Directions = p.Directions
                };

                var partId = await _examPartRepository.InsertAndGetIdAsync(part);

                // Standalone questions (Part 5)
                foreach (var q in p.StandaloneQuestions)
                {
                    var question = new Question
                    {
                        ExamId = examId,
                        ExamPartId = partId,
                        QuestionGroupId = null,
                        QuestionNumber = q.QuestionNumber,
                        QuestionText = q.QuestionText,
                        OptionA = q.OptionA,
                        OptionB = q.OptionB,
                        OptionC = q.OptionC,
                        OptionD = q.OptionD,
                        CorrectAnswer = q.CorrectAnswer
                    };
                    await _questionRepository.InsertAsync(question);
                }

                // Question groups (Part 6 & Part 7)
                foreach (var grp in p.Groups)
                {
                    string imageUrl = grp.ImageUrl;

                    // If image was extracted from Word document, save it to disk
                    if (grp.ImageData != null && !string.IsNullOrEmpty(uploadPhysicalFolder))
                    {
                        var fileName = $"{Guid.NewGuid():N}{grp.ImageExtension ?? ".png"}";
                        var fullPath = Path.Combine(uploadPhysicalFolder, fileName);
                        await File.WriteAllBytesAsync(fullPath, grp.ImageData);
                        imageUrl = $"/uploads/exams/{fileName}";
                    }

                    var group = new QuestionGroup
                    {
                        ExamPartId = partId,
                        GroupTitle = grp.GroupTitle,
                        PassageText = grp.PassageText,
                        ImageUrl = imageUrl,
                        OrderIndex = grp.OrderIndex
                    };

                    var groupId = await _questionGroupRepository.InsertAndGetIdAsync(group);

                    foreach (var q in grp.Questions)
                    {
                        var question = new Question
                        {
                            ExamId = examId,
                            ExamPartId = partId,
                            QuestionGroupId = groupId,
                            QuestionNumber = q.QuestionNumber,
                            QuestionText = q.QuestionText,
                            OptionA = q.OptionA,
                            OptionB = q.OptionB,
                            OptionC = q.OptionC,
                            OptionD = q.OptionD,
                            CorrectAnswer = q.CorrectAnswer
                        };
                        await _questionRepository.InsertAsync(question);
                    }
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new ImportExamResultDto
            {
                Success = true,
                ExamId = examId,
                ExamTitle = exam.Title,
                TotalQuestions = parsedExam.TotalQuestions
            };
        }

        public async Task<int> CreateSampleTestAsync()
        {
            var docxBytes = WordExamSampleGenerator.GenerateValidSampleDocx();
            using (var ms = new MemoryStream(docxBytes))
            {
                var result = await ImportWordExamAsync(ms, null);
                if (!result.Success || !result.ExamId.HasValue)
                {
                    throw new UserFriendlyException("Không thể tạo đề thi mẫu: " + string.Join("; ", result.Errors));
                }
                return result.ExamId.Value;
            }
        }

        private static QuestionDto MapToQuestionDto(Question q)
        {
            return new QuestionDto
            {
                Id = q.Id,
                ExamId = q.ExamId,
                ExamPartId = q.ExamPartId,
                QuestionGroupId = q.QuestionGroupId,
                QuestionNumber = q.QuestionNumber,
                QuestionText = q.QuestionText,
                OptionA = q.OptionA,
                OptionB = q.OptionB,
                OptionC = q.OptionC,
                OptionD = q.OptionD,
                CorrectAnswer = q.CorrectAnswer
            };
        }

        private static int CalculateToeicReadingScore(int correct, int total)
        {
            if (total == 0 || correct == 0) return 5;
            double pct = (double)correct / total;
            int score = (int)Math.Round(pct * 490.0 / 5.0) * 5 + 5;
            return Math.Clamp(score, 5, 495);
        }
    }
}
