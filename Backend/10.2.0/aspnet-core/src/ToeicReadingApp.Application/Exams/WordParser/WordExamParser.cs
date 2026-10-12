using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;

namespace ToeicReadingApp.Exams.WordParser
{
    public class WordExamParser
    {
        private class DocParagraphInfo
        {
            public string Text { get; set; }
            public byte[] ImageData { get; set; }
            public string ImageExtension { get; set; }
        }

        public WordExamValidationResult ParseAndValidate(Stream docxStream)
        {
            var result = new WordExamValidationResult();
            var exam = new ParsedExam();
            result.Exam = exam;

            List<DocParagraphInfo> paragraphInfos;
            try
            {
                paragraphInfos = ExtractParagraphs(docxStream);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Không thể đọc cấu trúc file Word (.docx): {ex.Message}");
                return result;
            }

            ParsedPart currentPart = null;
            ParsedGroup currentGroup = null;
            ParsedQuestion currentQuestion = null;
            string currentField = null; // "DIRECTIONS", "PASSAGE", "QUESTION_TEXT", "OPTION_A", "OPTION_B", "OPTION_C", "OPTION_D"
            bool inPassage = false;

            int groupOrderIndex = 0;

            foreach (var pInfo in paragraphInfos)
            {
                var text = pInfo.Text?.Trim() ?? string.Empty;

                // If paragraph has an image and we are in a group, capture it
                if (pInfo.ImageData != null && currentGroup != null)
                {
                    currentGroup.ImageData = pInfo.ImageData;
                    currentGroup.ImageExtension = pInfo.ImageExtension;
                }

                if (string.IsNullOrWhiteSpace(text) && !inPassage)
                {
                    continue;
                }

                // Check [EXAM_TITLE]
                var titleMatch = Regex.Match(text, @"^\[EXAM_TITLE\]\s*(.*)", RegexOptions.IgnoreCase);
                if (titleMatch.Success)
                {
                    exam.Title = titleMatch.Groups[1].Value.Trim();
                    currentField = "EXAM_TITLE";
                    continue;
                }

                // Check [DURATION]
                var durationMatch = Regex.Match(text, @"^\[DURATION\]\s*:?\s*(\d+)", RegexOptions.IgnoreCase);
                if (durationMatch.Success)
                {
                    if (int.TryParse(durationMatch.Groups[1].Value, out int dur))
                    {
                        exam.DurationMinutes = dur;
                    }
                    currentField = null;
                    continue;
                }

                // Check [PART:x]
                var partMatch = Regex.Match(text, @"^\[PART\s*:\s*(\d+)\]", RegexOptions.IgnoreCase);
                if (partMatch.Success)
                {
                    int partNum = int.Parse(partMatch.Groups[1].Value);
                    currentPart = new ParsedPart
                    {
                        PartNumber = partNum,
                        Title = $"PART {partNum}"
                    };
                    exam.Parts.Add(currentPart);
                    currentGroup = null;
                    currentQuestion = null;
                    currentField = null;
                    inPassage = false;
                    continue;
                }

                // Check [DIRECTIONS]
                var directionsMatch = Regex.Match(text, @"^\[DIRECTIONS\]\s*(.*)", RegexOptions.IgnoreCase);
                if (directionsMatch.Success)
                {
                    if (currentPart != null)
                    {
                        currentPart.Directions = directionsMatch.Groups[1].Value.Trim();
                    }
                    currentField = "DIRECTIONS";
                    continue;
                }

                // Check [GROUP_START]
                if (Regex.IsMatch(text, @"^\[GROUP_START\]", RegexOptions.IgnoreCase))
                {
                    if (currentPart == null)
                    {
                        result.Errors.Add("Gặp thẻ [GROUP_START] nhưng chưa khai báo phần thi [PART:...].");
                        continue;
                    }

                    currentGroup = new ParsedGroup
                    {
                        OrderIndex = ++groupOrderIndex
                    };
                    currentPart.Groups.Add(currentGroup);
                    currentQuestion = null;
                    currentField = null;
                    inPassage = false;
                    continue;
                }

                // Check [GROUP_TITLE]
                var groupTitleMatch = Regex.Match(text, @"^\[GROUP_TITLE\]\s*(.*)", RegexOptions.IgnoreCase);
                if (groupTitleMatch.Success)
                {
                    if (currentGroup != null)
                    {
                        currentGroup.GroupTitle = groupTitleMatch.Groups[1].Value.Trim();
                    }
                    currentField = null;
                    continue;
                }

                // Check [PASSAGE_START]
                if (Regex.IsMatch(text, @"^\[PASSAGE_START\]", RegexOptions.IgnoreCase))
                {
                    inPassage = true;
                    currentField = "PASSAGE";

                    var remaining = Regex.Replace(text, @"^\[PASSAGE_START\]\s*", "", RegexOptions.IgnoreCase);
                    if (!string.IsNullOrWhiteSpace(remaining))
                    {
                        if (Regex.IsMatch(remaining, @"\[PASSAGE_END\]", RegexOptions.IgnoreCase))
                        {
                            var passContent = Regex.Replace(remaining, @"\s*\[PASSAGE_END\].*", "", RegexOptions.IgnoreCase);
                            if (currentGroup != null) currentGroup.PassageText = passContent.Trim();
                            inPassage = false;
                            currentField = null;
                        }
                        else
                        {
                            if (currentGroup != null) currentGroup.PassageText = remaining.Trim();
                        }
                    }
                    continue;
                }

                // Check [PASSAGE_END]
                if (Regex.IsMatch(text, @"\[PASSAGE_END\]", RegexOptions.IgnoreCase))
                {
                    var beforeEnd = Regex.Replace(text, @"\s*\[PASSAGE_END\].*", "", RegexOptions.IgnoreCase).Trim();
                    if (!string.IsNullOrEmpty(beforeEnd) && currentGroup != null)
                    {
                        currentGroup.PassageText = string.IsNullOrEmpty(currentGroup.PassageText)
                            ? beforeEnd
                            : currentGroup.PassageText + Environment.NewLine + beforeEnd;
                    }
                    inPassage = false;
                    currentField = null;
                    continue;
                }

                // Check [GROUP_END]
                if (Regex.IsMatch(text, @"^\[GROUP_END\]", RegexOptions.IgnoreCase))
                {
                    currentGroup = null;
                    currentQuestion = null;
                    currentField = null;
                    inPassage = false;
                    continue;
                }

                // Check [Q:xxx]
                var qMatch = Regex.Match(text, @"^\[Q\s*:\s*(\d+)\]\s*(.*)", RegexOptions.IgnoreCase);
                if (qMatch.Success)
                {
                    int qNum = int.Parse(qMatch.Groups[1].Value);
                    string qText = qMatch.Groups[2].Value.Trim();

                    currentQuestion = new ParsedQuestion
                    {
                        QuestionNumber = qNum,
                        QuestionText = qText
                    };

                    if (currentPart == null)
                    {
                        result.Errors.Add($"Câu [Q:{qNum}] xuất hiện trước khi khai báo [PART:...].");
                    }
                    else
                    {
                        if (currentGroup != null)
                        {
                            currentGroup.Questions.Add(currentQuestion);
                        }
                        else
                        {
                            currentPart.StandaloneQuestions.Add(currentQuestion);
                        }
                    }

                    currentField = "QUESTION_TEXT";
                    continue;
                }

                // Check [A]
                var aMatch = Regex.Match(text, @"^\[A\]\s*(.*)", RegexOptions.IgnoreCase);
                if (aMatch.Success)
                {
                    if (currentQuestion != null) currentQuestion.OptionA = aMatch.Groups[1].Value.Trim();
                    currentField = "OPTION_A";
                    continue;
                }

                // Check [B]
                var bMatch = Regex.Match(text, @"^\[B\]\s*(.*)", RegexOptions.IgnoreCase);
                if (bMatch.Success)
                {
                    if (currentQuestion != null) currentQuestion.OptionB = bMatch.Groups[1].Value.Trim();
                    currentField = "OPTION_B";
                    continue;
                }

                // Check [C]
                var cMatch = Regex.Match(text, @"^\[C\]\s*(.*)", RegexOptions.IgnoreCase);
                if (cMatch.Success)
                {
                    if (currentQuestion != null) currentQuestion.OptionC = cMatch.Groups[1].Value.Trim();
                    currentField = "OPTION_C";
                    continue;
                }

                // Check [D]
                var dMatch = Regex.Match(text, @"^\[D\]\s*(.*)", RegexOptions.IgnoreCase);
                if (dMatch.Success)
                {
                    if (currentQuestion != null) currentQuestion.OptionD = dMatch.Groups[1].Value.Trim();
                    currentField = "OPTION_D";
                    continue;
                }

                // Check [ANS:x] or [ANSWER:x]
                var ansMatch = Regex.Match(text, @"^\[ANS(?:WER)?\s*:\s*([A-Za-z])\]", RegexOptions.IgnoreCase);
                if (ansMatch.Success)
                {
                    if (currentQuestion != null)
                    {
                        currentQuestion.CorrectAnswer = ansMatch.Groups[1].Value.Trim().ToUpperInvariant();
                    }
                    currentField = null;
                    continue;
                }

                // Multi-line continuation
                if (inPassage && currentGroup != null)
                {
                    currentGroup.PassageText = string.IsNullOrEmpty(currentGroup.PassageText)
                        ? text
                        : currentGroup.PassageText + Environment.NewLine + text;
                }
                else if (currentField == "DIRECTIONS" && currentPart != null)
                {
                    currentPart.Directions = string.IsNullOrEmpty(currentPart.Directions)
                        ? text
                        : currentPart.Directions + Environment.NewLine + text;
                }
                else if (currentField == "QUESTION_TEXT" && currentQuestion != null)
                {
                    currentQuestion.QuestionText = string.IsNullOrEmpty(currentQuestion.QuestionText)
                        ? text
                        : currentQuestion.QuestionText + " " + text;
                }
                else if (currentField == "OPTION_A" && currentQuestion != null)
                {
                    currentQuestion.OptionA = string.IsNullOrEmpty(currentQuestion.OptionA) ? text : currentQuestion.OptionA + " " + text;
                }
                else if (currentField == "OPTION_B" && currentQuestion != null)
                {
                    currentQuestion.OptionB = string.IsNullOrEmpty(currentQuestion.OptionB) ? text : currentQuestion.OptionB + " " + text;
                }
                else if (currentField == "OPTION_C" && currentQuestion != null)
                {
                    currentQuestion.OptionC = string.IsNullOrEmpty(currentQuestion.OptionC) ? text : currentQuestion.OptionC + " " + text;
                }
                else if (currentField == "OPTION_D" && currentQuestion != null)
                {
                    currentQuestion.OptionD = string.IsNullOrEmpty(currentQuestion.OptionD) ? text : currentQuestion.OptionD + " " + text;
                }
            }

            // Run strict validation
            ValidateExam(exam, result);

            return result;
        }

