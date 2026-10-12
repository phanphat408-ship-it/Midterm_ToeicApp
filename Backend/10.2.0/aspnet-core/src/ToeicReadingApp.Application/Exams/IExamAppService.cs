using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Abp.Application.Services;
using ToeicReadingApp.Exams.Dto;

namespace ToeicReadingApp.Exams
{
    public interface IExamAppService : IApplicationService
    {
        Task<List<ExamListDto>> GetAllExamsAsync();
        Task<ExamDetailDto> GetExamDetailAsync(int id);
        Task<UpdateExamDto> UpdateExamAsync(UpdateExamDto input);
        Task UpdateQuestionGroupAsync(UpdateQuestionGroupDto input);
        Task UpdateQuestionAsync(UpdateQuestionDto input);
        Task DeleteExamAsync(int id);
        Task<ExamSubmitResultDto> SubmitExamAsync(SubmitExamInputDto input);
        Task<ImportExamResultDto> ImportWordExamAsync(Stream docxStream, string uploadPhysicalFolder);
        Task<int> CreateSampleTestAsync();
    }
}
