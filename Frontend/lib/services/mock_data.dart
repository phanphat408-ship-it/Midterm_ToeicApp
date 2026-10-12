import '../models/exam_models.dart';
import '../models/submit_models.dart';

class MockExamData {
  static List<ExamListItem> getMockExamList() {
    return [
      ExamListItem(
        id: 1,
        title: 'TOEIC Reading Practice Test 05',
        durationMinutes: 75,
        totalQuestions: 100,
        description: 'Đề thi chuẩn TOEIC Reading format mới 2026 (Part 5, 6, 7)',
        isPublished: true,
        creationTime: DateTime.now().subtract(const Duration(days: 1)),
        partCount: 3,
      ),
      ExamListItem(
        id: 2,
        title: 'ETS TOEIC Reading Real Exam 01',
        durationMinutes: 75,
        totalQuestions: 100,
        description: 'Đề luyện thi cấp tốc sát đề thi thật tại IIG Vietnam',
        isPublished: true,
        creationTime: DateTime.now().subtract(const Duration(days: 3)),
        partCount: 3,
      ),
    ];
  }

  static ExamDetail getMockExamDetail(int id) {
    // Generate questions for Part 5 (101-130)
    var p5Questions = <QuestionItem>[];
    for (int i = 101; i <= 130; i++) {
      p5Questions.add(QuestionItem(
        id: i,
        examId: id,
        examPartId: 1,
        questionNumber: i,
        questionText: _getPart5QuestionText(i),
        optionA: _getPart5Option(i, 'A'),
        optionB: _getPart5Option(i, 'B'),
        optionC: _getPart5Option(i, 'C'),
        optionD: _getPart5Option(i, 'D'),
        correctAnswer: (i % 4 == 1) ? 'B' : (i % 4 == 2) ? 'A' : (i % 4 == 3) ? 'C' : 'D',
      ));
    }

    var part5 = ExamPartItem(
      id: 1,
      examId: id,
      partNumber: 5,
      title: 'PART 5',
      directions: 'In each question, you will find a word or phrase missing. Four answer choices are given. Choose the one that best completes the sentence.',
      questionGroups: [],
      standaloneQuestions: p5Questions,
      allQuestions: p5Questions,
    );

    // Part 6 (131-146 - 4 groups)
    var p6Groups = <QuestionGroupItem>[];
    int qIndex = 131;
    for (int g = 1; g <= 4; g++) {
      var grpQuestions = <QuestionItem>[];
      for (int k = 0; k < 4; k++) {
        grpQuestions.add(QuestionItem(
          id: qIndex,
          examId: id,
          examPartId: 2,
          questionGroupId: g,
          questionNumber: qIndex,
          questionText: '',
          optionA: 'Option A ($qIndex)',
          optionB: 'Option B ($qIndex)',
          optionC: 'Option C ($qIndex)',
          optionD: 'Option D ($qIndex)',
          correctAnswer: 'B',
        ));
        qIndex++;
      }

      p6Groups.add(QuestionGroupItem(
        id: g,
        examPartId: 2,
        groupTitle: 'Questions ${131 + (g - 1) * 4}-${134 + (g - 1) * 4} refer to the following announcement.',
        passageText: 'MEMORANDUM & NOTICE (Group $g)\n\nPlease be advised that our regional operations will undergo quarterly system upgrades. During this transition, automated file synchronization will be momentarily halted.\n\nWe anticipate all enterprise services to resume full functionality shortly. Please contact technical dispatch if any unexpected discrepancy persists.',
        orderIndex: g,
        questions: grpQuestions,
      ));
    }

    var p6All = <QuestionItem>[];
    for (var g in p6Groups) {
      p6All.addAll(g.questions);
    }

    var part6 = ExamPartItem(
      id: 2,
      examId: id,
      partNumber: 6,
      title: 'PART 6',
      directions: 'Read the texts that follow. A word, phrase, or sentence is missing in parts of each text. Four answer choices for each question are given on below. Select the best answer.',
      questionGroups: p6Groups,
      standaloneQuestions: [],
      allQuestions: p6All,
    );

    // Part 7 (147-200)
    var p7Groups = <QuestionGroupItem>[];
    int p7QIndex = 147;
    int groupCounter = 1;
    while (p7QIndex <= 200) {
      int count = (p7QIndex <= 160) ? 2 : (p7QIndex <= 175) ? 3 : 5;
      if (p7QIndex + count - 1 > 200) {
        count = 200 - p7QIndex + 1;
      }

      var grpQuestions = <QuestionItem>[];
      int startQ = p7QIndex;
      int endQ = p7QIndex + count - 1;

      for (int k = 0; k < count; k++) {
        grpQuestions.add(QuestionItem(
          id: p7QIndex,
          examId: id,
          examPartId: 3,
          questionGroupId: 10 + groupCounter,
          questionNumber: p7QIndex,
          questionText: _getPart7QuestionText(p7QIndex),
          optionA: 'Option A ($p7QIndex)',
          optionB: 'Option B ($p7QIndex)',
          optionC: 'Option C ($p7QIndex)',
          optionD: 'Option D ($p7QIndex)',
          correctAnswer: (p7QIndex % 2 == 0) ? 'B' : 'C',
        ));
        p7QIndex++;
      }

      p7Groups.add(QuestionGroupItem(
        id: 10 + groupCounter,
        examPartId: 3,
        groupTitle: 'Questions $startQ-$endQ refer to the following reading document.',
        passageText: 'SUMMIT CORPORATION BULLETIN (Document $groupCounter)\n\nPacific Commercial Services is pleased to announce its expanded international trade agreement. Operating across three global continents, our logistics network provides end-to-end cargo traceability.\n\nAll commercial stakeholders are encouraged to attend our upcoming informational seminar next month. Registration details are available on our official web portal.',
        orderIndex: groupCounter,
        questions: grpQuestions,
      ));
      groupCounter++;
    }

    var p7All = <QuestionItem>[];
    for (var g in p7Groups) {
      p7All.addAll(g.questions);
    }

    var part7 = ExamPartItem(
      id: 3,
      examId: id,
      partNumber: 7,
      title: 'PART 7',
      directions: 'In this part, you will read a selection of texts, such as magazine and newspaper articles, letters, and advertisements. Each text is followed by several questions. Select the best answer for each question.',
      questionGroups: p7Groups,
      standaloneQuestions: [],
      allQuestions: p7All,
    );

    return ExamDetail(
      id: id,
      title: id == 1 ? 'TOEIC Reading Practice Test 05' : 'ETS TOEIC Reading Real Exam 01',
      durationMinutes: 75,
      totalQuestions: 100,
      description: 'Đề thi chuẩn TOEIC Reading 100 câu',
      parts: [part5, part6, part7],
    );
  }