        private void ValidateExam(ParsedExam exam, WordExamValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(exam.Title))
            {
                result.Errors.Add("File thiếu thẻ [EXAM_TITLE] Tên đề thi.");
            }

            if (exam.Parts == null || exam.Parts.Count == 0)
            {
                result.Errors.Add("File không chứa phần thi nào ([PART:5], [PART:6], hoặc [PART:7]).");
            }

            var allQuestions = exam.Parts?
                .SelectMany(p => p.StandaloneQuestions.Concat(p.Groups.SelectMany(g => g.Questions)))
                .OrderBy(q => q.QuestionNumber)
                .ToList() ?? new List<ParsedQuestion>();

            exam.TotalQuestions = allQuestions.Count;

            if (allQuestions.Count == 0)
            {
                result.Errors.Add("File không chứa câu hỏi nào ([Q:xxx]).");
                return;
            }

            // Check duplicate question numbers
            var duplicates = allQuestions.GroupBy(q => q.QuestionNumber).Where(g => g.Count() > 1).ToList();
            foreach (var dup in duplicates)
            {
                result.Errors.Add($"Số câu hỏi bị trùng lặp: Câu {dup.Key} xuất hiện {dup.Count()} lần.");
            }

            // Check sequential continuity
            for (int i = 0; i < allQuestions.Count - 1; i++)
            {
                int currentNum = allQuestions[i].QuestionNumber;
                int nextNum = allQuestions[i + 1].QuestionNumber;

                if (nextNum > currentNum + 1)
                {
                    result.Errors.Add($"Sai thứ tự câu hỏi: Từ câu {currentNum} nhảy sang câu {nextNum}.");
                }
            }

