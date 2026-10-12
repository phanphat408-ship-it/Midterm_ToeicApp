class QuestionAnswerPayload {
  final int questionId;
  final int questionNumber;
  final String? selectedOption;

  QuestionAnswerPayload({
    required this.questionId,
    required this.questionNumber,
    this.selectedOption,
  });

  Map<String, dynamic> toJson() {
    return {
      'questionId': questionId,
      'questionNumber': questionNumber,
      'selectedOption': selectedOption,
    };
  }
}

class SubmitExamPayload {
  final int examId;
  final String studentName;
  final DateTime startTime;
  final DateTime submitTime;
  final List<QuestionAnswerPayload> answers;

  SubmitExamPayload({
    required this.examId,
    required this.studentName,
    required this.startTime,
    required this.submitTime,
    required this.answers,
  });

  Map<String, dynamic> toJson() {
    return {
      'examId': examId,
      'studentName': studentName,
      'startTime': startTime.toIso8601String(),
      'submitTime': submitTime.toIso8601String(),
      'answers': answers.map((a) => a.toJson()).toList(),
    };
  }
}

class QuestionResultItem {
  final int questionId;
  final int questionNumber;
  final String? selectedOption;
  final String correctOption;
  final bool isCorrect;
  final String? questionText;
  final String optionA;
  final String optionB;
  final String optionC;
  final String optionD;

  QuestionResultItem({
    required this.questionId,
    required this.questionNumber,
    this.selectedOption,
    required this.correctOption,
    required this.isCorrect,
    this.questionText,
    required this.optionA,
    required this.optionB,
    required this.optionC,
    required this.optionD,
  });

  factory QuestionResultItem.fromJson(Map<String, dynamic> json) {
    return QuestionResultItem(
      questionId: json['questionId'] ?? 0,
      questionNumber: json['questionNumber'] ?? 101,
      selectedOption: json['selectedOption'],
      correctOption: json['correctOption'] ?? '',
      isCorrect: json['isCorrect'] ?? false,
      questionText: json['questionText'],
      optionA: json['optionA'] ?? '',
      optionB: json['optionB'] ?? '',
      optionC: json['optionC'] ?? '',
      optionD: json['optionD'] ?? '',
    );
  }
}

class SubmitExamResult {
  final int attemptId;
  final int examId;
  final String studentName;
  final int totalQuestions;
  final int totalCorrect;
  final int totalWrong;
  final int totalSkipped;
  final int score;
  final DateTime? startTime;
  final DateTime? submitTime;
  final List<QuestionResultItem> details;

  SubmitExamResult({
    required this.attemptId,
    required this.examId,
    required this.studentName,
    required this.totalQuestions,
    required this.totalCorrect,
    required this.totalWrong,
    required this.totalSkipped,
    required this.score,
    this.startTime,
    this.submitTime,
    required this.details,
  });

  factory SubmitExamResult.fromJson(Map<String, dynamic> json) {
    var list = <QuestionResultItem>[];
    if (json['details'] != null) {
      list = (json['details'] as List)
          .map((d) => QuestionResultItem.fromJson(d))
          .toList();
    }
    list.sort((a, b) => a.questionNumber.compareTo(b.questionNumber));

    return SubmitExamResult(
      attemptId: json['attemptId'] ?? 0,
      examId: json['examId'] ?? 0,
      studentName: json['studentName'] ?? 'Thí sinh',
      totalQuestions: json['totalQuestions'] ?? 100,
      totalCorrect: json['totalCorrect'] ?? 0,
      totalWrong: json['totalWrong'] ?? 0,
      totalSkipped: json['totalSkipped'] ?? 0,
      score: json['score'] ?? 0,
      startTime: json['startTime'] != null ? DateTime.tryParse(json['startTime']) : null,
      submitTime: json['submitTime'] != null ? DateTime.tryParse(json['submitTime']) : null,
      details: list,
    );
  }
}
