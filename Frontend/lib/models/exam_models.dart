class ExamListItem {
  final int id;
  final String title;
  final int durationMinutes;
  final int totalQuestions;
  final String? description;
  final bool isPublished;
  final DateTime? creationTime;
  final int partCount;

  ExamListItem({
    required this.id,
    required this.title,
    required this.durationMinutes,
    required this.totalQuestions,
    this.description,
    required this.isPublished,
    this.creationTime,
    this.partCount = 3,
  });

  factory ExamListItem.fromJson(Map<String, dynamic> json) {
    return ExamListItem(
      id: json['id'] ?? 0,
      title: json['title'] ?? 'TOEIC Reading Test',
      durationMinutes: json['durationMinutes'] ?? 75,
      totalQuestions: json['totalQuestions'] ?? 100,
      description: json['description'],
      isPublished: json['isPublished'] ?? true,
      creationTime: json['creationTime'] != null
          ? DateTime.tryParse(json['creationTime'])
          : null,
      partCount: json['partCount'] ?? 3,
    );
  }
}

class QuestionItem {
  final int id;
  final int examId;
  final int examPartId;
  final int? questionGroupId;
  final int questionNumber;
  final String? questionText;
  final String optionA;
  final String optionB;
  final String optionC;
  final String optionD;
  final String? correctAnswer;

  QuestionItem({
    required this.id,
    required this.examId,
    required this.examPartId,
    this.questionGroupId,
    required this.questionNumber,
    this.questionText,
    required this.optionA,
    required this.optionB,
    required this.optionC,
    required this.optionD,
    this.correctAnswer,
  });

  factory QuestionItem.fromJson(Map<String, dynamic> json) {
    return QuestionItem(
      id: json['id'] ?? 0,
      examId: json['examId'] ?? 0,
      examPartId: json['examPartId'] ?? 0,
      questionGroupId: json['questionGroupId'],
      questionNumber: json['questionNumber'] ?? 101,
      questionText: json['questionText'],
      optionA: json['optionA'] ?? '',
      optionB: json['optionB'] ?? '',
      optionC: json['optionC'] ?? '',
      optionD: json['optionD'] ?? '',
      correctAnswer: json['correctAnswer'],
    );
  }
}

class QuestionGroupItem {
  final int id;
  final int examPartId;
  final String groupTitle;
  final String passageText;
  final String? imageUrl;
  final int orderIndex;
  final List<QuestionItem> questions;

  QuestionGroupItem({
    required this.id,
    required this.examPartId,
    required this.groupTitle,
    required this.passageText,
    this.imageUrl,
    this.orderIndex = 0,
    required this.questions,
  });

  factory QuestionGroupItem.fromJson(Map<String, dynamic> json) {
    var qList = <QuestionItem>[];
    if (json['questions'] != null) {
      qList = (json['questions'] as List)
          .map((q) => QuestionItem.fromJson(q))
          .toList();
    }
    return QuestionGroupItem(
      id: json['id'] ?? 0,
      examPartId: json['examPartId'] ?? 0,
      groupTitle: json['groupTitle'] ?? '',
      passageText: json['passageText'] ?? '',
      imageUrl: json['imageUrl'],
      orderIndex: json['orderIndex'] ?? 0,
      questions: qList,
    );
  }
}

class ExamPartItem {
  final int id;
  final int examId;
  final int partNumber; // 5, 6, 7
  final String title;
  final String directions;
  final List<QuestionGroupItem> questionGroups;
  final List<QuestionItem> standaloneQuestions;
  final List<QuestionItem> allQuestions;

  ExamPartItem({
    required this.id,
    required this.examId,
    required this.partNumber,
    required this.title,
    required this.directions,
    required this.questionGroups,
    required this.standaloneQuestions,
    required this.allQuestions,
  });

  factory ExamPartItem.fromJson(Map<String, dynamic> json) {
    var groups = <QuestionGroupItem>[];
    if (json['questionGroups'] != null) {
      groups = (json['questionGroups'] as List)
          .map((g) => QuestionGroupItem.fromJson(g))
          .toList();
    }

    var standalone = <QuestionItem>[];
    if (json['standaloneQuestions'] != null) {
      standalone = (json['standaloneQuestions'] as List)
          .map((q) => QuestionItem.fromJson(q))
          .toList();
    }

    var all = <QuestionItem>[];
    if (json['allQuestions'] != null && (json['allQuestions'] as List).isNotEmpty) {
      all = (json['allQuestions'] as List)
          .map((q) => QuestionItem.fromJson(q))
          .toList();
    } else {
      all.addAll(standalone);
      for (var g in groups) {
        all.addAll(g.questions);
      }
    }
    all.sort((a, b) => a.questionNumber.compareTo(b.questionNumber));

    return ExamPartItem(
      id: json['id'] ?? 0,
      examId: json['examId'] ?? 0,
      partNumber: json['partNumber'] ?? 5,
      title: json['title'] ?? 'PART 5',
      directions: json['directions'] ?? '',
      questionGroups: groups,
      standaloneQuestions: standalone,
      allQuestions: all,
    );
  }
}

class ExamDetail {
  final int id;
  final String title;
  final int durationMinutes;
  final int totalQuestions;
  final String? description;
  final List<ExamPartItem> parts;

  ExamDetail({
    required this.id,
    required this.title,
    required this.durationMinutes,
    required this.totalQuestions,
    this.description,
    required this.parts,
  });

  factory ExamDetail.fromJson(Map<String, dynamic> json) {
    var partsList = <ExamPartItem>[];
    if (json['parts'] != null) {
      partsList = (json['parts'] as List)
          .map((p) => ExamPartItem.fromJson(p))
          .toList();
    }
    partsList.sort((a, b) => a.partNumber.compareTo(b.partNumber));

    return ExamDetail(
      id: json['id'] ?? 0,
      title: json['title'] ?? 'TOEIC Reading Test',
      durationMinutes: json['durationMinutes'] ?? 75,
      totalQuestions: json['totalQuestions'] ?? 100,
      description: json['description'],
      parts: partsList,
    );
  }

  List<QuestionItem> getAllQuestions() {
    var list = <QuestionItem>[];
    for (var part in parts) {
      list.addAll(part.allQuestions);
    }
    list.sort((a, b) => a.questionNumber.compareTo(b.questionNumber));
    return list;
  }
}