            // Check each question options and answer
            foreach (var q in allQuestions)
            {
                if (string.IsNullOrWhiteSpace(q.OptionA)) result.Errors.Add($"Câu {q.QuestionNumber}: Thiếu đáp án [A].");
                if (string.IsNullOrWhiteSpace(q.OptionB)) result.Errors.Add($"Câu {q.QuestionNumber}: Thiếu đáp án [B].");
                if (string.IsNullOrWhiteSpace(q.OptionC)) result.Errors.Add($"Câu {q.QuestionNumber}: Thiếu đáp án [C].");
                if (string.IsNullOrWhiteSpace(q.OptionD)) result.Errors.Add($"Câu {q.QuestionNumber}: Thiếu đáp án [D].");

                if (string.IsNullOrWhiteSpace(q.CorrectAnswer))
                {
                    result.Errors.Add($"Câu {q.QuestionNumber}: Thiếu thẻ đáp án đúng [ANS:...].");
                }
                else
                {
                    var ans = q.CorrectAnswer.Trim().ToUpperInvariant();
                    if (ans != "A" && ans != "B" && ans != "C" && ans != "D")
                    {
                        result.Errors.Add($"Câu {q.QuestionNumber}: Đáp án đúng '{q.CorrectAnswer}' không hợp lệ (phải là A, B, C hoặc D).");
                    }
                }
            }