  static SubmitExamResult getMockSubmitResult(SubmitExamPayload payload) {
    int total = payload.answers.length;
    int correct = 0;
    int wrong = 0;
    int skipped = 0;

    var details = <QuestionResultItem>[];

    for (var ans in payload.answers) {
      String correctAns = (ans.questionNumber % 4 == 1) ? 'B' : (ans.questionNumber % 4 == 2) ? 'A' : (ans.questionNumber % 4 == 3) ? 'C' : 'D';
      bool isCorrect = false;

      if (ans.selectedOption == null || ans.selectedOption!.isEmpty) {
        skipped++;
      } else if (ans.selectedOption!.toUpperCase() == correctAns) {
        correct++;
        isCorrect = true;
      } else {
        wrong++;
      }

      details.add(QuestionResultItem(
        questionId: ans.questionId,
        questionNumber: ans.questionNumber,
        selectedOption: ans.selectedOption,
        correctOption: correctAns,
        isCorrect: isCorrect,
        questionText: 'Câu hỏi số ${ans.questionNumber}',
        optionA: 'Option A',
        optionB: 'Option B',
        optionC: 'Option C',
        optionD: 'Option D',
      ));
    }

    int score = (total > 0) ? ((correct / total) * 490).round() + 5 : 5;

    return SubmitExamResult(
      attemptId: 999,
      examId: payload.examId,
      studentName: payload.studentName,
      totalQuestions: total,
      totalCorrect: correct,
      totalWrong: wrong,
      totalSkipped: skipped,
      score: score,
      startTime: payload.startTime,
      submitTime: payload.submitTime,
      details: details,
    );
  }

  static String _getPart5QuestionText(int num) {
    switch (num) {
      case 101: return 'Customer satisfaction is our highest priority at Kingston Financial Services.';
      case 102: return 'Ms. Delgado will present the quarterly budget report ------- the board members tomorrow morning.';
      case 103: return 'All employees must submit their travel expense receipts within thirty days ------- their return.';
      case 104: return 'The newly designed electric automobile operates much more ------- than earlier models.';
      case 105: return 'Conference attendees are requested to wear their name tags at ------- times inside the convention center.';
      default: return 'The executive committee voted unanimously to implement the revised policy starting next -------.';
    }
  }

  static String _getPart5Option(int num, String opt) {
    if (num == 101) {
      if (opt == 'A') return 'completely';
      if (opt == 'B') return 'total';
      if (opt == 'C') return 'absolute';
      return 'entire';
    }
    if (num == 102) {
      if (opt == 'A') return 'to';
      if (opt == 'B') return 'at';
      if (opt == 'C') return 'for';
      return 'with';
    }
    if (num == 103) {
      if (opt == 'A') return 'following';
      if (opt == 'B') return 'before';
      if (opt == 'C') return 'while';
      return 'during';
    }
    if (num == 104) {
      if (opt == 'A') return 'efficient';
      if (opt == 'B') return 'efficiency';
      if (opt == 'C') return 'efficiently';
      return 'more efficient';
    }
    if (num == 105) {
      if (opt == 'A') return 'each';
      if (opt == 'B') return 'all';
      if (opt == 'C') return 'every';
      return 'whole';
    }
    return 'Option $opt';
  }

  static String _getPart7QuestionText(int num) {
    switch (num % 5) {
      case 0: return 'What is the main purpose of the notice?';
      case 1: return 'According to the passage, what is required of all employees?';
      case 2: return 'What will happen on next Friday afternoon?';
      case 3: return 'Who is eligible for the promotional discount?';
      default: return 'How can clients contact customer service?';
    }
  }
}
