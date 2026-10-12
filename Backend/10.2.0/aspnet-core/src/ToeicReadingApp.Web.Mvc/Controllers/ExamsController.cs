using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToeicReadingApp.Controllers;
using ToeicReadingApp.Exams;
using ToeicReadingApp.Exams.Dto;
using ToeicReadingApp.Exams.WordParser;

namespace ToeicReadingApp.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ExamsController : ToeicReadingAppControllerBase
    {
        private readonly IExamAppService _examAppService;
        private readonly IWebHostEnvironment _env;

        public ExamsController(IExamAppService examAppService, IWebHostEnvironment env)
        {
            _examAppService = examAppService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var exams = await _examAppService.GetAllExamsAsync();
            return View(exams);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Errors"] = new List<string> { "Vui lòng chọn một file Word (.docx) để tải lên." };
                return RedirectToAction(nameof(Index));
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".docx")
            {
                TempData["Errors"] = new List<string> { "Định dạng file không hợp lệ. Chỉ chấp nhận file Word (.docx)." };
                return RedirectToAction(nameof(Index));
            }

            var uploadFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "exams");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            using (var stream = file.OpenReadStream())
            {
                var result = await _examAppService.ImportWordExamAsync(stream, uploadFolder);
                if (!result.Success)
                {
                    TempData["Errors"] = result.Errors;
                }
                else
                {
                    TempData["Success"] = $"Import thành công đề thi '{result.ExamTitle}' với tổng số {result.TotalQuestions} câu hỏi!";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DownloadSample()
        {
            var bytes = WordExamSampleGenerator.GenerateValidSampleDocx();
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DeThi_TOEIC_Reading_Test05_Tagged.docx");
        }

        [HttpGet]
        public IActionResult DownloadErrorSample()
        {
            var bytes = WordExamSampleGenerator.GenerateInvalidSampleDocx();
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DeThi_TOEIC_Loi_DinhDang.docx");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSampleTest()
        {
            try
            {
                var examId = await _examAppService.CreateSampleTestAsync();
                TempData["Success"] = "Đã khởi tạo thành công đề thi chuẩn 'TOEIC Reading Practice Test 05' vào cơ sở dữ liệu!";
            }
            catch (Exception ex)
            {
                TempData["Errors"] = new List<string> { ex.Message };
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Preview(int id)
        {
            var exam = await _examAppService.GetExamDetailAsync(id);
            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionDto input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.CorrectAnswer))
            {
                return Json(new { success = false, message = "Dữ liệu câu hỏi không hợp lệ." });
            }

            await _examAppService.UpdateQuestionAsync(input);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuestionGroup(
            [FromForm] int id,
            [FromForm] string groupTitle,
            [FromForm] string passageText,
            [FromForm] int orderIndex,
            IFormFile imageFile)
        {
            string imageUrl = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "exams");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var ext = Path.GetExtension(imageFile.FileName);
                var fileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/exams/{fileName}";
            }

            var dto = new UpdateQuestionGroupDto
            {
                Id = id,
                GroupTitle = groupTitle,
                PassageText = passageText,
                ImageUrl = imageUrl,
                OrderIndex = orderIndex
            };

            await _examAppService.UpdateQuestionGroupAsync(dto);
            return Json(new { success = true, imageUrl = imageUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _examAppService.DeleteExamAsync(id);
            TempData["Success"] = "Đã xóa đề thi thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