            // Check Part 6 and Part 7 constraints
            foreach (var part in exam.Parts)
            {
                if (part.PartNumber == 6 || part.PartNumber == 7)
                {
                    // No standalone questions allowed in Part 6 and 7
                    if (part.StandaloneQuestions.Count > 0)
                    {
                        foreach (var sq in part.StandaloneQuestions)
                        {
                            result.Errors.Add($"Câu {sq.QuestionNumber} (Part {part.PartNumber}): Bắt buộc phải nằm trong cặp thẻ nhóm bài đọc [GROUP_START]...[GROUP_END].");
                        }
                    }

                    // Groups must have passage text and at least 1 question
                    foreach (var grp in part.Groups)
                    {
                        if (string.IsNullOrWhiteSpace(grp.PassageText))
                        {
                            result.Errors.Add($"Nhóm câu hỏi Part {part.PartNumber} '{(string.IsNullOrWhiteSpace(grp.GroupTitle) ? "Đoạn văn" : grp.GroupTitle)}': Thiếu nội dung bài đọc [PASSAGE_START]...[PASSAGE_END].");
                        }

                        if (grp.Questions.Count == 0)
                        {
                            result.Errors.Add($"Nhóm câu hỏi Part {part.PartNumber} '{(string.IsNullOrWhiteSpace(grp.GroupTitle) ? "Đoạn văn" : grp.GroupTitle)}': Không chứa bất kỳ câu hỏi nào.");
                        }
                    }
                }
            }
        }

        private List<DocParagraphInfo> ExtractParagraphs(Stream stream)
        {
            var list = new List<DocParagraphInfo>();

            using (var doc = WordprocessingDocument.Open(stream, false))
            {
                var mainPart = doc.MainDocumentPart;
                if (mainPart?.Document?.Body == null)
                {
                    return list;
                }

                var paragraphs = mainPart.Document.Body.Descendants<Paragraph>();

                foreach (var para in paragraphs)
                {
                    var sb = new StringBuilder();
                    byte[] imgBytes = null;
                    string imgExt = ".png";

                    foreach (var element in para.ChildElements)
                    {
                        if (element is Run run)
                        {
                            foreach (var runChild in run.ChildElements)
                            {
                                if (runChild is Text t)
                                {
                                    sb.Append(t.Text);
                                }
                                else if (runChild is Break)
                                {
                                    sb.AppendLine();
                                }
                                else if (runChild is TabChar)
                                {
                                    sb.Append(" ");
                                }
                            }
                        }
                    }

                    // Check for drawing/blip
                    var blips = para.Descendants<A.Blip>();
                    foreach (var blip in blips)
                    {
                        var embed = blip.Embed?.Value;
                        if (!string.IsNullOrEmpty(embed))
                        {
                            try
                            {
                                var part = mainPart.GetPartById(embed);
                                if (part is ImagePart imgPart)
                                {
                                    using var imgStream = imgPart.GetStream();
                                    using var ms = new MemoryStream();
                                    imgStream.CopyTo(ms);
                                    imgBytes = ms.ToArray();
                                    imgExt = GetExtensionFromContentType(imgPart.ContentType);
                                    break;
                                }
                            }
                            catch
                            {
                                // Ignore non-critical image reading errors
                            }
                        }
                    }

                    var text = sb.ToString();
                    list.Add(new DocParagraphInfo
                    {
                        Text = text,
                        ImageData = imgBytes,
                        ImageExtension = imgExt
                    });
                }
            }

            return list;
        }

        private string GetExtensionFromContentType(string contentType)
        {
            if (string.IsNullOrEmpty(contentType)) return ".png";
            contentType = contentType.ToLowerInvariant();
            if (contentType.Contains("jpeg") || contentType.Contains("jpg")) return ".jpg";
            if (contentType.Contains("png")) return ".png";
            if (contentType.Contains("gif")) return ".gif";
            if (contentType.Contains("webp")) return ".webp";
            return ".png";
        }
    }
}
